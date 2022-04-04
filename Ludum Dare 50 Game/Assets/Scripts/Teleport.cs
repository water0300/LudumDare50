using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public GameObject targetTP;
    public Camera firstCam;
    public Camera secondCam;
    Vector3 offset = new Vector3(0, 100, 0);

    private void Start()
    {
        firstCam.enabled = true;
        secondCam.enabled = false;
    }

  
    void OnTriggerEnter2D(Collider2D col){

    }

    void OnTriggerExit2D(Collider2D other) {
        
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("weorjo");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hhhh");
            player.transform.position = targetTP.transform.position + offset;

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
