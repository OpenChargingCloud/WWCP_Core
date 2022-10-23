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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the commom charging pool interface.
    /// </summary>
    public static partial class ChargingPoolExtensions
    {

        #region ToJSON(this ChargingPools, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<IChargingPool>                   ChargingPools,
                                    UInt64?                                           Skip                              = null,
                                    UInt64?                                           Take                              = null,
                                    Boolean                                           Embedded                          = false,
                                    InfoStatus                                        ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandChargingStationIds          = InfoStatus.Expanded,
                                    InfoStatus                                        ExpandEVSEIds                     = InfoStatus.Hidden,
                                    InfoStatus                                        ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                        ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<ChargingPool>     CustomChargingPoolSerializer      = null,
                                    CustomJObjectSerializerDelegate<ChargingStation>  CustomChargingStationSerializer   = null,
                                    CustomJObjectSerializerDelegate<EVSE>             CustomEVSESerializer              = null)


            => ChargingPools is not null && ChargingPools.Any()

                   ? new JArray(ChargingPools.
                                    Where         (pool => pool is not null).
                                    OrderBy       (pool => pool.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (pool => pool.ToJSON(Embedded,
                                                                       ExpandRoamingNetworkId,
                                                                       ExpandChargingStationOperatorId,
                                                                       ExpandChargingStationIds,
                                                                       ExpandEVSEIds,
                                                                       ExpandBrandIds,
                                                                       ExpandDataLicenses,
                                                                       CustomChargingPoolSerializer,
                                                                       CustomChargingStationSerializer,
                                                                       CustomEVSESerializer)).
                                    Where         (pool => pool is not null))

                   : new JArray();


        #endregion

    }


    /// <summary>
    /// The commom charging pool interface.
    /// </summary>
    public interface IChargingPool : IEntity<ChargingPool_Id>,
                                     IAdminStatus<ChargingPoolAdminStatusTypes>,
                                     IStatus<ChargingPoolStatusTypes>,
                                     ILocalReserveRemoteStartStop,
                                     IEquatable<IChargingPool>, IComparable<IChargingPool>, IComparable,
                                     IEnumerable<ChargingStation>

    {

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        IRoamingNetwork?          RoamingNetwork           { get; }

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        ChargingStationOperator?  Operator                 { get; }

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        IRemoteChargingPool?      RemoteChargingPool       { get; }



        Address?                  Address                  { get; }

        GeoCoordinate?            GeoLocation              { get; }

        OpeningTimes              OpeningTimes             { get; }

        IEnumerable<IEVSE>        EVSEs                    { get; }



        /// <summary>
        /// Return a JSON representation of the given charging pool.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean                                            Embedded                          = false,
                              InfoStatus                                         ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandChargingStationIds          = InfoStatus.Expanded,
                              InfoStatus                                         ExpandEVSEIds                     = InfoStatus.Hidden,
                              InfoStatus                                         ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                         ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<ChargingPool>?     CustomChargingPoolSerializer      = null,
                              CustomJObjectSerializerDelegate<ChargingStation>?  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<EVSE>?             CustomEVSESerializer              = null);


    }

}
