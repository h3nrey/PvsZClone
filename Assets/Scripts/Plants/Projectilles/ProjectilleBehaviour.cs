using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilleBehaviour : MonoBehaviour
{
    [SerializeField] private Projectille data;

    [SerializeField] int zombieLayer = 8;
    private SpriteRenderer sprRenderer;
    private Rigidbody2D rb;
    private bool dataGetted;

    private void OnBecameInvisible() {
        ProjectillePoolController.instance.projectillePool.Release(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (dataGetted) {
            if (other.gameObject.layer == zombieLayer) {
                ProjectillePoolController.instance.projectillePool.Release(this.gameObject);
                other.gameObject.GetComponent<CreatureBehaviour>().TakeDamage(data.damage);
            }
        }
    }

    private void FixedUpdate() {
        if(dataGetted) {
            rb.velocity = Vector2.right * data.speed * Time.fixedDeltaTime;
        }
    }
    public void GetData(Projectille myData) {
        data = myData;
        print($"{myData} getted");
        dataGetted = true;
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.sprite = data.sprite;
    }


}
