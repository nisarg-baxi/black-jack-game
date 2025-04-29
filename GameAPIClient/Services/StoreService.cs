namespace GameAPIClient.Services
{
    using GameAPIClient.Services.Interface;
    using GameSharedLib.Models;
    using System.Collections.Generic;

    public class StoreService: IStoreService
    {
        private readonly List<StoreItem> _items = new()
        {
            new StoreItem { Name = "VIP Skin", Cost = 500 },
            new StoreItem { Name = "Golden Deck", Cost = 800 },
            new StoreItem { Name = "Extra Life", Cost = 300 },
        };

        public IEnumerable<StoreItem> GetStoreItems() => _items;

        public StoreItem? GetItemById(string id)
        {
            return _items.Find(i => i.Id == id);
        }

        public StoreItem? GetItemByName(string name)
        {
            return _items.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

}
