using StackExchange.Redis;
using System;
using System.Linq;

namespace Yunyong.Redis.Base
{
    public abstract class Cache
    {
        private Cache() { }
        internal Cache(double ValidTimeSpan)
        {
            _DB = Common.Redis.DB();
            _defaultValidTimeSpan = TimeSpan.FromMinutes(ValidTimeSpan);
        }

        private RedisType _DataType(string key)
        {
            //
            if (!Exist(key))
            {
                return RedisType.None;
            }

            //
            var type = _DB.KeyType(key);
            if (type == RedisType.None
                || type == RedisType.Unknown)
            {
                return RedisType.None;
            }
            else
            {
                return type;
            }
        }
        private bool _IsRightType(RedisType type, params string[] keys)
        {
            //
            if (type == RedisType.None)
            {
                return false;
            }

            //
            if (keys == null || keys.Length == 0)
            {
                return false;
            }

            //
            if (keys.Length == 1)
            {
                return _DataType(keys[0]) == type;
            }
            if (keys.Length > 1)
            {
                var flag = true;
                foreach (var item in keys)
                {
                    var typeFlag = _DataType(item) == type;
                    if (!typeFlag)
                    {
                        flag = false;
                        break;
                    }
                }
                return flag;
            }
            return false;
        }

        protected TimeSpan _defaultValidTimeSpan = default(TimeSpan);
        protected IDatabase _DB { get; } = default(IDatabase);
        internal protected bool Delete(RedisType type, params string[] keys)
        {
            //
            if (keys == null
                || keys.Length == 0)
            {
                return true;
            }

            //
            if (!_IsRightType(type, keys))
            {
                return false;
            }

            //
            if (keys.Length == 1)
            {
                return _DB.KeyDelete(keys[0]);
            }
            if (keys.Length > 1)
            {
                var keyx = keys.Select(it => (RedisKey)it).ToArray();
                var num = _DB.KeyExists(keyx);
                return _DB.KeyDelete(keyx) == num;
            }

            //
            return false;
        }
        internal protected bool Exist(params string[] key)
        {
            //
            if (key == null || key.Length == 0)
            {
                return false;
            }

            //
            if (key.Length == 1)
            {
                return _DB.KeyExists(key[0]);
            }
            if (key.Length > 1)
            {
                return _DB.KeyExists(key.Select(it => (RedisKey)it).ToArray()) == key.LongLength;
            }

            //
            return false;
        }

        public bool Expire(TimeSpan validTimeSpan, params string[] key)
        {
            //
            if (validTimeSpan == null || validTimeSpan.Milliseconds == 0)
            {
                return false;
            }

            //
            if (key.Length == 1)
            {
                return _DB.KeyExpire(key[0], validTimeSpan);
            }
            if (key.Length > 1)
            {
                var flag = true;
                foreach (var item in key)
                {
                    if (Exist(item))
                    {
                        var itemFlag = Expire(validTimeSpan, item);
                        if (!itemFlag)
                        {
                            flag = false;
                        }
                    }
                }
                return flag;
            }

            //
            return false;
        }
        public bool Rename(string oldKey, string newKey)
        {
            //
            if (!Exist(oldKey))
            {
                return false;
            }

            //
            if (Exist(newKey))
            {
                return false;
            }

            //
            return _DB.KeyRename(oldKey, newKey, When.Exists);
        }

    }
}
