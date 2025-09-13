using System;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    /*
    1.
    Title: Introduction to AUDIO in Unity
    Author: Asbjørn Thirslund / Brackeys
    Date: 19 April 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=6OT43pvUyfY&t=620s
    
    2.
    Title: "Sophies Heroic Bloodbath - Assignment 4 - Exam - Playable Game"
    Author: Miguel Marindanise, Fatima Zahraa Bham, Yongama Ntloko 
    Date: 16 August 2025
    Code version: 1
    */

    public SoundScript[] sounds;

    public static AudioManagerScript instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        */

        foreach (SoundScript s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Background");
    }

    public void Play(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }

    public void PlayOnButtonPress(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip, 1f);

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }

    public void Stop(string name)
    {
        SoundScript s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();

        if (s == null)
        {
            Debug.Log("sound" + name + "is missing");
        }
        return;
    }
}
