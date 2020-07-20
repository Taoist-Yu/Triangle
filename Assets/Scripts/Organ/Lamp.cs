using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Lamp : Organ
{

	#region Editor

	[Header("亮灯持续时长")]
	public float duration = 30.0f;

	#endregion 


	GameObject m_light;
	//是否是亮着的状态
	public bool isLighting { get; private set; }
	private float timeVal = 0.0f;       //计时器，从灯亮起开始计时
	private float lightScale = 0;

	public override void OnUse(GameObject player)
	{
		if (this.CheckClip(player))
		{
			player.GetComponent<PlayerAudio>().PlayLightup();
			player.GetComponent<PlayerMove>().useClip();
			EnableLight();
		}
	}

	void Awake()
	{
		m_light = transform.Find("Light").gameObject;
			
	}

	void Start()
	{
		isLighting = false;
	}

	void Update()
	{
		timeVal -= Time.deltaTime;
		if(timeVal < 0)
		{
			DisableLight();			//包含了timeVal = 0
		}
		
	}

	void LateUpdate()
	{
		m_light.GetComponent<Light>().intensity *= lightScale;
	}

	//检查玩家是否有碎片,如果有，消耗掉
	private bool CheckClip(GameObject player)
	{
		bool flag = false;
		if (player.GetComponent<PlayerMove>().checkClip())
		{
			flag = true;
			player.GetComponent<PlayerMove>().useClip();
			EnableLight();
		}
		return flag;
	}

	//开启光照
	public void EnableLight()
	{
		DOTween.To(() => lightScale, x => lightScale = x, 1.0f, 0.3f);
		isLighting = true;
		timeVal = duration;
	}

	//关闭光照
	public void DisableLight()
	{
		DOTween.To(() => lightScale, x => lightScale = x, 0.0f, 0.3f);
		isLighting = false;
		timeVal = 0;
	}


}
