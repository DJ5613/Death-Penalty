using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaySoundOnTake : MonoBehaviour
{ 
    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        grabInteractable.selectEntered.AddListener(PlayGrabSound);
    }

    private void OnDestroy()
    {
        // Отписываемся от событий при уничтожении объекта
        grabInteractable.selectEntered.RemoveListener(PlayGrabSound);
    }

    private void PlayGrabSound(SelectEnterEventArgs arg)
    {
        // Воспроизводим звук, если он есть
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned for grab sound effect.", this);
        }
    }
}
