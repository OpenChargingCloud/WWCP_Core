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

    public static class ChargingConnectorExtetions
    {

        #region ToJSON(this ChargingConnectors, IncludeParentIds = true)

        public static JArray ToJSON(this IEnumerable<IChargingConnector>                 ChargingConnectors,
                                    Boolean                                              Embedded                            = false,
                                    CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)

            => ChargingConnectors is not null && ChargingConnectors.Any()
                   ? new JArray(ChargingConnectors.SafeSelect(chargingConnector => chargingConnector.ToJSON(Embedded,
                                                                                                            CustomChargingConnectorSerializer)))
                   : new JArray();

        #endregion

    }


    /// <summary>
    /// A charging connector to connect an electric vehicle (EV)
    /// to an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public class ChargingConnector : IEquatable<ChargingConnector>,
                                     IChargingConnector
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String  JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/ChargingConnector";

        #endregion

        #region Properties

        /// <summary>
        /// The parent EVSE of this charging connector.
        /// </summary>
        [InternalUseOnly]
        public IEVSE?                EVSE       { get; set; }

        /// <summary>
        /// The optional charging connector identification.
        /// </summary>
        [Mandatory]
        public ChargingConnector_Id  Id               { get; }

        /// <summary>
        /// The type of the charging plug.
        /// </summary>
        [Mandatory]
        public ChargingPlugTypes     Plug             { get; }

        /// <summary>
        /// Whether the charging plug is lockable or not.
        /// </summary>
        [Optional]
        public Boolean?              Lockable         { get; }

        /// <summary>
        /// Whether the charging plug has an attached cable or not.
        /// </summary>
        [Optional]
        public Boolean?              CableAttached    { get; }

        /// <summary>
        /// The length of the charging cable.
        /// </summary>
        [Optional]
        public Meter?                CableLength      { get; }

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
        /// <param name="Id">A charging connector identification.</param>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable.</param>
        public ChargingConnector(ChargingConnector_Id  Id,
                                 ChargingPlugTypes     Plug,
                                 Boolean?              Lockable        = null,
                                 Boolean?              CableAttached   = null,
                                 Meter?                CableLength     = null)
        {

            this.Id             = Id;
            this.Plug           = Plug;
            this.Lockable       = Lockable;
            this.CableAttached  = CableAttached;
            this.CableLength    = CableLength;

        }

        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Id">A charging connector identification.</param>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable.</param>
        public ChargingConnector(IEVSE?                ParentEVSE,
                                 ChargingConnector_Id  Id,
                                 ChargingPlugTypes     Plug,
                                 Boolean?              Lockable        = null,
                                 Boolean?              CableAttached   = null,
                                 Meter?                CableLength     = null)
        {

            this.EVSE     = ParentEVSE;
            this.Id             = Id;
            this.Plug           = Plug;
            this.Lockable       = Lockable;
            this.CableAttached  = CableAttached;
            this.CableLength    = CableLength;

        }

        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable.</param>
        public ChargingConnector(ChargingPlugTypes  Plug,
                                 Boolean?           Lockable        = null,
                                 Boolean?           CableAttached   = null,
                                 Meter?             CableLength     = null)
        {

            this.Id             = ChargingConnector_Id.Parse(1);
            this.Plug           = Plug;
            this.Lockable       = Lockable;
            this.CableAttached  = CableAttached;
            this.CableLength    = CableLength;

        }

        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Plug">The type of the charging plug.</param>
        /// <param name="Lockable">Whether the charging plug is lockable or not.</param>
        /// <param name="CableAttached">The type of the charging cable.</param>
        /// <param name="CableLength">The length of the charging cable.</param>
        public ChargingConnector(IEVSE?             ParentEVSE,
                                 ChargingPlugTypes  Plug,
                                 Boolean?           Lockable        = null,
                                 Boolean?           CableAttached   = null,
                                 Meter?             CableLength     = null)
        {

            this.EVSE     = ParentEVSE;
            this.Id             = ChargingConnector_Id.Parse(1);
            this.Plug           = Plug;
            this.Lockable       = Lockable;
            this.CableAttached  = CableAttached;
            this.CableLength    = CableLength;

        }

        #endregion


        #region ToJSON(this ChargingConnector, Embedded = false, CustomChargingConnectorSerializer = null)

        public JObject? ToJSON(Boolean                                              Embedded                            = false,
                               CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)
        {

            try
            {

                var json = JSONObject.Create(

                                 new JProperty("@id",             Id.ToString()),

                           !Embedded
                               ? new JProperty("@context",        JSONLDContext)
                               : null,

                                 new JProperty("plug",            Plug.         ToString()),

                           CableAttached.HasValue
                               ? new JProperty("cableAttached",   CableAttached.ToString())
                               : null,

                           CableLength.HasValue
                               ? new JProperty("cableLength",     CableLength.Value)
                               : null);

                return CustomChargingConnectorSerializer is not null
                           ? CustomChargingConnectorSerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                DebugX.LogException(e, "ChargingConnector.ToJSON(...)");
            }

            return null;

        }

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

               Id.  Equals(ChargingConnector.Id)   &&
               Plug.Equals(ChargingConnector.Plug) &&

             ((!Lockable.     HasValue && !ChargingConnector.Lockable.     HasValue) ||
               (Lockable.     HasValue &&  ChargingConnector.Lockable.     HasValue && Lockable.     Equals(ChargingConnector.Lockable)))      &&

             ((!CableAttached.HasValue && !ChargingConnector.CableAttached.HasValue) ||
               (CableAttached.HasValue &&  ChargingConnector.CableAttached.HasValue && CableAttached.Equals(ChargingConnector.CableAttached))) &&

             ((!CableLength.  HasValue && !ChargingConnector.CableLength.  HasValue) ||
               (CableLength.  HasValue &&  ChargingConnector.CableLength.  HasValue && CableLength.  Equals(ChargingConnector.CableLength)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.            GetHashCode()       * 11 ^
                       Plug.          GetHashCode()       *  7 ^
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
                   Id, ": ",
                   Plug,
                   Lockable.     HasValue ? ", lockable " : "",
                   CableAttached.HasValue ? ", with cable " : "",
                   CableLength.  HasValue ? ", " + CableLength + "cm" : ""
               );

        #endregion

    }

}
