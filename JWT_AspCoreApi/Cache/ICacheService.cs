using System.Data;

namespace JWT_AspCoreApi.Cache
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset dateTimeOffset);
    }
}
