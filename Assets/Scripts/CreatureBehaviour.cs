using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
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

    public UnityEvent onDie;

    private void Start() {
        onDie.AddListener(Die);
    }

    [Button("Initialize")]
    private void Initialize() {
        sprRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        GetComponent<Animator>().runtimeAnimatorController = data.animController;
        print("initialize");
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
            onDie?.Invoke();
        }
    }

    public void Die() {
        print("die");
        this.gameObject.SetActive(false);
        //Die Stuffs 
    }
}
