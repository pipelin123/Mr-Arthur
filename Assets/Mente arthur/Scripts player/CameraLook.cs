using UnityEngine;

/// <summary>
/// Agrega este script a la CÁMARA (hijo del jugador).
/// El mouse mueve la cámara verticalmente y rota el jugador horizontalmente.
/// </summary>
public class CameraLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    [Header("Límite vertical (grados)")]
    public float minY = -80f;
    public float maxY = 80f;

    private float rotationX = 0f;
    private Transform playerBody;

    void Start()
    {
        // El padre de la cámara es el jugador
        playerBody = transform.parent;

        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityY;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityX;

        // Vertical: rota la cámara (con límite para no dar voltereta)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minY, maxY);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Horizontal: rota el cuerpo del jugador entero
        playerBody.Rotate(Vector3.up * mouseX);

        // Desbloquear cursor con Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Volver a bloquear con click
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
