using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateImage
{
    public class Config
    {
        public string SourceDatabase { get; set; }
        public string DestinationDatabase { get; set; }
        public string FilePath { get; set; }
        public string AzureBlobConnectionString { get; set; }
        public string TenantName { get; set; }
        public string MainPath { get; set; }
        public bool ReuploadTxcEnabled { get; set; }
        public string AccountName { get; set; }
        public string BlobNamePrefix { get; set; }
        public string NodeUrlPrefix { get; set; }
        public bool ForceReplaceImage { get; set; }
        public bool KeepSameGuid { get; set; }
        public string TX2ConnectorUrl { get; set; }
        public string TXCAzureStorageUrl { get; set; }
        public int TenantId { get; set; }
        public int Delay { get; set; }
        public bool UseExternalUrlFileName { get; set; }
        public string OnlyProcessFileNames { get; set; }
        public bool OnlyReportMissingFiles { get; set; }
    }
}
