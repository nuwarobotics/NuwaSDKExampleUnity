using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalCommandCallback : MonoBehaviour
{
    public LocalCommandRecognition miboVoice;

    public Text txt;
    bool isFirst = true;

    private void Start()
    {
        if (miboVoice == null)
            return;

        miboVoice.trueEvent += MiboTrueFunction;
        miboVoice.falseEvent += MiboFalseFunction;
        miboVoice.startEvent += MiboStartLocalFunction;
    }

    private bool mIsNeedUpdate = false;
    private string mResultText = string.Empty;

    void MiboTrueFunction(Nuwa.NuwaVoiceRecognition recognitionInfo)
    {
        mResultText = "MiboTrueFunction ";
        mIsNeedUpdate = true;
        mResultText += "\n辨識正確 :  " + recognitionInfo.ToString();
    }

    void MiboFalseFunction(Nuwa.ResultType resultType, string json)
    {
        mResultText = "MiboFalseFunction ";
        mIsNeedUpdate = true;
        mResultText += "\n辨識錯誤 :  " + '\n' + "resultType :[" + resultType.ToString() + "]  json : " + json;
    }

    void MiboStartLocalFunction(string text)
    { 
        mIsNeedUpdate = true;
        mResultText = text;
    }

    public void RetuenToTitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    public void Update()
    {
        if (mIsNeedUpdate)
        {
            mIsNeedUpdate = false;
            txt.text = mResultText;
        }
    }
}
