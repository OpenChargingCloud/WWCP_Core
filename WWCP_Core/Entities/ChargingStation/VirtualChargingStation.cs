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

using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP.Virtual
{

    /// <summary>
    /// Extension methods for virtual charging stations.
    /// </summary>
    public static class VirtualChargingStationExtensions
    {

        #region AddVirtualStation           (this ChargingPool, ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new virtual charging station.
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
        public static Task<AddChargingStationResult> AddVirtualStation(this IChargingPool                                              ChargingPool,
                                                                       ChargingStation_Id                                              ChargingStationId,
                                                                       I18NString?                                                     Name                                 = null,
                                                                       I18NString?                                                     Description                          = null,

                                                                       Address?                                                        Address                              = null,
                                                                       GeoCoordinate?                                                  GeoLocation                          = null,
                                                                       OpeningTimes?                                                   OpeningTimes                         = null,
                                                                       Boolean?                                                        ChargingWhenClosed                   = null,
                                                                       AccessibilityType?                                              Accessibility                        = null,
                                                                       Languages?                                                      LocationLanguage                     = null,
                                                                       String?                                                         PhysicalReference                    = null,
                                                                       PhoneNumber?                                                    HotlinePhoneNumber                   = null,

                                                                       IEnumerable<AuthenticationModes>?                               AuthenticationModes                  = null,
                                                                       IEnumerable<PaymentOptions>?                                    PaymentOptions                       = null,
                                                                       IEnumerable<ChargingStationFeature>?                            Features                             = null,

                                                                       String?                                                         ServiceIdentification                = null,
                                                                       String?                                                         ModelCode                            = null,

                                                                       Boolean?                                                        Published                            = null,
                                                                       Boolean?                                                        Disabled                             = null,

                                                                       IEnumerable<Brand>?                                             Brands                               = null,
                                                                       IEnumerable<RootCAInfo>?                                        MobilityRootCAs                      = null,

                                                                       Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus                   = null,
                                                                       Timestamped<ChargingStationStatusTypes>?                        InitialStatus                        = null,
                                                                       UInt16?                                                         MaxAdminStatusScheduleSize           = null,
                                                                       UInt16?                                                         MaxStatusScheduleSize                = null,

                                                                       String                                                          EllipticCurve                        = "P-256",
                                                                       ECPrivateKeyParameters?                                         PrivateKey                           = null,
                                                                       PublicKeyCertificates?                                          PublicKeyCertificates                = null,
                                                                       TimeSpan?                                                       SelfCheckTimeSpan                    = null,

                                                                       String?                                                         DataSource                           = null,
                                                                       DateTime?                                                       LastChange                           = null,

                                                                       JObject?                                                        CustomData                           = null,
                                                                       UserDefinedDictionary?                                          InternalData                         = null,

                                                                       Action<IChargingStation>?                                       ChargingStationConfigurator          = null,
                                                                       Action<VirtualChargingStation>?                                 VirtualChargingStationConfigurator   = null,

                                                                       Action<IChargingStation,                EventTracking_Id>?      OnSuccess                            = null,
                                                                       Action<IChargingPool, IChargingStation, EventTracking_Id>?      OnError                              = null,

                                                                       Boolean                                                         SkipAddedNotifications               = false,
                                                                       Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds         = null,
                                                                       EventTracking_Id?                                               EventTrackingId                      = null,
                                                                       User_Id?                                                        CurrentUserId                        = null)

            => ChargingPool.AddChargingStation(

                   ChargingStationId,
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

                   ChargingStationConfigurator,
                   newStation => {

                       var virtualstation = new VirtualChargingStation(
                                                newStation.Id,
                                                ChargingPool.RoamingNetwork,
                                                newStation.Name,
                                                newStation.Description,
                                                InitialAdminStatus ?? ChargingStationAdminStatusTypes.Operational,
                                                InitialStatus      ?? ChargingStationStatusTypes.Available,
                                                EllipticCurve,
                                                PrivateKey,
                                                PublicKeyCertificates,
                                                SelfCheckTimeSpan,
                                                MaxAdminStatusScheduleSize,
                                                MaxStatusScheduleSize
                                            );

                       VirtualChargingStationConfigurator?.Invoke(virtualstation);

                       return virtualstation;

                   },

                   OnSuccess,
                   OnError,

                   SkipAddedNotifications,
                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId

               );

        #endregion

        #region AddVirtualStationIfNotExists(this ChargingPool, ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new virtual charging station, but do not fail when this charging station already exists.
        /// </summary>
        /// <param name="ChargingPool">The charging pool of the new charging station.</param>
        /// 
        /// <param name="OnSuccess">An optional delegate to be called after the successful addition of the charging station.</param>
        /// 
        /// <param name="SkipAddedNotifications">Whether to skip sending the 'OnAdded' event.</param>
        /// <param name="AllowInconsistentOperatorIds">A delegate to decide whether to allow inconsistent charging station operator identifications.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public static Task<AddChargingStationResult> AddVirtualStationIfNotExists(this IChargingPool                                              ChargingPool,
                                                                                  ChargingStation_Id                                              ChargingStationId,
                                                                                  I18NString?                                                     Name                                 = null,
                                                                                  I18NString?                                                     Description                          = null,

                                                                                  Address?                                                        Address                              = null,
                                                                                  GeoCoordinate?                                                  GeoLocation                          = null,
                                                                                  OpeningTimes?                                                   OpeningTimes                         = null,
                                                                                  Boolean?                                                        ChargingWhenClosed                   = null,
                                                                                  AccessibilityType?                                              Accessibility                        = null,
                                                                                  Languages?                                                      LocationLanguage                     = null,
                                                                                  String?                                                         PhysicalReference                    = null,
                                                                                  PhoneNumber?                                                    HotlinePhoneNumber                   = null,

                                                                                  IEnumerable<AuthenticationModes>?                               AuthenticationModes                  = null,
                                                                                  IEnumerable<PaymentOptions>?                                    PaymentOptions                       = null,
                                                                                  IEnumerable<ChargingStationFeature>?                            Features                             = null,

                                                                                  String?                                                         ServiceIdentification                = null,
                                                                                  String?                                                         ModelCode                            = null,

                                                                                  Boolean?                                                        Published                            = null,
                                                                                  Boolean?                                                        Disabled                             = null,

                                                                                  IEnumerable<Brand>?                                             Brands                               = null,
                                                                                  IEnumerable<RootCAInfo>?                                        MobilityRootCAs                      = null,

                                                                                  Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus                   = null,
                                                                                  Timestamped<ChargingStationStatusTypes>?                        InitialStatus                        = null,
                                                                                  UInt16?                                                         MaxAdminStatusScheduleSize           = null,
                                                                                  UInt16?                                                         MaxStatusScheduleSize                = null,

                                                                                  String                                                          EllipticCurve                        = "P-256",
                                                                                  ECPrivateKeyParameters?                                         PrivateKey                           = null,
                                                                                  PublicKeyCertificates?                                          PublicKeyCertificates                = null,
                                                                                  TimeSpan?                                                       SelfCheckTimeSpan                    = null,

                                                                                  String?                                                         DataSource                           = null,
                                                                                  DateTime?                                                       LastChange                           = null,

                                                                                  JObject?                                                        CustomData                           = null,
                                                                                  UserDefinedDictionary?                                          InternalData                         = null,

                                                                                  Action<IChargingStation>?                                       ChargingStationConfigurator          = null,
                                                                                  Action<VirtualChargingStation>?                                 VirtualChargingStationConfigurator   = null,

                                                                                  Action<IChargingStation, EventTracking_Id>?                     OnSuccess                            = null,

                                                                                  Boolean                                                         SkipAddedNotifications               = false,
                                                                                  Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds         = null,
                                                                                  EventTracking_Id?                                               EventTrackingId                      = null,
                                                                                  User_Id?                                                        CurrentUserId                        = null)

            => ChargingPool.AddChargingStationIfNotExists(

                   ChargingStationId,
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

                   ChargingStationConfigurator,
                   newStation => {

                       var virtualstation = new VirtualChargingStation(
                                                newStation.Id,
                                                ChargingPool.RoamingNetwork,
                                                newStation.Name,
                                                newStation.Description,
                                                InitialAdminStatus ?? ChargingStationAdminStatusTypes.Operational,
                                                InitialStatus      ?? ChargingStationStatusTypes.Available,
                                                EllipticCurve,
                                                PrivateKey,
                                                PublicKeyCertificates,
                                                SelfCheckTimeSpan,
                                                MaxAdminStatusScheduleSize,
                                                MaxStatusScheduleSize
                                            );

                       VirtualChargingStationConfigurator?.Invoke(virtualstation);

                       return virtualstation;

                   },

                   OnSuccess,

                   SkipAddedNotifications,
                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId

               );

        #endregion

        #region AddOrUpdateVirtualStation   (this ChargingPool, ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Add a new or update an existing virtual charging station.
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
        public static Task<AddOrUpdateChargingStationResult> AddOrUpdateVirtualStation(this IChargingPool                                              ChargingPool,
                                                                                       ChargingStation_Id                                              ChargingStationId,
                                                                                       I18NString?                                                     Name                                   = null,
                                                                                       I18NString?                                                     Description                            = null,

                                                                                       Address?                                                        Address                                = null,
                                                                                       GeoCoordinate?                                                  GeoLocation                            = null,
                                                                                       OpeningTimes?                                                   OpeningTimes                           = null,
                                                                                       Boolean?                                                        ChargingWhenClosed                     = null,
                                                                                       AccessibilityType?                                              Accessibility                          = null,
                                                                                       Languages?                                                      LocationLanguage                       = null,
                                                                                       String?                                                         PhysicalReference                      = null,
                                                                                       PhoneNumber?                                                    HotlinePhoneNumber                     = null,

                                                                                       IEnumerable<AuthenticationModes>?                               AuthenticationModes                    = null,
                                                                                       IEnumerable<PaymentOptions>?                                    PaymentOptions                         = null,
                                                                                       IEnumerable<ChargingStationFeature>?                            Features                               = null,

                                                                                       String?                                                         ServiceIdentification                  = null,
                                                                                       String?                                                         ModelCode                              = null,

                                                                                       Boolean?                                                        Published                              = null,
                                                                                       Boolean?                                                        Disabled                               = null,

                                                                                       IEnumerable<Brand>?                                             Brands                                 = null,
                                                                                       IEnumerable<RootCAInfo>?                                        MobilityRootCAs                        = null,

                                                                                       Timestamped<ChargingStationAdminStatusTypes>?                   InitialAdminStatus                     = null,
                                                                                       Timestamped<ChargingStationStatusTypes>?                        InitialStatus                          = null,
                                                                                       UInt16?                                                         MaxAdminStatusScheduleSize             = null,
                                                                                       UInt16?                                                         MaxStatusScheduleSize                  = null,

                                                                                       String                                                          EllipticCurve                          = "P-256",
                                                                                       ECPrivateKeyParameters?                                         PrivateKey                             = null,
                                                                                       PublicKeyCertificates?                                          PublicKeyCertificates                  = null,
                                                                                       TimeSpan?                                                       SelfCheckTimeSpan                      = null,

                                                                                       String?                                                         DataSource                             = null,
                                                                                       DateTime?                                                       LastChange                             = null,

                                                                                       JObject?                                                        CustomData                             = null,
                                                                                       UserDefinedDictionary?                                          InternalData                           = null,

                                                                                       Action<IChargingStation>?                                       ChargingStationConfigurator            = null,
                                                                                       Action<VirtualChargingStation>?                                 VirtualChargingStationConfigurator     = null,

                                                                                       Action<IChargingStation,                   EventTracking_Id>?   OnAdditionSuccess                      = null,
                                                                                       Action<IChargingStation, IChargingStation, EventTracking_Id>?   OnUpdateSuccess                        = null,
                                                                                       Action<IChargingPool,    IChargingStation, EventTracking_Id>?   OnError                                = null,

                                                                                       Boolean                                                         SkipAddOrUpdatedUpdatedNotifications   = false,
                                                                                       Func<ChargingStationOperator_Id, ChargingStation_Id, Boolean>?  AllowInconsistentOperatorIds           = null,
                                                                                       EventTracking_Id?                                               EventTrackingId                        = null,
                                                                                       User_Id?                                                        CurrentUserId                          = null)

            => ChargingPool.AddOrUpdateChargingStation(
                   ChargingStationId,
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

                   ChargingStationConfigurator,
                   newStation => {

                       var virtualstation = new VirtualChargingStation(
                                                newStation.Id,
                                                ChargingPool.RoamingNetwork,
                                                newStation.Name,
                                                newStation.Description,
                                                InitialAdminStatus ?? ChargingStationAdminStatusTypes.Operational,
                                                InitialStatus      ?? ChargingStationStatusTypes.Available,
                                                EllipticCurve,
                                                PrivateKey,
                                                PublicKeyCertificates,
                                                SelfCheckTimeSpan,
                                                MaxAdminStatusScheduleSize,
                                                MaxStatusScheduleSize
                                            );

                       VirtualChargingStationConfigurator?.Invoke(virtualstation);

                       return virtualstation;

                   },

                   OnAdditionSuccess,
                   OnUpdateSuccess,
                   OnError,

                   SkipAddOrUpdatedUpdatedNotifications,
                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId

               );

        #endregion

    }


    /// <summary>
    /// A virtual charging station for (internal) tests.
    /// </summary>
    public class VirtualChargingStation : ACryptoEMobilityEntity<ChargingStation_Id,
                                                                 ChargingStationAdminStatusTypes,
                                                                 ChargingStationStatusTypes>,
                                          IEquatable<VirtualChargingStation>, IComparable<VirtualChargingStation>, IComparable,
                                          IRemoteChargingStation
    {

        #region Data

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusScheduleSize = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusScheduleSize = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public  static readonly TimeSpan  MaxReservationDuration    = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public  static readonly TimeSpan  DefaultSelfCheckTimeSpan  = TimeSpan.FromSeconds(15);

        private        readonly Object    ReservationExpiredLock = new ();
        private        readonly Timer     ReservationExpiredTimer;


        public const String DefaultWhiteListName = "default";

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator of this charging station.
        /// </summary>
        public IChargingStationOperator?    Operator                 { get; }

        public IChargingStationOperator?    SubOperator              { get; }

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => Operator?.Id ?? Id.OperatorId;

        public IChargingPool?               ChargingPool             { get; }

        public IChargingStation?            ChargingStation          { get; }

        public IRemoteChargingStation?      RemoteChargingStation    { get; }


        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging station.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {

                return _Description;

            }

            set
            {

                if (value == _Description)
                    return;

                _Description = value;

            }

        }

        #endregion



        public Boolean                       Published                   { get; }

        public Boolean                       Disabled                    { get; }


        /// <summary>
        /// The authentication white lists.
        /// </summary>
        public Boolean                       UseWhiteLists               { get; set; }

        /// <summary>
        /// The authentication white lists.
        /// </summary>
        [InternalUseOnly]
        public Dictionary<String, HashSet<LocalAuthentication>> WhiteLists { get; }

        #region DefaultWhiteList

        /// <summary>
        /// The authentication white lists.
        /// </summary>
        [InternalUseOnly]
        public HashSet<LocalAuthentication> DefaultWhiteList
        {
            get
            {
                return WhiteLists[DefaultWhiteListName];
            }
        }

        #endregion


        /// <summary>
        /// An optional number/string printed on the outside of the EVSE for visual identification.
        /// </summary>
        public String? PhysicalReference { get; }


        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan SelfCheckTimeSpan { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="SelfCheckTimeSpan">The time span between self checks.</param>
        /// <param name="MaxStatusScheduleSize">The maximum size of the charging station status list.</param>
        /// <param name="MaxAdminStatusScheduleSize">The maximum size of the charging station admin status list.</param>
        public VirtualChargingStation(ChargingStation_Id                             Id,
                                      IRoamingNetwork                                RoamingNetwork,
                                      I18NString?                                    Name                     = null,
                                      I18NString?                                    Description              = null,
                                      Timestamped<ChargingStationAdminStatusTypes>?  InitialAdminStatus       = null,
                                      Timestamped<ChargingStationStatusTypes>?       InitialStatus            = null,
                                      String?                                        EllipticCurve            = "P-256",
                                      ECPrivateKeyParameters?                        PrivateKey               = null,
                                      PublicKeyCertificates?                         PublicKeyCertificates    = null,
                                      TimeSpan?                                      SelfCheckTimeSpan        = null,
                                      UInt16?                                        MaxAdminStatusScheduleSize   = null,
                                      UInt16?                                        MaxStatusScheduleSize        = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus     ?? ChargingStationAdminStatusTypes.Operational,
                   InitialStatus          ?? ChargingStationStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize)

        {

            #region Init data and properties

            this._Description          = Description ?? I18NString.Empty;

            this._EVSEs                = new HashSet<IEVSE>();

            this.WhiteLists            = new Dictionary<String, HashSet<LocalAuthentication>>();
            WhiteLists.Add("default", new HashSet<LocalAuthentication>());

            this.SelfCheckTimeSpan     = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;

            ReservationExpiredLock     = new Object();
            ReservationExpiredTimer    = new Timer(CheckIfReservationIsExpired, null, this.SelfCheckTimeSpan, this.SelfCheckTimeSpan);


            this.chargingReservations  = new Dictionary<ChargingReservation_Id, ChargingReservationCollection>();
            this.chargingSessions     = new Dictionary<ChargingSession_Id, ChargingSession>();

            #endregion

            #region Setup crypto

            if (PrivateKey is null && PublicKeyCertificates is null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKeys:          new PublicKeyLifetime[] {
                                                                           new PublicKeyLifetime(
                                                                               PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                               NotBefore:  Timestamp.Now,
                                                                               NotAfter:   Timestamp.Now + TimeSpan.FromDays(365),
                                                                               Algorithm:  "P-256",
                                                                               Comment:    I18NString.Empty
                                                                           )
                                                                       },
                                                  Description:         I18NString.Create("Auto-generated test keys for a virtual charging station!"),
                                                  Operations:          JSONObject.Create(
                                                                           new JProperty("signMeterValues",  true),
                                                                           new JProperty("signCertificates",
                                                                               JSONObject.Create(
                                                                                   new JProperty("maxChilds", 1)
                                                                               ))
                                                                       ),
                                                  ChargingStationId:   Id);

            }

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            this.statusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            #endregion

        }

        #endregion


        public ChargingStation_Id     RemoteChargingStationId    { get; set; }

        public String                 RemoteEVSEIdPrefix         { get; set; }

        public void AddMapping(EVSE_Id LocalEVSEId,
                               EVSE_Id RemoteEVSEId)
        {
        }


        #region (Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnChargingStationDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnChargingStationAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of thße charging station changed.
        /// </summary>
        public event OnChargingStationStatusChangedDelegate?       OnStatusChanged;

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                       Timestamp,
                                              EventTracking_Id                               EventTrackingId,
                                              Timestamped<ChargingStationAdminStatusTypes>   NewStatus,
                                              Timestamped<ChargingStationAdminStatusTypes>?  OldStatus    = null,
                                              Context?                                       DataSource   = null)
        {

            OnAdminStatusChanged?.Invoke(Timestamp,
                                         EventTrackingId,
                                         this,
                                         NewStatus,
                                         OldStatus,
                                         DataSource);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                                  Timestamp,
                                         EventTracking_Id                          EventTrackingId,
                                         Timestamped<ChargingStationStatusTypes>   NewStatus,
                                         Timestamped<ChargingStationStatusTypes>?  OldStatus    = null,
                                         Context?                                  DataSource   = null)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    NewStatus,
                                    OldStatus,
                                    DataSource);

        }

        #endregion

        #endregion


        #region EVSEs

        #region Data

        private readonly HashSet<IEVSE> _EVSEs;

        /// <summary>
        /// All registered EVSEs.
        /// </summary>
        public IEnumerable<IEVSE> EVSEs
            => _EVSEs;

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
            => _EVSEs.Any(evse => evse.Id == EVSEId);

        #endregion

        #region GetEVSEById(EVSEId)

        public IEVSE GetEVSEById(EVSE_Id EVSEId)
            => _EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? EVSE)
        {

            EVSE = GetEVSEById(EVSEId);

            return EVSE != null;

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnEVSEAdminStatusChanged;

        #endregion

        #region (private) AddEVSE(NewRemoteEVSE)

        private Boolean AddNewEVSE(IRemoteEVSE NewRemoteEVSE)
        {

            if (_EVSEs.Add(NewRemoteEVSE))
            {

                NewRemoteEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                NewRemoteEVSE.OnStatusChanged          += UpdateEVSEStatus;

                NewRemoteEVSE.OnNewReservation         += SendNewReservation;
                NewRemoteEVSE.OnReservationCanceled    += SendReservationCanceled;
                NewRemoteEVSE.OnNewChargingSession     += SendNewChargingSession;
                NewRemoteEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                return true;

            }

            return false;

        }

        #endregion

        #region CreateVirtualEVSE(EVSEId, ..., Configurator = null, OnSuccess = null, OnError = null, ...)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public VirtualEVSE? CreateVirtualEVSE(EVSE_Id                              EVSEId,
                                              I18NString?                          Name                         = null,
                                              I18NString?                          Description                  = null,

                                              EVSEAdminStatusTypes?                InitialAdminStatus           = null,
                                              EVSEStatusType?                     InitialStatus                = null,
                                              UInt16                               MaxAdminStatusScheduleSize       = VirtualEVSE.DefaultMaxAdminStatusScheduleSize,
                                              UInt16                               MaxStatusScheduleSize            = VirtualEVSE.DefaultMaxStatusScheduleSize,

                                              IEnumerable<URL>?                    PhotoURLs                    = null,
                                              IEnumerable<Brand>?                  Brands                       = null,
                                              IEnumerable<OpenDataLicense>?        OpenDataLicenses             = null,
                                              IEnumerable<ChargingModes>?          ChargingModes                = null,
                                              IEnumerable<ChargingTariff>?         ChargingTariffs              = null,
                                              CurrentTypes?                        CurrentType                  = null,
                                              Volt?                                AverageVoltage               = null,
                                              Timestamped<Volt>?                   AverageVoltageRealTime       = null,
                                              IEnumerable<Timestamped<Volt>>?      AverageVoltagePrognoses      = null,
                                              Ampere?                              MaxCurrent                   = null,
                                              Timestamped<Ampere>?                 MaxCurrentRealTime           = null,
                                              IEnumerable<Timestamped<Ampere>>?    MaxCurrentPrognoses          = null,
                                              Watt?                                MaxPower                     = null,
                                              Timestamped<Watt>?                   MaxPowerRealTime             = null,
                                              IEnumerable<Timestamped<Watt>>?      MaxPowerPrognoses            = null,
                                              WattHour?                            MaxCapacity                  = null,
                                              Timestamped<WattHour>?               MaxCapacityRealTime          = null,
                                              IEnumerable<Timestamped<WattHour>>?  MaxCapacityPrognoses         = null,
                                              EnergyMix?                           EnergyMix                    = null,
                                              Timestamped<EnergyMix>?              EnergyMixRealTime            = null,
                                              EnergyMixPrognosis?                  EnergyMixPrognoses           = null,
                                              EnergyMeter?                         EnergyMeter                  = null,
                                              Boolean?                             IsFreeOfCharge               = null,
                                              IEnumerable<IChargingConnector>?     ChargingConnectors           = null,

                                              String?                              EllipticCurve                = null,
                                              ECPrivateKeyParameters?              PrivateKey                   = null,
                                              PublicKeyCertificates?               PublicKeyCertificates        = null,
                                              TimeSpan?                            SelfCheckTimeSpan            = null,
                                              Action<VirtualEVSE>?                 Configurator                 = null,
                                              Action<VirtualEVSE>?                 OnSuccess                    = null,
                                              Action<VirtualEVSE, EVSE_Id>?        OnError                      = null)
        {

            #region Initial checks

            if (_EVSEs.Any(evse => evse.Id == EVSEId))
            {
                throw new Exception("EVSEAlreadyExistsInStation");
               // if (OnError == null)
               //     throw new EVSEAlreadyExistsInStation(this.ChargingStation, EVSEId);
               // else
               //     OnError?.Invoke(this, EVSEId);
            }

            #endregion

            var now             = Timestamp.Now;
            var newVirtualEVSE  = new VirtualEVSE(EVSEId,
                                                  this,
                                                  Name,
                                                  Description,

                                                  InitialAdminStatus ?? EVSEAdminStatusTypes.Operational,
                                                  InitialStatus      ?? EVSEStatusType.Available,
                                                  MaxAdminStatusScheduleSize,
                                                  MaxStatusScheduleSize,

                                                  PhotoURLs,
                                                  Brands,
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

                                                  EllipticCurve,
                                                  PrivateKey,
                                                  PublicKeyCertificates,
                                                  SelfCheckTimeSpan);

            Configurator?.Invoke(newVirtualEVSE);

            if (AddNewEVSE(newVirtualEVSE))
            {
                OnSuccess?.Invoke(newVirtualEVSE);
                return newVirtualEVSE;
            }

            return null;

        }

        #endregion

        #region AddEVSE(RemoteEVSE, Configurator = null, OnSuccess = null, OnError = null)

        public IRemoteEVSE AddEVSE(IRemoteEVSE                        RemoteEVSE,
                                   Action<IRemoteEVSE>?               Configurator   = null,
                                   Action<IRemoteEVSE>?               OnSuccess      = null,
                                   Action<ChargingStation, EVSE_Id>?  OnError        = null)

        {

            #region Initial checks

            if (_EVSEs.Any(evse => evse.Id == RemoteEVSE.Id))
            {
                throw new Exception("EVSEAlreadyExistsInStation");
                // if (OnError == null)
                //     throw new EVSEAlreadyExistsInStation(this.ChargingStation, EVSEId);
                // else
                //     OnError?.Invoke(this, EVSEId);
            }

            #endregion

            Configurator?.Invoke(RemoteEVSE);

            if (AddNewEVSE(RemoteEVSE))
            {
                OnSuccess?.Invoke(RemoteEVSE);
                return RemoteEVSE;
            }

            return null;

        }

        #endregion


        #region (internal) UpdateEVSEData       (Timestamp, RemoteEVSE, NewValue, OldValue = null, DataSource = null)

        /// <summary>
        /// Update the data of a remote EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="RemoteEVSE">The remote EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime  Timestamp,
                                     IEVSE     RemoteEVSE,
                                     String    PropertyName,
                                     Object?   NewValue,
                                     Object?   OldValue     = null,
                                     Context?  DataSource   = null)
        {

            OnEVSEDataChanged?.Invoke(Timestamp,
                                      EventTracking_Id.New,
                                      RemoteEVSE,
                                      PropertyName,
                                      NewValue,
                                      OldValue,
                                      DataSource);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, RemoteEVSE, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated remote EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                            Timestamp,
                                                  EventTracking_Id                    EventTrackingId,
                                                  IEVSE                               RemoteEVSE,
                                                  Timestamped<EVSEAdminStatusTypes>   NewStatus,
                                                  Timestamped<EVSEAdminStatusTypes>?  OldStatus    = null,
                                                  Context?                            DataSource   = null)
        {

            var onEVSEAdminStatusChanged = OnEVSEAdminStatusChanged;
            if (onEVSEAdminStatusChanged is not null)
                await onEVSEAdminStatusChanged(Timestamp,
                                                    EventTrackingId,
                                                    RemoteEVSE,
                                                    NewStatus,
                                                    OldStatus,
                                                    DataSource);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, RemoteEVSE, NewStatus, OldStatus = null, DataSource = null)

        /// <summary>
        /// Update the remote EVSE station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated EVSE.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        /// <param name="OldStatus">The optional old EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                       Timestamp,
                                             EventTracking_Id               EventTrackingId,
                                             IEVSE                          RemoteEVSE,
                                             Timestamped<EVSEStatusType>   NewStatus,
                                             Timestamped<EVSEStatusType>?  OldStatus    = null,
                                             Context?                       DataSource   = null)
        {

            var onEVSEStatusChanged = OnEVSEStatusChanged;
            if (onEVSEStatusChanged is not null)
                await onEVSEStatusChanged(Timestamp,
                                               EventTrackingId,
                                               RemoteEVSE,
                                               NewStatus,
                                               OldStatus,
                                               DataSource);

        }

        #endregion


        #region GetEVSEStatus(...)

        public async Task<IEnumerable<EVSEStatus>> GetEVSEStatus(DateTime           Timestamp,
                                                                 CancellationToken  CancellationToken,
                                                                 EventTracking_Id   EventTrackingId,
                                                                 TimeSpan?          RequestTimeout  = null)

            => _EVSEs.Select(evse => new EVSEStatus(evse.Id,
                                                    new Timestamped<EVSEStatusType>(
                                                        evse.Status.Timestamp,
                                                        evse.Status.Value
                                                    )));

        #endregion


        // Socket events

        #region ChargingConnectorAddition

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, VirtualChargingStation, ChargingConnector, Boolean> ChargingConnectorAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, VirtualChargingStation, ChargingConnector, Boolean> OnChargingConnectorAddition
        {
            get
            {
                return ChargingConnectorAddition;
            }
        }

        #endregion

        #region ChargingConnectorRemoval

        internal readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, VirtualChargingStation, ChargingConnector, Boolean> ChargingConnectorRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, VirtualChargingStation, ChargingConnector, Boolean> OnChargingConnectorRemoval
        {
            get
            {
                return ChargingConnectorRemoval;
            }
        }

        #endregion

        #endregion

        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservationCollection> chargingReservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => chargingReservations.Select(_ => _.Value).FirstOrDefault();

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate?             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate?            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate?             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate?   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate?  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate?        OnReservationCanceled;

        #endregion


        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this charging station.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                          StartTime              = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)


                => Reserve(ChargingLocation.FromChargingStationId(Id),
                           ChargingReservationLevel.ChargingStation,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           AuthenticationPath,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout,
                           CancellationToken);

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation? newReservation  = null;
            ReservationResult?   result          = null;

            #endregion

            #region Send OnReserveRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(startTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         LinkedReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingStationId.HasValue && ChargingLocation.ChargingStationId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    #region Check if the eMAId is on the white list

                    if (UseWhiteLists &&
                       !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                    {
                        result = ReservationResult.InvalidCredentials;
                    }

                    #endregion

                    else
                    {

                        if (ReservationLevel == ChargingReservationLevel.EVSE &&
                            ChargingLocation.EVSEId.HasValue &&
                            TryGetEVSEById(ChargingLocation.EVSEId.Value, out var remoteEVSE))
                        {

                            result = await remoteEVSE.
                                               Reserve(ChargingLocation,
                                                       ReservationLevel,
                                                       ReservationStartTime,
                                                       Duration,
                                                       ReservationId,
                                                       LinkedReservationId,
                                                       ProviderId,
                                                       RemoteAuthentication,
                                                       AuthenticationPath,
                                                       ChargingProduct,
                                                       AuthTokens,
                                                       eMAIds,
                                                       PINs,

                                                       Timestamp,
                                                       EventTrackingId,
                                                       RequestTimeout,
                                                       CancellationToken);

                            newReservation = result.Reservation;

                        }

                        else if (ReservationLevel == ChargingReservationLevel.ChargingStation &&
                                 ChargingLocation.ChargingStationId.HasValue)
                        {

                            var results = new List<ReservationResult>();

                            foreach (var remoteEVSE2 in _EVSEs)
                            {

                                results.Add(await remoteEVSE2.
                                                      Reserve(ChargingLocation,
                                                              ReservationLevel,
                                                              ReservationStartTime,
                                                              Duration,
                                                              ChargingReservation_Id.NewRandom(OperatorId),
                                                              LinkedReservationId,
                                                              ProviderId,
                                                              RemoteAuthentication,
                                                              AuthenticationPath,
                                                              ChargingProduct,
                                                              AuthTokens,
                                                              eMAIds,
                                                              PINs,

                                                              Timestamp,
                                                              EventTrackingId,
                                                              RequestTimeout,
                                                              CancellationToken));

                            }

                            var newReservations = results.Where (_result => _result.Result == ReservationResultType.Success).
                                                          Select(_result => _result.Reservation).
                                                          ToArray();

                            if (newReservations.Length > 0)
                            {

                                newReservation = new ChargingReservation(Id:                      ReservationId ?? ChargingReservation_Id.NewRandom(OperatorId),
                                                                         Timestamp:               Timestamp.Value,
                                                                         StartTime:               ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                         Duration:                Duration  ?? MaxReservationDuration,
                                                                         EndTime:                 (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                                         ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                         ReservationLevel:        ReservationLevel,
                                                                         ProviderId:              ProviderId,
                                                                         StartAuthentication:     RemoteAuthentication,
                                                                         RoamingNetworkId:        null,
                                                                         ChargingPoolId:          null,
                                                                         ChargingStationId:       Id,
                                                                         EVSEId:                  null,
                                                                         ChargingProduct:         ChargingProduct,
                                                                         SubReservations:         newReservations);

                                foreach (var subReservation in newReservation.SubReservations)
                                {
                                    subReservation.ParentReservation = newReservation;
                                    subReservation.ChargingStationId = Id;
                                }

                                result = ReservationResult.Success(newReservation);

                            }

                            else
                                result = ReservationResult.AlreadyReserved;

                        }

                        else
                            result = ReservationResult.UnknownLocation;

                    }

                }
                else
                {

                    result = AdminStatus.Value switch {
                        _ => ReservationResult.OutOfService,
                    };
                }


                if (result.Result == ReservationResultType.Success &&
                    newReservation != null)
                {

                    chargingReservations.Add(newReservation.Id, new ChargingReservationCollection(newReservation));

                    foreach (var subReservation in newReservation.SubReservations)
                        chargingReservations.Add(subReservation.Id, new ChargingReservationCollection(subReservation));

                    OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             this,
                                             newReservation);

                }

            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(endTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          LinkedReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          endTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp          = null,
                              EventTracking_Id?                      EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null,
                              CancellationToken                      CancellationToken  = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation?     canceledReservation  = null;
            CancelReservationResult? result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(startTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
            {

                if (TryGetChargingReservationById(ReservationId, out canceledReservation))
                {

                    if (canceledReservation.EVSEId.HasValue)
                    {

                        result = await GetEVSEById(canceledReservation.EVSEId.Value).
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             EventTrackingId,
                                                             RequestTimeout,
                                                             CancellationToken);

                    }

                    if (result?.Result == CancelReservationResultTypes.UnknownReservationId)
                    {
                        foreach (var evse in _EVSEs)
                        {

                            result = await evse.CancelReservation(ReservationId,
                                                                  Reason,

                                                                  Timestamp,
                                                                  EventTrackingId,
                                                                  RequestTimeout,
                                                                  CancellationToken);

                        }
                    }


                    if (canceledReservation.SubReservations.SafeAny())
                    {

                        var results = new List<CancelReservationResult>();

                        foreach (var subReservation in canceledReservation.SubReservations)
                        {
                            if (subReservation.EVSEId.HasValue)
                            {

                                results.Add(await GetEVSEById(subReservation.EVSEId.Value).
                                                      CancelReservation(ReservationId,
                                                                        Reason,

                                                                        Timestamp,
                                                                        EventTrackingId,
                                                                        RequestTimeout,
                                                                        CancellationToken));

                            }
                        }

                        if (result?.Result                        == CancelReservationResultTypes.Success &&
                            results.All(result2 => result2.Result == CancelReservationResultTypes.Success ||
                                                   result2.Result == CancelReservationResultTypes.UnknownReservationId))
                        {
                            result = CancelReservationResult.Success(ReservationId,
                                                                     Reason);
                        }

                        //ToDo: Define a better result!

                    }

                }


            }
            else
            {
                result = AdminStatus.Value switch {
                    _ => CancelReservationResult.OutOfService(ReservationId,
                                                              Reason),
                };
            }

            result ??= CancelReservationResult.OutOfService(ReservationId, Reason);


            #region Send OnCancelReservationResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(endTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    endTime - startTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CheckIfReservationIsExpired(State)

        /// <summary>
        /// Check if the reservation is expired.
        /// </summary>
        public void CheckIfReservationIsExpired(Object State)
        {

            if (Monitor.TryEnter(ReservationExpiredLock))
            {

                try
                {

                    ChargingReservation[]? expiredReservations = null;

                    lock (chargingReservations)
                    {
                        expiredReservations = chargingReservations.Values.Where(reservationCollection => reservationCollection.LastOrDefault()?.IsExpired() == true).LastOrDefault()?.ToArray() ?? Array.Empty<ChargingReservation>();
                    }

                    foreach (var expiredReservation in expiredReservations)
                    {

                        lock (chargingReservations)
                        {
                            chargingReservations.Remove(expiredReservation.Id);
                        }

                        //if (Status.Value == EVSEStatusType.Reserved &&
                        //    !_Reservations.Any())
                        //{
                        //    // Will send events!
                        //    SetStatus(EVSEStatusType.Available);
                        //}

                        OnReservationCanceled?.Invoke(Timestamp.Now,
                                                      this,
                                                      expiredReservation,
                                                      ChargingReservationCancellationReason.Expired);

                    }

                }
                catch (Exception e)
                {
                    DebugX.LogT(e.Message);
                }
                finally
                {
                    Monitor.Exit(ReservationExpiredLock);
                }

            }

        }

        #endregion


        #region GetChargingReservationById    (ReservationId)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservation? GetChargingReservationById(ChargingReservation_Id ReservationId)
        {

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
                return reservationCollection?.LastOrDefault();

            return null;

        }

        #endregion

        #region GetChargingReservationsById   (ReservationId)

        /// <summary>
        /// Return the charging reservations specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        public ChargingReservationCollection? GetChargingReservationsById(ChargingReservation_Id ReservationId)
        {

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
                return reservationCollection;

            return null;

        }

        #endregion

        #region TryGetChargingReservationById (ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? Reservation)
        {

            if (chargingReservations.TryGetValue(ReservationId, out var reservationCollection))
            {
                Reservation = reservationCollection?.LastOrDefault();
                return true;
            }

            Reservation = null;
            return false;

        }

        #endregion

        #region TryGetChargingReservationsById(ReservationId, out ChargingReservations)

        /// <summary>
        /// Return the charging reservation collection specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="ChargingReservations">The charging reservations.</param>
        public Boolean TryGetChargingReservationsById(ChargingReservation_Id ReservationId, out ChargingReservationCollection? ChargingReservations)

            => chargingReservations.TryGetValue(ReservationId, out ChargingReservations);

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region AuthorizeStart/-Stop

        #region Properties

        public IId      AuthId
            => RoamingNetwork.AuthId;

        /// <summary>
        /// Disable the local authorization of charging processes.
        /// </summary>
        public Boolean  DisableAuthorization    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate?   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStart request was received.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate?  OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate?    OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever a response to an AuthorizeStop request was received.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate?   OnAuthorizeStopResponse;

        #endregion

        #region AuthorizeStart           (LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    RequestTimestamp      = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStartResult? result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = ChargingPool is not null

                             ? await ChargingPool.AuthorizeStart(
                                         LocalAuthentication,
                                         ChargingLocation,
                                         ChargingProduct,
                                         SessionId,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStartResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStartResult.Error(
                             Id,
                             this,
                             SessionId:    SessionId,
                             Description:  I18NString.Create(e.Message),
                             Runtime:      Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          LocalAuthentication,
                          ChargingLocation,
                          ChargingProduct,
                          SessionId,
                          CPOPartnerSessionId,
                          [],
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    RequestTimestamp      = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= TimeSpan.FromSeconds(10);

            AuthStopResult? result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = ChargingPool is not null

                             ? await ChargingPool.AuthorizeStop(
                                         SessionId,
                                         LocalAuthentication,
                                         ChargingLocation,
                                         CPOPartnerSessionId,
                                         OperatorId,

                                         RequestTimestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     )

                             : AuthStopResult.OutOfService(
                                   Id,
                                   this,
                                   SessionId:  SessionId,
                                   Runtime:    Timestamp.Now - startTime
                               );

            }
            catch (Exception e)
            {

                result = AuthStopResult.Error(
                             SessionId,
                             this,
                             SessionId,
                             I18NString.Create(e.Message),
                             Timestamp.Now - startTime
                         );

            }


            #region Send OnAuthorizeStopResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStopResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          Id.ToString(),
                          EventTrackingId,
                          RoamingNetwork.Id,
                          null,
                          null,
                          OperatorId,
                          ChargingLocation,
                          SessionId,
                          CPOPartnerSessionId,
                          LocalAuthentication,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Data

        private readonly Dictionary<ChargingSession_Id, ChargingSession> chargingSessions;

        public IEnumerable<ChargingSession> ChargingSessions
            => chargingSessions.Select(_ => _.Value);

        #region ContainsChargingSessionId (ChargingSessionId)

        /// <summary>
        /// Whether the given charging session identification is known within the charging station.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public Boolean ContainsChargingSessionId(ChargingSession_Id ChargingSessionId)

            => chargingSessions.ContainsKey(ChargingSessionId);

        #endregion

        #region GetChargingSessionById    (ChargingSessionId)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        public ChargingSession? GetChargingSessionById(ChargingSession_Id ChargingSessionId)
        {

            if (chargingSessions.TryGetValue(ChargingSessionId, out var chargingSession))
                return chargingSession;

            return null;

        }

        #endregion

        #region TryGetChargingSessionById (ChargingSessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="ChargingSessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id ChargingSessionId, out ChargingSession? ChargingSession)

            => chargingSessions.TryGetValue(ChargingSessionId, out ChargingSession);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate?     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate?  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null,
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                Timestamp                = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)


                => RemoteStart(
                       ChargingLocation.FromChargingStationId(Id),
                       ChargingProduct,
                       ReservationId,
                       SessionId,
                       ProviderId,
                       RemoteAuthentication,
                       AdditionalSessionInfos,
                       AuthenticationPath,

                       Timestamp,
                       EventTrackingId,
                       RequestTimeout,
                       CancellationToken
                   );

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null,
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,

                        DateTime?                RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)

        {

            #region Initial checks

            SessionId       ??= ChargingSession_Id.NewRandom(OperatorId);
            RequestTimestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                if (ChargingLocation.ChargingStationId.HasValue &&
                    ChargingLocation.ChargingStationId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation(System_Id.Local);
                }

                else if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    #region Check if the eMAId is on the white list

                    if (UseWhiteLists &&
                       !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                    {
                        result = RemoteStartResult.InvalidCredentials(System_Id.Local);
                    }

                    #endregion

                    else if (!ChargingLocation.EVSEId.HasValue)
                        result = RemoteStartResult.UnknownLocation(System_Id.Local);

                    else if (!TryGetEVSEById(ChargingLocation.EVSEId.Value, out var remoteEVSE))
                        result = RemoteStartResult.UnknownLocation(System_Id.Local);

                    else
                        result = await remoteEVSE.RemoteStart(
                                           ChargingProduct,
                                           ReservationId,
                                           SessionId,
                                           ProviderId,
                                           RemoteAuthentication,
                                           AdditionalSessionInfos,
                                           AuthenticationPath,

                                           RequestTimestamp,
                                           EventTrackingId,
                                           RequestTimeout,
                                           CancellationToken
                                       );

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService(System_Id.Local);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message, System_Id.Local);
            }


            #region Send OnRemoteStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,
                       Auth_Path?             AuthenticationPath     = null,

                       DateTime?              Timestamp              = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null,
                       CancellationToken      CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            null,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingStationAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingStationAdminStatusTypes.InternalUse)
                {

                    #region Check if the eMAId is on the white list

                    if (UseWhiteLists &&
                       !WhiteLists["default"].Contains(RemoteAuthentication.ToLocal))
                    {
                        result = RemoteStopResult.InvalidCredentials(SessionId, System_Id.Local);
                    }

                    #endregion

                    else if (!TryGetChargingSessionById(SessionId, out ChargingSession chargingSession))
                    {

                        foreach (var remoteEVSE in _EVSEs)
                        {

                            result = await remoteEVSE.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,
                                                          AuthenticationPath,

                                                          Timestamp,
                                                          EventTrackingId,
                                                          RequestTimeout,
                                                          CancellationToken);

                            if (result.Result == RemoteStopResultTypes.Success)
                                break;

                        }

                        if (result.Result != RemoteStopResultTypes.Success)
                            result = RemoteStopResult.InvalidSessionId(SessionId, System_Id.Local);

                    }

                    else if (chargingSession.EVSE != null)
                    {

                        result = await chargingSession.EVSE.RemoteEVSE.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      AuthenticationPath,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                    }

                    else if (chargingSession.EVSEId.HasValue &&
                             TryGetEVSEById(chargingSession.EVSEId.Value, out var remoteEVSE))
                    {

                        result = await remoteEVSE.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,
                                                      AuthenticationPath,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

                    }

                    result = RemoteStopResult.UnknownLocation(SessionId, System_Id.Local);

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStopResult.OutOfService(SessionId, System_Id.Local);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                System_Id.Local,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingStation) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session is not null)
            {

                if (Session.ChargingStation is null)
                    Session.ChargingStationId  = Id;

                OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord is not null)
                OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region WhiteLists

        #region GetWhiteList(Name)

        public HashSet<LocalAuthentication> GetWhiteList(String Name)

            => WhiteLists[Name];

        #endregion

        #endregion



        public JObject ToJSON(Boolean                                              Embedded                            = false,
                              InfoStatus                                           ExpandRoamingNetworkId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingStationOperatorId     = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandChargingPoolId                = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandEVSEIds                       = InfoStatus.Expanded,
                              InfoStatus                                           ExpandBrandIds                      = InfoStatus.ShowIdOnly,
                              InfoStatus                                           ExpandDataLicenses                  = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IChargingStation>?   CustomChargingStationSerializer     = null,
                              CustomJObjectSerializerDelegate<IEVSE>?              CustomEVSESerializer                = null,
                              CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null)
        {
            throw new NotImplementedException();
        }






        public Address? Address
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public GeoCoordinate? GeoLocation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public OpeningTimes OpeningTimes
            => throw new NotImplementedException();

        public ConcurrentDictionary<Brand_Id, Brand> Brands => throw new NotImplementedException();

        public ReactiveSet<OpenDataLicense> DataLicenses => throw new NotImplementedException();

        public String? OpenStreetMapNodeId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Address? EntranceAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GeoCoordinate? EntranceLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public I18NString ArrivalInstructions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        OpeningTimes IChargingStation.OpeningTimes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<ParkingSpace> ParkingSpaces => throw new NotImplementedException();

        public ReactiveSet<UIFeatures> UIFeatures => throw new NotImplementedException();

        public ReactiveSet<AuthenticationModes> AuthenticationModes => throw new NotImplementedException();

        public ReactiveSet<PaymentOptions> PaymentOptions => throw new NotImplementedException();

        public AccessibilityType? Accessibility { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<String> PhotoURIs => throw new NotImplementedException();

        public PhoneNumber? HotlinePhoneNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Address ExitAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GeoCoordinate? ExitLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GridConnectionTypes? GridConnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Decimal? MaxCurrent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Timestamped<Decimal>? MaxCurrentRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxCurrentPrognoses => throw new NotImplementedException();

        public Decimal? MaxPower { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Timestamped<Decimal>? MaxPowerRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxPowerPrognoses => throw new NotImplementedException();

        public Decimal? MaxCapacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Timestamped<Decimal>? MaxCapacityRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxCapacityPrognoses => throw new NotImplementedException();

        public EnergyMix? EnergyMix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Timestamped<EnergyMix>? EnergyMixRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        TimeSpan IChargingStation.MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Boolean IsFreeOfCharge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Boolean IsHubjectCompatible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Boolean DynamicInfoAvailable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public String ServiceIdentification { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public String ModelCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public String HubjectStationId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<EVSEStatusReport, ChargingStationStatusTypes> StatusAggregationDelegate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IVotingSender<DateTime, User_Id, IChargingStation, IEVSE, Boolean> OnEVSEAddition => throw new NotImplementedException();

        public IVotingSender<DateTime, User_Id, IChargingStation, IEVSE, Boolean> OnEVSERemoval => throw new NotImplementedException();

        ReactiveSet<Brand> IChargingStation.Brands => throw new NotImplementedException();

        public ReactiveSet<URL> PhotoURLs => throw new NotImplementedException();

        public EnergyMixPrognosis? EnergyMixPrognoses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TimeSpan IChargingReservations.MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<ChargingStationFeature> Features => throw new NotImplementedException();

        public IVotingSender<DateTime, User_Id, IChargingStation, IEVSE, IEVSE, Boolean> OnEVSEUpdate => throw new NotImplementedException();

        public Boolean Equals(IChargingStation? other)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(IChargingStation? other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IEVSE> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }



        //-- Client-side methods -----------------------------------------

        #region Authenticate(LocalAuthentication)

        public Boolean Authenticate(LocalAuthentication LocalAuthentication)
        {
            return false;
        }

        #endregion


        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                         [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(EVSE),
                       Logger,
                       LogHandler,
                       EventName,
                       Command
                   );

        #endregion


        #region Operator overloading

        #region Operator == (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(VirtualChargingStation1, VirtualChargingStation2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualChargingStation1 is null) || (VirtualChargingStation2 is null))
                return false;

            return VirtualChargingStation1.Equals(VirtualChargingStation2);

        }

        #endregion

        #region Operator != (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
            => !(VirtualChargingStation1 == VirtualChargingStation2);

        #endregion

        #region Operator <  (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
        {

            if (VirtualChargingStation1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingStation1), "The given VirtualChargingStation1 must not be null!");

            return VirtualChargingStation1.CompareTo(VirtualChargingStation2) < 0;

        }

        #endregion

        #region Operator <= (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
            => !(VirtualChargingStation1 > VirtualChargingStation2);

        #endregion

        #region Operator >  (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
        {

            if (VirtualChargingStation1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingStation1), "The given VirtualChargingStation1 must not be null!");

            return VirtualChargingStation1.CompareTo(VirtualChargingStation2) > 0;

        }

        #endregion

        #region Operator >= (VirtualChargingStation1, VirtualChargingStation2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation1">A virtual charging station.</param>
        /// <param name="VirtualChargingStation2">Another virtual charging station.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (VirtualChargingStation VirtualChargingStation1, VirtualChargingStation VirtualChargingStation2)
            => !(VirtualChargingStation1 < VirtualChargingStation2);

        #endregion

        #endregion

        #region IComparable<VirtualChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualChargingStation VirtualChargingStation))
                throw new ArgumentException("The given object is not a virtual charging station!");

            return CompareTo(VirtualChargingStation);

        }

        #endregion

        #region CompareTo(VirtualChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingStation">An virtual charging station to compare with.</param>
        public Int32 CompareTo(VirtualChargingStation VirtualChargingStation)
        {

            if (VirtualChargingStation is null)
                throw new ArgumentNullException(nameof(VirtualChargingStation),  "The given virtual charging station must not be null!");

            return Id.CompareTo(VirtualChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualChargingStation> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is VirtualChargingStation VirtualChargingStation))
                return false;

            return Equals(VirtualChargingStation);

        }

        #endregion

        #region Equals(VirtualChargingStation)

        /// <summary>
        /// Compares two virtual charging stations for equality.
        /// </summary>
        /// <param name="VirtualChargingStation">A virtual charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualChargingStation VirtualChargingStation)
        {

            if (VirtualChargingStation is null)
                return false;

            return Id.Equals(VirtualChargingStation.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        public IEnumerable<EVSE_Id> EVSEIds(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEAdminStatus> EVSEAdminStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEStatus> EVSEStatus(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEAdminStatus> EVSEAdminStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EVSEStatus> EVSEStatusSchedule(IncludeEVSEDelegate IncludeEVSEs = null)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsEVSE(EVSE EVSE)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEAdminStatusTypes>>>> EVSEAdminStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null, Func<DateTime, Boolean>? TimestampFilter = null, Func<EVSEAdminStatusTypes, Boolean>? StatusFilter = null, UInt64? Skip = null, UInt64? Take = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<EVSE_Id, IEnumerable<Timestamped<EVSEStatusType>>>> EVSEStatusSchedule(IncludeEVSEDelegate? IncludeEVSEs = null, Func<DateTime, Boolean>? TimestampFilter = null, Func<EVSEStatusType, Boolean>? StatusFilter = null, UInt64? Skip = null, UInt64? Take = null)
        {
            throw new NotImplementedException();
        }

        public Boolean ContainsEVSE(IEVSE EVSE)
        {
            throw new NotImplementedException();
        }

        public IChargingStation UpdateWith(IChargingStation OtherChargingStation)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSE(EVSE_Id Id, I18NString? Name = null, I18NString? Description = null, Timestamped<EVSEAdminStatusTypes>? InitialAdminStatus = null, Timestamped<EVSEStatusType>? InitialStatus = null, UInt16? MaxAdminStatusScheduleSize = null, UInt16? MaxStatusScheduleSize = null, IEnumerable<URL>? PhotoURLs = null, IEnumerable<Brand>? Brands = null, IEnumerable<OpenDataLicense>? OpenDataLicenses = null, IEnumerable<ChargingModes>? ChargingModes = null, IEnumerable<ChargingTariff>? ChargingTariffs = null, CurrentTypes? CurrentType = null, Decimal? AverageVoltage = null, Timestamped<Decimal>? AverageVoltageRealTime = null, IEnumerable<Timestamped<Decimal>>? AverageVoltagePrognoses = null, Decimal? MaxCurrent = null, Timestamped<Decimal>? MaxCurrentRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxCurrentPrognoses = null, Decimal? MaxPower = null, Timestamped<Decimal>? MaxPowerRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxPowerPrognoses = null, Decimal? MaxCapacity = null, Timestamped<Decimal>? MaxCapacityRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxCapacityPrognoses = null, EnergyMix? EnergyMix = null, Timestamped<EnergyMix>? EnergyMixRealTime = null, EnergyMixPrognosis? EnergyMixPrognoses = null, EnergyMeter? EnergyMeter = null, Boolean? IsFreeOfCharge = null, IEnumerable<IChargingConnector>? ChargingConnectors = null, ChargingSession? ChargingSession = null, DateTime? LastStatusUpdate = null, String? DataSource = null, DateTime? LastChange = null, JObject? CustomData = null, UserDefinedDictionary? InternalData = null, Action<IEVSE>? Configurator = null, RemoteEVSECreatorDelegate? RemoteEVSECreator = null, Action<IEVSE>? OnSuccess = null, Action<IChargingStation, EVSE_Id>? OnError = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(EVSE_Id Id, I18NString? Name = null, I18NString? Description = null, Timestamped<EVSEAdminStatusTypes>? InitialAdminStatus = null, Timestamped<EVSEStatusType>? InitialStatus = null, UInt16? MaxAdminStatusScheduleSize = null, UInt16? MaxStatusScheduleSize = null, IEnumerable<URL>? PhotoURLs = null, IEnumerable<Brand>? Brands = null, IEnumerable<OpenDataLicense>? OpenDataLicenses = null, IEnumerable<ChargingModes>? ChargingModes = null, IEnumerable<ChargingTariff>? ChargingTariffs = null, CurrentTypes? CurrentType = null, Decimal? AverageVoltage = null, Timestamped<Decimal>? AverageVoltageRealTime = null, IEnumerable<Timestamped<Decimal>>? AverageVoltagePrognoses = null, Decimal? MaxCurrent = null, Timestamped<Decimal>? MaxCurrentRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxCurrentPrognoses = null, Decimal? MaxPower = null, Timestamped<Decimal>? MaxPowerRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxPowerPrognoses = null, Decimal? MaxCapacity = null, Timestamped<Decimal>? MaxCapacityRealTime = null, IEnumerable<Timestamped<Decimal>>? MaxCapacityPrognoses = null, EnergyMix? EnergyMix = null, Timestamped<EnergyMix>? EnergyMixRealTime = null, EnergyMixPrognosis? EnergyMixPrognoses = null, EnergyMeter? EnergyMeter = null, Boolean? IsFreeOfCharge = null, IEnumerable<IChargingConnector>? ChargingConnectors = null, ChargingSession? ChargingSession = null, DateTime? LastStatusUpdate = null, String? DataSource = null, DateTime? LastChange = null, JObject? CustomData = null, UserDefinedDictionary? InternalData = null, Action<IEVSE>? Configurator = null, RemoteEVSECreatorDelegate? RemoteEVSECreator = null, Action<IEVSE>? OnSuccess = null, Action<IChargingStation, EVSE_Id>? OnError = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSE(IEVSE EVSE, Action<IEVSE>? OnSuccess = null, Action<IChargingStation, IEVSE>? OnError = null, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSEIfNotExists(IEVSE EVSE, Action<IEVSE>? OnSuccess = null, Action<IChargingStation, IEVSE>? OnError = null, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(IEVSE EVSE, Action<IEVSE>? OnAdditionSuccess = null, Action<IEVSE, IEVSE>? OnUpdateSuccess = null, Action<IChargingStation, IEVSE>? OnError = null, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSE(IEVSE EVSE, Action<IEVSE, EventTracking_Id>? OnSuccess = null, Action<IChargingStation, IEVSE, EventTracking_Id>? OnError = null, Boolean SkipAddedNotifications = false, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddEVSEResult> AddEVSEIfNotExists(IEVSE EVSE, Action<IEVSE, EventTracking_Id>? OnSuccess = null, Boolean SkipAddedNotifications = false, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<AddOrUpdateEVSEResult> AddOrUpdateEVSE(IEVSE EVSE, Action<IEVSE, EventTracking_Id>? OnAdditionSuccess = null, Action<IEVSE, IEVSE, EventTracking_Id>? OnUpdateSuccess = null, Action<IChargingStation, IEVSE, EventTracking_Id>? OnError = null, Boolean SkipAddOrUpdatedUpdatedNotifications = false, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEResult> UpdateEVSE(IEVSE EVSE, Action<IEVSE, IEVSE, EventTracking_Id>? OnSuccess = null, Action<IChargingStation, IEVSE, EventTracking_Id>? OnError = null, Boolean SkipUpdatedNotifications = false, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateEVSEResult> UpdateEVSE(IEVSE EVSE, Action<IEVSE> UpdateDelegate, Action<IEVSE, IEVSE, EventTracking_Id>? OnSuccess = null, Action<IChargingStation, IEVSE, EventTracking_Id>? OnError = null, Boolean SkipUpdatedNotifications = false, Func<ChargingStationOperator_Id, EVSE_Id, Boolean>? AllowInconsistentOperatorIds = null, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteEVSEResult> RemoveEVSE(EVSE_Id Id, Action<IEVSE, EventTracking_Id>? OnSuccess = null, Action<IChargingStation, IEVSE, EventTracking_Id>? OnError = null, Boolean SkipRemovedNotifications = false, EventTracking_Id? EventTrackingId = null, User_Id? CurrentUserId = null)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
