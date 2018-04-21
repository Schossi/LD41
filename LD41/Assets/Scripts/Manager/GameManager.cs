using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    
    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        LevelManager.Instance.StartLevel(1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LevelFinished()
    {

    }
}
