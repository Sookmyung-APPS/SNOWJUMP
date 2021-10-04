using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    private Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");

        rigidbody.AddForce(Vector2.right * hor, ForceMode2D.Impulse);

        if (rigidbody.velocity.x > maxSpeed)
            rigidbody.velocity = new Vector2(maxSpeed, rigidbody.velocity.y);

        else if (rigidbody.velocity.x < maxSpeed * (-1))
            rigidbody.velocity = new Vector2(maxSpeed *(-1), rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump"))
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
}
