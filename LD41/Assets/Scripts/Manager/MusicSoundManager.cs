using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSoundManager : MonoBehaviour {

    private GameObject _sound;
    private GameObject _music;

    void Awake()
    {
        _sound = transform.Find("SoundButton").Find("ImageSoundWaves").gameObject;
        _music = transform.Find("MusicButton").Find("ImageMusic").gameObject;
    }

    // Use this for initialization
    void Start () {
        SetImages();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetImages()
    {
        _sound.SetActive(GameManager.Instance.SoundEnabled);
        _music.SetActive(GameManager.Instance.MusicEnabled);
    }

    public void ToggleSound()
    {
        GameManager.Instance.ToggleSound();
        SetImages();
    }

    public void ToggleMusic()
    {
        GameManager.Instance.ToggleMusic();
        SetImages();
    }
}
