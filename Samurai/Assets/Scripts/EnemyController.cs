using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private CharacterMovement playerScript;
    private Transform ground;
    private EnemySpawner spawnScript;
    private Transform cam;
    private CameraController camScript;
    private Transform target;
    private Vector3 targetPos;
    private Vector3 newTargetPos;
    public float speed = 1f;
    public Transform soulPrefab;
    public bool aggroed = false;
    public bool isAttacking = false;
    private float attackRange = 3f;
    private bool inCollision = false;
    private GameObject soulHolder;
    public AudioSource attackAudio;


    private float step;
    public int health = 4;

    private Material particles;
    private Color defaultColor;

    public Animator animator;

    private TreeController treeScript;

    public Transform system;
    private SystemsController systemScript;

    void Start()
    {
        target = GameObject.Find("Tree").transform;
        player = GameObject.Find("TheRonin").transform;
        ground = GameObject.Find("Ground").transform;
        cam = GameObject.Find("Main Camera").transform;
        system = GameObject.Find("Systems").transform;
        soulHolder = GameObject.Find("Soul Holder");

        playerScript = player.GetComponent<CharacterMovement>();
        spawnScript = ground.GetComponent<EnemySpawner>();
        treeScript = target.GetComponent<TreeController>();
        camScript = cam.GetComponent<CameraController>();
        systemScript = system.GetComponent<SystemsController>();


        particles = gameObject.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
        defaultColor = particles.color;

        step = speed * Time.deltaTime;
    }


    void Update()
    {
        //print(attackAudio.isPlaying);

        if (!playerScript.isFinalAttacking && !systemScript.isPaused)
        {
            var playerDist = Vector3.Distance(player.transform.position, transform.position);
            var treeDist = Vector3.Distance(target.transform.position, transform.position);

            if (playerDist <= attackRange && aggroed) //Aggro and in attack range of player
            {
                isAttacking = true;
                attackAudio.PlayDelayed(0f);
                animator.SetTrigger("Attack");
                            
                return;
            }

            if (treeDist <= attackRange && !aggroed) //Not aggro and attack range of tree
            {
                isAttacking = true;
                attackAudio.PlayDelayed(0f);
                animator.SetTrigger("Attack");
                
                return;
            }

            if (playerDist < treeDist && !spawnScript.playerAttack) //Player is closer than tree and not being attacked
            {
                targetPos = player.transform.position;
                aggroed = true;
                spawnScript.playerAttack = true;
            }
            else if (playerDist < treeDist && aggroed) //Player is closer and aggroed
            {
                targetPos = player.transform.position;
            }
            else if (treeDist < playerDist && !isAttacking) //Tree is closer and you arent attacking
            {
                targetPos = target.transform.position;
                if (aggroed)
                {
                    spawnScript.playerAttack = false;
                    aggroed = false;
                }
            }

            newTargetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if (!playerScript.isFinalAttacking && !isAttacking)
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
        animator.SetBool("Attack",false);
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
        if ((playerScript.isLA || playerScript.isHA) && inCollision && systemScript.GetIsWaitingForHit())
        {
            //print(source);
            inCollision = false;
            aggroed = true;
            StartCoroutine(HitColorChange());
            if (playerScript.isLA)
                health -= 1;               
            if (playerScript.isHA)
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
        spawnScript.playerAttack = false;
        spawnScript.enemiesToKill -= 1;
        Vector3 position = transform.position;
        var newSoul = Instantiate(soulPrefab, position, Quaternion.identity);
        newSoul.transform.parent = soulHolder.transform;
        Destroy(gameObject);
    }
}
