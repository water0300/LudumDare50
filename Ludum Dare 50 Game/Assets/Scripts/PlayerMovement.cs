using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 60f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;
    public GameObject weapon;
    private float horizontal;
    private float vertical;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Shooting gun = weapon.GetComponent<Shooting>();

        animator.SetFloat("Horizontal", gun.lookDir.x);
        animator.SetFloat("Vertical", gun.lookDir.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


}
