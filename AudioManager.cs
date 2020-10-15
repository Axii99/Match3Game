using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;
    public AudioMixerGroup ax;
    private void Awake() {

        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else {

            Destroy(this.gameObject);
        }


        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = ax;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.name == "bgm") {
                s.source.loop = true;
            }
        }
    }

    private void OnLevelWasLoaded(int level) {
        PlaySound("Theme");
    }

    // Start is called before the first frame update
    void Start()
    {   if (!PauseMenu.GamePause) {
            PlaySound("Theme");
        }
    }

    // Update is called once per frame
    void Update() {
        Sound theme = Array.Find(sounds, sound => sound.name == "Theme");
        if (theme != null) {
            if (PauseMenu.GamePause) {

                //theme.source.Pause();
            }
            else if (!theme.source.isPlaying) {
                theme.source.Play();
            }
        }
    }

    public void PlaySound(string name) {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return;
        }
        if (!s.source.isPlaying) {
            s.source.Play();
        }
    }

    public void PauseSound(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            return;
        }
        if (s.source.isPlaying) {
            s.source.Pause();
        }
    }
}
