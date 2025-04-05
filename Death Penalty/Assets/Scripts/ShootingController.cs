using UnityEngine;
using System.Collections;

public class EnemyArcher : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRadius = 15f;
    public float fireRate = 2f;
    public float projectileSpeed = 8f;
    public float aimingOffset = 0.5f;
    public float projectileGravity = 0.5f;
    public float heightOffset = 1.5f;

    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public LayerMask targetMask;

    private Transform playerTarget;
    private float nextFireTime;
    private bool hasTarget = false;
    private bool isAiming = false;

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTarget == null) return;

        CheckForTarget();
        RotateTowardsTarget();

        if (hasTarget && Time.time >= nextFireTime && !isAiming)
        {
            StartCoroutine(ShootWithDelay());
            nextFireTime = Time.time + fireRate;
        }
    }

    void CheckForTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        hasTarget = distanceToPlayer <= attackRadius;

        if (hasTarget)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (playerTarget.position + Vector3.up * heightOffset) - firePoint.position;
            if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, attackRadius, targetMask))
            {
                hasTarget = hit.transform.CompareTag("Player");
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (!hasTarget) return;

        Vector3 direction = playerTarget.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }

    IEnumerator ShootWithDelay()
    {
        isAiming = true;

        // �������� ����� ��������� (�������� ������������)
        yield return new WaitForSeconds(0.5f);

        if (hasTarget && playerTarget != null)
        {
            Shoot();
        }

        isAiming = false;
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        // ������ ������� � ������ �������� ���� � ����������
        Vector3 targetPosition = playerTarget.position + Vector3.up * heightOffset;
        Vector3 predictedPosition = targetPosition + playerTarget.GetComponent<Rigidbody>().linearVelocity * aimingOffset;

        Vector3 direction = (predictedPosition - firePoint.position).normalized;
        float distance = Vector3.Distance(firePoint.position, predictedPosition);

        // ���� ���������� ��� ������� ��������
        float elevationAngle = Mathf.Atan2(direction.y, Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z));
        float adjustedSpeed = Mathf.Sqrt(projectileGravity * distance / Mathf.Sin(2 * elevationAngle));

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(direction * adjustedSpeed, projectileGravity, 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damage = 15f;
    public Collider damageCollider;
    public string[] ignoreTags;
    public GameObject hitEffect;

    private Rigidbody rb;
    private bool hasHit = false;
    private float gravity;

    public void Initialize(Vector3 velocity, float gravity, float lifetime)
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = velocity;
        this.gravity = gravity;
        damageCollider.enabled = true;
        StartCoroutine(DestroyAfterLifetime(lifetime));
    }

    void FixedUpdate()
    {
        if (!hasHit)
        {
            // ��������� ����������
            rb.linearVelocity += Vector3.down * gravity * Time.fixedDeltaTime;
            // ������������ ������ �� ����������� ��������
            if (rb.linearVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
            }
        }
    }

    IEnumerator DestroyAfterLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (!hasHit) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        foreach (string tag in ignoreTags)
        {
            if (collision.gameObject.CompareTag(tag)) return;
        }

        // ��������� �����
        /*Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }*/

        // ������ ���������
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        // ��������� ���������
        hasHit = true;
        rb.isKinematic = true;
        damageCollider.enabled = false;
        Destroy(GetComponent<TrailRenderer>(), 0.5f);

        // ������������ � ����
        if (!collision.gameObject.CompareTag("Player"))
        {
            transform.SetParent(collision.transform);
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}