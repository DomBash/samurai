﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController characterController;
  
    private float gravity = 20.0f;
    private float dashRate = 2f;
    private float nextDashTime = 0f;

    private float laRate = 3f;
    private float nextLATime = 0f;

    private float haRate = 1f;
    private float nextHATime = 0f;

    public float speed = 6.0f;

    public Transform system;
    private SystemsController systemScript;

    private int dodgeTime = 0;

    private LineRenderer lr;

    public Text deathText;

    public TrailRenderer swordTrail;

    public Transform tree;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 noMovement = Vector3.zero;
    private Vector3 origin = Vector3.zero;
    private Vector3 dashDirection;

    public bool canMove = true;
    

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        systemScript = system.GetComponent<SystemsController>();
        lr = GetComponent<LineRenderer>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void RestartGame()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            systemScript.Dead(false);//Not treeDeath
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
        systemScript.SetIsPlayerPowered(false);     
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    public Transform GetPlayerTransform()
    {
        return gameObject.transform;
    }

    void Update()
    {
        if (transform.position.y < 0)
            systemScript.Dead(false);

        if (!systemScript.isPaused)
        {
            if (!systemScript.GetIsFinalAttacking() && !systemScript.GetIsTouchingTree())
            {
                if (dodgeTime > 0)
                {
                    Dodge();
                    return;
                }

                //if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
                if (Input.GetButtonDown("Dash") && Time.time >= nextDashTime)
                {
                    systemScript.SetIsLA(false);
                    systemScript.SetIsHA(false);
                    systemScript.SetLAAnim(false);
                    systemScript.SetHAAnim(false);
                    systemScript.SetDashAnim(true);

                    systemScript.PlayDashAudio();
                    swordTrail.emitting = false;


                    nextDashTime = Time.time + 1f / dashRate;

                    dashDirection = -transform.right;

                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
                    {
                        Vector3 forward = systemScript.GetCamTransform().TransformDirection(Vector3.forward);
                        forward.y = 0;
                        forward = forward.normalized;
                        Vector3 right = new Vector3(forward.z, 0, -forward.x);
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");

                        dashDirection = (h * right + v * forward);
                    }
                    Rotate(dashDirection);

                    moveDirection = (40f * -transform.right);
                    dodgeTime = 5;

                    Dodge();
                    return;
                }


                if (!systemScript.GetIsLA() && !systemScript.GetIsHA() && canMove)
                {

                    //if (Input.GetMouseButtonDown(0) && Time.time >= nextLATime)//Light Attack
                    if (Input.GetButtonDown("Fire1") && Time.time >= nextLATime)//Light Attack
                    {
                        systemScript.SetLAAnim(true);
                        systemScript.PlaySword1Audio();
                        nextLATime = Time.time + 1f / laRate;
                        return;
                    }

                    //if (Input.GetMouseButtonDown(1))//Heavy Attack
                    if (Input.GetButtonDown("Fire2") && Time.time >= nextHATime)//Heavy Attack
                    {
                        systemScript.SetHAAnim(true);
                        systemScript.PlayHeavyAudio();
                        nextHATime = Time.time + 1f / haRate;
                        return;
                    }
                    //if (Input.GetKeyDown(KeyCode.E) && spawnScript.canUseSpecial)
                    if (Input.GetButtonDown("Fire3") && systemScript.GetCanUseSpecial())
                    {
                        StartCoroutine(finalAttack());
                        return;
                    }

                    if (characterController.isGrounded)
                    {
                        Vector3 forward = systemScript.GetCamTransform().TransformDirection(Vector3.forward);
                        forward.y = 0;
                        forward = forward.normalized;
                        Vector3 right = new Vector3(forward.z, 0, -forward.x);
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");

                        moveDirection = (h * right + v * forward);

                        if (moveDirection != Vector3.zero)
                        {
                            Rotate(moveDirection);
                        }
                        if (moveDirection == noMovement)
                        {
                            systemScript.SetWalkAnim(false);                            
                        }
                        else
                        {
                            systemScript.SetWalkAnim(true);
                        }

                    }
                    moveDirection.y -= gravity * Time.deltaTime;
                    characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
                }
            }
        }
    }

    public void TouchTree()
    {
        var lookPos = new Vector3(tree.position.x, transform.position.y, tree.position.z);
        transform.LookAt(lookPos);
        var distance = 1.5f;
        transform.position = (transform.position - tree.position).normalized * distance + tree.position;
        transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
        transform.rotation *= Quaternion.Euler(0, 90, 0);
        systemScript.SetTreeTouchAnim(true);      
    }

    void Rotate(Vector3 lookDirection)
    {
        transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 90, 0); //Rotate to look direction +90 cause model on x axis
    }

    void Dodge()
    {
        characterController.Move(moveDirection * Time.deltaTime);
        dodgeTime -= 1;
    }

    void startLA()
    {
        systemScript.SetIsLA(true);
        systemScript.SetIsWaitingForHit(true);
        swordTrail.emitting = true;
    }

    void endLA()
    {
        systemScript.SetIsLA(false);
        systemScript.SetIsWaitingForHit(false);
        swordTrail.emitting = false;

        systemScript.SetLAAnim(false);
    }

    void startHA()
    {
        systemScript.SetIsHA(true);
        systemScript.SetIsWaitingForHit(true);
    }

    void endHA()
    {
        systemScript.SetIsHA(false);
        systemScript.SetIsWaitingForHit(false);
        systemScript.SetHAAnim(false);

    }

    void MovementAbilityOff()
    {
        canMove = false;
    }

    void MovementAbilityOn()
    {
        canMove = true;
    }

    IEnumerator finalAttack()
    {
        systemScript.SetIsFinalAttacking(true);
        dodgeTime = 0;
        systemScript.SetWalkAnim(false);
        origin = transform.position;
        lr.SetPosition(0, transform.position);

        GameObject enemy = GameObject.Find("Enemy(Clone)");
        if (enemy == null)
        {
            transform.position = origin;
            yield return new WaitForSeconds(0.1f);
            lr.positionCount = 1;
            systemScript.SetIsFinalAttacking(false);
            yield break;
        }
        var enemies = 20;
        for (int i = 0; i < enemies; i++)         
        {
            lr.positionCount += 1;
            EnemyController enemyScript = enemy.GetComponent<EnemyController>();    
            
            Vector3 deltaVector = enemy.transform.position - transform.position;
            Vector3 enemyPos = enemy.transform.position + deltaVector.normalized*2;
            enemyPos.y = 0.6f;
            transform.position = enemyPos;
            lr.SetPosition(i + 1, transform.position);

            transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
            transform.rotation *= Quaternion.Euler(0, -90, 0);

            yield return new WaitForSeconds(0.2f);
            systemScript.SetFinalAttackAnim(true);
            systemScript.PlaySword1Audio();
            yield return new WaitForSeconds(0.2f);
            enemyScript.Die();
            yield return new WaitForSeconds(0.1f);

            enemy = GameObject.Find("Enemy(Clone)");
            if (enemy == null)
            {
                transform.position = origin;
                lr.positionCount = 1;
                yield return new WaitForSeconds(0.5f);

                systemScript.SetIsFinalAttacking(false);
                yield break;
            }
        }
        
    }

}
