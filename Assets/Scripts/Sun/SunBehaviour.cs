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

    public bool isPlantSun;

    // Launch
    private Vector2 targetPos;
    private Vector2 startPos;
    Vector2 pointB;
    Vector2 pointC;
    private bool canLaunch;
    private bool onEndPoint;


    private void Update() {
        if(isPlantSun) {
            if(canLaunch) {
                Launching();
            }
        }   else {
            if (transform.position.y > minY) {
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector2.down * Time.deltaTime * fallingSpeed, Space.World);
            }
            else if (transform.position.y <= minY) {
                Coroutines.DoAfter(() => {
                    SunSpawner.instance.sunPool.Release(this.gameObject);
                }, destroyCooldown, this);

            }
        }
    }


    public void Launch(Vector2 plantPos) {
        print($"Start Launch {plantPos}");
        canLaunch = true;

        transform.position = plantPos;
        startPos = plantPos;

        pointB = new Vector2(startPos.x + 1.5f, startPos.y + 3f);
        pointC = new Vector2(pointB.x + 0.5f, pointB.y - 2f);
        targetPos = pointB;
    }

    private void Launching() {
        Vector2 pos = transform.position;
        transform.position = Vector2.MoveTowards(pos, targetPos, 10f * Time.deltaTime);

        if(pos == pointB) {
            targetPos = pointC;
        }

        if(pos == pointC) {
            canLaunch = false;
            Coroutines.DoAfter(() => {
                SunSpawner.instance.sunPool.Release(this.gameObject);
            }, destroyCooldown, this);
        }
    }


    private void ReleaseSun() {
        SunSpawner.instance.sunPool.Release(this.gameObject);
    }
}
