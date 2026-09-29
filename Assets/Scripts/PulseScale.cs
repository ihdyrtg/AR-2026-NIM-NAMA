using UnityEngine;

public class PulseScale : MonoBehaviour
{
    public float speed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, t);
    }
}
