using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FaceDeteh : MonoBehaviour {

    
    public Text originPos;
    public Text originSize;
    public Text faceCenter;
    public FaceTrackSystem faceTrackSystem;

    private Text maxCenter;
    private Text minCenter;

    public Image target;
    public Vector2 pos;
    public Vector2 fixPos;
    public void Update()
    {
        if (faceTrackSystem != null)
        {
            originPos.text = "Oringin Pos = " + faceTrackSystem.FaceOriginPos.ToString();
            originSize.text = "Origin Size = " + faceTrackSystem.FaceOriginSize.ToString();
            faceCenter.text = "Face Center  = " + faceTrackSystem.FaceCenterPos.ToString();

            //screen size / camera size = 1024/600 = 1.6f
            float newX = (640f - faceTrackSystem.FaceCenterPos.x) * 1.6f;
            float newY = (480f - faceTrackSystem.FaceCenterPos.y) * 1.6f;
            var t = new Vector2(newX, newY);

            target.rectTransform.DOAnchorPos(t, 0.1f);
            target.rectTransform.DOSizeDelta(faceTrackSystem.FaceOriginSize, 0.1f);
        }
    }

}
