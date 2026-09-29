using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI")]
    public SimpleHUD hud;

    [Header("Audio Feedback")]
    public AudioClip targetClip;
    public AudioClip obstacleClip;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        HandleContact(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleContact(collision.gameObject);
    }

    private void HandleContact(GameObject touchedObject)
    {
        if (touchedObject.CompareTag("Target"))
        {
            const string message = "Target tersentuh. Praktikum berhasil.";
            Debug.Log(message);

            if (hud != null)
                hud.SetStatus(message);

            PlayClip(targetClip, touchedObject.transform.position);
            touchedObject.SetActive(false);
        }
        else if (touchedObject.CompareTag("Obstacle"))
        {
            string message = "Obstacle tersentuh: " + touchedObject.name;
            Debug.Log(message);

            if (hud != null)
                hud.SetStatus(message);

            PlayClip(obstacleClip, touchedObject.transform.position);
        }
    }

    private void PlayClip(AudioClip clip, Vector3 position)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
    }
}
