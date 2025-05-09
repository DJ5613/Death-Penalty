using UnityEngine;
using TMPro;

public class FaceToCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        // Получаем трансформ главной камеры
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Поворачиваем текст лицом к камере
        transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                         _cameraTransform.rotation * Vector3.up);
    }
}