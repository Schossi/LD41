using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour {

    public const float TransitionDuration = 1.5f;

    public static TransitionManager Instance;

    private Image _fadeImage;
    
    private int _state = 1;//0>IDLE 1>IN 2>OUT
    private float _progress = 0f;
    private Action _onTransitioned;

    private void Awake()
    {
        Instance = this;

        _fadeImage = transform.Find("FadePanel").GetComponent<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        _fadeImage.color = new Color(0f, 0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == 0)
            return;

        _progress += Time.deltaTime;

        float t = _progress / TransitionDuration;
        float a;
        if (_state == 1)
            a = Mathf.SmoothStep(1f, 0f, t);
        else
            a = Mathf.SmoothStep(0f, 1f, t);

        _fadeImage.color = new Color(0f, 0f, 0f, a);

        if (_progress >= TransitionDuration)
        {
            if (_onTransitioned != null)
                _onTransitioned();
            _state = 0;
        }
    }

    public void TransitionOut(Action onTransitioned)
    {
        _onTransitioned = onTransitioned;
        _state = 2;
        _progress = 0f;
    }
}
