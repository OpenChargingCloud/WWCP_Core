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

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP.Virtual
{

    /// <summary>
    /// Extension methods for virtual EVSEs.
    /// </summary>
    public static class VirtualEVSEExtensions
    {

        #region AddVirtualEVSE           (this ChargingStation, Id = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="EVSEId">The EVSE identification for the EVSE to be created.</param>
        /// <param name="EVSEConfigurator">An optional delegate to configure the new (local) EVSE.</param>
        /// <param name="VirtualEVSEConfigurator">An optional delegate to configure the new EVSE.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static Task<AddEVSEResult> AddVirtualEVSE(this IChargingStation                                ChargingStation,
                                                         EVSE_Id                                              Id,
                                                         I18NString?                                          Name                           = null,
                                                         I18NString?                                          Description                    = null,

                                                         Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                         Timestamped<EVSEStatusTypes>?                        InitialStatus                  = null,
                                                         UInt16                                               MaxAdminStatusScheduleSize     = VirtualEVSE.DefaultMaxAdminStatusScheduleSize,
                                                         UInt16                                               MaxStatusScheduleSize          = VirtualEVSE.DefaultMaxStatusScheduleSize,

                                                         IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                         IEnumerable<Brand>?                                  Brands                         = null,
                                                         IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                         IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                         IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                         CurrentTypes?                                        CurrentType                    = null,
                                                         Decimal?                                             AverageVoltage                 = null,
                                                         Timestamped<Decimal>?                                AverageVoltageRealTime         = null,
                                                         IEnumerable<Timestamped<Decimal>>?                   AverageVoltagePrognoses        = null,
                                                         Decimal?                                             MaxCurrent                     = null,
                                                         Timestamped<Decimal>?                                MaxCurrentRealTime             = null,
                                                         IEnumerable<Timestamped<Decimal>>?                   MaxCurrentPrognoses            = null,
                                                         Decimal?                                             MaxPower                       = null,
                                                         Timestamped<Decimal>?                                MaxPowerRealTime               = null,
                                                         IEnumerable<Timestamped<Decimal>>?                   MaxPowerPrognoses              = null,
                                                         Decimal?                                             MaxCapacity                    = null,
                                                         Timestamped<Decimal>?                                MaxCapacityRealTime            = null,
                                                         IEnumerable<Timestamped<Decimal>>?                   MaxCapacityPrognoses           = null,
                                                         EnergyMix?                                           EnergyMix                      = null,
                                                         Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                         EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                         EnergyMeter?                                         EnergyMeter                    = null,
                                                         Boolean?                                             IsFreeOfCharge                 = null,
                                                         IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,

                                                         ChargingSession?                                     ChargingSession                = null,
                                                         DateTime?                                            LastStatusUpdate               = null,
                                                         String?                                              DataSource                     = null,
                                                         DateTime?                                            LastChange                     = null,

                                                         Action<IEVSE>?                                       Configurator                   = null,
                                                         Action<VirtualEVSE>?                                 VirtualEVSEConfigurator        = null,
                                                         RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                         JObject?                                             CustomData                     = null,
                                                         UserDefinedDictionary?                               InternalData                   = null,

                                                         String                                               EllipticCurve                  = "P-256",
                                                         ECPrivateKeyParameters?                              PrivateKey                     = null,
                                                         PublicKeyCertificates?                               PublicKeyCertificates          = null,
                                                         TimeSpan?                                            SelfCheckTimeSpan              = null,

                                                         Action<IEVSE>?                                       OnSuccess                      = null,
                                                         Action<IChargingStation, IEVSE>?                     OnError                        = null,

                                                         Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                         EventTracking_Id?                                    EventTrackingId                = null,
                                                         User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.AddEVSE(

                   Id,
                   Name,
                   Description,

                   InitialAdminStatus ?? EVSEAdminStatusTypes.Operational,
                   InitialStatus      ?? EVSEStatusTypes.Available,
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

                   ChargingSession,
                   LastStatusUpdate,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData,

                   Configurator,
                   newEVSE => {

                       var virtualevse = new VirtualEVSE(

                                             newEVSE.Id,
                                             ChargingStation,
                                             newEVSE.Name,
                                             newEVSE.Description,

                                             InitialAdminStatus,
                                             InitialStatus,
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
                                             SelfCheckTimeSpan

                                         );

                       VirtualEVSEConfigurator?.Invoke(virtualevse);

                       return virtualevse;

                   },

                   OnSuccess,
                   OnError,

                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId

               );

        #endregion

        #region AddVirtualEVSEIfNotExists(this ChargingStation, Id = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="EVSEId">The EVSE identification for the EVSE to be created.</param>
        /// <param name="EVSEConfigurator">An optional delegate to configure the new (local) EVSE.</param>
        /// <param name="VirtualEVSEConfigurator">An optional delegate to configure the new EVSE.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static Task<AddEVSEResult> AddVirtualEVSEIfNotExists(this IChargingStation                                ChargingStation,

                                                                    EVSE_Id                                              Id,
                                                                    I18NString?                                          Name                           = null,
                                                                    I18NString?                                          Description                    = null,

                                                                    Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                    Timestamped<EVSEStatusTypes>?                        InitialStatus                  = null,
                                                                    UInt16                                               MaxAdminStatusScheduleSize     = VirtualEVSE.DefaultMaxAdminStatusScheduleSize,
                                                                    UInt16                                               MaxStatusScheduleSize          = VirtualEVSE.DefaultMaxStatusScheduleSize,

                                                                    IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                                    IEnumerable<Brand>?                                  Brands                         = null,
                                                                    IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                                    IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                                    IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                                    CurrentTypes?                                        CurrentType                    = null,
                                                                    Decimal?                                             AverageVoltage                 = null,
                                                                    Timestamped<Decimal>?                                AverageVoltageRealTime         = null,
                                                                    IEnumerable<Timestamped<Decimal>>?                   AverageVoltagePrognoses        = null,
                                                                    Decimal?                                             MaxCurrent                     = null,
                                                                    Timestamped<Decimal>?                                MaxCurrentRealTime             = null,
                                                                    IEnumerable<Timestamped<Decimal>>?                   MaxCurrentPrognoses            = null,
                                                                    Decimal?                                             MaxPower                       = null,
                                                                    Timestamped<Decimal>?                                MaxPowerRealTime               = null,
                                                                    IEnumerable<Timestamped<Decimal>>?                   MaxPowerPrognoses              = null,
                                                                    Decimal?                                             MaxCapacity                    = null,
                                                                    Timestamped<Decimal>?                                MaxCapacityRealTime            = null,
                                                                    IEnumerable<Timestamped<Decimal>>?                   MaxCapacityPrognoses           = null,
                                                                    EnergyMix?                                           EnergyMix                      = null,
                                                                    Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                                    EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                                    EnergyMeter?                                         EnergyMeter                    = null,
                                                                    Boolean?                                             IsFreeOfCharge                 = null,
                                                                    IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,

                                                                    ChargingSession?                                     ChargingSession                = null,
                                                                    DateTime?                                            LastStatusUpdate               = null,
                                                                    String?                                              DataSource                     = null,
                                                                    DateTime?                                            LastChange                     = null,

                                                                    Action<IEVSE>?                                       Configurator                   = null,
                                                                    Action<VirtualEVSE>?                                 VirtualEVSEConfigurator        = null,
                                                                    RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                                    JObject?                                             CustomData                     = null,
                                                                    UserDefinedDictionary?                               InternalData                   = null,

                                                                    String                                               EllipticCurve                  = "P-256",
                                                                    ECPrivateKeyParameters?                              PrivateKey                     = null,
                                                                    PublicKeyCertificates?                               PublicKeyCertificates          = null,
                                                                    TimeSpan?                                            SelfCheckTimeSpan              = null,

                                                                    Action<IEVSE>?                                       OnSuccess                      = null,
                                                                    Action<IChargingStation, IEVSE>?                     OnError                        = null,

                                                                    Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                    EventTracking_Id?                                    EventTrackingId                = null,
                                                                    User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.AddEVSEIfNotExists(

                                   Id,
                                   Name,
                                   Description,

                                   InitialAdminStatus ?? EVSEAdminStatusTypes.Operational,
                                   InitialStatus      ?? EVSEStatusTypes.Available,
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

                                   ChargingSession,
                                   LastStatusUpdate,
                                   DataSource,
                                   LastChange,
                                   CustomData,
                                   InternalData,

                                   Configurator,
                                   newEVSE => {

                                       var virtualevse = new VirtualEVSE(

                                                             newEVSE.Id,
                                                             ChargingStation,
                                                             newEVSE.Name,
                                                             newEVSE.Description,

                                                             InitialAdminStatus,
                                                             InitialStatus,
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
                                                             SelfCheckTimeSpan

                                                         );

                                       VirtualEVSEConfigurator?.Invoke(virtualevse);

                                       return virtualevse;

                                   },

                                   OnSuccess,
                                   OnError

                               );

        #endregion

        #region AddOrUpdateVirtualEVSE   (this ChargingStation, Id = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="EVSEId">The EVSE identification for the EVSE to be created.</param>
        /// <param name="EVSEConfigurator">An optional delegate to configure the new (local) EVSE.</param>
        /// <param name="VirtualEVSEConfigurator">An optional delegate to configure the new EVSE.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static Task<AddOrUpdateEVSEResult> AddOrUpdateVirtualEVSE(this IChargingStation                                ChargingStation,

                                                                         EVSE_Id                                              Id,
                                                                         I18NString?                                          Name                           = null,
                                                                         I18NString?                                          Description                    = null,

                                                                         Timestamped<EVSEAdminStatusTypes>?                   InitialAdminStatus             = null,
                                                                         Timestamped<EVSEStatusTypes>?                        InitialStatus                  = null,
                                                                         UInt16                                               MaxAdminStatusScheduleSize     = VirtualEVSE.DefaultMaxAdminStatusScheduleSize,
                                                                         UInt16                                               MaxStatusScheduleSize          = VirtualEVSE.DefaultMaxStatusScheduleSize,

                                                                         IEnumerable<URL>?                                    PhotoURLs                      = null,
                                                                         IEnumerable<Brand>?                                  Brands                         = null,
                                                                         IEnumerable<OpenDataLicense>?                        OpenDataLicenses               = null,
                                                                         IEnumerable<ChargingModes>?                          ChargingModes                  = null,
                                                                         IEnumerable<ChargingTariff>?                         ChargingTariffs                = null,
                                                                         CurrentTypes?                                        CurrentType                    = null,
                                                                         Decimal?                                             AverageVoltage                 = null,
                                                                         Timestamped<Decimal>?                                AverageVoltageRealTime         = null,
                                                                         IEnumerable<Timestamped<Decimal>>?                   AverageVoltagePrognoses        = null,
                                                                         Decimal?                                             MaxCurrent                     = null,
                                                                         Timestamped<Decimal>?                                MaxCurrentRealTime             = null,
                                                                         IEnumerable<Timestamped<Decimal>>?                   MaxCurrentPrognoses            = null,
                                                                         Decimal?                                             MaxPower                       = null,
                                                                         Timestamped<Decimal>?                                MaxPowerRealTime               = null,
                                                                         IEnumerable<Timestamped<Decimal>>?                   MaxPowerPrognoses              = null,
                                                                         Decimal?                                             MaxCapacity                    = null,
                                                                         Timestamped<Decimal>?                                MaxCapacityRealTime            = null,
                                                                         IEnumerable<Timestamped<Decimal>>?                   MaxCapacityPrognoses           = null,
                                                                         EnergyMix?                                           EnergyMix                      = null,
                                                                         Timestamped<EnergyMix>?                              EnergyMixRealTime              = null,
                                                                         EnergyMixPrognosis?                                  EnergyMixPrognoses             = null,
                                                                         EnergyMeter?                                         EnergyMeter                    = null,
                                                                         Boolean?                                             IsFreeOfCharge                 = null,
                                                                         IEnumerable<IChargingConnector>?                     ChargingConnectors             = null,

                                                                         ChargingSession?                                     ChargingSession                = null,
                                                                         DateTime?                                            LastStatusUpdate               = null,
                                                                         String?                                              DataSource                     = null,
                                                                         DateTime?                                            LastChange                     = null,

                                                                         Action<IEVSE>?                                       Configurator                   = null,
                                                                         Action<VirtualEVSE>?                                 VirtualEVSEConfigurator        = null,
                                                                         RemoteEVSECreatorDelegate?                           RemoteEVSECreator              = null,

                                                                         JObject?                                             CustomData                     = null,
                                                                         UserDefinedDictionary?                               InternalData                   = null,

                                                                         String                                               EllipticCurve                  = "P-256",
                                                                         ECPrivateKeyParameters?                              PrivateKey                     = null,
                                                                         PublicKeyCertificates?                               PublicKeyCertificates          = null,
                                                                         TimeSpan?                                            SelfCheckTimeSpan              = null,

                                                                         Action<IEVSE>?                                       OnAdditionSuccess              = null,
                                                                         Action<IEVSE,            IEVSE>?                     OnUpdateSuccess                = null,
                                                                         Action<IChargingStation, IEVSE>?                     OnError                        = null,

                                                                         Func<ChargingStationOperator_Id, EVSE_Id, Boolean>?  AllowInconsistentOperatorIds   = null,
                                                                         EventTracking_Id?                                    EventTrackingId                = null,
                                                                         User_Id?                                             CurrentUserId                  = null)

            => ChargingStation.AddOrUpdateEVSE(

                   Id,
                   Name,
                   Description,

                   InitialAdminStatus ?? EVSEAdminStatusTypes.Operational,
                   InitialStatus      ?? EVSEStatusTypes.Available,
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

                   ChargingSession,
                   LastStatusUpdate,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData,

                   Configurator,
                   newEVSE => {

                       var virtualevse = new VirtualEVSE(

                                             newEVSE.Id,
                                             ChargingStation,
                                             newEVSE.Name,
                                             newEVSE.Description,

                                             InitialAdminStatus,
                                             InitialStatus,
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
                                             SelfCheckTimeSpan

                                         );

                       VirtualEVSEConfigurator?.Invoke(virtualevse);

                       return virtualevse;

                   },

                   OnAdditionSuccess,
                   OnUpdateSuccess,
                   OnError,

                   AllowInconsistentOperatorIds,
                   EventTrackingId,
                   CurrentUserId

               );

        #endregion


    }


    /// <summary>
    /// A virtual EVSE for (internal) tests.
    /// </summary>
    public class VirtualEVSE : ACryptoEMobilityEntity<EVSE_Id,
                                                      EVSEAdminStatusTypes,
                                                      EVSEStatusTypes>,
                               IEquatable<VirtualEVSE>, IComparable<VirtualEVSE>, IComparable,
                               IRemoteEVSE
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext                            = "https://open.charging.cloud/contexts/wwcp+json/EVSE";


        private readonly        Decimal   EPSILON                                  = 0.01m;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEAdminStatusScheduleSize    = 50;

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const            UInt16    DefaultMaxEVSEStatusScheduleSize         = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public  static readonly TimeSpan  DefaultMaxReservationDuration            = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public  static readonly TimeSpan  DefaultSelfCheckTimeSpan                 = TimeSpan.FromSeconds(15);

        private        readonly Object    EnergyMeterLock;
        private                 Timer     EnergyMeterTimer;

        private        readonly Object    ReservationExpiredLock;
        private        readonly Timer     ReservationExpiredTimer;

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id               OperatorId
            => Id.OperatorId;

        public IChargingStationOperator?                Operator
            => ChargingStation?.Operator;

        public IChargingPool?                           ChargingPool
            => ChargingStation?.ChargingPool;

        public IChargingStation?                        ChargingStation             { get; }

        public IRemoteEVSE?                             RemoteEVSE
            => null;



        /// <summary>
        /// An optional number/string printed on the outside of the EVSE for visual identification.
        /// </summary>
        public String?                                  PhysicalReference           { get; }

        /// <summary>
        /// An optional enumeration of links to photos related to the EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<URL>                         PhotoURLs                   { get; }

        /// <summary>
        /// An enumeration of all brands registered for this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<Brand>                       Brands                      { get; }

        /// <summary>
        /// An enumeration of all data license(s) of this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<OpenDataLicense>             OpenDataLicenses            { get; }

        /// <summary>
        /// An enumeration of all supported charging modes of this EVSE.
        /// </summary>
        [Mandatory, SlowData]
        public ReactiveSet<ChargingModes>               ChargingModes               { get; }

        /// <summary>
        /// An enumeration of all available charging tariffs at this EVSE.
        /// </summary>
        [Optional, SlowData]
        public ReactiveSet<ChargingTariff>              ChargingTariffs             { get; }


        #region CurrentType

        private CurrentTypes currentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory, SlowData]
        public CurrentTypes CurrentType
        {

            get
            {
                return currentType;
            }

            set
            {

                if (currentType != value)
                    SetProperty(ref currentType,
                                value);

            }

        }

        #endregion


        #region AverageVoltage

        private Decimal? averageVoltage;

        /// <summary>
        /// The average voltage [Volt].
        /// </summary>
        [Optional, SlowData]
        public Decimal? AverageVoltage
        {

            get
            {
                return averageVoltage;
            }

            set
            {

                if (value is not null)
                {

                    if (!averageVoltage.HasValue)
                        averageVoltage = value;

                    else if (Math.Abs(averageVoltage.Value - value.Value) > EPSILON)
                        SetProperty(ref averageVoltage,
                                    value);

                }
                else
                    DeleteProperty(ref averageVoltage);

            }

        }

        #endregion

        #region AverageVoltageRealTime

        private Timestamped<Decimal>? averageVoltageRealTime;

        /// <summary>
        /// The real-time average voltage [Volt].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? AverageVoltageRealTime
        {

            get
            {
                return averageVoltageRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!averageVoltageRealTime.HasValue || Math.Abs(averageVoltageRealTime.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref averageVoltageRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref averageVoltage);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the average voltage [Volt].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        AverageVoltagePrognoses     { get; }


        #region MaxCurrent

        private Decimal? maxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Decimal? MaxCurrent
        {

            get
            {
                return maxCurrent;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCurrent.HasValue)
                        maxCurrent = value;

                    else if (Math.Abs(maxCurrent.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCurrent,
                                    value);

                }
                else
                    DeleteProperty(ref maxCurrent);

            }

        }

        #endregion

        #region MaxCurrentRealTime

        private Timestamped<Decimal>? maxCurrentRealTime;

        /// <summary>
        /// The real-time maximum current [Ampere].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? MaxCurrentRealTime
        {

            get
            {
                return maxCurrentRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCurrentRealTime.HasValue || Math.Abs(maxCurrentRealTime.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxCurrentRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref maxCurrent);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum current [Ampere].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        MaxCurrentPrognoses     { get; }


        #region MaxPower

        private Decimal? maxPower;

        /// <summary>
        /// The maximum power [kW].
        /// </summary>
        [Mandatory]
        public Decimal? MaxPower
        {

            get
            {
                return maxPower;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxPower.HasValue)
                        maxPower = value;

                    else if (Math.Abs(maxPower.Value - value.Value) > EPSILON)
                        SetProperty(ref maxPower,
                                    value);

                }
                else
                    DeleteProperty(ref maxPower);

            }

        }

        #endregion

        #region MaxPowerRealTime

        private Timestamped<Decimal>? maxPowerRealTime;

        /// <summary>
        /// The real-time maximum power [kW].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? MaxPowerRealTime
        {

            get
            {
                return maxPowerRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxPowerRealTime.HasValue || Math.Abs(maxPowerRealTime.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxPowerRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref maxPower);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum power [kW].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        MaxPowerPrognoses     { get; }


        #region MaxCapacity

        private Decimal? maxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Decimal? MaxCapacity
        {

            get
            {
                return maxCapacity;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCapacity.HasValue)
                        maxCapacity = value;

                    else if (Math.Abs(maxCapacity.Value - value.Value) > EPSILON)
                        SetProperty(ref maxCapacity,
                                    value);

                }
                else
                    DeleteProperty(ref maxCapacity);

            }

        }

        #endregion

        #region MaxCapacityRealTime

        private Timestamped<Decimal>? maxCapacityRealTime;

        /// <summary>
        /// The real-time maximum capacity [kWh].
        /// </summary>
        [Optional, FastData]
        public Timestamped<Decimal>? MaxCapacityRealTime
        {

            get
            {
                return maxCapacityRealTime;
            }

            set
            {

                if (value is not null)
                {

                    if (!maxCapacityRealTime.HasValue || Math.Abs(maxCapacityRealTime.Value.Value - value.Value.Value) > EPSILON)
                        SetProperty(ref maxCapacityRealTime,
                                    value);

                }
                else
                    DeleteProperty(ref maxCapacity);

            }

        }

        #endregion

        /// <summary>
        /// Prognoses on future values of the maximum capacity [kWh].
        /// </summary>
        [Optional, FastData]
        public ReactiveSet<Timestamped<Decimal>>        MaxCapacityPrognoses     { get; }


        #region EnergyMix

        private EnergyMix? energyMix;

        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional, SlowData]
        public EnergyMix? EnergyMix
        {

            get
            {
                return energyMix ?? ChargingStation?.EnergyMix;
            }

            set
            {

                if (value != energyMix && value != ChargingStation?.EnergyMix)
                {

                    if (value == null)
                        DeleteProperty(ref energyMix);

                    else
                        SetProperty(ref energyMix, value);

                }

            }

        }

        #endregion

        #region EnergyMixRealTime

        private Timestamped<EnergyMix>? energyMixRealTime;

        /// <summary>
        /// The current energy mix.
        /// </summary>
        [Optional, FastData]
        public Timestamped<EnergyMix>? EnergyMixRealTime
        {

            get
            {
                return energyMixRealTime;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref energyMixRealTime,
                                value);

                else
                    DeleteProperty(ref energyMixRealTime);

            }

        }

        #endregion

        #region EnergyMixPrognoses

        private EnergyMixPrognosis? energyMixPrognoses;

        /// <summary>
        /// Prognoses on future values of the energy mix.
        /// </summary>
        [Optional, FastData]
        public EnergyMixPrognosis? EnergyMixPrognoses
        {

            get
            {
                return energyMixPrognoses ?? ChargingStation?.EnergyMixPrognoses;
            }

            set
            {

                if (value != energyMixPrognoses && value != ChargingStation?.EnergyMixPrognoses)
                {

                    if (value == null)
                        DeleteProperty(ref energyMixPrognoses);

                    else
                        SetProperty(ref energyMixPrognoses, value);

                }

            }

        }

        #endregion


        #region MaxReservationDuration

        private TimeSpan maxReservationDuration;

        /// <summary>
        /// The maximum reservation time at this EVSE.
        /// </summary>
        [Optional, SlowData]
        public TimeSpan MaxReservationDuration
        {

            get
            {
                return maxReservationDuration;
            }

            set
            {
                if (maxReservationDuration.TotalSeconds != value.TotalSeconds)
                    SetProperty(ref maxReservationDuration,
                                value);
            }

        }

        #endregion

        #region IsFreeOfCharge

        private Boolean isFreeOfCharge;

        /// <summary>
        /// Charging at this EVSE is ALWAYS free of charge.
        /// </summary>
        [Optional, SlowData]
        public Boolean IsFreeOfCharge
        {

            get
            {
                return isFreeOfCharge;
            }

            set
            {
                if (isFreeOfCharge != value)
                    SetProperty(ref isFreeOfCharge,
                                value);
            }

        }

        #endregion


        #region EnergyMeter

        private EnergyMeter? energyMeter;

        /// <summary>
        /// The smart energy meter attached to this EVSE.
        /// </summary>
        [Optional, SlowData]
        public EnergyMeter? EnergyMeter
        {

            get
            {
                return energyMeter;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref energyMeter, value);

                else
                    DeleteProperty(ref energyMeter);

            }

        }

        #endregion


        #region ChargingConnectors

        private ReactiveSet<IChargingConnector> _ChargingConnectors;

        public ReactiveSet<IChargingConnector> ChargingConnectors
        {

            get
            {
                return _ChargingConnectors;
            }

            set
            {

                if (_ChargingConnectors != value)
                    SetProperty(ref _ChargingConnectors, value);

            }

        }

        #endregion


        public DateTime?               LastStatusUpdate         { get; set; }


        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan                SelfCheckTimeSpan        { get; }




        public TimeSpan                EnergyMeterInterval      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual EVSE.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="MaxAdminStatusScheduleSize">The maximum size of the EVSE admin status schedule.</param>
        /// <param name="MaxStatusScheduleSize">The maximum size of the EVSE status schedule.</param>
        internal VirtualEVSE(EVSE_Id                             Id,
                             IChargingStation                    ChargingStation,
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
                             Timestamped<Decimal>?               AverageVoltageRealTime       = null,
                             IEnumerable<Timestamped<Decimal>>?  AverageVoltagePrognoses      = null,
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
                             IEnumerable<IChargingConnector>?    ChargingConnectors           = null,

                             String?                             EllipticCurve                = null,
                             ECPrivateKeyParameters?             PrivateKey                   = null,
                             PublicKeyCertificates?              PublicKeyCertificates        = null,
                             TimeSpan?                           SelfCheckTimeSpan            = null)

            : base(Id,
                   ChargingStation.RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus         ?? EVSEAdminStatusTypes.Operational,
                   InitialStatus              ?? EVSEStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxEVSEAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxEVSEStatusScheduleSize)

        {

            #region Init data and properties

            this.ChargingStation                    = ChargingStation;


            this.PhotoURLs                          = PhotoURLs is null
                                                          ? new ReactiveSet<URL>()
                                                          : new ReactiveSet<URL>(PhotoURLs);
            this.PhotoURLs.OnSetChanged            += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("PhotoURLs",
                                oldItems,
                                newItems);

            };

            this.Brands                             = Brands is null
                                                          ? new ReactiveSet<Brand>()
                                                          : new ReactiveSet<Brand>(Brands);
            this.Brands.OnSetChanged               += (timestamp, sender, newItems, oldItems) => {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };

            this.OpenDataLicenses                       = OpenDataLicenses is null
                                                          ? new ReactiveSet<OpenDataLicense>()
                                                          : new ReactiveSet<OpenDataLicense>(OpenDataLicenses);
            this.OpenDataLicenses.OnSetChanged         += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("DataLicenses",
                                oldItems,
                                newItems);

            };

            this.ChargingModes                      = ChargingModes is null
                                                          ? new ReactiveSet<ChargingModes>()
                                                          : new ReactiveSet<ChargingModes>(ChargingModes);
            this.ChargingModes.OnSetChanged        += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingModes",
                                oldItems,
                                newItems);

            };

            this.ChargingTariffs                    = ChargingTariffs is null
                                                          ? new ReactiveSet<ChargingTariff>()
                                                          : new ReactiveSet<ChargingTariff>(ChargingTariffs);
            this.ChargingTariffs.OnSetChanged      += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingTariffs",
                                oldItems,
                                newItems);

            };

            this.currentType                        = CurrentType ?? CurrentTypes.AC_ThreePhases;

            this.averageVoltage                     = AverageVoltage;
            this.averageVoltageRealTime             = AverageVoltageRealTime;

            this.AverageVoltagePrognoses            = AverageVoltagePrognoses is null
                                                          ? new ReactiveSet<Timestamped<Decimal>>()
                                                          : new ReactiveSet<Timestamped<Decimal>>(AverageVoltagePrognoses);
            this.AverageVoltagePrognoses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("AverageVoltagePrognoses",
                                oldItems,
                                newItems);

            };


            this.maxCurrent                         = MaxCurrent;
            this.maxCurrentRealTime                 = MaxCurrentRealTime;

            this.MaxCurrentPrognoses                = MaxCurrentPrognoses is null
                                                          ? new ReactiveSet<Timestamped<Decimal>>()
                                                          : new ReactiveSet<Timestamped<Decimal>>(MaxCurrentPrognoses);
            this.MaxCurrentPrognoses.OnSetChanged  += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCurrentPrognoses",
                                oldItems,
                                newItems);

            };

            this.maxPower                           = MaxPower;
            this.maxPowerRealTime                   = MaxPowerRealTime;

            this.MaxPowerPrognoses                  = MaxPowerPrognoses is null
                                                          ? new ReactiveSet<Timestamped<Decimal>>()
                                                          : new ReactiveSet<Timestamped<Decimal>>(MaxPowerPrognoses);
            this.MaxPowerPrognoses.OnSetChanged    += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxPowerPrognoses",
                                oldItems,
                                newItems);

            };

            this.maxCapacity                        = MaxCapacity;
            this.maxCapacityRealTime                = MaxCapacityRealTime;

            this.MaxCapacityPrognoses               = MaxCapacityPrognoses is null
                                                          ? new ReactiveSet<Timestamped<Decimal>>()
                                                          : new ReactiveSet<Timestamped<Decimal>>(MaxCapacityPrognoses);
            this.MaxCapacityPrognoses.OnSetChanged += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("MaxCapacityPrognoses",
                                oldItems,
                                newItems);

            };

            this.energyMix                          = EnergyMix;
            this.energyMixRealTime                  = EnergyMixRealTime;
            this.energyMixPrognoses                 = EnergyMixPrognoses;

            this.energyMeter                        = EnergyMeter;

            this.IsFreeOfCharge                     = IsFreeOfCharge ?? false;

            this.ChargingConnectors                      = ChargingConnectors is null
                                                          ? new ReactiveSet<IChargingConnector>()
                                                          : new ReactiveSet<IChargingConnector>(ChargingConnectors);
            this.ChargingConnectors.OnSetChanged        += (timestamp, reactiveSet, newItems, oldItems) =>
            {

                PropertyChanged("ChargingConnectors",
                                oldItems,
                                newItems);

            };

            this.ChargingModes          = new ReactiveSet<ChargingModes>();
            this._ChargingConnectors    = new ReactiveSet<IChargingConnector>();

            this.energyMeter            = EnergyMeter;

            this.SelfCheckTimeSpan      = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;

            this.reservations           = new Dictionary<ChargingReservation_Id, ChargingReservation>();

            #endregion

            #region Setup crypto

            if (PrivateKey == null && PublicKeyCertificates == null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKeys:      new PublicKeyLifetime[] {
                                                                       new PublicKeyLifetime(
                                                                           PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                           NotBefore:  Timestamp.Now,
                                                                           NotAfter:   Timestamp.Now + TimeSpan.FromDays(365),
                                                                           Algorithm:  "P-256",
                                                                           Comment:    I18NString.Empty
                                                                       )
                                                                   },
                                                  Description:     I18NString.Create(Languages.en, "Auto-generated test keys for a virtual EVSE!"),
                                                  Operations:      JSONObject.Create(
                                                                       new JProperty("signMeterValues",  true),
                                                                       new JProperty("signCertificates", false)
                                                                   ),
                                                  EVSEId:          Id,
                                                  EnergyMeterId:   EnergyMeter?.Id);

            }

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateAdminStatus(timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            this.statusSchedule.     OnStatusChanged += (timestamp, eventTrackingId, statusSchedule, newStatus, oldStatus, dataSource)
                                                          => UpdateStatus     (timestamp, eventTrackingId, newStatus, oldStatus, dataSource);

            #endregion

            ReservationExpiredLock   = new Object();
            ReservationExpiredTimer  = new Timer(CheckIfReservationIsExpired, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            EnergyMeterLock          = new Object();
            EnergyMeterTimer         = new Timer(ReadEnergyMeter,             null, Timeout.Infinite,        Timeout.Infinite);
            EnergyMeterInterval      = TimeSpan.FromSeconds(30);

        }

        //event OnEVSEAdminStatusChangedDelegate? IEVSE.OnAdminStatusChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //event OnEVSEDataChangedDelegate? IEVSE.OnDataChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //event OnEVSEStatusChangedDelegate? IEVSE.OnStatusChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion


        #region (private, Timer) ReadEnergyMeter(Status)

        private void ReadEnergyMeter(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(EnergyMeterLock))
            {

                try
                {

                    ChargingSession.AddEnergyMeterValue(new EnergyMeteringValue(Timestamp.Now, 1));

                }
                catch (Exception e)
                {
                    DebugX.LogT("'ReadEnergyMeter' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(EnergyMeterLock);
                }

            }

            else
                DebugX.LogT("'ReadEnergyMeter' skipped!");

        }

        #endregion


        #region UpdateWith(OtherEVSE)

        /// <summary>
        /// Update this EVSE with the data of the other EVSE.
        /// </summary>
        /// <param name="OtherEVSE">Another EVSE.</param>
        public IEVSE UpdateWith(IEVSE OtherEVSE)
        {

            Name.                   Set    (OtherEVSE.Name);
            Description.            Set    (OtherEVSE.Description);

            Brands.                 Replace(OtherEVSE.Brands);
            ChargingModes.          Replace(OtherEVSE.ChargingModes);
            ChargingConnectors.          Replace(OtherEVSE.ChargingConnectors);
            OpenDataLicenses.       Replace(OtherEVSE.OpenDataLicenses);
            AverageVoltagePrognoses.Replace(OtherEVSE.AverageVoltagePrognoses);
            MaxCurrentPrognoses.    Replace(OtherEVSE.MaxCurrentPrognoses);
            MaxPowerPrognoses.      Replace(OtherEVSE.MaxPowerPrognoses);
            MaxCapacityPrognoses.   Replace(OtherEVSE.MaxCapacityPrognoses);

            CurrentType                = OtherEVSE.CurrentType;
            AverageVoltage             = OtherEVSE.AverageVoltage;
            AverageVoltageRealTime     = OtherEVSE.AverageVoltageRealTime;
            MaxCurrent                 = OtherEVSE.MaxCurrent;
            MaxCurrentRealTime         = OtherEVSE.MaxCurrentRealTime;
            MaxPower                   = OtherEVSE.MaxPower;
            MaxPowerRealTime           = OtherEVSE.MaxPowerRealTime;
            MaxCapacity                = OtherEVSE.MaxCapacity;
            MaxCapacityRealTime        = OtherEVSE.MaxCapacityRealTime;
            EnergyMix                  = OtherEVSE.EnergyMix;               //ToDo: Implement Equality!
            EnergyMixRealTime          = OtherEVSE.EnergyMixRealTime;       //ToDo: Implement Equality!
            EnergyMixPrognoses         = OtherEVSE.EnergyMixPrognoses;      //ToDo: Implement Equality!
            EnergyMeter                = OtherEVSE.EnergyMeter;             //ToDo: Implement Equality!
            IsFreeOfCharge             = OtherEVSE.IsFreeOfCharge;
            MaxReservationDuration     = OtherEVSE.MaxReservationDuration;

            if (OtherEVSE.AdminStatus.Timestamp > AdminStatus.Timestamp)
                AdminStatus            = OtherEVSE.AdminStatus;

            if (OtherEVSE.Status.     Timestamp > Status.     Timestamp)
                Status                 = OtherEVSE.Status;

            return this;

        }

        #endregion


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of the EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnAdminStatusChanged;

        #endregion


        public void SetAdminStatus(EVSEAdminStatus EVSEAdminStatus)
        {

            //adminStatusSchedule.Insert(EVSEAdminStatus.Status,
            //                           EVSEAdminStatus.Timestamp,
            //                           EVSEAdminStatus.DataSource);

        }

        public void SetAdminStatus(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)
        {

        }

        public void SetStatus(EVSEStatus EVSEStatus)
        {

            statusSchedule.Insert(EVSEStatus.Status,
                                  EVSEStatus.Timestamp,
                                  EVSEStatus.Context);

        }

        public void SetStatus(EVSEStatusUpdate EVSEStatusUpdate)
        {

            statusSchedule.Insert(EVSEStatusUpdate.NewStatus,
                                  EVSEStatusUpdate.Context);

        }

        public void SetEnergyStatus(EVSEEnergyStatus EVSEEnergyStatus)
        {

        }

        public void SetEnergyStatus(EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate)
        {

        }


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                            Timestamp,
                                              EventTracking_Id                    EventTrackingId,
                                              Timestamped<EVSEAdminStatusTypes>   NewStatus,
                                              Timestamped<EVSEAdminStatusTypes>?  OldStatus    = null,
                                              Context?                            DataSource   = null)
        {

            var onAdminStatusChanged = OnAdminStatusChanged;
            if (onAdminStatusChanged is not null)
                await onAdminStatusChanged(Timestamp,
                                           EventTrackingId,
                                           this,
                                           NewStatus,
                                           OldStatus,
                                           DataSource);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                       Timestamp,
                                         EventTracking_Id               EventTrackingId,
                                         Timestamped<EVSEStatusTypes>   NewStatus,
                                         Timestamped<EVSEStatusTypes>?  OldStatus    = null,
                                         Context?                       DataSource   = null)
        {

            var onStatusChanged = OnStatusChanged;
            if (onStatusChanged is not null)
                await onStatusChanged(Timestamp,
                                      EventTrackingId,
                                      this,
                                      NewStatus,
                                      OldStatus,
                                      DataSource);

        }

        #endregion

        #endregion

        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => reservations.TryGetValue(ReservationId, out Reservation);

        #endregion

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
        /// Reserve the possibility to charge at this EVSE.
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                          StartTime              = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken                  CancellationToken      = default,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)


                => Reserve(ChargingLocation.FromEVSEId(Id),
                           ChargingReservationLevel.EVSE,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout);

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken                  CancellationToken      = default,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnReserveRequest));
            }

            #endregion

            try
            {

                if (ChargingLocation.EVSEId.HasValue && ChargingLocation.EVSEId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (reservations)
                    {

                        #region Check if this is a reservation update...

                        if (ReservationId.HasValue &&
                            reservations.TryGetValue(ReservationId.Value, out ChargingReservation oldReservation))
                        {

                            //ToDo: Calc if this reservation update is possible!
                            //      When their are other reservations => conflicts!

                            var updatedReservation  = reservations[ReservationId.Value]
                                                    = new ChargingReservation(oldReservation.Id,
                                                                              Timestamp.Value,
                                                                              oldReservation.StartTime,
                                                                              Duration ?? MaxReservationDuration,
                                                                              (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                                              oldReservation.ConsumedReservationTime + oldReservation.Duration - oldReservation.TimeLeft,
                                                                              ReservationLevel,
                                                                              ProviderId,
                                                                              RemoteAuthentication,
                                                                              RoamingNetwork.Id,
                                                                              null, //ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                                              null, //ChargingStation.ChargingPool.Id,
                                                                              null, //ChargingStation.Id,
                                                                              Id,
                                                                              ChargingProduct,
                                                                              AuthTokens,
                                                                              eMAIds,
                                                                              PINs);

                            OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, this, updatedReservation);

                            result = ReservationResult.Success(updatedReservation);

                        }

                        #endregion

                        #region ...or a new reservation

                        else
                        {

                            if (Status.Value == EVSEStatusTypes.OutOfService)
                                result = ReservationResult.OutOfService;

                            else if (Status.Value == EVSEStatusTypes.Charging ||
                                     Status.Value == EVSEStatusTypes.Reserved ||
                                     Status.Value == EVSEStatusTypes.Available)
                            {

                                 newReservation = new ChargingReservation(
                                                      Id:                      ReservationId ?? ChargingReservation_Id.Random(OperatorId),
                                                      Timestamp:               Timestamp.Value,
                                                      StartTime:               ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      Duration:                Duration  ?? MaxReservationDuration,
                                                      EndTime:                 (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                      ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                      ReservationLevel:        ReservationLevel,
                                                      ProviderId:              ProviderId,
                                                      StartAuthentication:     RemoteAuthentication,
                                                      RoamingNetworkId:        RoamingNetwork.Id,
                                                      ChargingPoolId:          null,
                                                      ChargingStationId:       null,
                                                      EVSEId:                  Id,
                                                      ChargingProduct:         ChargingProduct,
                                                      AuthTokens:              AuthTokens,
                                                      eMAIds:                  eMAIds,
                                                      PINs:                    PINs ?? (new[] { RandomExtensions.RandomUInt32(1000000) + 100000U })
                                                  );

                                 reservations.Add(newReservation.Id, newReservation);

                                 result = ReservationResult.Success(newReservation);

                            }

                            else
                                result = ReservationResult.Error();

                        }

                        #endregion

                    }

                }
                else
                {
                    result = AdminStatus.Value switch {
                        _ => ReservationResult.OutOfService,
                    };
                }


                if (result.Result  == ReservationResultType.Success &&
                    newReservation != null)
                {

                    Status = EVSEStatusTypes.Reserved;

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

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
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
                                          EndTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return Task.FromResult(result);

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
        public Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp           = null,
                              CancellationToken                      CancellationToken   = default,
                              EventTracking_Id?                      EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation?     canceledReservation  = null;
            CancelReservationResult? result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (reservations)
                    {

                        if (!reservations.TryGetValue(ReservationId, out canceledReservation))
                            return Task.FromResult(CancelReservationResult.UnknownReservationId(ReservationId,
                                                                                                Reason));

                        reservations.Remove(ReservationId);

                    }

                    result = CancelReservationResult.Success(ReservationId,
                                                             Reason,
                                                             canceledReservation);

                }

                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = CancelReservationResult.OutOfService(ReservationId,
                                                                          Reason);
                            break;

                    }

                }


                if (result.Result == CancelReservationResultTypes.Success)
                {

                    if (Status.Value == EVSEStatusTypes.Reserved &&
                    !reservations.Any())
                    {
                        // Will send events!
                        Status = EVSEStatusTypes.Available;
                    }

                    OnReservationCanceled?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                  this,
                                                  canceledReservation,
                                                  Reason);

                }


            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return Task.FromResult(result);

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

                    ChargingReservation[] expiredReservations = null;

                    lock (reservations)
                    {
                        expiredReservations = reservations.Values.Where(reservation => reservation.IsExpired()).ToArray();
                    }

                    foreach (var expiredReservation in expiredReservations)
                    {

                        lock (reservations)
                        {
                            reservations.Remove(expiredReservation.Id);
                        }

                        if (Status.Value == EVSEStatusTypes.Reserved &&
                            !reservations.Any())
                        {
                            // Will send events!
                            Status = EVSEStatusTypes.Available;
                        }

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

        #endregion

        #region RemoteStart/-Stop and Sessions...

        #region Data

        private ChargingSession? chargingSession;


        public IEnumerable<ChargingSession> ChargingSessions

            => chargingSession is not null
                   ? new ChargingSession[] { chargingSession }
                   : Array.Empty<ChargingSession>();


        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
        {

            if (SessionId == chargingSession.Id)
            {
                ChargingSession = chargingSession;
                return true;
            }

            ChargingSession = null;
            return false;

        }

        #endregion

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession? ChargingSession
        {

            get
            {
                return chargingSession;
            }

            set
            {

                // Skip, if the charging session is already known... 
                if (chargingSession != value)
                {

                    chargingSession = value;

                    if (chargingSession is not null)
                    {

                        Status = EVSEStatusTypes.Charging;

                        OnNewChargingSession?.Invoke(Timestamp.Now,
                                                     this,
                                                     chargingSession);

                    }

                    else
                        Status = EVSEStatusTypes.Available;

                }

            }

        }

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken        CancellationToken      = default,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromEVSEId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken        CancellationToken      = default,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)
        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(startTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.EVSEId.HasValue &&
                    ChargingLocation.EVSEId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation();
                }

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {


                    #region Available

                    if (Status.Value == EVSEStatusTypes.Available ||
                        Status.Value == EVSEStatusTypes.DoorNotClosed)
                    {

                        chargingSession = new ChargingSession(SessionId ?? ChargingSession_Id.NewRandom) {
                            EventTrackingId      = EventTrackingId,
                            ReservationId        = ReservationId,
                            Reservation          = reservations.Values.FirstOrDefault(reservation => reservation.Id == ReservationId),
                            EVSEId               = Id,
                            ChargingProduct      = ChargingProduct,
                            ProviderIdStart      = ProviderId,
                            AuthenticationStart  = RemoteAuthentication
                        };

                        chargingSession.AddEnergyMeterValue(new EnergyMeteringValue(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, 0));
                        EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                        Status = EVSEStatusTypes.Charging;

                        result = RemoteStartResult.Success(chargingSession);

                    }

                    #endregion

                    #region Reserved

                    else if (Status.Value == EVSEStatusTypes.Reserved)
                    {

                        var firstReservation = reservations.Values.OrderBy(reservation => reservation.StartTime).FirstOrDefault();

                        #region Not matching reservation identifications...

                        if (firstReservation != null && !ReservationId.HasValue)
                            result = RemoteStartResult.Reserved(I18NString.Create(Languages.en, "Missing reservation identification!"));

                        else if (firstReservation != null && ReservationId.HasValue && firstReservation.Id != ReservationId.Value)
                            result = RemoteStartResult.Reserved(I18NString.Create(Languages.en, "Invalid reservation identification!"));

                        #endregion

                        #region ...or a matching reservation identification!

                        // Check if this remote start is allowed!
                        else if (RemoteAuthentication?.RemoteIdentification.HasValue == true &&
                            !firstReservation.eMAIds.Contains(RemoteAuthentication.RemoteIdentification.Value))
                        {
                            result = RemoteStartResult.InvalidCredentials();
                        }

                        else
                        {

                            firstReservation.AddToConsumedReservationTime(firstReservation.Duration - firstReservation.TimeLeft);

                            // Will also set the status -> EVSEStatusType.Charging;
                            chargingSession = new ChargingSession(SessionId ?? ChargingSession_Id.NewRandom) {
                                EventTrackingId      = EventTrackingId,
                                ReservationId        = ReservationId,
                                Reservation          = firstReservation,
                                EVSEId               = Id,
                                ChargingProduct      = ChargingProduct,
                                ProviderIdStart      = ProviderId,
                                AuthenticationStart  = RemoteAuthentication
                            };

                            firstReservation.ChargingSession = ChargingSession;

                            chargingSession.AddEnergyMeterValue(new EnergyMeteringValue(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, 0));
                            EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                            Status = EVSEStatusTypes.Charging;

                            result = RemoteStartResult.Success(chargingSession);

                        }

                        #endregion

                    }

                    #endregion

                    #region Charging

                    else if (Status.Value == EVSEStatusTypes.Charging)
                        result = RemoteStartResult.AlreadyInUse();

                    #endregion

                    #region OutOfService

                    else if (Status.Value == EVSEStatusTypes.OutOfService)
                        result = RemoteStartResult.OutOfService();

                    #endregion

                    #region Offline

                    else if (Status.Value == EVSEStatusTypes.Offline)
                        result = RemoteStartResult.Offline();

                    #endregion

                    else
                        result = RemoteStartResult.Error("Could not start charging!");

                }
                else
                {

                    result = AdminStatus.Value switch {
                        _ => RemoteStartResult.OutOfService(),
                    };
                }


            } catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(endTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              null,
                                              null,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return Task.FromResult(result);

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
        public Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken      CancellationToken      = default,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    #region Available

                    if (Status.Value == EVSEStatusTypes.Available)
                        result = RemoteStopResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Reserved

                    else if (Status.Value == EVSEStatusTypes.Reserved)
                        result = RemoteStopResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Charging

                    else if (Status.Value == EVSEStatusTypes.Charging)
                    {

                        #region Matching session identification...

                        if (chargingSession?.Id == SessionId)
                        {

                            EnergyMeterTimer.Change(Timeout.Infinite, Timeout.Infinite);

                            var __ChargingSession    = chargingSession;
                            var now                  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            var sessionTime          = __ChargingSession.SessionTime;
                            sessionTime.EndTime      = now;
                            __ChargingSession.SessionTime.EndTime = now;
                            var duration             = now - __ChargingSession.SessionTime.StartTime;
                            var consumption          = MaxPower.HasValue
                                                           ? Math.Round(((Decimal) duration.TotalHours) * MaxPower.Value, 2)
                                                           : 0;

                            __ChargingSession.AddEnergyMeterValue(new EnergyMeteringValue(now, consumption));

                            var chargeDetailRecord  = new ChargeDetailRecord(
                                                          Id:                        ChargeDetailRecord_Id.Parse(__ChargingSession.Id.ToString()),
                                                          SessionId:                 __ChargingSession.Id,
                                                          Reservation:               __ChargingSession.Reservation,
                                                          EVSEId:                    __ChargingSession.EVSEId,
                                                          EVSE:                      __ChargingSession.EVSE,
                                                          ChargingStation:           __ChargingSession.EVSE?.ChargingStation,
                                                          ChargingPool:              __ChargingSession.EVSE?.ChargingStation?.ChargingPool,
                                                          ChargingStationOperator:   __ChargingSession.EVSE?.Operator,
                                                          ChargingProduct:           __ChargingSession.ChargingProduct,
                                                          ProviderIdStart:           __ChargingSession.ProviderIdStart,
                                                          ProviderIdStop:            __ChargingSession.ProviderIdStop,
                                                          SessionTime:               __ChargingSession.SessionTime,

                                                          AuthenticationStart:       __ChargingSession.AuthenticationStart,
                                                          AuthenticationStop:        __ChargingSession.AuthenticationStop,

                                                          EnergyMeterId:             EnergyMeter?.Id,
                                                          EnergyMeteringValues:      __ChargingSession.EnergyMeteringValues
                                                      );

                            // Will do: Status = EVSEStatusType.Available
                            ChargingSession = null;

                            if (!ReservationHandling.HasValue ||
                                (ReservationHandling.HasValue &&
                                !ReservationHandling.Value.IsKeepAlive))
                            {

                                // Will do: Status = EVSEStatusType.Available
                                //Reservation = null;

                            }

                            else
                            {
                                //ToDo: Reservation will live on!
                            }


                            //OnNewChargeDetailRecord?.Invoke(Timestamp.Now,
                            //                                this,
                            //                                _ChargeDetailRecord);

                            result = RemoteStopResult.Success(SessionId,
                                                              null,
                                                              null,
                                                              __ChargingSession.Reservation?.Id,
                                                              ReservationHandling,
                                                              chargeDetailRecord);

                        }

                        #endregion

                        #region ...or unknown session identification!

                            else
                                result = RemoteStopResult.InvalidSessionId(SessionId);

                            #endregion

                    }

                    #endregion

                    #region OutOfService

                    else if (Status.Value == EVSEStatusTypes.OutOfService)
                        result = RemoteStopResult.OutOfService(SessionId);

                    #endregion

                    #region Offline

                    else if (Status.Value == EVSEStatusTypes.Offline)
                        result = RemoteStopResult.Offline(SessionId);

                    #endregion

                    else
                        result = RemoteStopResult.Error(SessionId);

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStopResult.OutOfService(SessionId);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #endregion


        #region ToJSON(this EVSE, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public JObject ToJSON(Boolean                                  Embedded                          = false,
                              InfoStatus                               ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                               ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                               ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                               ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                              InfoStatus                               ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                               ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IEVSE>?  CustomEVSESerializer              = null)

            => ToJSON(Embedded,
                      ExpandRoamingNetworkId,
                      ExpandChargingStationOperatorId,
                      ExpandChargingPoolId,
                      ExpandChargingStationId,
                      ExpandBrandIds,
                      ExpandDataLicenses,
                      CustomEVSESerializer,
                      null);


        /// <summary>
        /// Return a JSON representation of the given EVSE.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public JObject ToJSON(Boolean                                        Embedded                          = false,
                              InfoStatus                                     ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                              InfoStatus                                     ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<IEVSE>?        CustomEVSESerializer              = null,
                              CustomJObjectSerializerDelegate<VirtualEVSE>?  CustomVirtualEVSESerializer       = null)

        {

            try
            {

                var json = JSONObject.Create(

                               new JProperty("@id", Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context", JSONLDContext)
                                   : null,

                               Description.IsNeitherNullNorEmpty()
                                   ? new JProperty("description", Description.ToJSON())
                                   : null,

                               Brands.SafeAny()
                                     ? ExpandBrandIds.Switch(
                                           () => new JProperty("brandId", Brands.Select(brand => brand.Id.ToString())),
                                           () => new JProperty("brand",   Brands.ToJSON()))
                                     : null,

                               (!Embedded || (ChargingStation is not null && DataSource       != ChargingStation.DataSource))
                                   ? new JProperty("dataSource", DataSource)
                                   : null,

                               (!Embedded || (ChargingStation is not null && OpenDataLicenses != ChargingStation.DataLicenses))
                                   ? ExpandDataLicenses.Switch(
                                         () => new JProperty("openDataLicenseIds", new JArray(OpenDataLicenses.SafeSelect(license => license.Id.ToString()))),
                                         () => new JProperty("openDataLicenses", OpenDataLicenses.ToJSON()))
                                   : null,

                               ExpandRoamingNetworkId != InfoStatus.Hidden && RoamingNetwork is not null
                                   ? ExpandRoamingNetworkId.Switch(
                                         () => new JProperty("roamingNetworkId",                  RoamingNetwork.Id.ToString()),
                                         () => new JProperty("roamingNetwork",                    RoamingNetwork.ToJSON(Embedded:                          true,
                                                                                                                        ExpandChargingStationOperatorIds:  InfoStatus.Hidden,
                                                                                                                        ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                        ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                        ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                        ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                        ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               ExpandChargingStationOperatorId != InfoStatus.Hidden && Operator is not null
                                   ? ExpandChargingStationOperatorId.Switch(
                                         () => new JProperty("chargingStationOperatorperatorId",  Operator.Id.ToString()),
                                         () => new JProperty("chargingStationOperatorperator",    Operator.ToJSON(Embedded:                          true,
                                                                                                                  ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                  ExpandChargingPoolIds:             InfoStatus.Hidden,
                                                                                                                  ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                  ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                  ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                  ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               ExpandChargingPoolId != InfoStatus.Hidden && ChargingPool is not null
                                   ? ExpandChargingPoolId.Switch(
                                         () => new JProperty("chargingPoolId",                    ChargingPool.Id.ToString()),
                                         () => new JProperty("chargingPool",                      ChargingPool.ToJSON(Embedded:                          true,
                                                                                                                      ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                      ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                      ExpandChargingStationIds:          InfoStatus.Hidden,
                                                                                                                      ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                      ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                      ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               ExpandChargingStationId != InfoStatus.Hidden && ChargingStation is not null
                                   ? ExpandChargingStationId.Switch(
                                         () => new JProperty("chargingStationId",                 ChargingStation.Id.ToString()),
                                         () => new JProperty("chargingStation",                   ChargingStation.ToJSON(Embedded:                          true,
                                                                                                                         ExpandRoamingNetworkId:            InfoStatus.Hidden,
                                                                                                                         ExpandChargingStationOperatorId:   InfoStatus.Hidden,
                                                                                                                         ExpandEVSEIds:                     InfoStatus.Hidden,
                                                                                                                         ExpandBrandIds:                    InfoStatus.Hidden,
                                                                                                                         ExpandDataLicenses:                InfoStatus.Hidden)))
                                   : null,

                               !Embedded && ChargingStation is not null && ChargingPool is not null && (ChargingStation.GeoLocation.HasValue || ChargingPool.GeoLocation.HasValue)
                                   ? new JProperty("geoLocation",          (ChargingStation.GeoLocation ?? ChargingPool.GeoLocation)?.ToJSON())
                                   : null,

                               !Embedded && ChargingStation is not null && ChargingPool is not null && (ChargingStation.Address is not null  || ChargingPool.Address is not null)
                                   ? new JProperty("address",              (ChargingStation.Address ?? ChargingPool.Address)?.ToJSON())
                                   : null,

                               !Embedded && ChargingStation is not null && ChargingStation.AuthenticationModes.Any()
                                   ? new JProperty("authenticationModes",  ChargingStation.AuthenticationModes.ToJSON())
                                   : null,

                               ChargingModes.SafeAny()
                                   ? new JProperty("chargingModes", new JArray(ChargingModes.SafeSelect(chargingMode => chargingMode.ToText())))
                                   : null,

                               new JProperty("currentType", CurrentType.ToText()),

                               AverageVoltage.HasValue && AverageVoltage   > 0
                                   ? new JProperty("averageVoltage", Math.Round(AverageVoltage.Value, 2))
                                   : null,

                               MaxCurrent.    HasValue && MaxCurrent       > 0
                                   ? new JProperty("maxCurrent",     Math.Round(MaxCurrent.    Value, 2))
                                   : null,

                               MaxPower.      HasValue && MaxPower.   HasValue
                                   ? new JProperty("maxPower",       Math.Round(MaxPower.      Value, 2))
                                   : null,

                               MaxCapacity.   HasValue && MaxCapacity.HasValue
                                   ? new JProperty("maxCapacity",    Math.Round(MaxCapacity.   Value, 2))
                                   : null,

                               ChargingConnectors.Count > 0
                                   ? new JProperty("socketOutlets", new JArray(ChargingConnectors.ToJSON()))
                                   : null,

                               EnergyMeter is not null
                                   ? new JProperty("energyMeter", EnergyMeter.ToJSON())
                                   : null,

                               !Embedded && ChargingStation?.OpeningTimes is not null
                                   ? new JProperty("openingTimes", ChargingStation.OpeningTimes.ToJSON())
                                   : null

                         );

                return CustomVirtualEVSESerializer is not null
                           ? CustomVirtualEVSESerializer(this, json)
                           : json;

                var ss = CustomEVSESerializer is not null
                           ? CustomEVSESerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                return new JObject(
                           new JProperty("@id",         Id.ToString()),
                           new JProperty("@context",    JSONLDContext),
                           new JProperty("exception",   e.Message),
                           new JProperty("stackTrace",  e.StackTrace)
                       );
            }

        }

        #endregion


        #region IEnumerable<ChargingConnector> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _ChargingConnectors.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<IChargingConnector> GetEnumerator()
            => _ChargingConnectors.GetEnumerator();

        #endregion


        #region Operator overloading

        #region Operator == (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (VirtualEVSE? VirtualEVSE1,
                                           VirtualEVSE? VirtualEVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(VirtualEVSE1, VirtualEVSE2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualEVSE1 is null) || (VirtualEVSE2 is null))
                return false;

            return VirtualEVSE1.Equals(VirtualEVSE2);

        }

        #endregion

        #region Operator != (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (VirtualEVSE? VirtualEVSE1,
                                           VirtualEVSE? VirtualEVSE2)

            => !(VirtualEVSE1 == VirtualEVSE2);

        #endregion

        #region Operator <  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (VirtualEVSE? VirtualEVSE1,
                                          VirtualEVSE? VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) < 0;

        }

        #endregion

        #region Operator <= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (VirtualEVSE? VirtualEVSE1,
                                           VirtualEVSE? VirtualEVSE2)

            => !(VirtualEVSE1 > VirtualEVSE2);

        #endregion

        #region Operator >  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (VirtualEVSE? VirtualEVSE1,
                                          VirtualEVSE? VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) > 0;

        }

        #endregion

        #region Operator >= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (VirtualEVSE? VirtualEVSE1,
                                           VirtualEVSE? VirtualEVSE2)

            => !(VirtualEVSE1 < VirtualEVSE2);

        #endregion

        #endregion

        #region IComparable<VirtualEVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two virtual EVSEs.
        /// </summary>
        /// <param name="Object">An EVSE to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is VirtualEVSE virtualEVSE
                   ? CompareTo(virtualEVSE)
                   : throw new ArgumentException("The given object is not a virtual EVSE!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(VirtualEVSE)

        /// <summary>
        /// Compares two virtual EVSEs.
        /// </summary>
        /// <param name="VirtualEVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(VirtualEVSE? VirtualEVSE)
        {

            if (VirtualEVSE is null)
                throw new ArgumentNullException(nameof(VirtualEVSE),  "The given virtual EVSE must not be null!");

            return Id.CompareTo(VirtualEVSE.Id);

        }

        /// <summary>
        /// Compares two virtual EVSEs.
        /// </summary>
        /// <param name="VirtualEVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(IEVSE? VirtualEVSE)
        {

            if (VirtualEVSE is null)
                throw new ArgumentNullException(nameof(VirtualEVSE), "The given virtual EVSE must not be null!");

            return Id.CompareTo(VirtualEVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualEVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two virtual EVSEs for equality.
        /// </summary>
        /// <param name="Object">A virtual EVSE to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSE evse &&
                   Equals(evse);

        #endregion

        #region Equals(VirtualEVSE)

        /// <summary>
        /// Compares two virtual EVSEs for equality.
        /// </summary>
        /// <param name="VirtualEVSE">A virtual EVSE to compare with.</param>
        public Boolean Equals(VirtualEVSE? VirtualEVSE)
        {

            if (VirtualEVSE is null)
                return false;

            return Id.Equals(VirtualEVSE.Id);

        }

        /// <summary>
        /// Compares two (virtual) EVSEs for equality.
        /// </summary>
        /// <param name="IEVSE">An EVSE to compare with.</param>
        public Boolean Equals(IEVSE? IEVSE)

            => IEVSE is not null &&
                   Id.Equals(IEVSE.Id);

        #endregion

        #endregion

        #region GetHashCode()

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

        #endregion

    }

}
