using C_API_Interview.IServices;
using Newtonsoft.Json;
using NSubstitute;
using Xunit.Abstractions;

namespace C_API_InterviewTest;

public class UnitTest1
{

    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void HttpTest()
    {
        var httpClient = new HttpClient();

        using (var fileStream = File.Open("TestFile.txt", FileMode.Open))
        {
            StreamContent content = new StreamContent(fileStream);

            httpClient.PostAsync("https://localhost:7120/", content);
        }
    }

    [Fact]
    public void StoreFile()
    {
        var jsonString =
@"[{
  ""Date"": ""2019-08-01T00:00:00-07:00"",
  ""Temperature"": 25,
  ""TemperatureRanges"": {
      ""Cold"": { ""High"": 20, ""Low"": -10 },
      ""Hot"": { ""High"": 60, ""Low"": 20 }
  },
  ""name"": ""Hot""
},
{
  ""Date"": ""2019-08-01T00:00:00-07:00"",
  ""Temperature"": 25,
  ""TemperatureRanges"": {
      ""Cold"": { ""High"": 20, ""Low"": -10 },
      ""Hot"": { ""High"": 60, ""Low"": 20 }
  },
  ""name"": ""Hot""
}]";
        using (var fileStream = File.Open("TestFile.txt", FileMode.Open))
        {
            var blobStore = Substitute.For<IBlobStor>();
            var validator = Substitute.For<IJsonValidator>();

            blobStore.SaveToBlob(Arg.Any<Stream>(), Arg.Any<string>());

            var orchestrator = new OrchestratorImpl(new JsonProcessorImpl(), blobStore);

            using var sw = new MemoryStream();
            using var w = new StreamWriter(sw);

            var mem = new MemoryStream();

            var stream = new MemoryStream();
            var e = stream.GetBuffer(); 
            var writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Position = 0;

            var a = orchestrator.StoreFile(fileStream, w);

            //StreamReader sr = new StreamReader(sw);
            //w.Flush();
            //var set = Encoding.UTF8.GetString(sw.ToArray());
            //sw.Position = 0;
            //sw.Flush();
            //string text = sr.ReadToEnd();

            //var input = File.Open("TestFileInput.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //sw.CopyTo(input);
            //_testOutputHelper.WriteLine(text);
        }
    }

    [Fact]
    public void Check()
    {
        var jsonString =
@"{
  ""Date"": ""2019-08-01T00:00:00-07:00"",
  ""Temperature"": 25,
  ""TemperatureRanges"": {
      ""Cold"": { ""High"": 20, ""Low"": -10 },
      ""Hot"": { ""High"": 60, ""Low"": 20 }
  },
  ""name"": ""Hot"",
}";

        var r = new JsonTextReader(new StringReader(jsonString));

        var result = new JsonProcessorImpl().Check(new JsonSerializer(), r);

        _testOutputHelper.WriteLine(result.Item3?.ToString());

        Assert.Equal(jsonString, result.Item3?.ToString());
    }


    //Both for eaches prints different answers
    [Fact]
    public void IEnumerableTest()
    {
        var arr = new[] { "dog", "cat", "bird" };

        //trades cpu capacity for memory space
        var coll = arr.Select(x => $"{new Random().Next()} {x}");

        foreach (var item in coll)
        {
            _testOutputHelper.WriteLine(item);
        }

        foreach (var item in coll)
        {
            _testOutputHelper.WriteLine(item);
        }
    }

    async Task ccc() { await Task.Delay(500); }
}