using UnityEngine;

/// <summary>
/// Pon este script en el jugador.
/// Si cae por debajo de fallLimit, lo regresa al spawn.
/// </summary>
public class FallRespawn : MonoBehaviour
{
    [Header("Configuración")]
    public float fallLimit = -10f;        // A qué altura se considera "caído"
    public Transform spawnPoint;          // Arrastra aquí un GameObject vacío como punto de respawn
                                          // Si lo dejas vacío, regresa a la posición inicial

    private Vector3 initialPosition;
    private Rigidbody rb;

    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < fallLimit)
            Respawn();
    }

    void Respawn()
    {
        // Resetear velocidad para no llegar con inercia
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = spawnPoint != null ? spawnPoint.position : initialPosition;
        Debug.Log("[FallRespawn] Jugador respawneado.");
    }
}
