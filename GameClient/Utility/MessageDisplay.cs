using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameClient.Utility
{
    public class MessageDisplay : IMessageDisplay
    {
        public void ShowMessage(string message)
        {
            HandyControl.Controls.MessageBox.Show(message);
        }
    }
}
