using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public UIController ui;
    private Rigidbody2D rb2D;
    private float thrust = 20.0f;

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Meteor")
            FindObjectOfType<SystemController>().GameOver();
    }

    public void Replay()
    {
        rb2D.gravityScale = 0;
        rb2D.velocity = Vector2.zero;
        transform.position = new Vector2(-4f, 0f);
    }

    public void TapToPlay()
    {
        rb2D.gravityScale = 3;
    }
}
