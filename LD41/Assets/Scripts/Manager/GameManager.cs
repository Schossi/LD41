using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public int CurrentLevel { get; set; }
    public int Progress { get; set; }
    public bool IsMain { get; set; }

    private bool _levelChanging = false;

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
        }
    }

    public void LevelFinished()
    {
        if (CurrentLevel > Progress)
            PlayerPrefs.SetInt("Progress", CurrentLevel);

        ChangeLevel(0);
    }

    public void LevelLost()
    {
        ChangeLevel(0);
    }

    public void ChangeLevel(int level)
    {
        if (_levelChanging)
            return;

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
}
