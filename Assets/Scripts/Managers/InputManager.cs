using System.Threading.Tasks;
using UnityEngine;
using Nakama;

public class InputManager : InputHandler
{
    private NakamaConnection _nakama;
    private PlayersManager _playersManager;
    private Inputs _inputs;
    private void Start()
    {
        _inputs = new Inputs(Vector2.zero);
        UnityMainThreadDispatcher mainThread = UnityMainThreadDispatcher.Instance();
        _nakama = GameObject.FindObjectOfType<GameConnectionManager>().nakamaConnection;
        _playersManager = GameObject.FindObjectOfType<PlayersManager>();

        _nakama.socket.ReceivedMatchState += m => mainThread.Enqueue(() => OnReceivedMatchState(m));
    }

    public async void SendInputs(Inputs inputs)
    {
        if (HostManager.isGameStarted)
        {   
            _playersManager.Move(new Vector2(i_axis,0f), HostManager.sessionID);
            if (HostManager.isHost)
            {
                Debug.Log("Hoster Inputs:" + inputs.horizontalInput + " " + inputs.jump + " " + inputs.attack + " " + inputs.sAtack + " " + inputs.mousePosition);
            }
            else
            {
                Debug.Log("Sending Inputs:" + inputs.horizontalInput + " " + inputs.jump + " " + inputs.attack + " " + inputs.sAtack + " " + inputs.mousePosition);
                await _nakama.socket.SendMatchStateAsync(_nakama.currentMatch.Id, OpCodes.Input, inputs.ToJson());
            }
        }
    }
    public void OnReceivedMatchState(IMatchState matchState)
    {
        if (matchState.OpCode == OpCodes.Input)
        {
            if (HostManager.isHost)
            {
                Inputs inputs = new Inputs(matchState.State);
                Debug.Log("Recive Inputs:" + inputs.horizontalInput + " " + inputs.jump + " " + inputs.attack + " " + inputs.sAtack + " " + inputs.mousePosition);
            }

        }
    }
    void Update()
    {
        i_axis = Input.GetAxis("Horizontal");
        i_atack = Input.GetMouseButtonDown(0);
        i_special_atack = Input.GetMouseButtonDown(1);
        i_jump = Input.GetButtonDown("Jump");

        if (_inputs.HasdInputsChanged(i_axis, i_jump, i_atack, i_special_atack))
        {
        print(_inputs.horizontalInput);
            if (i_atack || i_special_atack)
            {
                _inputs = new Inputs(Vector2.zero, i_axis, i_atack, i_jump, i_special_atack);
                SendInputs(_inputs);
            }
            else
            {
                _inputs = new Inputs(Vector2.zero, i_axis, i_atack, i_jump, i_special_atack);
                SendInputs(_inputs);
            }

        }
    }
}