using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    private Animator _animator;

    private int _health = 1;

    void Awake()
    {
        _animator = GetComponent<Animator>();    
    }

    // Use this for initialization
    void Start () {
        LevelManager.Instance.EnemyCount++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hit()
    {
        _health--;
        if (_health <= 0)
            die();
    }

    private void die()
    {
        LevelManager.Instance.EnemyDefeated();
        _animator.SetBool("dead", true);

    }
}
