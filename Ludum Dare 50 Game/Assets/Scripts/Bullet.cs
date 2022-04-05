using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("asdf");
            collision.gameObject.GetComponent<EnemyAI>().DeductHealth(damage);

            Destroy(this.gameObject);
            //Debug.Log(collision.collider.gameObject.GetComponent<Player>().health);
        }

        if (collision.gameObject.CompareTag("blocks")){
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<bossControllerScript>().DeductHealth(damage);
            Destroy(this.gameObject);
            //Debug.Log(collision.collider.gameObject.GetComponent<Player>().health);
        }
    }
}
