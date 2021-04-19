using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceDetectMain : MonoBehaviour
{
    private Nuwa.FaceRecognizeData[] mData = null;
    private bool mIsNeedCheck = false;

    public Text InfoText;
    private bool RecognizeAnswerClick = false;

    void Awake()
    {
        Nuwa.init();
    }

    //Receive after successful connection
    public void isConnectRecognizeSystem(bool isConnected)
    {
        if (isConnected)
        {
            //step2 : register event
            Nuwa.onFaceRecognize += ReconizeCheck;
        }
    }

    public void StartRecognizeClick()
    {
        //step1 : register event
        Nuwa.onConnected += isConnectRecognizeSystem;
        //step3 : start recognition
        Nuwa.startRecognition(Nuwa.NuwaRecognition.FACE_RECOGNITION);
        RecognizeAnswerClick = true;
        InfoText.text = "Start Recognize";
    }

    public void UnRegisterProcess()
    {
        Nuwa.stopRecognition();
        Nuwa.onFaceRecognize -= ReconizeCheck;
        Nuwa.onConnected -= isConnectRecognizeSystem;
        mIsNeedCheck = false;

        InfoText.text += "\nRecognizeSystem Disconnected";
    }

    public void RetuenToTitle()
    {
        UnRegisterProcess();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }



    //stpe4: get Face Recognize Data
    //ReconizeCheck is called once per frame
    public void ReconizeCheck(Nuwa.FaceRecognizeData[] data)
    {
        if (RecognizeAnswerClick)
        {
            mData = data;
            mIsNeedCheck = true;
        }
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

    private void SetInfoData(Nuwa.FaceRecognizeData[] data)
    {
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                InfoText.text += "\nid:" + data[i].idx + ", name:" + data[i].name + ", conf:" + data[i].conf + ", rect:" + JsonUtility.ToJson(data[i].rect);
            }
        }
        Debug.Log("FaceDetectMain, SetInfoData finish, UnRegisterProgress");

        UnRegisterProcess();
    }

}
