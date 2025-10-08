using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public Transform player;
    public float patrolRadius = 3f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float chaseRange = 8f;
    public float attackRange = 2f;
    public float attackDelay = 1.5f;

    private Vector3 startPos;
    private Vector3 patrolTarget;
    private float lastAttackTime;

    private enum State { Patrol, Chase, Attack }
    private State state = State.Patrol;

    void Start()
    {
        startPos = transform.position;
        SetPatrolTarget();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > chaseRange) state = State.Patrol;
        else if (dist > attackRange) state = State.Chase;
        else state = State.Attack;

        switch (state)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, patrolTarget) < 0.3f)
            SetPatrolTarget();
        LookAt(patrolTarget);
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        LookAt(player.position);
    }

    void Attack()
    {
        LookAt(player.position);
        if (Time.time - lastAttackTime > attackDelay)
        {
            lastAttackTime = Time.time;
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage();
                Debug.Log("Враг атаковал игрока!");
            }
        }
    }

    void SetPatrolTarget()
    {
        Vector2 rand = Random.insideUnitCircle * patrolRadius;
        patrolTarget = startPos + new Vector3(rand.x, 0, rand.y);
    }

    void LookAt(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }
    }
}
