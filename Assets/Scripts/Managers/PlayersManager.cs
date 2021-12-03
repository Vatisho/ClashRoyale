using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public GameObject rana;
    public GameObject ant;

    private string _clientId;
    private Dictionary<string, GameObject> _players;
    void Start()
    {
        _players = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        print("Inicia la fiestona!!!!!!!!!!!!!!");
        _clientId = HostManager.sessionID;
        _players.Add(HostManager.characters[0], rana);
        _players.Add(HostManager.characters[1], ant);
    }
    public void Move(Vector2 velocity, string playerID)
    {
        if (playerID == _clientId)
        {
            print("Soy sho");
            _players[playerID].GetComponent<Player>().Move(velocity);
        }
    }


}
