using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {
	
	public float SFX_VOLUME = 1.0f;
	
	Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Start () {
	
		LoadAllResources();
	}
	
	void OnEnable()
    {
        Messenger<string>.AddListener("OnPlaySFX", OnPlaySFX);
        Messenger<float>.AddListener("OnSetVolume", OnSetVolume);
    }

    void OnDisable()
    {
        Messenger<string>.RemoveListener("OnPlaySFX", OnPlaySFX);
        Messenger<float>.RemoveListener("OnSetVolume", OnSetVolume);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LoadAllResources()
	{
		audioClips.Add("Click", Resources.Load("Audio/Button", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Beeps", Resources.Load("Audio/Beeps", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Thunk", Resources.Load("Audio/thunk", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Surprise", Resources.Load("Audio/Bubbles/Bubbles 4", typeof(AudioClip)) as AudioClip);
		
		// Bubbles
		audioClips.Add("Bubbles_1", Resources.Load("Audio/Bubbles/Bubbles 1", typeof(AudioClip)) as AudioClip);
		
		// Collisions
		audioClips.Add("Puffer_Collision", Resources.Load("Audio/Collisions/Puffer Hit", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Mine_Collision", Resources.Load("Audio/Collisions/Mine Hit", typeof(AudioClip)) as AudioClip);
		
		// Divers
		audioClips.Add("Diver_1", Resources.Load("Audio/Screams/Scream 1", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Diver_2", Resources.Load("Audio/Screams/Scream 2", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Diver_3", Resources.Load("Audio/Screams/Scream 3", typeof(AudioClip)) as AudioClip);
		
		// Dolphin
		audioClips.Add("Dolphin_Scream", Resources.Load("Audio/Dolphin/Dolphin scream", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Dolphin_Damage", Resources.Load("Audio/Dolphin/Dolphin Scream 5", typeof(AudioClip)) as AudioClip);
		
		// Explosions
        audioClips.Add("Explosion_4", Resources.Load("Audio/Explosions/Explosion 4", typeof(AudioClip)) as AudioClip);
		audioClips.Add("Puffer", Resources.Load("Audio/Puffers/Puffer 1", typeof(AudioClip)) as AudioClip);

        //Intro Music
        audioClips.Add("OpeningMusic", Resources.Load("Audio/Opening Screen", typeof(AudioClip)) as AudioClip);

        //Enemies
        audioClips.Add("Shock2", Resources.Load("Audio/Jellyfish/Shock 2", typeof(AudioClip)) as AudioClip);
	}
	
	void OnPlaySFX(string sfx)
	{
		var audioSource = GetComponent<AudioSource>();
		audioSource.PlayOneShot(audioClips[sfx], SFX_VOLUME);
		
		if (audioClips[sfx] == null)
		{
			//Debugging.Instance.ShowText("Audio clip " + sfx + " was not found");
		}
	}
	
	void OnSetVolume(float volume)
	{
		var audioSource = GetComponent<AudioSource>();
		audioSource.volume = volume;
	}
}
