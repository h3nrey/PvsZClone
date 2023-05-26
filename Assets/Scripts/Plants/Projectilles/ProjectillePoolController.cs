using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectillePoolController : MonoBehaviour
{
    public static ProjectillePoolController instance;
    public ObjectPool<GameObject> projectillePool;

    [SerializeField] GameObject projectillePrefab;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        projectillePool = new ObjectPool<GameObject>(
            () => {
                return Instantiate(projectillePrefab, this.transform);
            },
            projectille => {
                projectille.SetActive(true);
            }, 
            projectille => {
                projectille.SetActive(false);
            },
            projectille => {
                Destroy(projectille);
            }, false, 100, 1000
            );
    }
}
