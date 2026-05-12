using UnityEngine;

public class CollectibleCounter : MonoBehaviour
{
    public static CollectibleCounter Instance { get; private set; }

    [Header("Configuración")]
    public int totalCollectibles = 3;

    [Header("DEBUG - solo para probar")]
    public bool activateFromStart = false;  // ← activa esto en el Inspector para saltarte los collectibles

    [Header("Referencias")]
    public DoorRotator door;
    public GameObject portalObject;

    private int collected = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (portalObject != null)
            portalObject.SetActive(false);
    }

    void Start()
    {
        if (activateFromStart)
            OnAllCollected();
    }

    public void RegisterCollect()
    {
        collected++;
        Debug.Log($"[Counter] Memorias: {collected}/{totalCollectibles}");

        if (collected >= totalCollectibles)
            OnAllCollected();
    }

    void OnAllCollected()
    {
        Debug.Log("[Counter] ¡Todas las memorias recogidas!");

        if (door != null) door.Open();
        if (portalObject != null) portalObject.SetActive(true);
    }

    public int GetCollected() => collected;
}