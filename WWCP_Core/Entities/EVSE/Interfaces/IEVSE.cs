/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
        public static JArray ToJSON(this IEnumerable<IEVSE>                              EVSEs,
                                    UInt64?                                              Skip                                = null,
                                    UInt64?                                              Take                                = null,
                                    Boolean                                              Embedded                            = false,
                                    InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingStationId             = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                                    CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)


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
                                                                         CustomEVSESerializer,
                                                                         CustomChargingConnectorSerializer)).
                                      Where         (json => json is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The common interface of all Electric Vehicle Supply Equipments (EVSEs).
    /// </summary>
    public interface IEVSE : IEntity<EVSE_Id>,
                             IAdminStatus<EVSEAdminStatusTypes>,
                             IStatus<EVSEStatusTypes>,
                             ILocalRemoteStartStop,
                             ILocalChargingReservations,
                             IChargingSessions,
                             IChargeDetailRecords,
                             IEquatable<IEVSE>, IComparable<IEVSE>, IComparable,
                             IEnumerable<IChargingConnector>
    {

        #region Properties

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        [InternalUseOnly]
        IRoamingNetwork?                       RoamingNetwork               { get; }

        /// <summary>
        /// The charging station operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        IChargingStationOperator?              Operator                     { get; }

        /// <summary>
        /// The charging pool of this EVSE.
        /// </summary>
        [InternalUseOnly]
        IChargingPool?                         ChargingPool                 { get; }

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        [InternalUseOnly]
        IChargingStation?                      ChargingStation              { get; }

        /// <summary>
        /// An optional remote EVSE.
        /// </summary>
        [InternalUseOnly]
        IRemoteEVSE?                           RemoteEVSE                   { get; }


        /// <summary>
        /// The current charging session of this EVSE.
        /// </summary>
        [InternalUseOnly]
        ChargingSession?                       ChargingSession              { get; set; }



        /// <summary>
        /// An optional number/string printed on the outside of the EVSE for visual identification.
        /// </summary>
        [Optional]
        String?                                PhysicalReference            { get; }

        /// <summary>
        /// An optional enumeration of links to photos related to the EVSE.
        /// </summary>
        [Optional, SlowData]
        ReactiveSet<URL>                       PhotoURLs                    { get; }

        /// <summary>
        /// An enumeration of all brands registered for this EVSE.
        /// </summary>
        [Optional, SlowData]
        ReactiveSet<Brand>                     Brands                       { get; }

        /// <summary>
        /// An enumeration of all data license(s) of this EVSE.
        /// </summary>
        [Optional, SlowData]
        ReactiveSet<OpenDataLicense>           OpenDataLicenses             { get; }

        /// <summary>
        /// An enumeration of all supported charging modes of this EVSE.
        /// </summary>
        [Mandatory, SlowData]
        ReactiveSet<ChargingModes>             ChargingModes                { get; }

        /// <summary>
        /// An enumeration of all available charging tariffs at this EVSE.
        /// </summary>
        [Optional, SlowData]
        ReactiveSet<ChargingTariff>            ChargingTariffs              { get; }


        /// <summary>
        /// The type of the current of this EVSE.
        /// </summary>
        [Mandatory]
        CurrentTypes                           CurrentType                  { get; set; }

        /// <summary>
        /// The average voltage of this EVSE [Volt].
        /// </summary>
        [Optional, SlowData]
        Decimal?                               AverageVoltage               { get; set; }

        /// <summary>
        /// The real-time average voltage of this EVSE [Volt].
        /// </summary>
        [Optional, FastData]
        Timestamped<Decimal>?                  AverageVoltageRealTime       { get; set; }

        /// <summary>
        /// Prognoses on future values of the average voltage of this EVSE [Volt].
        /// </summary>
        [Optional, FastData]
        ReactiveSet<Timestamped<Decimal>>      AverageVoltagePrognoses      { get; }


        /// <summary>
        /// The maximum current of this EVSE [Ampere].
        /// </summary>
        [Optional, SlowData]
        Decimal?                               MaxCurrent                   { get; set; }

        /// <summary>
        /// The real-time maximum current of this EVSE [Ampere].
        /// </summary>
        [Optional, FastData]
        Timestamped<Decimal>?                  MaxCurrentRealTime           { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum current of this EVSE [Ampere].
        /// </summary>
        [Optional, FastData]
        ReactiveSet<Timestamped<Decimal>>      MaxCurrentPrognoses          { get; }


        /// <summary>
        /// The maximum power of this EVSE [kWatt].
        /// </summary>
        [Optional, SlowData]
        Decimal?                               MaxPower                     { get; set; }

        /// <summary>
        /// The real-time maximum power of this EVSE [kWatt].
        /// </summary>
        [Optional, FastData]
        Timestamped<Decimal>?                  MaxPowerRealTime             { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum power of this EVSE [kWatt].
        /// </summary>
        [Optional, FastData]
        ReactiveSet<Timestamped<Decimal>>      MaxPowerPrognoses            { get; }


        /// <summary>
        /// The maximum capacity of this EVSE [kWh].
        /// </summary>
        [Optional, SlowData]
        Decimal?                               MaxCapacity                  { get; set; }

        /// <summary>
        /// The real-time maximum capacity of this EVSE [kWh].
        /// </summary>
        [Optional, FastData]
        Timestamped<Decimal>?                  MaxCapacityRealTime          { get; set; }

        /// <summary>
        /// Prognoses on future values of the capacity of this EVSE [kWh].
        /// </summary>
        [Optional, FastData]
        ReactiveSet<Timestamped<Decimal>>      MaxCapacityPrognoses         { get; }


        /// <summary>
        /// The energy mix of this EVSE.
        /// </summary>
        [Optional, SlowData]
        EnergyMix?                             EnergyMix                    { get; set; }

        /// <summary>
        /// The current energy mix of this EVSE.
        /// </summary>
        [Optional, FastData]
        Timestamped<EnergyMix>?                EnergyMixRealTime            { get; set; }

        /// <summary>
        /// Prognoses on future values of the energy mix of this EVSE.
        /// </summary>
        [Optional, FastData]
        EnergyMixPrognosis?                    EnergyMixPrognoses           { get; set; }

        /// <summary>
        /// An optional energy meter of this EVSE.
        /// </summary>
        [Optional]
        EnergyMeter?                           EnergyMeter                  { get; set; }

        /// <summary>
        /// Whether this EVSE is free of charge (shortcut for "no charging tariffs").
        /// </summary>
        [Optional]
        Boolean                                IsFreeOfCharge               { get; set; }

        /// <summary>
        /// The timestamp of the last status update.
        /// This might be different from the timestamp of the last status change,
        /// when the status was imported from a third party.
        /// </summary>
        [Optional]
        DateTime?                              LastStatusUpdate             { get; set; }


        /// <summary>
        /// The power socket outlets of this EVSE.
        /// </summary>
        [Mandatory, SlowData]
        ReactiveSet<IChargingConnector>        ChargingConnectors           { get; set; }

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


        void SetAdminStatus (EVSEAdminStatus         EVSEAdminStatus);
        void SetAdminStatus (EVSEAdminStatusUpdate   EVSEAdminStatusUpdate);

        void SetStatus      (EVSEStatus              EVSEStatus);
        void SetStatus      (EVSEStatusUpdate        EVSEStatusUpdate);

        void SetEnergyStatus(EVSEEnergyStatus        EVSEEnergyStatus);
        void SetEnergyStatus(EVSEEnergyStatusUpdate  EVSEEnergyStatusUpdate);


        /// <summary>
        /// Update this EVSE with the data of the other EVSE.
        /// </summary>
        /// <param name="OtherEVSE">Another EVSE.</param>
        IEVSE UpdateWith(IEVSE OtherEVSE);


        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        JObject? ToJSON(Boolean                                              Embedded                            = false,
                        InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                        InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                        InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                        InfoStatus                                           ExpandChargingStationId             = InfoStatus.ShowIdOnly,
                        InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                        InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                        CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                        CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null);


    }

}
