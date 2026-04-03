Scene & Prefab Setup (Scrummy)

Purpose: step-by-step editor instructions to create a minimal networked scene and player prefab using Netcode for GameObjects (NGO).

1) Create main scene
- File → New Scene → Save as `Assets/Scenes/Main.unity`.
- In Hierarchy create an empty GameObject named `NetworkManager`.
- Add the `NetworkManager` component (Component → Netcode → NetworkManager).

2) Configure Transport
- In `NetworkManager`, add `NetworkManager"s" Transport` (Unity Transport) or your chosen transport.
- If using Unity Gaming Services Relay, configure Relay/UGS per docs and set the transport to use it.

3) Create Player prefab
- GameObject → 3D Object → Capsule (or model). Reset transform to 0,0,0.
- Add `NetworkObject` component.
- Add the script `NetworkPlayer` (Assets/Scripts/NetworkPlayer.cs).
- (Optional) Add a child Camera and a simple `AudioListener` for owner. In `NetworkPlayer.OnNetworkSpawn()` enable camera only if `IsOwner`.
- Create folder `Assets/Prefabs/` and drag the Capsule into it to make `Player.prefab`.

4) Register player prefab
- In `NetworkManager` assign `Player Prefab` to `Assets/Prefabs/Player.prefab`.

5) Add PlayerSpawner
- Create empty GameObject `Spawner`, add `PlayerSpawner` script (Assets/Scripts/PlayerSpawner.cs), and set `playerPrefab` to `Player.prefab`.

6) Test locally
- In Editor: press Play and use `Start Host` (you can add simple UI to call `NetworkManager.StartHost()`/`StartClient()`).
- Build a standalone (File → Build Settings → Add Open Scenes) and run host/client instances to test networking across processes.

7) Relay & UGS quick checklist
- Install `com.unity.services.core` and `com.unity.netcode.gameobjects` via Package Manager.
- Set up a Unity project in Unity Dashboard and enable Relay + Authentication.
- Implement auth flow (simple anonymous login) and request a Relay allocation for client connections.
- Hook the Relay transport into `NetworkManager.NetworkConfig` or use the UGS starter packages.

8) Cross-platform build notes
- Build Settings → Add Desktop platforms (Windows/Mac/Linux) via Unity Hub.
- For Android: install Android SDK & NDK through Unity Hub, switch platform to Android, configure package name and minimum SDK.

9) Next improvements
- Replace ServerRpc movement with client-side prediction + reconciliation for smoothness.
- Add lobby scene and match lifecycle.

