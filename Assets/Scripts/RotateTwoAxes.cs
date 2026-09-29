using UnityEngine;

public class RotateTwoAxes : MonoBehaviour
{
    public float speedX = 40f;
    public float speedY = 60f;
    void Update() => transform.Rotate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0f);
}
