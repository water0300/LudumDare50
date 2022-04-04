using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    public int health = 3;
    public int damage;
    public AudioSource audio;
    public Animator anim;
    public Rigidbody2D rb;
    public UnityEvent onHitEvent;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject == null)
        {
            Debug.Log("Game over");
            StopAllCoroutines();
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit(0);
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("enemy"))
    //     {
    //         if (this.gameObject.GetComponent<PlayerMovement>().bounceTime != -1){
    //             Debug.Log("taking damage");
    //             health--;
    //             onHitEvent?.Invoke();
    //             Debug.Log(health);
    //             if (health == 0)
    //             {
    //                 OnDeath();
    //             }
    //         }
    //     }
    // }

    //this isn't going to work
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //boss bullet
        if (collision.gameObject.CompareTag("bossBullet"))
        {
            health--;
            Destroy(collision.gameObject);
            if (health == 0)
            {
                OnDeath();
            }
        }
    }




    void deathAnim()
    {
        audio.PlayOneShot(audio.clip);
        anim.Play("Base Layer.Death");
    }
 
    public void OnDeath()
    {
        deathAnim();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

}
