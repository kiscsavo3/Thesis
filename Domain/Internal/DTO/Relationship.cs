using Domain.Extensions;
using Domain.External.DTO;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Internal.DTO
{
    public class Relationship
    {
        public string Type { get; set;}

        public long StartNodeId { get; set; }

        public long EndNodeId { get; set; }

        public object this[string key]
        {
            get { Properties.TryGetValue(key, out object value); return value; }
        }

        public Dictionary<string, object> Properties { get; set; }

        public long Id { get; set; }

        public bool Equals([AllowNull] IRelationship other)
        {
            if (Id == other.Id) return true;
            else return false;
        }

        public string ToCreateQueryString()
        {
            string query = $"MATCH (src), (trg) WHERE Id(src) = {StartNodeId} AND Id(trg) = {EndNodeId} CREATE (src)-[r: {Type} {{";
            bool isFirstProp = true;
            foreach (var key in Properties.Keys)
            {
                if (isFirstProp) { query += " "; isFirstProp = false; }
                else { query += ", "; }
                query += $"{key}: {Properties.GetValue(key)}";
            }
            query += " }]->(trg) RETURN r";
            return query;
        }

        public string ToDeleteQueryString()
        {
            string query = $"MATCH (src)-[r :{Type}]->(trg) WHERE Id(r) = {Id} DELETE r RETURN r";
            return query;
        }

        public string ToUpdateQueryString()
        {
            string query = $"MATCH (src)-[r :{Type}]->(trg) WHERE Id(r) = {Id} SET r = {{";
            bool isFirstProp = true;
            foreach (var key in Properties.Keys)
            {
                if (isFirstProp) { query += " "; isFirstProp = false; }
                else { query += ", "; }
                query += $"{key}: {Properties.GetValue(key)}";
            }
            query += "} RETURN r";
            return query;
        }

        public string ToGetQueryString()
        {
            string query = $"MATCH (src)-[r :{Type}]->(trg) WHERE Id(src) = {StartNodeId} AND Id(trg) = {EndNodeId}";
            query = Type == nameof(Cast) || Type == nameof(Crew) ? query + $" AND r.EntityCredit = {Properties.GetValue("EntityCredit")} RETURN r" : query + " RETURN r";
            return query;
        }

        public string ToGetMultipleQueryString()
        {
            string query = $"MATCH (src)-[r: {Type}]->(trg) WHERE Id(src) = {StartNodeId} RETURN r";
            return query;
        }
    }
}
