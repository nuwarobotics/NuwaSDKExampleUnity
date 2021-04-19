using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecognizeMain : MonoBehaviour
{
    public bool RecognizeAnswerClick;   //button is click

    private Nuwa.OutputData[] mData = null;
    private bool mIsNeedCheck = false;

    public Text InfoText;

    // Use this for initialization
    void Awake()
    {
        Nuwa.init();
    }

    public void isConnectRecognizeSystem(bool isConnected)
    {
        if (isConnected)
        {
            //step2 : register event
            Nuwa.onOutput += ReconizeCheck;
        }
    }

    public void StartRecognizeClick()
    {
        //step1 : register event
        Nuwa.onConnected += isConnectRecognizeSystem;
        //step3 : start recognition obj
        Nuwa.startRecognition(Nuwa.NuwaRecognition.OBJ);
        RecognizeAnswerClick = true;
        InfoText.text = "Start Recognize";
    }

    //stpe4: get obj Recognize Data
    // ReconizeCheck is called once per frame
    public void ReconizeCheck(Nuwa.OutputData[] data)
    {
        Debug.Log("ReconizeCheck, RecognizeAnswerClick:" + RecognizeAnswerClick);
        if (RecognizeAnswerClick)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].dataSets.Length; j++)
                {
                    Debug.Log("ReconizeCheck," + i + ", " + j + "," + data[i].dataSets[j].ToString());
                }
            }

            mData = data;
            mIsNeedCheck = true;
        }
    }

    private void UnRegisterProcess()
    {
        Nuwa.stopRecognition();
        Nuwa.onOutput -= ReconizeCheck;
        Nuwa.onConnected -= isConnectRecognizeSystem;

        InfoText.text += "\nRecognizeSystem Disconnected";
    }

    public void RetuenToTitle()
    {
        UnRegisterProcess();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    private void Update()
    {
        if (mIsNeedCheck)
        {
            InfoText.text += "\nRecognizeSystem Connected ,setInfo";
            mIsNeedCheck = false;
            SetInfoData(mData);
        }
    }

    private void SetInfoData(Nuwa.OutputData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].dataSets.Length; j++)
            {
                InfoText.text += "\n i:" + i + ", j:" + j + ", " + data[i].dataSets[j].title;
            }
        }
        UnRegisterProcess();
    }
}