using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

public class VelocityAndPosition : Models, IToJson
{
    public Vector2 velocity { get; private set; }
    public Vector3 position { get; private set; }

    public VelocityAndPosition(Vector2 velocity, Vector3 position)
    {
        this.velocity = velocity;
        this.position = position;
    }

    public VelocityAndPosition(byte[] buffer)
    {
        IDictionary<string, string> json = GetStateAsDictionary(buffer);
        velocity = new Vector2(float.Parse(json["velocity_x"]), float.Parse(json["velocity_y"]));
        position = new Vector3(float.Parse(json["position_x"]), float.Parse(json["position_y"]));
    }

    public string ToJson()
    {
        var values = new Dictionary<string, string>
        {
            { "velocity_x", velocity.x.ToString() },
            { "velocity_y", velocity.y.ToString() },
            { "position_x", position.x.ToString() },
            { "position_y", position.y.ToString() }
        };

        return values.ToJson();
    }
}