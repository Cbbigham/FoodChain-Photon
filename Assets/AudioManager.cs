using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource source;
	public AudioSource ambiance;

	public AudioClip button;
	public AudioClip attack;

	public AudioClip death;
	public AudioClip timeLow;

	public AudioClip[] ambianceTracks;

	public static AudioManager instance;

	void Start ()
	{
		instance = this;
	}
	
	void Update () {
	
	}

	public void ChangeAmbianceTrack(int newTrack)
	{
		ambiance.Stop ();
		ambiance.clip = ambianceTracks [newTrack];
		ambiance.Play ();

	}

	public void PlayButtonSound()
	{
		source.PlayOneShot (button);
	}
	public void PlayAtackSound()
	{
		source.PlayOneShot (attack);
	}
	public void PlayTimeLowSound()
	{
		source.PlayOneShot (timeLow);
	
	}
	public void PlayDeathSound()
	{
		source.PlayOneShot (death);
	}
}
