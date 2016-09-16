using UnityEngine;
using System.Collections;
using System;

public class LineMoving : MonoBehaviour {

    public bool isMoving = false;
    float step = 0.1f;
    RectTransform image;
	// Use this for initialization
	void Start () {
        image = GetComponent<RectTransform>();

    }
	
	// Update is called once per frame
	void Update() {
        if (isMoving) Move();
	}

    private void Move()
    {
        image.position += Vector3.up*step;
        if (image.position.y < -Screen.height / 2) {
            image.position =  new Vector3(0,- Screen.height / 2,0);
            step = -step;
        }
        if (image.position.y > Screen.height / 2)
        {
            image.position = new Vector3(0, Screen.height / 2, 0);
            step = -step;
        }
    }

    public void StartMoving() {
        isMoving = true;
    }

    public void StopMoving() {
        isMoving = false;
    }
}
