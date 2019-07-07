using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        playerLight = GameObject.Find("PlayerLight").GetComponent<PlayerLight>();
        playerMove = this.transform.GetComponent<PlayerMove>();

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        //GameObject.FindGameObjectWithTag

    }
    public float playDeathTime = 100;
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
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            playDeathTime = 0;
        }
        if (playDeathTime >= 60)
        {
            enemy.SetSpeed(playerMove.moveSpeed * 1.0f, 1);
            //playerLight.HighLevel();
            playerLight.HighLevel();
            flag = 10 - playDeathTime / 10.0f;
            for (int i = Energy.Length - 1; i > Energy.Length - flag; i--)
            {
                Energy[i].SetActive(false);
            }
        }
        if (playDeathTime < 60 && playDeathTime >= 20)
        {
            enemy.SetSpeed(playerMove.moveSpeed * 1.5f, 2);
            playerLight.NormalLevel();
            flag = 10 - playDeathTime / 10.0f;
            for (int i = Energy.Length - 1; i > Energy.Length - flag; i--)
            {
                Energy[i].SetActive(false);
            }

        }
        if (playDeathTime < 20)
        {
            enemy.SetSpeed(playerMove.moveSpeed * 2.0f, 3);
            playerLight.LowLevel();
            flag = 10 - playDeathTime / 10.0f;
            for (int i = Energy.Length - 1; i > Energy.Length - flag; i--)
            {
                Energy[i].SetActive(false);
            }
        }
    }


    #endregion
}
