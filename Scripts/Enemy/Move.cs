using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Move : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Animator animator;

    [SerializeField] float chaseDistance = 15;
    [SerializeField] float attackDistance = 5;
    [SerializeField] float confuseTime = 5;

    [Header("Patrol相關")]
    [SerializeField] PatroPath patroPath;
    [Tooltip("在每個waypoint等待一段時間")]
    [SerializeField] float wayPointToWaitTime = 5;
    [Tooltip("需要到達waypoint的距離")]
    [SerializeField] float wayPointToStay = 3;

    Vector3 originPosition;//原點座標
    [SerializeField] int currentWayPoint = 0;//當前需到達的waypoint編號
    [SerializeField] float timeSinceArriveWayPoint = 0;//距離上次抵達waypoint的時間
    float timeSinceSeePlayer = 150;
    bool isArrive = false;
    bool isSeePlayer = false;

    public static event Action ReGen;
    public static event Action<Animator> onAttack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();

        originPosition = transform.position;
        agent.SetDestination(patroPath.GetWayPointPosition(currentWayPoint));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange() < attackDistance)
        {
            transform.LookAt(transform.position);
            animator.SetTrigger("Attack");
        }
        else if (playerInRange() <= chaseDistance)
        {
            agent.SetDestination(player.position);
            animator.SetBool("Chasing", true);
            isSeePlayer = true;
        }
        else
        {
            if (isSeePlayer)
            {
                agent.SetDestination(patroPath.GetWayPointPosition(currentWayPoint));
                isSeePlayer = false;
            }

            patro();
            ReGen?.Invoke();
        }

        updateTime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            onAttack?.Invoke(animator);
        }
    }

    float playerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    private void patro()
    {
        //Vector3 nextWayPointPosition = originPosition;

        if (timeSinceArriveWayPoint > wayPointToWaitTime)
        {
            isArrive = false;
            timeSinceArriveWayPoint = 0;
            animator.SetBool("Chasing", true);
            agent.SetDestination(patroPath.GetWayPointPosition(currentWayPoint));
        }

        if (isAtWayPoint())
        {
            animator.SetBool("Chasing", false);
            currentWayPoint = patroPath.GetNextWayPointNum(currentWayPoint);
            isArrive = true;
        }

    }

    //檢查是否抵達waypoint
    private bool isAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patroPath.GetWayPointPosition(currentWayPoint)) < wayPointToStay);
    }

    void updateTime()
    {
        if (isArrive)
        {
            timeSinceArriveWayPoint += Time.deltaTime;
        }

    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
