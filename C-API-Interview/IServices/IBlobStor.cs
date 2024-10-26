namespace C_API_Interview.IServices;

public interface IBlobStor
{
    public Task SaveToBlob(Stream file, string blobName);
    public Stream ReadFromFile(string file_name);
}