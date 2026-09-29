# Checklist Project AR Pertemuan 2-7

## P02 - Scene 3D

- Scene `Pertemuan02_Diorama.unity`.
- Ground + Collider.
- Minimal lima primitive.
- Minimal tiga material.
- Directional Light.
- Minimal satu Rigidbody + Collider untuk uji gravitasi.
- Minimal satu prefab dipakai minimal dua kali.
- Hierarchy memiliki kelompok `Environment` dan `Props`.
- Console tanpa error merah.

## P03 - Camera dan Audio

- Scene `Pertemuan03_CameraAudio.unity` sebagai salinan P02.
- Main Camera dengan komposisi dan FOV yang layak.
- Satu ambience 2D (`Spatial Blend = 0`).
- Minimal dua sumber audio 3D (`Spatial Blend = 1`).
- Perubahan volume berdasarkan jarak.
- Hanya satu Audio Listener aktif.

## P04 - C# Dasar

- `Assets/Scripts/RotateObject.cs`.
- `Assets/Scripts/MoveObject.cs`.
- Satu script perilaku ketiga; paket memakai `PulseScale.cs`.
- Parameter public terlihat di Inspector.
- Gerak/rotasi memakai `Time.deltaTime`.
- Console bersih dari error compile.

## P05 - Interaction

- Scene `Pertemuan05_Interaction.unity`.
- Floor + Collider.
- Player + Collider + Rigidbody.
- Input gerak.
- Target: Collider `Is Trigger = On`, Tag `Target`.
- Obstacle: Collider normal, Tag `Obstacle`.
- `OnTriggerEnter` mendeteksi Target.
- `OnCollisionEnter` mendeteksi Obstacle.
- if/else membedakan respons.
- Obstacle sebagai prefab dan memiliki beberapa instance.
- Target nonaktif setelah disentuh.
- Debug.Log hanya muncul saat event, bukan setiap frame.

## P06 - Android Environment

- Android Build Support terpasang pada versi Unity Editor yang dipakai.
- Android SDK & NDK Tools + OpenJDK terpasang.
- External Tools valid.
- Developer Options + USB Debugging aktif.
- RSA fingerprint disetujui.
- Project switch ke Android.
- Company Name, Product Name, Package Identifier terisi.
- Smartphone terdeteksi sebagai Run Device / `adb devices` berstatus `device`.

## P07 - Build dan Deploy

- Project berjalan di Editor tanpa error merah.
- Scene utama masuk daftar build.
- Android aktif.
- Product identity konsisten.
- Orientasi sesuai desain.
- Minimum API, Target API, ARM64 mengikuti standar kelas; paket otomatis menambahkan ARM64 tetapi API level tetap Anda verifikasi.
- Smartphone dipilih sebagai Run Device.
- APK dibuat di `Builds/Android/P07_NIM_Nama.apk`.
- Aplikasi terpasang dan dibuka di smartphone.
- Input, collision/trigger, UI, dan audio diuji pada perangkat nyata.
- Screenshot/video dan catatan kendala disimpan.
