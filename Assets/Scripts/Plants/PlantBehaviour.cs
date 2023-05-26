using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlantBehaviour : MonoBehaviour
{
    [Expandable]
    public Plant data;

    ProjectillePoolController poolInstance;

    [SerializeField] Transform shootPoint;


    private void Awake() {
        poolInstance = ProjectillePoolController.instance;
        
    }

    private void OnDisable() {
        CancelInvoke();
    }
    public void AwakePlant(Plant dataRef) {
        data = dataRef;
        InvokeRepeating(nameof(Shoot), 0.1f, data.shootCooldown);
    }
    private void Shoot() {
        GameObject projectille = ProjectillePoolController.instance.projectillePool.Get();
        projectille.transform.position = shootPoint.position;
        projectille.GetComponent<ProjectilleBehaviour>().GetData(data.projectileData);
    }
}
