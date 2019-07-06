using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //----------------------------动画状态切换--- -----------
    enum PlayerStatus
    {
        PS_None,
        PS_Stand,
        PS_Run,
        PS_Dead
    }

    PlayerStatus curStatus = PlayerStatus.PS_Stand;
    PlayerStatus nextStatus = PlayerStatus.PS_None;

    Ray2D ray2D;

    //private Animator animator;

    //------------------------------------玩家的移动--------------------------
    public enum Directon
    {
        D_NONE,
        D_RIGHT,
        D_LEFT,
    }

    float moveSpeed = 5.0f;
    Directon curDirection = Directon.D_RIGHT;

    //获取精灵对象的SpriteRenderer组件
    SpriteRenderer spriteRenderer;

    //-------------------------------------------------------------------------

    // Use this for initialization
    void Start()
    {
        //发射射线
        //ray2D = new Ray2D(transform.position, Vector2.right);

        //animator = this.GetComponent<Animator>();

        //获得当前对象的SpriteRenderer组件
        spriteRenderer = this.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------实现动画状态的切换------------------------------------------------
        //1# 根据[用户输入和游戏逻辑] ，在每帧 检查 状态 是否 会有变化

        ////当玩家有移动时，就切换为行走状态-------
        var xDelta = Input.GetAxis("Horizontal");
        //var yDelta = Input.GetAxis("Vertical");

        if (xDelta != 0 /*|| yDelta != 0*/)
        {
            nextStatus = PlayerStatus.PS_Run;
        }

        //------时刻检测 当玩家没有移动时，就切换为待机状态-------
        if (xDelta == 0/* && yDelta == 0*/)
        {
            nextStatus = PlayerStatus.PS_Stand;
        }
        ////---当玩家 按下J时，就切换为攻击状态-
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    nextStatus = PlayerStatus.PS_Attack;
        //}

        //2# 更新当前动画状态，并作逻辑处理
        //if (nextStatus != curStatus)
        //{
        //    //重置参数，避免上次遗留的状态，影响下次判断
        //    resetAnimatorParamsValue();

        //    switch (nextStatus)
        //    {
        //        case PlayerStatus.PS_Stand:
        //            {
        //                if (curStatus == PlayerStatus.PS_Run)
        //                {
        //                    animator.SetBool("Run2Stand", true);
        //                }

        //                break;
        //            }
        //        case PlayerStatus.PS_Run:
        //            {
        //                animator.SetTrigger("ToRun");
        //                break;
        //            }
        //        //case PlayerStatus.PS_Attack:
        //        //    {
        //        //        animator.SetTrigger("ToAttack");
        //        //        break;
        //        //    }
        //        default: break;
        //    }

        //    curStatus = nextStatus;
        //    print("curStatus = " + curStatus);
        //}
    }


    //void resetAnimatorParamsValue()
    //{
    //    animator.SetBool("Run2Stand", false);
    //    animator.SetBool("Death2Stand", false);
    //}
    
    void moveControl()
    {
        //if (curStatus == PlayerStatus.PS_Attack)
        //{
        //    return;
        //}

        //--------根据按键输入移动玩家-------------  
        float xDeltaMove = Input.GetAxis("Horizontal");
        //float yDeltaMove = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(xDeltaMove, 0);
        this.transform.Translate(movement.normalized * moveSpeed * Time.deltaTime);

        //--------更新玩家方向--------
        if (xDeltaMove < 0 || Input.GetKeyDown(KeyCode.A)) //按下A键
        {
            curDirection = Directon.D_LEFT;  //当前方向 更新为左
        }
        else if (xDeltaMove > 0 || Input.GetKeyDown(KeyCode.D))
        {
            curDirection = Directon.D_RIGHT; //当前方向 更新为右
        }

        //根据实时方向去反转精灵对象
        //---再根据方向做处理
        switch (curDirection)
        {
            case Directon.D_LEFT:
                {
                    spriteRenderer.flipX = true;
                    //------------------------
                    ray2D = new Ray2D(transform.position, -Vector2.right);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(this.transform.position, new Vector2(this.transform.position.x+1,this.transform.position.y));//起点，方向，颜色（可选）
                    RaycastHit2D info = Physics2D.Raycast(ray2D.origin, ray2D.direction, 1.0f);

                    if (info.collider != null)
                    {//如果发生了碰撞
                        GameObject obj = info.collider.gameObject;
                        if (obj.CompareTag("Lift"))//用tag判断碰到了什么对象
                        {
                            Debug.Log(obj.name);
                        }

                    }

                    break;
                }

            case Directon.D_RIGHT:
                {
                    spriteRenderer.flipX = false;
                    //检测玩家前方向是否有电梯
                    ray2D = new Ray2D(transform.position, Vector2.right);
                    Debug.DrawRay(ray2D.origin, ray2D.direction, Color.red);//起点，方向，颜色（可选）
                    RaycastHit2D info = Physics2D.Raycast(ray2D.origin, ray2D.direction, 0.5f);

                    if (info.collider != null)
                    {//如果发生了碰撞
                        GameObject obj = info.collider.gameObject;
                        if (obj.CompareTag("Lift"))//用tag判断碰到了什么对象
                        {
                            Debug.Log(obj.name);
                        }

                    }
                    break;
                }

            default: break;
        }
    }
    void FixedUpdate()
    {        
        moveControl();
    }
}
