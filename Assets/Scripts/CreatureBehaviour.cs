using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class CreatureBehaviour : MonoBehaviour
{
    public Creature data;

    [SerializeField] 
    private int currentLife;

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer sprRenderer;

    [SerializeField]
    private Animator anim;

    private void Awake() {
        Initialize();
        GetData();
        currentLife =  data.life;
    }

    private void Initialize() {
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void GetData() {
        sprRenderer.sprite = data.sprite;
    }

    public void TakeDamage(int damage) {
        currentLife -= damage;
        print($"{name} is taking {damage} of damage");

        if(currentLife <= 0) {
            Die();
        }
    }

    public void Die() {
        print("die");
        this.gameObject.SetActive(false);
        //Die Stuffs 
    }
}
