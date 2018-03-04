using UnityEngine;
using System.Collections;

public class DeathCamFollow : MonoBehaviour
{
    public float DeathFollowTime = 5f;
    public float RandomRange = 3f;
    public float RandomizeTime = 0.5f;
    //public Vector3 DeathOffset = new Vector3(-1f, 0, -1f);

    Transform MainCam;
    Vector3 Velocity = Vector3.zero;
    Vector3 RandomVector = Vector3.zero;
    float NextRandom = 0f;

    void Update()
    {
        if (Time.time > NextRandom)
        {
            RandomVector = new Vector3(Random.Range(-RandomRange, RandomRange), Random.Range(-RandomRange, RandomRange), Random.Range(-RandomRange, RandomRange));
            NextRandom = Time.time + RandomizeTime;
        }
    }

    public void DeathCam(GameObject plyRagdoll)
    {
        StartCoroutine(FollowCorpse(plyRagdoll));
    }

    IEnumerator FollowCorpse(GameObject plyRagdoll)
    {
        float ElapsedTime = 0f;
        MainCam = Camera.main.transform;
        while (/*ElapsedTime < DeathFollowTime*/ true)
        {
            MainCam.LookAt(plyRagdoll.transform.GetComponentInChildren<SphereCollider>().transform);
            MainCam.position = Vector3.SmoothDamp(MainCam.position, MainCam.position + RandomVector, ref Velocity, DeathFollowTime);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
