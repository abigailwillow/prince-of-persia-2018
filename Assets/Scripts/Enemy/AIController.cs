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
        if (nav.desiredVelocity.magnitude > 0.5f)
        {
            anim.SetFloat("MoveSpeed", 1f);
        }
        else
        {
            anim.SetFloat("MoveSpeed", 0f);
        }
    }

    void LateUpdate()
    {
        headBone.transform.LookAt(ply.position + new Vector3(0, 1.5f, 0));
    }
}
