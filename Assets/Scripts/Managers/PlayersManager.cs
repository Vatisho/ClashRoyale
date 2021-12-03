using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

public class PlayersManager : MonoBehaviour
{
    public GameObject rana;
    public GameObject ant;
    public int ticksPerSecond = 60;

    NakamaConnection _nakama;
    private string _clientId;
    private bool _hasStarted = false;
    private Dictionary<string, GameObject> _players;
    private float _lastUpdate;
    void Start()
    {
        _players = new Dictionary<string, GameObject>();
        _lastUpdate = Time.timeSinceLevelLoad;
        _nakama = GameObject.FindObjectOfType<GameConnectionManager>().nakamaConnection;
        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();
        _nakama.socket.ReceivedMatchState += m => mainThread.Enqueue(() => OnReceivedMatchState(m));

    }

    void Update()
    {
        if (_hasStarted && HostManager.isHost)
        {
            if (Time.timeSinceLevelLoad > _lastUpdate + (60 / ticksPerSecond))
            {
                foreach (var player in _players)
                {
                    Player playerComp = player.Value.GetComponent<Player>();
                    _nakama.socket.SendMatchStateAsync(_nakama.currentMatch.Id, OpCodes.VelocityAndPosition, new VelocityAndPosition(playerComp.velocity, playerComp.position).ToJson());
                }
            }
        }


    }

    public void StartGame()
    {
        _clientId = HostManager.sessionID;
        _players.Add(HostManager.characters[0], rana);
        _players.Add(HostManager.characters[1], ant);
        foreach (var player in _players)
        {
            if(!HostManager.isHost){
                player.Value.AddComponent<LocalSync>();
            }
        }
        _hasStarted = true;
    }
    public void Move(Vector2 velocity, string playerID)
    {
        if (playerID == _clientId)
        {
            _players[playerID].GetComponent<Player>().Move(velocity);
        }
    }

    private void OnReceivedMatchState(IMatchState state)
    {
        if(state.OpCode == OpCodes.VelocityAndPosition)
        {
            VelocityAndPosition vp = new VelocityAndPosition(state.State);
;
        
            _players[state.UserPresence.SessionId].GetComponent<LocalSync>().Sync(vp.velocity, vp.position);
        }
    }

}
