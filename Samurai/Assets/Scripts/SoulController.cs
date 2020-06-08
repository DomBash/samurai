using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    private Transform tree;
    private Vector3 treePos;
    public float speed = 5f;
    private float step;
    private GameObject soulHolder;
    private GameObject playerSoulHolder;
    private Vector3 targetPos;
    private Transform player;
    private CharacterMovement playerScript;
    private float orbitDist = 3;
    private bool isOnPlayer = false;

    public Transform system;
    private SystemsController systemScript;

    void Start()
    {
        system = GameObject.Find("Systems").transform;
        systemScript = system.GetComponent<SystemsController>();

        player = GameObject.Find("TheRonin").transform;
        playerScript = player.GetComponent<CharacterMovement>();
        orbitDist = 3f;
        soulHolder = GameObject.Find("Soul Holder");
        playerSoulHolder = GameObject.Find("Player Soul Holder");
        targetPos = soulHolder.transform.position;
        step = speed * Time.deltaTime; // calculate distance to move
        transform.LookAt(targetPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (systemScript.GetIsPlayerPowered())
        {
            targetPos = new Vector3(player.position.x, 1.5f, player.position.z);
            orbitDist = 1f;
            transform.parent = playerSoulHolder.transform;
            if (!isOnPlayer)
            {
                transform.position = (transform.position - targetPos).normalized * Random.Range(-orbitDist, orbitDist) + targetPos;
                isOnPlayer = true;
            }
            transform.RotateAround(targetPos, Vector3.up, 120 * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPos) < orbitDist)
            {
                transform.RotateAround(targetPos, Vector3.up, 60 * Time.deltaTime);
                //Destroy(gameObject);
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }
}
