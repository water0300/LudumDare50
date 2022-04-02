using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public GameObject enemy;

    public GameObject player;

    public GameObject map;

    public int enemyNumber = 2;

    public int preventGenerationBlockSize = 1;

    private float mapWidth;

    private float mapHeight;

    private Vector3 mapPosition;

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
        //Get bounds of tileMap
        var mapBounds = map.GetComponent<UnityEngine.Tilemaps.Tilemap>().localBounds;
        var mapSize = mapBounds.size;
        //Initialize map information
        mapWidth = mapSize.x;
        mapHeight = mapSize.y;
        mapPosition = map.transform.position;
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

    }

    private void enemyCreation(int enemyNumber)
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

            UnityEngine.Object.Instantiate(enemy, new Vector3(randomX, randomY, 0) - mapExtend + mapCenter, Quaternion.identity);
        }
    }

    private void checkTableFormation()
    {
        //player position shouldn't generate and player around within prevent size shouldn't generate
        var relativePlayerPosition = playerPosition - mapPosition + mapExtend;
        for (int i = 0; i < playerWidth; i++) {
            for (int j = 0; j < playerHeight; j++) {
                var actualX = (int)(relativePlayerPosition.x - playerExtend.x + i);
                var actualY = (int)(relativePlayerPosition.y - playerExtend.y + i);
                locationCheck[actualX, actualY] = true;
                for (int a = -preventGenerationBlockSize; a <= preventGenerationBlockSize; a++) {
                    for (int b = -preventGenerationBlockSize; b <= preventGenerationBlockSize; b++) {
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
                if (distanceBetweenGivenPointInMap(mapCenter, mapExtend, xRight, yUp) > mapExtend.x ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xRight, yDown) > mapExtend.x ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xLeft, yUp) > mapExtend.x ||
                    distanceBetweenGivenPointInMap(mapCenter, mapExtend, xLeft, yDown) > mapExtend.x) 
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
