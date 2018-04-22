using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public int CurrentLevel { get; set; }
    public int Progress { get; set; }
    public bool IsMain { get; set; }

    private bool _musicEnabled = true;
    public bool MusicEnabled
    {
        get { return _musicEnabled; }
    }

    private bool _soundEnabled = true;
    public bool SoundEnabled
    {
        get { return _soundEnabled; }
    }

    public bool SwiftEnabled { get; set; }
    public bool WidthEnabled { get; set; }
    public bool ChargeEnabled { get; set; }

    private bool _levelChanging = false;
    private bool _paused = false;
    public bool Paused
    {
        get
        {
            return _paused;
        }
    }

    public bool IsLocked
    {
        get
        {
            return Cursor.lockState == CursorLockMode.Confined;
        }
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start ()
    {
	}
    	
	// Update is called once per frame
	void Update () {

        if (IsMain)
        {
            if (!IsLocked && Input.GetMouseButton(0))
            {
                Lock();
            }
        }

        if (Input.GetKeyDown("escape"))
        {
            Unlock();

            if (IsMain)
            {
                if (_paused)
                    Resume();
                else
                    Pause();
            }
        }
    }
    
    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Pause()
    {
        _paused = true;
        Time.timeScale = 0f;
        PauseUIManager.Instance.Show();
    }
    public void Resume()
    {
        _paused = false;
        Time.timeScale = 1f;
        PauseUIManager.Instance.Hide();
    }

    public void StartScene()
    {
        IsMain = SceneManager.GetActiveScene().name == "Main";

        if (PlayerPrefs.HasKey("Progress"))
            Progress = PlayerPrefs.GetInt("Progress");
        else
            Progress = 0;

        if (IsMain)
        {
            LevelManager.Instance.StartLevel(CurrentLevel);
        }
        else
        {
            Unlock();
            UIManager.Instance.SetProgress(Progress);
            SkillsManager.Instance.StartSkills();

            FindObjectOfType<Sphere>().ApplyForce(1, new Vector3(1, 1, 0));
        }
    }

    public void LevelFinished()
    {
        PauseUIManager.Instance.ShowAnnouncement("CHARGE DISPELLED!");

        if (CurrentLevel > Progress)
            PlayerPrefs.SetInt("Progress", CurrentLevel);

        ChangeLevel(0);
    }

    public void LevelLost()
    {
        PauseUIManager.Instance.ShowAnnouncement("MONSTERS GOT PAST YOU!" + Environment.NewLine + "THE VILLAGE IS DOOMED!");
        ChangeLevel(0);
    }

    public void ChangeLevel(int level)
    {
        if (_levelChanging)
            return;

        if (IsMain)
            Resume();

        _levelChanging = true;

        TransitionManager.Instance.TransitionOut(() =>
        {
            _levelChanging = false;

            CurrentLevel = level;

            string sceneName;
            if (level == 0)
                sceneName = "Menu";
            else
                sceneName = "Main";

            SceneManager.LoadScene(sceneName);
        });
    }

    public void ToggleSound()
    {
        _soundEnabled = !_soundEnabled;
    }
    public void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
    }
}
