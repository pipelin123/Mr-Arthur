using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private AudioSource audioSource;
    private bool activated = false;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            audioSource.Play();
            activated = true;
        }
    }
}