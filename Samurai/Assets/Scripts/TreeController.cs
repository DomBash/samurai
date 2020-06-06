using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject system;
    private SystemsController systemScript;

    void Start()
    {
        systemScript = system.GetComponent<SystemsController>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && !other.GetComponentInParent<EnemyController>().aggroed)
        {
            systemScript.Dead(true);
        }
    }
}
