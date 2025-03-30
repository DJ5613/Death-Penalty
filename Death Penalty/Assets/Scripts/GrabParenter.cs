using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabParenter : MonoBehaviour
{
    public void OnGrab(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.SetParent(args.interactorObject.transform);
    }

    public void UnGrab(SelectExitEventArgs args)
    {
        args.interactableObject.transform.SetParent(null);
    }
}

