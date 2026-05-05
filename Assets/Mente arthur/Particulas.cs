using UnityEngine;

public class BloqueParticulas : MonoBehaviour
{
    public ParticleSystem ps;

    void OnMouseDown()
    {
        var emission = ps.emission;
        emission.rateOverTime = 30;
    }
}