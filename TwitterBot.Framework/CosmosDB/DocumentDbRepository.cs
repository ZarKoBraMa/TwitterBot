using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitterBot.Framework.Contracts.Data;
using TwitterBot.Framework.Types;

namespace TwitterBot.Framework.CosmosDB
{
    public class DocumentDbRepository<T> : IDocumentDbRepository<T>
        where T : BaseType
    {
        private IDocumentDbContext _context;
        private DocumentCollection _documentCollection;

        public DocumentDbRepository(IDocumentDbContext context)
        {
            _context = context;

            var entityMetadata = _context.EntityCollection.FirstOrDefault(p => p.EntityType == typeof(T));
            Task.Run(async () => _documentCollection = await _context.DocumentClient.ReadDocumentCollectionAsync(
                UriFactory.CreateDocumentCollectionUri(_context.DatabaseId, entityMetadata.Name)))
                .Wait();
        }

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            var upsertedDoc = await _context.DocumentClient.UpsertDocumentAsync(_documentCollection.SelfLink, entity);
            return JsonConvert.DeserializeObject<T>(upsertedDoc.Resource.ToString());
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var result = await Task.Run(() => 
                _context.DocumentClient.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_context.DatabaseId, _documentCollection.Id))
                .Where(p => p.Id == id)
                .ToList());
            
            return result != null && result.Any() ? result.FirstOrDefault() : null;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var result = await _context.DocumentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_context.DatabaseId, _documentCollection.Id, id));
            
            return result.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<IQueryable<T>> TopAsync(Expression<Func<T, bool>> predicate, int n)
        {
            return await Task.Run(() =>
                _context.DocumentClient.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_context.DatabaseId, _documentCollection.Id))
                .Where(predicate));
                //.Take(n));
        }

        public async Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => 
                _context.DocumentClient.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_context.DatabaseId, _documentCollection.Id))
                .Where(predicate));
        }
    }
}
