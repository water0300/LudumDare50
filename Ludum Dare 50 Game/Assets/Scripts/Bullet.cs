using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public List<GameObject> enemies;
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
        foreach (GameObject enemy in enemies)
        {
            if (collision.collider.gameObject.name.Contains(enemy.name))
            {
                collision.collider.gameObject.GetComponent<EnemyAI>().DeductHealth(damage);
                //Debug.Log(collision.collider.gameObject.GetComponent<Player>().health);
            }
        }
    }
}
