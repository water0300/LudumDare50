using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody2D rb;
    public Camera cam;
    Vector2 mousePos;
    public GameObject player;
    private Vector2 offset = new Vector2(0.25f, -0.25f);
    Vector3 position;
    public Vector2 lookDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position = player.transform.position;
        Vector2 playerPosition = new Vector2(position.x, position.y);
        transform.position = playerPosition + offset;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        lookDir = mousePos - rb.position;
        Debug.Log(lookDir);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 45f;
        rb.rotation = angle;
    }
}
