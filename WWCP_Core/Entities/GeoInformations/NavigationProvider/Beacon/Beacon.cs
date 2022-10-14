using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cloud.charging.open.protocols.WWCP.Core.Entities.Beacon
{

    public class Beacon
    {

        /// <summary>
        /// Same as UUID
        /// </summary>
        public String Id                      { get; }

        public String name                    { get; }

        public String description             { get; }

        public String proximityId             { get; }

        public String major                   { get; }

        public String minor                   { get; }

        public String latitude                { get; }

        public String longitude               { get; }

        public String TXPower                 { get; }

        public String AdvertisementInterval   { get; }

        // Eddystone
        public String UID { get; }
        public String URL { get; }
        public String TLM { get; }
        public String EID { get; }

    }

}
