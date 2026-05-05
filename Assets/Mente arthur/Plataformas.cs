using UnityEngine;

public class Plataformas : MonoBehaviour
{
    public float velocidad = 1f;   // velocidad del movimiento
    public float altura = 0.05f;   // cuánto sube y baja

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float movimientoY = Mathf.Sin(Time.time * velocidad) * altura;
        transform.position = posicionInicial + new Vector3(0, movimientoY, 0);
    }
}