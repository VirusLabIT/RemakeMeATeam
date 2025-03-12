using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float maxSpeed;
    public float cameraSpeed;
    private GameObject player;
    private Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = transform.position;

        // will be easier to make the camera follow you
        Vector3 playerPosition = new Vector3(player.transform.position.x, cameraPosition.y, player.transform.position.z);

        transform.position = Vector3.SmoothDamp(cameraPosition, playerPosition, ref currentVelocity, cameraSpeed, maxSpeed);
    }
}
