using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 targetPos;
    private Vector3 newTargetPos;
    public float speed = 1f;
    public Transform soulPrefab;
    public bool aggroed = false;
    public bool isAttacking = false;
    private float attackRange = 3f;
    private bool inCollision = false;
    private GameObject soulHolder;

    private float step;
    public int health = 4;

    public Animator animator;

    private Material particles;
    private Color defaultColor;

    private Transform system;
    private SystemsController systemScript;

    void Start()
    {
        system = GameObject.Find("Systems").transform;
        soulHolder = GameObject.Find("Soul Holder");
        systemScript = system.GetComponent<SystemsController>();
        particles = gameObject.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
        defaultColor = particles.color;
        step = speed * Time.deltaTime;
    }


    void Update()
    {
        //print(attackAudio.isPlaying);

        if (!systemScript.GetIsFinalAttacking() && !systemScript.isPaused)
        {
            var playerDist = Vector3.Distance(systemScript.GetPlayerTransform().position, transform.position);
            var treeDist = Vector3.Distance(Vector3.zero, transform.position);

            if (playerDist <= attackRange && aggroed) //Aggro and in attack range of player
            {
                isAttacking = true;
                systemScript.PlayEnemyAttackAudio();
                animator.SetBool("Attack", true);
                return;
            }

            if (treeDist <= attackRange && !aggroed) //Not aggro and attack range of tree
            {
                isAttacking = true;
                systemScript.PlayEnemyAttackAudio();
                animator.SetBool("Attack", true);
                return;
            }

            if (playerDist < treeDist && !systemScript.GetIsBeingAttacked()) //Player is closer than tree and not being attacked
            {
                targetPos = systemScript.GetPlayerTransform().position;
                aggroed = true;
                systemScript.SetIsBeingAttacked(true);
            }
            else if (playerDist < treeDist && aggroed) //Player is closer and aggroed
            {
                targetPos = systemScript.GetPlayerTransform().position;
            }
            else if (treeDist < playerDist && !isAttacking) //Tree is closer and you arent attacking
            {
                targetPos = Vector3.zero;
                if (aggroed)
                {
                    systemScript.SetIsBeingAttacked(false);
                    aggroed = false;
                }
            }

            newTargetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if (!systemScript.GetIsFinalAttacking() && !isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, newTargetPos, step);
                transform.LookAt(newTargetPos);
            }


            if (Vector3.Distance(transform.position, newTargetPos) < 0.5f)
            {
                //UnityEditor.EditorApplication.isPlaying = false;
            }
        }
        
    }



    void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inCollision = true;
            GetHurt(1);
        }
    }

    void OnTriggerStay()
    {
        //print("here");
        GetHurt(0);
    }

    void GetHurt(int source)
    {
        if ((systemScript.GetIsLA() || systemScript.GetIsHA()) && inCollision && systemScript.GetIsWaitingForHit())
        {
            //print(source);
            inCollision = false;
            aggroed = true;
            StartCoroutine(HitColorChange());
            if (systemScript.GetIsLA())
                health -= 1;               
            if (systemScript.GetIsHA())
                health -= 2;

            if (health <= 0)
            {
                particles.color = Color.red;
                Die();
            }
            print("Enemy health: " + health);
            systemScript.SetIsWaitingForHit(false);
        }
    }

    IEnumerator HitColorChange()
    {
        particles.color = Color.red;
        yield return new WaitForSeconds(0.02f);
        particles.color = defaultColor;
    }

    public void Die()
    {
        systemScript.SetIsBeingAttacked(false);

        var newSoul = Instantiate(soulPrefab, transform.position, Quaternion.identity);
        newSoul.transform.parent = soulHolder.transform;

        systemScript.EnemyDeath();
        Destroy(gameObject);
    }
}
