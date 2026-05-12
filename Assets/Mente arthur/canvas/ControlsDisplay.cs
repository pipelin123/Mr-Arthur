using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Genera los gráficos de los controles (WASD + Espacio) en runtime.
///
/// OPCIÓN A — Automático (recomendada):
///   Pon este script en el ControlsPanel y deja que cree los botones solo.
///   No necesitas crear nada en el Inspector, solo asegúrate de que ControlsPanel
///   tenga un RectTransform con tamaño razonable (ej. 400x200).
///
/// OPCIÓN B — Manual:
///   Crea tú mismo los 5 GameObjects de teclas en el Inspector y asígnalos abajo.
/// </summary>
public class ControlsDisplay : MonoBehaviour
{
    [Header("Solo si usas Opción B (manual)")]
    public Image keyW, keyA, keyS, keyD, keySpace;

    [Header("Estilo de teclas")]
    public Color keyColor = new Color(0.15f, 0.15f, 0.15f, 0.92f);        // Fondo oscuro
    public Color keyBorderColor = new Color(0.6f, 0.6f, 0.6f, 1f);        // Borde gris
    public Color keyTextColor = Color.white;
    public Color labelColor = new Color(0.8f, 0.8f, 0.8f, 1f);            // Texto descriptivo

    [Header("Fuente (opcional)")]
    public TMP_FontAsset font; // Arrastra una fuente si tienes, si no usa la default

    private void Start()
    {
        // Si no se asignaron manualmente, construimos los controles en runtime
        if (keyW == null)
            BuildControls();
    }

    void BuildControls()
    {
        // Limpiar hijos previos
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // Layout raíz: vertical — fila superior (WASD) + barra espacio
        RectTransform rt = GetComponent<RectTransform>();

        // — Fila WASD —
        GameObject wasdRow = CreateGroup("WASD_Row", transform, new Vector2(0, 30), new Vector2(200, 55));
        SetHorizontalLayout(wasdRow, 8);

        CreateKey(wasdRow.transform, "A", "");
        CreateKey(wasdRow.transform, "W", "");
        CreateKey(wasdRow.transform, "S", "");
        CreateKey(wasdRow.transform, "D", "");

        // Etiqueta de movimiento
        CreateLabel(wasdRow.transform, "Mover");

        // — Barra de espacio —
        GameObject spaceRow = CreateGroup("Space_Row", transform, new Vector2(0, -30), new Vector2(320, 55));
        SetHorizontalLayout(spaceRow, 8);

        CreateSpaceBar(spaceRow.transform);
        CreateLabel(spaceRow.transform, "Saltar");

        // Colocar filas verticalmente usando un layout vertical en el padre
        VerticalLayoutGroup vlg = gameObject.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 12;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = false;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = false;
        vlg.childForceExpandHeight = false;

        // Ajustar ContentSizeFitter al padre
        ContentSizeFitter csf = gameObject.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    // ── Helpers de construcción ───────────────────────────────────────────────

    GameObject CreateGroup(string name, Transform parent, Vector2 anchoredPos, Vector2 size)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = anchoredPos;
        return go;
    }

    void SetHorizontalLayout(GameObject go, float spacing)
    {
        HorizontalLayoutGroup hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = spacing;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;
    }

    GameObject CreateKey(Transform parent, string keyLabel, string actionLabel)
    {
        // Contenedor de la tecla
        GameObject keyGo = new GameObject($"Key_{keyLabel}", typeof(RectTransform), typeof(Image));
        keyGo.transform.SetParent(parent, false);

        RectTransform rt = keyGo.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(48, 48);

        // Fondo de la tecla
        Image bg = keyGo.GetComponent<Image>();
        bg.color = keyColor;
        bg.sprite = CreateRoundedSprite();

        // Borde (hijo con Outline o segundo Image ligeramente más grande)
        GameObject border = new GameObject("Border", typeof(RectTransform), typeof(Image));
        border.transform.SetParent(keyGo.transform, false);
        RectTransform brt = border.GetComponent<RectTransform>();
        brt.anchorMin = Vector2.zero;
        brt.anchorMax = Vector2.one;
        brt.offsetMin = new Vector2(-2, -2);
        brt.offsetMax = new Vector2(2, 2);
        Image bImg = border.GetComponent<Image>();
        bImg.color = keyBorderColor;
        bImg.sprite = CreateRoundedSprite();
        border.transform.SetSiblingIndex(0); // detrás

        // Etiqueta de la tecla (W, A, S, D)
        GameObject labelGo = new GameObject("KeyLabel", typeof(RectTransform));
        labelGo.transform.SetParent(keyGo.transform, false);
        RectTransform lrt = labelGo.GetComponent<RectTransform>();
        lrt.anchorMin = Vector2.zero;
        lrt.anchorMax = Vector2.one;
        lrt.offsetMin = Vector2.zero;
        lrt.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = labelGo.AddComponent<TextMeshProUGUI>();
        tmp.text = keyLabel;
        tmp.fontSize = 22;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = keyTextColor;
        tmp.alignment = TextAlignmentOptions.Center;
        if (font != null) tmp.font = font;

        return keyGo;
    }

    GameObject CreateSpaceBar(Transform parent)
    {
        GameObject keyGo = new GameObject("Key_Space", typeof(RectTransform), typeof(Image));
        keyGo.transform.SetParent(parent, false);

        RectTransform rt = keyGo.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(180, 40);

        Image bg = keyGo.GetComponent<Image>();
        bg.color = keyColor;
        bg.sprite = CreateRoundedSprite();

        // Borde
        GameObject border = new GameObject("Border", typeof(RectTransform), typeof(Image));
        border.transform.SetParent(keyGo.transform, false);
        RectTransform brt = border.GetComponent<RectTransform>();
        brt.anchorMin = Vector2.zero;
        brt.anchorMax = Vector2.one;
        brt.offsetMin = new Vector2(-2, -2);
        brt.offsetMax = new Vector2(2, 2);
        Image bImg = border.GetComponent<Image>();
        bImg.color = keyBorderColor;
        bImg.sprite = CreateRoundedSprite();
        border.transform.SetSiblingIndex(0);

        // Texto "ESPACIO"
        GameObject labelGo = new GameObject("KeyLabel", typeof(RectTransform));
        labelGo.transform.SetParent(keyGo.transform, false);
        RectTransform lrt = labelGo.GetComponent<RectTransform>();
        lrt.anchorMin = Vector2.zero;
        lrt.anchorMax = Vector2.one;
        lrt.offsetMin = Vector2.zero;
        lrt.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = labelGo.AddComponent<TextMeshProUGUI>();
        tmp.text = "ESPACIO";
        tmp.fontSize = 14;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = keyTextColor;
        tmp.alignment = TextAlignmentOptions.Center;
        if (font != null) tmp.font = font;

        return keyGo;
    }

    void CreateLabel(Transform parent, string text)
    {
        GameObject labelGo = new GameObject("ActionLabel", typeof(RectTransform));
        labelGo.transform.SetParent(parent, false);

        RectTransform rt = labelGo.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(80, 40);

        TextMeshProUGUI tmp = labelGo.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 14;
        tmp.color = labelColor;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) tmp.font = font;
    }

    /// <summary>
    /// Crea un sprite de rectángulo con esquinas redondeadas en runtime.
    /// Unity 2021+ tiene UI Toolkit pero para uGUI usamos una textura programática.
    /// </summary>
    Sprite CreateRoundedSprite()
    {
        int size = 64;
        int radius = 10;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                pixels[y * size + x] = IsInsideRoundedRect(x, y, size, size, radius)
                    ? Color.white
                    : Color.clear;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f,
                             0, SpriteMeshType.Tight,
                             new Vector4(radius, radius, radius, radius)); // 9-slice borders
    }

    bool IsInsideRoundedRect(int px, int py, int w, int h, int r)
    {
        int cx = px < r ? r : (px > w - 1 - r ? w - 1 - r : px);
        int cy = py < r ? r : (py > h - 1 - r ? h - 1 - r : py);
        float dx = px - cx;
        float dy = py - cy;
        return dx * dx + dy * dy <= r * r;
    }
}
