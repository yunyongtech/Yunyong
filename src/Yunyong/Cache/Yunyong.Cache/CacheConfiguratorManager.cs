using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Yunyong.Cache.Abstractions;

namespace Yunyong.Cache
{
    /// <summary>
    ///     缓存配置管理
    /// </summary>
    public class CacheConfiguratorManager : ICacheConfiguratorManager
    {
        private readonly List<ICacheConfigurator> _configurators;

        public CacheConfiguratorManager()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        public IReadOnlyList<ICacheConfigurator> CacheConfigurators
        {
            get => _configurators.ToImmutableList();
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }

        public virtual void Dispose()
        {
        }
    }
}