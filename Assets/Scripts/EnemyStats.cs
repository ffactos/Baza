using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    float maxHealth;
    float healElapse;
    public float health;
    public float dmg;
    public float heal;
    public float healSpeed;
    public float speed = 3.5f;
    public float runSpeed = 6f;
    public float attackInterval;
    public float attackDistance;
    public float armor;
    public float locateDistance;

    public NavMeshAgent agent;

    public ParticleSystem deathParticle;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        health = maxHealth;

        agent.speed = speed;
    }

    private void Update()
    {
        healElapse += Time.deltaTime;
        if(healElapse > healSpeed)
        {
            healElapse = 0;
            health += heal;
        }
    }

    public void Damage(float dmg)
    {
        if (dmg > health)
        {
            Death();
        }
        else
        {
            health -= dmg;
        }
    }

    public void Death()
    {
        if(deathParticle != null)
            Instantiate(deathParticle);
        Destroy(gameObject);
    }

}
