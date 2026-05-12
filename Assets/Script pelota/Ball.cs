using UnityEngine;

/// <summary>
/// La pelota NO se hace hija del HoldPoint para evitar deformación por scale.
/// En cambio, sigue la posición del HoldPoint por código cada frame.
/// </summary>
public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private bool isHeld = false;
    private Transform holdPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Seguir el holdPoint manualmente sin ser hijo
        if (isHeld && holdPoint != null)
        {
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;
        }
    }

    public void GetPickedUp(Transform point)
    {
        isHeld = true;
        holdPoint = point;

        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // NO hacer hijo — solo seguir la posición
        transform.SetParent(null);

        GetComponent<Collider>().enabled = false;
    }

    public void Throw(Vector3 force)
    {
        isHeld = false;
        holdPoint = null;

        GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
    }

    public bool IsHeld() => isHeld;
}