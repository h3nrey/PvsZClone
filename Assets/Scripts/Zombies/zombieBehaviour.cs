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

    [SerializeField] LayerMask plantLayer;
    [SerializeField] float attackRange;
    private RaycastHit2D attackRay;
    [SerializeField] float attackCooldown;
    [ReadOnly] [SerializeField]private float attackTime;
    [ReadOnly] [SerializeField] bool hasTarget;

    [SerializeField]
    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    private void FixedUpdate() {
        Move();

        CheckRange();
        Attack();

    }
    private void Move() {
        if (canMove) {
            rb.velocity = data.speed * Vector2.left * Time.deltaTime;
        } else {
            rb.velocity = Vector2.zero;
        }
        
    }

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

    //Gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = hasTarget ? Color.blue : Color.red;
        Gizmos.DrawRay(transform.position, attackRange * Vector2.left);
    }
}
