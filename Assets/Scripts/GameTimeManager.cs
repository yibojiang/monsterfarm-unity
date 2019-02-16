using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

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

    private float currentTime_;
    private Camera cam_;
    ColorGradingModel colorGrading_;
    ColorGradingModel.Settings colorGradingSettings_;

    // Use this for initialization
    void Start () {
        cam_ = Camera.main;
        var postProcess = cam_.GetComponent<PostProcessingBehaviour>();
        colorGrading_ = postProcess.profile.colorGrading;
        currentTime_ = 0;

        //PostProcessVolume volume = cam_.GetComponent<PostProcessVolume>();
        //volume.profile.TryGetSettings(out colorGradingLayer);
    }

    void ApplyTimeChanged(float time)
    {
        colorGradingSettings_ = colorGrading_.settings;
        colorGradingSettings_.basic.temperature = time;
        colorGrading_.settings = colorGradingSettings_;
    }

    // Update is called once per frame
    void Update () {

        currentTime_ += Time.deltaTime;
        currentTime_ = currentTime_ % 200;
        ApplyTimeChanged(currentTime_ - 100);
        //Debug.Log(currentTime_);
    }
}
