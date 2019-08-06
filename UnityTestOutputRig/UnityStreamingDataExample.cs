using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTestOutputRig
{
    class UnityStreamingDataExample
    {
        public const string Deliminator = ",";

        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public bool HasChanged { get; set; }

        
        public UnityStreamingDataExample()
        {
            X = "0";
            Y = "0";
            Z = "0";
            HasChanged = false;
        }

        public override string ToString()
        {
            string value =  HasChanged + Deliminator + X + Deliminator + Y + Deliminator + Z;
            HasChanged = false;
            return value;
        }
    }
}
