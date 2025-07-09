using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject oneStar;  // 1star
    public GameObject twoStar;  // 2star
    public GameObject threeStar; // 3star
    // Start is called before the first frame update
    void Start()
    {
        SetStars(0); // 初始化时隐藏所有星星
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetStars(int score)
    {
        // 隐藏所有星星
        oneStar.SetActive(false);
        twoStar.SetActive(false);
        threeStar.SetActive(false);

        // 根据分数显示对应的星星
        if (score >= 1) oneStar.SetActive(true);  // 激活1颗星
        if (score >= 2) twoStar.SetActive(true);  // 激活2颗星
        if (score >= 3) threeStar.SetActive(true);  // 激活3颗星
    }
}
