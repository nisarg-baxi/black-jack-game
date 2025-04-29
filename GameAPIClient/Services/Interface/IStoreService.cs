using GameSharedLib.Models;

namespace GameAPIClient.Services.Interface
{
    public interface IStoreService
    {
        IEnumerable<StoreItem> GetStoreItems();

        StoreItem? GetItemById(string id);

        StoreItem? GetItemByName(string name);
    }
}
