using UnityEngine;
 
/// <summary>
/// Pon este script en el GameObject de la puerta.
/// IMPORTANTE: el pivot de la puerta debe estar en el borde (donde estarían las bisagras)
/// no en el centro, para que gire correctamente.
/// </summary>
public class DoorRotator : MonoBehaviour
{
    [Header("Rotación")]
    public float closedAngle = 180f;     // Ángulo inicial (cerrada)
    public float openAngle = 70f;        // Ángulo final (abierta)
    public float rotationSpeed = 2f;     // Qué tan rápido gira
 
    [Header("Eje de rotación")]
    public Vector3 rotationAxis = Vector3.up; // Y = gira horizontal (como puerta normal)
 
    public AudioClip doorSound;
    [Range(0f, 1f)] public float doorVolume = 0.5f;
 
    private bool isOpening = false;
    private Quaternion targetRotation;
 
    void Start()
    {
        // Asegurarse de que empieza en el ángulo cerrado
        transform.localRotation = Quaternion.AngleAxis(closedAngle, rotationAxis);
    }
 
    void Update()
    {
        if (!isOpening) return;
 
        // Rotar suavemente hacia el ángulo abierto
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
 
        // Parar cuando llegue
        if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
        {
            transform.localRotation = targetRotation;
            isOpening = false;
            Debug.Log("[Door] Puerta abierta");
        }
    }
 
    public void Open()
    {
        targetRotation = Quaternion.AngleAxis(openAngle, rotationAxis);
        isOpening = true;
 
        if (doorSound != null)
            AudioSource.PlayClipAtPoint(doorSound, transform.position, doorVolume);
 
        Debug.Log("[Door] Abriendo puerta...");
    }
}