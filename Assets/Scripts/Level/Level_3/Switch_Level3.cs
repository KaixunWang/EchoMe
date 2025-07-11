using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Switch_Level3 : MonoBehaviour
{
    float timer = 0f;
    public float switchTime = 2f;
    public Cainos.PixelArtPlatformer_Dungeon.Switch switchObj = null;
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time + switchTime;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        if (currentTime >= timer)
        {
            // Switch the level
            if (currentTime >= timer && switchObj != null)
            {
                timer = currentTime + switchTime;
                switchObj.targetPlatform.RemainingCount ++;
            }
            
        }
    }
}
