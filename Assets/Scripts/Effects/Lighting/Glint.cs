using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glint : MonoBehaviour
{

	Animator animator;
	Light m_light;

	public bool EnableGlint = true;

	[Serializable]
	public struct Scale_Range
	{
		public float min;
		public float max;
	}
	[Header("光照强度Scale范围")]
	public Scale_Range itensity_Range;
	[Header("光照角Scale范围")]
	public Scale_Range angle_Range;

	[Header("闪烁速度")]
	public float glint_speed;


	private float intensity;
	private float angle;


	//动画变量
	[HideInInspector]
	public float glint_intensity;

	void Awake()
	{
		animator = GetComponent<Animator>();
		m_light = GetComponent<Light>();
	}

	void Start()
	{
		this.intensity = m_light.intensity;
		this.angle = m_light.spotAngle;
		
	}

    // Update is called once per frame
    void Update()
    {
		if (EnableGlint)
		{
			animator.speed = glint_speed;
			float itensity_scale = glint_intensity * (itensity_Range.max - itensity_Range.min) + itensity_Range.min;
			float angle_scale = glint_intensity * (angle_Range.max - angle_Range.min) + angle_Range.min;
			switch (m_light.type)
			{
				case LightType.Point:
					m_light.intensity = itensity_scale * this.intensity;
					break;
				case LightType.Spot:
					m_light.intensity = itensity_scale * this.intensity;
					m_light.spotAngle = angle_scale * this.angle;
					break;
			}
		}
		else
		{
			m_light.intensity = this.intensity;
			m_light.spotAngle = this.angle;
		}

    }
}
