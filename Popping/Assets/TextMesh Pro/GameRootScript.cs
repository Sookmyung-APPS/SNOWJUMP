using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRootScript : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clickSound;

    // Start is called before the first frame update
    void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();

        this.audio.clip = this.clickSound;
        this.audio.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.audio.Play();
        }
    }
}
