using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Func<Enemy> EnemyTemplate;

    private Enemy _createdEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateEnemy()
    {
        _createdEnemy = Instantiate(EnemyTemplate(), transform.parent);
        _createdEnemy.transform.position = transform.position;
    }

    public void Finish()
    {
        _createdEnemy.StartMoving();
        Destroy(gameObject);
    }
}
