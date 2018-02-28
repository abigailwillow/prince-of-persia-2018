using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public float HitDistance = 1.5f;
    public float KnockbackAmount = 2f;
    public float KnockbackTime = 0.2f;
    public string EnemyHitSound = "Grunt_Temp";
    public string WorldHitSound = "Sword_Clang";
    public GameObject BloodEmitter;
    public float SmoothFactor = 0.5f;

    Quaternion DesiredRotation;
    SoundManager VMSounds;
    AudioSource SoundSource;
    Vector3 SoundOrigin;
    float KnockbackElapsed;

    void Awake()
    {
        VMSounds = GetComponent<SoundManager>();
        SoundSource = GetComponent<AudioSource>();
        SoundOrigin = SoundSource.transform.localPosition;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(DesiredRotation, transform.parent.rotation, SmoothFactor);
        DesiredRotation = transform.rotation;
    }

    public void Attack()
    {
        RaycastHit SwordHit;
        if (Physics.Raycast(FirstPersonCamera.transform.position, FirstPersonCamera.transform.forward, out SwordHit, HitDistance))
        {
            if (SwordHit.transform.tag == "Attackable")
            {
                SoundSource.transform.position = SwordHit.point;
                VMSounds.PlaySound(EnemyHitSound);
                SoundSource.transform.localPosition = SoundOrigin;
                StartCoroutine(Knockback(SwordHit));
                GameObject ParticleEmitter = Instantiate(BloodEmitter, SwordHit.point, Quaternion.LookRotation(SwordHit.normal));
                StartCoroutine(GCParticles(ParticleEmitter));
            }
            else
            {
                SoundSource.transform.position = SwordHit.point;
                VMSounds.PlaySound(WorldHitSound);
                SoundSource.transform.localPosition = SoundOrigin;
            }
        }
    }

    IEnumerator GCParticles(GameObject DeleteEmitter)
    {
        yield return new WaitForSeconds(5f);
        Destroy(DeleteEmitter);
    }

    IEnumerator Knockback(RaycastHit SwordHit)
    {
        KnockbackElapsed = 0f;
        while (KnockbackElapsed < KnockbackTime)
        {
            Vector3 Origin = SwordHit.transform.position;
            SwordHit.transform.position = Vector3.Lerp(Origin, Origin + transform.forward * KnockbackAmount, 0.1f);
            KnockbackElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
