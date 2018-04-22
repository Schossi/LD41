using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;    
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetProgress(int progress)
    {
        transform.Find("LevelButtons").Find("ButtonLevel2").GetComponent<Button>().interactable = progress > 0;
        transform.Find("LevelButtons").Find("ButtonLevel3").GetComponent<Button>().interactable = progress > 1;

        transform.Find("ImageWon").gameObject.SetActive(progress > 2);
    }

    public void GoToLevel1()
    {
        GameManager.Instance.ChangeLevel(1);
    }
    public void GoToLevel2()
    {
        GameManager.Instance.ChangeLevel(2);
    }
    public void GoToLevel3()
    {
        GameManager.Instance.ChangeLevel(3);
    }
}
