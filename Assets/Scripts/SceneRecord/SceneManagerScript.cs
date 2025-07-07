using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    private SceneState currentState;
    private bool isRecording = false;
    public GameObject player;
    public PlayerBehaviour playerBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player != null, "Player GameObject is not assigned in the SceneManagerScript.");
        Debug.Assert(playerBehaviour != null, "PlayerBehaviour component is not assigned in the SceneManagerScript.");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerBehaviour.getState() && !isRecording)
        {
            SaveState();
        }
        if (!(playerBehaviour.getState()) && isRecording)
        {
            LoadState();
        }
    }

    void SaveState()
    {
        currentState = new SceneState {
            playerPosition = player.transform.position
            // switchState = switchState, // Assuming you have a switch state
            // pressurePlateState = pressurePlateState, // Assuming you have a pressure plate state
            // doorState = doorState, // Assuming you have a door state
            // boxPosition = boxPosition // Assuming you have a box position
        };
        isRecording = true;
        Debug.Log("Scene state saved");
    }
    void LoadState()
    {
        if (currentState != null)
        {
            player.transform.position = currentState.playerPosition;
            // Restore other states as needed
            isRecording = false;
            Debug.Log("Scene state loaded");
        }
        else
        {
            Debug.LogWarning("No saved state to load");
        }
    }
}
