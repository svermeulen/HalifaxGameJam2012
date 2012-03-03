using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {
	
	public float FADE_TIME = 1.0f;
	
	float target = 1.0f;
	float current = 0.0f;
	
	Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Start () {
		audioClips.Add("Level", Resources.Load("Music/Mission-1", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Boss", Resources.Load("Music/Octoboss Music", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Opening", Resources.Load("Music/Opening Screen", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Victory", Resources.Load("Music/Victory Sting", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Defeat", Resources.Load("Music/You Lose Sting", typeof(AudioClip)) as AudioClip);
	}
	
	void OnEnable()
    {
        Messenger<string>.AddListener("OnPlaySong", OnPlaySong);
		Messenger.AddListener("OnStopSong", OnStopSong);
        Messenger<float>.AddListener("OnFade", OnFade);
    }

    void OnDisable()
    {
        Messenger<string>.RemoveListener("OnPlaySong", OnPlaySong);
        Messenger<float>.RemoveListener("OnFade", OnFade);
		Messenger.RemoveListener("OnStop", OnStopSong);
    }
	
	// Update is called once per frame
	void Update() 
    {
		if (target != current)
		{
			float sign = Mathf.Sign(target - current);
			float delta = sign * Time.deltaTime * FADE_TIME;
			current += delta;
			current = Mathf.Clamp(current, 0.0f, 1.0f);
			
			AudioSource audioSource = GetComponent<AudioSource>();
			audioSource.volume = current;
			
			//Debugging.Instance.ShowText(current.ToString());
		}
	}
	
	void OnPlaySong(string song)
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip = audioClips[song];
		audioSource.loop = false;
		audioSource.Play();
	}
	
	void OnStopSong()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.Stop();
	}
	
	void OnFade(float fade)
	{
		target = fade;
	}
}
