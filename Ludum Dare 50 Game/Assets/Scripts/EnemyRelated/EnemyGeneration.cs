using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{

    private int counts = 0;

    public GameObject enemy;

    public GameObject player;

    public GameObject map;

    public GameObject shop;

    private List<Vector3Int> possiblePosition;

    private bool[,] checkTable;

    public List<GameObject> enemies;

    public int enemyNumber = 2;

    public int preventGenerationBlockSize = 1;

    public GameObject target;

    public float[] times;

    private Vector3 playerExtend;

    private float enemyWidth;

    private float enemyHeight;

    private Vector3 enemyExtend;

    private UnityEngine.Tilemaps.Tilemap mapTilemap;

    private UnityEngine.Tilemaps.Tilemap shopTilemap;

    private Vector3 mapCenter;

    private Vector3 mapExtent;

    private List<Vector2Int> dynammicPositionRecord;

    private bool start = true;

    private bool dead = false;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new List<GameObject>();

        possiblePosition = new List<Vector3Int>();

        dynammicPositionRecord = new List<Vector2Int>();

        //Get bounds of palyer
        var playerBounds = player.GetComponent<SpriteRenderer>().bounds;
        //Initialize palyer information
        playerExtend = playerBounds.extents;

        //Get bounds of enemy
        var enemyBounds = enemy.GetComponent<SpriteRenderer>().bounds;
        var enemySize = enemyBounds.size;
        //Initialize palyer information
        enemyWidth = enemySize.x;
        enemyHeight = enemySize.y;
        enemyExtend = enemyBounds.extents;


        mapTilemap = map.GetComponent<UnityEngine.Tilemaps.Tilemap>();
        shopTilemap = shop.GetComponent<UnityEngine.Tilemaps.Tilemap>();


        mapCenter = mapTilemap.transform.position;
        mapExtent = mapTilemap.size / 2;

        checkTableFormation();

    }

    public void enemyCreation(int enemyNumber)
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            int randomP = UnityEngine.Random.Range(0, possiblePosition.Count);
            var position = possiblePosition[randomP];
            var tablePosition = position + mapExtent - mapCenter;
            while (checkTable[(int)tablePosition.x, (int)tablePosition.y])
            {
                randomP = UnityEngine.Random.Range(0, possiblePosition.Count);
                position = possiblePosition[randomP];
                tablePosition = position + mapExtent - mapCenter;
            }
            checkTable[(int)tablePosition.x, (int)tablePosition.y] = true;
            dynammicPositionRecord.Add(new Vector2Int((int)tablePosition.x, (int)tablePosition.y));
            for (int j = 0; j < enemyWidth; j++) {
                for (int k = 0; k < enemyHeight; k++) {
                    var x = (int)(position.x - enemyExtend.x + j);
                    var y = (int)(position.y - enemyExtend.y + k);
                    Vector3 blockedTablePos = new Vector3(x, y, 0) + mapExtent + mapCenter;
                    if ((int)blockedTablePos.x > 0 && (int)blockedTablePos.y > 0)
                    {
                        if (!checkTable[(int)blockedTablePos.x, (int)blockedTablePos.y])
                        {
                            checkTable[(int)blockedTablePos.x, (int)blockedTablePos.y] = true;

                            dynammicPositionRecord.Add(new Vector2Int((int)blockedTablePos.x, (int)blockedTablePos.y));
                        }
                    }
                }
            }

            var temp = UnityEngine.Object.Instantiate(enemy, position, Quaternion.identity);

            temp.transform.position = new Vector3(position.x, position.y, 0);

            temp.GetComponent<EnemyAI>().targetObject = target;

            temp.GetComponent<EnemyAI>().patrolPath = waypointsCreation(position);
            
            temp.GetComponent<EnemyAI>().patrolPathIdleTimes = times;


            enemies.Add(temp);
        }
    }

    public GameObject[] waypointsCreation(Vector3Int position) {
        var distance = UnityEngine.Random.Range(1, 3) * 20;
        Vector3Int vertical = new Vector3Int(0, distance, 0);
        Vector3Int horizontal = new Vector3Int(distance,0, 0);
        var up = position + vertical;
        var down = position - vertical;
        var left = position + horizontal;
        var right = position - horizontal;
        GameObject[] waypoints = new GameObject[2];
        if (possiblePosition.Contains(up))
        {
            GameObject s = new GameObject("waypoint" + counts);
            counts++;
            s.transform.position = up;
            waypoints[0] = s;

            GameObject t = new GameObject("waypoint" + counts);
            counts++;
            t.transform.position = position;
            waypoints[1] = t;
        }
        else {
            if (possiblePosition.Contains(down))
            {
                GameObject s = new GameObject("waypoint" + counts);
                counts++;
                s.transform.position = down;
                waypoints[0] = s;

                GameObject t = new GameObject("waypoint" + counts);
                counts++;
                t.transform.position = position;
                waypoints[1] = t;
            }
            else
            {
                if (possiblePosition.Contains(left))
                {
                    GameObject s = new GameObject("waypoint" + counts);
                    counts++;
                    s.transform.position = left;
                    waypoints[0] = s;

                    GameObject t = new GameObject("waypoint" + counts);
                    counts++;
                    t.transform.position = position;
                    waypoints[1] = t;
                }
                else
                {
                    if (possiblePosition.Contains(right))
                    {
                        GameObject s = new GameObject("waypoint" + counts);
                        counts++;
                        s.transform.position = right;
                        waypoints[0] = s;

                        GameObject t = new GameObject("waypoint" + counts);
                        counts++;
                        t.transform.position = position;
                        waypoints[1] = t;
                    }
                    else
                    {
                        waypointsCreation(position);
                    }
                }
            }
        }


        return waypoints;
    }

    public void clearEnemies()
    {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i] != null)
            {
                for (int j = 0; j < enemies[i].GetComponent<EnemyAI>().patrolPath.Length; j++)
                {
                    Destroy(enemies[i].GetComponent<EnemyAI>().patrolPath[j]);
                }
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();

        foreach (Vector2Int position in dynammicPositionRecord) {
            checkTable[position.x, position.y] = false;
        }
        dynammicPositionRecord.Clear();
    }

    private void checkTableFormation()
    {
        checkTable = new bool[mapTilemap.size.x,mapTilemap.size.y];

        foreach (var position in mapTilemap.cellBounds.allPositionsWithin)
        {
            if (mapTilemap.HasTile(position) && !shopTilemap.HasTile(position))
            {
                var rightUpCorner = position + enemyExtend;
                var leftBotCorner = position - enemyExtend;
                var rightBotCorner = position + enemyExtend - new Vector3(0, 2 * enemyExtend.y, 0);
                var leftUpCorner = position - enemyExtend + new Vector3(0, 2 * enemyExtend.y, 0);
                Vector3Int rightUpCorner1 = new Vector3Int((int)rightUpCorner.x, (int)rightUpCorner.y, (int)rightUpCorner.z);
                Vector3Int leftBotCorner1 = new Vector3Int((int)leftBotCorner.x, (int)leftBotCorner.y, (int)leftBotCorner.z);
                Vector3Int rightBotCorner1 = new Vector3Int((int)rightBotCorner.x, (int)rightBotCorner.y, (int)rightBotCorner.z);
                Vector3Int leftUpCorner1 = new Vector3Int((int)leftUpCorner.x, (int)leftUpCorner.y, (int)leftUpCorner.z);
                var tablePosition = position + mapExtent - mapCenter;
                if (mapTilemap.HasTile(rightUpCorner1) && mapTilemap.HasTile(leftBotCorner1) && mapTilemap.HasTile(rightBotCorner1) && mapTilemap.HasTile(leftUpCorner1))
                {

                    
                    if ((int)tablePosition.x > 0 && (int)tablePosition.y > 0)
                    {
                        possiblePosition.Add(position);
                        checkTable[(int)tablePosition.x, (int)tablePosition.y] = false;
                    }
                }
                else
                {
                    if ((int)tablePosition.x > 0 && (int)tablePosition.y > 0)
                    {
                        checkTable[(int)tablePosition.x, (int)tablePosition.y] = true;
                    }
                }
            }
        }
    }

    private void playerAroundBlock() {
        //player position shouldn't generate and player around within prevent size shouldn't generate

        var playerLonger = playerExtend.y;
        if (playerExtend.x > playerExtend.y)
        {
            playerLonger = playerExtend.y;
        }

        var enemyLonger = enemyExtend.y;
        if (enemyExtend.x > enemyExtend.y)
        {
            enemyLonger = enemyExtend.y;
        }
        var distance = (int) playerLonger + enemyLonger + preventGenerationBlockSize;

        var tablePosition3D = player.transform.position + mapExtent - mapCenter;

        for (int i = 0; i < distance; i++) {
            for (int j = 0; j < distance; j++) {
                Vector2Int tablePosition = new Vector2Int((int)(tablePosition3D.x - distance + i), (int)(tablePosition3D.y - distance + j));
                checkTable[tablePosition.x, tablePosition.y] = true;
                dynammicPositionRecord.Add(tablePosition);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            if (enemies.Count > 0) {
                clearEnemies();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!dead)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("pass");
                playerAroundBlock();
                enemyCreation(enemyNumber);
            }
        }
    }

    public void destroy()
    {
        dead = true;
        clearEnemies();
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
