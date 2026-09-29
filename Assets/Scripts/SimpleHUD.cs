using UnityEngine;

public class SimpleHUD : MonoBehaviour
{
    public PlayerMovement player;
    public string defaultMessage = "Cari Target hijau. Hindari obstacle merah.";
    public bool showTouchControlsInEditor = true;
    private string status;

    void Start() => status = defaultMessage;
    public void SetStatus(string message) => status = message;

    void OnGUI()
    {
        GUI.Box(new Rect(15, 15, Mathf.Min(Screen.width - 30, 700), 70),
            "AR Praktikum P05/P07\n" + (string.IsNullOrEmpty(status) ? defaultMessage : status));

        if (!(Application.isMobilePlatform || showTouchControlsInEditor) || player == null) return;

        float s = Mathf.Clamp(Screen.height * 0.12f, 70f, 110f);
        float x = 25f;
        float y = Screen.height - (s * 2.2f) - 25f;
        Vector2 d = Vector2.zero;
        if (GUI.RepeatButton(new Rect(x + s, y, s, s), "UP")) d.y += 1f;
        if (GUI.RepeatButton(new Rect(x, y + s, s, s), "LEFT")) d.x -= 1f;
        if (GUI.RepeatButton(new Rect(x + s, y + s, s, s), "DOWN")) d.y -= 1f;
        if (GUI.RepeatButton(new Rect(x + s * 2f, y + s, s, s), "RIGHT")) d.x += 1f;
        if (d.sqrMagnitude > 0.001f) player.SetMobileInput(d.x, d.y);
    }
}
