using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Configuration
{
    //todo: [optimization] maybe it would be nice to depends on message type instead of IMessage instance
    //      it may boost performance due to cache IMessageConfig per message type 
    public class MessageConfigProvider : IMessageConfigProvider, IDisposable
    {
        private IEnumerable<IMessageConfigDefiner> definers;
        private ConcurrentDictionary<Type,IMessageConfigDefiner[]> definersCache = new ConcurrentDictionary<Type, IMessageConfigDefiner[]>();
       
        public MessageConfigProvider(IEnumerable<IMessageConfigDefiner> definers)
        {
            this.definers = definers;
        }

        public IMessageConfig GetConfig(IMessage message)
        {
            var msgType = message.GetType();
         
            IMessageConfig config = new MessageConfig();

            
            if (!definersCache.TryGetValue(msgType, out var matchedDefiners))
            {
                matchedDefiners = definers
                    .Where(e => e.IsForThisMessage(message))
                    .OrderBy(e => e.Order).ToArray();

                definersCache.TryAdd(msgType, matchedDefiners);  
            }

            foreach (var definer in matchedDefiners)
            {
                definer.GetConfig(message, ref config);
            }
          
            return config;
        }

        public void Dispose()
        {
            definersCache.Clear();
            definersCache = null;
            definers = null;
        }
    }

}