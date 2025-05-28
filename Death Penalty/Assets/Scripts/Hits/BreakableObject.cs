using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int hitsToDestroy = 3;
    private int hitCount = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip destroySound; // Добавляем новый звук для разрушения

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeHit()
    {
        hitCount++;
        audioSource.PlayOneShot(hitSound);
        Debug.Log("Объект получил удар оружием! Счётчик: " + hitCount);

        if (hitCount >= hitsToDestroy)
        {
            // Воспроизводим звук разрушения через статический метод, чтобы он не прерывался при уничтожении объекта
            AudioSource.PlayClipAtPoint(destroySound, transform.position);

            Destroy(gameObject);
            if (Random.Range(0, 101) > 70)
            {
                DamageDetector.PlayerHP += 20;
            }
            Debug.Log("Объект разрушен!");
        }
    }
}