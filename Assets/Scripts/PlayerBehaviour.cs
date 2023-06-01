using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    #region Variables
    #region Plant

    [SerializeField]
    PlantCard[] plantCards;

    public CurrentPlant currentPlant = new CurrentPlant(0, null);

    public PlantMiniature plantMiniature;

    [SerializeField]
    GameObject plantPrefab;

    #endregion

    #region Mouse Stuffs
    [Header("Click")]
    [HideInInspector]
    public UnityEvent onClick;

    RaycastHit2D ray;

    [SerializeField] 
    LayerMask clicableLayer;
    
    [ReadOnly] [SerializeField] 
    Vector2 mousePos;

    #endregion

    #region Cell
    [Header("Cell")]
    [SerializeField] int cellLayer;
    [ReadOnly] [SerializeField] Transform curCell;
    #endregion

    #region Energy
    [Header("Energy")]
    [SerializeField] int sunLayer;
    [SerializeField] int sunAmout; // the amount of energy that get on click on sun  
    [SerializeField] int startSunAmount;
    [ReadOnly] public int currentEnergy;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField]
    GameObject[] UICards;
    #endregion

    #endregion

    private void Start() {
        onClick.AddListener(HandleClick);
        currentEnergy = startSunAmount;
        plantMiniature = new PlantMiniature(transform.GetChild(0));

        foreach (PlantCard card in plantCards) {
            card.plantTime = 100;   
        }
    }
    private void Update() {
        foreach (PlantCard card in plantCards) {
            card.UpdateCard();
                
        }

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        HandleMiniaturePosition();
    }

    private void FixedUpdate() {
        CastRay();
    }

    #region Input

    public void Click(InputAction.CallbackContext context) {
        if(context.started) {
            Camera mainCam = Camera.main;
            
            onClick?.Invoke();  
        }
    }

    private void HandleClick() {
        if (ray.collider != null) {
            GameObject otherObj = ray.collider.gameObject;

            if (otherObj.layer == cellLayer) {
                CheckIfCanPlant(otherObj);
                plantMiniature.Toogle();
                return;
            }
            else if (otherObj.layer == sunLayer) {
                GetSun(otherObj);
            }
        }
    }

    #endregion

    private void CastRay() {
        ray = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clicableLayer);

        if (ray.collider) {
            GameObject touchingObj = ray.collider.gameObject;

            if(touchingObj.layer == cellLayer) {
                curCell = touchingObj.transform;
            }
        }

        if(!ray.collider) {
            curCell = null;
        }
    }

    private void HandleMiniaturePosition() {
        if(curCell) {
            plantMiniature.Move(curCell.position);
        } else {
            plantMiniature.Move(mousePos);
        } 
    }

    private void GetSun(GameObject sun) {
        SunSpawner.instance.sunPool.Release(sun);
        currentEnergy += sunAmout;
    }

    #region Create Plant
    private void CreatePlant(GameObject cell) {
        Plant plantData = currentPlant.data;

        // Instantiate the plant
        Vector2 pos = cell.transform.position;
        GameObject plant = Instantiate(plantPrefab, pos, Quaternion.identity, cell.transform) as GameObject;


        // Send Data to the plant prefab for his creature behaviour component
        plant.GetComponent<PlantBehaviour>().AwakePlant(plantData);

        // Add a plant to the current cell
        cell.GetComponent<CellBehaviour>().AddPlant();
    }

    private void UseSun() {
        currentEnergy -= currentPlant.data.sunCost;
    }

    private void CheckIfCanPlant(GameObject cellObj) {
        if (currentPlant.data == null) return;

        CellBehaviour cell = cellObj.GetComponent<CellBehaviour>();
        int sunCost = plantCards[currentPlant.index].data.sunCost; // the sun cost of the current plant
        float plantTime = plantCards[currentPlant.index].plantTime;

        if (!cell.hasPlant && currentEnergy >= sunCost && plantTime>= 100) {
            CreatePlant(cellObj);
            UseSun();
            plantCards[currentPlant.index].Reset();
            currentPlant.clean();
        }
        return;
    }

    #endregion

    public void SetCurrentPlant(int index) {
        currentPlant.data = plantCards[index].data;
        currentPlant.index = index;
        plantMiniature.Toogle(plantCards[index].data.sprite);
    }
}

[System.Serializable]
public class PlantCard {
    public Plant data;
    [ProgressBar("Plant Time", 100, EColor.Green)]
    public float plantTime = 100;
    public GameObject cardBtn;
    public Image cardImg;

    public void Reset() {
        cardImg.fillAmount = 0;
        plantTime = 0;
    }

    public void UpdateCard() {
        if (plantTime < 100) {
            cardBtn.GetComponent<Button>().interactable = false;
            plantTime += data.plantCooldown;
            cardImg.fillAmount += data.plantCooldown / 100;
        }
        else if (plantTime >= 100) {
            cardBtn.GetComponent<Button>().interactable = true;
        }

    }
}

[System.Serializable]
public class CurrentPlant {
    public int index;
    public Plant data;

    public CurrentPlant(int index, Plant data) {
        this.index = index;
        this.data = data;
    }

    public void clean() {
        data = null;
    }
}

[System.Serializable]
public class PlantMiniature {
    public Transform point;
    public SpriteRenderer spr;

    public PlantMiniature(Transform point) {
        this.point = point;
        this.spr = this.point.GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 pos) {
        point.position = pos;
    }

    public void Toogle(Sprite sprite = null) {
        spr.sprite = sprite;
    }
}