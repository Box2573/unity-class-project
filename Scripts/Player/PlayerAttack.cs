using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator animator;
    InputController inputController;

    [SerializeField] GameObject hand;
    [SerializeField] GameObject hand2;

    public float damage = 10f;

    int count = 0;

    void Start()
    {
        inputController = GameManagerSingle.Instance.InputController;
    }

    void Update()
    {
        if (inputController.GetFireDown())
        {
            count++;

            if (count == 1)
            {
                hand.SetActive(true);
                animator.SetInteger("Combo", 1);
            }
            else if (count >= 10)
            {
                resetCombo();
            }
        }
    }

    public void comboCount()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack001"))
        {
            if (count > 1)
            {
                animator.SetInteger("Combo", 2);
                hand.SetActive(false);
                hand2.SetActive(true);
            }
            else
            {
                resetCombo();
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack002"))
        {
            if (count > 2)
            {
                animator.SetInteger("Combo", 3);
                hand.SetActive(true);
                hand2.SetActive(false);
            }
            else
            {
                resetCombo();
            }

        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack003"))
        {
            resetCombo();
        }
    }

    private void resetCombo()
    {
        count = 0;
        animator.SetInteger("Combo", 0);
        hand.SetActive(false);
        hand2.SetActive(false);
    }
}

