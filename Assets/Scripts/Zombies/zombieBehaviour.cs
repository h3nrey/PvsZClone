using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class zombieBehaviour : MonoBehaviour
{
    [Expandable]
    public Zombie data;

    private int damage => data.damage;

    private bool canMove;

    [Header("Attack")]
    [SerializeField] LayerMask plantLayer;
    [SerializeField] float attackRange;
    private RaycastHit2D attackRay;
    [SerializeField] float attackCooldown;
    [ReadOnly] [SerializeField]private float attackTime;
    [ReadOnly] [SerializeField] bool hasTarget;

    [Header("Components")]
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private CreatureBehaviour crBehaviour;
    private Animator anim;

    [Header("colors")]
    [SerializeField]
    Color[] hurtColors;
    [SerializeField]
    float blinkDuration;

    public void LoadData(Zombie dataRef) {
        data = dataRef;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        crBehaviour.onHurt.AddListener(() => crBehaviour.StartBlink(hurtColors, blinkDuration));
        canMove = true;
    }

    private void Update() {
        anim.SetBool("Moving", canMove);
        anim.SetBool("Eating", hasTarget);
    }

    private void FixedUpdate() {
        Move();

        CheckRange();
        Attack();

    }

    #region Collision
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject otherObj = other.gameObject;

        if (otherObj.CompareTag(GameTags.house)) {
            GameManager.game.onZombiesReach?.Invoke();
        }

        if(otherObj.CompareTag(GameTags.projectille)) {
            crBehaviour.TakeDamage(data.damage);
        }
    }
    #endregion

    #region Movement
    private void Move() {
        if (canMove) {
            rb.velocity = data.speed * Time.deltaTime * Vector2.left;
        } else {
            rb.velocity = Vector2.zero;
        }
        
    }
    #endregion

    #region Attack
    private void CheckRange() {
        attackRay = Physics2D.Raycast(transform.position, Vector2.left, attackRange, plantLayer);

        if(attackRay.collider) {
            hasTarget = true;
            canMove = false;
        } else {
            hasTarget = false;
            canMove = true;
        }
    }

    private void Attack() {
        if (hasTarget && Time.time > attackTime) {
            GameObject targetPlant = attackRay.collider.gameObject;
            targetPlant.GetComponent<CreatureBehaviour>().TakeDamage(damage);
            attackTime = Time.time + attackCooldown;
        }
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = hasTarget ? Color.blue : Color.red;
        Gizmos.DrawRay(transform.position, attackRange * Vector2.left);
    }
    #endregion
}
