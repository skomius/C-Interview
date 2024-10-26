using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace C_API_Interview.IServices;

public interface IJsonValidator
{
    public (bool, string, JObject?) Check(JsonSerializer serializer, JsonTextReader text);
}