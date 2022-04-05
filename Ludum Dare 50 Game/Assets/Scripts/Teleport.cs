using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    public Gun gun;
    public GameObject targetTP;
    public Camera firstCam;
    public Camera secondCam;
    Vector3 offset = new Vector3(0, 100, 0);
    public UnityEvent shopEnter;
    public UnityEvent shopExit;

    bool inZone = false;

    public void Start(){
        gun = FindObjectOfType<Gun>();
    }

    // void Update(){
    //     if (inZone)
    //     {
    //         Debug.Log("hhhh");
    //         player.transform.position = targetTP.transform.position + offset;

    //         if (firstCam.enabled)
    //         {
    //             firstCam.enabled = false;
    //             secondCam.enabled = true;
    //         }

    //         else
    //         {
    //             firstCam.enabled = true;
    //             secondCam.enabled = false;
    //         }
    //     }
    // }
    void OnTriggerEnter2D(Collider2D col){
        gun.inShop = true;
        inZone = true;
        shopEnter?.Invoke();
    }

    void OnTriggerExit2D(Collider2D other) {
        gun.inShop = false;
        inZone = false;
        shopExit?.Invoke();
    }


}
