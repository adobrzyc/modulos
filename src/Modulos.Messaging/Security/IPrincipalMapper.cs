using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public interface IPrincipalMapper
    {
        bool IsMatch([JetBrains.Annotations.NotNull] IAuthenticationData authData, [JetBrains.Annotations.NotNull] Type userType);
        ValueTask<IPrincipal> Map([JetBrains.Annotations.NotNull] IAuthenticationData authData);
    }
}