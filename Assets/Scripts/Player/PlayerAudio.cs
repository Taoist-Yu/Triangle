using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
	public AudioClip step;
	public AudioClip stair;
	public AudioClip healing;
	public AudioClip lightingUp;

	private AudioSource audioSource;

	float isPlaying_timeVal = 0;

	//走路声音
	public void PlayWalk()
	{
		isPlaying_timeVal = 0.1f;
	}

	//治疗声音
	public void PlayHeal()
	{
		AudioPlayer.Instance.PlayClip(healing);
	}

	//点灯声音
	public void PlayLightup()
	{
		AudioPlayer.Instance.PlayClip(lightingUp);
	}

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = stair;
	}

	void Update()
	{
		if (isPlaying_timeVal > 0)
		{
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		else
		{
			audioSource.Stop();
		}
		isPlaying_timeVal -= Time.deltaTime;
	}


}
