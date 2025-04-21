using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HandBasedAttachTransform : MonoBehaviour
{
    public Transform leftHandAttach;
    public Transform rightHandAttach;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetAttachTransformByHand);
    }

    private void SetAttachTransformByHand(SelectEnterEventArgs args)
    {
        string interactorName = args.interactorObject.transform.name.ToLower();
        Debug.Log(interactorName);
        if (interactorName.Contains("left interaction"))
        {
            grabInteractable.attachTransform = leftHandAttach;
        }
        else if (interactorName.Contains("right interaction"))
        {
            grabInteractable.attachTransform = rightHandAttach;
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(SetAttachTransformByHand);
    }
}
