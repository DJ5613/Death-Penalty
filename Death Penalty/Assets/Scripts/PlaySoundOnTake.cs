using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaySoundSword : MonoBehaviour
{ 
    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    private void Awake()
    {
        //grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        //grabInteractable.selectEntered.AddListener(PlayGrabSound);
    }

    public void playSound()
    {
        audioSource.Play();
    }
    /*
    private void PlayGrabSound(SelectEnterEventArgs arg)
    {
        // Воспроизводим звук, если он есть
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("НЕТУ ЗВУКА ДУРАК хахаха бака типо хахаа", this);
        }
    }
    */
} 
