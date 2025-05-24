namespace DesignPatterns.Behavioral.Strategy
{
    internal interface IStorageHandler
    {
        void Save();
    }

    interface IStorageAccessor : IStorageHandler
    {
        void SetStrategy(StorageType storageType);
    }

    public abstract class StorageHandler : IStorageHandler
    {
        public abstract void Save();
    }

    class InMemoryCache : StorageHandler
    {
        public override void Save()
        {
        }
    }

    class InCookie : StorageHandler
    {
        public override void Save()
        {
        }
    }

    class InLocal : StorageHandler
    {
        public override void Save()
        {
        }
    }

    public enum StorageType
    {
        Memory,
        Cookies,
        Local
    }

    /// <summary>
    /// The 'Context' class
    /// </summary>

    public class StorageAccessor : IStorageAccessor
    {
        private StorageType _storageType = StorageType.Memory;
        private Lazy<StorageHandler> _storage =>
            new Lazy<StorageHandler>(() => _storageType switch
            {
                StorageType.Local => new InLocal(),
                StorageType.Cookies => new InCookie(),
                _ => new InMemoryCache()
            });

        public void SetStrategy(StorageType storageType)
        {
            _storageType = storageType;
        }

        public void Save()
        {
            _storage.Value.Save();
        }
    }
}
