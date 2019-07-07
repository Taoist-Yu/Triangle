using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

	private float moveSpeed = 5;

	private float verticalSpeed = 0;

	private bool isEnemyEnable = false;

	#endregion

	#region 对外接口

	/// <summary>
	/// 设置速度与心跳声
	/// </summary>
	/// <param name="speed"></param>移动速度
	/// <param name="beatLevel"></param>心跳声速等级(0,1,2)
	public void SetSpeed(float speed, int beatLevel)
	{
		if (isPlayerDeath)
			return;

		this.moveSpeed = speed;

		audioSource.clip = beat_clips[beatLevel];
		
		if(!audioSource.isPlaying)
			audioSource.Play();

	}

	public void EnableEnemy()
	{
		isEnemyEnable = true;
		transform.position = new Vector3(35, -9, 0);
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
	Direction direction = Direction.left;

	//玩家当前朝向向量
	Vector3 Forward
	{
		get
		{
			return (direction == Direction.right) ? Vector3.right : Vector3.left;
		}
	}

	//上一帧是否是检测到楼梯
	private bool lastCastLift = false;

	//是否检测楼梯
	private bool castLift = false;

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
						lastCastLift = false;
					}
					break;
				case "Lift":
					if( castLift && flag == false)
					{
						pos = hit.point;
						transform.position = pos - footPos;
						flag = true;
						//若敌人在一楼，特判
						if(!lastCastLift && transform.position.y < 0 && Mathf.Abs(transform.position.x) < 10)
						{
							//if (direction == Direction.left)
							//{
							//	direction = Direction.right;
							//	sr.flipX = false;
							//}
							//else
							//{
							//	sr.flipX = true;
							//	direction = Direction.left;
							//}

							if(transform.position.y < -5)
							{
								sr.flipX = true;
								direction = Direction.left;
							}
							else
							{
								direction = Direction.right;
								sr.flipX = false;
							}
								
						}
						lastCastLift = true;
					}
					break;
				case "Gate":
					if (!castLift && flag == false)
					{
						pos = hit.point;
						transform.position = pos - footPos;
						flag = true;
						lastCastLift = false;
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
						//有墙壁,反向,更新爬梯状态
						sr.flipX = true;
						this.direction = Direction.left;
						castLift = !castLift;
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
						//有墙壁，终止移动,更新爬梯状态
						sr.flipX = false;
						this.direction = Direction.right;
						castLift = !castLift;
						return;
					}
				}
			}

			transform.position = transform.position + Vector3.left * moveSpeed * Time.fixedDeltaTime;
		}

	}

	//向前移动
	private void Move()
	{
		Move(this.direction);
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
		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		AIStart();
		cry_clip = cry_clips[Random.Range(0, 2)]; ;
		audioSource.clip = beat_clips[0];
		audioSource.Play();
	}

	void Update()
	{
		if (!isEnemyEnable)
			return;
		AIUpdate();
	}

	void FixedUpdate()
	{
		if (!isEnemyEnable)
			return;

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

		//AI
		Gizmos.color = Color.green;
		foreach (Vector3 pos in lifts)
		{
			Gizmos.DrawCube(pos, new Vector3(0.5f, 0.5f, 0.5f));
		}
		Gizmos.color = Color.red;
		foreach (Vector3 pos in gates)
		{
			Gizmos.DrawCube(pos, new Vector3(0.5f, 0.5f, 0.5f));
		}

	}

	#endregion

	#region AI控制器

	//引用
	[Header("AI控制器对场景物品的引用")]
	public GameObject[] lamps;
	public Vector3[] lifts;
	public Vector3[] gates;

	//状态
	private Vector2 ai_target;

	private void AIStart()
	{
		StartCoroutine(FindPlayer());
	}

	private void AIUpdate()
	{
		Move();

		//检测前方是否有激活灯和玩家
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Forward, 5.0f);
		bool flag = false;
		foreach(var hit in hits)
		{
			if (hit.transform.CompareTag("Organ"))
			{
				Lamp lamp = hit.transform.GetComponent<Lamp>();
				if (lamp != null)
				{
					if(lamp.isLighting == true)
					{
						flag = true;
					}
				}
			}
			else if(hit.transform.CompareTag("Player"))
			{
				if (!hit.transform.GetComponent<PlayerHide>().isHide)
				{
					preKillPlayer(hit.transform.gameObject);
				}
				
			}
		}
		if (flag)
		{
			if (direction == Direction.left)
				direction = Direction.right;
			else
				direction = Direction.left;
		}


	}

	//根据Y坐标获取楼层
	private int GetFloor(float y)
	{
		int floor = 1;

		if (y > -4)
			floor = 2;
		if (y > 4)
			floor = 3;

		return floor;
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
	
	private void preKillPlayer(GameObject player)
	{
		Sequence s = DOTween.Sequence();

		audioSource.clip = cry_clip;
		if (!isPlayerDeath)
		{
			audioSource.Play();
			audioSource.loop = false;
			isPlayerDeath = true;
		}
		

		s.Append(transform.DOMove(player.transform.position, 0.5f));
		SpriteRenderer _sr = GameObject.FindGameObjectWithTag("2DMask").GetComponent<SpriteRenderer>();
		s.Append(_sr.
			DOColor(new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1), 0.3f));


		StartCoroutine(KillPlayer(player));
	}

	IEnumerator KillPlayer(GameObject player)
	{
		yield return new WaitForSeconds(0.3f);

		player.transform.Find("PlayerLight").GetComponent<PlayerLight>().LightDown();

		yield return new WaitForSeconds(1);

		//加载新场景

	}

	#endregion

	#region 音效支持

	AudioSource audioSource;
	public AudioClip[] beat_clips;
	public AudioClip[] cry_clips;
	public AudioClip cry_clip;

	bool isPlayerDeath = false;

	#endregion

}
