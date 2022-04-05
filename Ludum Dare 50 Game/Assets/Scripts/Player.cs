using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    public int health = 3;
    public int startTime = 60;
    public int coins = 0;
    public int damage;
    public AudioSource audio;
    public Animator anim;
    public Rigidbody2D rb;
    public UnityEvent onHitEvent;
    public UnityEvent onHealEvent;
    public UnityEvent coinEvent;
    public UnityEvent onTimeEvent;
    public UnityEvent onLoss;
    public UnityEvent onWin;

    public int priceInc = 5;
    public int time {get; set; } = 60;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject == null)
        {
            Debug.Log("Game over");
            StopAllCoroutines();   
        }
    }


    public void OnShopHeal(){ //event
        if(coins >= priceInc){
            health = Mathf.Min(health+1, 8);
            onHealEvent?.Invoke();
            coins-= priceInc;
            coinEvent?.Invoke();
            priceInc+=5;
        }
    }

    public void OnGainTime(){
        if(coins >= priceInc){
            time += 5;
            onTimeEvent?.Invoke();
            coins-= priceInc;
            coinEvent?.Invoke();
            priceInc+=5;
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




    void deathAnim()
    {
        audio.PlayOneShot(audio.clip);
        anim.Play("Base Layer.Death");
    }
 
    public void OnDeath()
    {
        deathAnim();
        StopAllCoroutines();
        onLoss?.Invoke();
        Destroy(this.gameObject);
    }

}
