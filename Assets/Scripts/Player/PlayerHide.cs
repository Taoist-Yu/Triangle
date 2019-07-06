using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public bool isHide = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "HideSpace")
        {
            if (Input.GetKey(KeyCode.E))
            {
                //玩家隐藏
                isHide = true;
            }
            else
            {
                isHide = false;
            }
        }
    }

}