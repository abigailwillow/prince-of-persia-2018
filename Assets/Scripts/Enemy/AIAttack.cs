using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public float RNGCooldown = 1f;
    public float AttackChance = 50f;

    float RandomNum;
    float NextRNG;
    AIController Enemy;

    void Awake()
    {
        Enemy = GetComponentInParent<AIController>();
    }

	void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (Time.time > NextRNG)
            {
                RandomNum = Random.Range(0, 100);
                if (RandomNum < AttackChance)
                {
                    Enemy.PlayAttack();
                }
                else
                {
                    Enemy.PlayBlock();
                }
                NextRNG = Time.time + RNGCooldown;
            }
        }
    }

    void Update()
    {
        if (Time.time > NextRNG)
        {
            Enemy.Blocking = false;
            Enemy.Attacking = false;
        }
    }
}
