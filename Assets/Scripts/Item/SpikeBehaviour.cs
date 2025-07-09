using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavioudr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player hit the spike!");
            // 示例调用：玩家掉血
            other.GetComponent<PlayerBehaviour>().TakeDamage("Spike");

        }
        if (other.name == "Shadow(Clone)")
        {
            Debug.Log("Shadow hit the spike!");
            // 示例调用：影子掉血
            other.GetComponent<ShadowBehaviour>().DestroybyTrap();
        }
        if (other.name == "Echo(Clone)")
        {
            Debug.Log("Echo hit the spike!");
            // 示例调用：回声掉血
            other.GetComponent<EchoBehaviour>().DestroyImmediate();
        }
    }
}
