using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    public float speed = 5f;
    private float step;
    private GameObject soulHolder;
    private Vector3 targetPos;
    private float orbitDist = 3f;

    public Transform system;
    private SystemsController systemScript;

    void Start()
    {
        system = GameObject.Find("Systems").transform;
        systemScript = system.GetComponent<SystemsController>(); 
        soulHolder = GameObject.Find("Soul Holder");

        targetPos = soulHolder.transform.position;
        step = speed * Time.deltaTime; // calculate distance to move
        transform.LookAt(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (systemScript.GetIsPlayerPowered())
        {
            targetPos = soulHolder.transform.position;
            if (Vector3.Distance(transform.position, targetPos) < orbitDist - 1.5f)
            {
                transform.RotateAround(targetPos, Vector3.up, 60 * Time.deltaTime);
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPos) < orbitDist)
            {
                transform.RotateAround(targetPos, Vector3.up, 60 * Time.deltaTime);
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }
}
