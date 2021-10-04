using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour {
    GameObject canvasText;

    private void Awake()
    {
        canvasText = GameObject.Find("Main Camera/Canvas");
        transform.SetParent(canvasText.transform);
    }

    //-------------------
    // 게임 초기화 
    //-------------------
    void Start()
    {
        StartCoroutine("DisplayScore");
    }

    //-------------------
    // 게임 루프 
    //-------------------
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y += 0.01f;
        transform.position = pos;
    }
    //-------------------
    // 점수를 투명하게 처리 
    //-------------------
    IEnumerator DisplayScore()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
