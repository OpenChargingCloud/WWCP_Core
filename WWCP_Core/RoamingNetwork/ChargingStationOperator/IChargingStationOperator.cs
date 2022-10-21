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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Charging station extentions.
    /// </summary>
    public static partial class ChargingStationOperatorExtensions
    {

        #region ToJSON(this ChargingStationOperators, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging station operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JArray ToJSON(this IEnumerable<IChargingStationOperator>                 ChargingStationOperators,
                                    UInt64?                                                    Skip                                      = null,
                                    UInt64?                                                    Take                                      = null,
                                    Boolean                                                    Embedded                                  = false,
                                    InfoStatus                                                 ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                                 ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<ChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                                    CustomJObjectSerializerDelegate<ChargingPool>?             CustomChargingPoolSerializer              = null,
                                    CustomJObjectSerializerDelegate<ChargingStation>?          CustomChargingStationSerializer           = null,
                                    CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null)


            => ChargingStationOperators?.Any() == true

                   ? new JArray(ChargingStationOperators.
                                    Where         (cso => cso is not null).
                                    OrderBy       (cso => cso.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (cso => cso.ToJSON(Embedded,
                                                                     ExpandRoamingNetworkId,
                                                                     ExpandChargingPoolIds,
                                                                     ExpandChargingStationIds,
                                                                     ExpandEVSEIds,
                                                                     ExpandBrandIds,
                                                                     ExpandDataLicenses,
                                                                     CustomChargingStationOperatorSerializer,
                                                                     CustomChargingPoolSerializer,
                                                                     CustomChargingStationSerializer,
                                                                     CustomEVSESerializer)).
                                    Where         (cso => cso is not null))

                   : new JArray();

        #endregion

    }


    public interface IChargingStationOperator : IAdminStatus<ChargingStationOperatorAdminStatusTypes>,
                                                IStatus<ChargingStationOperatorStatusTypes>,
                                                IReserveRemoteStartStop,
                                                IEnumerable<ChargingPool>,
                                                IEquatable<ChargingStationOperator>,
                                                IComparable<ChargingStationOperator>,
                                                IComparable
    {

        /// <summary>
        /// The unique charging station operator identification.
        /// </summary>
        ChargingStationOperator_Id      Id                               { get; }

        I18NString                      Name                             { get; }
        I18NString                      Description                      { get; }
        ReactiveSet<DataLicense>        DataLicenses                     { get; }
        String?                         DataSource                       { get; }


        IRoamingNetwork                 RoamingNetwork                   { get; }

        IRemoteChargingStationOperator  RemoteChargingStationOperator    { get; }


        /// <summary>
        /// Return a JSON representation for the given charging station operator.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        JObject ToJSON(Boolean                                                    Embedded                                  = false,
                       InfoStatus                                                 ExpandRoamingNetworkId                    = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandChargingPoolIds                     = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandChargingStationIds                  = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandEVSEIds                             = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandBrandIds                            = InfoStatus.ShowIdOnly,
                       InfoStatus                                                 ExpandDataLicenses                        = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<ChargingStationOperator>?  CustomChargingStationOperatorSerializer   = null,
                       CustomJObjectSerializerDelegate<ChargingPool>?             CustomChargingPoolSerializer              = null,
                       CustomJObjectSerializerDelegate<ChargingStation>?          CustomChargingStationSerializer           = null,
                       CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSESerializer                      = null);


    }

}
