﻿using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterBot.Framework.Contracts.Data
{
    public interface IDocumentDbContext
    {
        string DatabaseId { get; set; }
        string EndpointUri { get; set; }
        string AuthKey { get; set; }
        IDocumentClient DocumentClient { get; }
        ICollection<IDocumentDbEntity> EntityCollection { get; }
        Task CreateDatabaseAndCollectionsAsync();
    }
}
