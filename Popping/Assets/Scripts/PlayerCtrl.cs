﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
    public Text tScore;          //점수,높이 ,아이템갯수 
    public Text tHeight;
    public Text tItem;

    public Transform tile;        // 프리팹 
    public Transform[] item;
    public Transform bird;

  
    public Transform startPos;
    public Transform endPos;

    public AudioClip sndJump;       // 효과음 및 배경음악 
    public AudioClip sndGift;
    public AudioClip sndBird;
    public AudioClip sndStage;
    public AudioClip sndOver;

    Transform spPoint;
    Transform newTile;
    float maxY = 0;
    int giftCnt = 0;                // 획득한 선물 수 
    int score = 0;                // 득점 

    int speedSide = 10;             // 좌우 이동 속도 
    int speedJump = 16;             // 점프 속도 
    int gravity = 25;               // 추락 속도 

    Vector3 moveDir = Vector3.zero;
 
    bool isDead = false;
    Animator anim;
    public Button retryGame;


    void Start()
    {
        anim = GetComponent<Animator>();

        // 모바일 단말기 설정
        Screen.orientation = ScreenOrientation.LandscapeRight;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        spPoint = GameObject.Find("spPoint").transform;

        // Tile 만들기 
        newTile = Instantiate(tile, spPoint.position, spPoint.rotation) as Transform;

        //Cursor.visible = false;     // 커서 감추기 
        retryGame.gameObject.SetActive(false);
    }

    //-------------------
    // 게임 루프
    //-------------------
    void Update()
    {
        if (isDead) return;
        JumpPlayer();          // Player 점프
        MovePlayer();          // Player 이동 
        MoveCamera();          // 카메라 이동 
        MakeItem();
        UIText();
    }


    //-------------------
    // Player 점프
    //-------------------
    void JumpPlayer()
    {
        RaycastHit2D hit;
        hit = Physics2D.Linecast(startPos.position, endPos.position, 1 << LayerMask.NameToLayer("Tile"));

        if (hit)
        {
            moveDir.y = speedJump;
            AudioSource.PlayClipAtPoint(sndJump, transform.position);
        }
    }

    //-------------------
    //  Player 이동
    //-------------------
    void MovePlayer()
    {
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);

        if (view.y < -50)
        {       // 화면 아래를 벗어나면  
            isDead = true;      // 게임 오버 
            GameOver();
            return;
        }

        moveDir.x = 0;      // Player의 좌우 이동 방향

        // Mobile 처리 
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // 중력 가속도 센서 읽기 
            float x = Input.acceleration.x;

            // 왼쪽으로 기울였나?
            if (x < -0.2f && view.x > 35)
            {
                moveDir.x = 2 * x * speedSide;
            }

            if (x > 0.2f && view.x < Screen.width - 35)
            {
                moveDir.x = 2 * x * speedSide;
            }

        }
        else
        {   // Keyboard 읽기 
            float key = Input.GetAxis("Horizontal");
            if ((key < 0 && view.x > 35) ||
                (key > 0 && view.x < Screen.width - 35))
            {
                moveDir.x = key * speedSide;
            }
        }

        // 매 프레임마다 점프 속도 감소
        moveDir.y -= gravity * Time.deltaTime;
        transform.Translate(moveDir * Time.smoothDeltaTime);


        //애니메이션 설정
        if (moveDir.y > 0)
        {
            anim.Play("PlayerJump");
        }
        else
        {
            anim.Play("PlayerIdle");
        }
    }

    void MoveCamera()
    {
        // Player 최대 높이 구하기 
        if (transform.position.y > maxY)
        {
            maxY = transform.position.y;

            // 카메라 위치 이동 
            Camera.main.transform.position = new Vector3(0, maxY - 2.5f, -10);
           // score = (int)maxY * 1000;
        }

        // 가장 최근의 Tile과 spPoint와의 거리 구하기
        if (spPoint.position.y - newTile.position.y >= 4)
        {
            float x = Random.Range(-10f, 10f) * 0.5f;
            Vector3 pos = new Vector3(x, spPoint.position.y, 0.3f);
            newTile = Instantiate(tile, pos, Quaternion.identity) as Transform;

            // 나뭇가지의 회전방향 설정 
            int mx = (Random.Range(0, 2) == 0) ? -1 : 1;
            int my = (Random.Range(0, 2) == 0) ? -1 : 1;
            newTile.GetComponent<SpriteRenderer>().material.mainTextureScale = new Vector2(mx, my);
        }
    }
    void MakeItem()
    {
        if (Random.Range(1, 1000) < 990) return;

        // 오브젝트 표시 위치 
        Vector3 pos = Vector3.zero;
        pos.y = maxY + Random.Range(4, 5.5f);

        if (Random.Range(0, 100) < 50)
        {
            // 참새 만들기 
            pos.x = -12f;
            Instantiate(bird, pos, Quaternion.identity);
        }
        else
        {
            for (int i = 1; i < item.Length; i++)
            {
                // 화면의 선물 수를 5개 이내로 제한 
                int n1 = GameObject.FindGameObjectsWithTag("Item1").Length;
                int n2 = GameObject.FindGameObjectsWithTag("Item2").Length;
                int n3 = GameObject.FindGameObjectsWithTag("Item3").Length;

                if (n1 + n2 + n3 >= 5) return;

                // Item 만들기 
                pos.x = Random.Range(-5f, 5f);

                int n = Random.Range(0, 3);
                Transform obj = Instantiate(item[n], pos, Quaternion.identity) as Transform;

            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.transform.tag)
        {
            case "Item1":
                AudioSource.PlayClipAtPoint(sndGift, transform.position);
                // 득점 처리              
                score += 500;
                giftCnt++;

                // 선물 제거 
                coll.transform.SendMessage("DisplayScore", 1);
                break;

            case "Item2":
                AudioSource.PlayClipAtPoint(sndGift, transform.position);
                // 득점 처리 
                score += 1000;
                giftCnt++;

                // 선물 제거 
                coll.transform.SendMessage("DisplayScore", 2);
                break;

            case "Item3":
                AudioSource.PlayClipAtPoint(sndGift, transform.position);
                // 득점 처리 
                score += 1500;
                giftCnt++;

                // 선물 제거 
                coll.transform.SendMessage("DisplayScore", 3);
                break;

            case "Bird":
                if (coll.transform.eulerAngles.z != 0) return;

                AudioSource.PlayClipAtPoint(sndBird, transform.position);
                score -= 1000;

                // 참새 추락 처리 
                coll.transform.SendMessage("DropBird", SendMessageOptions.DontRequireReceiver);
                break;
        }
    }

    void UIText()
    {
        tScore.text = "Score :" + score;
        tHeight.text = "Height :" + (int)maxY;
        tItem.text = "Gift :" + giftCnt;
    }

    //게임 오버 설정
    void GameOver()
    {
         retryGame.gameObject.SetActive(true);
        if (GetComponent<AudioSource>().clip != sndOver)
        {
            GetComponent<AudioSource>().clip = sndOver;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }
    }
    // 재시작 
    public void OnClick()
    {
        SceneManager.LoadScene("Main");
    }

}
