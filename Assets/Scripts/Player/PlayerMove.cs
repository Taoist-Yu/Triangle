using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;//==================DOtween命名空间

public class PlayerMove : MonoBehaviour
{
    #region 初始化

    [Header("落脚点")]
    [Range(0.1f, 10.0f)]
    public float spot;

    PlayerHide player;
    #endregion

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
    [HideInInspector]
    public float moveSpeed = 5.0f;

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

    public enum Direction
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


    ////计时器相关
    //private float lift_timeVal;

    private void GenRayCast()
    {
        Vector3 bottomPos = new Vector3(0, -bottomEdge) + transform.position;
        Vector3 upPos = new Vector3(0, upEdge) + transform.position;

        bottomHit = Physics2D.RaycastAll(bottomPos, Vector3.down, bottomRange);
        upHit = Physics2D.RaycastAll(upPos, Forward, forwardRange);
    }

    //是否检测楼梯
    private bool castLift = false;

    bool castLiftGate = true;
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
                        pos = new Vector2(hit.point.x, hit.point.y + spot);
                        transform.position = (Vector2)(pos);
                        flag = true;
                    }
                    break;
                case "Lift"://==================================楼梯
                    if (castLift && flag == false)
                    {
                        count = 0.1f;
                        pos = new Vector2(hit.point.x, hit.point.y + spot);

                        transform.position = (Vector2)(pos);
                        flag = true;
                    }
                    break;
                case "Gate"://---------------------------------楼梯门
                    if (castLiftGate && flag == false)
                    {
                        pos = new Vector2(hit.point.x, hit.point.y + spot);

						castLift = true;
						count = 0.1f;

                        transform.position = (Vector2)(pos);
                        flag = true;

                    }
                    break;
            }
        }

        return flag;
    }
    private void Move(Direction direction)
    {
		this.direction = direction;
        //移动玩家
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
						return;
					}
				}
			}
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
    GameObject organ;
    Animator animator;
    void Start()
    {
        //organ = GameObject.FindWithTag("Organ");
        player = this.GetComponent<PlayerHide>();

        animator = this.transform.GetComponent<Animator>();
    }

    //---------

    float count = 0;
    float castLiftCount = 0;

    void Update()
    {
        count -= Time.deltaTime;
        castLiftCount -= Time.deltaTime;
        if (count < 0)
        {
            count = 0;
            castLift = false;
        }
        if (castLiftCount < 0)
        {
            castLiftCount = 0;
            castLiftGate = true;
        }

		//玩家尝试治愈自己
		if (this.checkClip())
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.useClip();
				GetComponent<PlayerEnergy>().Reply();
				GetComponent<PlayerAudio>().PlayHeal();
			}

		}

    }

    void FixedUpdate()
    {
        //发射射线
        GenRayCast();
        if (CheckGround())
        {
			playerMove();
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
		Gizmos.DrawSphere(transform.position + Vector3.down * spot, 0.1f);

	}

    #endregion

    #region 玩家移动

    private void playerMove()
    {
		PlayerAudio playerAudio = GetComponent<PlayerAudio>();


        float xDelta = Input.GetAxis("Horizontal");
        if (xDelta > 0)
        {
            Move(Direction.right);
			playerAudio.PlayWalk();
            animator.SetBool("ToRunAnim", true);
        }
        else if (xDelta < 0)
        {
            Move(Direction.left);
			playerAudio.PlayWalk();
            animator.SetBool("ToRunAnim", true);
        }
        else
        {
            animator.SetTrigger("Run2Stand");
            animator.SetBool("ToRunAnim", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            count = 0.1f;
            castLift = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            castLiftCount = 0.1f;
            castLiftGate = false;

            count = 0.1f;
            castLift = true;
        }

    }
	#endregion

	#region 碎片的增删查

	GameObject clip;
    bool isClip = false;
    public void addClip(GameObject clip)
    {
		if(this.clip != null)
		{
			Destroy(this.clip);
			this.clip = null;
		}
		this.clip = clip;
        if (isClip == false)
        {
            isClip = true;
        }
    }
    public bool checkClip()
    {
        if (isClip == true)
        {
            return true;
        }
        return false;
    }
    public void useClip()
    {
        if (checkClip() == true)
        {
            isClip = false;
			Destroy(clip);
			clip = null;
        }
    }




    #endregion

    #region 玩家和道具的交互

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Organ")
        {
            organ = other.GetComponent<Organ>().gameObject;
            if (Input.GetKeyDown(KeyCode.E))
            {
                organ.GetComponent<Organ>().OnUse(this.gameObject);
            }
        }
    }
    #endregion

}
