using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lemp : Organ
{

	#region Editor

	[Header("亮灯持续时长")]
	public float duration = 30.0f;

	#endregion 


	GameObject m_light;
	//是否是亮着的状态
	public bool isLighting { get; private set; }
	private float timeVal = 0.0f;		//计时器，从灯亮起开始计时

	public override void OnUse(GameObject player)
	{
		if (this.CheckClip(player))
		{
			EnableLight();
		}
	}

	void Awake()
	{
		m_light = transform.Find("Light").gameObject;
		m_light.SetActive(false);	
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

	//检查玩家是否有碎片,如果有，消耗掉
	private bool CheckClip(GameObject player)
	{
		bool flag = true;
		return true;
	}

	//开启光照
	public void EnableLight()
	{
		m_light.SetActive(true);
		isLighting = true;
		timeVal = duration;
	}

	//关闭光照
	public void DisableLight()
	{
		m_light.SetActive(false);
		isLighting = false;
		timeVal = 0;
	}


}
