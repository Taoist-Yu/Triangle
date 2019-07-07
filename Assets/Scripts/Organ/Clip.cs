using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Clip : Organ
{
	GameObject player = null;
	Vector3 targetPos;

	[Header("碎片可存在的总时间")]
	public float timeAll = 3;

	float timeVal;		//生命计时器

	public override void OnUse(GameObject player)
	{
		this.player = player;
		player.GetComponent<PlayerMove>().addClip(gameObject);


		transform.DOScale(0.4f, 1.0f);

		//重置计时器
		timeVal = timeAll;
		
	}

	public void Heal(GameObject player)
	{

	}

	void Start()
	{
		timeVal = timeAll;
	}

	void Update()
	{
		//跟随玩家状态
		if(player != null)
		{
			targetPos = player.transform.position + Vector3.up;
			bool flip = player.GetComponent<SpriteRenderer>().flipX;
			targetPos += (flip ? Vector3.right : Vector3.left) * 3f;

			targetPos.y += 0.5f * Mathf.Sin(Time.time);

			transform.DOMove(targetPos, 0.5f);
		}


		//计时相关
		timeVal -= Time.deltaTime;
		if(timeVal < 0)
		{
			timeVal = 0;
			Destroy(gameObject);
		}


	}

	void OnDestroy()
	{
		if(player != null)
		{
			player.GetComponent<PlayerMove>().useClip();
		}
	}

}
