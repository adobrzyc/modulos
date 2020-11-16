namespace Modulos.Pipes
{
    /// <summary>
    /// Defines optional pipe, which will be executed after required parameter
    /// becomes available during pipeline process. Checking process is executed
    /// after each <see cref="IPipe"/> execution.
    /// </summary>
    public interface IOptionalPipe : IPipe
    {

    }
}