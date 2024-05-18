using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	[Header("Audio Settings")]
	public List<AudioClip> clips;
	[Range(0f, 1f)]
	public float masterVolume = 1f;
	AudioSource source;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
		
		source = GetComponent<AudioSource>();
		if (source == null)
		{
			source = gameObject.AddComponent<AudioSource>();
		}
	}

	public void Play(int idx)
	{
		if (IsValidIndex(idx))
		{
			source.PlayOneShot(clips[idx], masterVolume);
		}
	}

	public void Play(int idx, float vol)
	{
		if (IsValidIndex(idx))
		{
			source.PlayOneShot(clips[idx], vol * masterVolume);
		}
	}

	public void Play(AudioClip clip)
	{
		if (clip != null)
		{
			source.PlayOneShot(clip, masterVolume);
		}
		else
		{
			Debug.LogWarning("AudioManager: Provided AudioClip is null.");
		}
	}

	public void Play(AudioClip clip, float vol)
	{
		if (clip != null)
		{
			source.PlayOneShot(clip, vol * masterVolume);
		}
		else
		{
			Debug.LogWarning("AudioManager: Provided AudioClip is null.");
		}
	}

	bool IsValidIndex(int idx)
	{
		if (idx < 0 || idx >= clips.Count)
		{
			Debug.LogWarning($"AudioManager: Invalid clip index: {idx}");
			return false;
		}
		return true;
	}

	public void SetVolume(float volume)
	{
		masterVolume = volume < 0 ? 0 : volume;
	}
}
