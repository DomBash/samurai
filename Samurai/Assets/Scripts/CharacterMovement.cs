using System.Collections;
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

    public Transform swordEnd;

    private int dodgeTime = 0;

    private LineRenderer lr;

    public Text deathText;

    public TrailRenderer swordTrail;
    public TrailRenderer swordTrailHeavy1;
    public TrailRenderer swordTrailHeavy2;
    public TrailRenderer swordTrailHeavy3;
    public TrailRenderer swordTrailHeavy4;

    public Transform heavyTrail1;
    public Transform heavyTrail2;
    public Transform heavyTrail3;
    public Transform heavyTrail4;

    public Transform tree;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 noMovement = Vector3.zero;
    private Vector3 origin = Vector3.zero;
    private Vector3 dashDirection;

    public bool canMove = true;
    private bool canStartLA = true;
    private bool canStartHA = true;
    

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
        speed = 6.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            systemScript.Dead(false);//Not treeDeath
        }
        else if (other.tag == "Spike")
            systemScript.Dead(false);//Not treeDeath
        else if (other.tag == "Slam")
            systemScript.Dead(false);//Not treeDeath

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

        if (!systemScript.GetIsPaused() && !systemScript.GetIsTouchingTree())
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
                    endLA();
                    endHA();

                    systemScript.SetDashAnim(true);

                    systemScript.PlayDashAudio();

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
                        canStartLA = true;
                        systemScript.SetLAAnim(true);
                        systemScript.PlaySword1Audio();
                        nextLATime = Time.time + 1f / laRate;
                        return;
                    }

                    //if (Input.GetMouseButtonDown(1))//Heavy Attack
                    if (Input.GetButtonDown("Fire2") && Time.time >= nextHATime)//Heavy Attack
                    {
                        canStartHA = true;
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
        if (canStartLA)
        {
            systemScript.SetIsLA(true);
            systemScript.SetIsWaitingForHit(true);
            swordTrail.emitting = true;
        }
    }

    void endLA()
    {
        canStartLA = false;
        systemScript.SetIsWaitingForHit(false);
        swordTrail.emitting = false;
        systemScript.SetLAAnim(false);
        systemScript.SetIsLA(false);
    }

    void startHA()
    {
        if (canStartHA)
        {
            systemScript.SetIsHA(true);
            systemScript.SetIsWaitingForHit(true);
            StartHeavyTrail();
        }
       
    }

    void endHA()
    {
        canStartHA = false;
        systemScript.SetIsHA(false);
        systemScript.SetIsWaitingForHit(false);
        EndHeavyTrail();

        systemScript.SetHAAnim(false);

    }

    void StartHeavyTrail()
    {
        heavyTrail1.localPosition = new Vector3(-0.0035f, 0.0255f, 0.0022f);
        heavyTrail2.localPosition = new Vector3(0.0052f, 0.0222f, -0.0031f);
        heavyTrail3.localPosition = new Vector3(0.0037f, 0.0249f, 0.0052f);
        heavyTrail4.localPosition = new Vector3(-0.0037f, 0.0232f, -0.0058f);

        swordTrailHeavy1.emitting = true;
        swordTrailHeavy2.emitting = true;
        swordTrailHeavy3.emitting = true;
        swordTrailHeavy4.emitting = true;
    }

    void EndHeavyTrail()
    {
        heavyTrail1.localPosition = new Vector3(-0.0014f, 0.0491f, -0.0005f);
        heavyTrail2.localPosition = new Vector3(-0.0014f, 0.0491f, -0.0005f);
        heavyTrail3.localPosition = new Vector3(-0.0014f, 0.0491f, -0.0005f);
        heavyTrail4.localPosition = new Vector3(-0.0014f, 0.0491f, -0.0005f);

        swordTrailHeavy1.emitting = false;
        swordTrailHeavy2.emitting = false;
        swordTrailHeavy3.emitting = false;
        swordTrailHeavy4.emitting = false;
    }

    void MovementAbilityOff()
    {
        canMove = false;
    }

    void MovementAbilityOn()
    {
        systemScript.SetIsTouchingTree(false);
        canMove = true;
    }

    IEnumerator finalAttack()
    {
        if (systemScript.GetNumSouls() > 0)
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

            int enemies = systemScript.GetNumSouls();
            for (int i = 0; i < enemies; i++)
            {
                print("1");

                lr.positionCount += 1;
                EnemyController enemyScript = enemy.GetComponent<EnemyController>();

                Vector3 deltaVector = enemy.transform.position - transform.position;
                Vector3 enemyPos = enemy.transform.position + deltaVector.normalized * 2;
                enemyPos.y = 0.6f;
                transform.position = enemyPos;
                lr.SetPosition(i + 1, transform.position);

                transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
                transform.rotation *= Quaternion.Euler(0, -90, 0);

                yield return new WaitForSeconds(0.2f);
                systemScript.SetFinalAttackAnim(true);
                systemScript.PlaySword1Audio();
                yield return new WaitForSeconds(0.2f);
                enemyScript.Die(false);
                GameObject soul = GameObject.Find("Soul(Clone)");
                Destroy(soul);
                systemScript.SetNumSouls(systemScript.GetNumSouls() - 1);
                yield return new WaitForSeconds(0.1f);

                enemy = GameObject.Find("Enemy(Clone)");
                if (enemy == null || systemScript.GetNumSouls() < 1)
                {
                    print("2");
                    transform.position = origin;
                    lr.positionCount = 1;
                    yield return new WaitForSeconds(0.5f);

                    systemScript.SetIsFinalAttacking(false);
                    yield break;
                }
            }
        }
        
        
    }

}
