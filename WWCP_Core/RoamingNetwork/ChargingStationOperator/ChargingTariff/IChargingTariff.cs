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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public interface IChargingTariff : IEquatable<ChargingTariff>,
                                       IComparable<ChargingTariff>,
                                       IComparable
    {

        ChargingTariff_Id                   Id                { get; }
        I18NString                          Name              { get; }
        I18NString                          Description       { get; }
        Brand?                              Brand             { get; }
        Currency                            Currency          { get; }
        EnergyMix?                          EnergyMix         { get; }
        IEnumerable<ChargingTariffElement>  TariffElements    { get; }
        URL?                                TariffURL         { get; }

        String?                             DataSource        { get; }
        DateTime                            LastChange        { get; }
        JObject?                            CustomData        { get; }
        UserDefinedDictionary?              InternalData      { get; }

        JObject ToJSON(Boolean    Embedded                         = false,
                       InfoStatus ExpandRoamingNetworkId           = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandChargingStationOperatorId  = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandChargingPoolId             = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandEVSEIds                    = InfoStatus.Expanded,
                       InfoStatus ExpandBrandIds                   = InfoStatus.ShowIdOnly,
                       InfoStatus ExpandDataLicenses               = InfoStatus.ShowIdOnly);

    }

}
