using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test.Factory
{
    public class Data
    {
        public Data()
        {

        }

        //Url
        public string URL = "";
        public string DEV_BASE_URL = "https://api-dev.txc.edenred.net/gateway/";
        public string STAGING_BASE_URL = "https://api-staging.txc.edenred.net/gateway/";
        public string UAT_BASE_URL = "https://api-uat.txc.edenred.net/gateway/";
        //public string DEV_BASE_URL = "http://localhost:30103/";
        //public string STAGING_BASE_URL = "http://localhost:30103/";
        //public string UAT_BASE_URL = "http://localhost:30103/";

        // API call
        public string Environment = "";
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
        public Dictionary<string, string> connectionStrings = new Dictionary<string, string> {
            { "dev1", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_th;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev2", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_in;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev3", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_id;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev4", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_jp;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev5", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gr;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev6", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_sg;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev7", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_tw;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev8", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_ma;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "dev9", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging1", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_th;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging2", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_in;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging3", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_id;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging4", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_jp;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging5", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_gr;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging6", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_sg;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging7", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_tw;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging8", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_ma;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "staging9", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_staging_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },            
            //{ "staging1", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_th;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging2", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_in;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging3", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_id;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging4", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_jp;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging5", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gr;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging6", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_sg;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging7", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_tw;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging8", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_ma;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            //{ "staging9", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat1", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_sg;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat2", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_in;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat3", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_id;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat4", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_jp;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat5", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_gr;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat6", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_sg;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat7", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_tw;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat8", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_ma;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
            { "uat9", "Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_uat_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true" },
        };
        public bool ResponseSuccess;
        public string ResponseMessage;
        public string MustContain;
        public int NumberOfRecords;
        public int ReturnId;

        // API call Request and Response obj
        public object RequestObj;
        public object ResponseObj;
    }
}
