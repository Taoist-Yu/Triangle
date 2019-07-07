using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

	#region 静态
	private static  AudioPlayer instance = null;
	public static AudioPlayer Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("AudioPlayer").AddComponent<AudioPlayer>();
				GameObject.DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}
	#endregion

	AudioSource audioSource;

	void Awake()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
	}

    public void PlayClip(AudioClip clip)
	{
		audioSource.clip = clip;
		if (!audioSource.isPlaying)
			audioSource.Play();
	}
}
