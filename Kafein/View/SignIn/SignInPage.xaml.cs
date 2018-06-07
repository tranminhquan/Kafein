using System.Windows;
using Kafein.Utilities;

namespace Kafein.View.SignIn
{
    /// <summary>
    ///     Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            var socketAPI = SocketAPI.GetInstance();
        }
    }
}