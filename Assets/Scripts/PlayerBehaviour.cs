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
    private GameObject currentCell;

    [SerializeField] int currentEnergy;

    [SerializeField] Vector2 mousePos;

    [SerializeField] LayerMask plantMask;



    private void Start() {
        onClick.AddListener(CreatePlant);
    }
    public void Click(InputAction.CallbackContext context) {
        if(context.started) {
            Camera mainCam = Camera.main;
            
            onClick?.Invoke();  
        }
    }

    private void FixedUpdate() {
        CastRay();
    }

    private void CastRay() {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, plantMask);

        if(ray.collider != null) {
            GameObject cell = ray.collider.gameObject;
            ray.collider.gameObject.GetComponent<CellBehaviour>().HoverMe();
            currentCell = cell;
        }
        else {
            currentCell = null;
        }
    }
    private void CreatePlant() {
        if(currentCell != null) {
            bool canPlant = !currentCell.GetComponent<CellBehaviour>().hasPlant;

            if(canPlant && currentPlant) {
                Vector2 pos = currentCell.transform.position;
                GameObject plant = Instantiate(plantPrefab, pos, Quaternion.identity) as GameObject;
                plant.GetComponent<CreatureBehaviour>().GetData(currentPlant);
                plant.GetComponent<PlantBehaviour>().AwakePlant(currentPlant);
                //plant.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                currentCell.GetComponent<CellBehaviour>().AddPlant();
            }
        }
    }

    public void SetCurrentPlant(int index) {
        print($"current plant: {index}");
        currentPlant = allplants[index];
    }
}
