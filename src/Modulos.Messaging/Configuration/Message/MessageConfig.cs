using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Security;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Transport;

namespace Modulos.Messaging.Configuration
{
    public class MessageConfig : Freezable, IMessageConfig
    {
        public MessageConfig()
        {
          
        }

        public MessageConfig(IMessageConfig source)
        {
            CopyFromSource(source);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void CopyFromSource(IMessageConfig source)
        {
            authenticationMode = source.AuthenticationMode;
            requestSerializer = source.RequestSerializer;
            responseSerializerId = source.ResponseSerializer;
            requestCompressionEngine = source.RequestCompressor;
            responseCompressionEngine = source.ResponseCompressor;
            timeout = source.Timeout;
            maxInvokeAttempts = source.MaxInvokeAttempts;
            endpointConfigMark = source.EndpointConfigMark;
            transportEngine = source.TransportEngine;
            supportConnectionMark = source.SupportConnectionMark;
            
            if(source.Properties.Count > 0)
                Properties = new Dictionary<string, string>(source.Properties);
        }

        public TransportEngineId TransportEngine
        {
            get => transportEngine;
            set
            {
                ThrowIfFrozen();
                transportEngine = value;
            }
        }

        public AuthenticationMode AuthenticationMode
        {
            get => authenticationMode;
            set
            {
                ThrowIfFrozen();
                authenticationMode = value;
            }
        }

        public SerializerId RequestSerializer
        {
            get => requestSerializer;
            set
            {
                ThrowIfFrozen();
                requestSerializer = value;
            }
        }

        public SerializerId ResponseSerializer
        {
            get => responseSerializerId;
            set
            {
                ThrowIfFrozen();
                responseSerializerId = value;
            }
        }

        public CompressionEngineId RequestCompressor
        {
            get => requestCompressionEngine;
            set
            {
                ThrowIfFrozen();
                requestCompressionEngine = value;
            }
        }

        public CompressionEngineId ResponseCompressor
        {
            get => responseCompressionEngine;
            set
            {
                ThrowIfFrozen(); 
                responseCompressionEngine = value;
            }
        }

        public TimeSpan Timeout
        {
            get => timeout;
            set
            {
                ThrowIfFrozen();
                timeout = value;
            }
        }

        public int MaxInvokeAttempts
        {
            get => maxInvokeAttempts;
            set
            {
                ThrowIfFrozen();
                maxInvokeAttempts = value;
            }
        }

        public bool SupportConnectionMark
        {
            get => supportConnectionMark;
            set
            {
                ThrowIfFrozen();
                supportConnectionMark = value;
            }
        }


        public IDictionary<string, string> Properties { get; private set; } = new ConcurrentDictionary<string, string>();

        public EndpointConfigMark EndpointConfigMark
        {
            get => endpointConfigMark;
            set
            {
                ThrowIfFrozen();
                endpointConfigMark = value;
            }
        }

        public override void Freeze()
        {
            base.Freeze();
            Properties = new ReadOnlyDictionary<string, string>(Properties);
        }

        public string this[string key]
        {
            get => Properties[key];
            set
            {
                if (IsFrozen)
                    throw new ObjectIsFrozenException("Collection is frozen.");

                if (Properties.ContainsKey(key))
                    Properties[key] = value;
                else Properties.Add(key,value);
            }
        }

        private AuthenticationMode authenticationMode = AuthenticationMode.None;
        private TimeSpan timeout;
        private int maxInvokeAttempts = 1;
        private EndpointConfigMark endpointConfigMark;
        private SerializerId requestSerializer = Serializers.JsonNet;
        private SerializerId responseSerializerId = Serializers.JsonNet;
        private CompressionEngineId requestCompressionEngine = CompressionEngines.None;
        private CompressionEngineId responseCompressionEngine = CompressionEngines.None;
        private TransportEngineId transportEngine;
        private bool supportConnectionMark;
    }
}