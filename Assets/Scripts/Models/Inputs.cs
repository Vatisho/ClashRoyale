using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;


public class Inputs : Models, IToJson
{
    public float horizontalInput { get; private set; }
    public bool jump { get; private set; }
    public bool attack { get; private set; }
    public bool sAtack { get; private set; }
    public Vector2 mousePosition { get; private set; }

    public Inputs(Vector2 mouseposition, float horizontalInput = 0f, bool jump = false, bool attack = false, bool sAtack = false)
    {
        this.horizontalInput = horizontalInput;
        this.jump = jump;
        this.attack = attack;
        this.sAtack = sAtack;
        this.mousePosition = mouseposition == null ? Vector2.zero : mouseposition;
    }

    public Inputs(byte[] buffer){
        IDictionary<string, string> json = GetStateAsDictionary(buffer);
        this.horizontalInput = float.Parse(json["horizontalInput"]);
        this.jump = bool.Parse(json["jump"]);
        this.attack = bool.Parse(json["attack"]);
        this.sAtack = bool.Parse(json["sAtack"]);
        this.mousePosition = new Vector2(float.Parse(json["mousePosition_x"]), float.Parse(json["mousePosition_y"]));
    }

    public bool HasdInputsChanged(float horizontalInput, bool jump, bool attack, bool sAtack)=> this.horizontalInput != horizontalInput || this.jump != jump || this.attack != attack || this.sAtack != sAtack;
    
    public string ToJson()
    {
        var values = new Dictionary<string, string>
        {
            { "horizontalInput", horizontalInput.ToString() },
            { "jump", jump.ToString() },
            { "attack", attack.ToString() },
            { "sAtack", sAtack.ToString() },
            { "mousePosition_x", mousePosition.x.ToString() },
            { "mousePosition_y", mousePosition.y.ToString() }
        };

        return values.ToJson();
    }
}