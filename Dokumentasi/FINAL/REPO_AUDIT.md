# Audit Ringkas Repository Awal

Repository acuan: `ihdyrtg/AR-2026-NIM-NAMA` branch `main`.

Kondisi yang diperiksa sebelum paket final dibuat:

- Unity Editor version: `6000.2.9f1`.
- Universal Render Pipeline: `17.2.0`.
- Input System: `1.14.2`.
- Active Input Handling pada ProjectSettings bernilai `Both`.
- `EditorBuildSettings.asset` belum memiliki scene build (`m_Scenes: []`).
- Tag awal yang terlihat: `Crate`, `Dirt`, dan `CinemachineTarget`; tag `Target` dan `Obstacle` belum ada.
- Assets utama masih didominasi template Unity Essentials/TextMesh Pro; pencarian repository belum menemukan `PlayerMovement`, `RotateObject`, atau scene bernama `Pertemuan...`.

Karena itu, paket ini tidak mengganti project template. Paket menambahkan overlay file praktikum dan menyediakan Editor Builder untuk membuat struktur P02-P07 secara konsisten.
