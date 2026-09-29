using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float touchRadius = 120f;

    private Rigidbody rb;
    private float horizontal;
    private float vertical;
    private Vector2 mobileInput;
    private Vector2 touchStart;

    void Awake() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) touchStart = touch.position;
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                mobileInput = Vector2.ClampMagnitude((touch.position - touchStart) / Mathf.Max(1f, touchRadius), 1f);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) mobileInput = Vector2.zero;
        }

        if (mobileInput.sqrMagnitude > 0.001f)
        {
            horizontal = mobileInput.x;
            vertical = mobileInput.y;
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetMobileInput(float x, float y) => mobileInput = Vector2.ClampMagnitude(new Vector2(x, y), 1f);
    public void ClearMobileInput() => mobileInput = Vector2.zero;
}
