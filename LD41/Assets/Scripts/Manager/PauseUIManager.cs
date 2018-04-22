using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour {

    public static PauseUIManager Instance;

    private GameObject _inner;
    private Text _announcementText;

    void Awake()
    {
        Instance = this;

        _inner = transform.Find("PauseCanvasInner").gameObject;
        _announcementText = transform.Find("AnnouncementText").GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        Hide();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show()
    {
        _inner.SetActive(true);
    }

    public void Hide()
    {
        _inner.SetActive(false);
    }

    public void BackToMenu()
    {
        GameManager.Instance.ChangeLevel(0);
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
    }

    public void ShowAnnouncement(string text)
    {
        _announcementText.text = text;
        _announcementText.gameObject.SetActive(true);
    }
}
