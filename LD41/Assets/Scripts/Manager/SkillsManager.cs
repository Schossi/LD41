using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour {

    public static SkillsManager Instance;

    private Text _pointsText;

    private Toggle _swiftToggle;
    private Toggle _widthToggle;
    private Toggle _chargeToggle;

    private bool _started = false;

    void Awake()
    {
        Instance = this;

        _pointsText = transform.Find("PointsText").GetComponent<Text>();

        _swiftToggle = transform.Find("SwiftToggle").GetComponent<Toggle>();
        _widthToggle = transform.Find("WidthToggle").GetComponent<Toggle>();
        _chargeToggle = transform.Find("ChargeToggle").GetComponent<Toggle>();
    }
    
    public void StartSkills()
    {
        _swiftToggle.isOn = GameManager.Instance.SwiftEnabled;
        _widthToggle.isOn = GameManager.Instance.WidthEnabled;
        _chargeToggle.isOn = GameManager.Instance.ChargeEnabled;

        refreshPoints();

        _started = true;
    }

    public void SkillChanged()
    {
        if (!_started)
            return;

        GameManager.Instance.SwiftEnabled = _swiftToggle.isOn;
        GameManager.Instance.WidthEnabled = _widthToggle.isOn;
        GameManager.Instance.ChargeEnabled = _chargeToggle.isOn;

        refreshPoints();
    }

    private void refreshPoints()
    {
        int points = GameManager.Instance.Progress;

        if (GameManager.Instance.SwiftEnabled)
            points--;
        if (GameManager.Instance.WidthEnabled)
            points--;
        if (GameManager.Instance.ChargeEnabled)
            points--;

        _pointsText.text = points.ToString();

        setTogglesInteractable(points > 0);
    }

    private void setTogglesInteractable(bool value)
    {
        if (value)
        {
            _swiftToggle.interactable = value;
            _widthToggle.interactable = value;
            _chargeToggle.interactable = value;
        }
        else
        {
            _swiftToggle.interactable = _swiftToggle.isOn;
            _widthToggle.interactable = _widthToggle.isOn;
            _chargeToggle.interactable = _chargeToggle.isOn;
        }
    }
}
