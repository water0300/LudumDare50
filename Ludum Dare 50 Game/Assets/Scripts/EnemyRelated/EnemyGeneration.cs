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

    public List<GameObject> enemies;

    public int enemyNumber = 2;

    public int preventGenerationBlockSize = 1;

    public GameObject target;

    public float[] times;

    private float mapWidth;

    private float mapHeight;

    private Vector3 mapExtend;

    private Vector3 mapCenter;

    private float playerWidth;

    private float playerHeight;

    private Vector3 playerPosition;

    private Vector3 playerExtend;

    private float enemyWidth;

    private float enemyHeight;

    private Vector3 enemyExtend;

    private bool[,] locationCheck;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new List<GameObject>();

        //Get bounds of tileMap
        var mapBounds = map.GetComponent<UnityEngine.Tilemaps.Tilemap>().localBounds;
        var mapSize = mapBounds.size;
        //Initialize map information
        mapWidth = mapSize.x;
        mapHeight = mapSize.y;
        mapExtend = mapBounds.extents;
        mapCenter = mapBounds.center;

        //map array initialize
        locationCheck = new bool[(int)mapWidth, (int)mapHeight];
        System.Array.Clear(locationCheck, 0, (int)mapWidth * (int)mapHeight);

        //Get bounds of palyer
        var playerBounds = player.GetComponent<SpriteRenderer>().bounds;
        var playerSize = playerBounds.size;
        //Initialize palyer information
        playerWidth = playerSize.x;
        playerHeight = playerSize.y;
        playerPosition = player.transform.position;
        playerExtend = playerBounds.extents;

        //Get bounds of enemy
        var enemyBounds = enemy.GetComponent<SpriteRenderer>().bounds;
        var enemySize = enemyBounds.size;
        //Initialize palyer information
        enemyWidth = enemySize.x;
        enemyHeight = enemySize.y;
        enemyExtend = enemyBounds.extents;

        //checkTable formation
        checkTableFormation();
        enemyCreation(enemyNumber);
    }

    public void enemyCreation(int enemyNumber)
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            int randomX = (int)Mathf.Round(UnityEngine.Random.Range(0f, mapWidth - 1f));
            int randomY = (int)Mathf.Round(UnityEngine.Random.Range(0f, mapHeight - 1f));

            while (locationCheck[randomX, randomY])
            {
                randomX = (int)Mathf.Round(UnityEngine.Random.Range(0f, mapWidth - 1f));
                randomY = (int)Mathf.Round(UnityEngine.Random.Range(0f, mapHeight - 1f));
            }
            for (int j = 0; j < enemyWidth; j++) {
                for (int k = 0; k < enemyHeight; k++) {
                    locationCheck[(int)(randomX - enemyExtend.x + j), (int)(randomY - enemyExtend.y + k)] = true;
                }
            }

            var temp = UnityEngine.Object.Instantiate(enemy, new Vector3(randomX, randomY, 0) - mapExtend + mapCenter, Quaternion.identity);

            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0f);

            temp.GetComponent<EnemyAI>().targetObject = target;

            temp.GetComponent<EnemyAI>().patrolPath = waypointsCreation(randomX, randomY); ;
            
            temp.GetComponent<EnemyAI>().patrolPathIdleTimes = times;


            enemies.Add(temp);
        }
    }

    public GameObject[] waypointsCreation(int x, int y) {
        var distance = UnityEngine.Random.Range(1, 3) * 20;
        GameObject[] waypoints = new GameObject[2];
        if (x + distance > mapWidth)
        {
            distance = -distance;
            if (x + distance < 0)
            {
                if (y + distance < 0)
                {
                    distance = -distance;
                    if (y + distance > mapHeight)
                    {
                        waypointsCreation(x, y);
                    }
                    else {
                        GameObject s = new GameObject("waypoint" + counts);
                        counts++;
                        s.transform.position = new Vector3(x, y + distance, 0) + mapCenter - mapExtend;
                        waypoints[0] = s;

                        GameObject t = new GameObject("waypoint" + counts);
                        counts++;
                        t.transform.position = new Vector3(x, y, 0) + mapCenter - mapExtend;
                        waypoints[1] = t;
                    }
                }
                else {
                    GameObject s = new GameObject("waypoint" + counts);
                    counts++;
                    s.transform.position = new Vector3(x, y + distance, 0) + mapCenter - mapExtend;
                    waypoints[0] = s;

                    GameObject t = new GameObject("waypoint" + counts);
                    counts++;
                    t.transform.position = new Vector3(x, y, 0) + mapCenter - mapExtend;
                    waypoints[1] = t;
                }
            }
            else
            {
                GameObject s = new GameObject("waypoint" + counts);
                counts++;
                s.transform.position = new Vector3(x + distance, y, 0) + mapCenter - mapExtend;
                waypoints[0] = s;

                GameObject t = new GameObject("waypoint" + counts);
                counts++;
                t.transform.position = new Vector3(x, y, 0) + mapCenter - mapExtend;
                waypoints[1] = t;
            }
        }
        else
        {

            GameObject s = new GameObject("waypoint" + counts);
            counts++;
            s.transform.position = new Vector3(x + distance, y, 0) + mapCenter - mapExtend;
            waypoints[0] = s;

            GameObject t = new GameObject("waypoint" + counts);
            counts++;
            t.transform.position = new Vector3(x, y, 0) + mapCenter - mapExtend;
            waypoints[1] = t;

        }
        return waypoints;
    }

    public void clearEnemies()
    {
        for (int i = 0; i < enemies.Count; i++) {
            Destroy(enemies[i]);
        }
        enemies.Clear();
    }

    private void checkTableFormation()
    {
        //player position shouldn't generate and player around within prevent size shouldn't generate
        var relativePlayerPosition = playerPosition - mapCenter + mapExtend;
        for (int i = 0; i < playerWidth; i++)
        {
            for (int j = 0; j < playerHeight; j++)
            {
                var actualX = (int)(relativePlayerPosition.x - playerExtend.x + i);
                var actualY = (int)(relativePlayerPosition.y - playerExtend.y + i);
                locationCheck[actualX, actualY] = true;
                for (int a = -preventGenerationBlockSize; a <= preventGenerationBlockSize; a++)
                {
                    for (int b = -preventGenerationBlockSize; b <= preventGenerationBlockSize; b++)
                    {
                        if (actualX + a < mapWidth && actualX + a >= 0 && actualY + b < mapHeight && actualY + b >= 0)
                            locationCheck[actualX + a, actualY + b] = true;
                    }
                }
            }
        }

        //circle checking and enemy size checking
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                //enemy boundraies
                var xRight = i + enemyExtend.x;
                var xLeft = i - enemyExtend.x;
                var yUp = j + enemyExtend.y;
                var yDown = j - enemyExtend.y;
                //enemy boundray case
                if ( xRight > mapWidth || xLeft < 0 || yUp > mapHeight || yDown < 0){
                    locationCheck[i, j] = true;
                }
                //circle calculation
                var distance = distanceBetweenGivenPointInMap(mapCenter, mapExtend, i,j);
                //circle boundray case
                if( distance > mapExtend.x){
                    locationCheck[i, j] = true;
                }
                //enemy circle boundray cases
                var circlecheker = Mathf.Abs(mapExtend.x);
                if (Mathf.Abs(mapExtend.x) > Mathf.Abs(mapExtend.y)) {
                    circlecheker = Mathf.Abs(mapExtend.y);
                }
                if (distanceBetweenGivenPointInMap(mapCenter, mapExtend, xRight, yUp) > circlecheker ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xRight, yDown) > circlecheker ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xLeft, yUp) > circlecheker ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xLeft, yDown) > circlecheker) 
                {
                    locationCheck[i, j] = true;
                }
            }
        }

    }

    private float distanceBetweenGivenPointInMap(Vector3 center, Vector3 extend, float x, float y) { 
        return Vector3.Distance(new Vector3(x, y, 0) + mapCenter - mapExtend, mapCenter);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
