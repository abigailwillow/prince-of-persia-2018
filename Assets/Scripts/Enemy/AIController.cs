using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    public GameObject headBone;

    Transform ply;
    NavMeshAgent nav;

    void Awake()
    {
        ply = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(ply.position);
        headBone.transform.LookAt(ply.position + new Vector3(0,1,0));
    }
}
