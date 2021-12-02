using System.Threading.Tasks;
using System;
using UnityEngine;
using Nakama;

[Serializable]
[CreateAssetMenu]
public class NakamaConnection : ScriptableObject
{
    public string scheme = "http";
    public string host = "localhost";
    public int port = 7350;
    public string serverKey = "defaultkey";
    public int connections= 8;
    public IClient client;
    public ISession session;
    public ISocket socket;

    private const string _SessionPrefName = "nakama.session";
    private const string _DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";
    private IMatchmakerTicket _ticket;
    public IMatch currentMatch{get; private set;}

    public async Task Connect(){

        
        client = new Client(scheme, host, port, serverKey);

        string authToken = PlayerPrefs.GetString(_SessionPrefName);
        if (!string.IsNullOrEmpty(authToken))
        {
            ISession s = Nakama.Session.Restore(authToken);
            if (!s.IsExpired)
            {
                session = s;
            }
        }
        if (session == null)
        {
            string deviceId;

            if (PlayerPrefs.HasKey(_DeviceIdentifierPrefName))
            {
                deviceId = PlayerPrefs.GetString(_DeviceIdentifierPrefName);
            }
            else
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                if (deviceId == SystemInfo.unsupportedIdentifier)
                {
                    deviceId = System.Guid.NewGuid().ToString();
                }

                PlayerPrefs.SetString(_DeviceIdentifierPrefName, deviceId);
            }
            session = await client.AuthenticateDeviceAsync(deviceId);
            
            PlayerPrefs.SetString(_SessionPrefName, session.AuthToken);
        }
        socket = client.NewSocket();
        await socket.ConnectAsync(session, true);
        Debug.Log("Connected to Nakama server");
    }

    public async Task FindMatch(int minPlayers, int maxPlayers)
    {
        Debug.Log("Finding Match");
         _ticket = await socket.AddMatchmakerAsync("*", minPlayers, maxPlayers);
    }

    public async Task CancelMatchmaking()
    {
        Debug.Log("Cancel Matchmaking");
        await socket.RemoveMatchmakerAsync(_ticket);
    }
    public async Task LeaveMatch()
    {
        Debug.Log("Leave Match");
        await socket.LeaveMatchAsync(currentMatch.Id);
    }

    public void SetCurrentMatch(IMatch currentMatch)
    {
        this.currentMatch = currentMatch;
    }
}
