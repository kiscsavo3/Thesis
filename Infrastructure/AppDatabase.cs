using Domain.Common;
using Domain.Internal.DTO;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class AppDatabase : IAppDatabase, IDisposable
    {
        private readonly IDriver driver;
        public AppDatabase(IOptions<DbCredentials> options)
        {
            driver = GraphDatabase.Driver(options.Value.Host, AuthTokens.Basic(options.Value.Username, options.Value.Password));
        }
        public List<INode> AddNode(Node node)
        {
            var queryString = node.ToCreateQueryString();
            return ExecuteDatabaseNode(queryString);
        }

        public List<IRelationship> AddRelationship(Relationship relationship)
        {
            var queryString = relationship.ToCreateQueryString();
            return ExecuteDatabaseRelationship(queryString);
        }

        public List<INode> DeleteNode(Node node)
        {
            var queryString = node.ToDeleteQueryString();
            return ExecuteDatabaseNode(queryString);
        }

        public List<IRelationship> DeleteRelationship(Relationship relationship)
        {
            var queryString = relationship.ToDeleteQueryString();
            return ExecuteDatabaseRelationship(queryString);
        }

        public void Dispose()
        {
            driver?.Dispose();
        }

        public List<INode> GetNode(long id)
        {
            var queryString = new Node() { Id = id }.ToGetQueryString();
            return ExecuteDatabaseNode(queryString);
        }

        public List<INode> GetNodes(string EntityId, string type)
        {
            var queryString = new Node() { Label = type, Properties = new Dictionary<string, object> { { nameof(EntityId), EntityId } } }.ToGetByEntityIdQueryString();
            return ExecuteDatabaseNode(queryString);
        }

        public List<IRelationship> GetRelationship(Relationship relationship)
        {
            var queryString = relationship.ToGetQueryString();
            return ExecuteDatabaseRelationship(queryString);
        }

        public List<IRelationship> GetRelationships<BaseEntity>(long sourceId)
        {
            var queryString = new Relationship() { StartNodeId = sourceId, Type = typeof(BaseEntity).ToString() }.ToGetMultipleQueryString();
            return ExecuteDatabaseRelationship(queryString);
        }

        public List<INode> UpdateNode(Node node)
        {
            var queryString = node.ToUpdateQueryString();
            return ExecuteDatabaseNode(queryString);
        }

        public List<IRelationship> UpdateRelationship(Relationship relationship)
        {
            var queryString = relationship.ToUpdateQueryString();
            return ExecuteDatabaseRelationship(queryString);
        }

      
        private List<INode> ExecuteDatabaseNode(string query)
        {
            lock (driver)
            {
                var session = driver.AsyncSession();
                var nodes = new List<INode>();
                try
                {
                    session.ReadTransactionAsync(async tx =>
                    {
                        var cursor = tx.RunAsync(query, new { id = 0 }).Result;
                        while (cursor.FetchAsync().Result)
                        {
                            var node = cursor.Current.Values.Values.Select(x => x.As<INode>()).SingleOrDefault();
                            nodes.Add(node);
                        }
                        return nodes;
                    }).Wait();
                }
                finally{ session.CloseAsync().Wait(); }
                return nodes;
            }
        }

        
        private List<IRelationship> ExecuteDatabaseRelationship(string query)
        {
            lock (driver)
            {
                var session = driver.AsyncSession();
                var relationships = new List<IRelationship>();
                try
                {
                    session.ReadTransactionAsync(async tx =>
                    {
                        var cursor = tx.RunAsync(query, new { id = 0 }).Result;
                        while (cursor.FetchAsync().Result)
                        {
                            var relationship = cursor.Current.Values.Values.Select(x => x.As<IRelationship>()).SingleOrDefault();
                            relationships.Add(relationship);
                        }
                        return relationships;
                    }).Wait();
                }
                finally { session.CloseAsync().Wait(); }
                return relationships;
            }
        }
    }
}
