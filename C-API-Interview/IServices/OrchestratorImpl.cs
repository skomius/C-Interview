using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace C_API_Interview.IServices;

//Define domain. End the choose the naming of component. Would be better jsonstreamprocessor
public class OrchestratorImpl(IJsonValidator jsonProcessor, IBlobStor blob) : Orchestrator
{
    // Better name ProcessStream, too many details hide it under abstraction(StreamToJObjects, Filter, SendBatch)
    // Use IEnumerable feature differed execution iterate thru only one iteration and send one by one.
    // Avoiding allocating collection of batches in memory. unit test    
    public ActionResult StoreFile(Stream stream, StreamWriter streamWriter)
    {
        var errors = new List<string>();
        var s = new Newtonsoft.Json.JsonSerializer();
        using var sr = new StreamReader(stream);
        using var r = new JsonTextReader(sr);
        var input = File.Open("TestFileInput.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        while (r.Read())
        {
            if (r.TokenType != JsonToken.StartObject)
            {
                continue;
            }

            try
            {
                var (yes, output, o) = jsonProcessor.Check(s, r);
                if (!yes)
                    errors.Add(output);
                s.Serialize(streamWriter, o);

                var a = (MemoryStream)streamWriter.BaseStream;
                streamWriter.Flush();
                if (a.Length > 80000)
                {
                    a.WriteTo(input);
                    //async in loop not works in parrallel  
                    blob.SaveToBlob(a, "validObjects.json");
                }

                a.SetLength(0);
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
            }
            finally 
            {
                streamWriter.Flush();
            }

        }

        return errors.Count > 0 ? new OkResult() : new BadRequestObjectResult(errors);
    }
}