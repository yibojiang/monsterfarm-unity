using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {
    private static GameTimeManager _instance;
    public static GameTimeManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<GameTimeManager>();
                if (!_instance) {
                    var gameObj = new GameObject();
                    _instance = gameObj.AddComponent<GameTimeManager>();
                }
            }
            return _instance;
        }
    }

    private float _currentTime;
    private Camera _cam;
    ColorGradingModel _colorGrading;
    ColorGradingModel.Settings _colorGradingSettings;
    public Image pointer;
    private const float _daySeconds = 300f;
    public int Day { get; private set; }

    // Use this for initialization
    void Start () {
        _cam = Camera.main;
        var postProcess = _cam.GetComponent<PostProcessingBehaviour>();
        _colorGrading = postProcess.profile.colorGrading;
        _currentTime = 0;
    }

    void ApplyTimeChanged(float time)
    {
        _colorGradingSettings = _colorGrading.settings;
        float temp;
        float blackout;
        if (time < 0.5f)
        {
            temp = (time * 2 * 100 - 50);
        }
        else
        {
            temp = ((1f - time) * 2 * 100 - 50);
        }

        if (time > 0.5f && time < 0.75f)
        {            
            blackout = ((time - 0.5f) * 4 * -0.09f);
        }
        else if (time >= 0.75f && time < 1.0f)
        {   
            blackout = (((0.75f - time) * 4 + 1f) * -0.09f);
        }
        else
        {
            blackout = 0f;
        }
        
        _colorGradingSettings.basic.temperature = temp;
        _colorGradingSettings.tonemapping.neutralBlackOut = blackout;
        _colorGrading.settings = _colorGradingSettings;
    }

    public void NewDay()
    {
        Day++;
        var monsters = FindObjectsOfType<MonsterPawn>();
        for (int i = 0; i < monsters.Length; i++)
        {
            var monster = monsters[i];
            monster.AddAge();
        }
    }

    // Update is called once per frame
    void Update () {

        _currentTime += Time.deltaTime;
        if (_currentTime > _daySeconds)
        {
            _currentTime -= _daySeconds;
            NewDay();
        }

        float timePercent = _currentTime / _daySeconds;
        ApplyTimeChanged(timePercent);
        
        var tmpRotation = pointer.transform.eulerAngles;
        tmpRotation.z = (1f - _currentTime / _daySeconds) * 360;
        pointer.transform.eulerAngles = tmpRotation;
    }
}
