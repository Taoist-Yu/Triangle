using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    //=====玩家隐藏--------默认不隐藏
    public bool isHide = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "HideSpace")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //玩家隐藏，周围光线变暗，只显示玩家心脏闪烁
                print("111");
                isHide = true;
            }
        }
        if (other.transform.tag == "Chip")
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                //玩家捡起碎片
                print("111");
            }
        }
    }
}