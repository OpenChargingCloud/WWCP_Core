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
    /// Extension methods for the common charging pool interface.
    /// </summary>
    public static partial class ChargingPoolExtensions
    {

        #region AddChargingStation           (this ChargingPool, Id, ..., Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new charging station.
        /// </summary>
        /// <param name="ChargingPool">The charging pool of the new charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddChargingStationResult> AddChargingStation(this IChargingPool                                              ChargingPool,

                                                                        ChargingStation_Id                                              Id,
                                                                        I18NString?                                                     Name                           = null,
                                                                        I18NString?                                                     Description                    = null,

                                                                        Address?                                                        Address                        = null,
                                                                        GeoCoordinate?                                                  GeoLocation                    = null,
                                                                        OpeningTimes?                                                   OpeningTimes                   = null,
                                                                        Boolean?                                                        ChargingWhenClosed             = null,
                                                                        AccessibilityTypes?                                             Accessibility                  = null,
                                                                        Languages?                                                      LocationLanguage               = null,
                                                                        String?                                                         PhysicalReference              = null,
                                                                        PhoneNumber?                                                    HotlinePhoneNumber             = null,

                                                                        IEnumerable<AuthenticationModes>?                               AuthenticationModes            = null,
                                                                        IEnumerable<PaymentOptions>?                                    PaymentOptions                 = null,
                                                                        IEnumerable<ChargingStationFeature>?                            Features                       = null,

                                                                        String?                                                         ServiceIdentification          = null,
                                                                        String?                                                         ModelCode                      = null,

                                                                        Boolean?                                                        Published                      = null,
                                                                        Boolean?                                                        Disabled                       = null,

                                                                        IEnumerable<Brand>?                                             Brands                         = null,
                                                                        IEnumerable<RootCAInfo>?                                        MobilityRootCAs                = null,

                                                                        Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                        Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                        UInt16?                                                         MaxAdminStatusScheduleSize     = null,
                                                                        UInt16?                                                         MaxStatusScheduleSize          = null,

                                                                        String?                                                         DataSource                     = null,
                                                                        DateTime?                                                       LastChange                     = null,

                                                                        JObject?                                                        CustomData                     = null,
                                                                        UserDefinedDictionary?                                          InternalData                   = null,

                                                                        Action<IChargingStation>?                                       Configurator                   = null,
                                                                        RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,

                                                                        Action<IChargingStation,                EventTracking_Id>?      OnSuccess                      = null,
                                                                        Action<IChargingPool, IChargingStation, EventTracking_Id>?      OnError                        = null,

                                                                        Boolean                                                         SkipAddedNotifications         = false,
                                                                        Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                        EventTracking_Id?                                               EventTrackingId                = null,
                                                                        User_Id?                                                        CurrentUserId                  = null)


            => ChargingPool.AddChargingStation(new ChargingStation(
                                                   Id,
                                                   ChargingPool,
                                                   Name,
                                                   Description,

                                                   Address,
                                                   GeoLocation,
                                                   OpeningTimes,
                                                   ChargingWhenClosed,
                                                   Accessibility,
                                                   LocationLanguage,
                                                   PhysicalReference,
                                                   HotlinePhoneNumber,

                                                   AuthenticationModes,
                                                   PaymentOptions,
                                                   Features,

                                                   ServiceIdentification,
                                                   ModelCode,

                                                   Published,
                                                   Disabled,

                                                   Brands,
                                                   MobilityRootCAs,

                                                   InitialAdminStatus,
                                                   InitialStatus,
                                                   MaxAdminStatusScheduleSize,
                                                   MaxStatusScheduleSize,

                                                   DataSource,
                                                   LastChange,

                                                   CustomData,
                                                   InternalData,

                                                   Configurator,
                                                   RemoteChargingStationCreator
                                               ),

                                               OnSuccess,
                                               OnError,

                                               SkipAddedNotifications,
                                               AllowInconsistentOperatorIds,
                                               EventTrackingId,
                                               CurrentUserId);

        #endregion

        #region AddChargingStationIfNotExists(this ChargingPool, Id, ..., Configurator = null, OnSuccess = null)

        /// <summary>
        /// Add a new charging station, but do not fail when this charging station already exists.
        /// </summary>
        /// <param name="ChargingPool">The charging pool of the new charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddChargingStationResult> AddChargingStationIfNotExists(this IChargingPool                                              ChargingPool,

                                                                                   ChargingStation_Id                                              Id,
                                                                                   I18NString?                                                     Name                           = null,
                                                                                   I18NString?                                                     Description                    = null,

                                                                                   Address?                                                        Address                        = null,
                                                                                   GeoCoordinate?                                                  GeoLocation                    = null,
                                                                                   OpeningTimes?                                                   OpeningTimes                   = null,
                                                                                   Boolean?                                                        ChargingWhenClosed             = null,
                                                                                   AccessibilityTypes?                                             Accessibility                  = null,
                                                                                   Languages?                                                      LocationLanguage               = null,
                                                                                   String?                                                         PhysicalReference              = null,
                                                                                   PhoneNumber?                                                    HotlinePhoneNumber             = null,

                                                                                   IEnumerable<AuthenticationModes>?                               AuthenticationModes            = null,
                                                                                   IEnumerable<PaymentOptions>?                                    PaymentOptions                 = null,
                                                                                   IEnumerable<ChargingStationFeature>?                            Features                       = null,

                                                                                   String?                                                         ServiceIdentification          = null,
                                                                                   String?                                                         ModelCode                      = null,

                                                                                   Boolean?                                                        Published                      = null,
                                                                                   Boolean?                                                        Disabled                       = null,

                                                                                   IEnumerable<Brand>?                                             Brands                         = null,
                                                                                   IEnumerable<RootCAInfo>?                                        MobilityRootCAs                = null,

                                                                                   Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                                   Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                                   UInt16?                                                         MaxAdminStatusScheduleSize     = null,
                                                                                   UInt16?                                                         MaxStatusScheduleSize          = null,

                                                                                   String?                                                         DataSource                     = null,
                                                                                   DateTime?                                                       LastChange                     = null,

                                                                                   JObject?                                                        CustomData                     = null,
                                                                                   UserDefinedDictionary?                                          InternalData                   = null,

                                                                                   Action<IChargingStation>?                                       Configurator                   = null,
                                                                                   RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,

                                                                                   Action<IChargingStation, EventTracking_Id>?                     OnSuccess                      = null,

                                                                                   Boolean                                                         SkipAddedNotifications         = false,
                                                                                   Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                                   EventTracking_Id?                                               EventTrackingId                = null,
                                                                                   User_Id?                                                        CurrentUserId                  = null)


            => ChargingPool.AddChargingStationIfNotExists(new ChargingStation(
                                                              Id,
                                                              ChargingPool,
                                                              Name,
                                                              Description,

                                                              Address,
                                                              GeoLocation,
                                                              OpeningTimes,
                                                              ChargingWhenClosed,
                                                              Accessibility,
                                                              LocationLanguage,
                                                              PhysicalReference,
                                                              HotlinePhoneNumber,

                                                              AuthenticationModes,
                                                              PaymentOptions,
                                                              Features,

                                                              ServiceIdentification,
                                                              ModelCode,

                                                              Published,
                                                              Disabled,

                                                              Brands,
                                                              MobilityRootCAs,

                                                              InitialAdminStatus,
                                                              InitialStatus,
                                                              MaxAdminStatusScheduleSize,
                                                              MaxStatusScheduleSize,

                                                              DataSource,
                                                              LastChange,

                                                              CustomData,
                                                              InternalData,

                                                              Configurator,
                                                              RemoteChargingStationCreator
                                                          ),

                                                          OnSuccess,

                                                          SkipAddedNotifications,
                                                          AllowInconsistentOperatorIds,
                                                          EventTrackingId,
                                                          CurrentUserId);

        #endregion

        #region AddOrUpdateChargingStation   (this ChargingPool, Id, ..., Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new or update an existing charging station.
        /// </summary>
        /// <param name="ChargingPool">The charging pool of the new or updated charging station.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(this IChargingPool                                              ChargingPool,

                                                                                        ChargingStation_Id                                              Id,
                                                                                        I18NString?                                                     Name                                   = null,
                                                                                        I18NString?                                                     Description                            = null,

                                                                                        Address?                                                        Address                                = null,
                                                                                        GeoCoordinate?                                                  GeoLocation                            = null,
                                                                                        OpeningTimes?                                                   OpeningTimes                           = null,
                                                                                        Boolean?                                                        ChargingWhenClosed                     = null,
                                                                                        AccessibilityTypes?                                             Accessibility                          = null,
                                                                                        Languages?                                                      LocationLanguage                       = null,
                                                                                        String?                                                         PhysicalReference                      = null,
                                                                                        PhoneNumber?                                                    HotlinePhoneNumber                     = null,

                                                                                        IEnumerable<AuthenticationModes>?                               AuthenticationModes                    = null,
                                                                                        IEnumerable<PaymentOptions>?                                    PaymentOptions                         = null,
                                                                                        IEnumerable<ChargingStationFeature>?                            Features                               = null,

                                                                                        String?                                                         ServiceIdentification                  = null,
                                                                                        String?                                                         ModelCode                              = null,

                                                                                        Boolean?                                                        Published                      = null,
                                                                                        Boolean?                                                        Disabled                       = null,

                                                                                        IEnumerable<Brand>?                                             Brands                                 = null,
                                                                                        IEnumerable<RootCAInfo>?                                        MobilityRootCAs                        = null,

                                                                                        Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus                     = null,
                                                                                        Timestamped<ChargingStationStatusTypes>?                        InitialStatus                          = null,
                                                                                        UInt16?                                                         MaxAdminStatusScheduleSize             = null,
                                                                                        UInt16?                                                         MaxStatusScheduleSize                  = null,

                                                                                        String?                                                         DataSource                             = null,
                                                                                        DateTime?                                                       LastChange                             = null,

                                                                                        JObject?                                                        CustomData                             = null,
                                                                                        UserDefinedDictionary?                                          InternalData                           = null,

                                                                                        Action<IChargingStation>?                                       Configurator                           = null,
                                                                                        RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator           = null,

                                                                                        Action<IChargingStation,                   EventTracking_Id>?   OnAdditionSuccess                      = null,
                                                                                        Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                        = null,
                                                                                        Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                                = null,

                                                                                        Boolean                                                         SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                                        Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds           = null,
                                                                                        EventTracking_Id?                                               EventTrackingId                        = null,
                                                                                        User_Id?                                                        CurrentUserId                          = null)

            => ChargingPool.AddOrUpdateChargingStation(new ChargingStation(
                                                           Id,
                                                           ChargingPool,
                                                           Name,
                                                           Description,

                                                           Address,
                                                           GeoLocation,
                                                           OpeningTimes,
                                                           ChargingWhenClosed,
                                                           Accessibility,
                                                           LocationLanguage,
                                                           PhysicalReference,
                                                           HotlinePhoneNumber,

                                                           AuthenticationModes,
                                                           PaymentOptions,
                                                           Features,

                                                           ServiceIdentification,
                                                           ModelCode,

                                                           Published,
                                                           Disabled,

                                                           Brands,
                                                           MobilityRootCAs,

                                                           InitialAdminStatus,
                                                           InitialStatus,
                                                           MaxAdminStatusScheduleSize,
                                                           MaxStatusScheduleSize,

                                                           DataSource,
                                                           LastChange,

                                                           CustomData,
                                                           InternalData,

                                                           Configurator,
                                                           RemoteChargingStationCreator
                                                       ),

                                                       OnAdditionSuccess,
                                                       OnUpdateSuccess,
                                                       OnError,

                                                       SkipAddOrUpdatedUpdatedNotifications,
                                                       AllowInconsistentOperatorIds,
                                                       EventTrackingId,
                                                       CurrentUserId);

        #endregion

        #region UpdateChargingStation        (this ChargingPool, Id, ..., Configurator = null, OnAdditionSuccess = null, OnUpdateSuccess = null, OnError = null)

        /// <summary>
        /// Update the given charging station.
        /// </summary>
        /// <param name="ChargingPool">The charging pool of the updated charging station.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<UpdateChargingStationResult> UpdateChargingStation(this IChargingPool                                              ChargingPool,

                                                                              ChargingStation_Id                                              Id,
                                                                              I18NString?                                                     Name                           = null,
                                                                              I18NString?                                                     Description                    = null,

                                                                              Address?                                                        Address                        = null,
                                                                              GeoCoordinate?                                                  GeoLocation                    = null,
                                                                              OpeningTimes?                                                   OpeningTimes                   = null,
                                                                              Boolean?                                                        ChargingWhenClosed             = null,
                                                                              AccessibilityTypes?                                             Accessibility                  = null,
                                                                              Languages?                                                      LocationLanguage               = null,
                                                                              String?                                                         PhysicalReference              = null,
                                                                              PhoneNumber?                                                    HotlinePhoneNumber             = null,

                                                                              IEnumerable<AuthenticationModes>?                               AuthenticationModes            = null,
                                                                              IEnumerable<PaymentOptions>?                                    PaymentOptions                 = null,
                                                                              IEnumerable<ChargingStationFeature>?                            Features                       = null,

                                                                              String?                                                         ServiceIdentification          = null,
                                                                              String?                                                         ModelCode                      = null,

                                                                              Boolean?                                                        Published                      = null,
                                                                              Boolean?                                                        Disabled                       = null,

                                                                              IEnumerable<Brand>?                                             Brands                         = null,
                                                                              IEnumerable<RootCAInfo>?                                        MobilityRootCAs                = null,

                                                                              Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                              Timestamped<ChargingStationStatusTypes>?                        InitialStatus                  = null,
                                                                              UInt16?                                                         MaxAdminStatusScheduleSize     = null,
                                                                              UInt16?                                                         MaxStatusScheduleSize          = null,

                                                                              String?                                                         DataSource                     = null,
                                                                              DateTime?                                                       LastChange                     = null,

                                                                              JObject?                                                        CustomData                     = null,
                                                                              UserDefinedDictionary?                                          InternalData                   = null,

                                                                              Action<IChargingStation>?                                       Configurator                   = null,
                                                                              RemoteChargingStationCreatorDelegate?                           RemoteChargingStationCreator   = null,

                                                                              Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                = null,
                                                                              Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                        = null,

                                                                              Boolean                                                         SkipUpdatedNotifications       = false,
                                                                              Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                              EventTracking_Id?                                               EventTrackingId                = null,
                                                                              User_Id?                                                        CurrentUserId                  = null)

            => ChargingPool.UpdateChargingStation(new ChargingStation(
                                                      Id,
                                                      ChargingPool,
                                                      Name,
                                                      Description,

                                                      Address,
                                                      GeoLocation,
                                                      OpeningTimes,
                                                      ChargingWhenClosed,
                                                      Accessibility,
                                                      LocationLanguage,
                                                      PhysicalReference,
                                                      HotlinePhoneNumber,

                                                      AuthenticationModes,
                                                      PaymentOptions,
                                                      Features,

                                                      ServiceIdentification,
                                                      ModelCode,

                                                      Published,
                                                      Disabled,

                                                      Brands,
                                                      MobilityRootCAs,

                                                      InitialAdminStatus,
                                                      InitialStatus,
                                                      MaxAdminStatusScheduleSize,
                                                      MaxStatusScheduleSize,

                                                      DataSource,
                                                      LastChange,

                                                      CustomData,
                                                      InternalData,

                                                      Configurator,
                                                      RemoteChargingStationCreator
                                                  ),

                                                  OnUpdateSuccess,
                                                  OnError,

                                                  SkipUpdatedNotifications,
                                                  AllowInconsistentOperatorIds,
                                                  EventTrackingId,
                                                  CurrentUserId);

        #endregion


        #region ToJSON(this ChargingPools, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="Skip">The optional number of charging pools to skip.</param>
        /// <param name="Take">The optional number of charging pools to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station operator.</param>
        public static JArray ToJSON(this IEnumerable<IChargingPool>                      ChargingPools,
                                    UInt64?                                              Skip                                = null,
                                    UInt64?                                              Take                                = null,
                                    Boolean                                              Embedded                            = false,
                                    InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandChargingStationIds            = InfoStatus.Expanded,
                                    InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Hidden,
                                    InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                                    InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<IChargingPool>?      CustomChargingPoolSerializer        = null,
                                    CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                                    CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                                    CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)


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
                                                                       CustomEVSESerializer,
                                                                       CustomChargingConnectorSerializer)).
                                    Where         (pool => pool is not null))

                   : new JArray();


        #endregion

    }


    /// <summary>
    /// The common interface of all charging pools.
    /// </summary>
    public interface IChargingPool : IEntity<ChargingPool_Id>,
                                     IAdminStatus<ChargingPoolAdminStatusTypes>,
                                     IStatus<ChargingPoolStatusTypes>,
                                     ISendAuthorizeStartStop,
                                     ILocalRemoteStartStop,
                                     ILocalChargingReservations,
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
        IChargingStationOperator?               Operator                    { get; }

        /// <summary>
        /// The charging station sub operator of this charging pool.
        /// </summary>
        [Optional]
        IChargingStationOperator?               SubOperator                 { get; }

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
        /// The time zone of this charging pool.
        /// </summary>
        [Optional]
        Time_Zone?                              TimeZone                    { get; set; }

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
        ReactiveSet<ChargingPoolFeature>        Features                    { get; }

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
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean>                    OnChargingStationAddition    { get; }

        /// <summary>
        /// Called whenever a charging station will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, IChargingStation, Boolean>  OnChargingStationUpdate      { get; }

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingPool, IChargingStation, Boolean>                    OnChargingStationRemoval     { get; }


        /// <summary>
        /// Add a new charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddChargingStationResult> AddChargingStation(IChargingStation                                                ChargingStation,

                                                          Action<IChargingStation,                EventTracking_Id>?      OnSuccess                      = null,
                                                          Action<IChargingPool, IChargingStation, EventTracking_Id>?      OnError                        = null,

                                                          Boolean                                                         SkipAddedNotifications         = false,
                                                          Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                          EventTracking_Id?                                               EventTrackingId                = null,
                                                          User_Id?                                                        CurrentUserId                  = null);


        /// <summary>
        /// Add a new charging station, but do not fail when this charging station already exists.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddChargingStationResult> AddChargingStationIfNotExists(IChargingStation                                                ChargingStation,

                                                                     Action<IChargingStation, EventTracking_Id>?                     OnSuccess                      = null,

                                                                     Boolean                                                         SkipAddedNotifications         = false,
                                                                     Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                     EventTracking_Id?                                               EventTrackingId                = null,
                                                                     User_Id?                                                        CurrentUserId                  = null);


        /// <summary>
        /// Add a new or update an existing charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="OnAdditionSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipAddOrUpdatedUpdatedNotifications">Whether to skip sending the 'OnAddedOrUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<AddOrUpdateChargingStationResult> AddOrUpdateChargingStation(IChargingStation                                                ChargingStation,

                                                                          Action<IChargingStation,                   EventTracking_Id>?   OnAdditionSuccess                      = null,
                                                                          Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                        = null,
                                                                          Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                                = null,

                                                                          Boolean                                                         SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                          Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds           = null,
                                                                          EventTracking_Id?                                               EventTrackingId                        = null,
                                                                          User_Id?                                                        CurrentUserId                          = null);


        /// <summary>
        /// Update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation                                                ChargingStation,

                                                                Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                = null,
                                                                Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                        = null,

                                                                Boolean                                                         SkipUpdatedNotifications       = false,
                                                                Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                EventTracking_Id?                                               EventTrackingId                = null,
                                                                User_Id?                                                        CurrentUserId                  = null);

        /// <summary>
        /// Update the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="UpdateDelegate">A delegate for updating the given charging station.</param>
        /// 
        /// <param name="OnUpdateSuccess">An optional delegate to be called after the successful update of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the addition of the new charging station failed.</param>
        /// 
        /// <param name="SkipUpdatedNotifications">Whether to skip sending the 'OnUpdated' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<UpdateChargingStationResult> UpdateChargingStation(IChargingStation                                                ChargingStation,
                                                                Action<IChargingStation>                                        UpdateDelegate,

                                                                Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                = null,
                                                                Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                        = null,

                                                                Boolean                                                         SkipUpdatedNotifications       = false,
                                                                Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                EventTracking_Id?                                               EventTrackingId                = null,
                                                                User_Id?                                                        CurrentUserId                  = null);


        /// <summary>
        /// Remove the given charging station.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful removal of the charging station.</param>
        /// <param name="OnError">An optional delegate to be called whenever the removal of the new charging station failed.</param>
        /// 
        /// <param name="SkipRemovedNotifications">Whether to skip sending the 'OnRemoved' event.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        Task<DeleteChargingStationResult> RemoveChargingStation(ChargingStation_Id                                          Id,

                                                                Action<IChargingStation,                EventTracking_Id>?  OnSuccess                  = null,
                                                                Action<IChargingPool, IChargingStation, EventTracking_Id>?  OnError                    = null,

                                                                Boolean                                                     SkipRemovedNotifications   = false,
                                                                EventTracking_Id?                                           EventTrackingId            = null,
                                                                User_Id?                                                    CurrentUserId              = null);


        /// <summary>
        /// Clone this charging pool.
        /// </summary>
        ChargingPool Clone();


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



        /// <summary>
        /// An event fired whenever the static data of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationDataChangedDelegate?         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationStatusChangedDelegate?       OnChargingStationStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        /// </summary>
        event OnChargingStationAdminStatusChangedDelegate?  OnChargingStationAdminStatusChanged;


        Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? ChargingStation);

        #endregion

        #region EVSEs

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was updated.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> OnEVSEUpdate { get; }

        /// <summary>
        /// Called whenever an EVSE will be or was removed.
        /// </summary>
        IVotingSender<DateTime, EventTracking_Id, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval { get; }



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

        #endregion

        #region Energy Meters

        /// <summary>
        /// Return all charging stations registered within this charing pool.
        /// </summary>
        IEnumerable<IEnergyMeter> EnergyMeters { get; }

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
        public JObject ToJSON(Boolean                                              Embedded                            = false,
                              InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingStationIds            = InfoStatus.Expanded,
                              InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Hidden,
                              InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingPool>?      CustomChargingPoolSerializer        = null,
                              CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                              CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                              CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null);


    }

}
