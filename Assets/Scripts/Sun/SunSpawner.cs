using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SunSpawner : MonoBehaviour
{
    public static SunSpawner instance;
    [SerializeField]
    GameObject sunPrefab;

    [SerializeField]
    float maxX, minX;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float sunCooldown;

    public ObjectPool<GameObject> sunPool;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        sunPool = new ObjectPool<GameObject>(
            () => {
                return Instantiate(sunPrefab);
            },
            sun => {
                sun.SetActive(true);
            },
            sun => {
                sun.SetActive(false);
            },
            sun => {
                if(sun != null) {
                    Destroy(sun);
                }
            }, false, 4, 10
            );

        InvokeRepeating(nameof(GetSun), 0.5f, sunCooldown);
    }

    private void GetSun() {
        GameObject sun = sunPool.Get();
        sun.transform.position = GetRandomSunPos();
    }

    private Vector2 GetRandomSunPos() {
        float posX = Random.Range(minX, maxX);
        Vector2 pos = new Vector2(posX, spawnPoint.position.y);
        return pos;
    }
}
