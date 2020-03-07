using Domain.Extensions;
using Neo4j.Driver;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Domain.Internal.DTO
{
    public class Node
    {
        public object this[string key]
        {
            get { Properties.TryGetValue(key, out object value); return value; }
        }
        public string Label { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public long Id { get; set; }

        public bool Equals([AllowNull] INode other)
        {
            if (Id.Equals(other.Id)) { return true; }
            else { return false; }
        }

        public string ToCreateQueryString()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            string query = "CREATE(";
            query += $"n :{Label} {{";
            bool isFirstProp = true;
            foreach (var key in Properties.Keys)
            {
                if (isFirstProp) { query += " "; isFirstProp = false; }
                else { query += ", "; }
                query += $"{key}: {Properties.GetValue(key)}";
            }
            query += "}) RETURN n";
            return query;
        }

        public string ToUpdateQueryString() 
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            string query = $"MATCH (n) WHERE Id(n) = {Id} SET n = {{";
            bool isFirstProp = true;
            foreach (var key in Properties.Keys)
            {
                if (isFirstProp) { query += " "; isFirstProp = false; }
                else { query += ", "; }
                query += $"{key}: {Properties.GetValue(key)}";
            }
            query += "} RETURN n";
            return query;
        }

        public string ToDeleteQueryString()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            string query = $"MATCH (n) WHERE Id(n) = {Id} DETACH DELETE n RETURN n";
            return query;
        }

        public string ToGetQueryString()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            string query = $"MATCH (n) WHERE Id(n) = {Id} RETURN n";
            return query;
        }

        public string ToGetByEntityIdQueryString()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-GB");
            string query = $"MATCH (n :{Label}) WHERE n.EntityId = {this["EntityId"]} RETURN n";
            return query;
        }
    }
}
