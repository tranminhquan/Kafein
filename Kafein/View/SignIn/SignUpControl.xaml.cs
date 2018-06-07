using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kafein.Utilities;
using Kafein.View.Dialog;

namespace Kafein.View.SignIn
{
    /// <summary>
    ///     Interaction logic for SignUpControl.xaml
    /// </summary>
    public partial class SignUpControl : UserControl, IHavePassword
    {
        public SignUpControl()
        {
            InitializeComponent();
        }

        public SecureString Password => FloatingPasswordBox.SecurePassword;

        public SecureString ConfirmPassword => FloatingConfirmPasswordBox.SecurePassword;
    }
}