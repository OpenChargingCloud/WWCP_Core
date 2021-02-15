/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.Virtual
{

    /// <summary>
    /// Extention methods
    /// </summary>
    public static partial class ExtentionMethods
    {

        #region CreateVirtualPool   (this ChargingStationOperator, ChargingPoolId    = null, ChargingPoolConfigurator    = null, VirtualChargingPoolConfigurator    = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging pool.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// <param name="ChargingPoolId">The charging station identification for the charging station to be created.</param>
        /// <param name="ChargingPoolConfigurator">An optional delegate to configure the new (local) charging station.</param>
        /// <param name="VirtualChargingPoolConfigurator">An optional delegate to configure the new virtual charging station.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static ChargingPool CreateVirtualPool(this ChargingStationOperator                      ChargingStationOperator,
                                                     ChargingPool_Id                                   ChargingPoolId,
                                                     I18NString                                        Description                       = null,
                                                     ChargingPoolAdminStatusTypes                      InitialAdminStatus                = ChargingPoolAdminStatusTypes.Operational,
                                                     ChargingPoolStatusTypes                           InitialStatus                     = ChargingPoolStatusTypes.Available,
                                                     String                                            EllipticCurve                     = "P-256",
                                                     ECPrivateKeyParameters                            PrivateKey                        = null,
                                                     PublicKeyCertificates                             PublicKeyCertificates             = null,
                                                     TimeSpan?                                         SelfCheckTimeSpan                 = null,
                                                     UInt16                                            MaxAdminStatusListSize            = VirtualChargingPool.DefaultMaxAdminStatusListSize,
                                                     UInt16                                            MaxStatusListSize                 = VirtualChargingPool.DefaultMaxStatusListSize,
                                                     Action<ChargingPool>                              ChargingPoolConfigurator          = null,
                                                     Action<VirtualChargingPool>                       VirtualChargingPoolConfigurator   = null,
                                                     Action<ChargingPool>                              OnSuccess                         = null,
                                                     Action<ChargingStationOperator, ChargingPool_Id>  OnError                           = null)
        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return ChargingStationOperator.CreateChargingPool(ChargingPoolId,
                                                              ChargingPoolConfigurator,
                                                              newPool => {

                                                                  var virtualstation = new VirtualChargingPool(newPool.Id,
                                                                                                               newPool.Name,
                                                                                                               ChargingStationOperator.RoamingNetwork,
                                                                                                               Description,
                                                                                                               InitialAdminStatus,
                                                                                                               InitialStatus,
                                                                                                               EllipticCurve,
                                                                                                               PrivateKey,
                                                                                                               PublicKeyCertificates,
                                                                                                               SelfCheckTimeSpan,
                                                                                                               MaxAdminStatusListSize,
                                                                                                               MaxStatusListSize);

                                                                  VirtualChargingPoolConfigurator?.Invoke(virtualstation);

                                                                  return virtualstation;

                                                              },

                                                              OnSuccess: OnSuccess,
                                                              OnError:   OnError);

        }

        #endregion

        #region CreateVirtualStation(this ChargingPool,            ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ChargingStationId">The charging station identification for the charging station to be created.</param>
        /// <param name="ChargingStationConfigurator">An optional delegate to configure the new (local) charging station.</param>
        /// <param name="VirtualChargingStationConfigurator">An optional delegate to configure the new virtual charging station.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static ChargingStation CreateVirtualStation(this ChargingPool                         ChargingPool,
                                                           ChargingStation_Id                        ChargingStationId,
                                                           I18NString                                Description                          = null,
                                                           ChargingStationAdminStatusTypes           InitialAdminStatus                   = ChargingStationAdminStatusTypes.Operational,
                                                           ChargingStationStatusTypes                InitialStatus                        = ChargingStationStatusTypes.Available,
                                                           String                                    EllipticCurve                        = "P-256",
                                                           ECPrivateKeyParameters                    PrivateKey                           = null,
                                                           PublicKeyCertificates                     PublicKeyCertificates                = null,
                                                           TimeSpan?                                 SelfCheckTimeSpan                    = null,
                                                           UInt16                                    MaxAdminStatusListSize               = VirtualChargingStation.DefaultMaxAdminStatusListSize,
                                                           UInt16                                    MaxStatusListSize                    = VirtualChargingStation.DefaultMaxStatusListSize,
                                                           Action<ChargingStation>                   ChargingStationConfigurator          = null,
                                                           Action<VirtualChargingStation>            VirtualChargingStationConfigurator   = null,
                                                           Action<ChargingStation>                   OnSuccess                            = null,
                                                           Action<ChargingPool, ChargingStation_Id>  OnError                              = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return ChargingPool.CreateChargingStation(ChargingStationId,
                                                      ChargingStationConfigurator,
                                                      newStation => {

                                                          var virtualstation = new VirtualChargingStation(newStation.Id,
                                                                                                          newStation.Name,
                                                                                                          ChargingPool.RoamingNetwork,
                                                                                                          Description,
                                                                                                          InitialAdminStatus,
                                                                                                          InitialStatus,
                                                                                                          EllipticCurve,
                                                                                                          PrivateKey,
                                                                                                          PublicKeyCertificates,
                                                                                                          SelfCheckTimeSpan,
                                                                                                          MaxAdminStatusListSize,
                                                                                                          MaxStatusListSize);

                                                          VirtualChargingStationConfigurator?.Invoke(virtualstation);

                                                          return virtualstation;

                                                      },

                                                      OnSuccess: OnSuccess,
                                                      OnError:   OnError);

        }

        #endregion

        #region CreateVirtualEVSE   (this ChargingStation,         EVSEId            = null, EVSEConfigurator            = null, VirtualEVSEConfigurator            = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="EVSEId">The EVSE identification for the EVSE to be created.</param>
        /// <param name="EVSEConfigurator">An optional delegate to configure the new (local) EVSE.</param>
        /// <param name="VirtualEVSEConfigurator">An optional delegate to configure the new EVSE.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static EVSE CreateVirtualEVSE(this ChargingStation              ChargingStation,
                                             EVSE_Id                           EVSEId,
                                             I18NString                        Description               = null,
                                             EVSEAdminStatusTypes              InitialAdminStatus        = EVSEAdminStatusTypes.Operational,
                                             EVSEStatusTypes                   InitialStatus             = EVSEStatusTypes.Available,
                                             EnergyMeter_Id?                   EnergyMeterId             = null,
                                             String                            EllipticCurve             = "P-256",
                                             ECPrivateKeyParameters            PrivateKey                = null,
                                             PublicKeyCertificates             PublicKeyCertificates     = null,
                                             TimeSpan?                         SelfCheckTimeSpan         = null,
                                             UInt16                            MaxAdminStatusListSize    = VirtualEVSE.DefaultMaxAdminStatusListSize,
                                             UInt16                            MaxStatusListSize         = VirtualEVSE.DefaultMaxStatusListSize,
                                             Action<EVSE>                      EVSEConfigurator          = null,
                                             Action<VirtualEVSE>               VirtualEVSEConfigurator   = null,
                                             Action<EVSE>                      OnSuccess                 = null,
                                             Action<ChargingStation, EVSE_Id>  OnError                   = null)
        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return ChargingStation.CreateEVSE(EVSEId,
                                              EVSEConfigurator,
                                              newEVSE => {

                                                  var virtualevse = new VirtualEVSE(newEVSE.Id,
                                                                                    null,
                                                                                    ChargingStation.RoamingNetwork,
                                                                                    Description,
                                                                                    InitialAdminStatus,
                                                                                    InitialStatus,
                                                                                    EnergyMeterId,
                                                                                    EllipticCurve,
                                                                                    PrivateKey,
                                                                                    PublicKeyCertificates,
                                                                                    SelfCheckTimeSpan,
                                                                                    MaxAdminStatusListSize,
                                                                                    MaxStatusListSize);

                                                  VirtualEVSEConfigurator?.Invoke(virtualevse);

                                                  return virtualevse;

                                              },

                                              OnSuccess: OnSuccess,
                                              OnError:   OnError);

        }

        #endregion

    }

}
