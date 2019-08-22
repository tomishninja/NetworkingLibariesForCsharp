using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityTestOutputRig
{
    class UnityStreamingDataExample
    {
        public const string Deliminator = "_";
        
        public bool HasChanged { get; set; }

        public float posX { get; set; }
        public float posY { get; set; }
        public float posZ { get; set; }

        public UnityStreamingDataExample()
        {
            posX = 0;
            posY = 0;
            posZ = 0;
            HasChanged = false;
        }

        public override string ToString()
        {
            string value =  HasChanged + Deliminator + posX + Deliminator + posY + Deliminator + posZ;
            HasChanged = false;
            return value;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
