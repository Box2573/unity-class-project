using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMove : MonoBehaviour
{
    Transform player;
    Animator animator;


    [SerializeField] bool canMove = false;
    [SerializeField] BossFightAni bossFight;
    [SerializeField] NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (distanceToPlayer() >= 7)
            {
                //transform.LookAt(player.position);
                //animator.SetTrigger("JumpAttack");
            }
            else if (distanceToPlayer() >= 5)
            {
                animator.SetTrigger("Walk");
                //transform.LookAt(player.position);
                agent.SetDestination(player.position);
            }
        }

        if (bossFight != null)
        {
            canMove = bossFight.canMove;
        }
    }

    private float distanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, GameObject.FindWithTag("Player").transform.position);
    }
}
