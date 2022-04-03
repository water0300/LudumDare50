using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public GameObject controller;
    public float bulletForce = 10f;
    public int damage = 1;
    public float horizontal;
    public float vertical;

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
}
