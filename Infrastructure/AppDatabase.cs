using Domain.Common;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDatabase : IAppDatabase, IDisposable
    {
        private readonly IDriver driver;
        public AppDatabase(IOptions<DbCredentials> options)
        {
            driver = GraphDatabase.Driver(options.Value.Host, AuthTokens.Basic(options.Value.Username, options.Value.Password));
        }

        public void Dispose()
        {
            driver?.Dispose();
        }

       
        public async Task<bool> ExecuteWriteQuery(string query)
        {
            var session = driver.AsyncSession();
            try
            {
                await session.WriteTransactionAsync(async tx => await tx.RunAsync(query));
            }
            finally { await session.CloseAsync(); }
            return true;
        }

        public async Task<List<T>> ExecuteReadQuery<T>(string query) 
        {
            var session = driver.AsyncSession();
            var nodes = new List<T>();
            try
            {
                return await session.ReadTransactionAsync(async tx => 
                {
                    var cursor =  await tx.RunAsync(query, new { id = 0 });
                    while (await cursor.FetchAsync())
                    {
                        var valami = cursor.Current.Values.Values;
                        var node = cursor.Current.Values.Values.Select(x => x.As<T>()).SingleOrDefault();
                        nodes.Add(node);
                    }
                    return nodes;
                });
            }
            finally { await session.CloseAsync(); }
        }
    }
}
