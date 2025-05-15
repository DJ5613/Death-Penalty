using UnityEngine;
using UnityEngine.Audio;

public class BreakableObject : MonoBehaviour
{
    public int hitsToDestroy = 3;
    private int hitCount = 0;
    private AudioSource audioSource;

    private void Awake() {audioSource = GetComponent<AudioSource>();}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon"))
        {
            hitCount++;
            audioSource.Play();

            Debug.Log("Объект получил удар оружием! Счётчик: " + hitCount);

            if (hitCount >= hitsToDestroy)
            {
                Destroy(gameObject);
                if (Random.Range(0, 101) > 70)
                {
                    DamageDetector.PlayerHP += 20;
                }
                Debug.Log("Объект разрушен!");
            }
        }
    }
}
