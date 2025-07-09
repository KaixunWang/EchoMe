using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesBehavior : MonoBehaviour
{
    float moveSpeed = 0f;

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushLeft()
    {
        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
    }

    public void PushRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
