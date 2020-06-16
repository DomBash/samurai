using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    private int spriteNum = 0;
    private float rotationSpeed;
    public float moveSpeed = 1f;
    public bool middle = false;

    void Start()
    {
        rotationSpeed = Random.Range(30, 60);
        SetSprite();
    }


    void Update()
    {
        /*if (Input.GetButtonDown("Fire2"))
            SetSprite();
            */
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); //rotates 50 degrees per second around z axis
        transform.Translate(-Time.deltaTime * moveSpeed, 0f, 0f, Space.World);

        if (transform.position.x <= -10)
        {
            if (!middle)
                ResetMeteor();
            else
                Destroy(gameObject);
        }

    }

    void ResetMeteor()
    {
        if(transform.position.y > 4)
        {
            transform.position = new Vector2(12f, 4.5f);
        }
        else
        {
            transform.position = new Vector2(12f, -4.5f);
        }
    }

    void SetSprite()
    {
        transform.GetChild(spriteNum).gameObject.SetActive(false);
        spriteNum = Random.Range(0, 6);
        transform.GetChild(spriteNum).gameObject.SetActive(true);
    }
}
