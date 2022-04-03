using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public GameObject shop;
    public Camera firstCam;
    public Camera secondCam;
    Vector3 offset = new Vector3(0, 100, 0);

    private void Start()
    {
        firstCam.enabled = true;
        secondCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hhhh");
            player.transform.position = shop.transform.position + offset;

            if (firstCam.enabled)
            {
                firstCam.enabled = false;
                secondCam.enabled = true;
            }

            else
            {
                firstCam.enabled = true;
                secondCam.enabled = false;
            }
        }
    }
}
