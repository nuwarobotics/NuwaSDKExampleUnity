using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class TTsPlayMain : MonoBehaviour {

    public string[] TTsArr = new string[] {
       
    };

    public Dropdown TTSDropdown;

    public Text logText;

    public InputField InputFieldText;

    public Slider TTsSpeedSlider;
    private Text mTTsSpeedText;
    public Slider TTsPitchSlider;
    private Text mTTsPitchText;

    private void Start()
    {
        TTSDropdown.ClearOptions();
        TTSDropdown.AddOptions(TTsArr.ToList());

        //step1: add event
        NuwaEventTrigger trigger = this.GetComponent<NuwaEventTrigger>();
        if(trigger != null)
        {
            trigger.onTTSComplete.AddListener(OnTTsComplete);
        }


        TTsSpeedSlider.onValueChanged.AddListener(
            delegate
            {
                float value = TTsSpeedSlider.value;
                //Step2: Set TTS speed, range 1~9.
                Nuwa.SetSpeakParameter("speed", value.ToString());
                if (mTTsSpeedText == null)
                    mTTsSpeedText = TTsSpeedSlider.GetComponentInChildren<Text>();
                mTTsSpeedText.text = "Speed : " + value;
            }
        );

        TTsPitchSlider.onValueChanged.AddListener(
            delegate
            {
                float value = TTsPitchSlider.value;
                //Step2: Set TTS pitch, range 1~9.
                Nuwa.SetSpeakParameter("pitch", value.ToString());
                if (mTTsPitchText == null)
                    mTTsPitchText = TTsPitchSlider.GetComponentInChildren<Text>();
                mTTsPitchText.text = "Pitch : " + value;
            }
        );
    }

    private void OnTTsComplete(bool isError)
    {
        logText.text += "\n OnTTsComplete, isError:"+ isError;
    }

    public void TTsPlay()
    {
        int idx = TTSDropdown.value;
        //step3: set value and start TTS
        Nuwa.startTTS(TTsArr[idx]);
        logText.text = "Play tts";
    }

    public void TTsStop()
    {
        Nuwa.stopTTS();
        logText.text += "\nStopTTs";
    }

    public void TTsResume()
    {
        Nuwa.resumeTTS();
        logText.text += "\nResumeTTs";
    }

    public void TTsPause()
    {
        Nuwa.pauseTTS();
        logText.text += "\nPauseTTs";
    }

    public void OnReturnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    public void PlayInputTTs()
    {
        String text = InputFieldText.text;
        Nuwa.startTTS(text);
    }

    public void PlayMotion()
    {
        Nuwa.motionPlay("888_ML_AiYA_10");
    }

    public void PlayMotor()
    {
        int id = UnityEngine.Random.Range(3, 8);
        int range = UnityEngine.Random.Range(-30, 30);
        Nuwa.setMotorPositionInDegree(id, range, 10);
    }
}
