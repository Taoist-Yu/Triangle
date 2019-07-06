using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	#region Editor变量

	[Header("射线检测相关变量")]
	[Range(0.1f, 10.0f)]
	public float upEdge;
	[Range(0.1f, 10.0f)]
	public float bottomEdge;
	[Range(0.1f, 10.0f)]
	public float bottomRange;
	[Range(0.1f, 10.0f)]
	public float forwardRange;

	[Header("脚的局部坐标")]
	public Vector2 footPos;

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

	//碰撞射线检测结果存储
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

	//发射碰撞射线
	private void GenRayCast()
	{
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;

		bottomHit = Physics2D.RaycastAll(bottomPos, Vector3.down, bottomRange);
		upHit = Physics2D.RaycastAll(upPos, Forward, forwardRange);
	}

	//检查角色是否位于地面
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
						transform.position = pos - footPos;
						flag = true;
					}
					break;
				case "Lift":
					if( castLift && flag == false)
					{
						pos = hit.point;
						transform.position = pos - footPos;
						flag = true;
					}
					break;
				case "Gate":
					if (!castLift && flag == false)
					{
						pos = hit.point;
						transform.position = pos - footPos;
						flag = true;
					}
					break;
			}
		}

		return flag;
	}

	//移动角色(同时计算撞墙)
	private void Move(Direction direction)
	{
		this.direction = direction;
		//移动敌人
		if (direction == Direction.right)
		{
			sr.flipX = false;

			//检测墙壁
			if (upHit.Length != 0)
			{
				Vector3 pos = Vector3.zero;
				foreach (RaycastHit2D hit in upHit)
				{
					if (hit.collider.tag == "Wall")
					{
						//有墙壁，终止移动
						return;
					}
				}
			}

			transform.position = transform.position + Vector3.right * moveSpeed * Time.fixedDeltaTime;
		}
		else
		{
			sr.flipX = true;

			//检测墙壁
			if (upHit.Length != 0)
			{
				Vector3 pos = Vector3.zero;
				foreach (RaycastHit2D hit in upHit)
				{
					if (hit.collider.tag == "Wall")
					{
						//有墙壁，终止移动
						return;
					}
				}
			}

			transform.position = transform.position + Vector3.left * moveSpeed * Time.fixedDeltaTime;
		}



	}

	//应用重力
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
		AIStart();
	}

	void Update()
	{
		AIUpdate();


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

//		TestMove();
	}

	private void OnDrawGizmos()
	{
		Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
		Vector3 upPos = new Vector3(0, upEdge) + transform.position;
		Gizmos.color = Color.red;

		Gizmos.DrawLine(bottomPos, bottomPos + Vector3.down * bottomRange);
		Gizmos.DrawLine(upPos, upPos + Forward * forwardRange);

		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(footPos + (Vector2)transform.position, 0.1f);

	}

	#endregion

	#region AI控制器

	//状态
	private Vector2 ai_target;
	

	private void AIStart()
	{
		StartCoroutine(FindPlayer());
	}

	private void AIUpdate()
	{
		//判断玩家和怪物是否在同一个楼层
		if(Mathf.Abs(ai_target.y - transform.position.y) < 0.5f)		//同楼层
		{
			castLift = false;
			if(ai_target.x > transform.position.x)
			{
				Move(Direction.right);
			}
			else
			{
				Move(Direction.left);
			}
		}
		else														//不同楼层
		{
			castLift = true;
			if(transform.position.x > 0)
			{
				Move(Direction.right);
			}
			else
			{
				Move(Direction.left);
			}
		}

	}

	//协程
	IEnumerator FindPlayer()
	{
		while (true)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if(player != null)
			{
				ai_target = player.transform.position;
			}
			yield return new WaitForSeconds(1);
		}
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
