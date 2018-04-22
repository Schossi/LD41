using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour {

    private static bool _forceColorsSet = false;
    public static Color Force1Color;
    public static Color Force2Color;
    public static Color Force3Color;

    public int Level;
    
    public SpriteRenderer SpriteRenderer { get; private set; }

    public static Color GetForceColor(int level)
    {
        if (!_forceColorsSet)
        {
            _forceColorsSet = true;
            ColorUtility.TryParseHtmlString("#33A5FF", out Force1Color);
            ColorUtility.TryParseHtmlString("#3259E2", out Force2Color);
            ColorUtility.TryParseHtmlString("#28237B", out Force3Color);
        }

        switch (level)
        {
            case 1:
                return Force1Color;
            case 2:
                return Force2Color;
            case 3:
                return Force3Color;
            default:
                return Force1Color;
        }
    }
    public static float GetForcePower(int level)
    {
        switch (level)
        {
            case 1:
                return 5;
            case 2:
                return 7;
            case 3:
                return 10;
            default:
                return 5;
        }
    }

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();            
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);

        SpriteRenderer.color = GetForceColor(Level);
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
