using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class PlayerAutoSpeed : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 10.0f;
    int tmp1 = 0;
    GameObject pointTarget;
    Animator animator1;
    void Start()
    {
        pointTarget = GameObject.Find("PointTarget");
        animator1 = this.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tmp1 < 60)
        {
            this.transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);
            animator1.SetBool("ToRunAnim", true);
            tmp1 += 1;
            print(tmp1);
        }
        else
        {
            animator1.SetTrigger("Run2Stand");
            animator1.SetBool("ToRunAnim", false);
            this.transform.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}
