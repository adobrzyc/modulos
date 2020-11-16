using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class SampleQueryHandler : IQueryHandler<SampleQuery, string>
    {
        public Task<string> Handle(SampleQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            if(query.ThrowException)
                throw new Exception(DateTime.Now.ToString(CultureInfo.InvariantCulture));

            var output = query.Data + " " + DateTime.Now.ToLongTimeString();
            return Task.FromResult(output);
        }
    }
}