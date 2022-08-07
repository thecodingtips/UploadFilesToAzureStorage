using Azure.Storage.Blobs;

var azureFuntionUrl = "azure URL";

var httpClient = new HttpClient();

var blobSASUrl = await httpClient.GetStringAsync(azureFuntionUrl);

var uri = new Uri(blobSASUrl);

var client = new BlobClient(uri);

Console.WriteLine("Starting upload");

var ret = await client.UploadAsync(@"C:\somefile.bak");

Console.WriteLine("Upload Finished");

Console.ReadLine();