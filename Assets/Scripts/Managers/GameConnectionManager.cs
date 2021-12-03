using Nakama;
using Nakama.TinyJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameConnectionManager : MonoBehaviour
{
    [SerializeField] public NakamaConnection nakamaConnection;
    UiConnectionManager uiConnectionManager;
    PickerManager pickerManager;
    HostManager hostManager;
    private PlayersManager _playersManager;

    public List<string> playersSessionIDs;
    private IUserPresence localUser;
    [SerializeField] public string localUserSessionID;
    int playerCount = 0;

    bool isHost = false;
    bool isHostSet = false;

    async void Awake()
    {
        playersSessionIDs = new List<string>();
        uiConnectionManager = FindObjectOfType<UiConnectionManager>();
        hostManager = FindObjectOfType<HostManager>();
        pickerManager = FindObjectOfType<PickerManager>();
        _playersManager = FindObjectOfType<PlayersManager>();

        await Connect();
    }
    public async Task Connect()
    {
        await nakamaConnection.Connect();
        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();
        nakamaConnection.socket.ReceivedMatchmakerMatched += T => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(T));
        nakamaConnection.socket.ReceivedMatchPresence += T => mainThread.Enqueue(() => OnReceivedMatchPresence(T));
        // nakamaConnection.socket.ReceivedMatchState += m => mainThread.Enqueue(async () => await OnReceivedMatchState(m));
        nakamaConnection.socket.ReceivedMatchState += m => mainThread.Enqueue( () => OnReceivedMatchState(m));
    }
    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
    {
        print("Matchs encontrado");
        localUser = matched.Self.Presence;
        localUserSessionID = localUser.SessionId;

        IMatch match = await nakamaConnection.socket.JoinMatchAsync(matched);
        foreach (IMatchmakerUser user in matched.Users)
        {
            if (!playersSessionIDs.Contains(user.Presence.SessionId))
            {
                playersSessionIDs.Add(user.Presence.SessionId);
                playerCount++;
            }
        }
        uiConnectionManager.ActivateMatchFound();
        nakamaConnection.SetCurrentMatch(match);

        // Debug.Log(playersSessionIDs.Count);
        // Debug.Log(playerCount);
        if (nakamaConnection.connections == playerCount)
        {
            Debug.Log("Match is full");
            // await StartHost();
            StartHost();
        }
    }
    // private async void OnReceivedMatchPresence(IMatchPresenceEvent presence)
    private void OnReceivedMatchPresence(IMatchPresenceEvent presence)
    {
        foreach (IUserPresence user in presence.Joins)
        {
            if (!playersSessionIDs.Contains(user.SessionId))
            {
                playersSessionIDs.Add(user.SessionId);
                playerCount++;
            }
        }

        // For each player that leaves, despawn their player.
        foreach (var user in presence.Leaves)
        {
            if (playersSessionIDs.Contains(user.SessionId))
            {
                playersSessionIDs.Remove(user.SessionId);
                playerCount--;
            }
        }

        // presence.SetPlayerCountText(playersSessionIDs.Count, maxPlayers);
    }
    // private async Task OnReceivedMatchState(IMatchState state)
    private void OnReceivedMatchState(IMatchState state)
    {
        switch (state.OpCode)
        {

            case OpCodes.StartGame:
                if(!isHost)
                {
                    uiConnectionManager.StartGame();
                    HostManager.isGameStarted = true;
                }
                _playersManager.StartGame();
                break;
        }
    }

    void RegisterEvents()
    {
        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();

    }

    public async void FindMatch()
    {
        await nakamaConnection.FindMatch(nakamaConnection.connections, nakamaConnection.connections);
        uiConnectionManager.FindMatch();
    }



    public async void CanelMatchMacking()
    {
        await nakamaConnection.CancelMatchmaking();
        uiConnectionManager.CancelMatch();
    }


    public async Task LeaveMatch()
    {
        uiConnectionManager.ResetUI();
        // FindObjectOfType<InGameUIController>().ResetUI();
        // FindObjectOfType<PlayersManager>().KillPlayers();
        playerCount = 0;
        playersSessionIDs = new List<string>();
        isHost = false;
        isHostSet = false;
        //Send message that the player left
        // string jsonState = MatchDataJson.SetUserID(localUserSessionID);
        // await SendMatchStateAsync(OpCodes.PlayerLeft, jsonState);

        await nakamaConnection.LeaveMatch();
    }



    void StartHost()
    {
        SetHost();
        uiConnectionManager.StartPicker();

    }

    void SetHost()
    {
        if (!isHostSet)
        {
            playersSessionIDs.Sort();
            isHost = localUserSessionID == playersSessionIDs[0];
            isHostSet = true;
            Debug.Log("Host: " + playersSessionIDs[0]);
            Debug.Log("Local: " + localUserSessionID);
            Debug.Log("isHost: " + isHost);

            uiConnectionManager.SetHost();
            hostManager.SetHost(playersSessionIDs, playersSessionIDs[0], localUserSessionID, isHost);

        }
    }
}
