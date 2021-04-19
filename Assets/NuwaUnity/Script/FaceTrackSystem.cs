using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTrackSystem : MonoBehaviour {

    public bool isRecognization = false;
    public bool initRecog = false;
    

    public Vector2 FaceOriginPos;
    public Vector2 FaceCenterPos;
    public Vector2 FaceOriginSize;
    public Vector2 FaceFixedPos;


    private void Awake()
    {
        Nuwa.init();
        isRecognization = false;
    }

    public void StartRecognize()
    {
        initRecog = true;
        //step2
        Nuwa.startRecognition(Nuwa.NuwaRecognition.FACE_TRACK);
    }
    public void StopRecognize()
    {
        if(isRecognization)
        Nuwa.stopRecognition();
    }

    public void ReturnToMenu()
    {
        if (initRecog) return;

        UnRegisterProcess();
        UnityEngine.SceneManagement.SceneManager.LoadScene(ESceneConfig.Demo_Title.ToString());
    }

    private void UnRegisterProcess()
    {
        if (initRecog) return;

        StopRecognize();
    }

    void GetTrackData(Nuwa.TrackData[] data)
    {
        Debug.Log("GetTrackData");
        if (initRecog) initRecog = false;

        if (data == null)
        {
            isRecognization = false;
            FaceOriginPos = Vector2.zero;
            FaceOriginSize = Vector2.zero;
        }
        else
        {
            //step3
            isRecognization = true;
            //only get frist data
            float _x  = float.Parse(data[0].x); 
            float _y = float.Parse(data[0].y);
            float _w = float.Parse(data[0].width);
            float _h = float.Parse(data[0].height);

            FaceOriginPos = new Vector2(_x, _y); // set face pos
            FaceOriginSize = new Vector2(_w, _h); // set face size

            FaceCenterPos = FaceOriginPos + (FaceOriginSize / 2f);
        }

    }

    private void OnEnable()
    {
        //step1
        Nuwa.onTrack += GetTrackData;
    }

    private void OnDisable()
    {
        StopRecognize();
        Nuwa.onTrack -= GetTrackData;
    }
    
}
