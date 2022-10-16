/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the commom charging station interface.
    /// </summary>
    public static class IChargingStationExtensions
    {

        #region ToJSON(this ChargingStations, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<IChargingStation>                 ChargingStations,
                                    UInt64?                                            Skip                              = null,
                                    UInt64?                                            Take                              = null,
                                    Boolean                                            Embedded                          = false,
                                    InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Expanded,
                                    InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<ChargingStation>?  CustomChargingStationSerializer   = null,
                                    CustomJObjectSerializerDelegate<EVSE>?             CustomEVSESerializer              = null)


            => ChargingStations is not null && ChargingStations.Any()

                   ? new JArray(ChargingStations.
                                    Where         (station => station is not null).
                                    OrderBy       (station => station.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (station => station.ToJSON(Embedded,
                                                                             ExpandRoamingNetworkId,
                                                                             ExpandChargingStationOperatorId,
                                                                             ExpandChargingPoolId,
                                                                             ExpandEVSEIds,
                                                                             ExpandBrandIds,
                                                                             ExpandDataLicenses,
                                                                             CustomChargingStationSerializer,
                                                                             CustomEVSESerializer)).
                                    Where         (station => station is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The commom charging station interface.
    /// </summary>
    public interface IChargingStation : IEntity<ChargingStation_Id>,
                                        IAdminStatus<ChargingStationAdminStatusTypes>,
                                        IStatus<ChargingStationStatusTypes>,
                                        IEquatable<IChargingStation>, IComparable<IChargingStation>, IComparable,
                                        IEnumerable<EVSE>
    {


        /// <summary>
        /// The roaming network of this charging Station.
        /// </summary>
        IRoamingNetwork?          RoamingNetwork           { get; }

        /// <summary>
        /// The charging station operator of this charging Station.
        /// </summary>
        [Optional]
        ChargingStationOperator?  Operator                 { get; }

        /// <summary>
        /// The remote charging Station.
        /// </summary>
        [Optional]
        IRemoteChargingStation?   RemoteChargingStation    { get; }


        /// <summary>
        /// Return a JSON representation of the given charging station.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        JObject ToJSON(Boolean                                            Embedded                          = false,
                       InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                       InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                       InfoStatus                                         ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                       InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Expanded,
                       InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                       InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<ChargingStation>?  CustomChargingStationSerializer   = null,
                       CustomJObjectSerializerDelegate<EVSE>?             CustomEVSESerializer              = null);


    }

}
