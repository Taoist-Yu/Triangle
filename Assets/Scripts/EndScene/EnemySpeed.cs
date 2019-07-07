using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpeed : MonoBehaviour
{
    public float moveSpeed2 = 10.0f;
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
            this.transform.Translate(moveSpeed2 * Time.deltaTime * Vector2.right);
            tmp2 += 1;
            print(tmp2);

            this.transform.DOScale(0, 2);
        }

    }
}
