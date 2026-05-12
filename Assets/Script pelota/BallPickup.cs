using UnityEngine;

/// <summary>
/// Pon este script en el JUGADOR.
/// Crea un GameObject vacío hijo de la cámara llamado "HoldPoint"
/// y asígnalo en el Inspector — ahí es donde aparecerá la pelota al agarrarla.
/// </summary>
public class BallPickup : MonoBehaviour
{
    [Header("Configuración")]
    public Transform holdPoint;         // Arrastra aquí el GameObject vacío "HoldPoint"
    public float pickupRange = 2.5f;    // Distancia máxima para agarrar
    public float throwForce = 15f;      // Fuerza del lanzamiento

    [Header("UI feedback (opcional)")]
    public GameObject pickupHint;       // Un texto "Presiona F" que puedes mostrar/ocultar

    private Ball heldBall = null;

    void Update()
    {
        // Buscar pelota cercana
        Ball nearbyBall = FindNearbyBall();

        // Mostrar/ocultar hint
        if (pickupHint != null)
            pickupHint.SetActive(nearbyBall != null && heldBall == null);

        // F → agarrar
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldBall == null && nearbyBall != null)
                PickUp(nearbyBall);
            else if (heldBall != null)
                Drop();
        }

        // Click izquierdo → lanzar
        if (Input.GetMouseButtonDown(0) && heldBall != null)
            Throw();
    }

    Ball FindNearbyBall()
    {
        // Busca todas las pelotas en la escena y devuelve la más cercana en rango
        Ball[] balls = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        Ball closest = null;
        float closestDist = pickupRange;

        foreach (Ball b in balls)
        {
            if (b.IsHeld()) continue;
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = b;
            }
        }
        return closest;
    }

    void PickUp(Ball ball)
    {
        heldBall = ball;
        heldBall.GetPickedUp(holdPoint);
        Debug.Log("[BallPickup] Pelota agarrada");
    }

    void Drop()
    {
        heldBall.Throw(Vector3.zero); // Soltar sin fuerza
        heldBall = null;
        Debug.Log("[BallPickup] Pelota soltada");
    }

    void Throw()
    {
        // Lanzar en la dirección que mira la cámara
        Vector3 throwDirection = Camera.main.transform.forward;
        heldBall.Throw(throwDirection * throwForce);
        heldBall = null;
        Debug.Log("[BallPickup] Pelota lanzada");
    }
}
