﻿using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Models;
using Core.CustomExceptions;

namespace Core.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        public readonly IOptions<RedisConfiguration> _redisConfig;
        private readonly RedisEndpoint conf = null;

        public RedisCacheService(IOptions<RedisConfiguration> redisConfig)
        {
            _redisConfig = redisConfig;
            conf = new RedisEndpoint { Host = _redisConfig.Value.RedisEndPoint, Port = _redisConfig.Value.RedisPort, Password = "", RetryTimeout = _redisConfig.Value.RedisTimeout };
        }

        public T Get<T>(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    return client.Get<T>(key);
                }
            }
            catch
            {
                //throw new RedisNotAvailableException();
                return default;
            }
        }

        public IList<T> GetAll<T>(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    var keys = client.SearchKeys(key);
                    if (keys.Any())
                    {
                        IEnumerable<T> dataList = client.GetAll<T>(keys).Values;
                        return dataList.ToList();
                    }
                    return new List<T>();
                }
            }
            catch
            {
                return new List<T>();
                //throw new RedisNotAvailableException();
            }
        }

        public void Set(string key, object data)
        {
            Set(key, data, DateTime.Now.AddMinutes(_redisConfig.Value.RedisTimeout));
        }

        public void Set(string key, object data, DateTime time)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    var dataSerialize = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                    client.Set(key, Encoding.UTF8.GetBytes(dataSerialize), time);
                }
            }
            catch(Exception ex)
            {
                throw new RedisNotAvailableException();                
            }
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    client.SetAll(values);
                }
            }
            catch
            {

                throw new RedisNotAvailableException();
            }

        }

        public int Count(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    return client.SearchKeys(key).Where(s => s.Contains(":") && s.Contains("Mobile-RefreshToken")).ToList().Count;
                }
            }
            catch
            {

                throw new RedisNotAvailableException();
            }
        }

        public bool IsSet(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    return client.ContainsKey(key);
                }
            }
            catch
            {

                throw new RedisNotAvailableException();
            }
        }

        public void Remove(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    client.Remove(key);
                }
            }
            catch
            {
                throw new RedisNotAvailableException();
            }
        }

        public void RemoveByPattern(string pattern)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    var keys = client.SearchKeys(pattern);
                    client.RemoveAll(keys);
                }
            }
            catch
            {

                throw new RedisNotAvailableException();
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
