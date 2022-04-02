using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public GameObject controller;
    public List<GameObject> enemies;
    public float bulletForce = 10f;
    public int damage = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        GameObject b = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        rb.AddForce((firePoint.up + firePoint.right) * bulletForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (GameObject enemy in enemies)
        {
            if (collision.gameObject == enemy)
            {
                //collision.gameObject.health--;
            }
        }
    }
}
