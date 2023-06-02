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
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging pools.
    /// </summary>
    /// <param name="ChargingPool">A charging pool to include.</param>
    public delegate Boolean IncludeChargingPoolDelegate(IChargingPool ChargingPool);


    /// <summary>
    /// Extension methods for the commom charging pool interface.
    /// </summary>
    public static partial class ChargingPoolExtensions
    {

        #region AddChargingStation        (this ChargingPool, Id, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        public static Task<AddChargingStationResult> AddChargingStation(this IChargingPool                                              ChargingPool,

                                                                        ChargingStation_Id                                              Id,
                                                                        I18NString?                                                     Name                           = null,
                                                                        I18NString?                                                     Description                    = null,

                                                                        Address?                                                        Address                        = null,
                                                                        GeoCoordinate?                                                  GeoLocation                    = null,

                                                                        Action<IChargingStation>?                                       Configurator                   = null,
                                                                        RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                                        Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                        Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                        UInt16?                                                         MaxAdminStatusListSize         = null,
                                                                        UInt16?                                                         MaxStatusListSize              = null,

                                                                        EventTracking_Id?                                               EventTrackingId                = null,
                                                                        Action<IChargingStation>?                                       OnSuccess                      = null,
                                                                        Action<IChargingPool, ChargingStation>?                         OnError                        = null,
                                                                        Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null)


            => ChargingPool.AddChargingStation(new ChargingStation(Id,
                                                                   ChargingPool,
                                                                   Name,
                                                                   Description,

                                                                   Address,
                                                                   GeoLocation,

                                                                   Configurator,
                                                                   RemoteChargingStationCreator,
                                                                   InitialAdminStatus,
                                                                   InitialStatus,
                                                                   MaxAdminStatusListSize,
                                                                   MaxStatusListSize),

                                               RemoteChargingStationCreator,

                                               EventTrackingId,
                                               OnSuccess,
                                               OnError,
                                               AllowInconsistentOperatorIds);

        #endregion


        #region ToJSON(this ChargingPools, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<IChargingPool>                     ChargingPools,
                                    UInt64?                                             Skip                              = null,
                                    UInt64?                                             Take                              = null,
                                    Boolean                                             Embedded                          = false,
                                    InfoStatus                                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                                          ExpandChargingStationIds          = InfoStatus.Expanded,
                                    InfoStatus                                          ExpandEVSEIds                     = InfoStatus.Hidden,
                                    InfoStatus                                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                                          ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<IChargingPool>?     CustomChargingPoolSerializer      = null,
                                    CustomJObjectSerializerDelegate<IChargingStation>?  CustomChargingStationSerializer   = null,
                                    CustomJObjectSerializerDelegate<IEVSE>?             CustomEVSESerializer              = null)


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
                                     ILocalChargingReservations,
                                     ILocalRemoteStartStop,
                                     IChargingSessions,
                                     IChargeDetailRecords,
                                     IEquatable<IChargingPool>, IComparable<IChargingPool>, IComparable,
                                     IEnumerable<IChargingStation>

    {

        #region Properties

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        [Mandatory]
        IRoamingNetwork?                        RoamingNetwork              { get; }

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        ChargingStationOperator?                Operator                    { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        [Optional]
        ChargingStationOperator?                SubOperator                 { get; }

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        IRemoteChargingPool?                    RemoteChargingPool          { get; }


        /// <summary>
        /// All brands registered for this charging pool.
        /// </summary>
        [Optional]
        ReactiveSet<Brand>                      Brands                      { get; }

        /// <summary>
        /// The license of the charging pool data.
        /// </summary>
        [Optional]
        ReactiveSet<OpenDataLicense>            DataLicenses                { get; }

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        [Optional]
        Languages?                              LocationLanguage            { get; set; }

        /// <summary>
        /// The address of this charging pool.
        /// </summary>
        [Optional]
        Address?                                Address                     { get; set; }

        /// <summary>
        /// The geographical location of this charging pool.
        /// </summary>
        [Optional]
        GeoCoordinate?                          GeoLocation                 { get; set; }

        /// <summary>
        /// The address of the entrance to this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        Address?                                EntranceAddress             { get; set; }

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        GeoCoordinate?                          EntranceLocation            { get; set; }

        /// <summary>
        /// An optional (multi-language) description of how to find the charging pool.
        /// </summary>
        [Optional]
        I18NString                              ArrivalInstructions         { get; }

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        [Optional]
        OpeningTimes                            OpeningTimes                { get; set; }

        /// <summary>
        /// Indicates if the charging stations are still charging outside the opening hours of the charging pool.
        /// </summary>
        [Optional]
        Boolean?                                ChargingWhenClosed          { get; set; }

        /// <summary>
        /// User interface features of the charging pool, when those features
        /// are not features of the charging stations, e.g. an external payment terminal.
        /// </summary>
        [Optional]
        ReactiveSet<UIFeatures>                 UIFeatures                  { get; }

        /// <summary>
        /// The authentication options an EV driver can use.
        /// </summary>
        [Optional]
        ReactiveSet<AuthenticationModes>        AuthenticationModes         { get; }

        /// <summary>
        /// The payment options an EV driver can use.
        /// </summary>
        [Optional]
        ReactiveSet<PaymentOptions>             PaymentOptions              { get; }

        /// <summary>
        /// The accessibility of the charging station.
        /// </summary>
        [Optional]
        AccessibilityTypes?                     Accessibility               { get; set; }

        /// <summary>
        /// Charging features of the charging pool, when those features
        /// are not features of the charging stations, e.g. hasARoof.
        /// </summary>
        [Optional]
        ReactiveSet<Features>                   Features                    { get; }

        /// <summary>
        /// Charging facilities of the charging pool, e.g. a supermarket.
        /// </summary>
        [Optional]
        ReactiveSet<Facilities>                 Facilities                  { get; }


        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        [Optional]
        ReactiveSet<URL>                        PhotoURLs                   { get; }

        /// <summary>
        /// The telephone number of the charging station operator hotline.
        /// </summary>
        [Optional]
        PhoneNumber?                            HotlinePhoneNumber          { get; }

        /// <summary>
        /// The grid connection of the charging pool.
        /// </summary>
        [Optional]
        GridConnectionTypes?                    GridConnection              { get; set; }


        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Optional]
        Decimal?                                MaxCurrent                  { get; set; }

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Optional]
        Timestamped<Decimal>?                   MaxCurrentRealTime          { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        [Optional]
        ReactiveSet<Timestamped<Decimal>>       MaxCurrentPrognoses         { get; }


        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Optional]
        Decimal?                                MaxPower                    { get; set; }

        /// <summary>
        /// The real-time maximum power [kWatt].
        /// </summary>
        [Optional]
        Timestamped<Decimal>?                   MaxPowerRealTime            { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum power [kWatt].
        /// </summary>
        [Optional]
        ReactiveSet<Timestamped<Decimal>>       MaxPowerPrognoses           { get; }


        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Optional]
        Decimal?                                MaxCapacity                 { get; set; }

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        [Optional]
        Timestamped<Decimal>?                   MaxCapacityRealTime         { get; set; }

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        [Optional]
        ReactiveSet<Timestamped<Decimal>>       MaxCapacityPrognoses        { get; }


        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional]
        EnergyMix?                              EnergyMix                   { get; set; }

        /// <summary>
        /// The current energy mix.
        /// </summary>
        [Optional]
        Timestamped<EnergyMix>?                 EnergyMixRealTime           { get; set; }

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        [Optional]
        EnergyMixPrognosis?                     EnergyMixPrognoses          { get; set; }


        /// <summary>
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        [Optional]
        Address                                 ExitAddress                 { get; set; }

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        [Optional]
        GeoCoordinate?                          ExitLocation                { get; set; }


        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
        [Optional]
        Func<ChargingStationStatusReport, ChargingPoolStatusTypes>? StatusAggregationDelegate { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        event OnChargingPoolDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        event OnChargingPoolAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        event OnChargingPoolStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region Charging stations

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        IEnumerable<IChargingStation> ChargingStations { get; }

        /// <summary>
        /// Return an enumeration of all charging station identifications.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate? IncludeStations = null);

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationAddition { get; }

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        IVotingSender<DateTime, IChargingPool, IChargingStation, Boolean> OnChargingStationRemoval { get; }


        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        Task<AddChargingStationResult> AddChargingStation(ChargingStation                                                 ChargingStation,
                                                          RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,

                                                          EventTracking_Id?                                               EventTrackingId                = null,
                                                          Action<IChargingStation>?                                       OnSuccess                      = null,
                                                          Action<IChargingPool, ChargingStation>?                         OnError                        = null,
                                                          Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null);

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="Id">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station before its successful creation.</param>
        /// <param name="RemoteChargingStationCreator">A delegate to attach a remote charging station.</param>
        /// <param name="InitialAdminStatus">An optional initial admin status of the EVSE.</param>
        /// <param name="InitialStatus">An optional initial status of the EVSE.</param>
        /// <param name="MaxAdminStatusListSize">An optional max length of the admin staus list.</param>
        /// <param name="MaxStatusListSize">An optional max length of the staus list.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to configure the new charging station after its successful update.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(ChargingStation_Id                                              Id,
                                                                          I18NString?                                                     Name                           = null,
                                                                          I18NString?                                                     Description                    = null,

                                                                          Address?                                                        Address                        = null,
                                                                          GeoCoordinate?                                                  GeoLocation                    = null,

                                                                          Action<IChargingStation>?                                       Configurator                   = null,
                                                                          RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                                          Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                          Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                          UInt16?                                                         MaxAdminStatusListSize         = null,
                                                                          UInt16?                                                         MaxStatusListSize              = null,

                                                                          String?                                                         DataSource                     = null,
                                                                          DateTime?                                                       LastChange                     = null,

                                                                          JObject?                                                        CustomData                     = null,
                                                                          UserDefinedDictionary?                                          InternalData                   = null,
                                                                          EventTracking_Id?                                               EventTrackingId                = null,

                                                                          Action<IChargingStation>?                                       OnAdditionSuccess              = null,
                                                                          Action<IChargingStation, IChargingStation>?                     OnUpdateSuccess                = null,
                                                                          Action<IChargingPool, ChargingStation_Id>?                      OnError                        = null,
                                                                          Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null);


        /// <summary>
        /// Check if the given ChargingStation is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        Boolean ContainsChargingStation(IChargingStation ChargingStation);

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging pool.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        Boolean ContainsChargingStation(ChargingStation_Id ChargingStationId);

        IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId);

        Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation);

        Task<RemoveChargingStationResult> RemoveChargingStation(ChargingStation_Id ChargingStationId);

        Boolean TryRemoveChargingStation(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation);


        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationDataChangedDelegate         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationStatusChangedDelegate       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationAdminStatusChangedDelegate  OnChargingStationAdminStatusChanged;


        Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? ChargingStation);

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
        IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null);

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
        Boolean ContainsEVSE(EVSE EVSE);

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        Boolean ContainsEVSE(EVSE_Id EVSEId);

        IEVSE GetEVSEById(EVSE_Id EVSEId);

        Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE);


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
        /// Update this charging pool with the data of the other charging pool.
        /// </summary>
        /// <param name="OtherEVSE">Another charging pool.</param>
        ChargingPool UpdateWith(ChargingPool OtherChargingPool);


        /// <summary>
        /// Return a JSON representation of the given charging pool.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public JObject ToJSON(Boolean                                             Embedded                          = false,
                              InfoStatus                                          ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandChargingStationIds          = InfoStatus.Expanded,
                              InfoStatus                                          ExpandEVSEIds                     = InfoStatus.Hidden,
                              InfoStatus                                          ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                          ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingPool>?     CustomChargingPoolSerializer      = null,
                              CustomJObjectSerializerDelegate<IChargingStation>?  CustomChargingStationSerializer   = null,
                              CustomJObjectSerializerDelegate<IEVSE>?             CustomEVSESerializer              = null);


    }

}
