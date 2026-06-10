using GraphQL.Client.Abstractions;

namespace Services.Utility.GraphQLClient
{
    public interface ITXCGraphQLClient
    {
        public IGraphQLClient GraphQLClient { get; }
        public void AddHeaders(string key, string value);
    }

    /// <summary>
    /// or the graphql call that required tenant, 
    /// because old implementation can work but not
    /// testable by unit test
    /// </summary>
    public interface ITXCGraphqlClientWithHeader
    {   
        /// <summary>
        /// get graphql client with header tenant id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public IGraphQLClient GetGraphQLClient(int tenantId);
    }
}
