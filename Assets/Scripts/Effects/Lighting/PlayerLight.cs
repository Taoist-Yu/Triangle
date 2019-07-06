using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerLight : MonoBehaviour
{

	GameObject light_spot, light_point;

	float timeVal;

	private float lightScale = 1;

	void Awake()
	{
		light_spot = transform.Find("Spot Light").gameObject;
		light_point = transform.Find("Point Light").gameObject;
	}

    public void HighLevel()
	{
		light_point.SetActive(true);
		light_point.GetComponent<Glint>().itensityScale = 1.0f;
		light_point.GetComponent<Glint>().angleScale = 1.0f;
		light_point.GetComponent<Glint>().glint_speed = 1;

		light_spot.GetComponent<Glint>().EnableGlint = false;
		light_spot.GetComponent<Glint>().itensityScale = 1.0f;
		light_spot.GetComponent<Glint>().angleScale = 1.0f;
	}

	public void NormalLevel()
	{
		light_point.SetActive(true);
		light_point.GetComponent<Glint>().itensityScale = 0.7f;
		light_point.GetComponent<Glint>().angleScale = 0.7f;
		light_point.GetComponent<Glint>().glint_speed = 1;


		light_spot.GetComponent<Glint>().EnableGlint = false;
		light_spot.GetComponent<Glint>().itensityScale = 0.7f;
		light_spot.GetComponent<Glint>().angleScale = 0.7f;
	}

	public void LowLevel()
	{
		light_point.SetActive(true);
		light_point.GetComponent<Glint>().itensityScale = 0.5f;
		light_point.GetComponent<Glint>().angleScale = 0.5f;
		light_point.GetComponent<Glint>().glint_speed = 2;

		light_spot.GetComponent<Glint>().EnableGlint = true;
		light_spot.GetComponent<Glint>().itensityScale = 0.7f;
		light_spot.GetComponent<Glint>().angleScale = 0.7f;
	}

	//开启玩家灯
	public void LightUp()
	{
		Tween tween;
		

	}

	//熄灭玩家灯
	public void LightDown()
	{
		DOTween.To(() => lightScale, x => lightScale = x, 0.5f,1);
	}

	void Update()
	{
		//判断当前是否为低光照
		if(light_spot.GetComponent<Glint>().EnableGlint == true)
		{
			timeVal += Time.deltaTime;
			if(timeVal < 1.5f)
			{
				light_point.GetComponent<Glint>().itensityScale = 0.5f;
				light_point.GetComponent<Glint>().angleScale = 0.5f;
				light_spot.GetComponent<Glint>().itensityScale = 0.7f;
				light_spot.GetComponent<Glint>().angleScale = 0.7f;
			}
			else
			{
				light_point.GetComponent<Glint>().itensityScale = 0f;
				light_point.GetComponent<Glint>().angleScale = 0f;
				light_spot.GetComponent<Glint>().itensityScale = 0f;
				light_spot.GetComponent<Glint>().angleScale = 0f;
			}
			if(timeVal > 2.0f)
			{
				timeVal = 0;
			}
		}
		else
		{
			timeVal = 0;
		}

		//Apply Scale
		light_point.GetComponent<Glint>().itensityScale *= lightScale;
		light_spot.GetComponent<Glint>().itensityScale *= lightScale;


		//test
		if (Input.GetKeyDown(KeyCode.J))
		{
			this.HighLevel();
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			this.NormalLevel();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			this.LowLevel();
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.LightUp();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this.LightDown();
		}


	}


}
