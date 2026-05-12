using UnityEngine;

/// <summary>
/// Pon este script en el trigger de la canasta.
/// Arrastra el GameObject del parkour en el Inspector.
/// </summary>
public class Basket : MonoBehaviour
{
    [Header("Puntos por encestar")]
    public int pointsPerBasket = 2;

    [Header("Parkour")]
    public ParkourActivator parkour; // Arrastra aquí el GameObject del parkour

    public AudioClip basketSound;

    private int totalScore = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Ball>(out Ball ball)) return;
        if (ball.IsHeld()) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && rb.linearVelocity.y < 0)
        {
            totalScore += pointsPerBasket;
            Debug.Log($"[Basket] ¡Canasta! +{pointsPerBasket} | Total: {totalScore}");

            if (basketSound != null)
                AudioSource.PlayClipAtPoint(basketSound, transform.position);

            // Activar el parkour
            if (parkour != null)
                parkour.Activate();
        }
    }

    public int GetScore() => totalScore;
}