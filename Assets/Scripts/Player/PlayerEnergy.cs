using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class PlayerEnergy : MonoBehaviour
{
    PlayerMove playerMove = null;
    Enemy enemy = null;
    PlayerLight playerLight = null;
    //预制体获取
    GameObject[] Energy;
    // Start is called before the first frame update
    void Start()
    {
        //找到玩家的能量
        Energy = GameObject.FindGameObjectsWithTag("PlayerEnergy");
		for(int i = 0; i < Energy.Length; i++)
		{
			for(int j = i; j < Energy.Length; j++)
			{
				if(Energy[i].transform.position.x > Energy[j].transform.position.x)
				{
					GameObject temp = Energy[i];
					Energy[i] = Energy[j];
					Energy[j] = temp;
				}
			}
		}


        playerLight = GameObject.Find("PlayerLight").GetComponent<PlayerLight>();
        playerMove = this.transform.GetComponent<PlayerMove>();

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
		//GameObject.FindGameObjectWithTag

		this.playDeathTime = timeAll;

    }

	[Header("玩家拥有的总时间")]
	public float timeAll = 100;
    float playDeathTime;
    //玩家死亡时间
    float deathTime = 0;
    //float playDeathTime = 100;
    void Update()
    {
        deathTime += Time.deltaTime;
        if (deathTime >= 1)
        {
            deathTime = 1;
            playDeathTime -= deathTime;
            deathTime = 0;
        }
        //print("energy" + playDeathTime);
        playerEnergy();
    }



    #region 玩家能量值

	public void Reply()
	{
		playDeathTime = timeAll;
		float flag = 10 - playDeathTime / 10.0f;
		for (int i = Energy.Length - 1; i >= 0; i--)
		{
			Energy[i].SetActive(true);
		}
	}

    void playerEnergy()
    {
        float flag = 0;
        if (playDeathTime <= 0)
        {
            playerLight.LightDown();
            foreach (var go in Energy)
            {
                go.SetActive(false);
            }
			StartCoroutine(PlayerDied());
            playDeathTime = 0;
        }

		//Update Energy UI
		flag = 10 - playDeathTime / 10.0f;
		for (int i = Energy.Length - 1; i > Energy.Length - flag; i--)
		{
			Energy[i].SetActive(false);
		}

		if (playDeathTime >= 60)
        {
			enemy.SetSpeed(playerMove.moveSpeed * 1.0f, 0);
            playerLight.HighLevel();
        }
        if (playDeathTime < 60 && playDeathTime >= 20)
        {
			enemy.SetSpeed(playerMove.moveSpeed * 1.5f, 1);
            playerLight.NormalLevel();

        }
        if (playDeathTime < 20)
        {
			enemy.SetSpeed(playerMove.moveSpeed * 2.0f, 2);
            playerLight.LowLevel();
        }
    }

	public IEnumerator PlayerDied()
	{
		GetComponent<PlayerMove>().enabled = false;
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("DiedScene");
	}

    #endregion
}
