using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Pantalla de introducción: texto con efecto de máquina de escribir,
/// luego aparecen los gráficos de controles (WASD + Espacio).
///
/// SETUP:
/// 1. Crea un Canvas (Screen Space - Overlay) con un Panel negro semitransparente.
/// 2. Dentro del Panel:
///    - IntroText (TextMeshProUGUI) — centrado, grande
///    - ControlsPanel (GameObject vacío) — con los 5 íconos de teclas
///      - KeyW, KeyA, KeyS, KeyD, KeySpace (cada uno es un Image + Text hijo)
///    - SkipText (TextMeshProUGUI) — "Presiona cualquier tecla para continuar"
/// 3. Arrastra este script al Canvas y asigna las referencias en el Inspector.
/// </summary>
public class IntroScreen : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI introText;
    public GameObject controlsPanel;
    public TextMeshProUGUI skipText;
    public CanvasGroup screenGroup; // CanvasGroup del panel principal para hacer fade

    [Header("Texto introductorio")]
    [TextArea(3, 8)]
    public string locationText = "Estación Lunar Alfa-7\nCoordenadas: 23.4°N, 180.2°E";

    [TextArea(3, 8)]
    public string descriptionText = "Eres un explorador en una misión de reconocimiento.\nGravedad reducida. Terreno desconocido.\nRecolecta los cristales de energía antes de que se acabe el tiempo.";

    [Header("Velocidades")]
    public float typeSpeed = 0.04f;       // Segundos por carácter
    public float pauseBetweenBlocks = 1f; // Pausa entre bloque 1 y 2
    public float controlsFadeSpeed = 1.5f; // Velocidad del fade de controles

    [Header("Auto-ocultar")]
    public float autoHideAfter = 8f; // Segundos antes de ocultarse solo (0 = nunca)

    // Estado interno
    private bool skipped = false;
    private CanvasGroup controlsGroup;

    void Start()
    {
        // Ocultar controles al inicio
        controlsGroup = controlsPanel.GetComponent<CanvasGroup>();
        if (controlsGroup == null)
            controlsGroup = controlsPanel.AddComponent<CanvasGroup>();

        controlsGroup.alpha = 0f;
        controlsPanel.SetActive(true);

        if (skipText != null) skipText.alpha = 0f;

        introText.text = "";

        // Bloquear movimiento del jugador mientras dura la intro
        Time.timeScale = 1f; // No congelamos el tiempo, solo bloqueamos input si quieres

        StartCoroutine(PlayIntro());
    }

    void Update()
    {
        // Cualquier tecla salta la intro
        if (!skipped && Input.anyKeyDown)
        {
            skipped = true;
            StopAllCoroutines();
            HideScreen();
        }
    }

    IEnumerator PlayIntro()
    {
        // — Bloque 1: ¿Dónde estoy? —
        yield return StartCoroutine(Typewrite(locationText));
        yield return new WaitForSeconds(pauseBetweenBlocks);

        // — Bloque 2: ¿Qué es esto? —
        introText.text += "\n\n";
        yield return StartCoroutine(Typewrite(descriptionText));
        yield return new WaitForSeconds(pauseBetweenBlocks * 0.5f);

        // — Mostrar controles —
        yield return StartCoroutine(FadeIn(controlsGroup, controlsFadeSpeed));

        // — Mostrar "presiona para continuar" —
        if (skipText != null)
            yield return StartCoroutine(FadeInText(skipText, 0.8f));

        // — Auto-ocultar si está configurado —
        if (autoHideAfter > 0f)
        {
            yield return new WaitForSeconds(autoHideAfter);
            if (!skipped) HideScreen();
        }
    }

    IEnumerator Typewrite(string text)
    {
        foreach (char c in text)
        {
            introText.text += c;
            // Pausa más larga en puntuación para efecto dramático
            float delay = (c == '.' || c == '\n' || c == ',') ? typeSpeed * 6f : typeSpeed;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator FadeIn(CanvasGroup group, float speed)
    {
        group.alpha = 0f;
        while (group.alpha < 1f)
        {
            group.alpha += Time.deltaTime * speed;
            yield return null;
        }
        group.alpha = 1f;
    }

    IEnumerator FadeInText(TextMeshProUGUI tmp, float speed)
    {
        float a = 0f;
        while (a < 1f)
        {
            a += Time.deltaTime * speed;
            tmp.alpha = a;
            yield return null;
        }
        tmp.alpha = 1f;
    }

    void HideScreen()
    {
        StartCoroutine(FadeOutAndDisable());
    }

    IEnumerator FadeOutAndDisable()
    {
        if (screenGroup != null)
        {
            while (screenGroup.alpha > 0f)
            {
                screenGroup.alpha -= Time.deltaTime * 2f;
                yield return null;
            }
        }
        gameObject.SetActive(false);

        // Opcional: desbloquear cursor si lo tenías bloqueado antes de la intro
        // Cursor.lockState = CursorLockMode.Locked;
    }
}
