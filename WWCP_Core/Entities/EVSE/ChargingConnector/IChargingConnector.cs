/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public static class ChargingConnectorExtensions
    {

        #region ToJSON(this ChargingConnectors, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Serialize an enumeration of charging connectors to a JSON array.
        /// </summary>
        /// <param name="ChargingConnectors">The enumeration of charging connectors.</param>
        /// <param name="Skip">An optional number of charging connectors to skip.</param>
        /// <param name="Take">An optional number of charging connectors to take.</param>
        /// <param name="Embedded">Whether the charging connectors should be embedded into another JSON structure or not.</param>
        /// <param name="CustomChargingConnectorSerializer">An optional delegate to serialize custom charging connector JSON objects.</param>
        public static JArray ToJSON(this IEnumerable<IChargingConnector>                 ChargingConnectors,
                                    UInt64?                                              Skip                                = null,
                                    UInt64?                                              Take                                = null,
                                    Boolean                                              Embedded                            = false,
                                    CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)

            => ChargingConnectors is not null && ChargingConnectors.Any()

                   ? new JArray(
                         ChargingConnectors.
                             Where          (chargingConnector => chargingConnector is not null).
                             OrderBy        (chargingConnector => chargingConnector.Id).
                             SkipTakeFilter (Skip, Take).
                             Select         (chargingConnector => chargingConnector.ToJSON(Embedded,
                                                                                           //ExpandRoamingNetworkId,
                                                                                           //ExpandChargingStationOperatorId,
                                                                                           //ExpandChargingPoolId,
                                                                                           //ExpandEVSEIds,
                                                                                           //ExpandBrandIds,
                                                                                           //ExpandDataLicenses,
                                                                                           //CustomChargingStationSerializer,
                                                                                           //CustomEVSESerializer,
                                                                                           CustomChargingConnectorSerializer)).
                             Where          (chargingConnector => chargingConnector is not null)
                     )

                   : [];

        #endregion

    }


    public interface IChargingConnector
    {

        /// <summary>
        /// The parent EVSE of this charging connector.
        /// </summary>
        [InternalUseOnly]
        IEVSE?                          EVSE                  { get; set; }

        /// <summary>
        /// The optional charging connector identification.
        /// </summary>
        ChargingConnector_Id            Id                    { get; }

        /// <summary>
        /// The type of the charging connector.
        /// </summary>
        ChargingConnectorType           Type                  { get; }

        //Boolean                         IsDC                  { get; }

        /// <summary>
        /// Whether the charging connector is lockable or not.
        /// </summary>
        Boolean?                        Lockable              { get; }

        /// <summary>
        /// The optional charging cable attached.
        /// </summary>
        ChargingCable?                  ChargingCable         { get; }

        /// <summary>
        /// Optional tariff identifications that can be used with this charging connector.
        /// </summary>
        IEnumerable<ChargingTariff_Id>  TariffIds             { get; }

        /// <summary>
        /// URL to the operator’s terms and conditions.
        /// </summary>
        /// <remarks>Ask OCPI why this is here!</remarks>
        [Optional]
        URL?                            TermsAndConditions    { get; }


        JObject? ToJSON(Boolean                                              Embedded                            = false,
                        CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null,
                        CustomJObjectSerializerDelegate<ChargingCable>?      CustomChargingCableSerializer       = null);

    }

}
