﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public static class ChargingLocationExtensions
    {

        public static Boolean IsNull(this ChargingLocation? ChargingLocation)

            => ChargingLocation is null ||
               !(ChargingLocation.EVSEId.                   HasValue ||
                 ChargingLocation.ChargingStationId.        HasValue ||
                 ChargingLocation.ChargingPoolId.           HasValue ||
                 ChargingLocation.ChargingStationOperatorId.HasValue);

        public static Boolean IsDefined(this ChargingLocation? ChargingLocation)

            => ChargingLocation is not null &&
              (ChargingLocation.EVSEId.                   HasValue ||
               ChargingLocation.ChargingStationId.        HasValue ||
               ChargingLocation.ChargingPoolId.           HasValue ||
               ChargingLocation.ChargingStationOperatorId.HasValue);

    }

    public class ChargingLocation : IEquatable<ChargingLocation>,
                                    IComparable<ChargingLocation>
    {

        #region Properties

        public EVSE_Id?                     EVSEId                       { get; }

        public ChargingStation_Id?          ChargingStationId            { get; }

        public ChargingPool_Id?             ChargingPoolId               { get; }

        public ChargingStationOperator_Id?  ChargingStationOperatorId    { get; private set; }

        // EVSEGroupId
        // ChargingStationGroupId
        // ChargingPoolGroupId

        #endregion

        #region Constructor(s)

        private ChargingLocation(EVSE_Id?                     EVSEId                      = null,
                                 ChargingStation_Id?          ChargingStationId           = null,
                                 ChargingPool_Id?             ChargingPoolId              = null,
                                 ChargingStationOperator_Id?  ChargingStationOperatorId   = null)
        {

            this.EVSEId                     = EVSEId;
            this.ChargingStationId          = ChargingStationId;
            this.ChargingPoolId             = ChargingPoolId;
            this.ChargingStationOperatorId  = ChargingStationOperatorId ?? ChargingPoolId?.OperatorId ?? ChargingStationId?.OperatorId ?? EVSEId?.OperatorId;

        }

        #endregion


        #region FromEVSEId                   (EVSEId)

        public static ChargingLocation FromEVSEId(EVSE_Id EVSEId)

            => new (EVSEId: EVSEId);

        public static ChargingLocation? FromEVSEId(EVSE_Id? EVSEId)

            => EVSEId.HasValue
                   ? new ChargingLocation(EVSEId: EVSEId)
                   : null;

        //public static ChargingLocation? ParseEVSEId(String Text)
        //{

        //    if (EVSE_Id.TryParse(Text, out var evseId))
        //        return new ChargingLocation(EVSEId: evseId);

        //    return null;

        //}

        //public static Boolean TryParseEVSEId(String Text, out ChargingLocation? ChargingLocation)
        //{

        //    if (EVSE_Id.TryParse(Text, out var evseId))
        //    {
        //        ChargingLocation = new ChargingLocation(EVSEId: evseId);
        //        return true;
        //    }

        //    ChargingLocation = null;
        //    return false;

        //}

        #endregion

        #region FromChargingStationId        (ChargingStationId)

        public static ChargingLocation FromChargingStationId(ChargingStation_Id ChargingStationId)

            => new (ChargingStationId: ChargingStationId);

        public static ChargingLocation? FromChargingStationId(ChargingStation_Id? ChargingStationId)

            => ChargingStationId.HasValue
                   ? new ChargingLocation(ChargingStationId: ChargingStationId)
                   : null;

        #endregion

        #region FromChargingPoolId           (ChargingPoolId)

        public static ChargingLocation FromChargingPoolId(ChargingPool_Id ChargingPoolId)

            => new (ChargingPoolId: ChargingPoolId);

        public static ChargingLocation? FromChargingPoolId(ChargingPool_Id? ChargingPoolId)

            => ChargingPoolId.HasValue
                   ? new ChargingLocation(ChargingPoolId: ChargingPoolId)
                   : null;

        #endregion

        #region FromChargingStationOperatorId(ChargingStationOperatorId)

        public static ChargingLocation FromChargingStationOperatorId(ChargingStationOperator_Id ChargingStationOperatorId)

            => new (ChargingStationOperatorId: ChargingStationOperatorId);

        public static ChargingLocation? FromChargingStationOperatorId(ChargingStationOperator_Id? ChargingStationOperatorId)

            => ChargingStationOperatorId.HasValue
                   ? new ChargingLocation(ChargingStationOperatorId: ChargingStationOperatorId)
                   : null;

        #endregion



        public ChargingLocation SetChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId)
        {
            this.ChargingStationOperatorId = ChargingStationOperatorId;
            return this;
        }



        public JObject ToJSON()

            => JSONObject.Create(

                   EVSEId.HasValue
                       ? new JProperty("EVSEId",                      EVSEId.                   Value.ToString())
                       : null,

                   ChargingStationId.HasValue
                       ? new JProperty("chargingStationId",           ChargingStationId.        Value.ToString())
                       : null,

                   ChargingPoolId.HasValue
                       ? new JProperty("chargingPoolId",              ChargingPoolId.           Value.ToString())
                       : null,

                   ChargingStationOperatorId.HasValue
                       ? new JProperty("chargingStationOperatorId",   ChargingStationOperatorId.Value.ToString())
                       : null

               );


        public static Boolean TryParse(JObject                JSON,
                                       out ChargingLocation?  ChargingLocation,
                                       out String?            ErrorResponse)
        {

            ChargingLocation  = null;
            ErrorResponse     = null;
            return false;

        }



        #region Operator overloading

        #region Operator == (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingLocation ChargingLocation1,
                                           ChargingLocation ChargingLocation2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingLocation1, ChargingLocation2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingLocation1 is null || ChargingLocation2 is null)
                return false;

            return ChargingLocation1.Equals(ChargingLocation2);

        }

        #endregion

        #region Operator != (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingLocation ChargingLocation1,
                                           ChargingLocation ChargingLocation2)
        {
            return !(ChargingLocation1 == ChargingLocation2);
        }

        #endregion

        #region Operator <  (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingLocation ChargingLocation1,
                                          ChargingLocation ChargingLocation2)
        {

            if (ChargingLocation1 is null)
                throw new ArgumentNullException("The given ChargingLocation1 must not be null!");

            return ChargingLocation1.CompareTo(ChargingLocation2) < 0;

        }

        #endregion

        #region Operator <= (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingLocation ChargingLocation1,
                                           ChargingLocation ChargingLocation2)
        {
            return !(ChargingLocation1 > ChargingLocation2);
        }

        #endregion

        #region Operator >  (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingLocation ChargingLocation1,
                                          ChargingLocation ChargingLocation2)
        {

            if (ChargingLocation1 is null)
                throw new ArgumentNullException("The given ChargingLocation1 must not be null!");

            return ChargingLocation1.CompareTo(ChargingLocation2) > 0;

        }

        #endregion

        #region Operator >= (ChargingLocation1, ChargingLocation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation1">A charging location.</param>
        /// <param name="ChargingLocation2">Another charging location.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingLocation ChargingLocation1,
                                           ChargingLocation ChargingLocation2)
        {
            return !(ChargingLocation1 < ChargingLocation2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingLocation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is not ChargingLocation ChargingLocation)
                throw new ArgumentException("The given object is not a charging location!");

            return CompareTo(ChargingLocation);

        }

        #endregion

        #region CompareTo(ChargingLocation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingLocation">An object to compare with.</param>
        public Int32 CompareTo(ChargingLocation? ChargingLocation)
        {

            if (ChargingLocation is null)
                throw new ArgumentNullException(nameof(ChargingLocation),  "The given charging location must not be null!");

            if (EVSEId.HasValue && ChargingLocation.EVSEId.HasValue)
                return EVSEId.Value.CompareTo(ChargingLocation.EVSEId.Value);

            if (ChargingStationId.HasValue && ChargingLocation.ChargingStationId.HasValue)
                return ChargingStationId.Value.CompareTo(ChargingLocation.ChargingStationId.Value);

            if (ChargingPoolId.HasValue && ChargingLocation.ChargingPoolId.HasValue)
                return ChargingPoolId.Value.CompareTo(ChargingLocation.ChargingPoolId.Value);

            if (ChargingStationOperatorId.HasValue && ChargingLocation.ChargingStationOperatorId.HasValue)
                return ChargingStationOperatorId.Value.CompareTo(ChargingLocation.ChargingStationOperatorId.Value);

            return String.Compare(ToString(), ChargingLocation.ToString(), StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingLocation> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)
        {

            if (Object is not ChargingLocation ChargingLocation)
                return false;

            return Equals(ChargingLocation);

        }

        #endregion

        #region Equals(ChargingLocation)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="ChargingLocation">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingLocation? ChargingLocation)
        {

            if (ChargingLocation is null)
                return false;

            if (EVSEId.HasValue && ChargingLocation.EVSEId.HasValue)
                return EVSEId.Value.Equals(ChargingLocation.EVSEId.Value);

            if (ChargingStationId.HasValue && ChargingLocation.ChargingStationId.HasValue)
                return ChargingStationId.Value.Equals(ChargingLocation.ChargingStationId.Value);

            if (ChargingPoolId.HasValue && ChargingLocation.ChargingPoolId.HasValue)
                return ChargingPoolId.Value.Equals(ChargingLocation.ChargingPoolId.Value);

            if (ChargingStationOperatorId.HasValue && ChargingLocation.ChargingStationOperatorId.HasValue)
                return ChargingStationOperatorId.Value.Equals(ChargingLocation.ChargingStationOperatorId.Value);

            return false;

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ToString().GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => (new String[] {
                    EVSEId?.                   ToString() ?? String.Empty,
                    ChargingStationId?.        ToString() ?? String.Empty,
                    ChargingPoolId?.           ToString() ?? String.Empty,
                    ChargingStationOperatorId?.ToString() ?? String.Empty
                }).
                Where(_ => _.IsNotNullOrEmpty()).
                AggregateWith(", ");

        #endregion

    }

}
