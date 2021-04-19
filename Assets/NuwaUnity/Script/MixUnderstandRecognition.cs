using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MixUnderstandRecognition : MonoBehaviour
{
    public string mi_Name;
    public string[] values;

    public Action<Nuwa.NuwaVoiceRecognition> trueEvent;
    public Action<Nuwa.ResultType, string> falseEvent;
    public Action<bool, Nuwa.ResultType, string> mixUnderstandEvent;

    public Action<string> startEvent;

    private void Start()
    {
        GameObject.Find("ButtonGroup").transform.Find("Start").GetComponent<Button>().onClick.AddListener(Register);
        GameObject.Find("ButtonGroup").transform.Find("Stop").GetComponent<Button>().onClick.AddListener(StopListen);
    }

    void OnGrammarState(bool isError, string info)
    {
        Debug.Log(string.Format("OnGrammarState isError = {0} , info = {1}", isError, info));
        //step3: Set Grammar finish, and startMixUnderstand
        Nuwa.startMixUnderstand();
    }

    //step4: Get result
    /// <summary>
    /// startMixUnderstand result
    /// </summary>
    /// <param name="resultType"> two type: UNDERSTAND , LOCAL_COMMAND </param>
    /// <param name="json"> result json string </param>
    /// LOCAL_COMMAND json format:
    /// {
    ///     "result": "test",
    ///     "x-trace-id": "2ed6670726b47c5a3ac006aafa9216d",
    ///     "engine": "Google Cloud",
    ///     "type": 1,
    ///     "class": "com.nuwarobotics.lib.voice.hybrid.engine.NuwaTWMixEngine",
    ///     "version": 1,
    ///     "extra": {"content": "String"},
    ///     "content": "{\"sn\":1,\"ls\":true,\"bg\":0,\"ed\":0,\"ws\":[{\"bg\":0,\"slot\":\"<MiboMixunderstand>\",\"cw\":[{\"id\":10001,\"w\":\"測試\",\"sc\":96,\"gm\":0}]}],\"sc\":94}"
    /// }
    /// 
    /// UNDERSTAND json format:
    /// {
    ///     "result": "test",
    ///     "x-trace-id": "2ed6670726b47c5a3ac006aafa9216d",
    ///     "engine": "Google Cloud",
    ///     "type": 2,
    ///     "class": "com.nuwarobotics.lib.voice.hybrid.engine.NuwaTWMixEngine",
    ///     "version": 1,
    /// }
    void MixUnderstandFunction(bool isError, Nuwa.ResultType resultType, string json)
    {
        mixUnderstandEvent.Invoke(isError, resultType, json);
        RemoveVoiceRecognition();
    }

    /// <summary> ture result </summary>
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
        //step1: Register Event
        Nuwa.onMixUnderstandComplete += MixUnderstandFunction;
        Nuwa.onLocalCommandComplete += TrueFunction;
        Nuwa.onLocalCommandException += FalseFunction;
        Nuwa.onGrammarState += OnGrammarState;
    }

    public void RemoveVoiceRecognition()
    {
        Debug.Log("Remove Voice Recognition");

        Nuwa.onMixUnderstandComplete -= MixUnderstandFunction;
        Nuwa.onLocalCommandComplete -= TrueFunction;
        Nuwa.onLocalCommandException -= FalseFunction;
        Nuwa.onGrammarState -= OnGrammarState;
    }

    #region Button Event
    public void StopListen()
    {
        Debug.Log("StopListen");
        Nuwa.stopListen();
        RemoveVoiceRecognition();
    }

    public void Register()
    {
        Debug.Log("Register word");
        string ans = "";
        foreach (string str in values)
            ans = ans + "," + str;

        RegisterVoiceRecognition();

        //step2: Register answer
        Nuwa.prepareGrammarToRobot(mi_Name, values);

        string s = ans == "" ? "Not registered word" : "Registered word:";
        string textContent = string.Format("Start MixUnderstand \n {0} {1}", s, ans);
        startEvent(textContent);
    }

    public void ReturnTitle()
    {
        Nuwa.stopListen();
        RemoveVoiceRecognition();

        StartCoroutine(DelayBack());
    }
    #endregion

    IEnumerator DelayBack()
    {
        yield return null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }
}
