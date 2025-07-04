﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.Domain.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly JsonSerializerSettings s_jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static async Task<T?> GetOrCreateJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            Func<Task<T?>> factory,
            DistributedCacheEntryOptions options,
            CancellationToken cancellationToken = default) where T : class
        {
            var json = await cache.GetStringAsync(key, cancellationToken);

            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<T>(json, s_jsonSerializerSettings)!;
            }

            var value = await factory();

            if (value == default)
            {
                return value;
            }

            json = JsonConvert.SerializeObject(value, s_jsonSerializerSettings);

            await cache.SetStringAsync(key, json, options, cancellationToken);

            return value;
        }
    }
}
