using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAppDatabase
    {
        Task<bool> ExecuteWriteQuery(string query);

        Task<List<T>> ExecuteReadQuery<T>(string query);
    }
}
