using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public GameObject map;
    public List<Vector3> possiblePosition;
    public float shootTime = 0.5f;
    public float portalTime = 10;
    public GameObject player;
    public float bulletForce;
    public Animator anim;
    private List<GameObject> bulletsTracker = new List<GameObject>();
    public int health = 3; 
    public GameObject bulletShootPoint;
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

        player.GetComponent<Player>().onBossSpawn?.Invoke();
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
                this.gameObject.transform.localScale = new Vector3(-Mathf.Abs(this.gameObject.transform.localScale.x), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            }
            else
            {
                anim.SetFloat("x", 0);
                this.gameObject.transform.localScale = new Vector3(Mathf.Abs(this.gameObject.transform.localScale.x), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
            }

            if (portalTime > 0)
            {
                portalTime -= Time.deltaTime;
            }
            else
            {
                var portalPosition = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
                var bossExtend = this.gameObject.GetComponent<SpriteRenderer>().bounds.extents;
                var leftBot = portalPosition - bossExtend;
                var rightUp = portalPosition + bossExtend;
                var playerExtend = player.gameObject.GetComponent<SpriteRenderer>().bounds.extents;
                var playerLeftBot = player.transform.position - playerExtend;
                var playerRightUp = player.transform.position + playerExtend;
                while (
                    (playerLeftBot.x < rightUp.x && playerLeftBot.x > leftBot.x) ||
                    (playerRightUp.x < rightUp.x && playerRightUp.x > leftBot.x) ||
                    (playerLeftBot.y < rightUp.y && playerLeftBot.y > leftBot.y) ||
                    (playerRightUp.y < rightUp.y && playerRightUp.y > leftBot.y)
                    ) {

                    portalPosition = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
                    leftBot = portalPosition - bossExtend;
                    rightUp = portalPosition + bossExtend;
                }
                this.transform.position = portalPosition;

                portalTime = 10;
            }
            if (shootTime > 0)
            {
                shootTime -= Time.deltaTime;
            }
            else
            {
                Shoot();
                shootTime = 1;
            }
        }

    }

    void Shoot()
    {
        for (int i = -50; i <= 50; i = i + 50)
        {
            var dir =player.transform.position - bulletShootPoint.transform.position;
            dir.x = dir.x + i;
            var dir2D = new Vector2(dir.x, dir.y);

            GameObject b = UnityEngine.Object.Instantiate(bullet, bulletShootPoint.transform.position, Quaternion.identity);
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            bulletsTracker.Add(b);

            rb.AddForce(dir2D.normalized * bulletForce, ForceMode2D.Impulse);
        }
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
        player.GetComponent<Player>().onWin?.Invoke();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            this.transform.position = possiblePosition[UnityEngine.Random.Range(0, possiblePosition.Count)];
        }
    }
}
