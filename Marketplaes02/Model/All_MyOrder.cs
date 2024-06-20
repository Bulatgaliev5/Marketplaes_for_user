

namespace Marketplaes02.Model
{
    public class All_MyOrder :  MyOrder_items
    {

       
            public MyOrders Order { get; set; }
            public IList<MyOrder_items> Items { get; set; }
 
    }
}
