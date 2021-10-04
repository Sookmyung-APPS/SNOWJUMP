using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("flag"))
        {
            Debug.Log("µµÂø");
            AsyncOperation async;
            async = SceneManager.LoadSceneAsync("Win");
        }
    }
}