using UnityEngine;

public class MobileHUD : MonoBehaviour
{
    [Tooltip("Aktifkan agar D-pad juga terlihat di Game View saat diuji dari Editor.")]
    public bool showOnDesktop = true;

    [Tooltip("Ukuran tombol D-pad dalam pixel.")]
    public float buttonSize = 72f;

    private GUIStyle titleStyle;
    private GUIStyle infoStyle;
    private GUIStyle buttonStyle;

    void OnDisable()
    {
        MobileInputState.Move = Vector2.zero;
    }

    void OnGUI()
    {
        EnsureStyles();

        GUI.Label(new Rect(16, 12, 520, 40), "AR Praktikum - Pertemuan 5-7", titleStyle);
        GUI.Label(new Rect(16, 48, 620, 50), "Tujuan: gerakkan Player, hindari obstacle, dan sentuh Target.", infoStyle);

        if (!Application.isMobilePlatform && !showOnDesktop)
        {
            MobileInputState.Move = Vector2.zero;
            return;
        }

        float size = Mathf.Clamp(buttonSize, 52f, 120f);
        float margin = 24f;
        float x = margin + size;
        float y = Screen.height - margin - (size * 2f);

        bool up = GUI.RepeatButton(new Rect(x, y, size, size), "UP", buttonStyle);
        bool left = GUI.RepeatButton(new Rect(x - size, y + size, size, size), "LEFT", buttonStyle);
        bool down = GUI.RepeatButton(new Rect(x, y + size, size, size), "DOWN", buttonStyle);
        bool right = GUI.RepeatButton(new Rect(x + size, y + size, size, size), "RIGHT", buttonStyle);

        Vector2 move = Vector2.zero;
        if (left) move.x -= 1f;
        if (right) move.x += 1f;
        if (down) move.y -= 1f;
        if (up) move.y += 1f;
        MobileInputState.Move = move.normalized;
    }

    private void EnsureStyles()
    {
        if (titleStyle != null) return;

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold
        };

        infoStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            wordWrap = true
        };

        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };
    }
}
