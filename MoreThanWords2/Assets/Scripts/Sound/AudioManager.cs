﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;

	public static AudioManager instance;

	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		foreach (Sound s in sounds) 
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;		
		}
			
		Play ("soundTrack"); 
	}

	public void Play (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Play();
	}

	public void Stop (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.Stop();
	}

	//FROM THIS POINT THOSE ARE MY OWN FUNCTIONS, PLEASE NOTICE ME IF YOU CHANGE SOMETHING

	public void ReduceMusicVolume(float time) //only background
	{
		StartCoroutine(TemporalSilence(time));
	}
		
	public void deathSound() //called from AttackCalculate.SetDeathState();
	{
		for (int s = 1; s < sounds.Length; s++) //stop all sounds except background
		{
			Stop(sounds [s].name);
		}
		FindObjectOfType<AudioManager>().Play("kallumDeath");
		ReduceMusicVolume(4.5f);//mute background sound
	}

	private IEnumerator TemporalSilence(float timeToWait) //stops soundtrack for x time and restore it again with a fade in
	{
		Sound backgroundSound = sounds [0];
		float originalVolume = backgroundSound.source.volume;//stores background music volume
		backgroundSound.source.volume = 0;
		yield return new WaitForSeconds (timeToWait);
		while (backgroundSound.source.volume < originalVolume)
		{
			//print("iteration");
			backgroundSound.source.volume += 0.005f;  
			yield return null;
		}
	}

	public void SetPitch(string name, float value) //changes pitch value of a sound
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
			return;
		s.source.pitch = value;
	}

	public void PlaySoundWithRandomPitch(int index) 
	{
		if (index == 0) //sword hit on enemy
		{
			float rdmPitch = UnityEngine.Random.Range(0.85f, 1.22f); //pitch range
			Sound attack1 = Array.Find(sounds, sound => sound.name == "swordImpact");
			attack1.source.pitch = rdmPitch;
			Play("swordImpact");
		}
		else if (index == 1) //sword hit on air
		{		
			float rdmPitch = UnityEngine.Random.Range(0.85f, 1.22f); //pitch range	
			Sound attack2 = Array.Find(sounds, sound => sound.name == "swordAir");
			attack2.source.pitch = rdmPitch;
			Play("swordAir");
		}
		else if (index == 2) //sword hit and parry
		{			
			float rdmPitch = UnityEngine.Random.Range(0.85f, 1f); //pitch range
			Sound attack3 = Array.Find(sounds, sound => sound.name == "swordParry");
			attack3.source.pitch = rdmPitch;
			Play("swordParry");
		}
	}


}
