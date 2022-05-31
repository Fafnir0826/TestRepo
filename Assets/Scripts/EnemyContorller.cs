using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates{GUARD,PATROL,CHASE,DEAD}
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyContorller : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;
    //private Collider coll;

    protected CharacterStats characterStats;
    
    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    private float speed;
    public GameObject attackTarget;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    private Quaternion guardRotation;

    [Header("Patrol State")]

    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;

    //配合動畫
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
       // coll = GetComponent<Collider>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;      
    }
    void Start()
    {
        if(isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //FIXME:場景切換後修改掉
        GameManager.Instance.AddObserver(this);
    }
    //切換場景時啟用
   /* void OnEnable() 
    {
       
    }*/

    void OnDisable() 
    {
        if(!GameManager.IsInitizlized) return;

        GameManager.Instance.RemoveObserver(this);
    }

    void Update()
    {
        if(characterStats.CurrentHealth ==0)
        {
            isDead = true;
        }
        if(!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -=Time.deltaTime;
        }
    }
    void SwitchAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Chase",isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical",characterStats.isCritical);
        anim.SetBool("Death",isDead);
    }

    void SwitchStates()
    {
        if(isDead)
            enemyStates = EnemyStates.DEAD;
        //如果發現player切換到CHASE
        else if(FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                
                if(transform.position !=guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if(Vector3.SqrMagnitude(guardPos - transform.position)<=agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
                    }
                        
                }
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed*0.5f;

                //判斷是否到了隨機巡邏點
                if(Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if(remainLookAtTime>0)
                        remainLookAtTime -=Time.deltaTime;
                    else
                        GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                
                isWalk = false;
                isChase = true;

                agent.speed = speed;

                if(!FoundPlayer())
                {
                    //拉脫回到上一個狀態
                     isFollow = false;
                     if(remainLookAtTime>0)
                     {
                         agent.destination = transform.position;
                         remainLookAtTime -=Time.deltaTime;
                     }
                     else if(isGuard)
                        enemyStates =EnemyStates.GUARD;
                     
                     else
                       enemyStates = EnemyStates.PATROL;                           
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                //在攻擊範圍內攻擊
               if(TargetInAttackRange()||TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;

                    if(lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        //暴擊
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChange;
                        //執行攻擊
                        Attack();
                    }
                }
                break;    
            case EnemyStates.DEAD:
                //agent.enabled = false;
                agent.radius = 0;
                //coll.enabled = false;
                Destroy(gameObject,2f);
                break;    
        } 
    }

    void Attack()
    {
      transform.LookAt(attackTarget.transform);
      if(TargetInAttackRange())
      {
          //近身攻擊動畫
          anim.SetTrigger("Attack");
      }
      if(TargetInSkillRange())
      {
          //技能攻擊動畫
          anim.SetTrigger("Skill");
      }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position,sightRadius);

        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
//-----在是否攻擊範圍內攻擊----
    bool TargetInAttackRange()
    {
        if(attackTarget !=null)
            return Vector3.Distance(attackTarget.transform.position,transform.position)<= characterStats.attackData.attackRange;
        else
            return false;
    }
    bool TargetInSkillRange()
    {
        if(attackTarget !=null)
            return Vector3.Distance(attackTarget.transform.position,transform.position)<= characterStats.attackData.skillRange;
        else
            return false;     
    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX =Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange,patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX,transform.position.y,guardPos.z + randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint,out hit ,patrolRange,1/*1為walkable的layer*/)?hit.position:transform.position;
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }

    //Animation Event
    void Hit()
    {
        if(attackTarget !=null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats,targetStats);
        }
    }

    //接口程式
    public void EndNotify()
    {
        //勝利動畫
        //停止所有移動
        //停止Agent
        anim.SetBool("Win",true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;

    }


}
