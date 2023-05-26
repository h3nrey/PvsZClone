using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField]
    Plant[] allplants;
    [SerializeField]
    public Plant currentPlant;   

    [SerializeField]
    GameObject plantPrefab;

    [SerializeField]
    Transform plantHolder;

    [Header("Click")]
    [SerializeField] LayerMask clicableLayer;
    [SerializeField] Vector2 mousePos;

    [SerializeField] int cellLayer;

    [SerializeField] int sunLayer;
    [SerializeField] int sunAmout; // the amount of energy that get on click on sun  
    [SerializeField] int currentEnergy;

    private void Start() {
        onClick.AddListener(HandleClick);
        //onClick.AddListener(CreatePlant);
    }
    public void Click(InputAction.CallbackContext context) {
        if(context.started) {
            Camera mainCam = Camera.main;
            
            onClick?.Invoke();  
        }
    }

    private void GetSun(GameObject sun) {
        SunSpawner.instance.sunPool.Release(sun);
        currentEnergy += sunAmout;
    }

    private void FixedUpdate() {
        //CastRay();
    }

    private void HandleClick() {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clicableLayer);

        if(ray.collider != null) {
            GameObject otherObj = ray.collider.gameObject;
            
            if (otherObj.layer == cellLayer) {
                CreatePlant(otherObj);
            } else if(otherObj.layer == sunLayer) {
                GetSun(otherObj);
            }
        }
    }
    private void CreatePlant(GameObject cell) {
        bool canPlant = !cell.GetComponent<CellBehaviour>().hasPlant;

        if (canPlant && currentPlant && currentEnergy >= currentPlant.sunCost) {
            Vector2 pos = cell.transform.position;
            GameObject plant = Instantiate(plantPrefab, pos, Quaternion.identity, plantHolder) as GameObject;
            plant.GetComponent<CreatureBehaviour>().GetData(currentPlant);
            plant.GetComponent<PlantBehaviour>().AwakePlant(currentPlant);
            cell.GetComponent<CellBehaviour>().AddPlant();
            currentEnergy -= currentPlant.sunCost;
            plant.GetComponent<CreatureBehaviour>().onDie.AddListener(() => cell.GetComponent<CellBehaviour>().RemovePlant());
        }
    }

    public void SetCurrentPlant(int index) {
        print($"current plant: {index}");
        currentPlant = allplants[index];
    }
}
