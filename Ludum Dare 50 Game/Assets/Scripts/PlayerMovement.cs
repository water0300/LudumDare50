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
    public Player player;
    public float bounceTime = -1;
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        Shooting gun = weapon.GetComponent<Shooting>();
        player = GetComponent<Player>();
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (bounceTime <= 0)
        {
            bounceTime = -1;
            rb.velocity = Vector2.zero;
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            bounceTime -= Time.deltaTime;
        }
    }

    //assume this is when player takes damage lol
    public void bounce(Vector3 velocity)
    {

        Debug.Log(velocity);
        rb.AddForce(velocity * 100, ForceMode2D.Impulse);
        bounceTime = 1;

        Debug.Log("taking damage");
        player.health--;
        player.onHitEvent?.Invoke();
        if (player.health == 0)
        {
            player.OnDeath();
        }
    }

}
