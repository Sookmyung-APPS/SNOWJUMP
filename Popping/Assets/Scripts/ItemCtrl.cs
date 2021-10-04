using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCtrl : MonoBehaviour {

    public Transform txtScore;      // 프리팹 

    //-------------------
    // 화면을 벗어난 Gift 제거
    //-------------------
    void Update()
    {
        // World 좌표를 Screen 좌표로 변환 
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        if (view.y < -50)
        {
            Destroy(gameObject);
        }
    }
    //-------------------
    // 점수 표시 - 외부 호출 
    //-------------------
    void DisplayScore(int n)
    {
       
        // 점수 표시용 UIText 만들기 
        Transform obj = Instantiate(txtScore, transform.position, Quaternion.identity) as Transform;
        obj.GetComponent<Text>().text = "+" + n * 500;

        // World 좌표를 Viewport 좌표로 변환 
        var pos = Camera.main.WorldToViewportPoint(transform.position);
        // obj.position = pos;

        // Gift 제거 
        Destroy(gameObject);
    }
}
