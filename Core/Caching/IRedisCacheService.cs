﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Caching
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);
        IList<T> GetAll<T>(string key);
        void Set(string key, object data);
        void Set(string key, object data, DateTime time);
        void SetAll<T>(IDictionary<string, T> values);
        bool IsSet(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        void Clear();
        int Count(string key);
    }
}
 
