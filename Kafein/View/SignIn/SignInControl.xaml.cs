using System.Security;
using System.Windows.Controls;
using Kafein.Utilities;
using Kafein.ViewModel;
using Newtonsoft.Json;

namespace Kafein.View.SignIn
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInControl : UserControl, IHavePassword
    {
        public SignInControl()
        {
            InitializeComponent();
            //DataContext = new LoginViewModel();
        }

        public SecureString Password
        {
            get
            {
                return FloatingPasswordBox.SecurePassword;
            }
        }

        public SecureString ConfirmPassword { get; }
    }
}
