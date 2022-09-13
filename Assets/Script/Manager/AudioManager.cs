using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Script.Manager
{
	public class AudioManager : MonoBehaviour
	{
		//public static AudioManager instance;
		private static AudioManager _instance;

		public static AudioManager Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<AudioManager>();
				}

				return _instance;
			}
		}

		public AudioMixerGroup mixerGroup;

		public Sound[] sounds;

		public void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
			foreach (Sound s in sounds)
			{
				s.source = gameObject.AddComponent<AudioSource>();
				s.source.clip = s.clip;
				s.source.loop = s.loop;
				s.source.spatialBlend = s.spatialBlend;
				s.source.rolloffMode = s.rolloffMode;
				s.source.maxDistance = s.maxDistance;

				s.source.outputAudioMixerGroup = mixerGroup;
			}
		}

		public void Play(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}

			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			s.source.Play();
		}

		public void Stop(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			if (s == null)
			{
				Debug.LogWarning("Sound: " + name + " not found!");
				return;
			}
			s.source.Stop();
		}

	}
}
