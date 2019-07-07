using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
	[Header("Y方向相对玩家的缩放")]
	public float scale_Y = 1.0f;


	private Vector3 targetPos;
	private GameObject player;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

    // Update is called once per frame
    void Update()
    {
		targetPos.x = player.transform.position.x;
		targetPos.z = transform.position.z;
		targetPos.y = player.transform.position.y * scale_Y;

		transform.DOMove(targetPos, 1.0f);

    }
}
