using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slime_Attack") && collision.gameObject.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().Move(-collision.gameObject.transform.forward * Time.deltaTime * 5f); // 調整力量的大小
        }
    }

}
