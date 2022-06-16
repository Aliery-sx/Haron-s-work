using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent _agent;
    
    public LayerMask mIsGround, mIsPlayer;
    
    public Transform _target;
    public GameObject player;
    public Player playerScript;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public int health = 1;

    private void Awake()
    {
        player = GameObject.Find("Boat");
        _target = player.transform;
        _agent = GetComponent<NavMeshAgent>();
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Check for sight and attack rage
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, mIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, mIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            _agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 20f, mIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_target.position);
    }

    private void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        transform.LookAt(_target);

        if (!alreadyAttacked)
        {
            ///Attack code
            playerScript.health -= 1;
            
            ///
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 1f);
    }
    
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
