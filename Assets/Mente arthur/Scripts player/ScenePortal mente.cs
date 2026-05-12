using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Collider con Is Trigger activado.
/// Cuando el jugador lo toca carga la escena indicada.
/// </summary>
public class ScenePortal2 : MonoBehaviour
{
    [Header("Escena destino")]
    public string sceneName = "";
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[Portal] No hay escena asignada.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
