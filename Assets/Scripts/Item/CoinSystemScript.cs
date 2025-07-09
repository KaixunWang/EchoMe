using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystemScript : MonoBehaviour
{
    private int gotCoin = 0; // 已收集的硬币数量
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddCoin()
    {
        gotCoin++;
        Debug.Log("Coin collected! Total coins: " + gotCoin);
    }
    public int GetCoinCount()
    {
        return gotCoin;
    }

}
