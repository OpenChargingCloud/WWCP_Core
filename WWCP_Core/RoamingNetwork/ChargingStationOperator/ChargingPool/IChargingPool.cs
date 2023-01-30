﻿/*
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

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
                                     IEnumerable<IChargingStation>

    {

        #region Properties

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        IRoamingNetwork?                        RoamingNetwork              { get; }

        /// <summary>
        /// The charging station operator of this charging pool.
        /// </summary>
        [Optional]
        ChargingStationOperator?                Operator                    { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        ChargingStationOperator?                SubOperator                 { get; }

        /// <summary>
        /// The remote charging pool.
        /// </summary>
        [Optional]
        IRemoteChargingPool?                    RemoteChargingPool          { get; }


        /// <summary>
        /// All brands registered for this charging pool.
        /// </summary>
        ReactiveSet<Brand>                      Brands                      { get; }

        /// <summary>
        /// The license of the charging pool data.
        /// </summary>
        ReactiveSet<DataLicense>                DataLicenses                { get; }

        /// <summary>
        /// The official language at this charging pool.
        /// </summary>
        Languages?                              LocationLanguage            { get; set; }

        /// <summary>
        /// The address of this charging pool.
        /// </summary>
        Address                                 Address                     { get; set; }

        /// <summary>
        /// The geographical location of this charging pool.
        /// </summary>
        GeoCoordinate?                          GeoLocation                 { get; set; }

        /// <summary>
        /// The address of the entrance to this charging pool.
        /// (If different from 'Address').
        /// </summary>
        Address                                 EntranceAddress             { get; set; }

        /// <summary>
        /// The geographical location of the entrance to this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        GeoCoordinate?                          EntranceLocation            { get; set; }

        /// <summary>
        /// An optional (multi-language) description of how to find the charging pool.
        /// </summary>
        I18NString                              ArrivalInstructions         { get; }

        /// <summary>
        /// The opening times of this charging pool.
        /// </summary>
        OpeningTimes                            OpeningTimes                { get; set; }

        /// <summary>
        /// The user interface features of the charging station.
        /// </summary>
        UIFeatures?                             UIFeatures                  { get; set; }

        ReactiveSet<AuthenticationModes>        AuthenticationModes         { get; }

        ReactiveSet<PaymentOptions>             PaymentOptions              { get; }

        AccessibilityTypes?                     Accessibility               { get; set; }

        /// <summary>
        /// URIs of photos of this charging pool.
        /// </summary>
        ReactiveSet<URL>                        PhotoURLs                   { get; }

        /// <summary>
        /// The telephone number of the charging station operator hotline.
        /// </summary>
        I18NString                              HotlinePhoneNumber          { get; }

        /// <summary>
        /// The grid connection of the charging pool.
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

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
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
        /// The address of the exit of this charging pool.
        /// (If different from 'Address').
        /// </summary>
        Address                                 ExitAddress                 { get; set; }

        /// <summary>
        /// The geographical location of the exit of this charging pool.
        /// (If different from 'GeoLocation').
        /// </summary>
        GeoCoordinate?                          ExitLocation                { get; set; }


        Partly                                  IsHubjectCompatible         { get; }

        Partly                                  DynamicInfoAvailable        { get; }


        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated charging stations.
        /// </summary>
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
        IEnumerable<ChargingStation_Id> ChargingStationIds(IncludeChargingStationDelegate IncludeStations = null);

        /// <summary>
        /// Return an enumeration of all charging station admin status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationAdminStatus> ChargingStationAdminStatus(IncludeChargingStationDelegate IncludeStations = null);

        /// <summary>
        /// Return an enumeration of all charging station status.
        /// </summary>
        /// <param name="IncludeStations">An optional delegate for filtering charging stations.</param>
        IEnumerable<ChargingStationStatus> ChargingStationStatus(IncludeChargingStationDelegate IncludeStations = null);

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
        IChargingStation? CreateChargingStation(ChargingStation_Id                                              Id,
                                                I18NString?                                                     Name                           = null,
                                                I18NString?                                                     Description                    = null,
                                                Action<IChargingStation>?                                       Configurator                   = null,
                                                RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                UInt16?                                                         MaxAdminStatusListSize         = null,
                                                UInt16?                                                         MaxStatusListSize              = null,
                                                Action<IChargingStation>?                                       OnSuccess                      = null,
                                                Action<IChargingPool, ChargingStation_Id>?                      OnError                        = null,
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
        /// <param name="OnSuccess">An optional delegate to configure the new charging station after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging station failed.</param>
        IChargingStation? CreateOrUpdateChargingStation(ChargingStation_Id                                              Id,
                                                        I18NString?                                                     Name                           = null,
                                                        I18NString?                                                     Description                    = null,
                                                        Action<IChargingStation>?                                       Configurator                   = null,
                                                        RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,
                                                        Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                        Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                        UInt16?                                                         MaxAdminStatusListSize         = null,
                                                        UInt16?                                                         MaxStatusListSize              = null,
                                                        Action<IChargingStation>?                                       OnSuccess                      = null,
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

        IChargingStation? RemoveChargingStation(ChargingStation_Id ChargingStationId);

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


        Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? Station);

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
        IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate IncludeEVSEs = null);

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
        Boolean ContainsEVSE(EVSE EVSE);

        /// <summary>
        /// Check if the given EVSE identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        Boolean ContainsEVSE(EVSE_Id EVSEId);

        IEVSE GetEVSEById(EVSE_Id EVSEId);

        Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE EVSE);


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


        ChargingPool UpdateWith(ChargingPool OtherChargingPool);


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