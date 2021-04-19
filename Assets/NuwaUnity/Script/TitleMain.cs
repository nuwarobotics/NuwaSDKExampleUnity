using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMain : MonoBehaviour {


    private GameObject Button;

    public Transform Container;

    private Button[] ButtonArr;

    private void Awake()
    {
        TitleMain[] titles = FindObjectsOfType<TitleMain>();
        if (titles.Length == 2)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        Nuwa.init();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.LogError("scene.name : " + scene.name);
        if(scene.name.Equals("Demo_Title"))
        {
            Container.transform.parent.parent.gameObject.SetActive(true);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (Container.childCount > 0)
        {
            Button = Container.GetChild(0).gameObject;
        }

        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        Scene[] AllScenes = new Scene[sceneCount];
        
        //Debug.Log("AllScene length : " + AllScenes.Length + " , " + sceneCount);
        ButtonArr = new Button[AllScenes.Length];
        //foreach (Scene scene in AllScenes)
        Debug.Log(SceneManager.GetSceneByBuildIndex(1).name + " , all scene count : " + AllScenes.Length);
        for(int idx = 0; idx < AllScenes.Length; idx++)
        {


            Scene scene = AllScenes[idx];
            GameObject go = Instantiate(Button, Vector2.zero, Quaternion.identity, Container);
            Button btn = go.GetComponent<Button>();
            if (btn != null)
            {
                //Debug.Log("idx : " + idx + " , name : " + SceneManager.GetSceneByBuildIndex(idx).name + " , " + SceneUtility.GetScenePathByBuildIndex(idx));
                ButtonArr[idx] = btn; 
                Text txt = btn.GetComponentInChildren<Text>();
                txt.text = SceneUtility.GetScenePathByBuildIndex(idx);
                txt.text = txt.text.Replace("Assets/NuwaUnity/Scene/", "").Replace(".unity", "");

            }
            else
                Debug.LogError("+===Button err");
        }
        Destroy(Button);

        for (int i = 0; i < ButtonArr.Length; i++)
        {
            if(ButtonArr[i] != null)
            {
                Button btn = ButtonArr[i];
                btn.onClick.AddListener(delegate { OnButtonClickEvent(btn); });
            }
        }
	}


    private void OnButtonClickEvent(Button button)
    {
        Container.transform.parent.parent.gameObject.SetActive(false);
        int idx = button.transform.GetSiblingIndex();
        string text = button.GetComponentInChildren<Text>().text;
        Debug.Log(idx +",Scene:"+ text);
        SceneManager.LoadScene(text);
    }


}

public enum ESceneConfig
{
    Demo_Title,
    Demo_LED,
    Demo_LocalCommand,
    Demo_Speed2Text,
    Demo_Motor,
    Demo_touch,
    Demo_motion_play,
    Demo_tts,
    Demo_Recognize,
    Demo_Facetrack,
    Demo_movement,
    Demo_FaceDetect,
}
