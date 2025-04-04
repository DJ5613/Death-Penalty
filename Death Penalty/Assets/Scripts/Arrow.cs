using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool isFlying = false;
    private float destroyDelay = 5f;
    public int damage = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyDelay);
    }

    void FixedUpdate()
    {
        if (isFlying && rb.linearVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
        }
    }

    public void Launch(Vector3 force)
    {
        isFlying = true;
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        GetComponent<Collider>().enabled = true;
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        rb.isKinematic = true;
        Destroy(rb);
        Destroy(this);
    }*/
}
