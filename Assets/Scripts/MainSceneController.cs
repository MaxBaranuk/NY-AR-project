using UnityEngine;
using System.Collections;

public class MainSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
    //    AndroidJavaClass ajc = new
    //AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //    AndroidJavaObject ajo = ajc.GetStatic<AndroidJavaObject>("com.onevcat.uniwebview.AndroidPlugin");
    //    var jc = new AndroidJavaClass("com.unity3d.player.UnityPlayerNativeActivity");
    //    jc.CallStatic("launchScannerImpl", ajo, true, "", "", "");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform.tag == "House") Application.OpenURL("http://tour360.proc.lv/buildings/13");
            }

        }
    }
}
