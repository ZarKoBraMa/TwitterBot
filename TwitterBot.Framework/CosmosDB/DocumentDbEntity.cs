using System;
using TwitterBot.Framework.Contracts.Data;

namespace TwitterBot.Framework.CosmosDB
{
    public class DocumentDbEntity : IDocumentDbEntity
    {
        public Type EntityType { get; set; }
        public string Name { get; set; }
    }
}
