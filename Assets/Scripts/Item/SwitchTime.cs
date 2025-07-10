using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTime : MonoBehaviour
{
    public GameObject switchObject; // Reference to the Switch object
    private float InitBarWidth = 0f;
    private bool isOn = false; // 开关状态
    private Vector3 initialPosition; // Initial position of the bar
    private float yoffset = 1f; // Offset to adjust the bar's position
    // Start is called before the first frame update
    void Start()
    {
        InitBarWidth = transform.localScale.x; // Store the initial width of the bar
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z); // Set the initial scale of the bar
        if (switchObject == null)
        {
            Debug.LogError("Switch object not found in the scene. Please assign it in the inspector.");
        }
        else
        {
            initialPosition = switchObject.transform.position + new Vector3(0, yoffset, 0); // Calculate the initial position of the bar based on the switch's position
            transform.position = initialPosition; // Set the initial position of the bar
            isOn = switchObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>().IsOn; // Get the initial state of the switch
            if (isOn)
            {
                UpdateBarWidth(); // Update the bar width if the switch is initially on
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        isOn = switchObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>().IsOn; // Get the current state of the switch
        // if (isOn)
        // {
        //     Debug.Log("Switch is ON, updating bar width.");
            UpdateBarWidth();
        // }
        if(!isOn)
        {
            transform.position = initialPosition; // Reset the position of the bar to its initial position
            // Debug.Log("Switch is OFF, no update needed.");
        }
    }
    private void UpdateBarWidth()
    {
        float sec = switchObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>().GetRemainingTime(); // Get the remaining time from the switch
        float SecToCoolDown = switchObject.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>().GetAutoCloseDelay(); // Get the auto close delay from the switch
        float percent = sec / SecToCoolDown;
        if (sec < 0)
        {
            percent = 0f; // Ensure we don't go below 0
            isOn = false; // Reset the active state if cooldown is complete
        }
        transform.position = initialPosition - new Vector3(InitBarWidth * (1 - percent) / 2, 0, 0); // Adjust the position of the bar based on the width
        transform.localScale = new Vector3(InitBarWidth * percent, transform.localScale.y, transform.localScale.z);
        
    }
}
