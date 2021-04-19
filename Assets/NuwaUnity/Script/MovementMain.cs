using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MovementMain : MonoBehaviour {

    //[Range(-0.2f, 0.2f)]
    public Slider MoveSlider;
    public Text MoveSliderText;

    //[Range(-20, 20)]
    public Slider TurnSlider;
    public Text TurnSliderText;

    public Text LockWheelText;
    private string mLockWheelText = "LockWheel:";

	// Use this for initialization
	void Start () {
        Nuwa.init();

        MoveSlider.onValueChanged.AddListener(
            delegate (float value) { MoveSliderText.text = value.ToString(); }
            );

        TurnSlider.onValueChanged.AddListener(
            delegate (float value) { TurnSliderText.text = value.ToString(); }
            );

        LockWheel();
    }




    #region Move
    public void OnMoveButtonClick()
    {
        Debug.Log("OnMoveButtonClick, value:"+MoveSlider.value);
        Nuwa.SetMove(MoveSlider.value);
    }

    public void OnStopMoveButtonClick()
    {
        Nuwa.SetMove(0);
    }

    public void MoveForwardInAccelerationEx()
    {
        Nuwa.MoveForwardInAccelerationEx();
    }

    public void MoveBackInAccelerationEx()
    {
        Nuwa.MoveBackInAccelerationEx();
    }

    public void StopInAcclerationEx()
    {
        Nuwa.StopInAcclerationEx();
    }

    #endregion Move

    #region Turn


    public void OnTurnButtonClick()
    {
        Debug.Log("OnTurnButtonClick, value:" + TurnSlider.value);
        Nuwa.SetTurn(TurnSlider.value);
    }


    public void OnStopTurnButtonClick()
    {
        Nuwa.SetTurn(0);
    }

    public void TurnLeftEx()
    {
        Nuwa.TurnLeftEx();
    }

    public void TurnRightEx()
    {
        Nuwa.TurnRightEx();
    }

    public void StopTurnEx()
    {
        Nuwa.StopTurnEx();
    }

    #endregion turn


    #region Wheel

    public void LockWheel()
    {
        Nuwa.LockWheel();
        LockWheelText.text = mLockWheelText + "true";
    }

    public void UnlockWheel()
    {
        Nuwa.UnlockWheel();
        LockWheelText.text = mLockWheelText + "false";
    }


    #endregion Wheel

    public void OnReturnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());

        //stop move
        Nuwa.SetMove(0);
        Nuwa.StopInAcclerationEx();
        Nuwa.SetTurn(0);
        Nuwa.StopTurnEx();
    }
}
