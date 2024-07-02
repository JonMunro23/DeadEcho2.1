using GameBuilders.FPSBuilder.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    ZombieHealthController zombieHealth;
    public NavMeshAgent agent;
    Animator animator;
    [SerializeField]
    Transform leftMeleePos, rightMeleePos, playerPosition;
    public float speed, meleeCooldown, meleePerformTime, meleeRange;
    public float distanceToPerformMeleeAttack;
    [SerializeField] float minTimeBetweenGrowls, maxTimeBetweenGrowls;
    public int damage;
    public bool isDead, isMoving, canPerformMeleeAttack = true, seekPlayer = true, canGrowl;

    AudioSource zombieAudioSource;
    [SerializeField]
    AudioClip[] zombieGrowlAudioClips, zombieAttackingAudioClips;

    Vector2 velocity;
    Vector2 SmoothDeltaPosition;
    private void OnEnable()
    {
        //PlayerHealth.onDeath += SetIdle;
    }

    private void OnDisable()
    {
        //PlayerHealth.onDeath -= SetIdle;
    }

    private void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        zombieHealth = GetComponent<ZombieHealthController>();
        animator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
        StartCoroutine(GrowlCooldown());

        animator.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = speed;
        agent.height = 1.82f;
        //GetComponent<AgentLinkMover>().StartCoroutine(Start());
        //agent.baseOffset = -.065f;
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = agent.nextPosition.y;
        transform.position = rootPosition;
        agent.nextPosition = rootPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!zombieHealth.IsAlive)
            return;

        SyncAnimatiorAndAgent();

        if (canGrowl)
            Growl();

        if (seekPlayer)
        {
            SetTarget();    

            if (Vector3.Distance(transform.position, playerPosition.position) > distanceToPerformMeleeAttack)
            {
                if (!isMoving)
                {
                    MoveTowardsTarget();
                }
            }
            else if(Vector3.Distance(transform.position, playerPosition.position) <= distanceToPerformMeleeAttack)
            {
                if (isMoving)
                    HaltMovement();

                LookAtTarget(agent.destination);

                if (canPerformMeleeAttack)
                    MeleeAttack();
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(leftMeleePos.position, meleeRange);
    //    Gizmos.DrawSphere(rightMeleePos.position, meleeRange);
    //    Gizmos.color = Color.yellow;
    //}

    void SyncAnimatiorAndAgent()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        velocity = SmoothDeltaPosition / Time.deltaTime;
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero, velocity, agent.remainingDistance / agent.stoppingDistance);
        }

        bool shouldMove = velocity.magnitude > .5f && agent.remainingDistance > agent.stoppingDistance;

        animator.SetBool("IsMoving", shouldMove);
        animator.SetFloat("locomotion", velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if(deltaMagnitude > agent.radius / 2f)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, agent.nextPosition, smooth);
        }
    }

    void HaltMovement()
    {
        isMoving = false;
        animator.SetBool("IsMoving", isMoving);
        if(agent)
            agent.isStopped = true;
    }

    void MoveTowardsTarget()
    {
        isMoving = true;
        animator.SetBool("IsMoving", isMoving);
        agent.isStopped = false;
    }

    private void SetTarget()
    {
        agent.SetDestination(playerPosition.position);
    }

    void MeleeAttack()
    {
        canPerformMeleeAttack = false;
        int rand;

        if (zombieHealth.hasLostLeftArm)
            rand = Random.Range(2, 4);
        else if (zombieHealth.hasLostRightArm)
            rand = Random.Range(0, 2);
        else
            rand = Random.Range(0, 4);


        animator.SetInteger("MeleeAttackIndex", rand);
        animator.Play("MeleeAttack", 2);
        PlayAttackAudio();
        StartCoroutine(MeleeAttackCooldown());
    }

    /// <summary>
    /// Called within zombie attack animations.
    /// Checks the current MeleeAttackIndex to determine left or right hand hit checking.
    /// </summary>
    void CheckForHit()
    {
        if (animator.GetInteger("MeleeAttackIndex") == 0 || animator.GetInteger("MeleeAttackIndex") == 1)
            CheckForRightHandHit();
        else
            CheckForLeftHandHit();
    }

    void CheckForLeftHandHit()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(leftMeleePos.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.tag);
            if (collider.transform.CompareTag("Player"))
            {
                IProjectileDamageable damageableTarget = collider.GetComponent<IProjectileDamageable>();
                damageableTarget?.ProjectileDamage(damage, transform.root.position, collider.transform.position, 0);
            }
        }
    }
    void CheckForRightHandHit()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(rightMeleePos.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            if (collider.transform.CompareTag("Player"))
            {
                IProjectileDamageable damageableTarget = collider.GetComponent<IProjectileDamageable>();
                damageableTarget?.ProjectileDamage(damage, transform.root.position, collider.transform.position, 0);
            }
        }
    }
    IEnumerator MeleeAttackCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canPerformMeleeAttack = true;
    }

    void LookAtTarget(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.LookRotation(dir), agent.angularSpeed * Time.deltaTime);
    }

    void SetIdle()
    {
        seekPlayer = false;
        HaltMovement();
    }

    void Growl()
    {
        canGrowl = false;
        int rand = Random.Range(0, zombieGrowlAudioClips.Length);

        zombieAudioSource.clip = zombieGrowlAudioClips[rand];
        zombieAudioSource.Play();
        StartCoroutine(GrowlCooldown());
    }

    void PlayAttackAudio()
    {
        int rand = Random.Range(0, zombieAttackingAudioClips.Length);

        zombieAudioSource.clip = zombieAttackingAudioClips[rand];
        zombieAudioSource.Play();
    }

    IEnumerator GrowlCooldown()
    {
        float cooldownTime = Random.Range(minTimeBetweenGrowls, maxTimeBetweenGrowls);
        yield return new WaitForSeconds(cooldownTime);
        canGrowl = true;
    }
}
