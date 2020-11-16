using System;
using System.Text;

namespace Modulos.Errors
{
    internal static class Extensions
    {
        public static string GetMessage(this Exception e)
        {
            var sb =new StringBuilder(e.Message);
            var inner = e.InnerException;
            int counter = 1;
            while (inner != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append($" [{counter}] ");
                sb.Append(inner.Message);
               
                inner = inner.InnerException;
                counter++;
            }
            return sb.ToString();
        }
    }
}
