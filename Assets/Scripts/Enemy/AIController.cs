using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    public GameObject headBone;

    Transform ply;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        ply = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        nav.SetDestination(ply.position);
        headBone.transform.LookAt(ply.position + new Vector3(0,1,0));
        print(nav.desiredVelocity.magnitude);
        if (nav.desiredVelocity.magnitude > 0.5f)
        {
            anim.SetFloat("MoveSpeed", 1f);
        }
        else
        {
            anim.SetFloat("MoveSpeed", 0f);
        }
    }
}
