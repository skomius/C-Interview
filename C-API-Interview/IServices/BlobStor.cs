namespace C_API_Interview.IServices;

public class BlobStor : IBlobStor
{


    public async Task SaveToBlob(Stream file, string blobName)
    {
        throw new NotImplementedException();    
        // Stores to a file using the blob name. Needs to be implemented from scratch.
    }

    public Stream ReadFromFile(string file_name)
    {
        throw new NotImplementedException();
    }
}