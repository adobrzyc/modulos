namespace Modulos.Pipes
{
    /// <summary>
    /// Defines optional pipe, which will be executed after the required parameters become available
    /// during the pipeline process. The checking process is executed after each <see cref="IPipe" /> execution.
    /// </summary>
    public interface IOptionalPipe : IPipe
    {
    }
}