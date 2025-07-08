using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private static InteractiveManager sManager;

    [SerializeField]
    private Cainos.PixelArtPlatformer_Dungeon.Door door = null;

    public void TriggerDoor(){
        if (isOpen && door != null)
        {
            Debug.Log("Open the door");
            door.SetDoor(true);
        }
        else if (!isOpen && door != null)
        {
            Debug.Log("Close the door");
            door.SetDoor(false);
        }
    }
    public static void InitializeBoardSystem(InteractiveManager I)
    {
        // This method can be used to initialize the board system if needed
        // For now, it does nothing but can be expanded later
        sManager = I;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isOpen", isOpen);
        TriggerDoor();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Open the door");
            isOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            isOpen = false;
    }

    public bool GetBoardState()
    {
        return isOpen;
    }
    public void SetBoardState(bool state)
    {
        isOpen = state;
    }
}
