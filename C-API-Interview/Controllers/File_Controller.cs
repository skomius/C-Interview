using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using C_API_Interview.IServices;
using Microsoft.AspNetCore.Mvc;

namespace C_API_Interview.Controllers;

[ApiController]
[Route("CreateFile")]
public class File_Controller : ControllerBase
{
    private Orchestrator orchestrator;
    private const string Multipart = "multipart/form-data";

    public File_Controller(Orchestrator orchestrator)
    {
        this.orchestrator = orchestrator;
    }
    
    //httpcompletionmode possible? Possible not write reponse to memory stream,but stream from networkstream?
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json, Multipart, MediaTypeNames.Application.Octet), ]
    public async Task<IActionResult> Create_A_FileAsync()
    {
        if (!TryGetContent(out var s))
        {
            return Forbid();
        }

        //memorystream limit 2gb, memory streams from memory, memory stream not shrinks, you have to set capacity to zero to reset it 
        await using var sw = new MemoryStream();
        //streamwrite writes to memorystream when its buffer is fulled
        await using var w = new StreamWriter(sw);

        return orchestrator.StoreFile(s, w);
    }

    private bool TryGetContent(out Stream x)
    {
        x = Stream.Null;
        if (Request is { HasFormContentType: true, Form.Files.Count: 1 })
        {
            x = Request.Form.Files[0].OpenReadStream();
            return true;
        }

        if (Request is { ContentType: "application/json" })
        {
            x = Request.Body;
            return true;
        }

        return false;
    }
    
    [HttpGet]
    public IActionResult HelloWorld()
    {
        return Ok("Hello World!");
    }
}