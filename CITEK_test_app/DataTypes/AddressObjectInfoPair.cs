using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITEK_test_app
{
    public class AddressObjectInfoPair
    {
        public string Type { get; private set; }

        public string Name { get; private set; }

        public AddressObjectInfoPair(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
