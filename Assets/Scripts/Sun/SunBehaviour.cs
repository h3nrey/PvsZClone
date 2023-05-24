using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SunBehaviour : MonoBehaviour
{
    [SerializeField] 
    private float fallingSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float destroyCooldown;

    [SerializeField]
    private float minY;

    private void Update() {
        if(transform.position.y > minY) {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector2.down * Time.deltaTime * fallingSpeed, Space.World);
        } else if(transform.position.y <= minY) {
            Coroutines.DoAfter(() => {
                SunSpawner.instance.sunPool.Release(this.gameObject);
            }, destroyCooldown, this);
            
        }
    }

    private void ReleaseSun() {
        SunSpawner.instance.sunPool.Release(this.gameObject);
    }
}
