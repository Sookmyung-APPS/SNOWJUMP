using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyCtrl : MonoBehaviour {

    float speed = 0.03f;

    //------------------------------
    // 화면 스크롤
    //------------------------------
    void Update()
    {
        float ofsX = speed * Time.time;
        transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(ofsX, 0);
    }
}
