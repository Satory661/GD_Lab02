using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -7);
    public float smoothSpeed = 0.125f;
    public float mouseSensitivity = 3f;

    private float yaw = 0f; // горизонтальное вращение
    private float pitch = 15f; // вертикальное вращение (начальный угол камеры сверху)

    void LateUpdate()
    {
        if (target == null) return;

        // Чтение мыши
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, 5f, 60f); // Ограничения вверх/вниз

        // Вычисляем позицию камеры вокруг игрока
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Плавное движение камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Смотрим на игрока
        transform.LookAt(target.position + Vector3.up * 1.5f); // Можно чуть выше центра игрока
    }
}