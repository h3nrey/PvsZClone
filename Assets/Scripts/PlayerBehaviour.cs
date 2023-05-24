using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField]
    public GameObject currentPlant;

    [SerializeField]
    private GameObject currentCell;

    [SerializeField] List<Vector2> usedPositions;

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

            if(canPlant) {
                Vector2 pos = currentCell.transform.position;
                GameObject plant = Instantiate(currentPlant, pos, Quaternion.identity) as GameObject;
                //plant.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                currentCell.GetComponent<CellBehaviour>().AddPlant();
            }
        }
    }
}
