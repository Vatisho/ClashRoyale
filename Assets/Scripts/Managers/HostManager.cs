using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HostManager : MonoBehaviour
{
    private GameConnectionManager gameConnectionManager;
    private List<string> _playersSessionIDs;
    public static string hostId{ get; private set; }
    public static string sessionID{ get; private set; }
    public static bool isHost{ get; private set; }
    public static bool isGameStarted= false;
    public static string [] characters = {"","","","","","","",""};

    void Awake()
    {
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
    }

    public void SetHost(List<string> playersSessionIDs, string hostId, string sessionID, bool isHost)
    {
        Debug.Log("Si te creo el host retrasado mental hijo de la gran puta madre de la muerte que te creo el host");
        this._playersSessionIDs = playersSessionIDs;
        HostManager.hostId = hostId;
        HostManager.sessionID = sessionID;
        HostManager.isHost = isHost;
    }
}
