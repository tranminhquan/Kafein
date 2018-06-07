using System.Security;

namespace Kafein.Model.Interface
{
    public interface IHavePassword
    {
        SecureString Password { get; }
        SecureString ConfirmPassword { get; }
    }
}