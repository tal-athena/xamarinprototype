using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecisionsMobile.Services
{
    /// <summary>
    /// Interface for stores of different types of things... 
    /// </summary>
    /// not sure it's really a useful abstraction though.
    /// It probably makes mores sense for unique stores/services to reflect the type of thing
    /// and/or the service they interact with on the server side, instead of a generic CRUD interface like this.
    /// <typeparam name="T"></typeparam>
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);

        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<string> GetItemActionUrlAsync(T item);
    }
}
