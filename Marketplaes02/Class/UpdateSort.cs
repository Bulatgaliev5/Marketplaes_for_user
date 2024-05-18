using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Model;

namespace Marketplaes02.Class
{
    public class UpdateSort
    {
        public string SelectParam { get; }

        public UpdateSort(string selectParam)
        {
            SelectParam = selectParam;
        }
    }

}
