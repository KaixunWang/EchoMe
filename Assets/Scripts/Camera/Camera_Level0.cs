using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Level0   : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public Vector3 origin=new Vector3(0, 0, 0); // Reference to the origin Transform (assign in Inspector)
    public Vector3 target=new Vector3(0, 0, 0); // Reference to the target Transform (assign in Inspector)
    public Vector3 offset = new Vector3(0, 0, 0);
    public float xBoundary = -4f;
    public float smoothSpeed = 10f;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void LateUpdate()
    {
        if (player==null||player.GetComponent<PlayerBehaviour>()==null||player.GetComponent<PlayerBehaviour>().IsShadow())
            player = GameObject.Find("Shadow(Clone)"); // Switch to Shadow if in shadow mode
        else player = GameObject.Find("Player"); // Ensure we are following the Player
        if (player.transform.position.x > xBoundary)
        {
            Vector3 desiredPosition = target + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 desiredPosition = origin + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
