using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.EventBus;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Messaging.Configuration
{
    [OptimizationRequired]
    public class EndpointConfigProvider : IEndpointConfigProvider, 
        IHandleEvent<DiscardEndpointConfigEvent>, 
        IHandleEvent<DiscardAllEndpointConfigsEvent>, IDisposable
    {
        private readonly SemaphoreSlim mutex = new SemaphoreSlim(1,1);
        private readonly Dictionary<EndpointConfigMark, IEnumerable<IEndpointConfig>> configs = new Dictionary<EndpointConfigMark, IEnumerable<IEndpointConfig>>();
        private readonly List<EndpointConfigInfo> configInfoCollection = new List<EndpointConfigInfo>();
       
        private readonly IEnumerable<IEndpointConfigSource> sources;

        public EndpointConfigProvider(IEnumerable<IEndpointConfigSource> sources)
        {
            this.sources = sources;
        }

        public async Task<bool> Exists(EndpointConfigInfo endpointConfigInfo)
        {
            if (endpointConfigInfo == null) throw new ArgumentNullException(nameof(endpointConfigInfo));

            await mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                if (! await PrepareConfigSet(endpointConfigInfo.EndpointConfigMark).ConfigureAwait(false))
                    throw new TodoException($"Unable to determine client configuration for: {endpointConfigInfo.EndpointConfigMark}.");
               
                return configInfoCollection.Contains(endpointConfigInfo);
            }
            finally
            {
                mutex.Release();
            }
        }

        public async Task<IEnumerable<IEndpointConfig>> GetAll(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

            await mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                if(! await PrepareConfigSet(endpointConfigMark).ConfigureAwait(false))
                    throw new TodoException($"Unable to determine client configuration for: {endpointConfigMark}.");

                return configs[endpointConfigMark];
            }
            finally
            {
                mutex.Release();
            }
        }

        public async Task<IEndpointConfig> GetPreferred(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

            await mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                if(! await PrepareConfigSet(endpointConfigMark).ConfigureAwait(false))
                    throw new TodoException($"Unable to determine client configuration for: {endpointConfigMark}.");

                var getPreferred = new Func<IEndpointConfig>(() =>
                {
                    var matched = configs[endpointConfigMark]
                        .Where(e=>e.IsAvailable);

                    return matched.OrderByDescending(i => i.IsDefault)
                        .ThenBy(i => i.Order)
                        .ThenBy(i => i.ExpirationTimeUtc ?? DateTime.MaxValue.ToUniversalTime())
                        .FirstOrDefault();
                });

                var preferred = getPreferred();
                if (preferred == null)
                    await Refresh(endpointConfigMark);
                preferred = getPreferred();

                if (preferred == null)
                    throw new TodoException("Unable to determine client configuration.");

            
                return preferred;
            }
            finally
            {
                mutex.Release();
            }
        }


        public async Task Discard(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

            await mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                if (configs.ContainsKey(endpointConfigMark))
                    configs.Remove(endpointConfigMark);
            }
            finally
            {
                mutex.Release();
            }
        }

        public async Task DiscardAll()
        {
            await mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                configs.Clear();
            }
            finally
            {
                mutex.Release();
            }
        }

        private async Task Refresh(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

            await Discard(endpointConfigMark);
            await PrepareConfigSet(endpointConfigMark);
        }


        private async Task<bool> PrepareConfigSet(EndpointConfigMark endpointConfigMark)
        {
            if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

            if (configs.ContainsKey(endpointConfigMark)) return true;

            var configurations = new List<IEndpointConfig>();

            var filteredSources = sources.Where(e => e.Transport == endpointConfigMark.Transport);
            foreach (var endpointConfigSource in filteredSources)
            {
                var configurationToAdd = await endpointConfigSource.GetConfiguration(endpointConfigMark).ConfigureAwait(false);
                configurations.AddRange(configurationToAdd);
            }

            if (!configurations.Any()) return false;
                
            configInfoCollection.RemoveAll(e => Equals(e.EndpointConfigMark, endpointConfigMark));
            configInfoCollection.AddRange(configurations.Select(e=>e.Info));
            configs.Add(endpointConfigMark, configurations);

            return true;
        }


        public async Task Handle(DiscardEndpointConfigEvent @event)
        {
            await Discard(@event.EndpointConfigMark);
        }

        public async Task Handle(DiscardAllEndpointConfigsEvent @event)
        {
            await DiscardAll();
        }

        public void Dispose()
        {
            mutex?.Dispose();
        }


        //todo: cleanup
        //public async Task<T> GetPreferred<T>(EndpointConfigMark endpointConfigMark) where T : IEndpointConfig
        //{
        //    if (endpointConfigMark == null) throw new ArgumentNullException(nameof(endpointConfigMark));

        //    await mutex.WaitAsync().ConfigureAwait(false);

        //    try
        //    {
        //        if(! await PrepareConfigSet(endpointConfigMark).ConfigureAwait(false))
        //            throw new TodoException($"Unable to determine client configuration for: {endpointConfigMark}.");

        //        var getPreferred = new Func<IEndpointConfig>(() =>
        //        {
        //            var matched = configs[endpointConfigMark]
        //                .Where(e=>e.IsAvailable);

        //            return matched.OrderByDescending(i => i.IsDefault)
        //                .ThenBy(i => i.Order)
        //                .ThenBy(i => i.ExpirationTimeUtc ?? DateTime.MaxValue.ToUniversalTime())
        //                .FirstOrDefault();
        //        });

        //        var preferred = getPreferred();
        //        if (preferred == null)
        //            await Refresh(endpointConfigMark);
        //        preferred = getPreferred();

        //        if (preferred == null)
        //            throw new TodoException("Unable to determine client configuration.");

        //        if(!(preferred is T))
        //            throw new TodoException("Invalid configuration: invalid type of config.");

        //        return (T)preferred;
        //    }
        //    finally
        //    {
        //        mutex.Release();
        //    }
        //}
    }
}