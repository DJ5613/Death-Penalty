using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class BreakableObject : MonoBehaviour
{
    public int hitsToDestroy = 3;
    private int hitCount = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip healSound;
    [SerializeField] private AudioClip destroySound;
    [SerializeField] private AudioMixerGroup effectsMixerGroup;

    private void Awake() {audioSource = GetComponent<AudioSource>();}

    public void TakeHit()
    {
        hitCount++;
        audioSource.PlayOneShot(hitSound);
        Debug.Log("Объект получил удар оружием! Счётчик: " + hitCount);

        if (hitCount >= hitsToDestroy)
        {
            if (Random.Range(0, 101) > 70)
            {
                DamageDetector.PlayerHP += 20;
                PlaySoundAtPosition(transform.position, healSound);
            }
            PlaySoundAtPosition(transform.position, destroySound);
            Debug.Log("Объект разрушен!");
            Destroy(gameObject);
        }
    }

    // Метод для создания временного аудиосурса
    private void PlaySoundAtPosition(Vector3 position, AudioClip clip)
    {
        GameObject soundObject = new GameObject("TempAudioSource");
        soundObject.transform.position = position;
        AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
        tempAudioSource.outputAudioMixerGroup = effectsMixerGroup;
        tempAudioSource.PlayOneShot(clip);

        // Уничтожаем объект после завершения воспроизведения
        Destroy(soundObject, clip.length);
    }
}
