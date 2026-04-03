using UnityEngine;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    public void StartHost()
    {
        if (NetworkManager.Singleton == null) { Debug.LogError("NetworkManager not found"); return; }
        Debug.Log("Starting Host...");
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton == null) { Debug.LogError("NetworkManager not found"); return; }
        Debug.Log("Starting Client...");
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        if (NetworkManager.Singleton == null) { Debug.LogError("NetworkManager not found"); return; }
        Debug.Log("Starting Server...");
        NetworkManager.Singleton.StartServer();
    }

    public void StopNetwork()
    {
        if (NetworkManager.Singleton == null) { Debug.LogError("NetworkManager not found"); return; }
        Debug.Log("Stopping Network...");
        NetworkManager.Singleton.Shutdown();
    }
}
