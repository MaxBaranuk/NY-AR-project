using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class WebWievInitialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AndroidJavaClass ajc = new
  AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ajo = ajc.GetStatic<AndroidJavaObject>("com.onevcat.uniwebview.AndroidPlugin");
        var jc = new AndroidJavaClass("com.wear.newyork.UnityPlayerNativeActivity");
        jc.CallStatic("launchWebWievImpl", ajo, true, "", "", "");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }
}
