using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public bool isHide = false;
    PlayerLight playerLight;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "HideSpace")
        {
            if (Input.GetKey(KeyCode.E))
            {
                playerLight.LightDown();
                //玩家隐藏
                isHide = true;

                //--------UI提示信息
            }
            else
            {
                playerLight.LightUp();
                print("LightUp");
                isHide = false;
            }
        }
    }
    void Awake()
    {
        playerLight = transform.Find("PlayerLight").GetComponent<PlayerLight>();
    }
}