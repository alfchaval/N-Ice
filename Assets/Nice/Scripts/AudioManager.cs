using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
	private static Dictionary<SoundName, AudioClip> sounds;
    private static Dictionary<LanguageManager.Language, Dictionary<DialogueName, AudioClip>> dialogues;

    private const string audioFolder = "Audio";

	public AudioClip GetAudioClip(SoundName name)
	{
        if (sounds == null)
        {
            sounds = new Dictionary<SoundName, AudioClip>();
        }
		else if (sounds.ContainsKey(name))
		{
			return sounds[name];
		}
		AudioClip clip = LoadAudioClip(name);
		return clip;
	}

	private AudioClip LoadAudioClip(SoundName name)
	{
		AudioClip clip = Resources.Load<AudioClip>(audioFolder + "/" + name);
		if (clip)
		{
			sounds[name] = clip;
		}
		return clip;
	}

	public AudioClip GetAudioClip(DialogueName name)
	{
        LanguageManager.Language audioLanguage = LanguageManager.GetLanguage();
        if (dialogues == null)
        {
            dialogues = new Dictionary<LanguageManager.Language, Dictionary<DialogueName, AudioClip>>();
            dialogues[audioLanguage] = new Dictionary<DialogueName, AudioClip>();
        }
        else if (!dialogues.ContainsKey(audioLanguage))
        {
            dialogues[audioLanguage] = new Dictionary<DialogueName, AudioClip>();
        }
		else if (dialogues[audioLanguage].ContainsKey(name))
		{
			return dialogues[audioLanguage][name];
		}
		AudioClip clip = LoadAudioClip(name, audioLanguage);
		return clip;
	}

	private AudioClip LoadAudioClip(DialogueName name, LanguageManager.Language audioLanguage)
	{
		AudioClip clip = Resources.Load<AudioClip>(audioFolder + "/" + audioLanguage + "/" + name);
		if (clip)
		{
			dialogues[audioLanguage][name] = clip;
		}
		return clip;
	}

	public enum SoundName
	{

	}

    public enum DialogueName
    {

    }
}