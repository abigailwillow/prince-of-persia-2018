using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject headBone;
    public LayerMask RaycastMask;
    public float HitDistance = 1.6f;
    public float KnockbackTime = 0.2f;
    public float KnockbackAmount = 2f;
    public GameObject BloodEmitter;
    public GameObject DustEmitter;
    public string HitSound;
    public string BlockSound;
    public Sprite HeartEmpty;
    public GameObject EnemyRagdoll;
    //public Vector3 PatrolPoint1;
    //public Vector3 PatrolPoint2;
    //public float PatrolTime = 2f;
    //public float StepLength = 1f;

    public bool Blocking;
    public bool Attacking;
    public int Health = 3;

    public bool PlayerInRoom;

    Transform ply;
    NavMeshAgent nav;
    Animator anim;
    SoundManager EnemySounds;
    AudioSource SoundSource;
    Vector3 SoundOrigin;
    Vector3 Origin;
    HeartLocator HealthCanvas;
    //int PatrolState = 2;
    //bool LastPlayerCheck;
    FPSController plyc;

    void Awake()
    {
        ply = GameObject.FindGameObjectWithTag("Player").transform;
        plyc = ply.GetComponent<FPSController>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        EnemySounds = GetComponent<SoundManager>();
        SoundSource = GetComponent<AudioSource>();

        HealthCanvas = GetComponentInChildren<Canvas>().transform.gameObject.GetComponent<HeartLocator>();

        Origin = transform.position;
    }

    void Update()
    {
        if (nav.desiredVelocity.magnitude > 0.5f)
        {
            anim.SetFloat("MoveSpeed", 1f);
        }
        else
        {
            anim.SetFloat("MoveSpeed", 0f);
        }

        if (PlayerInRoom)
        {
            nav.SetDestination(ply.transform.position);
        }
        else 
        {
            nav.SetDestination(Origin);
        }
    }

    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void Attack()
    {
        Attacking = true;
        RaycastHit SwordHit;
        if (Physics.Raycast(headBone.transform.position, headBone.transform.forward, out SwordHit, HitDistance, RaycastMask))
        {
            if (SwordHit.transform.tag == "Player")
            {
                FPSController ply = SwordHit.transform.GetComponent<FPSController>();
                if (!ply.Blocking)
                {
                    ply.HitDamaged(1);
                    StartCoroutine(Knockback(SwordHit));
                    SoundOrigin = SoundSource.transform.localPosition;
                    SoundSource.transform.position = SwordHit.point;
                    EnemySounds.PlaySound(HitSound);
                    SoundSource.transform.localPosition = SoundOrigin;
                    GameObject ParticleEmitter = Instantiate(BloodEmitter, SwordHit.point, Quaternion.LookRotation(SwordHit.normal));
                    StartCoroutine(GCParticles(ParticleEmitter));
                }
                else
                {
                    SoundOrigin = SoundSource.transform.localPosition;
                    SoundSource.transform.position = SwordHit.point;
                    EnemySounds.PlaySound(BlockSound);
                    SoundSource.transform.localPosition = SoundOrigin;
                    GameObject ParticleEmitter = Instantiate(DustEmitter, SwordHit.point, Quaternion.LookRotation(SwordHit.normal));
                    StartCoroutine(GCParticles(ParticleEmitter));
                }
            }
        }
    }

    public void PlayBlock()
    {
        Blocking = true;
        anim.SetTrigger("Block");
    }

    public void Block()
    {
        Blocking = true;
    }

    void LateUpdate()
    {
        headBone.transform.LookAt(plyc.headBone.position);
    }

    public void HitDamaged()
    {
        anim.SetTrigger("Knockback");
        switch (Health)
        {
            case 3:
                Health = 2;
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                break;
            case 2:
                Health = 1;
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                HealthCanvas.Heart_M.sprite = HeartEmpty;
                break;
            case 1:
                Health = 0;
                HitDeath();
                HealthCanvas.Heart_R.sprite = HeartEmpty;
                HealthCanvas.Heart_M.sprite = HeartEmpty;
                HealthCanvas.Heart_L.sprite = HeartEmpty;
                break;
            default:
                Health = 0;
                HitDeath();
                break;
        }
    }

    void HitDeath()
    {
        Health = 0;
        Instantiate(EnemyRagdoll, gameObject.transform.position, gameObject.transform.rotation);
        gameObject.transform.gameObject.SetActive(false);
        enabled = false;
    }

    IEnumerator Knockback(RaycastHit SwordHit)
    {
        float KnockbackElapsed = 0f;
        while (KnockbackElapsed < KnockbackTime)
        {
            Vector3 Origin = SwordHit.transform.position;
            SwordHit.transform.position = Vector3.Lerp(Origin, Origin + transform.forward * KnockbackAmount, 0.1f);
            KnockbackElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator GCParticles(GameObject DeleteEmitter)
    {
        yield return new WaitForSeconds(5f);
        Destroy(DeleteEmitter);
    }

    /*IEnumerator Approach()
    {
        nav.SetDestination(ply.position);
        yield return new WaitForSeconds(StepLength);
        nav.isStopped = true;
        yield return new WaitForSeconds(StepLength);
        nav.isStopped = false;
        if (PlayerInRoom)
        {
            StartCoroutine(Approach());
        }
    }*/
}
