using GraphQL.Client.Abstractions;

namespace Service.Utility.GraphQLClient.Interface
{
    public interface ITXCGraphQLClient
    {
        public IGraphQLClient GraphQLClient { get; }
        public void AddHeaders(string key, string value);
    }
    public interface ITXCGraphqlClientWithHeader
    {
        public IGraphQLClient GetGraphQLClient(int tenantId);
    }
}
