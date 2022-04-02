using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health = 3;
    public List<GameObject> enemies;
    public int damage;

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
        foreach (GameObject enemy in enemies)
        {
            if (collision.gameObject == enemy)
            {
                health--;
                if (health == 0)
                {
                    OnDeath();
                }
            }
        }
    }

    void OnDeath()
    {
        Application.Quit();
    }
}
