using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour
{
    public GameObject BrokenObject;
    public float MaxForce = 10f;
    public bool UseCleanup = true;
    public float CleanupTime = 10f;
    public float RandomCleanTime = 1.5f;

    [HideInInspector] public bool Broken;

    MeshRenderer Breakable;

    void Awake()
    {
        Broken = false;
        Breakable = GetComponentInChildren<MeshRenderer>();
    }

	public void Break()
    {
        Broken = true;
        GameObject Shards = Instantiate(BrokenObject, Breakable.transform.position, Breakable.transform.rotation, transform);

        Destroy(Breakable.gameObject);

        Rigidbody[] ShardArray = Shards.transform.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rbi in ShardArray)
        {
            rbi.AddForce(new Vector3(Random.Range(0, MaxForce), Random.Range(0, MaxForce), Random.Range(0, MaxForce)));
            if (UseCleanup)
            {
                StartCoroutine(GarbageCollect(rbi.gameObject));
            }
        }
    }

    IEnumerator GarbageCollect(GameObject Trash)
    {
        yield return new WaitForSeconds(CleanupTime + Random.Range(0, RandomCleanTime));
        Destroy(Trash);
    }
}
