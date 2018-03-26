using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfferTest.Func
{
    public class CountryProxy
    {
        string country;
        string proxy;

        public string Country { get => country; set => country = value; }
        public string Proxy { get => proxy; set => proxy = value; }
    }
}
