using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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

	#region 敌人属性参数

	private float moveSpeed = 3;

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
	private bool castLift;

	//计时器相关
	private float lift_timeVal;

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

		foreach(RaycastHit2D hit in bottomHit)
		{
			switch (hit.collider.tag)
			{
				case "Plane":
					if(flag == false)
					{
						pos = hit.point;
						transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
						flag = true;
					}
					break;
				case "Lift":
					if( castLift && flag == false)
					{
						pos = hit.point;
						transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
						flag = true;
					}
					break;
				case "LiftGate":
					if (!castLift && flag == false)
					{
						pos = hit.point;
						transform.position = (Vector2)(pos + Vector2.up * (bottomEdge + bottomRange));
						flag = true;
					}
					break;
			}
		}

		return flag;
	}

	private void Move(Direction direction)
	{
		//移动敌人
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

	}

	void Update()
	{

		Debug.Log(castLift);

	}

	void FixedUpdate()
	{
		//发射射线
		GenRayCast();

		//检查地面
		if (CheckGround())
		{
			verticalSpeed = 0;
		}
		//下落
		else
		{
			ApplyGravity();
		}

		TestMove();
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

	private void TestMove()
	{
		float h = Input.GetAxis("Horizontal");
		if(h > 0)
		{
			Move(Direction.right);
		}
		else if(h < 0)
		{
			Move(Direction.left);
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			castLift = true;
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			castLift = false;
		}

	}

}
