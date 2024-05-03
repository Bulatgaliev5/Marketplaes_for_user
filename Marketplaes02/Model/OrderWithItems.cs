using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class OrderWithItems :  MyOrder_items,  INotifyPropertyChanged
    {

       
            public MyOrders Order { get; set; }
            public IList<MyOrder_items> Items { get; set; }
 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
    }
}
