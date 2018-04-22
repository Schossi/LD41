using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {
    
    void Awake()
    {
        if (GameManager.Instance == null)
            gameObject.AddComponent(typeof(GameManager));
    }

    // Use this for initialization
    void Start () {
        GameManager.Instance.StartScene();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
