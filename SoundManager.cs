using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;
	//Highest and lowest pitch for the SFX
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	//Plays single sound audio clips
	public void PlaySingle (AudioClip clip)
	{
		efxSource.clip = clip;
		//Play the selected clip
		efxSource.Play ();
	}

	//Choose a random audio clip and slightly alters the pitch
	public void RandomizeSfx (params AudioClip [] clips)
	{
		int randomIndex = Random.Range (0, clips.Length);
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips [randomIndex];
		//Plays the clip
		efxSource.Play ();
	}
}
