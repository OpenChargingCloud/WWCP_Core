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

using System.Collections.Concurrent;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering EVSEs.
    /// </summary>
    /// <param name="EVSE">An EVSE to include.</param>
    public delegate Boolean IncludeEVSEDelegate(IEVSE EVSE);


    /// <summary>
    /// Extension methods for the common Electric Vehicle Supply Equipments (EVSEs) interface.
    /// </summary>
    public static class EVSEExtensions
    {

        #region ToJSON(this EVSEs, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Skip">The optional number of EVSEs to skip.</param>
        /// <param name="Take">The optional number of EVSEs to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public static JArray ToJSON(this IEnumerable<IEVSE>                 EVSEs,
                                    UInt64?                                 Skip                              = null,
                                    UInt64?                                 Take                              = null,
                                    Boolean                                 Embedded                          = false,
                                    InfoStatus                              ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<EVSE>?  CustomEVSESerializer              = null)


            => EVSEs is not null && EVSEs.Any()

                   ? new JArray(EVSEs.Where         (evse => evse is not null).
                                      OrderBy       (evse => evse.Id).
                                      SkipTakeFilter(Skip, Take).
                                      SafeSelect    (evse => evse.ToJSON(Embedded,
                                                                         ExpandRoamingNetworkId,
                                                                         ExpandChargingStationOperatorId,
                                                                         ExpandChargingPoolId,
                                                                         ExpandChargingStationId,
                                                                         ExpandBrandIds,
                                                                         ExpandDataLicenses,
                                                                         CustomEVSESerializer)).
                                      Where         (json => json is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The commom Electric Vehicle Supply Equipments (EVSEs) interface.
    /// </summary>
    public interface IEVSE : IEntity<EVSE_Id>,
                             IAdminStatus<EVSEAdminStatusTypes>,
                             IStatus<EVSEStatusTypes>,
                             ILocalReserveRemoteStartStop,
                             IEquatable<IEVSE>, IComparable<IEVSE>, IComparable,
                             IEnumerable<SocketOutlet>
    {

        #region Properties

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        IRoamingNetwork?                       RoamingNetwork          { get; }

        /// <summary>
        /// The charging station operator of this EVSE.
        /// </summary>
        [Optional]
        ChargingStationOperator?               Operator                { get; }

        /// <summary>
        /// The charging pool of this EVSE.
        /// </summary>
        ChargingPool?                          ChargingPool            { get; }

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        ChargingStation?                       ChargingStation         { get; }

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        [Optional]
        IRemoteEVSE?                           RemoteEVSE              { get; }



        /// <summary>
        /// All brands registered for this EVSE.
        /// </summary>
        ReactiveSet<Brand>                     Brands                  { get; }

        /// <summary>
        /// The license of the EVSE data.
        /// </summary>
        ReactiveSet<DataLicense>               DataLicenses            { get; }


        /// <summary>
        /// Charging modes.
        /// </summary>
        ReactiveSet<ChargingModes>             ChargingModes           { get; }

        /// <summary>
        /// The power socket outlets.
        /// </summary>
        ReactiveSet<SocketOutlet>              SocketOutlets           { get; set; }


        /// <summary>
        /// The type of the current.
        /// </summary>
        CurrentTypes?                          CurrentType             { get; set; }

        /// <summary>
        /// The average voltage.
        /// </summary>
        Decimal?                               AverageVoltage          { get; set; }


        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        Decimal?                               MaxCurrent              { get; set; }

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        Timestamped<Decimal>?                  MaxCurrentRealTime      { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        ReactiveSet<Timestamped<Decimal>>      MaxCurrentPrognoses     { get; }


        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        Decimal?                               MaxPower                { get; set; }

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        Timestamped<Decimal>?                  MaxPowerRealTime        { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        ReactiveSet<Timestamped<Decimal>>      MaxPowerPrognoses       { get; }


        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        Decimal?                               MaxCapacity             { get; set; }

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        Timestamped<Decimal>?                  MaxCapacityRealTime     { get; set; }

        /// <summary>
        /// Prognoses on future values of the capacity [kWh].
        /// </summary>
        ReactiveSet<Timestamped<Decimal>>      MaxCapacityPrognoses    { get; }


        /// <summary>
        /// The energy mix.
        /// </summary>
        EnergyMix?                             EnergyMix               { get; set; }

        /// <summary>
        /// The current energy mix.
        /// </summary>
        Timestamped<EnergyMix>?                EnergyMixRealTime       { get; set; }

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        EnergyMixPrognosis?                    EnergyMixPrognoses      { get; set; }


        /// <summary>
        /// An optional energy meter.
        /// </summary>
        EnergyMeter?                           EnergyMeter             { get; set; }



        Boolean IsFreeOfCharge { get; set; }



        TimeSpan MaxReservationDuration { get; set; }
        IEnumerable<ChargingReservation> Reservations { get; }
        ChargingSession? ChargingSession { get; set; }



        DateTime? LastStatusUpdate { get; set; }

        #endregion

        #region Events

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #endregion


        void AddCurrentType(CurrentTypes CurrentType);
        int CompareTo(EVSE? EVSE);
        bool Equals(EVSE? EVSE);
        bool Equals(object? Object);
        int GetHashCode();
        JObject ToJSON(bool Embedded = false, InfoStatus ExpandRoamingNetworkId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationOperatorId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingPoolId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationId = InfoStatus.ShowIdOnly, InfoStatus ExpandBrandIds = InfoStatus.ShowIdOnly, InfoStatus ExpandDataLicenses = InfoStatus.ShowIdOnly, CustomJObjectSerializerDelegate<EVSE>? CustomEVSESerializer = null);
        string ToString();
        EVSE UpdateWith(EVSE OtherEVSE);

    }

}
