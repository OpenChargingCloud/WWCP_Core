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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering energy meters.
    /// </summary>
    /// <param name="EnergyMeter">An energy meter to include.</param>
    public delegate Boolean IncludeEnergyMeterDelegate(IEnergyMeter EnergyMeter);


    /// <summary>
    /// Extension methods for the common energy meter interface.
    /// </summary>
    public static class IEnergyMeterExtensions
    {

        #region ToJSON(this EnergyMeters, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EnergyMeters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of smart energy meters.</param>
        /// <param name="Skip">The optional number of smart energy meters to skip.</param>
        /// <param name="Take">The optional number of smart energy meters to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into an EVSE.</param>
        public static JArray ToJSON(this IEnumerable<IEnergyMeter>                                EnergyMeters,
                                    UInt64?                                                       Skip                                         = null,
                                    UInt64?                                                       Take                                         = null,
                                    Boolean                                                       Embedded                                     = false,
                                    CustomJObjectSerializerDelegate<IEnergyMeter>?                CustomEnergyMeterSerializer                  = null,
                                    CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                                    CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null)


            => EnergyMeters?.Any() == true

                   ? new JArray(EnergyMeters.Where         (energyMeter => energyMeter is not null).
                                             OrderBy       (energyMeter => energyMeter.Id).
                                             SkipTakeFilter(Skip, Take).
                                             SafeSelect    (energyMeter => energyMeter.ToJSON(Embedded,
                                                                                              CustomEnergyMeterSerializer,
                                                                                              CustomTransparencySoftwareStatusSerializer,
                                                                                              CustomTransparencySoftwareSerializer)).
                                             Where         (energyMeter => energyMeter is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The common interface of all energy meters.
    /// </summary>
    public interface IEnergyMeter : IEntity<EnergyMeter_Id>,
                                    IAdminStatus<EnergyMeterAdminStatusTypes>,
                                    IStatus<EnergyMeterStatusTypes>,
                                    IEquatable<IEnergyMeter>,
                                    IComparable<IEnergyMeter>,
                                    IComparable
    {

        #region Properties

        String?                                  Manufacturer                 { get; }
        URL?                                     ManufacturerURL              { get; }
        String?                                  Model                        { get; }
        URL?                                     ModelURL                     { get; }
        String?                                  SerialNumber                 { get; }
        String?                                  FirmwareVersion              { get; }
        String?                                  HardwareVersion              { get; }
        IEnumerable<PublicKey>                   PublicKeys                   { get; }
        CertificateChain?                        PublicKeyCertificateChain    { get; }
        IEnumerable<TransparencySoftwareStatus>  TransparencySoftware        { get; }

        #endregion

        #region Events

        #endregion


        JObject ToJSON(Boolean                                                       Embedded                                     = false,
                       CustomJObjectSerializerDelegate<IEnergyMeter>?                CustomEnergyMeterSerializer                  = null,
                       CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                       CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null);


    }

}
