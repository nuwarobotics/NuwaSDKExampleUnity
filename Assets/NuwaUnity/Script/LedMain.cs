using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LedMain : MonoBehaviour {


    private List<Color> mColorList = new List<Color>();
    private List<Nuwa.LEDPosition> mTypeList = new List<Nuwa.LEDPosition>();

    public Dropdown ColorDropDown;

    public Dropdown TypeDropDown;

    private int mColorIdx = 0;
    private int mTypeIdx = 0;

    private int mBreathValue = 3;

    public Text BreathSpeedText;
    public Slider BreathSlider;

    // Use this for initialization
    void Start () {
        Nuwa.init();

        TypeDropDown.ClearOptions();
        mTypeList.Add(Nuwa.LEDPosition.Head);
        mTypeList.Add(Nuwa.LEDPosition.Chest);
        mTypeList.Add(Nuwa.LEDPosition.LeftHand);
        mTypeList.Add(Nuwa.LEDPosition.RightHand);
        List<string> dataList = new List<string>();
        for (int i = 0; i < mTypeList.Count; i++)
            dataList.Add(mTypeList[i].ToString());
        TypeDropDown.AddOptions(dataList);

        ColorDropDown.ClearOptions();
        mColorList.Add(Color.red);
        mColorList.Add(Color.blue);
        mColorList.Add(Color.green);
        mColorList.Add(Color.yellow);
        mColorList.Add(Color.black);
        mColorList.Add(Color.gray);
        dataList.Clear();

        dataList.Add("Red");
        dataList.Add("Blue");
        dataList.Add("Green");
        dataList.Add("Yellow");
        dataList.Add("black");
        dataList.Add("gray");
        ColorDropDown.AddOptions(dataList);

        TypeDropDown.onValueChanged.AddListener(OnTypeChange);
        ColorDropDown.onValueChanged.AddListener(OnColorChange);


        BreathSlider.onValueChanged.AddListener(OnSliderValueChange);

        SetSppedText();
    }

    private void OnTypeChange(int value)
    {
        mTypeIdx = value;
    }

    private void OnColorChange(int value)
    {
        mColorIdx = value;
    }


    public void EnableLED()
    {
        //step1: close systemLED
        Nuwa.disableSystemLED();

        //step2: set color and position
        Nuwa.setLedColor(mTypeList[mTypeIdx] , mColorList[mColorIdx]);

        //step3: open led
        Nuwa.enableLed(mTypeList[mTypeIdx], true);
    }

    public void DisableLed()
    {
        //step4: close all led
        for (int idx = 0; idx < mTypeList.Count; idx++)
            Nuwa.enableLed(mTypeList[idx], false);
        //step5: open systemLED
        Nuwa.enableSystemLED(); 
    }

    public void RetuenToTitle()
    {
        DisableLed();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    private void OnSliderValueChange(float value)
    {
        mBreathValue = (int)value;
        SetSppedText();
    }

    public void SetLedBreath()
    {
        //Breathing effect
        Nuwa.enableLedBreath(mTypeList[mTypeIdx], mBreathValue, mBreathValue);
    }

    private void SetSppedText()
    {
        BreathSpeedText.text = "BreathSpeed:" + mBreathValue.ToString();
    }
}
