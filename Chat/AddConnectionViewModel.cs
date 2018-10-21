using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Chat.Model.BasicTypes;

namespace Chat
{
    public class AddConnectionViewModel
    {
        public string IPAddress { get; set; }
        public string Port { get; set; }
        public ICommand Accept
        {
            get
            {
                return _accept ?? (_accept = new RelayCommand(@object => {
                    DialogAccepted.Invoke(this, new AddClientEventArgs(IPAddress, Int32.Parse(Port)));
                }));
            }
        }
        public EventHandler<AddClientEventArgs> DialogAccepted;
        private ICommand _accept;
    }
}
