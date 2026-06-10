using System;
using System.Collections.Generic;
using Domain.Models.ConfigOptions;
using Microsoft.Azure.Management.Cdn;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace Services.CDN
{
    public interface ICdnHelper
    {
        void PurgeCdnEndpoints(List<string> contentPaths);
    }

    public class CdnHelper : ICdnHelper
    {
        private readonly CdnConfiguration _cdnConfig;
        public CdnHelper(IOptions<CdnConfiguration> cdnConfig)
        {
            _cdnConfig = cdnConfig.Value;
        }

        private AuthenticationResult _authResult = null;
        private CdnManagementClient _cdnManagementClient = null;
        private CdnManagementClient CdnManagementClient
        {
            get
            {
                if(_cdnManagementClient == null)
                    CreateCdnManagementClient();
                else if (_authResult != null && DateTimeOffset.UtcNow >= _authResult.ExpiresOn) //refresh
                    CreateCdnManagementClient();

                return _cdnManagementClient;
            }
        }

        private void CreateCdnManagementClient()
        {
            // Get Auth Result
            _authResult = null;
            AuthenticationContext authContext = new AuthenticationContext(_cdnConfig.Authority);
            ClientCredential credential = new ClientCredential(_cdnConfig.ClientId, _cdnConfig.ClientSecret);
            _authResult = authContext.AcquireTokenAsync("https://management.core.windows.net/", credential).Result;

            // Create CDN client
            _cdnManagementClient = new CdnManagementClient(new TokenCredentials(_authResult.AccessToken))
            { SubscriptionId = _cdnConfig.SubscriptionId };
        }


        //Akamai allows only purging individual endpoints, no support for wildcards
        public void PurgeCdnEndpoints(List<string> contentPaths)
        {
             CdnManagementClient.Endpoints.PurgeContent(_cdnConfig.ResourceGroupName, _cdnConfig.ProfileName, _cdnConfig.EndpointName, contentPaths);
        }
    }
}
