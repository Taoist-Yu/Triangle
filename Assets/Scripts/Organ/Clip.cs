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
	}

	void Update()
	{
		targetPos = player.transform.position;
		transform.DOMove(targetPos + 3 * Vector3.up, 1.0f);
	}

}
