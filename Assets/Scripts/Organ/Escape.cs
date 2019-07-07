using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : Organ
{

	[Header("电梯总耗时长")]
	public float timeAll = 120;

	private float timeVal = 0;

	public enum Status
	{
		close,			//电梯关闭中
		wait,			//等电梯
		open			//已开启
	}
	Status status;

    public override void OnUse(GameObject player)
    {
		//===========
		switch (status)
		{
			case Status.close:
				this.BeginWait();
				break;
			case Status.wait:
				break;
			case Status.open:
				PlayerEscape(player);

                SceneManager.LoadScene("黑屏场景", LoadSceneMode.Single);

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
		}


	}

	void BeginWait()
	{
		timeVal = timeAll;
		status = Status.wait;
	}

	void Open()
	{
		status = Status.open;
	}

	void PlayerEscape(GameObject player)
	{

	}


}
