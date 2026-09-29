# Peta File Final P02-P07

| Area | File/Folder | Fungsi |
|---|---|---|
| Scene | Assets/Scenes/Pertemuan02_Diorama.unity | Diorama, material, lighting, physics, prefab |
| Scene | Assets/Scenes/Pertemuan03_CameraAudio.unity | Camera, FOV, ambience 2D, dua audio 3D |
| Scene | Assets/Scenes/Pertemuan04_Scripting.unity | Tiga perilaku C# |
| Scene | Assets/Scenes/Pertemuan05_Interaction.unity | Mini aplikasi final untuk build Android |
| Script | Assets/Scripts/RotateObject.cs | Rotasi dengan Time.deltaTime |
| Script | Assets/Scripts/MoveObject.cs | Translate maju/mundur |
| Script | Assets/Scripts/FloatObject.cs | Perilaku ketiga naik-turun |
| Script | Assets/Scripts/PlayerMovement.cs | Input keyboard + dukungan touch, Rigidbody.MovePosition |
| Script | Assets/Scripts/PlayerInteraction.cs | Trigger/collision, Tag, if/else, respons Target |
| Script | Assets/Scripts/MobileMoveButton.cs | Tombol sentuh Android |
| Script | Assets/Scripts/SimpleHUD.cs | Status UI |
| Script | Assets/Scripts/CameraFollow.cs | Camera mengikuti Player pada scene final |
| Prefab | Assets/Prefabs/Chair_Prefab.prefab | Prefab P02, minimal dua instance |
| Prefab | Assets/Prefabs/Obstacle.prefab | Prefab obstacle P05 |
| Material | Assets/Materials/*.mat | Material lantai, dinding, meja, player, target, obstacle |
| Audio | Assets/Audio/ambience_ruangan.wav | Ambience 2D |
| Audio | Assets/Audio/sfx_mesin.wav | SFX 3D 1 |
| Audio | Assets/Audio/sfx_lonceng.wav | SFX 3D 2 |
| Editor | Assets/Editor/ARCourseProjectBuilder.cs | Generator, validator, identity, Android setup, build APK |
| Build | Builds/Android/P07_NIM_Nama.apk | Output setelah build di mesin mahasiswa |

## Yang tetap perlu dilakukan manual

P06-P07 mengandung bagian yang bergantung pada hardware dan instalasi lokal, sehingga tidak dapat "ditanam" dalam ZIP:

1. Android Build Support, SDK/NDK, OpenJDK harus terpasang pada Unity Editor yang dipakai.
2. Developer Options dan USB Debugging aktif di smartphone.
3. Kabel USB harus mendukung data.
4. RSA fingerprint harus disetujui di smartphone.
5. Device harus muncul sebagai Run Device/`adb devices` berstatus `device`.
6. Minimum API dan Target API mengikuti standar kelas/SDK lokal.
7. Screenshot/video pengujian smartphone dan catatan kendala harus dibuat dari perangkat nyata.
