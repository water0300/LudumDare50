using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dronefollow : MonoBehaviour
{
    // Update is called once per frame
    public Transform target;
    void Update() {
        transform.position = target.position;
    }
}
