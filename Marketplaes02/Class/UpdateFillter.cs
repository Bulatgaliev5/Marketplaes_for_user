using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplaes02.Class
{
    public class UpdateFillter
    {
        public float OtPrice { get; }
        public float DoPrice { get; }
        public UpdateFillter(float otPrice, float doPrice)
        {
            OtPrice = otPrice;
            DoPrice = doPrice;
        }
    }

}
