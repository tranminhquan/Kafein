using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kafein.View.Dialog
{
    /// <summary>
    /// Interaction logic for MessageInfo.xaml
    /// </summary>
    public partial class MessageInfo : Window
    {
        public MessageInfo()
        {
            InitializeComponent();
        }

        public MessageInfo(string content, string title): this()
        {
            tbContent.Text = content;
            tbTitle.Text = title;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "MessageInfo")
                    window.Close();
        }
    }
}
