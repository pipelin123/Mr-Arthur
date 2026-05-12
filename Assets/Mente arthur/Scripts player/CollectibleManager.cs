using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manager singleton que lleva el puntaje y conteo de coleccionables.
/// Coloca este script en un GameObject vacío llamado "CollectibleManager" en la escena.
/// </summary>
public class CollectibleManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────────────────────────
    public static CollectibleManager Instance { get; private set; }

    // ── Estado ─────────────────────────────────────────────────────────────────
    [Header("Estado (solo lectura en runtime)")]
    [SerializeField] private int totalScore = 0;
    [SerializeField] private int totalCollected = 0;

    // ── Eventos ────────────────────────────────────────────────────────────────
    [Header("Eventos")]
    public UnityEvent<int> onScoreChanged;       // Pasa el puntaje actual
    public UnityEvent<int> onItemCollected;      // Pasa el total recolectado
    public UnityEvent<string> onCollectibleType; // Pasa el ID del tipo recolectado

    // ── Propiedades públicas ───────────────────────────────────────────────────
    public int TotalScore => totalScore;
    public int TotalCollected => totalCollected;

    // ──────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        // Patrón singleton: solo una instancia persiste entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Comenta esta línea si NO quieres que persista entre escenas
    }

    /// <summary>
    /// Llamado por Collectible.cs cuando el jugador toca un ítem.
    /// </summary>
    public void OnCollect(int points, string id = "")
    {
        totalScore += points;
        totalCollected++;

        // Disparar eventos para que la UI u otros sistemas reaccionen
        onScoreChanged?.Invoke(totalScore);
        onItemCollected?.Invoke(totalCollected);

        if (!string.IsNullOrEmpty(id))
            onCollectibleType?.Invoke(id);

        Debug.Log($"[CollectibleManager] +{points} pts | Total: {totalScore} | Recogidos: {totalCollected}");
    }

    /// <summary>
    /// Reinicia el puntaje (útil al cargar una nueva partida).
    /// </summary>
    public void ResetScore()
    {
        totalScore = 0;
        totalCollected = 0;
        onScoreChanged?.Invoke(totalScore);
        onItemCollected?.Invoke(totalCollected);
    }
}
