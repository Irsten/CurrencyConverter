using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class Root
    {
        // Get all Record in rates and Set in Rate Class as Currency Name Wise
        public Rate rates { get; set; }
        public long timestamp;
        public string license;
    }
}
