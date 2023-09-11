/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Collections;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class ChargeDetailRecordCollection : IId<ChargingSession_Id>,
                                                IHasId<ChargingSession_Id>,
                                                IEnumerable<ChargeDetailRecord>
    {

        private List<ChargeDetailRecord> _ChargeDetailRecords;

        public ChargingSession_Id Id { get; }

        public ulong Length => throw new NotImplementedException();

        public bool IsNullOrEmpty => throw new NotImplementedException();

        public bool IsNotNullOrEmpty => throw new NotImplementedException();

        public ChargeDetailRecordCollection(ChargeDetailRecord ChargeDetailRecord)
            : this(ChargeDetailRecord.SessionId)
        {
            Add(ChargeDetailRecord);
        }

        public ChargeDetailRecordCollection(ChargingSession_Id ChargingSessionId)
        {
            this.Id                    = ChargingSessionId;
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



        public int CompareTo(Object? obj)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(ChargingSession_Id other)
            => Id.CompareTo(other);

        public Boolean Equals(ChargingSession_Id other)
            => Id.Equals(other);

        public override String ToString()
            => "";

    }

}
