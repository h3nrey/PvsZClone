using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Utils;

public class PlantBehaviour : MonoBehaviour
{
    [Expandable]
    public Plant data;

    ProjectillePoolController poolInstance;

    [Header("shootable")]
    [SerializeField] Transform shootPoint;
    [SerializeField] LayerMask zombieLayer;
    [SerializeField] bool zombieOnRange;

    [Header("Sunflower")]
    [SerializeField] GameObject sunPrefab;
    [SerializeField] float sunCooldown;

    [SerializeField] Animator anim;
    [SerializeField] CreatureBehaviour crBehaviour;



    private void Awake() {
        poolInstance = ProjectillePoolController.instance;
        
    }

    private void Start() {
        anim = GetComponent<Animator>();

        Transform cellTransform = transform.parent;
        CellBehaviour cell = cellTransform.GetComponent<CellBehaviour>();

        crBehaviour.onDie.AddListener(
            () => cell.RemovePlant());
    }

    private void OnDisable() {
        CancelInvoke();
        StopAllCoroutines();
    }
    public void AwakePlant(Plant dataRef) {
        data = dataRef;
        GetComponent<CreatureBehaviour>().GetData(dataRef);

        if(data.isShootable) {
            InvokeRepeating(nameof(Shoot), 0.1f, data.shootCooldown);
        } else {
            Coroutines.DoAfter(() => StartCoroutine(CreateSun()), sunCooldown, this);
        }
    }

    private void FixedUpdate() {
        CheckRange();
    }

    private void CheckRange() {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, data.shootRange, zombieLayer);

        if (ray.collider) {
            zombieOnRange = true;
        }
        else {
            zombieOnRange = false;
        }
    }
    private void Shoot() {
        if(zombieOnRange) {
            anim.SetTrigger("shoot");
            Coroutines.DoAfter(() => {
                GameObject projectille = ProjectillePoolController.instance.projectillePool.Get();
                projectille.transform.position = shootPoint.position;
                projectille.GetComponent<ProjectilleBehaviour>().GetData(data.projectileData);
            }, 0.5f, this);
        }
    }

    private IEnumerator CreateSun() {
        while (true) {
            GameObject sunObj = SunSpawner.instance.sunPool.Get();
            sunObj.GetComponent<SunBehaviour>().isPlantSun = true;
            sunObj.GetComponent<SunBehaviour>().Launch(transform.position);

            yield return new WaitForSeconds(sunCooldown);
        }
    }

    #region Gizmos
    private void OnDrawGizmosSelected() {
        Color rayColor = zombieOnRange ? Color.red : Color.red;
        Gizmos.color = rayColor;
        float distance = data != null ? data.shootRange : 200f;
        Gizmos.DrawRay(new Ray(transform.position, Vector2.right * distance));
        print("gizmos");
    }
    #endregion
}
