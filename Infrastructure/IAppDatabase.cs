using Domain.Internal.DTO;
using Neo4j.Driver;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IAppDatabase
    {
        List<INode> AddNode(Node node);

        List<IRelationship> AddRelationship(Relationship relationship);

        List<INode> UpdateNode(Node node);

        List<IRelationship> UpdateRelationship(Relationship relationship);

        List<INode> DeleteNode(Node node);

        List<IRelationship> DeleteRelationship(Relationship relationship);

        List<INode> GetNode(long id);

        List<INode> GetNodes(string EntityId, string type);

        List<IRelationship> GetRelationship(Relationship relationship);

        List<IRelationship> GetRelationships<BaseEntity>(long sourceId);
    }
}
