Scrummy — Unity 3D multiplayer

Overview
- Engine: Unity (LTS recommended)
- Genre: 3D multiplayer game
- Targets: Windows, macOS, Linux (desktop); Android planned

Decisions
- Multiplayer: start with Unity Netcode for GameObjects (NGO) and Unity Gaming Services (Relay/Authentication). Photon is an alternative if lower-latency peer-to-peer is needed.
- Networking model: host-client for quick iteration; optional dedicated server later.

Quick start
1. Install Unity Hub and an LTS Editor (2022.3+ recommended).
2. Create a new project using the "3D" template and name it `Scrummy`.
3. Add this repo as a remote and commit the project files (remember to keep large Builds out).
4. Import Netcode: Window → Package Manager → + (Add package from git URL) → `https://github.com/Unity-Technologies/com.unity.netcode.gameobjects.git`
5. Follow `unity-project/README.md` for detailed setup steps.

Next steps
- Create base scene with `NetworkManager` and test host/local multiplayer.
- Implement core gameplay loop and authoritative state sync.
- Add cross-platform build targets and CI.
