using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class MotorMain : MonoBehaviour {

    public Text InfoText;
    public Dropdown SelectDropdown;
    public Slider MotorRotateDegreeSlider;
    public Slider MotorSpeedSlider;

    public Text InfoText2;

	
	void Start ()
    {
        InvokeRepeating("GetMoterText", 0.5f, 0.1f);

        SelectDropdown.ClearOptions();

        string [] values = Enum.GetNames(typeof(Nuwa.NuwaMotorType));

        Debug.Log(JsonUtility.ToJson(values));
        SelectDropdown.AddOptions(values.ToList());

        MotorRotateDegreeSlider.onValueChanged.AddListener(OnSliderValueChange);
        MotorSpeedSlider.onValueChanged.AddListener(OnSliderValueChange);

        SetInfoText();
    }

    public void GetMoterText()
    {
        // get motor degree
        string output =
                 Nuwa.NuwaMotorType.neck_y.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.neck_y)
        + "\n" + Nuwa.NuwaMotorType.neck_z.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.neck_z)
        + "\n" + Nuwa.NuwaMotorType.right_shoulder_z.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.right_shoulder_z)
        + "\n" + Nuwa.NuwaMotorType.right_shoulder_y.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.right_shoulder_y)
        + "\n" + Nuwa.NuwaMotorType.right_shoulder_x.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.right_shoulder_x)
        + "\n" + Nuwa.NuwaMotorType.right_bow_y.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.right_bow_y)
        + "\n" + Nuwa.NuwaMotorType.left_shoulder_z.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.left_shoulder_z)
        + "\n" + Nuwa.NuwaMotorType.left_shoulder_y.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.left_shoulder_y)
        + "\n" + Nuwa.NuwaMotorType.left_shoulder_x.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.left_shoulder_x)
        + "\n" + Nuwa.NuwaMotorType.left_bow_y.ToString() + ":" + Nuwa.getMotorPresentPossitionInDegree(Nuwa.NuwaMotorType.left_bow_y);

        InfoText.text = output;
    }
    
    public void OnReturnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    public void OnPlayMotorButtonClick()
    {
        Debug.LogWarning(SelectDropdown.value);
        string motor_name = SelectDropdown.options[SelectDropdown.value].text;
        Debug.LogWarning(motor_name);

        try
        {
            int motorRotateDegree = (int)MotorRotateDegreeSlider.value;
            int motorSpeed = (int)MotorSpeedSlider.value;

            Nuwa.NuwaMotorType type = (Nuwa.NuwaMotorType)Enum.Parse(typeof(Nuwa.NuwaMotorType), motor_name);
            Debug.LogWarning(type + ", " + (int)type);
            //control motor
            Nuwa.setMotorPositionInDegree((int)type, motorRotateDegree, motorSpeed);
            SetInfoText(true);
        }
        catch (Exception e)
        {
            Debug.LogWarning("inputField cound null, exception :" + e.ToString() );
        }
    }

	private void OnSliderValueChange(float value)
    {
        SetInfoText();
    }

    private void SetInfoText(bool isPlayMotor = false)
    {
        InfoText2.text =
            "Set"+
            "\nMotorDegree:" + (int)MotorRotateDegreeSlider.value
           + "\nMotorSpeed:" + (int)MotorSpeedSlider.value;

        if (isPlayMotor)
            InfoText2.text = "\n Play!";
    }

}
