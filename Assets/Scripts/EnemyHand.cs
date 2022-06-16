using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    public GameObject player;
    public Player playerScript;
    public EnemySpawner spawnerHand;

    bool alreadyAttacked;
    bool awaked;
    public GameObject model;
    Animator _anim;

    [Space(20)]
    public float timeBetweenAttacks = 1f;
    public float distanceToAttacks = 5f;
    float distance;
    
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Boat");
        playerScript = player.GetComponent<Player>();
        spawnerHand = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _anim = model.GetComponent<Animator>();
        _anim.SetBool("Awake", true);
        Invoke(nameof(AwakeIt), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, 0.0f, player.transform.position.z));
        distance = Vector3.Distance(player.transform.position, transform.position);


        if (distance <= distanceToAttacks && awaked)
        {
            if (!alreadyAttacked)
            {

                playerScript.health -= 1;

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            _anim.SetBool("Attack", true);
        }
        else _anim.SetBool("Attack", false);
        
        if (distance >= distanceToAttacks*4 || playerScript.playerIsDead)
        {
            _anim.SetFloat("SpeedAwake", -1);
            _anim.SetBool("Awake", true);
            //_anim.SetBool("Attack", false);
            Invoke(nameof(DestroyHand), 1f);
        }

        
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void DestroyHand()
    {

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        spawnerHand.spawnCount -= 1f;
    }


    void AwakeIt()
    {
        awaked = true;
        _anim.SetBool("Awake", false);
    }
}
