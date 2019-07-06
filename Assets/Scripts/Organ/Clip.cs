using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Clip : Organ
{
	GameObject player;
	Vector3 targetPos;

	public override void OnUse(GameObject player)
	{
		this.player = player;
		player.GetComponent<PlayerMove>().addClip();

		transform.DOScale(0.4f, 1.0f);
		
	}

	void Update()
	{
		targetPos = player.transform.position + Vector3.up;
		bool flip = player.GetComponent<SpriteRenderer>().flipX;
		targetPos += (flip ? Vector3.right : Vector3.left) * 3f;

		targetPos.y += 0.5f * Mathf.Sin(Time.time);

		transform.DOMove(targetPos, 0.5f);
	}

}
