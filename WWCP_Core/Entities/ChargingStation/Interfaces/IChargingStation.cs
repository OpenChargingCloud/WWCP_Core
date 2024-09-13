/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for the common charging station interface.
    /// </summary>
    public static class IChargingStationExtensions
    {

        #region AddEVSE           (this ChargingStation, Id, ..., Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new EVSE.
        /// </summary>
        /// <param name="ChargingStation">The charging station of the new EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddEVSEResult> AddEVSE(this IChargingStation                                ChargingStation,

                                                  EVSE_Id                                              Id,
                                                  I18NString?                                          Name                           = null,
                                                  I18NString?                                          Description                    = null,

                                                  IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                  IEnumerable<Brand>?                                  Brands                         = null,
                                                  IEnumerable<RootCAInfo>?                             MobilityRootCAs                = null,
                                                  IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                  IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                  IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                  CurrentTypes?                                        CurrentType                    = null,
                                                  Volt?                                                AverageVoltage                 = null,
                                                  Timestamped<Volt>?                                   AverageVoltageRealTime         = null,
                                                  IEnumerable<Timestamped<Volt>>?                      AverageVoltagePrognoses        = null,
                                                  Ampere?                                              MaxCurrent                     = null,
                                                  Timestamped<Ampere>?                                 MaxCurrentRealTime             = null,
                                                  IEnumerable<Timestamped<Ampere>>?                    MaxCurrentPrognoses            = null,
                                                  Watt?                                                MaxPower                       = null,
                                                  Timestamped<Watt>?                                   MaxPowerRealTime               = null,
                                                  IEnumerable<Timestamped<Watt>>?                      MaxPowerPrognoses              = null,
                                                  WattHour?                                            MaxCapacity                    = null,
                                                  Timestamped<WattHour>?                               MaxCapacityRealTime            = null,
                                                  IEnumerable<Timestamped<WattHour>>?                  MaxCapacityPrognoses           = null,
                                                  EnergyMix?                                           EnergyMix                      = null,
                                                  Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                  EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                  EnergyMeter?                                         EnergyMeter                    = null,
                                                  Boolean?                                             IsFreeOfCharge                 = null,
                                                  IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,
                                                  ChargingSession?                                     ChargingSession                = null,

                                                  Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                  Timestamped<EVSEStatusType>?                        InitialStatus                  = null,
                                                  UInt16?                                              MaxAdminStatusScheduleSize     = null,
                                                  UInt16?                                              MaxStatusScheduleSize          = null,
                                                  DateTime?                                            LastStatusUpdate               = null,

                                                  String?                                              DataSource                     = null,
                                                  DateTime?                                            LastChange                     = null,

                                                  JObject?                                             CustomData                     = null,
                                                  UserDefinedDictionary?                               InternalData                   = null,

                                                  Action<IEVSE>?                                       Configurator                   = null,
                                                  RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                  Action<IEVSE,                   EventTracking_Id>?   OnSuccess                      = null,
                                                  Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                        = null,

                                                  Boolean                                              SkipAddedNotifications         = false,
                                                  Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                  EventTracking_Id?                                    EventTrackingId                = null,
                                                  User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.AddEVSE(new EVSE(
                                           Id,
                                           ChargingStation,
                                           Name,
                                           Description,

                                           PhotoURLs,
                                           Brands,
                                           MobilityRootCAs,
                                           OpenDataLicenses,
                                           ChargingModes,
                                           ChargingTariffs,
                                           CurrentType,
                                           AverageVoltage,
                                           AverageVoltageRealTime,
                                           AverageVoltagePrognoses,
                                           MaxCurrent,
                                           MaxCurrentRealTime,
                                           MaxCurrentPrognoses,
                                           MaxPower,
                                           MaxPowerRealTime,
                                           MaxPowerPrognoses,
                                           MaxCapacity,
                                           MaxCapacityRealTime,
                                           MaxCapacityPrognoses,
                                           EnergyMix,
                                           EnergyMixRealTime,
                                           EnergyMixPrognoses,
                                           EnergyMeter,
                                           IsFreeOfCharge,
                                           ChargingConnectors,
                                           ChargingSession,

                                           InitialAdminStatus,
                                           InitialStatus,
                                           MaxAdminStatusScheduleSize,
                                           MaxStatusScheduleSize,
                                           LastStatusUpdate,

                                           DataSource,
                                           LastChange,

                                           CustomData,
                                           InternalData,

                                           Configurator,
                                           RemoteEVSECreator
                                       ),

                                       OnSuccess,
                                       OnError,

                                       SkipAddedNotifications,
                                       AllowInconsistentOperatorIds,
                                       EventTrackingId,
                                       CurrentUserId);

        #endregion

        #region AddEVSEIfNotExists(this ChargingStation, Id, ..., Configurator = null, OnSuccess = null)

        /// <summary>
        /// Add a new EVSE, but do not fail when this EVSE already exists.
        /// </summary>
        /// <param name="ChargingStation">The charging station of the new EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddEVSEResult> AddEVSEIfNotExists(this IChargingStation                                ChargingStation,

                                                             EVSE_Id                                              Id,
                                                             I18NString?                                          Name                           = null,
                                                             I18NString?                                          Description                    = null,

                                                             IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                             IEnumerable<Brand>?                                  Brands                         = null,
                                                             IEnumerable<RootCAInfo>?                             MobilityRootCAs                = null,
                                                             IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                             IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                             IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                             CurrentTypes?                                        CurrentType                    = null,
                                                             Volt?                                                AverageVoltage                 = null,
                                                             Timestamped<Volt>?                                   AverageVoltageRealTime         = null,
                                                             IEnumerable<Timestamped<Volt>>?                      AverageVoltagePrognoses        = null,
                                                             Ampere?                                              MaxCurrent                     = null,
                                                             Timestamped<Ampere>?                                 MaxCurrentRealTime             = null,
                                                             IEnumerable<Timestamped<Ampere>>?                    MaxCurrentPrognoses            = null,
                                                             Watt?                                                MaxPower                       = null,
                                                             Timestamped<Watt>?                                   MaxPowerRealTime               = null,
                                                             IEnumerable<Timestamped<Watt>>?                      MaxPowerPrognoses              = null,
                                                             WattHour?                                            MaxCapacity                    = null,
                                                             Timestamped<WattHour>?                               MaxCapacityRealTime            = null,
                                                             IEnumerable<Timestamped<WattHour>>?                  MaxCapacityPrognoses           = null,
                                                             EnergyMix?                                           EnergyMix                      = null,
                                                             Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                             EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                             EnergyMeter?                                         EnergyMeter                    = null,
                                                             Boolean?                                             IsFreeOfCharge                 = null,
                                                             IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,

                                                             ChargingSession?                                     ChargingSession                = null,

                                                             Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                             Timestamped<EVSEStatusType>?                        InitialStatus                  = null,
                                                             UInt16?                                              MaxAdminStatusScheduleSize     = null,
                                                             UInt16?                                              MaxStatusScheduleSize          = null,
                                                             DateTime?                                            LastStatusUpdate               = null,

                                                             String?                                              DataSource                     = null,
                                                             DateTime?                                            LastChange                     = null,

                                                             JObject?                                             CustomData                     = null,
                                                             UserDefinedDictionary?                               InternalData                   = null,

                                                             Action<IEVSE>?                                       Configurator                   = null,
                                                             RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                             Action<IEVSE, EventTracking_Id>?                     OnSuccess                      = null,

                                                             Boolean                                              SkipAddedNotifications         = false,
                                                             Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                             EventTracking_Id?                                    EventTrackingId                = null,
                                                             User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.AddEVSEIfNotExists(new EVSE(
                                                      Id,
                                                      ChargingStation,
                                                      Name,
                                                      Description,

                                                      PhotoURLs,
                                                      Brands,
                                                      MobilityRootCAs,
                                                      OpenDataLicenses,
                                                      ChargingModes,
                                                      ChargingTariffs,
                                                      CurrentType,
                                                      AverageVoltage,
                                                      AverageVoltageRealTime,
                                                      AverageVoltagePrognoses,
                                                      MaxCurrent,
                                                      MaxCurrentRealTime,
                                                      MaxCurrentPrognoses,
                                                      MaxPower,
                                                      MaxPowerRealTime,
                                                      MaxPowerPrognoses,
                                                      MaxCapacity,
                                                      MaxCapacityRealTime,
                                                      MaxCapacityPrognoses,
                                                      EnergyMix,
                                                      EnergyMixRealTime,
                                                      EnergyMixPrognoses,
                                                      EnergyMeter,
                                                      IsFreeOfCharge,
                                                      ChargingConnectors,
                                                      ChargingSession,

                                                      InitialAdminStatus,
                                                      InitialStatus,
                                                      MaxAdminStatusScheduleSize,
                                                      MaxStatusScheduleSize,
                                                      LastStatusUpdate,

                                                      DataSource,
                                                      LastChange,

                                                      CustomData,
                                                      InternalData,

                                                      Configurator,
                                                      RemoteEVSECreator
                                                  ),

                                                  OnSuccess,

                                                  SkipAddedNotifications,
                                                  AllowInconsistentOperatorIds,
                                                  EventTrackingId,
                                                  CurrentUserId);

        #endregion

        #region AddOrUpdateEVSE   (this IChargingStation, Id, ..., Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new or update an existing EVSE.
        /// </summary>
        /// <param name="ChargingStation">The charging station of the new or EVSE.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(this IChargingStation                                ChargingStation,

                                                                  EVSE_Id                                              Id,
                                                                  I18NString?                                          Name                                   = null,
                                                                  I18NString?                                          Description                            = null,

                                                                  Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus                     = null,
                                                                  Timestamped<EVSEStatusType>?                        InitialStatus                          = null,
                                                                  UInt16?                                              MaxAdminStatusScheduleSize             = null,
                                                                  UInt16?                                              MaxStatusScheduleSize                  = null,

                                                                  IEnumerable<URL>?                                    PhotoURLs                              = null,
                                                                  IEnumerable<Brand>?                                  Brands                                 = null,
                                                                  IEnumerable<RootCAInfo>?                             MobilityRootCAs                        = null,
                                                                  IEnumerable<OpenDataLicense>?                        OpenDataLicenses                       = null,
                                                                  IEnumerable<ChargingModes>?                          ChargingModes                          = null,
                                                                  IEnumerable<ChargingTariff>?                         ChargingTariffs                        = null,
                                                                  CurrentTypes?                                        CurrentType                            = null,
                                                                  Volt?                                                AverageVoltage                         = null,
                                                                  Timestamped<Volt>?                                   AverageVoltageRealTime                 = null,
                                                                  IEnumerable<Timestamped<Volt>>?                      AverageVoltagePrognoses                = null,
                                                                  Ampere?                                              MaxCurrent                             = null,
                                                                  Timestamped<Ampere>?                                 MaxCurrentRealTime                     = null,
                                                                  IEnumerable<Timestamped<Ampere>>?                    MaxCurrentPrognoses                    = null,
                                                                  Watt?                                                MaxPower                               = null,
                                                                  Timestamped<Watt>?                                   MaxPowerRealTime                       = null,
                                                                  IEnumerable<Timestamped<Watt>>?                      MaxPowerPrognoses                      = null,
                                                                  WattHour?                                            MaxCapacity                            = null,
                                                                  Timestamped<WattHour>?                               MaxCapacityRealTime                    = null,
                                                                  IEnumerable<Timestamped<WattHour>>?                  MaxCapacityPrognoses                   = null,
                                                                  EnergyMix?                                           EnergyMix                              = null,
                                                                  Timestamped<EnergyMix>?                              EnergyMixRealTime                      = null,
                                                                  EnergyMixPrognosis?                                  EnergyMixPrognoses                     = null,
                                                                  EnergyMeter?                                         EnergyMeter                            = null,
                                                                  Boolean?                                             IsFreeOfCharge                         = null,
                                                                  IEnumerable<IChargingConnector>?                     ChargingConnectors                     = null,

                                                                  ChargingSession?                                     ChargingSession                        = null,
                                                                  DateTime?                                            LastStatusUpdate                       = null,

                                                                  String?                                              DataSource                             = null,
                                                                  DateTime?                                            LastChange                             = null,

                                                                  JObject?                                             CustomData                             = null,
                                                                  UserDefinedDictionary?                               InternalData                           = null,

                                                                  Action<IEVSE>?                                       Configurator                           = null,
                                                                  RemoteEVSECreatorDelegate?                           RemoteEVSECreator                      = null,

                                                                  Action<IEVSE,                   EventTracking_Id>?   OnAdditionSuccess                      = null,
                                                                  Action<IEVSE,            IEVSE, EventTracking_Id>?   OnUpdateSuccess                        = null,
                                                                  Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                                = null,

                                                                  Boolean                                              SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                  Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds           = null,
                                                                  EventTracking_Id?                                    EventTrackingId                        = null,
                                                                  User_Id?                                             CurrentUserId                          = null)

            => ChargingStation.AddOrUpdateEVSE(new EVSE(
                                                   Id,
                                                   ChargingStation,
                                                   Name,
                                                   Description,

                                                   PhotoURLs,
                                                   Brands,
                                                   MobilityRootCAs,
                                                   OpenDataLicenses,
                                                   ChargingModes,
                                                   ChargingTariffs,
                                                   CurrentType,
                                                   AverageVoltage,
                                                   AverageVoltageRealTime,
                                                   AverageVoltagePrognoses,
                                                   MaxCurrent,
                                                   MaxCurrentRealTime,
                                                   MaxCurrentPrognoses,
                                                   MaxPower,
                                                   MaxPowerRealTime,
                                                   MaxPowerPrognoses,
                                                   MaxCapacity,
                                                   MaxCapacityRealTime,
                                                   MaxCapacityPrognoses,
                                                   EnergyMix,
                                                   EnergyMixRealTime,
                                                   EnergyMixPrognoses,
                                                   EnergyMeter,
                                                   IsFreeOfCharge,
                                                   ChargingConnectors,
                                                   ChargingSession,

                                                   InitialAdminStatus,
                                                   InitialStatus,
                                                   MaxAdminStatusScheduleSize,
                                                   MaxStatusScheduleSize,
                                                   LastStatusUpdate,

                                                   DataSource,
                                                   LastChange,

                                                   CustomData,
                                                   InternalData,

                                                   Configurator,
                                                   RemoteEVSECreator
                                               ),

                                               OnAdditionSuccess,
                                               OnUpdateSuccess,
                                               OnError,

                                               SkipAddOrUpdatedUpdatedNotifications,
                                               AllowInconsistentOperatorIds,
                                               EventTrackingId,
                                               CurrentUserId);

        #endregion

        #region UpdateEVSE        (this IChargingStation, Id, ..., Configurator = null, OnAdditionSuccess = null, OnUpdateSuccess = null, OnError = null)

        /// <summary>
        /// Update the given EVSE.
        /// </summary>
        /// <param name="ChargingStation">The charging station of the updated EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful update of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<UpdateEVSEResult> UpdateEVSE(this IChargingStation                                ChargingStation,

                                                        EVSE_Id                                              Id,
                                                        I18NString?                                          Name                           = null,
                                                        I18NString?                                          Description                    = null,

                                                        IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                        IEnumerable<Brand>?                                  Brands                         = null,
                                                        IEnumerable<RootCAInfo>?                             MobilityRootCAs                = null,
                                                        IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                        IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                        IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                        CurrentTypes?                                        CurrentType                    = null,
                                                        Volt?                                                AverageVoltage                 = null,
                                                        Timestamped<Volt>?                                   AverageVoltageRealTime         = null,
                                                        IEnumerable<Timestamped<Volt>>?                      AverageVoltagePrognoses        = null,
                                                        Ampere?                                              MaxCurrent                     = null,
                                                        Timestamped<Ampere>?                                 MaxCurrentRealTime             = null,
                                                        IEnumerable<Timestamped<Ampere>>?                    MaxCurrentPrognoses            = null,
                                                        Watt?                                                MaxPower                       = null,
                                                        Timestamped<Watt>?                                   MaxPowerRealTime               = null,
                                                        IEnumerable<Timestamped<Watt>>?                      MaxPowerPrognoses              = null,
                                                        WattHour?                                            MaxCapacity                    = null,
                                                        Timestamped<WattHour>?                               MaxCapacityRealTime            = null,
                                                        IEnumerable<Timestamped<WattHour>>?                  MaxCapacityPrognoses           = null,
                                                        EnergyMix?                                           EnergyMix                      = null,
                                                        Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                        EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                        EnergyMeter?                                         EnergyMeter                    = null,
                                                        Boolean?                                             IsFreeOfCharge                 = null,
                                                        IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,
                                                        ChargingSession?                                     ChargingSession                = null,

                                                        Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                        Timestamped<EVSEStatusType>?                        InitialStatus                  = null,
                                                        UInt16?                                              MaxAdminStatusScheduleSize     = null,
                                                        UInt16?                                              MaxStatusScheduleSize          = null,
                                                        DateTime?                                            LastStatusUpdate               = null,

                                                        String?                                              DataSource                     = null,
                                                        DateTime?                                            LastChange                     = null,

                                                        JObject?                                             CustomData                     = null,
                                                        UserDefinedDictionary?                               InternalData                   = null,

                                                        Action<IEVSE>?                                       Configurator                   = null,
                                                        RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                        Action<IEVSE,            IEVSE, EventTracking_Id>?   OnSuccess                      = null,
                                                        Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                        = null,

                                                        Boolean                                              SkipUpdatedNotifications       = false,
                                                        Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                        EventTracking_Id?                                    EventTrackingId                = null,
                                                        User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.UpdateEVSE(new EVSE(
                                              Id,
                                              ChargingStation,
                                              Name,
                                              Description,

                                              PhotoURLs,
                                              Brands,
                                              MobilityRootCAs,
                                              OpenDataLicenses,
                                              ChargingModes,
                                              ChargingTariffs,
                                              CurrentType,
                                              AverageVoltage,
                                              AverageVoltageRealTime,
                                              AverageVoltagePrognoses,
                                              MaxCurrent,
                                              MaxCurrentRealTime,
                                              MaxCurrentPrognoses,
                                              MaxPower,
                                              MaxPowerRealTime,
                                              MaxPowerPrognoses,
                                              MaxCapacity,
                                              MaxCapacityRealTime,
                                              MaxCapacityPrognoses,
                                              EnergyMix,
                                              EnergyMixRealTime,
                                              EnergyMixPrognoses,
                                              EnergyMeter,
                                              IsFreeOfCharge,
                                              ChargingConnectors,
                                              ChargingSession,

                                              InitialAdminStatus,
                                              InitialStatus,
                                              MaxAdminStatusScheduleSize,
                                              MaxStatusScheduleSize,
                                              LastStatusUpdate,

                                              DataSource,
                                              LastChange,

                                              CustomData,
                                              InternalData,

                                              Configurator,
                                              RemoteEVSECreator
                                          ),

                                          OnSuccess,
                                          OnError,

                                          SkipUpdatedNotifications,
                                          AllowInconsistentOperatorIds,
                                          EventTrackingId,
                                          CurrentUserId);

        #endregion


        #region ToJSON(this ChargingStations, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="Skip">The optional number of charging stations to skip.</param>
        /// <param name="Take">The optional number of charging stations to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging pool.</param>
        public static JArray ToJSON(this IEnumerable<IChargingStation>                   ChargingStations,
                                    UInt64?                                              Skip                                = null,
                                    UInt64?                                              Take                                = null,
                                    Boolean                                              Embedded                            = false,
                                    InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Expanded,
                                    InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                                    CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                                    CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)


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
                                                                             CustomEVSESerializer,
                                                                             CustomChargingConnectorSerializer)).
                                    Where         (station => station is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The common interface of all charging stations.
    /// </summary>
    public interface IChargingStation : IEntity<ChargingStation_Id>,
                                        IAdminStatus<ChargingStationAdminStatusTypes>,
                                        IStatus<ChargingStationStatusTypes>,
                                        ISendAuthorizeStartStop,
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
        IChargingStationOperator?               Operator                    { get; }

        /// <summary>
        /// The charging station sub operator of this charging station.
        /// </summary>
        IChargingStationOperator?               SubOperator                 { get; }

        /// <summary>
        /// The charging pool of this charging station.
        /// </summary>
        IChargingPool?                          ChargingPool                { get; }

        /// <summary>
        /// An optional remote charging station.
        /// </summary>
        [Optional]
        IRemoteChargingStation?                 RemoteChargingStation       { get; }


        Boolean                                 Published                   { get; }

        Boolean                                 Disabled                    { get; }


        /// <summary>
        /// All brands registered for this charging station.
        /// </summary>
        ReactiveSet<Brand>                      Brands                      { get; }

        /// <summary>
        /// The license of the charging station data.
        /// </summary>
        ReactiveSet<OpenDataLicense>            DataLicenses                { get; }


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
        /// User interface features of the charging station.
        /// </summary>
        ReactiveSet<UIFeatures>                 UIFeatures                  { get; }

        /// <summary>
        /// The authentication options an EV driver can use.
        /// </summary>
        ReactiveSet<AuthenticationModes>        AuthenticationModes         { get; }

        /// <summary>
        /// The payment options an EV driver can use.
        /// </summary>
        ReactiveSet<PaymentOptions>             PaymentOptions              { get; }

        /// <summary>
        /// The accessibility of the charging station.
        /// </summary>
        AccessibilityTypes?                     Accessibility               { get; set; }

        /// <summary>
        /// Charging features of the charging station.
        /// </summary>
        ReactiveSet<ChargingStationFeature>     Features                    { get; }


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
        event OnChargingStationDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        event OnChargingStationStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        event OnChargingStationAdminStatusChangedDelegate?  OnAdminStatusChanged;

        #endregion


        #region EVSEs

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>         OnEVSEAddition    { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean>  OnEVSEUpdate      { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean>         OnEVSERemoval     { get; }



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
        IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>>

            EVSEStatusSchedule(IncludeEVSEDelegate?             IncludeEVSEs      = null,
                               Func<DateTime,        Boolean>?  TimestampFilter   = null,
                               Func<EVSEStatusType, Boolean>?  StatusFilter      = null,
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
        /// Add a new EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddEVSEResult> AddEVSE(IEVSE                                                EVSE,

                                    Action<IEVSE,                   EventTracking_Id>?   OnSuccess                      = null,
                                    Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                        = null,

                                    Boolean                                              SkipAddedNotifications         = false,
                                    Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                    EventTracking_Id?                                    EventTrackingId                = null,
                                    User_Id?                                             CurrentUserId                  = null);


        /// <summary>
        /// Add a new EVSE, but do not fail when this EVSE already exists.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddEVSEResult> AddEVSEIfNotExists(IEVSE                                                EVSE,

                                               Action<IEVSE, EventTracking_Id>?                     OnSuccess                      = null,

                                               Boolean                                              SkipAddedNotifications         = false,
                                               Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                               EventTracking_Id?                                    EventTrackingId                = null,
                                               User_Id?                                             CurrentUserId                  = null);


        /// <summary>
        /// Add a new or update an existing EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the EVSE.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(IEVSE                                                EVSE,

                                                    Action<IEVSE,                   EventTracking_Id>?   OnAdditionSuccess                      = null,
                                                    Action<IEVSE,            IEVSE, EventTracking_Id>?   OnUpdateSuccess                        = null,
                                                    Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                                = null,

                                                    Boolean                                              SkipAddOrUpdatedUpdatedNotifications   = false,
                                                    Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds           = null,
                                                    EventTracking_Id?                                    EventTrackingId                        = null,
                                                    User_Id?                                             CurrentUserId                          = null);


        /// <summary>
        /// Update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful update of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateEVSEResult> UpdateEVSE(IEVSE                                                EVSE,

                                          Action<IEVSE,            IEVSE, EventTracking_Id>?   OnSuccess                      = null,
                                          Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                        = null,

                                          Boolean                                              SkipUpdatedNotifications       = false,
                                          Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                          EventTracking_Id?                                    EventTrackingId                = null,
                                          User_Id?                                             CurrentUserId                  = null);

        /// <summary>
        /// Update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="UpdateDelegate">A delegate for updating the given EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful update of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new EVSE failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateEVSEResult> UpdateEVSE(IEVSE                                                EVSE,
                                          Action<IEVSE>                                        UpdateDelegate,

                                          Action<IEVSE,            IEVSE, EventTracking_Id>?   OnSuccess                      = null,
                                          Action<IChargingStation, IEVSE, EventTracking_Id>?   OnError                        = null,

                                          Boolean                                              SkipUpdatedNotifications       = false,
                                          Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                          EventTracking_Id?                                    EventTrackingId                = null,
                                          User_Id?                                             CurrentUserId                  = null);


        /// <summary>
        /// Remove the given EVSE.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful removal of the EVSE.</param>
        /// <param name="OnError">An optional delegate to be called whenever the removal of the new EVSE failed.</param>
        /// 
        /// <param name="SkipRemovedNotifications">Whether to skip sending the 'OnRemoved' event.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<DeleteEVSEResult> RemoveEVSE(EVSE_Id                                             Id,

                                          Action<IEVSE,                   EventTracking_Id>?  OnSuccess                  = null,
                                          Action<IChargingStation, IEVSE, EventTracking_Id>?  OnError                    = null,

                                          Boolean                                             SkipRemovedNotifications   = false,
                                          EventTracking_Id?                                   EventTrackingId            = null,
                                          User_Id?                                            CurrentUserId              = null);


        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEDataChangedDelegate?         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEStatusChangedDelegate?       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnEVSEAdminStatusChangedDelegate?  OnEVSEAdminStatusChanged;

        #endregion

        //#region ChargingConnectorAddition

        //internal readonly IVotingNotificator<DateTime, EVSE, ChargingConnector, Boolean> ChargingConnectorAddition;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, ChargingConnector, Boolean> OnChargingConnectorAddition

        //    => ChargingConnectorAddition;

        //#endregion

        //#region ChargingConnectorRemoval

        //internal readonly IVotingNotificator<DateTime, EVSE, ChargingConnector, Boolean> ChargingConnectorRemoval;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, EVSE, ChargingConnector, Boolean> OnChargingConnectorRemoval

        //    => ChargingConnectorRemoval;

        //#endregion

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
        JObject ToJSON(Boolean                                              Embedded                            = false,
                       InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                       InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                       InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                       InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Expanded,
                       InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                       InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                       CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                       CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                       CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null);


    }

}
