using System;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public interface IResult : IComparable<IResult>, IComparable
    {
        Kind Kind { get; }
        ValueKind ValueKind { get; }
        decimal Value { get; }
    }
}