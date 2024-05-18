using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Class
{
    public class UpdateFillter
    {
        public int OtPrice { get; }
        public int DoPrice { get; }
        public UpdateFillter(int otPrice, int doPrice)
        {
            OtPrice = otPrice;
            DoPrice = doPrice;
        }
    }

}
