using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour {

    public int Level;
    public float Power;
    
    public SpriteRenderer SpriteRenderer { get; private set; }

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();            
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Activate(float duration)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
