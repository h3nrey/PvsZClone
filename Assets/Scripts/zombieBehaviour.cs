using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class zombieBehaviour : MonoBehaviour
{
    [Expandable]
    public Zombie data;

    [SerializeField]
    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    private void FixedUpdate() {
        Move();

    }
    private void Move() {
        rb.velocity = data.speed * Vector2.left * Time.deltaTime;
    }
}
