using Elastic.Clients.Elasticsearch;
using UserPermissionsAdmin.Models;

namespace UserPermissionsAdmin.Data
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public ElasticSearchService(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<Permission> GetPermissionByIdAsync(int permissionId)
        {
            var response = await _elasticsearchClient.GetAsync<Permission>(permissionId);

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to get product: {response.DebugInformation} in ElasticSearch");
            }

            return response.Source;
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            var response = await _elasticsearchClient.IndexAsync(permission);

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to index product: {response.DebugInformation} in ElasticSearch");
            }
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            var response = await _elasticsearchClient.UpdateAsync<Permission, Permission>(permission.Id, 
                                p => p.Doc(permission).DocAsUpsert(true));

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to update product: {response.DebugInformation} in ElasticSearch");
            }
        }
    }
}
