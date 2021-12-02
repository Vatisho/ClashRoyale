using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

public static class MatchDataJson
{   
    public static string Died(Vector3 position)
    {
        var values = new Dictionary<string, string>
        {
            { "position.x", position.x.ToString() },
            { "position.y", position.y.ToString() }
        };

        return values.ToJson();
    }

    public static string Respawned(int spawnIndex)
    {
        var values = new Dictionary<string, string>
        {
            { "spawnIndex", spawnIndex.ToString() },
        };

        return values.ToJson();
    }

    public static string StartNewRound(string winnerPlayerName)
    {
        var values = new Dictionary<string, string>
        {
            { "winningPlayerName", winnerPlayerName }
        };
        
        return values.ToJson();
    }
    
   
}
