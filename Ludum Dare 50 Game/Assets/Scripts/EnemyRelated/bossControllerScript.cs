using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject map;
    public List<Vector3> possiblePosition;
    public float portalTime = 10;
    public GameObject player;
    public float bulletForce;
    public Animator anim;

    public int health = 100;
    void Start()
    {
        var tilemap = map.GetComponent<UnityEngine.Tilemaps.Tilemap>();
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                possiblePosition.Add(position);
            }
        }
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var dir = player.transform.position - this.transform.position;
        if (dir.x > 0)
        {
            Debug.Log("pass");
            anim.SetFloat("x", 1);
        }
        else {
            anim.SetFloat("x", 0);
        }
        if (portalTime > 0)
        {
            portalTime -= Time.deltaTime;
        }
        else
        {
            this.transform.position = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
            portalTime = 10;
        }
    }

    private void FixedUpdate()
    {
        Shoot();
    }
    void Shoot()
    {
        GameObject b = Instantiate(bullet, this.transform.position, Quaternion.identity);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        var dir = player.transform.position - this.transform.position;
        rb.AddForce(new Vector2(dir.x, dir.y) * bulletForce, ForceMode2D.Impulse);
    }
}
