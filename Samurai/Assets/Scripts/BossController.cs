using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator animator;

    public Transform spikePrefab;
    public Transform tickPrefab;
    public GameObject spikeHolder;
    public GameObject bossHealth;

    public GameObject system;
    private SystemsController systemScript;

    private float attackRange = 2f;
    public float speed = 0.8f;
    public float step;
    private float spikeRate = 0.1f;
    private float nextSpikeTime = 0f;

    private int health = 30;

    private Vector3 targetPos;

    private bool inCollision;
    private bool isAttacking = false;

    private Material particles;
    private Color defaultColor;

    private List<Transform> healthTicks = new List<Transform>();
    
    void Start()
    {
        spikeHolder = GameObject.Find("Spike Holder");
        bossHealth = GameObject.Find("Boss Health");
        system = GameObject.Find("Systems");

        particles = gameObject.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
        defaultColor = particles.color;

        systemScript = system.GetComponent<SystemsController>();

        step = speed * Time.deltaTime;

        var setJ = 20;
        for(int i = 0; i < 2; i++)
        {
            for (int j = 0; j < setJ; j++)
            {
                var newTick = Instantiate(tickPrefab, new Vector3(15f + (10 * j), -20f + (-20 * i)), Quaternion.identity);
                newTick.transform.SetParent(bossHealth.transform, false);
                healthTicks.Add(newTick);
            }
            setJ = 10;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            animator.SetBool("Attack", true);

        if (!systemScript.GetIsFinalAttacking() && !systemScript.GetIsPaused() && !isAttacking)
        {
            var playerDist = Vector3.Distance(systemScript.GetPlayerTransform().position, transform.position);
            targetPos = new Vector3(systemScript.GetPlayerTransform().position.x, 6.68f, systemScript.GetPlayerTransform().position.z);

            if (playerDist <= attackRange) //Aggro and in attack range of player
            {
                print("in melle range");
                //systemScript.PlayEnemyAttackAudio();
                //animator.SetBool("Attack", true);
                //return;
            }
            else if (Time.time >= nextSpikeTime)
            {
                StartCoroutine(RangedAttack());
                nextSpikeTime = Time.time + 1f / spikeRate;
            }

            if (!systemScript.GetIsFinalAttacking())
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);       
                transform.LookAt(targetPos);
            }
        }
    }

    IEnumerator SpikeAttack()
    {
        var newSpike = Instantiate(spikePrefab, systemScript.GetPlayerTransform().position, Quaternion.identity);
        newSpike.transform.parent = spikeHolder.transform;
        yield return new WaitForSeconds(0.4f);
        newSpike.GetChild(0).gameObject.SetActive(true);
        newSpike.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Destroy(newSpike.gameObject);
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
        if ((systemScript.GetIsLA() || systemScript.GetIsHA()) && inCollision)
        {
            //print(source);
            inCollision = false;
            StartCoroutine(HitColorChange());
            if (systemScript.GetIsLA())
            {
                health -= 1;
                DestroyTicks(1);
            }
            if (systemScript.GetIsHA())
            {
                health -= 3;
                DestroyTicks(3);

            }

            if (health <= 0)
            {
                particles.color = Color.red;
                Die();
            }
        }
    }

    IEnumerator HitColorChange()
    {
        particles.color = Color.red;
        yield return new WaitForSeconds(0.02f);
        particles.color = defaultColor;
    }

    IEnumerator RangedAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);

        //animator.SetBool("Summon", true);
        SummonAttack();
    }

    void SummonAttack()
    {
        print("2 new enemys spawned");
        //spawn enemy random on circle * 2
        isAttacking = false;
    }

    private void Die()
    {
        print("boss died");
        //death anim
        //death sound
        //you win = true
    }

    private void DestroyTicks(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Destroy(healthTicks[healthTicks.Count - 1].gameObject);
            healthTicks.Remove(healthTicks[healthTicks.Count - 1]);
        }
    }
}
