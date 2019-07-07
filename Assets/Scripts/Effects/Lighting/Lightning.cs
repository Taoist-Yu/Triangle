using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lightning : MonoBehaviour
{
	[Header("闪电时间间隔")]
	public float duration = 10;


	GameObject clip;
	GameObject lightning;

	float lightScale = 0;
	float intensity;

	void Awake()
	{
		lightning = transform.Find("Lightning").gameObject;
	}

    // Start is called before the first frame update
    void Start()
    {
		intensity = lightning.GetComponent<Light>().intensity;
		StartCoroutine(GenLightning());
    }

    IEnumerator GenLightning()
	{
		

		while (true)
		{
			yield return new WaitForSeconds(duration);

			GenClip();
			LightningEffect();
		}
		
	}

	void GenClip()
	{
		float[] floory = new float[4] { 0, -11.2f, -3.2f, 4.8f };
		Vector2 pos = Vector2.zero;
		bool flag = false;
		while (!flag)
		{
			//随机坐标
			int randf = Random.Range(1, 4);
			pos.x = Random.Range(-35, 35);
			pos.y = floory[randf];

			//检查是否与别的机关重合
			flag = true;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 5.0f);
			foreach (var collider in colliders)
			{
				if (collider.CompareTag("Organ"))
				{
					flag = false;
				}
			}
		}

		pos.y += 2.0f;
		Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Clip"), new Vector3(pos.x, pos.y), Quaternion.Euler(0, 0, 0), transform);

	}

	void LightningEffect()
	{
		Sequence s = DOTween.Sequence();

		s.Append(DOTween.To(() => lightScale, x => lightScale = x, 1.0f, 0.05f));
		s.Append(DOTween.To(() => lightScale, x => lightScale = x, 0.3f, 0.1f));
		s.Append(DOTween.To(() => lightScale, x => lightScale = x, 1.0f, 0.05f));
		s.Append(DOTween.To(() => lightScale, x => lightScale = x, 0.0f, 1.0f));

	}

	void LateUpdate()
	{
		lightning.GetComponent<Light>().intensity = intensity * lightScale;
	}


}
