using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Coleccionable tipo memoria. Avisa al CollectibleCounter al ser recogido.
/// </summary>
public class Collectible : MonoBehaviour
{
    [Header("Configuración")]
    public string playerTag = "Player";
    public int pointValue = 10;

    [Header("Aura / Halo")]
    public Renderer auraRenderer;
    public float auraMinAlpha = 0.0f;
    public float auraMaxAlpha = 0.5f;
    public float auraPulseSpeed = 1.6f;

    [Header("Floteo y rotación")]
    public float rotationSpeed = 60f;
    public float bobAmplitude = 0.2f;
    public float bobFrequency = 1.2f;

    [Header("Efectos al recolectar")]
    public GameObject collectFX;
    public AudioClip collectSound;
    [Range(0f, 1f)] public float collectVolume = 0.5f;

    private const string SCORE_KEY = "CollectibleScore";
    private Vector3 startPosition;
    private Material auraMat;

    void Start()
    {
        startPosition = transform.position;
        if (auraRenderer != null)
            auraMat = auraRenderer.material;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        if (auraMat != null)
        {
            float t = (Mathf.Sin(Time.time * auraPulseSpeed) + 1f) / 2f;
            Color c = auraMat.color;
            c.a = Mathf.Lerp(auraMinAlpha, auraMaxAlpha, t);
            auraMat.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Sumar puntos
        int score = PlayerPrefs.GetInt(SCORE_KEY, 0) + pointValue;
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.Save();

        // Avisar al contador ← nuevo
        if (CollectibleCounter.Instance != null)
            CollectibleCounter.Instance.RegisterCollect();

        if (collectFX != null) Destroy(Instantiate(collectFX, transform.position, Quaternion.identity), 3f);
        if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position, collectVolume);

        Destroy(gameObject);
    }

    public static int GetScore()    => PlayerPrefs.GetInt(SCORE_KEY, 0);
    public static void ResetScore() { PlayerPrefs.SetInt(SCORE_KEY, 0); PlayerPrefs.Save(); }
}