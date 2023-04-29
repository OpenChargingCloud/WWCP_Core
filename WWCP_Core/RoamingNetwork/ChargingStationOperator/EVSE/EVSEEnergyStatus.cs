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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the EVSE energy status.
    /// </summary>
    public static class EVSEEnergyStatusExtensions
    {

        #region ToJSON(this EVSEEnergyStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EVSEEnergyStatus>  EVSEEnergyStatus,
                                     UInt64?                             Skip   = null,
                                     UInt64?                             Take   = null)
        {

            #region Initial checks

            if (EVSEEnergyStatus is null || !EVSEEnergyStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate EVSE identifications in the enumeration... take the newest one!

            var filteredEnergyInfo = new Dictionary<EVSE_Id, EVSEEnergyStatus>();

            foreach (var status in EVSEEnergyStatus)
            {

                if (!filteredEnergyInfo.ContainsKey(status.Id))
                    filteredEnergyInfo.Add(status.Id, status);

                else if (filteredEnergyInfo[status.Id].Timestamp >= status.Timestamp)
                    filteredEnergyInfo[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? filteredEnergyInfo.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : filteredEnergyInfo.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Timestamp.  ToIso8601(),
                                                                          kvp.Value.EnergyInfo.ToString())
                                                              )));

        }

        #endregion

    }


    /// <summary>
    /// The current energy status of an EVSE.
    /// </summary>
    public readonly struct EVSEEnergyStatus : IEquatable<EVSEEnergyStatus>,
                                              IComparable<EVSEEnergyStatus>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id     Id            { get; }

        /// <summary>
        /// The timestamp of the current energy status of the EVSE.
        /// </summary>
        public DateTime    Timestamp     { get; }

        /// <summary>
        /// The current energy status of the EVSE.
        /// </summary>
        public EnergyInfo  EnergyInfo    { get; }

        /// <summary>
        /// An optional data source or context for this EVSE energy status.
        /// </summary>
        public String?     DataSource    { get; }

        /// <summary>
        /// The timestamped energy status of the EVSE.
        /// </summary>
        public Timestamped<EnergyInfo> TimestampedEnergyStatus
            => new (Timestamp, EnergyInfo);

        #endregion

        #region Constructor(s)

        #region EVSEEnergyStatus(Id,            EnergyInfo, DataSource = null)

        /// <summary>
        /// Create a new EVSE energy status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="EnergyInfo">The current timestamped energy information of the EVSE.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE energy status.</param>
        public EVSEEnergyStatus(EVSE_Id                  Id,
                                Timestamped<EnergyInfo>  EnergyInfo,
                                String?                  DataSource   = null)

            : this(Id,
                   EnergyInfo.Timestamp,
                   EnergyInfo.Value,
                   DataSource)

        { }

        #endregion

        #region EVSEEnergyStatus(Id, Timestamp, EnergyInfo, DataSource = null)

        /// <summary>
        /// Create a new EVSE energy status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Timestamp">The timestamp of the energy information of the EVSE.</param>
        /// <param name="EnergyInfo">The current energy information of the EVSE.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE energy status.</param>
        public EVSEEnergyStatus(EVSE_Id     Id,
                                DateTime    Timestamp,
                                EnergyInfo  EnergyInfo,
                                String?     DataSource   = null)
        {

            this.Id          = Id;
            this.EnergyInfo  = EnergyInfo;
            this.Timestamp   = Timestamp;
            this.DataSource  = DataSource;

            unchecked
            {

                hashCode = Id.         GetHashCode() * 7 ^
                           EnergyInfo. GetHashCode() * 5 ^
                           Timestamp.  GetHashCode() * 3 ^
                          (DataSource?.GetHashCode() ?? 0);

            }

        }

        #endregion

        #endregion



        #region Operator overloading

        #region Operator == (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEEnergyStatus EVSEEnergyStatus1,
                                           EVSEEnergyStatus EVSEEnergyStatus2)

            => EVSEEnergyStatus1.Equals(EVSEEnergyStatus2);

        #endregion

        #region Operator != (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEEnergyStatus EVSEEnergyStatus1,
                                           EVSEEnergyStatus EVSEEnergyStatus2)

            => !EVSEEnergyStatus1.Equals(EVSEEnergyStatus2);

        #endregion

        #region Operator <  (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEEnergyStatus EVSEEnergyStatus1,
                                          EVSEEnergyStatus EVSEEnergyStatus2)

            => EVSEEnergyStatus1.CompareTo(EVSEEnergyStatus2) < 0;

        #endregion

        #region Operator <= (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEEnergyStatus EVSEEnergyStatus1,
                                           EVSEEnergyStatus EVSEEnergyStatus2)

            => EVSEEnergyStatus1.CompareTo(EVSEEnergyStatus2) <= 0;

        #endregion

        #region Operator >  (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEEnergyStatus EVSEEnergyStatus1,
                                          EVSEEnergyStatus EVSEEnergyStatus2)

            => EVSEEnergyStatus1.CompareTo(EVSEEnergyStatus2) > 0;

        #endregion

        #region Operator >= (EVSEEnergyStatus1, EVSEEnergyStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus1">An EVSE energy status.</param>
        /// <param name="EVSEEnergyStatus2">Another EVSE energy status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEEnergyStatus EVSEEnergyStatus1,
                                           EVSEEnergyStatus EVSEEnergyStatus2)

            => EVSEEnergyStatus1.CompareTo(EVSEEnergyStatus2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEEnergyStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEEnergyStatus evseEnergyStatus
                   ? CompareTo(evseEnergyStatus)
                   : throw new ArgumentException("The given object is not an EVSE energy status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEEnergyStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatus">An object to compare with.</param>
        public Int32 CompareTo(EVSEEnergyStatus EVSEEnergyStatus)
        {

            var c = Id.                   CompareTo(EVSEEnergyStatus.Id);

            if (c == 0)
                c = EnergyInfo.           CompareTo(EVSEEnergyStatus.EnergyInfo);

            if (c == 0)
                c = Timestamp.ToIso8601().CompareTo(EVSEEnergyStatus.Timestamp.ToIso8601());

            if (c == 0 && DataSource is not null && EVSEEnergyStatus.DataSource is not null)
                c = DataSource.           CompareTo(EVSEEnergyStatus.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEEnergyStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EVSEEnergyStatus evseEnergyStatus &&
                   Equals(evseEnergyStatus);

        #endregion

        #region Equals(EVSEEnergyStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEEnergyStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEEnergyStatus EVSEEnergyStatus)

            => Id.                   Equals(EVSEEnergyStatus.Id)                    &&
               EnergyInfo.           Equals(EVSEEnergyStatus.EnergyInfo)            &&
               Timestamp.ToIso8601().Equals(EVSEEnergyStatus.Timestamp.ToIso8601()) &&

             ((DataSource is null     && EVSEEnergyStatus.DataSource is null) ||
              (DataSource is not null && EVSEEnergyStatus.DataSource is not null && DataSource.Equals(EVSEEnergyStatus.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id} -> '{EnergyInfo}' since {Timestamp.ToIso8601()}";

        #endregion

    }

}
