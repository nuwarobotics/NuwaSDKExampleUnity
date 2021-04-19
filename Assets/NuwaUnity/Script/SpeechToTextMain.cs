using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechToTextMain : MonoBehaviour
{
    public Text ouputTxt;
    private bool mNeedChange = false;
    private string mJsonString = string.Empty;

    void Start()
    {
        Nuwa.init();
        //Step1: Set Event
        Nuwa.onSpeech2TextComplete += SpeechCallback;
    }

    private void OnDisable()
    {
        Nuwa.onSpeech2TextComplete -= SpeechCallback;
    }

    //Step2: Set Listen Parameter 
    public void SetupLeistenParameter()
    {
        Nuwa.setListenParameter(Nuwa.ListenType.RECOGNIZE, "language", "en_us");    //set language
        Nuwa.setListenParameter(Nuwa.ListenType.RECOGNIZE, "accent", null);   
    }

    public void StartSpeech2Text()
    {
        SetupLeistenParameter();
        //Step3: Start STT
        Nuwa.startSpeech2Text(false);
        mJsonString = "StartSpeechToText\n";
        mNeedChange = true;
    }

    //result data
    private void SpeechCallback(bool isError, string json)
    {
        if (!isError)
        {
            mJsonString = "FinishSpeechToText, length:" + json.Length + "\n";
            mJsonString += json;
            mNeedChange = true;
            //ouputTxt.text = json;
        }
    }

    public void RetuenToTitle()
    {
        this.enabled = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    private void Update()
    {
        if (mNeedChange)
        {
            mNeedChange = false;
            ouputTxt.text = mJsonString;
        }
    }
}