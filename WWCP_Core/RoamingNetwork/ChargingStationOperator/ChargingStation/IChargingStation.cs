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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging stations.
    /// </summary>
    /// <param name="ChargingStation">A charging station to include.</param>
    public delegate Boolean IncludeChargingStationDelegate(IChargingStation ChargingStation);


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
                                        ILocalRemoteStartStop,
                                        ILocalChargingReservations,
                                        IChargingSessions,
                                        IChargeDetailRecords,
                                        IEquatable<IChargingStation>, IComparable<IChargingStation>, IComparable,
                                        IEnumerable<IEVSE>
    {

        #region Properties

        /// <summary>
        /// The roaming network of this charging station.
        /// </summary>
        IRoamingNetwork?                        RoamingNetwork              { get; }

        /// <summary>
        /// The charging station operator of this charging station.
        /// </summary>
        ChargingStationOperator?                Operator                    { get; }

        /// <summary>
        /// The charging station sub operator of this charging station.
        /// </summary>
        ChargingStationOperator?                SubOperator                 { get; }

        /// <summary>
        /// The charging pool of this charging station.
        /// </summary>
        ChargingPool?                           ChargingPool                { get; }

        /// <summary>
        /// An optional remote charging station.
        /// </summary>
        [Optional]
        IRemoteChargingStation?                 RemoteChargingStation       { get; }



        /// <summary>
        /// All brands registered for this charging station.
        /// </summary>
        ReactiveSet<Brand>                      Brands                      { get; }

        /// <summary>
        /// The license of the charging station data.
        /// </summary>
        ReactiveSet<OpenDataLicense>                DataLicenses                { get; }


        /// <summary>
        /// The address of this charging station.
        /// </summary>
        Address?                                Address                     { get; set; }

        /// <summary>
        /// OpenStreetMap Node Id.
        /// </summary>
        String?                                 OpenStreetMapNodeId         { get; set; }

        /// <summary>
        /// The geographical location of this charging station.
        /// </summary>
        GeoCoordinate?                          GeoLocation                 { get; set; }

        /// <summary>
        /// The address of the entrance to this charging station.
        /// (If different from 'Address').
        /// </summary>
        Address?                                EntranceAddress             { get; set; }

        /// <summary>
        /// The geographical location of the entrance to this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        GeoCoordinate?                          EntranceLocation            { get; set; }

        /// <summary>
        /// An optional (multi-language) description of how to find the charging station.
        /// </summary>
        I18NString                              ArrivalInstructions         { get; }

        /// <summary>
        /// The opening times of this charging station (non recursive).
        /// </summary>
        OpeningTimes                            OpeningTimes                { get; set; }

        /// <summary>
        /// Parking spaces located at the charging station.
        /// </summary>
        ReactiveSet<ParkingSpace>               ParkingSpaces               { get; }

        /// <summary>
        /// The user interface features of the charging station.
        /// </summary>
        UIFeatures?                             UIFeatures                  { get; set; }

        ReactiveSet<AuthenticationModes>        AuthenticationModes         { get; }
        ReactiveSet<PaymentOptions>             PaymentOptions              { get; }
        AccessibilityTypes?                     Accessibility               { get; set; }


        /// <summary>
        /// An optional number/string printed on the outside of the charging station for visual identification.
        /// </summary>
        String?                                 PhysicalReference           { get; }

        /// <summary>
        /// URIs of photos of this charging station.
        /// </summary>
        ReactiveSet<URL>                        PhotoURLs                   { get; }

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        PhoneNumber?                            HotlinePhoneNumber          { get; set; }

        /// <summary>
        /// The address of the exit of this charging station.
        /// (If different from 'Address').
        /// </summary>
        Address                                 ExitAddress                 { get; set; }

        /// <summary>
        /// The geographical location of the exit of this charging station.
        /// (If different from 'GeoLocation').
        /// </summary>
        GeoCoordinate?                          ExitLocation                { get; set; }

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        GridConnectionTypes?                    GridConnection              { get; set; }



        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        Decimal?                                MaxCurrent                  { get; set; }

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        Timestamped<Decimal>?                   MaxCurrentRealTime          { get; set; }

        ReactiveSet<Timestamped<Decimal>>       MaxCurrentPrognoses         { get; }


        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        Decimal?                                MaxPower                    { get; set; }

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        Timestamped<Decimal>?                   MaxPowerRealTime            { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        ReactiveSet<Timestamped<Decimal>>       MaxPowerPrognoses           { get; }


        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        Decimal?                                MaxCapacity                 { get; set; }

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        Timestamped<Decimal>?                   MaxCapacityRealTime         { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        ReactiveSet<Timestamped<Decimal>>       MaxCapacityPrognoses        { get; }


        /// <summary>
        /// The energy mix.
        /// </summary>
        EnergyMix?                              EnergyMix                   { get; set; }

        /// <summary>
        /// The current energy mix.
        /// </summary>
        Timestamped<EnergyMix>?                 EnergyMixRealTime           { get; set; }

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        EnergyMixPrognosis?                     EnergyMixPrognoses          { get; set; }


        /// <summary>
        /// The maximum reservation time at this EVSE.
        /// </summary>
        TimeSpan                                MaxReservationDuration      { get; set; }

        /// <summary>
        /// Charging at this EVSE is ALWAYS free of charge.
        /// </summary>
        Boolean                                 IsFreeOfCharge              { get; set; }


        Boolean                                 IsHubjectCompatible         { get; set; }

        Boolean                                 DynamicInfoAvailable        { get; set; }


        /// <summary>
        /// The internal service identification of the charging station maintained by the Charging Station Operator.
        /// </summary>
        String?                                 ServiceIdentification       { get; set; }

        /// <summary>
        /// The internal model code of the charging station maintained by the Charging Station Operator.
        /// </summary>
        String?                                 ModelCode                   { get; set; }

        String?                                 HubjectStationId            { get; set; }


        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        Func<EVSEStatusReport, ChargingStationStatusTypes> StatusAggregationDelegate { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        event OnChargingStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        event OnChargingStationStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        event OnChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region EVSEs

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSEAddition { get; }

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging pool.
        /// </summary>
        IEnumerable<IEVSE> EVSEs { get; }

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment
        /// (EVSEs) present within this charging pool.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>>

            EVSEAdminStatusSchedule(IncludeEVSEDelegate?                  IncludeEVSEs      = null,
                                    Func<DateTime,             Boolean>?  TimestampFilter   = null,
                                    Func<EVSEAdminStatusTypes, Boolean>?  StatusFilter      = null,
                                    UInt64?                               Skip              = null,
                                    UInt64?                               Take              = null);

        /// <summary>
        /// Return the admin status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate IncludeEVSEs = null);

        /// <summary>
        /// Return the status of all EVSEs registered within this roaming network.
        /// </summary>
        /// <param name="IncludeEVSEs">An optional delegate for filtering EVSEs.</param>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="HistorySize">The size of the history.</param>
        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusTypes>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusTypes, Boolean>?  StatusFilter      = null,
                               UInt64?                          Skip              = null,
                               UInt64?                          Take              = null);



        /// <summary>
        /// Check if the given EVSE is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        Boolean ContainsEVSE(IEVSE EVSE);

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        Boolean ContainsEVSE(EVSE_Id EVSEId);

        IEVSE GetEVSEById(EVSE_Id EVSEId);

        Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE EVSE);



        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="RemoteEVSECreator">An optional delegate to configure a new remote EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        Task<AddEVSEResult> CreateEVSE(EVSE_Id                             Id,
                                       I18NString?                         Name                         = null,
                                       I18NString?                         Description                  = null,

                                       Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                       Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                       UInt16?                             MaxAdminStatusScheduleSize   = null,
                                       UInt16?                             MaxStatusScheduleSize        = null,

                                       IEnumerable<URL>?                   PhotoURLs                    = null,
                                       IEnumerable<Brand>?                 Brands                       = null,
                                       IEnumerable<OpenDataLicense>?       OpenDataLicenses             = null,
                                       IEnumerable<ChargingModes>?         ChargingModes                = null,
                                       IEnumerable<ChargingTariff>?        ChargingTariffs              = null,
                                       CurrentTypes?                       CurrentType                  = null,
                                       Decimal?                            AverageVoltage               = null,
                                       Decimal?                            MaxCurrent                   = null,
                                       Timestamped<Decimal>?               MaxCurrentRealTime           = null,
                                       IEnumerable<Timestamped<Decimal>>?  MaxCurrentPrognoses          = null,
                                       Decimal?                            MaxPower                     = null,
                                       Timestamped<Decimal>?               MaxPowerRealTime             = null,
                                       IEnumerable<Timestamped<Decimal>>?  MaxPowerPrognoses            = null,
                                       Decimal?                            MaxCapacity                  = null,
                                       Timestamped<Decimal>?               MaxCapacityRealTime          = null,
                                       IEnumerable<Timestamped<Decimal>>?  MaxCapacityPrognoses         = null,
                                       EnergyMix?                          EnergyMix                    = null,
                                       Timestamped<EnergyMix>?             EnergyMixRealTime            = null,
                                       EnergyMixPrognosis?                 EnergyMixPrognoses           = null,
                                       EnergyMeter?                        EnergyMeter                  = null,
                                       Boolean?                            IsFreeOfCharge               = null,
                                       IEnumerable<SocketOutlet>?          SocketOutlets                = null,

                                       ChargingSession?                    ChargingSession              = null,
                                       DateTime?                           LastStatusUpdate             = null,
                                       String?                             DataSource                   = null,
                                       DateTime?                           LastChange                   = null,
                                       JObject?                            CustomData                   = null,
                                       UserDefinedDictionary?              InternalData                 = null,

                                       Action<IEVSE>?                      Configurator                 = null,
                                       RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                       Action<IEVSE>?                      OnSuccess                    = null,
                                       Action<IChargingStation, EVSE_Id>?  OnError                      = null);

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new EVSE after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the EVSE failed.</param>
        Task<AddOrUpdateEVSEResult> CreateOrUpdateEVSE(EVSE_Id                             Id,
                                                       I18NString?                         Name                         = null,
                                                       I18NString?                         Description                  = null,

                                                       Timestamped<EVSEAdminStatusTypes>?  InitialAdminStatus           = null,
                                                       Timestamped<EVSEStatusTypes>?       InitialStatus                = null,
                                                       UInt16?                             MaxAdminStatusScheduleSize   = null,
                                                       UInt16?                             MaxStatusScheduleSize        = null,

                                                       IEnumerable<URL>?                   PhotoURLs                    = null,
                                                       IEnumerable<Brand>?                 Brands                       = null,
                                                       IEnumerable<OpenDataLicense>?       OpenDataLicenses             = null,
                                                       IEnumerable<ChargingModes>?         ChargingModes                = null,
                                                       IEnumerable<ChargingTariff>?        ChargingTariffs              = null,
                                                       CurrentTypes?                       CurrentType                  = null,
                                                       Decimal?                            AverageVoltage               = null,
                                                       Decimal?                            MaxCurrent                   = null,
                                                       Timestamped<Decimal>?               MaxCurrentRealTime           = null,
                                                       IEnumerable<Timestamped<Decimal>>?  MaxCurrentPrognoses          = null,
                                                       Decimal?                            MaxPower                     = null,
                                                       Timestamped<Decimal>?               MaxPowerRealTime             = null,
                                                       IEnumerable<Timestamped<Decimal>>?  MaxPowerPrognoses            = null,
                                                       Decimal?                            MaxCapacity                  = null,
                                                       Timestamped<Decimal>?               MaxCapacityRealTime          = null,
                                                       IEnumerable<Timestamped<Decimal>>?  MaxCapacityPrognoses         = null,
                                                       EnergyMix?                          EnergyMix                    = null,
                                                       Timestamped<EnergyMix>?             EnergyMixRealTime            = null,
                                                       EnergyMixPrognosis?                 EnergyMixPrognoses           = null,
                                                       EnergyMeter?                        EnergyMeter                  = null,
                                                       Boolean?                            IsFreeOfCharge               = null,
                                                       IEnumerable<SocketOutlet>?          SocketOutlets                = null,

                                                       ChargingSession?                    ChargingSession              = null,
                                                       DateTime?                           LastStatusUpdate             = null,
                                                       String?                             DataSource                   = null,
                                                       DateTime?                           LastChange                   = null,
                                                       JObject?                            CustomData                   = null,
                                                       UserDefinedDictionary?              InternalData                 = null,

                                                       Action<IEVSE>?                      Configurator                 = null,
                                                       RemoteEVSECreatorDelegate?          RemoteEVSECreator            = null,
                                                       Action<IEVSE>?                      OnSuccess                    = null,
                                                       Action<IChargingStation, EVSE_Id>?  OnError                      = null);


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        //#region SocketOutletAddition

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition

        //    => SocketOutletAddition;

        //#endregion

        //#region SocketOutletRemoval

        //internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval

        //    => SocketOutletRemoval;

        //#endregion


        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        IVotingSender<DateTime, IChargingStation, IEVSE, Boolean> OnEVSERemoval { get; }

        #endregion


        /// <summary>
        /// Update this charging station with the data of the other charging station.
        /// </summary>
        /// <param name="OtherChargingStation">Another charging station.</param>
        IChargingStation UpdateWith(IChargingStation OtherChargingStation);


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
