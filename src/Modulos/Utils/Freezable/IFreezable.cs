// ReSharper disable UnusedMemberInSuper.Global
namespace Modulos
{
    public interface IFreezable
    {
        void Freeze();
        bool IsFrozen { get; }
    }
}