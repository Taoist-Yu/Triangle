using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escape : Organ
{

	[Header("电梯总耗时长")]
	public float timeAll = 120;

	private float timeVal = 0;

	private Text text;

	public enum Status
	{
		close,			//电梯关闭中
		wait,			//等电梯
		open			//已开启
	}
	Status status = Status.close;

	void Awake()
	{
		text = transform.Find("Canvas/Text").GetComponent<Text>();
	}

    public override void OnUse(GameObject player)
    {
		switch (status)
		{
			case Status.close:
				this.BeginWait();
				break;
			case Status.wait:
				break;
			case Status.open:
				PlayerEscape(player);
				break;
		}
    }

	void Update()
	{
		if(status == Status.wait)
		{
			timeVal -= Time.deltaTime;
			if(timeVal < 0)
			{
				timeVal = 0;
				this.Open();
			}

			int floor = (int)((timeVal / timeAll) * 20.0f);
			if(floor >= 10)
			{
				text.text = floor.ToString();
			}
			else
			{
				text.text = "0" + floor.ToString();
			}
		}


	}

	void BeginWait()
	{
		timeVal = timeAll;
		status = Status.wait;


		StartCoroutine(GenEnemy());
	}

	IEnumerator GenEnemy()
	{
		yield return new WaitForSeconds(3.0f);
		GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>().EnableEnemy();
	}

	void Open()
	{
		status = Status.open;
	}

	void PlayerEscape(GameObject player)
	{

	}


}
