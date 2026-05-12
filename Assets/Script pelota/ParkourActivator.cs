using UnityEngine;

/// <summary>
/// Pon este script en el GameObject vacío padre del parkour.
/// El parkour empieza desactivado y se activa cuando se encesta la pelota.
/// </summary>
public class ParkourActivator : MonoBehaviour
{
    [Header("Animación de aparición")]
    public float riseHeight = 5f;        // Desde qué altura bajan los cubos al aparecer
    public float riseSpeed = 4f;         // Qué tan rápido bajan

    private bool isActivated = false;
    private bool isAnimating = false;
    private Vector3 targetPosition;
    private Vector3 hiddenPosition;

    void Start()
    {
        // Guardar posición final (donde quieres que esté el parkour)
        targetPosition = transform.position;

        // Posición oculta: arriba o abajo según prefieras
        hiddenPosition = targetPosition + Vector3.down * riseHeight;

        // Empezar oculto
        transform.position = hiddenPosition;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isAnimating) return;

        // Mover suavemente hacia la posición final
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            isAnimating = false;
            Debug.Log("[Parkour] ¡Parkour listo!");
        }
    }

    /// <summary>
    /// Llamado por Basket.cs cuando se encesta.
    /// </summary>
    public void Activate()
    {
        if (isActivated) return;
        isActivated = true;

        gameObject.SetActive(true);
        isAnimating = true;

        Debug.Log("[Parkour] ¡Activando parkour!");
    }
}
