using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    public bool hasPlant;
    public SpriteRenderer sprRenderer;

    public void HoverMe() {
        print("hovering");
    }
    public void AddPlant() {
        hasPlant = true;
    }

    public void RemovePlant() {
        hasPlant = false;
    }
}
