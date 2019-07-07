using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVSpeed : MonoBehaviour
{
    public float moveSpeed3 = 10.0f;
    int tmp2 = 0;
    GameObject pointTarget1;
    void Start()
    {
        pointTarget1 = GameObject.Find("PointTarget1");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tmp2 < 40)
        {
            this.transform.Translate(moveSpeed3 * Time.deltaTime * Vector2.right);
            tmp2 += 1;
            print(tmp2);
        }
    }
}
