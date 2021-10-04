using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    float speed = 0.03f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float ofsX = speed * Time.time;

        transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(ofsX, 0);
    }
}
