/*
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

    public static class ChargingConnectorExtensions
    {

        #region ToJSON(this ChargingConnectors, IncludeParentIds = true)

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
                             SafeSelect     (chargingConnector => chargingConnector.ToJSON(Embedded,
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
        /// The EVSE of this charging connector.
        /// </summary>
        [InternalUseOnly]
        IEVSE?                          EVSE             { get; set; }

        ChargingConnector_Id            Id               { get; }
        Boolean                         CableAttached    { get; }
        Meter?                          CableLength      { get; }
        Boolean?                        Lockable         { get; }
        ChargingPlugTypes               Plug             { get; }
        Boolean                         IsDC             { get; }

        IEnumerable<ChargingTariff_Id>  Tariffs          { get; }

        JObject? ToJSON(Boolean                                              Embedded                            = false,
                        CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null);

    }

}
