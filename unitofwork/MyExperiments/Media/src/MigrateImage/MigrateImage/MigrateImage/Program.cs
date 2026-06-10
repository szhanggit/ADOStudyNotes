using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using MimeMapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MigrateImage
{
    class Program
    {
        public static StringBuilder sb = new StringBuilder();
        private static readonly HttpClient _client = new HttpClient();

        private static void WriteLine(string message)
        {
            Console.WriteLine(message);
            sb.AppendLine(message);
        }
        static void Main(string[] args)
        {
            WriteLine("Start Migrating");
            var config = Read();
            var mediaLibraries = GetTx2MediaLibrary(config.SourceDatabase);
            List<Media> medias = new List<Media>();

            foreach (var medialibrary in mediaLibraries)
            {
                try
                {
                    if (config.KeepSameGuid)
                    {
                        if(!string.IsNullOrEmpty(config.OnlyProcessFileNames))
                        {
                            if (!config.OnlyProcessFileNames.Contains(medialibrary.FileName))
                                continue;
                        }

                        if (!string.IsNullOrEmpty(medialibrary.PhysicalFullPath))
                        {
                            string fileName = "";
                            
                            if(config.UseExternalUrlFileName)
                                fileName = Path.GetFileName(medialibrary.ExternalURL);
                            else
                                fileName = Path.GetFileName(medialibrary.PhysicalFullPath);

                            string fullPath = Path.Combine(config.FilePath, fileName);

                            if (File.Exists(fullPath))
                            {
                                if (!config.OnlyReportMissingFiles)
                                {
                                    WriteLine($"Start uploading {medialibrary.FileName}");
                                    if (!IsValidContainer(config.AzureBlobConnectionString, config.TenantName))
                                    {
                                        WriteLine($"{config.TenantName} not exists");
                                        return;
                                    }

                                    var result = Upload(fileName, fullPath, config.AzureBlobConnectionString, config.TenantName, config.MainPath, config.ForceReplaceImage);

                                    if (result.IsSuccess)
                                    {
                                        var media = MediaFactory(medialibrary, result.Data);
                                        medias.Add(media);
                                    };
                                }


                            }
                            else
                            {
                                WriteLine($"Image {medialibrary.KeyWord} is not exists in the folder");
                                continue;
                            }
                        }
                        else if (config.ReuploadTxcEnabled && string.IsNullOrEmpty(medialibrary.PhysicalFullPath))
                        {
                            var media = MediaFactory(medialibrary, config);
                            medias.Add(media);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(medialibrary.PhysicalFullPath))
                        {
                            string fileName = Path.GetFileName(medialibrary.PhysicalFullPath);
                            string fullPath = Path.Combine(config.FilePath, fileName);

                            if (File.Exists(fullPath))
                            {
                                using (var fs = File.Open(fullPath, FileMode.Open))
                                {
                                    UploadTX2Connector(config.TX2ConnectorUrl, fs, fileName, medialibrary.MediaCategory, medialibrary.KeyWord, MimeUtility.GetMimeMapping(Path.GetFileName(fileName)), config.TenantName, config.TenantId, config.Delay);
                                }
                            }
                        }
                        else if (config.ReuploadTxcEnabled && string.IsNullOrEmpty(medialibrary.PhysicalFullPath))
                        {
                            var downloadResponse = _client.GetAsync(config.TXCAzureStorageUrl + medialibrary.FileName).Result;
                            if (downloadResponse.IsSuccessStatusCode)
                            {
                                var fs = downloadResponse.Content.ReadAsStreamAsync().Result;
                                UploadTX2Connector(config.TX2ConnectorUrl, fs, medialibrary.FileName, medialibrary.MediaCategory, medialibrary.KeyWord, MimeUtility.GetMimeMapping(Path.GetFileName(medialibrary.FileName)), config.TenantName, config.TenantId, config.Delay);
                            }
                            else
                                WriteLine($"Image {medialibrary.KeyWord} is not exists in the AZURE");
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                    continue;
                }

            }

            if(!config.OnlyReportMissingFiles)
                Save(medias, config.DestinationDatabase);


            bool exists = System.IO.Directory.Exists("Logs");
            if (!exists)
                System.IO.Directory.CreateDirectory("Logs");

            var path = string.Concat(Environment.CurrentDirectory, $"\\Logs\\MediaMigrate-{config.TenantName}-{DateTime.UtcNow.Ticks}.txt");

            File.WriteAllText(path, sb.ToString());
        }

        public static Config Read()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"settings.json");
            using (StreamReader reader = new StreamReader(path))
            {
                try
                {
                    Config config = new Config();
                    var jsonString = reader.ReadToEnd();
                    JObject jsonObject = JObject.Parse(jsonString);

                    config.SourceDatabase = (string)jsonObject["SourceDatabase"];
                    config.DestinationDatabase = (string)jsonObject["DestinationDatabase"];
                    config.FilePath = (string)jsonObject["FilePath"];
                    config.AzureBlobConnectionString = (string)jsonObject["AzureBlobConnectionString"];
                    config.TenantName = (string)jsonObject["TenantName"];
                    config.MainPath = (string)jsonObject["MainPath"];

                    config.ReuploadTxcEnabled = (bool)jsonObject["ReuploadTxcEnabled"];
                    config.AccountName = (string)jsonObject["AccountName"];
                    config.BlobNamePrefix = (string)jsonObject["BlobNamePrefix"];
                    config.NodeUrlPrefix = (string)jsonObject["NodeUrlPrefix"];
                    config.ForceReplaceImage = (bool)jsonObject["ForceReplaceImage"];
                    config.KeepSameGuid = (bool)jsonObject["KeepSameGuid"];
                    config.TX2ConnectorUrl = (string)jsonObject["TX2ConnectorUrl"];
                    config.TXCAzureStorageUrl = (string)jsonObject["TXCAzureStorageUrl"];
                    config.TenantId = (int)jsonObject["TenantId"];
                    config.Delay = (int)jsonObject["Delay"];
                    config.UseExternalUrlFileName = (bool)jsonObject["UseExternalUrlFileName"];
                    config.OnlyProcessFileNames = (string)jsonObject["OnlyProcessFileNames"];
                    config.OnlyReportMissingFiles = (bool)jsonObject["OnlyReportMissingFiles"];

                    return config;
                }
                catch (Exception ex)
                {
                    WriteLine("Problem reading file");

                    return null;
                }
            }
        }
        public bool ValidateConfig()
        {
            bool result = false;
            return result;
        }
        public static List<MediaLibrary> GetTx2MediaLibrary(string sourceDatabase)
        {
            try
            {
                //List<MediaLibrary> mediaLibraries = new List<MediaLibrary>();

                using (var connection = new SqlConnection(sourceDatabase))
                {

                    List<MediaLibrary> mediaLibraries = connection.Query<MediaLibrary>("select * from dbo.MediaLibrary").ToList();
                    return mediaLibraries;
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
                throw;
            }
        }
        public static bool IsValidContainer(string connectionString, string containerName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

            if (!blobContainerClient.Exists())
            {
                WriteLine("Container not exists");
                return false;
            }

            return true;
        }
        public static MediaUploadResponse Upload(string fileName, string imagePath, string connectionString, string containerName, string mainpath, bool forceReplace)
        {

            try
            {
                BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
                BlobClient blobClient = blobContainerClient.GetBlobClient(@$"{mainpath}/{fileName}");

                if (blobClient.Exists() && forceReplace == false)
                {
                    WriteLine($"{fileName} already exists");
                    return new MediaUploadResponse() { IsSuccess = false, Data = null };
                }

                var response = blobClient.Upload(imagePath, new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() { ContentType = MimeUtility.GetMimeMapping(imagePath) } });
                
                if (response.GetRawResponse().Status == ((int)HttpStatusCode.Created) ||
                    response.GetRawResponse().Status == ((int)HttpStatusCode.OK))
                {
                    WriteLine($"{fileName} sucessfully upload");

                    BlobMediaInfo blobMediaInfo = new BlobMediaInfo()
                    {
                        AccountName = blobClient.AccountName,
                        ContainerName = blobClient.BlobContainerName,
                        Name = blobClient.Name,
                        Url = blobClient.Uri.AbsolutePath
                    };
                    
                    
                    return new MediaUploadResponse() { IsSuccess = true, Data = blobMediaInfo };

                }
                else
                {
                    WriteLine($"{fileName} failed to upload");
                    return new MediaUploadResponse() { IsSuccess = false, Data = null };
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
                return new MediaUploadResponse() { IsSuccess = false, Data = null };
            }

        }
        public static MediaUploadResponse UploadTX2Connector(string tx2ConnectorUrl, Stream stream, string fileName, int mediaCategory, string keyWord, string contentType, string tenantName, int tenantId, int delay)
        {
            try
            {
                //Create Media Blob
                Uri uri = new Uri(tx2ConnectorUrl + "api/TxImageBlobMedia");

                MultipartFormDataContent content = new MultipartFormDataContent();

                StreamContent streamContent = new StreamContent(stream);

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                content.Add(streamContent, "Image", fileName);
                content.Add(new StringContent(mediaCategory.ToString()), "Type");

                content.Headers.Add("TenantName", tenantName);
                content.Headers.Add("TenantBasicInfoId", tenantId.ToString());

                var result = _client.PostAsync(tx2ConnectorUrl + "api/TxImageBlobMedia", content).Result;

                if (result.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<Response>(result.Content.ReadAsStringAsync().Result);

                    //Update Keyword
                    var renameKeyword = new
                    {
                        MediaId = response.Data.ToString(),
                        Keyword = keyWord,
                    };
                    JsonContent jsonContent = JsonContent.Create(renameKeyword);

                    jsonContent.Headers.Add("TenantName", tenantName);
                    jsonContent.Headers.Add("TenantBasicInfoId", tenantId.ToString());
                    var putResponse = _client.PutAsync(tx2ConnectorUrl + "api/TxMedia", jsonContent).Result;

                    WriteLine($"{fileName} sucessfully upload");
                    return new MediaUploadResponse() { IsSuccess = true, Data = null };
                }
                else
                {
                    WriteLine($"{fileName} failed to upload: " + result.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
            }

            WriteLine($"{fileName} failed to upload");
            return new MediaUploadResponse() { IsSuccess = false, Data = null };
        }

        public static Media MediaFactory(MediaLibrary mediaLibrary, BlobMediaInfo blobMediaInfo)
        {

            Media media = new Media();
            media.media_id = mediaLibrary.Id;
            media.file_name = Path.GetFileNameWithoutExtension(blobMediaInfo.Name);
            media.file_content_type = MimeUtility.GetMimeMapping(Path.GetFileName(mediaLibrary.PhysicalFullPath));
            media.account = blobMediaInfo.AccountName;
            media.blob_name = blobMediaInfo.Name;
            media.node_url = blobMediaInfo.Url;
            media.type = mediaLibrary.MediaCategory;
            media.width = mediaLibrary.Width.ToString();
            media.height = mediaLibrary.Height.ToString();
            media.keyword = mediaLibrary.KeyWord;

            return media;
        }
        public static Media MediaFactory(MediaLibrary mediaLibrary, Config config)
        {

            Media media = new Media();
            media.media_id = mediaLibrary.Id;
            media.file_name = Path.GetFileNameWithoutExtension(mediaLibrary.FileName);
            media.file_content_type = MimeUtility.GetMimeMapping(mediaLibrary.FileName);
            media.account = config.AccountName;
            media.blob_name = config.BlobNamePrefix + mediaLibrary.FileName;
            media.node_url = config.NodeUrlPrefix + mediaLibrary.FileName;
            media.type = mediaLibrary.MediaCategory;
            media.width = mediaLibrary.Width.ToString();
            media.height = mediaLibrary.Height.ToString();
            media.keyword = mediaLibrary.KeyWord;

            return media;
        }
        public static void Save(List<Media> tb_m_media, string destinationDatabase)
        {
            try
            {
                string sql = "SET IDENTITY_INSERT media.tb_m_media ON;" + Environment.NewLine +
                             "insert into media.tb_m_media(media_id, file_name, file_content_type, account, blob_name, node_url, type, width, height, keyword)" + Environment.NewLine +
                             "values" + Environment.NewLine +
                             "(@media_id,@file_name,@file_content_type,@account,@blob_name,@node_url,@type,@width,@height,@keyword)" + Environment.NewLine +
                             "SET IDENTITY_INSERT media.tb_m_media OFF;";
                using (var connection = new SqlConnection(destinationDatabase))
                {
                    connection.Open();

                    var affectedRows = connection.Execute(sql,tb_m_media);
                };
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
                throw;
            }
        }

    }
}
