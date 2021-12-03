using System.Threading.Tasks;
using UnityEngine;
using Nakama;


public class PickerManager : MonoBehaviour
{
    // GameConnectionManager gameConnectionManager;
    NakamaConnection nakama;
    UiConnectionManager uiConnectionManager;
    private PlayersManager _playersManager;
    public bool[] characters = { false, false, false, false, false, false, false, false };
    //TODO: set players to 0
    private int players = 6;
    private bool hasSelected = false;

    void Awake()
    {
        // gameConnectionManager = GameObject.Find("GameConnectionManager").GetComponent<GameConnectionManager>();
        nakama = GameObject.FindObjectOfType<GameConnectionManager>().nakamaConnection;

        uiConnectionManager = FindObjectOfType<UiConnectionManager>();
        _playersManager = FindObjectOfType<PlayersManager>();

        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();
        nakama.socket.ReceivedMatchState += m => mainThread.Enqueue(async () => await OnReceivedMatchState(m));
    }
    public async void ChosseCharacter(int character)
    {
        if (!hasSelected)
        {
            if (HostManager.isHost) await HostPicker(character, HostManager.hostId, true);
            else
            {
                CharSelector chara = new CharSelector(character, false);
                await nakama.socket.SendMatchStateAsync(nakama.currentMatch.Id, OpCodes.charSelector, chara.ToJson());
            }
        }
        // SelectCharacter(character);
    }
    public void SelectCharacter(int character)
    {
        characters[character - 1] = true;
        uiConnectionManager.SetSelected(character);
    }
    public async Task HostPicker(int character, string sessionID, bool isFromHost = false)
    {
        if (HostManager.characters[character - 1] == "")
        {
            HostManager.characters[character - 1] = sessionID;
            SelectCharacter(character);
            CharSelector notification = new CharSelector(character, true, sessionID);
            await nakama.socket.SendMatchStateAsync(nakama.currentMatch.Id, OpCodes.charSelector, notification.ToJson());
            if (isFromHost)
            {
                hasSelected = true;
            }
            players++;
            
            if (players ==8) StartGame();
        }
        else
            return;

    }
    public void NetworkPicker(CharSelector charact)
    {
        HostManager.characters[charact.CharacterIndex - 1] = charact.sessionID;
        uiConnectionManager.SetSelected((int)charact.CharacterIndex);
        if (charact.sessionID == HostManager.sessionID)
            hasSelected = true;

    }

    private async void StartGame()
    {
        Debug.Log("StartGame");
        await nakama.socket.SendMatchStateAsync(nakama.currentMatch.Id, OpCodes.StartGame, "0");
        uiConnectionManager.StartGame();
        HostManager.isGameStarted = true;
        _playersManager.StartGame();
    }

    private async Task OnReceivedMatchState(IMatchState state)
    {
        switch (state.OpCode)
        {
            case OpCodes.charSelector:
                CharSelector resp = new CharSelector(state.State);
                if (!resp.Broadcast && HostManager.isHost)
                {
                    await HostPicker((int)resp.CharacterIndex, state.UserPresence.SessionId);
                }
                else
                {
                    NetworkPicker(resp);
                }
                break;
        }
    }
}
