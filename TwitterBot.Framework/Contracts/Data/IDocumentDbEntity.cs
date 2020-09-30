using System;

namespace TwitterBot.Framework.Contracts.Data
{
    public interface IDocumentDbEntity
    {
        Type EntityType { get; set; }
        string Name { get; set; }
    }
}
