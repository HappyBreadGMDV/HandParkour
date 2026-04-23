using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    public NavMeshAgent Agent;

    public string PlayerTag;

    public float hp = 100;

    public float DamageRadius;

    public float Damage;
    public float DamageDelay;

    public LayerMask PlayerLayer;

    public float time;
    private Transform Target;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag(PlayerTag).transform;
    }

    void Update()
    {
        time += Time.deltaTime;
        Agent.destination = Target.position;

        if (time >= DamageDelay)
        {
            Collider[] cl = Physics.OverlapSphere(transform.position, DamageRadius, PlayerLayer);

            if(cl != null)
            {
                cl[0].gameObject.GetComponent<Player>().hp -= Damage; 
            }

            time = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DamageRadius);
    }
}
