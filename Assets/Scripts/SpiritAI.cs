using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiritAI : MonoBehaviour
{
    public NavMeshAgent _agent;
    Renderer render;

    public LayerMask mIsGround, mIsPlayer;

    public Transform _target;
    public GameObject player;
    public GameObject modelGO;
    public Player playerScript;
    public EnemySpawner spawnerSpirit;
    public UI gameScript;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //States
    public float sightRange;
    public bool playerInSightRange;

    public int health = 1;
    public float speed = 4f;

    float distanceToPlayer;
    public float maxDistanceToPlayer;
    bool flowStarted = false;
    public bool flowFailed = false;
    public float timerOutRun;
    public float flowPower;

    private void Awake()
    {
        player = GameObject.Find("Boat");
        _target = player.transform;
        _agent = GetComponent<NavMeshAgent>();
        playerScript = player.GetComponent<Player>();
        spawnerSpirit = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        gameScript = GameObject.Find("Main Camera").GetComponent<UI>();
        //render = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        DestroyEnemy();
        //Check for sight and attack rage
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, mIsPlayer);
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!flowStarted && !flowFailed) Patrolling();
        if (playerInSightRange && flowFailed)
        {
            if (distanceToPlayer < maxDistanceToPlayer*2)
            { RunOutPlayer(); }
            if (distanceToPlayer > maxDistanceToPlayer*2)
            {
                flowFailed = false;
                _agent.speed = speed;
                //render.material.color = Color.green;
            }
        }
        
        if (flowStarted && distanceToPlayer <= maxDistanceToPlayer)
        {
            ChasePlayer();
        }

        if (Input.GetMouseButtonUp(0) && flowStarted)
        {
            flowStarted = false;
            playerScript.flowStarted = false;
            flowFailed = true;
            _agent.speed = speed;
            //render.material.color = Color.white;
        }

        if (distanceToPlayer < 3f && flowStarted)
        {
            playerScript.spiritHave += 1;
            playerScript.flowStarted = false;
            spawnerSpirit.spiritHave -= 1;
            Destroy(gameObject);
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            _agent.SetDestination(walkPoint);
            transform.LookAt(walkPoint);
        }
            

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

    private void RunOutPlayer()
    {
        _agent.speed = speed*4;
        Vector3 moveDirection = transform.position - player.transform.position;
        _agent.SetDestination(moveDirection);
    }

    private void ChasePlayer()
    {
        _agent.speed = playerScript.flowPower / 10;
        _agent.SetDestination(_target.position);
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && distanceToPlayer <= maxDistanceToPlayer)
        {
            flowStarted = true;
            playerScript.flowStarted = true;
            //render.material.color = Color.red;
            transform.LookAt(_target.position);
            playerScript.CharonGO.transform.LookAt(transform.position);
            //playerScript.ClothGO.transform.LookAt(transform.position);
            playerScript.PaddleGO.transform.LookAt(transform.position);

            playerScript._animCharon.SetBool("flow", true);
            //playerScript._animCloth.SetBool("flow", true);
            playerScript._animPaddle.SetBool("flow", true);
        }

    }

    

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 1f);
    }

    private void DestroyEnemy()
    {
        if (gameScript.gameLevel != 0)
        {
            playerScript.flowStarted = false;
            spawnerSpirit.spiritHave -= 1;
            Destroy(gameObject);
        }
        
    }
}
