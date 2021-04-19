using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LocalCommandRecognition : MonoBehaviour
{
    public string mi_Name;
    public string[] values;

    public Action<Nuwa.NuwaVoiceRecognition> trueEvent;
    public Action<Nuwa.ResultType, string> falseEvent;
    public Action<string> startEvent;

    void OnGrammarState(bool isError, string info)
    {
        Debug.Log(string.Format("OnGrammarState isError = {0} , info = {1}", isError, info));
        Nuwa.startLocalCommand();
    }

    #region new Local Command
    void TrueFunction(Nuwa.NuwaVoiceRecognition recognitionInfo)
    {
        trueEvent.Invoke(recognitionInfo);
        RemoveVoiceRecognition();
    }

    void FalseFunction(Nuwa.ResultType resultType, string json)
    {
        falseEvent.Invoke(resultType, json);
        RemoveVoiceRecognition();
    }

    public void RegisterVoiceRecognition()
    {
        Debug.Log("unity: RegisterVoiceRecognition");

        Nuwa.onLocalCommandComplete += TrueFunction;
        Nuwa.onLocalCommandException += FalseFunction;
        Nuwa.onGrammarState += OnGrammarState;
    }

    public void RemoveVoiceRecognition()
    {
        Debug.Log("unity: RemoveVoiceRecognition");

        Nuwa.onLocalCommandComplete -= TrueFunction;
        Nuwa.onLocalCommandException -= FalseFunction;
        Nuwa.onGrammarState -= OnGrammarState;
    }
    #endregion

    #region Button Event
    public void StartLocalCommand()
    {
        Debug.Log("開始localCommand");
        Nuwa.startLocalCommand();
        startEvent("開始localCommand");
        RegisterVoiceRecognition();
    }

    public void StopListen()
    {
        Debug.Log("關閉聆聽");
        Nuwa.stopListen();
        RemoveVoiceRecognition();
    }

    public void Register()
    {
        Debug.Log("註冊1");
        string ans = "";
        foreach (string str in values)
            ans = ans + " " + str;
        startEvent("開始localCommand \n " + ans);

        RegisterVoiceRecognition();
        Nuwa.prepareGrammarToRobot(mi_Name, values);
    }
    #endregion
}

