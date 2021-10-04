using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdCtrl : MonoBehaviour {

    public Transform txtScore;  // 프리팹 
    int speed;                  // 이동 속도 
    bool isDrop = false;	// Use this for initialization

    Animator anim;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    void Update()
    {
        speed = Random.Range(3, 7);

        float amtMove = speed * Time.smoothDeltaTime;

        if (!isDrop)
        {
            AnimateBird();
            transform.Translate(Vector3.right * amtMove);
        }
        else
        {           // 아래로 이동 
            transform.Translate(Vector3.down * amtMove, Space.World);
        }

        // 화면을 벗어난 오브젝트 제거 
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        if (view.y < -50 || view.x > Screen.width + 50)
        {
            Destroy(gameObject);
        }

    }

    void AnimateBird()
    {
        anim.Play("BirdFly");
    }

    void DropBird()
    {
        isDrop = true;

        // 참새 회전 
        transform.eulerAngles = new Vector3(0, 0, 180);

        // 감점 표시 
        Transform obj = Instantiate(txtScore) as Transform;
        obj.GetComponent<Text>().text = "<color=red><size=22>-1000</size></color>";

        // World 좌표를 Viewport 좌표로 변환 
        var pos = Camera.main.WorldToViewportPoint(transform.position);
        obj.position = transform.position;
    }
    // Update is called once per frame
}
