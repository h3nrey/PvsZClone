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
    Animator anim;

    public UnityEvent onDie;
    public UnityEvent onHurt;

    private void Start() {
        onDie.AddListener(Die);
    }

    [Button("Initialize")]
    private void Initialize() {
        sprRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        GetComponent<Animator>().runtimeAnimatorController = data.animController;
        currentLife = data.life;
        sprRenderer.sprite = data.sprite;
    }
    public void GetData(Creature dataRef) {
        data = dataRef;
        Initialize();
    }

    public void TakeDamage(int damage) {
        currentLife -= damage;
        onHurt?.Invoke();

        if(currentLife <= 0) {
            onDie?.Invoke();
        }
    }

    public void Die() {
        print("die");
        this.gameObject.SetActive(false);
        //Die Stuffs 
    }

    IEnumerator BlinkEffect(Color[] colors, float durantion = 6f) {
        Color baseColor = sprRenderer.color;

        for (int i = 0; i < durantion; i++) {
            foreach (Color color in colors) {
                sprRenderer.color = color;
                yield return new WaitForSeconds(0.1f);
            }
        } 
        sprRenderer.color = baseColor;
    }

    public void StartBlink(Color[] colors, float durantion = 6f) {
        StartCoroutine(BlinkEffect(colors));
    }
}
