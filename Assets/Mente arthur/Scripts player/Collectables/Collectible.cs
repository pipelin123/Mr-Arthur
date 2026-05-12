using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Configuración")]
    public string playerTag = "Player";
    public int pointValue = 10;

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

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        int score = PlayerPrefs.GetInt(SCORE_KEY, 0) + pointValue;
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.Save();

        if (CollectibleCounter.Instance != null)
            CollectibleCounter.Instance.RegisterCollect();

        if (collectFX != null) Destroy(Instantiate(collectFX, transform.position, Quaternion.identity), 3f);
        if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position, collectVolume);

        Destroy(gameObject);
    }

    public static int GetScore()    => PlayerPrefs.GetInt(SCORE_KEY, 0);
    public static void ResetScore() { PlayerPrefs.SetInt(SCORE_KEY, 0); PlayerPrefs.Save(); }
}