using System.Collections.Generic;
using System.Text;
using Nakama.TinyJson;

public abstract class Models
{
    protected IDictionary<string, string> GetStateAsDictionary(byte[] state)
    {
        return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
    }
}