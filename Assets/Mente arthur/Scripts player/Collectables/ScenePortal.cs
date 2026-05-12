using UnityEngine;

/// <summary>
/// Pon este script en el asset portal.
/// Debe tener un Collider con "Is Trigger" activado.
/// Arrastra el punto de destino (GameObject vacío) al campo TeleportTarget.
/// </summary>
public class ScenePortal : MonoBehaviour
{
    [Header("Destino")]
    public Transform teleportTarget;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (teleportTarget == null) { Debug.LogWarning("[Portal] No hay destino asignado."); return; }

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = Vector3.zero;

        other.transform.position = teleportTarget.position;

        Debug.Log("[Portal] ¡Teletransportado!");
    }
}