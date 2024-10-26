using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace C_API_Interview.IServices;

//better name JObjectFilter 
public class JsonProcessorImpl : IJsonValidator
{
 // better pass Jobject Filter(JObject jobject)
  public (bool, string, JObject?) Check(JsonSerializer serializer, JsonTextReader text)
  {
    JObject o;
    try
    {
      o = serializer.Deserialize<JObject>(text);
      if (o["name"] is null) return (false, "Name was missing", o);
      o["timestamp"] = DateTime.UtcNow;
    }
    catch (Exception e)
    {
      return (false, e.Message, null);
    }
    return (true, "", o);
  }
}
