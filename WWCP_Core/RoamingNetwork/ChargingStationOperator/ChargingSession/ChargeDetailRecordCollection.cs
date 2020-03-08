/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using System.IO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections;

#endregion

namespace org.GraphDefined.WWCP
{

    public class ChargeDetailRecordCollection : IEnumerable<ChargeDetailRecord>
    {

        private List<ChargeDetailRecord> _ChargeDetailRecords;

        public ChargingSession_Id ChargingSessionId { get; }


        public ChargeDetailRecordCollection(ChargeDetailRecord ChargeDetailRecord)
            : this(ChargeDetailRecord.SessionId)
        {
            Add(ChargeDetailRecord);
        }

        public ChargeDetailRecordCollection(ChargingSession_Id ChargingSessionId)
        {
            this.ChargingSessionId     = ChargingSessionId;
            this._ChargeDetailRecords  = new List<ChargeDetailRecord>();
        }

        public ChargeDetailRecordCollection(ChargingSession_Id  ChargingSessionId,
                                            ChargeDetailRecord  ChargeDetailRecord)
            : this(ChargingSessionId)
        {
            Add(ChargeDetailRecord);
        }

        public ChargeDetailRecordCollection(ChargingSession_Id               ChargingSessionId,
                                            IEnumerable<ChargeDetailRecord>  ChargeDetailRecords)
            : this(ChargingSessionId)
        {
            Add(ChargeDetailRecords);
        }





        public ChargeDetailRecordCollection Add(ChargeDetailRecord ChargeDetailRecord)
        {
            lock (_ChargeDetailRecords)
            {
                _ChargeDetailRecords.Add(ChargeDetailRecord);
            }
            return this;
        }

        public ChargeDetailRecordCollection Add(IEnumerable<ChargeDetailRecord> ChargeDetailRecords)
        {
            lock (_ChargeDetailRecords)
            {
                _ChargeDetailRecords.AddRange(ChargeDetailRecords);
            }
            return this;
        }

        public IEnumerator<ChargeDetailRecord> GetEnumerator()
            => _ChargeDetailRecords.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _ChargeDetailRecords.GetEnumerator();

    }

}
