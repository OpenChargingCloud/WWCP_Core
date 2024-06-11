/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for the e-mobility provider status.
    /// </summary>
    public static class EMobilityProviderStatusExtensions
    {

        #region ToJSON(this EMobilityProviderStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EMobilityProviderStatus>  EMobilityProviderStatus,
                                     UInt64?                                          Skip   = null,
                                     UInt64?                                          Take   = null)
        {

            #region Initial checks

            if (EMobilityProviderStatus is null || !EMobilityProviderStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate EMobilityProvider identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<EMobilityProvider_Id, EMobilityProviderStatus>();

            foreach (var status in EMobilityProviderStatus)
            {

                if (!filteredStatus.ContainsKey(status.Id))
                    filteredStatus.Add(status.Id, status);

                else if (filteredStatus[status.Id].Timestamp >= status.Timestamp)
                    filteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject((Take.HasValue ? filteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                                              : filteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                                   Select(kvp => new JProperty(kvp.Key.ToString(),
                                                               new JArray(kvp.Value.Timestamp.  ToIso8601(),
                                                                          kvp.Value.Status.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this EMobilityProviderStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of e-mobility providers and their current status
        /// contains the given pair of e-mobility provider identification and status.
        /// </summary>
        /// <param name="EMobilityProviderStatus">An enumeration of e-mobility providers and their current status.</param>
        /// <param name="Id">A e-mobility provider identification.</param>
        /// <param name="Status">A e-mobility provider status.</param>
        public static Boolean Contains(this IEnumerable<EMobilityProviderStatus>  EMobilityProviderStatus,
                                       EMobilityProvider_Id                       Id,
                                       EMobilityProviderStatusTypes               Status)
        {

            foreach (var status in EMobilityProviderStatus)
            {
                if (status.Id          == Id &&
                    status.Status == Status)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current status of an e-mobility provider.
    /// </summary>
    public class EMobilityProviderStatus : AInternalData,
                                           IEquatable<EMobilityProviderStatus>,
                                           IComparable<EMobilityProviderStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id          Id           { get; }

        /// <summary>
        /// The current status of the e-mobility provider.
        /// </summary>
        public EMobilityProviderStatusTypes  Status       { get; }

        /// <summary>
        /// The timestamp of the current status of the e-mobility provider.
        /// </summary>
        public DateTime                      Timestamp    { get; }

        /// <summary>
        /// The timestamped status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderStatusTypes> TimestampedStatus
            => new (Timestamp, Status);

        #endregion

        #region Constructor(s)

        #region EMobilityProviderStatus(Id, Status,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new e-mobility provider status.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="Status">The current timestamped status of the e-mobility provider.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EMobilityProviderStatus(EMobilityProvider_Id                       Id,
                                       Timestamped<EMobilityProviderStatusTypes>  Status,
                                       JObject?                                   CustomData     = null,
                                       UserDefinedDictionary?                     InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status.Value;
            this.Timestamp  = Status.Timestamp;

        }

        #endregion

        #region EMobilityProviderStatus(Id, Status, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new e-mobility provider status.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="Status">The current status of the e-mobility provider.</param>
        /// <param name="Timestamp">The timestamp of the status change of the e-mobility provider.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EMobilityProviderStatus(EMobilityProvider_Id          Id,
                                       EMobilityProviderStatusTypes  Status,
                                       DateTime                      Timestamp,
                                       JObject?                      CustomData     = null,
                                       UserDefinedDictionary?        InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(EMobilityProvider)

        /// <summary>
        /// Take a snapshot of the current e-mobility provider status.
        /// </summary>
        /// <param name="EMobilityProvider">A e-mobility provider.</param>
        public static EMobilityProviderStatus Snapshot(EMobilityProvider EMobilityProvider)

            => new (EMobilityProvider.Id,
                    EMobilityProvider.Status);

        #endregion


        #region Operator overloading

        #region Operator == (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityProviderStatus EMobilityProviderStatus1,
                                           EMobilityProviderStatus EMobilityProviderStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EMobilityProviderStatus1, EMobilityProviderStatus2))
                return true;

            // If one is null, but not both, return false.
            if (EMobilityProviderStatus1 is null || EMobilityProviderStatus2 is null)
                return false;

            return EMobilityProviderStatus1.Equals(EMobilityProviderStatus2);

        }

        #endregion

        #region Operator != (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityProviderStatus EMobilityProviderStatus1,
                                           EMobilityProviderStatus EMobilityProviderStatus2)

            => !(EMobilityProviderStatus1 == EMobilityProviderStatus2);

        #endregion

        #region Operator <  (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityProviderStatus EMobilityProviderStatus1,
                                          EMobilityProviderStatus EMobilityProviderStatus2)
        {

            if (EMobilityProviderStatus1 is null)
                throw new ArgumentNullException(nameof(EMobilityProviderStatus1), "The given EMobilityProviderStatus1 must not be null!");

            return EMobilityProviderStatus1.CompareTo(EMobilityProviderStatus2) < 0;

        }

        #endregion

        #region Operator <= (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityProviderStatus EMobilityProviderStatus1,
                                           EMobilityProviderStatus EMobilityProviderStatus2)

            => !(EMobilityProviderStatus1 > EMobilityProviderStatus2);

        #endregion

        #region Operator >  (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityProviderStatus EMobilityProviderStatus1,
                                          EMobilityProviderStatus EMobilityProviderStatus2)
        {

            if (EMobilityProviderStatus1 is null)
                throw new ArgumentNullException(nameof(EMobilityProviderStatus1), "The given EMobilityProviderStatus1 must not be null!");

            return EMobilityProviderStatus1.CompareTo(EMobilityProviderStatus2) > 0;

        }

        #endregion

        #region Operator >= (EMobilityProviderStatus1, EMobilityProviderStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus1">A e-mobility provider status.</param>
        /// <param name="EMobilityProviderStatus2">Another e-mobility provider status.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityProviderStatus EMobilityProviderStatus1,
                                           EMobilityProviderStatus EMobilityProviderStatus2)

            => !(EMobilityProviderStatus1 < EMobilityProviderStatus2);

        #endregion

        #endregion

        #region IComparable<EMobilityProviderStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMobilityProviderStatus chargingStationOperatorStatus
                   ? CompareTo(chargingStationOperatorStatus)
                   : throw new ArgumentException("The given object is not an e-mobility provider status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityProviderStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatus">An object to compare with.</param>
        public Int32 CompareTo(EMobilityProviderStatus? EMobilityProviderStatus)
        {

            if (EMobilityProviderStatus is null)
                throw new ArgumentNullException(nameof(EMobilityProviderStatus), "The given e-mobility provider status must not be null!");

            var c = Id.       CompareTo(EMobilityProviderStatus.Id);

            if (c == 0)
                c = Status.   CompareTo(EMobilityProviderStatus.Status);

            if (c == 0)
                c = Timestamp.CompareTo(EMobilityProviderStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EMobilityProviderStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityProviderStatus chargingStationOperatorStatus &&
                   Equals(chargingStationOperatorStatus);

        #endregion

        #region Equals(EMobilityProviderStatus)

        /// <summary>
        /// Compares two EMobilityProvider identifications for equality.
        /// </summary>
        /// <param name="EMobilityProviderStatus">A e-mobility provider identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMobilityProviderStatus? EMobilityProviderStatus)

            => EMobilityProviderStatus is not null              &&
               Id.       Equals(EMobilityProviderStatus.Id)     &&
               Status.   Equals(EMobilityProviderStatus.Status) &&
               Timestamp.Equals(EMobilityProviderStatus.Timestamp);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.       GetHashCode() * 5 ^
                       Status.   GetHashCode() * 3 ^
                       Timestamp.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
