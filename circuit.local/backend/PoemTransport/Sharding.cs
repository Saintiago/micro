using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace PoemUtils
{
    public class Sharding
    {
        const string REDIS1 = "redis_1";
        const string REDIS2 = "redis_2";

        private ConnectionMultiplexer _redis1, _redis2;

        private static Sharding instance = null;

        public static Sharding GetInstance()
        {
            if (instance == null)
            {
                instance = new Sharding();
            }
            return instance;
        }

        private Sharding()
        {
            _redis1 = ConnectionMultiplexer.Connect(GetShardAddress(REDIS1));
            _redis2 = ConnectionMultiplexer.Connect(GetShardAddress(REDIS2));
        }

        public void Write(int tenant, string key, string value)
        {
            this.GetShard(tenant).GetDatabase().StringSet(key, value);
        }

        public string Read(int tenant, string key)
        {
            return this.GetShard(tenant).GetDatabase().StringGet(key);
        }

        private ConnectionMultiplexer GetShard(int tenantId)
        {
            switch (GetCode(tenantId))
            {
                case 0:
                    return GetShardInstance(REDIS1);
                case 1:
                    return GetShardInstance(REDIS2);
            }
            throw new ArgumentException("Unknown tenant: " + tenantId + ".");
        }

        private int GetCode(int tenantId)
        {
            byte[] hash = Utils.GetHash(tenantId);
            int firstBit = (hash[0] & 0b00000001) == 0 ? 0 : 1;
            return firstBit;
        }

        private ConnectionMultiplexer GetShardInstance(string name)
        {
            switch (name)
            {
                case Sharding.REDIS1:
                    return _redis1;
                case Sharding.REDIS2:
                    return _redis2;
            }
            throw new ArgumentException("Unknown shard name: " + name + ".");
        }

        private static string GetShardAddress(string name)
        {
            switch (name)
            {
                case Sharding.REDIS1:
                    return "127.0.0.1:6370";
                case Sharding.REDIS2:
                    return "127.0.0.1:6371";
            }
            throw new ArgumentException("Unknown shard name: " + name + ".");
        }
    }
}
