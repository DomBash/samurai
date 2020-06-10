using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator playerAnimator;

    public Transform system;
    private SystemsController systemScript;

    private Coroutine treeTouch;

    void Start()
    {
        systemScript = system.GetComponent<SystemsController>();
    }

    void Update()
    {
        
    }

    public void SetAnimSpeed(float speed)
    {
        playerAnimator.speed = speed;
    }

    public void SetTreeTouchAnim(bool parity)
    {
        playerAnimator.SetBool("TreeTouch", parity);
        if(parity)
            treeTouch = StartCoroutine(TouchTreeLength());
    }

    public void SetLAAnim(bool parity)
    {
        playerAnimator.SetBool("LightAttack", parity);
    }

    public void SetHAAnim(bool parity)
    {
        playerAnimator.SetBool("HeavyAttack", parity);
    }

    public void SetDashAnim(bool parity)
    {
        playerAnimator.SetBool("Dash", parity);
    }

    public void SetWalkAnim(bool parity)
    {
        playerAnimator.SetBool("Walking", parity);
    }

    public void SetFinalAttackAnim(bool parity)
    {
        playerAnimator.SetBool("FinalAttack", parity);
    }

    IEnumerator TouchTreeLength()
    {
        yield return new WaitForSeconds(2f);
        SetTreeTouchAnim(false);
        systemScript.SetIsTouchingTree(false);
    }

}
