using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject map;
    public List<Vector3> possiblePosition;
    public float shootTime = 2;
    public float portalTime = 10;
    public GameObject player;
    public float bulletForce;
    public Animator anim;
    private List<GameObject> bulletsTracker = new List<GameObject>();
    public int health = 3;
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
        if (health > 0)
        {
            var dir = player.transform.position - this.transform.position;
            if (dir.x > 0)
            {
                anim.SetFloat("x", 1);
            }
            else
            {
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
            if (shootTime > 0)
            {
                shootTime -= Time.deltaTime;
            }
            else
            {
                Shoot();
                shootTime = 2;
            }
        }

    }

    void Shoot()
    {
        var dir = player.transform.position - this.transform.position;
        var dir2D= new Vector2(dir.x, dir.y);

        var startPosition = new Vector2(this.transform.position.x, this.transform.position.y) + this.transform.localScale.x *this.GetComponent<CircleCollider2D>().offset + this.transform.localScale.x*this.GetComponent<CircleCollider2D>().radius*dir2D.normalized;

        GameObject b = UnityEngine.Object.Instantiate(bullet, startPosition, Quaternion.identity);
        Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
        dir = player.transform.position - new Vector3(startPosition.x, startPosition.y, 0);
        dir2D = new Vector2(dir.x, dir.y);
        bulletsTracker.Add(b);

        rb.AddForce(dir2D/5f, ForceMode2D.Impulse);
    }

    public void DeductHealth(int damage) {
        if (health > 0)
        {
            health = health - damage;
            if (health <= 0)
            {
                Debug.Log("check");
                anim.SetBool("dead", true);
                for (int i = 0; i < bulletsTracker.Count; i++)
                {
                    if (bulletsTracker[i])
                    {
                        Destroy(bulletsTracker[i]);
                    }
                }
            }
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
