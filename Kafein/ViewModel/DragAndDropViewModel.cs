using Kafein.Utilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kafein.ViewModel
{
    public class DragAndDropViewModel
    {
        public DragAndDropViewModel()
        {
            MouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnMouseDown);
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            Debug.LogOutput(e.GetPosition(null).X + " " + e.GetPosition(null).Y);
        }

        public DelegateCommand<MouseButtonEventArgs> MouseDownCommand { get; set; }
    }
}
