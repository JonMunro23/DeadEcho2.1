using GameBuilders.FPSBuilder.Interfaces;
using System;
using System.Collections;
using UnityEngine;

public class ZombieHealthController : MonoBehaviour, IProjectileDamageable, IExplosionDamageable, IMeleeDamageable
{
    [SerializeField] float m_CurrentHealth;
    /*[SerializeField]*/ string hitBodyPart;
    [SerializeField] int powerUpDropChance;

    public static event Action<bool> onDeath;
    public static event Action<Vector3> dropPowerUp;

    bool wasMeleeAttack, canPlayHitAnim = true;
    [SerializeField] float hitAnimCooldownLength = 2;
    [SerializeField] float delayBeforeDestroying = 15;

    Animator animator;
    Rigidbody[] rigidbodies;
    ZombieAI zombieAI;

    [Header("Body Parts")]
    [SerializeField] SkinnedMeshRenderer zombieHead;
    [SerializeField] SkinnedMeshRenderer zombieLeftLowerArm;
    [SerializeField] SkinnedMeshRenderer zombieLeftHand;
    [SerializeField] SkinnedMeshRenderer zombieRightLowerArm;
    [SerializeField] SkinnedMeshRenderer zombieRightHand;
    [SerializeField] Collider zombieHeadCollider;
    [SerializeField] Collider zombieLeftLowerArmCollider;
    [SerializeField] Collider zombieRightLowerArmCollider;
    public bool hasLostLeftArm, hasLostRightArm;

    bool hasDied;



    /// <summary>
    /// Returns the current health of the zombie
    /// </summary>
    public float CurrentHealth => m_CurrentHealth;

    /// <summary>
    /// Returns the alive status of the zombie
    /// </summary>
    public bool IsAlive => m_CurrentHealth > 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        zombieAI = GetComponent<ZombieAI>();
    }

    public void SetHealth(float newHealth)
    {
        m_CurrentHealth = newHealth;
    }

    public void Damage(float damage)
    {
        if (damage > 0)
            ApplyDamage(damage);
    }

    public void Damage(float damage, Vector3 targetPosition, Vector3 hitPosition)
    {
        if (Math.Abs(damage) < Mathf.Epsilon)
            return;

        ApplyDamage(damage);
    }

    public void ExplosionDamage(float damage, Vector3 targetPosition, Vector3 hitPosition)
    {
        wasMeleeAttack = false;

        if (Math.Abs(damage) < Mathf.Epsilon)
            return;

        ApplyDamage(damage);
    }

    public void ProjectileDamage(float damage, Vector3 targetPosition, Vector3 hitPosition, float penetrationPower, string hitBodyPartTag = null)
    {
        wasMeleeAttack = false;
        hitBodyPart = hitBodyPartTag;
        Damage(damage, targetPosition, hitPosition);
    }

    public void MeleeDamage(float damage, Vector3 targetPosition, Vector3 hitPosition, string hitBodyPartTag = null)
    {
        wasMeleeAttack = true;
        hitBodyPart = hitBodyPartTag;
        Damage(damage, targetPosition, hitPosition);
    }

    private void ApplyDamage(float damage)
    {
        if (hasDied)
            return;


        if (PowerUpManager.isInstantKillActive)
        {
            m_CurrentHealth = 0;
            ExplodeHead();
        }
        else
            m_CurrentHealth = Mathf.Max(m_CurrentHealth - damage, 0);

        if (!IsAlive)
        {
            hasDied = true;

            if (DoesSpawnPowerUp())
                dropPowerUp?.Invoke(transform.position);

            bool wasHeadshot = hitBodyPart == "ZombieHead";

            onDeath?.Invoke(wasHeadshot);

            if(hitBodyPart == "ZombieLeftArm" || hitBodyPart == "ZombieRightArm")
                ExplodeArm(hitBodyPart == "ZombieLeftArm");

            int pointsToAward;
            if (wasHeadshot)
            {
                pointsToAward = PointsManager.Instance.awardedPointsData.headshotKill;
                ExplodeHead();
            }
            else if (wasMeleeAttack)
                pointsToAward = PointsManager.Instance.awardedPointsData.meleeKill;
            else
                pointsToAward = PointsManager.Instance.awardedPointsData.kill;

            PointsManager.Instance.AddPoints(pointsToAward);
            ActivateRagdoll();
            Destroy(gameObject, delayBeforeDestroying);
        }
        else
        {
            if (hitBodyPart == "ZombieLeftArm" || hitBodyPart == "ZombieRightArm")
                ExplodeArm(hitBodyPart == "ZombieLeftArm");

            if (canPlayHitAnim)
                PlayHitAnim();

            PointsManager.Instance.AddPoints(PointsManager.Instance.awardedPointsData.hit);
        }
    }

    void ExplodeHead()
    {
        zombieHead.enabled = false;
        
        //Slight delay so force can be applied to rigidbody
        Invoke("RemoveHeadCollider", .001f);

        //play head explosing particle effect
    }

    void ExplodeArm(bool isLeftArm)
    {
        if (hasLostLeftArm == true || hasLostRightArm == true)
            return;

        if (isLeftArm)
        {
            hasLostLeftArm = true;
            zombieLeftLowerArm.enabled = false;
            zombieLeftHand.enabled = false;
        }
        else
        {
            hasLostRightArm = true;
            zombieRightLowerArm.enabled = false;
            zombieRightHand.enabled = false;
        }
        
        //Slight delay so force can be applied to rigidbody
        Invoke("RemoveArmCollider", .001f);

    }
    void RemoveArmCollider()
    {
        if (hitBodyPart == "ZombieLeftArm")
            zombieLeftLowerArmCollider.enabled = false;
        else
            zombieRightLowerArmCollider.enabled = false;
    }

    void RemoveHeadCollider()
    {
        zombieHeadCollider.enabled = false;
    }

    void ActivateRagdoll()
    {
        zombieAI.isMoving = false;
        zombieAI.agent.velocity = Vector3.zero;
        zombieAI.agent.isStopped = true;
        zombieAI.agent.enabled = false;
        animator.enabled = false;
        
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            
            rigidbody.gameObject.layer = LayerMask.NameToLayer("ZombieRagdollLayer");
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
    bool DoesSpawnPowerUp()
    {
        int randInt = UnityEngine.Random.Range(0, 100);
        if (randInt <= powerUpDropChance)
            return true;
        else
            return false;

    }

    void PlayHitAnim()
    {
        canPlayHitAnim = false;
        StartCoroutine(HitAnimCooldown());
        switch (hitBodyPart)
        {
            case "ZombieHead":
                animator.Play("Hit-Head", 1);
                break;
            case "ZombieBody":
                int rand = UnityEngine.Random.Range(0, 2);
                if (rand == 1)
                    animator.Play("Hit-LeftShoulder", 1);
                else
                    animator.Play("Hit-RightShoulder", 1);
                break;
            case "ZombieLeftArm":
                animator.Play("Hit-LeftShoulder", 1);
                break;
            case "ZombieRightArm":
                animator.Play("Hit-RightShoulder", 1);
                break;
                //case "ZombieLeftLeg":
                //    animator.Play("Hit-LeftLeg", 0);
                //    break;
                //case "ZombieRightLeg":
                //    animator.Play("Hit-RightLeg", 0);
                //    break;
        }
    }

    IEnumerator HitAnimCooldown()
    {
        yield return new WaitForSeconds(hitAnimCooldownLength);
        canPlayHitAnim = true;
    }
}
