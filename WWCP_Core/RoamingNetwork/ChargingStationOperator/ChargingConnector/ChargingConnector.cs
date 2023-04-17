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

    public static class ChargingConnectorExtetions
    {

        #region ToJSON(this ChargingConnectors, IncludeParentIds = true)

        public static JArray ToJSON(this IEnumerable<ChargingConnector>  ChargingConnectors,
                                    Boolean                              IncludeParentIds = true)
        {

            #region Initial checks

            if (ChargingConnectors == null)
                return new JArray();

            #endregion

            return ChargingConnectors != null && ChargingConnectors.Any()
                       ? new JArray(ChargingConnectors.SafeSelect(chargingConnector => chargingConnector.ToJSON(IncludeParentIds)))
                       : new JArray();

        }

        #endregion

    }


    /// <summary>
    /// A charging connector to connect an electric vehicle (EV)
    /// to an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public class ChargingConnector : IEquatable<ChargingConnector>,
                                     IChargingConnector
    {

        #region Properties

        /// <summary>
        /// The type of the charging plug.
        /// </summary>
        [Mandatory]
        public ChargingPlugTypes              Plug             { get; }

        /// <summary>
        /// The optional charging connector identification.
        /// </summary>
        public ChargingConnector_Id?  Id               { get; }

        /// <summary>
        /// Whether the charging plug is lockable or not.
        /// </summary>
        [Optional]
        public Boolean?               Lockable         { get; }

        /// <summary>
        /// Whether the charging plug has an attached cable or not.
        /// </summary>
        [Optional]
        public Boolean?               CableAttached    { get; }

        /// <summary>
        /// The length of the charging cable [cm].
        /// </summary>
        [Optional]
        public Double?                CableLength      { get; }

        /// <summary>
        /// Whether the charging connector is DC or AC.
        /// </summary>
        [Mandatory]
        public Boolean IsDC
            => Plug.IsDC();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Id">An optional charging connector identification.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable [mm].</param>
        public ChargingConnector(ChargingPlugTypes Plug,
                            ChargingConnector_Id? Id = null,
                            Boolean? Lockable = null,
                            Boolean? CableAttached = null,
                            Double? CableLength = null)
        {

            this.Plug = Plug;
            this.Id = Id;
            this.Lockable = Lockable;
            this.CableAttached = CableAttached;
            this.CableLength = CableLength;

        }

        #endregion


        #region ToJSON(this ChargingConnector,  IncludeParentIds = true)

        public JObject ToJSON(Boolean IncludeParentIds = true)

            => JSONObject.Create(

                         new JProperty("Plug",            Plug.         ToString()),

                   CableAttached.HasValue
                       ? new JProperty("cableAttached",   CableAttached.ToString())
                       : null,

                   CableLength > 0
                       ? new JProperty("cableLength",     CableLength.  ToString())
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingConnector1, ChargingConnector2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnector1">A charging connector.</param>
        /// <param name="ChargingConnector2">Another charging connector.</param>
        /// <returns>true|false</returns>
        public static Boolean operator ==(ChargingConnector ChargingConnector1,
                                          ChargingConnector ChargingConnector2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingConnector1, ChargingConnector2))
                return true;

            // If one is null, but not both, return false.
            if (((Object)ChargingConnector1 == null) || ((Object)ChargingConnector2 == null))
                return false;

            return ChargingConnector1.Equals(ChargingConnector2);

        }

        #endregion

        #region Operator != (ChargingConnector1, ChargingConnector2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnector1">A charging connector.</param>
        /// <param name="ChargingConnector2">Another charging connector.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingConnector ChargingConnector1,
                                           ChargingConnector ChargingConnector2)

            => !(ChargingConnector1 == ChargingConnector2);

        #endregion

        #endregion

        #region IEquatable<ChargingConnector> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="Object">A charging connector to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingConnector chargingConnector &&
                   Equals(chargingConnector);

        #endregion

        #region Equals(ChargingConnector)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="ChargingConnector">A charging connector to compare with.</param>
        public Boolean Equals(ChargingConnector? ChargingConnector)

            => ChargingConnector is not null &&

               Plug.Equals(ChargingConnector.Plug) &&

             ((!Id.           HasValue && !ChargingConnector.Id.           HasValue) ||
               (Id.           HasValue &&  ChargingConnector.Id.           HasValue && Id.           Equals(ChargingConnector.Id)))            &&

             ((!Lockable.     HasValue && !ChargingConnector.Lockable.     HasValue) ||
               (Lockable.     HasValue &&  ChargingConnector.Lockable.     HasValue && Lockable.     Equals(ChargingConnector.Lockable)))      &&

             ((!CableAttached.HasValue && !ChargingConnector.CableAttached.HasValue) ||
               (CableAttached.HasValue &&  ChargingConnector.CableAttached.HasValue && CableAttached.Equals(ChargingConnector.CableAttached))) &&

             ((!CableLength.  HasValue && !ChargingConnector.CableLength.  HasValue) ||
               (CableLength.  HasValue &&  ChargingConnector.CableLength.  HasValue && CableLength.  Equals(ChargingConnector.CableLength)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Plug.          GetHashCode()       * 11 ^
                      (Id?.           GetHashCode() ?? 0) *  7 ^
                      (Lockable?.     GetHashCode() ?? 0) *  5 ^
                      (CableAttached?.GetHashCode() ?? 0) *  3 ^
                      (CableLength?.  GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(
                   Plug,
                   Id.           HasValue ? "(" + Id.ToString() + ") " : "",
                   Lockable.     HasValue ? ", lockable " : "",
                   CableAttached.HasValue ? ", with cable " : "",
                   CableLength.  HasValue ? ", " + CableLength + "cm" : ""
               );

        #endregion

    }

}
