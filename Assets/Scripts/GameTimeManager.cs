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
        colorGradingSettings_.basic.temperature = time;
        colorGrading_.settings = colorGradingSettings_;
    }

    // Update is called once per frame
    void Update () {

        _currentTime += Time.deltaTime;
        _currentTime = _currentTime % 200;
        var tmpRotation = pointer.transform.eulerAngles;
        tmpRotation.z = (1f - _currentTime / 200) * 360;
        pointer.transform.eulerAngles = tmpRotation;
        ApplyTimeChanged(_currentTime - 100);
    }
}
