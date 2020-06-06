using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTouch : MonoBehaviour
{
    public Transform soulHolder;
    public GameObject finalAttackEffect;
    public Transform system;
    private SystemsController systemScript;

    void Start()
    {
        systemScript = system.GetComponent<SystemsController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            //soulHolder.position = player.position;
        if (systemScript.isDead)
            StopAllCoroutines();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (systemScript.GetIsReadyForNextRound())
            {               
                systemScript.StartNextRound();
            }
        }
    }

    IEnumerator DelaySoulMove()
    {
        yield return new WaitForSeconds(0.2f);
        systemScript.SetIsPlayerPowered(true);
    }
}