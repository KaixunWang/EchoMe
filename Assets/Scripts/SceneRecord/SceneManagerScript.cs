using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    private SceneState currentState;
    private bool isRecording = false;
    public GameObject player;
    public List<GameObject> switches; // List of switch GameObjects
    public List<GameObject> pressurePlates; // List of pressure plate GameObjects
    public List<GameObject> doors; // List of door GameObjects
    public List<GameObject> boxes; // List of box GameObjects
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(player != null, "Player GameObject is not assigned in the SceneManagerScript.");
    }

    // Update is called once per frame
    void Update()
    {
        var playerBehaviour = player.GetComponent<PlayerBehaviour>();
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
        foreach (var switchObj in switches)
        {
            var switchComponent = switchObj.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>();
            if (switchComponent != null)
            {
                currentState.switchStates.Add(switchComponent.IsOn);
                Debug.Log($"Switch {switchObj.name} state: {switchComponent.IsOn}");
            }
        }
        foreach (var plateObj in pressurePlates)
        {
            var plateComponent = plateObj.GetComponent<BoardBehavior>();
            if (plateComponent != null)
            {
                currentState.pressurePlateStates.Add(plateComponent.GetBoardState());
                Debug.Log($"Pressure Plate {plateObj.name} state: {plateComponent.GetBoardState()}");
            }
        }
        foreach (var doorObj in doors)
        {
            var doorComponent = doorObj.GetComponent<Cainos.PixelArtPlatformer_Dungeon.Door>();
            if (doorComponent != null)
            {
                currentState.doorStates.Add(doorComponent.IsOpened);
                Debug.Log($"Door {doorObj.name} state: {doorComponent.IsOpened}");
            }
        }
        foreach (var boxObj in boxes)
        {
            currentState.boxPositions.Add(boxObj.transform.position);
            currentState.boxSpeed.Add(boxObj.GetComponent<Rigidbody2D>().velocity); // Assuming you want to save the speed of the box
        }
        isRecording = true;
        Debug.Log("Scene state saved");
    }
    void LoadState()
    {
        if (currentState != null)
        {
            player.transform.position = currentState.playerPosition;
            for(int i = 0; i < switches.Count && i < currentState.switchStates.Count; i++)
            {
                var switchComponent = switches[i].GetComponent<Cainos.PixelArtPlatformer_Dungeon.Switch>();
                if (switchComponent != null)
                {
                    switchComponent.IsOn = currentState.switchStates[i];
                }
            }
            for(int i = 0; i < pressurePlates.Count && i < currentState.pressurePlateStates.Count; i++)
            {
                var plateComponent = pressurePlates[i].GetComponent<BoardBehavior>();
                if (plateComponent != null)
                {
                    plateComponent.SetBoardState(currentState.pressurePlateStates[i]);
                }
            }
            for(int i = 0; i < doors.Count && i < currentState.doorStates.Count; i++)
            {
                var doorComponent = doors[i].GetComponent<Cainos.PixelArtPlatformer_Dungeon.Door>();
                if (doorComponent != null)
                {
                    doorComponent.IsOpened = currentState.doorStates[i];
                }
            }
            for (int i = 0; i < boxes.Count && i < currentState.boxPositions.Count; i++)
            {
                boxes[i].transform.position = currentState.boxPositions[i];
                boxes[i].GetComponent<Rigidbody2D>().velocity = currentState.boxSpeed[i]; // Restore box speed
                // var boxComponent = boxes[i].GetComponent<BoxesBehavior>();
                // if (boxComponent != null)
                // {
                //     boxComponent.transform.position = currentState.boxPositions[i];
                // }
            }
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
