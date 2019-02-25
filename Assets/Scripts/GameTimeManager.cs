using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour {
    private static GameTimeManager Instance_;
    public static GameTimeManager Instance
    {
        get
        {
            if (!Instance_)
            {
                Instance_ = GameObject.FindObjectOfType<GameTimeManager>();
                if (!Instance_) {
                    var gameObj = new GameObject();
                    Instance_ = gameObj.AddComponent<GameTimeManager>();
                }
            }
            return Instance_;
        }
    }

    private float _currentTime;
    private Camera cam_;
    ColorGradingModel colorGrading_;
    ColorGradingModel.Settings colorGradingSettings_;
    public Image pointer;
    private const float _daySeconds = 60f;
    public int Day { get; private set; }

    // Use this for initialization
    void Start () {
        cam_ = Camera.main;
        var postProcess = cam_.GetComponent<PostProcessingBehaviour>();
        colorGrading_ = postProcess.profile.colorGrading;
        _currentTime = 0;
    }

    void ApplyTimeChanged(float time)
    {
        colorGradingSettings_ = colorGrading_.settings;
        var temp = time;
        var blackout = time;
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
        
        colorGradingSettings_.basic.temperature = temp;
        colorGradingSettings_.tonemapping.neutralBlackOut = blackout;
        colorGrading_.settings = colorGradingSettings_;
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
