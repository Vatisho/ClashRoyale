using System.Collections.Generic;
using Nakama.TinyJson;



public class CharSelector : Models, IToJson
{

    public long CharacterIndex { get; private set; }
    public bool Broadcast { get; private set; }
    public string sessionID { get; private set; }

    public CharSelector(long characterIndex, bool broadcast)
    {
        this.CharacterIndex = characterIndex;
        this.Broadcast = broadcast;
    }
    public CharSelector(long characterIndex, bool broadcast, string sessionID)
    {
        this.CharacterIndex = characterIndex;
        this.Broadcast = broadcast;
        this.sessionID = sessionID;
    }

    public CharSelector(byte[] buffer)
    {
        IDictionary<string, string> json = GetStateAsDictionary(buffer);
        this.CharacterIndex = long.Parse(json["characterIndex"]);
        this.Broadcast = bool.Parse(json["broadcast"]);
        this.sessionID = json["sessionID"];
    }

    public string ToJson()
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            { "characterIndex", this.CharacterIndex.ToString() },
            { "broadcast", this.Broadcast.ToString() },
            { "sessionID", this.sessionID }

        };
        return values.ToJson();
    }
}

