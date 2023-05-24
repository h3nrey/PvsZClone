using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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

    [Button("Initialize")]
    private void Initialize() {
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentLife = data.life;
        sprRenderer.sprite = data.sprite;
    }
    public void GetData(Creature dataRef) {
        data = dataRef;
        Initialize();
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
