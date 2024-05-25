using Marketplaes02.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Model
{
    public class All_MyOrder :  MyOrder_items
    {

       
            public MyOrders Order { get; set; }
            public IList<MyOrder_items> Items { get; set; }
 
    }
}
