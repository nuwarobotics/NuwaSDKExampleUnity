using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMain : MonoBehaviour
{
    private List<RemoteDevice> m_RemoteDeviceList = new List<RemoteDevice>();
    private List<RemoteDevice> notAddedDevice = new List<RemoteDevice>();
    public Button m_ReturnButton;
    public Button m_BaseButton;
    private InputField m_InputField;

    private void Start()
    {
        //step1 : Create ConnectionManager
        NuwaConnection.init();
        m_ReturnButton.onClick.AddListener(OnReturnButtonClick);
        m_InputField = FindObjectOfType<InputField>();

        //step1 : register event
        NuwaConnection.OnReceiveScanResultEvent += OnReceiveScanResult;
        NuwaConnection.OnReceiveConnectionResultEvent += OnReceiveConnectionResult;

        //tts
        Debug.Log("add onTTSComplete event");
        Nuwa.onTTSComplete += OnTTsComplete;
    }

    public void OnTTsComplete(bool isError)
    {
        Debug.LogFormat("DeviceName {1} : onTTSComplete , isError = {0}", isError, SystemInfo.deviceName);
    }

    #region Button Event


    public void Init()
    {
        NuwaConnection.CreateConnectionManager();
    }

    public void Scan()
    {
        //step2 : Find all the robots in wifi
        NuwaConnection.StartScan();
    }

    public void StopScan()
    {
        NuwaConnection.StopScan();
    }

    public void DisConnect()
    {
        NuwaConnection.Disconnect();
    }

    public void PlayTTs()
    {
        //if you connect robot, you can play tts on Robot.
        string msg = m_InputField.textComponent.text;
        Nuwa.startTTS(msg);
    }

    public void PlayMotion()
    {
        Nuwa.motionPlay("666_DA_Sleep");
    }

    public void Open_LED()
    {
        Nuwa.disableSystemLED();
        Nuwa.setLedColor(Nuwa.LEDPosition.Chest, Color.red);
        Nuwa.enableLed(Nuwa.LEDPosition.Chest, true);
    }

    public void Close_LED()
    {
        Nuwa.enableLed(Nuwa.LEDPosition.Chest, false);
        Nuwa.enableSystemLED(); 
    }
    #endregion

    private void OnDestroy()
    {
        NuwaConnection.StopScan();
        NuwaConnection.Disconnect();
    }

    public void OnReceiveScanResult(string value)
    {
        RemoteDevice device = JsonUtility.FromJson<RemoteDevice>(value);
        Debug.Log("OnReceiveResult, value : " + value + " , device == null : " + (device == null) + " , " + (device == null ? " , " : device.name));
        if (!m_RemoteDeviceList.Contains(device) && !notAddedDevice.Contains(device))
        {
            notAddedDevice.Add(device);
            m_RemoteDeviceList.Add(device);
        }
    }

    private void Update()
    {
        if(notAddedDevice.Count > 0)
        {
            for(int i = 0; i < notAddedDevice.Count; i++)
            {
                RemoteDevice device = notAddedDevice[i];
                Button btn = Instantiate(m_BaseButton, m_BaseButton.transform.parent);
                btn.gameObject.SetActive(true);
                btn.onClick.AddListener(delegate { OnStartConnect(device); });
                btn.GetComponentInChildren<Text>().text = device.name + " : " + device.address;
            }
            notAddedDevice.Clear();
        }
    }

    private RemoteDevice mCurrentRemoteDevice;
    public void OnStartConnect(RemoteDevice device)
    {
        //step3 : Select the robot and connect.
        NuwaConnection.StartConnect(device.name, device.address);
        mCurrentRemoteDevice = device;
    }

    public void OnReceiveConnectionResult(NuwaConnection.EConnectResult eConnectResult)
    {
        //step4 : Get connect result.
        Debug.Log("ConnectionMain OnConnectionResult");

        Transform connectionListParent = m_BaseButton.transform.parent;
        int connectionListCount = connectionListParent.childCount;

        if (eConnectResult == NuwaConnection.EConnectResult.Connected)
        {
            string deviceText = mCurrentRemoteDevice.name + " : " + mCurrentRemoteDevice.address;
            for (int i = 1; i < connectionListCount; i++)
            {
                Button btn = connectionListParent.GetChild(i).gameObject.GetComponent<Button>();
                string text = btn.GetComponentInChildren<Text>().text;
                btn.interactable = text.Equals(deviceText);
            }
        }
        else if (eConnectResult == NuwaConnection.EConnectResult.Disconnected)
        {
            mCurrentRemoteDevice = null;
            for (int i = 1; i < connectionListCount; i++)
            {
                connectionListParent.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            Debug.LogError("Connection error !");
        }
    }

    public void OnReturnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }
}

