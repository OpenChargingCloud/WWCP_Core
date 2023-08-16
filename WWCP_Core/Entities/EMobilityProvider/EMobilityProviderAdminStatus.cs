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
    /// Extension methods for the e-mobility provider admin status.
    /// </summary>
    public static class EMobilityProviderAdminStatusExtensions
    {

        #region ToJSON(this EMobilityProviderAdminStatus, Skip = null, Take = null)

        public static JObject ToJSON(this IEnumerable<EMobilityProviderAdminStatus>  EMobilityProviderAdminStatus,
                                     UInt64?                                               Skip   = null,
                                     UInt64?                                               Take   = null)
        {

            #region Initial checks

            if (EMobilityProviderAdminStatus is null || !EMobilityProviderAdminStatus.Any())
                return new JObject();

            #endregion

            #region Maybe there are duplicate EMobilityProviderAdmin identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<EMobilityProvider_Id, EMobilityProviderAdminStatus>();

            foreach (var status in EMobilityProviderAdminStatus)
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
                                                                          kvp.Value.AdminStatus.ToString())
                                                              )));

        }

        #endregion

        #region Contains(this EMobilityProviderAdminStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of e-mobility providers and their current admin status
        /// contains the given pair of e-mobility provider identification and admin status.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus">An enumeration of e-mobility providers and their current admin status.</param>
        /// <param name="Id">A e-mobility provider identification.</param>
        /// <param name="AdminStatus">A e-mobility provider admin status.</param>
        public static Boolean Contains(this IEnumerable<EMobilityProviderAdminStatus>  EMobilityProviderAdminStatus,
                                       EMobilityProvider_Id                            Id,
                                       EMobilityProviderAdminStatusTypes               AdminStatus)
        {

            foreach (var status in EMobilityProviderAdminStatus)
            {
                if (status.Id          == Id &&
                    status.AdminStatus == AdminStatus)
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

    }


    /// <summary>
    /// The current admin status of an e-mobility provider.
    /// </summary>
    public class EMobilityProviderAdminStatus : AInternalData,
                                                IEquatable<EMobilityProviderAdminStatus>,
                                                IComparable<EMobilityProviderAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id               Id             { get; }

        /// <summary>
        /// The current admin status of the e-mobility provider.
        /// </summary>
        public EMobilityProviderAdminStatusTypes  AdminStatus    { get; }

        /// <summary>
        /// The timestamp of the current admin status of the e-mobility provider.
        /// </summary>
        public DateTime                           Timestamp      { get; }

        /// <summary>
        /// The timestamped admin status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderAdminStatusTypes> TimestampedAdminStatus
            => new (Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        #region EMobilityProviderStatus(Id, AdminStatus,            CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new e-mobility provider admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="AdminStatus">The current timestamped admin status of the e-mobility provider.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EMobilityProviderAdminStatus(EMobilityProvider_Id                            Id,
                                            Timestamped<EMobilityProviderAdminStatusTypes>  AdminStatus,
                                            JObject?                                        CustomData     = null,
                                            UserDefinedDictionary?                          InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus.Value;
            this.Timestamp    = AdminStatus.Timestamp;

        }

        #endregion

        #region EMobilityProviderStatus(Id, AdminStatus, Timestamp, CustomData = null, InternalData = null)

        /// <summary>
        /// Create a new e-mobility provider admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="Status">The current admin status of the e-mobility provider.</param>
        /// <param name="Timestamp">The timestamp of the status change of the e-mobility provider.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EMobilityProviderAdminStatus(EMobilityProvider_Id               Id,
                                            EMobilityProviderAdminStatusTypes  AdminStatus,
                                            DateTime                           Timestamp,
                                            JObject?                           CustomData     = null,
                                            UserDefinedDictionary?             InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id           = Id;
            this.AdminStatus  = AdminStatus;
            this.Timestamp    = Timestamp;

        }

        #endregion

        #endregion


        #region (static) Snapshot(EMobilityProvider)

        /// <summary>
        /// Take a snapshot of the current e-mobility provider admin status.
        /// </summary>
        /// <param name="EMobilityProvider">A e-mobility provider.</param>
        public static EMobilityProviderAdminStatus Snapshot(EMobilityProvider EMobilityProvider)

            => new (EMobilityProvider.Id,
                    EMobilityProvider.AdminStatus.Value,
                    EMobilityProvider.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                           EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (EMobilityProviderAdminStatus1 is null || EMobilityProviderAdminStatus2 is null)
                return false;

            return EMobilityProviderAdminStatus1.Equals(EMobilityProviderAdminStatus2);

        }

        #endregion

        #region Operator != (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                           EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)

            => !(EMobilityProviderAdminStatus1 == EMobilityProviderAdminStatus2);

        #endregion

        #region Operator <  (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                          EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)
        {

            if (EMobilityProviderAdminStatus1 is null)
                throw new ArgumentNullException(nameof(EMobilityProviderAdminStatus1), "The given EMobilityProviderAdminStatus1 must not be null!");

            return EMobilityProviderAdminStatus1.CompareTo(EMobilityProviderAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                           EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)

            => !(EMobilityProviderAdminStatus1 > EMobilityProviderAdminStatus2);

        #endregion

        #region Operator >  (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                          EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)
        {

            if (EMobilityProviderAdminStatus1 is null)
                throw new ArgumentNullException(nameof(EMobilityProviderAdminStatus1), "The given EMobilityProviderAdminStatus1 must not be null!");

            return EMobilityProviderAdminStatus1.CompareTo(EMobilityProviderAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (EMobilityProviderAdminStatus1, EMobilityProviderAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus1">A e-mobility provider admin status.</param>
        /// <param name="EMobilityProviderAdminStatus2">Another e-mobility provider admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EMobilityProviderAdminStatus EMobilityProviderAdminStatus1,
                                           EMobilityProviderAdminStatus EMobilityProviderAdminStatus2)

            => !(EMobilityProviderAdminStatus1 < EMobilityProviderAdminStatus2);

        #endregion

        #endregion

        #region IComparable<EMobilityProviderAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMobilityProviderAdminStatus eMobilityProviderAdminStatus
                   ? CompareTo(eMobilityProviderAdminStatus)
                   : throw new ArgumentException("The given object is not an e-mobility provider admin status!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityProviderAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(EMobilityProviderAdminStatus? EMobilityProviderAdminStatus)
        {

            if (EMobilityProviderAdminStatus is null)
                throw new ArgumentNullException(nameof(EMobilityProviderAdminStatus), "The given e-mobility provider admin status must not be null!");

            var c = Id.         CompareTo(EMobilityProviderAdminStatus.Id);

            if (c == 0)
                c = AdminStatus.CompareTo(EMobilityProviderAdminStatus.AdminStatus);

            if (c == 0)
                c = Timestamp.  CompareTo(EMobilityProviderAdminStatus.Timestamp);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EMobilityProviderAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityProviderAdminStatus eMobilityProviderAdminStatus &&
                   Equals(eMobilityProviderAdminStatus);

        #endregion

        #region Equals(EMobilityProviderAdminStatus)

        /// <summary>
        /// Compares two EMobilityProvider identifications for equality.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatus">A e-mobility provider identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMobilityProviderAdminStatus? EMobilityProviderAdminStatus)

            => EMobilityProviderAdminStatus is not null                     &&
               Id.         Equals(EMobilityProviderAdminStatus.Id)          &&
               AdminStatus.Equals(EMobilityProviderAdminStatus.AdminStatus) &&
               Timestamp.  Equals(EMobilityProviderAdminStatus.Timestamp);

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

                return Id.         GetHashCode() * 5 ^
                       AdminStatus.GetHashCode() * 3 ^
                       Timestamp.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             AdminStatus,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
