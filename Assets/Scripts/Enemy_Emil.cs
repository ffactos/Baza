using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Emil : MonoBehaviour
{
    public Transform playerTransform;
    public Transform self;
    public NavMeshAgent emil;
    public EnemyStats stats;
    public GameObject projectile;
    public Animator animator;

    public bool playerGettingDamage = false;
    private float attackElapse = 0.0f;
    public float handDamage = 50f;
    public float handAttackRadius = 5f;
    private float changePositionElapse = 0.0f;

    public float findPathCD = 1f;
    public float distanceToStartSearh = 3f;
    public float randomPosRange = 50f;
    public float randomPosBring = 30f;

    Collider collider;

    PlayerStats player;

    public AnimationEvent handAttack;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform.GetComponent<PlayerStats>();
        emil = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>();
        handAttack.functionName = "HandAttack";
    }
    private void Update()
    {
        if (Vector3.Distance(self.transform.position, playerTransform.position) < handAttackRadius)
        {
            emil.isStopped = true;
            emil.transform.LookAt(playerTransform);
            animator.SetBool("isAttacking", true);
        }
        else
        {
            emil.isStopped = false;
            animator.SetBool("isAttacking", false);
            
        }

        if (Vector3.Distance(self.transform.position, playerTransform.position) < stats.locateDistance)
        {
            emil.destination = playerTransform.position;
            emil.speed = stats.runSpeed;
        }
        else
        {
            emil.speed = stats.speed;
            changePositionElapse += Time.deltaTime;
            if (changePositionElapse >= findPathCD)
            {
                if(emil.remainingDistance <= distanceToStartSearh)
                {
                    Vector3 point;
                    if(RandomPoint(transform.position, randomPosRange, out point))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1f);
                        emil.destination = point;
                    }
                }
                changePositionElapse = 0f;
            }
        }

        animator.SetFloat("speed", emil.velocity.magnitude);

        if (playerGettingDamage)
        {
            if (collider.gameObject.GetComponent<PlayerStats>() != null)
            {
                attackElapse += Time.deltaTime;
                Debug.Log("Elapse");
                if (attackElapse > stats.attackInterval)
                {
                    Debug.Log("Attack!");
                    if (stats.dmg > player.health)
                    {
                        player.Die();
                    }
                    else
                    {
                        player.Damage(stats.dmg);
                        float distance = Vector3.Distance(transform.position, player.gameObject.transform.position);
                        if (distance / 10 + .1f > player.gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Grain>().intensity.value)
                            player.gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Grain>().intensity.value = .35f + (1f - distance / 10);
                        Debug.Log("Player Damaged");
                    }
                    attackElapse = 0f;
                    playerGettingDamage = true;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        collider = other;
        playerGettingDamage = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerStats>() != null)
        {
            collider = null;
            player.gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Grain>().intensity.value = 0f;
            playerGettingDamage = false;
        }
    }

    bool RandomPoint(Vector3 point, float range, out Vector3 result)
    {
        Vector3 randomPoint = point + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, randomPosBring, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero; return false;
    }
    public void HandAttack()
    {
        foreach (var collider in Physics.OverlapSphere(transform.position, handAttackRadius))
        {
            PlayerStats stats = collider.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.Damage(handDamage);
            }
        }
    }
}
