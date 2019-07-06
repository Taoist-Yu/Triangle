﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("落脚点")]
    [Range(0.1f, 10.0f)]
    public float spot;

    PlayerHide player;

    #region Editor变量

    [Range(0.1f, 10.0f)]
	public float upEdge;
	[Range(0.1f, 10.0f)]
	public float bottomEdge;
	[Range(0.1f, 10.0f)]
	public float bottomRange;
	[Range(0.1f, 10.0f)]
	public float forwardRange;

	#endregion

	#region 组件引用

	private SpriteRenderer sr;

	#endregion

	#region 玩家属性参数

	private float moveSpeed = 10.0f;

	private float verticalSpeed = 0;

	#endregion

	#region 对外接口

	public void SetSpeed(float speed)
	{
		this.moveSpeed = speed;
	}

	#endregion

	#region 移动机制

	RaycastHit2D[] bottomHit, upHit;

	private enum Direction
	{
		right,
		left
	};
	Direction direction;

	//玩家当前朝向向量
	Vector3 Forward
	{
		get
		{
			return (direction == Direction.right) ? Vector3.right : Vector3.left;
		}
	}

	//是否检测楼梯
	private bool castLift = false;

    bool castLiftGate = false;

    public GameObject edge2D;
	////计时器相关
	//private float lift_timeVal;

	private void GenRayCast()
	{
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;

		bottomHit = Physics2D.RaycastAll(bottomPos, Vector3.down, bottomRange);
		upHit = Physics2D.RaycastAll(upPos, Forward, forwardRange);
	}

    private bool CheckGround()
    {
        bool flag = false;

        Vector2 pos = new Vector2();

        foreach (var hit in bottomHit)
        {
            switch (hit.collider.tag)
            {
                case "Plane"://============================地面
                    if (flag == false)
                    {
                        pos = new Vector2(hit.point.x, hit.point.y - spot);
                        transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
                        flag = true;
                    }
                    break;
                case "Lift"://==================================楼梯
                    if (castLift && flag == false)
                    {
                        count = 0.1f;
                        pos = new Vector2(hit.point.x, hit.point.y - spot);

                        transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
                        flag = true;
                    }
                    break;
                case "LiftGate"://---------------------------------楼梯门
                    if (!castLift && flag == false)
                    {
                        pos = new Vector2(hit.point.x, hit.point.y - spot);

                        transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
                        flag = true;

                    }
                    break;
            }
        }

        return flag;
    }
    //private bool CheckLift()
    //{
    //    bool flag = false;

    //    Vector2 pos = new Vector2();

    //    foreach (var hitLift in upHit)
    //    {
    //        switch (hitLift.collider.tag)
    //        {
    //            case "Plane"://============================地面
    //                if (flag == false)
    //                {
    //                    pos = new Vector2(hitLift.point.x, hitLift.point.y - spot);
    //                    transform.position = (Vector2)(pos - Vector2.up * (bottomEdge + bottomRange));
    //                    flag = true;
    //                }
    //                break;
    //            case "Lift"://==================================楼梯
    //                if (!castLift && flag == false)
    //                {
    //                    count = 0.1f;
    //                    pos = new Vector2(hitLift.point.x, hitLift.point.y - spot);

    //                    transform.position = (Vector2)(pos - Vector2.up * (bottomEdge + bottomRange));
    //                    flag = true;
    //                }
    //                break;
    //            case "LiftGate"://---------------------------------楼梯门
    //                if (castLift && flag == false)
    //                {
    //                    pos = new Vector2(hitLift.point.x, hitLift.point.y - spot);

    //                    transform.position = (Vector2)(pos - Vector2.up * (bottomEdge + bottomRange));
    //                    flag = true;
    //                }
    //                break;
    //        }
    //    }

    //    return flag;
    //}
    private void Move(Direction direction)
	{
		//移动玩家
		if(direction == Direction.right)
		{
			sr.flipX = false;
				
			transform.position = transform.position + Vector3.right * moveSpeed * Time.fixedDeltaTime;
		}
		else
		{
			sr.flipX = true;
			
			transform.position = transform.position + Vector3.left * moveSpeed * Time.fixedDeltaTime;
		}
		//移动
	}
	private void ApplyGravity()
	{
		float g = 9.8f;
		verticalSpeed += g * Time.fixedDeltaTime;

		Vector3 pos = transform.position;
		pos.y -= verticalSpeed * Time.fixedDeltaTime;
		transform.position = pos;

	}

	#endregion

	#region 生命周期

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
        player = this.GetComponent<PlayerHide>();
	}

    //---------

    float count = 0;
    //bool isStandLift = false;
	void Update()
	{
        count -= Time.deltaTime;
        if (count < 0)
        {
            count = 0;
            castLift = false;
            edge2D.GetComponent<EdgeCollider2D>().isTrigger = false;
            //castLiftGate = false;
        }
		Debug.Log(castLift);
		Debug.Log(castLiftGate);

    }

	void FixedUpdate()
	{
		//发射射线
		GenRayCast();
        ////检查地面
        //if (CheckLift())
        //{
        //    verticalSpeed = 0;
        //}
        //检查电梯
        if (CheckGround())
		{
			verticalSpeed = 0;
		}
		//下落
		else
		{
			ApplyGravity();
		}
        
        playerMove();
	}

	private void OnDrawGizmos()
	{
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;
		Gizmos.color = Color.red;

		Gizmos.DrawLine(bottomPos, bottomPos + Vector3.down * bottomRange);
		Gizmos.DrawLine(upPos, upPos + Forward * forwardRange);
	}

    #endregion


    private void playerMove()
	{
		float xDelta = Input.GetAxis("Horizontal");
		if(xDelta > 0)
		{
            player.isHide = false;
			Move(Direction.right);
		}
		else if(xDelta < 0)
		{
            player.isHide = false;
			Move(Direction.left);
		}

        if (Input.GetKey(KeyCode.W))
        {
            count = 0.1f;
            castLift = true;
            castLiftGate = false;
        }
		if (Input.GetKey(KeyCode.S))
		{
            count = 0.1f;
			//castLiftGate = true;
   //         castLift = false;
            edge2D.GetComponent<EdgeCollider2D>().isTrigger = true;
        }

	}

}