/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Cloud <https://github.com/GraphDefined/WWCP_Cloud>
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

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.WWCP.ChargingPools;

#endregion

namespace org.GraphDefined.WWCP.ChargingStations
{

    /// <summary>
    /// Extention methods
    /// </summary>
    public static partial class ExtentionMethods
    {

        #region CreateVirtualStation(this ChargingPool, ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

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
                                                           ChargingStationAdminStatusTypes           InitialAdminStatus                  = ChargingStationAdminStatusTypes.Operational,
                                                           ChargingStationStatusTypes                InitialStatus                       = ChargingStationStatusTypes.Available,
                                                           UInt16                                    MaxAdminStatusListSize              = VirtualChargingStation.DefaultMaxAdminStatusListSize,
                                                           UInt16                                    MaxStatusListSize                   = VirtualChargingStation.DefaultMaxStatusListSize,
                                                           Action<ChargingStation>                   ChargingStationConfigurator         = null,
                                                           Action<VirtualChargingStation>            VirtualChargingStationConfigurator  = null,
                                                           Action<ChargingStation>                   OnSuccess                           = null,
                                                           Action<ChargingPool, ChargingStation_Id>  OnError                             = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return ChargingPool.CreateChargingStation(ChargingStationId,
                                                      ChargingStationConfigurator,
                                                      newstation => {

                                                          var virtualstation = new VirtualChargingStation(newstation.Id,
                                                                                                          ChargingPool.RemoteChargingPool as VirtualChargingPool,
                                                                                                          null,
                                                                                                          null,
                                                                                                          InitialAdminStatus,
                                                                                                          InitialStatus,
                                                                                                          MaxAdminStatusListSize,
                                                                                                          MaxStatusListSize);

                                                          VirtualChargingStationConfigurator?.Invoke(virtualstation);

                                                          return virtualstation;

                                                      },

                                                      OnSuccess: OnSuccess,
                                                      OnError:   OnError);

        }

        #endregion

        #region CreateVirtualStation(this ChargingPool, ChargingStationId = null, ChargingStationConfigurator = null, VirtualChargingStationConfigurator = null, OnSuccess = null, OnError = null)

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
                                                           PgpSecretKeyRing                          SecretKeyRing                       = null,
                                                           PgpPublicKeyRing                          PublicKeyRing                       = null,
                                                           ChargingStationAdminStatusTypes           InitialAdminStatus                  = ChargingStationAdminStatusTypes.Operational,
                                                           ChargingStationStatusTypes                InitialStatus                       = ChargingStationStatusTypes.Available,
                                                           UInt16                                    MaxAdminStatusListSize              = VirtualChargingStation.DefaultMaxAdminStatusListSize,
                                                           UInt16                                    MaxStatusListSize                   = VirtualChargingStation.DefaultMaxStatusListSize,
                                                           Action<ChargingStation>                   ChargingStationConfigurator         = null,
                                                           Action<VirtualChargingStation>            VirtualChargingStationConfigurator  = null,
                                                           Action<ChargingStation>                   OnSuccess                           = null,
                                                           Action<ChargingPool, ChargingStation_Id>  OnError                             = null)
        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return ChargingPool.CreateChargingStation(ChargingStationId,
                                                      ChargingStationConfigurator,
                                                      newstation => {

                                                          var virtualstation = new VirtualChargingStation(newstation.Id,
                                                                                                          ChargingPool.RemoteChargingPool as VirtualChargingPool,
                                                                                                          SecretKeyRing,
                                                                                                          PublicKeyRing,
                                                                                                          InitialAdminStatus,
                                                                                                          InitialStatus,
                                                                                                          MaxAdminStatusListSize,
                                                                                                          MaxStatusListSize);

                                                          VirtualChargingStationConfigurator?.Invoke(virtualstation);

                                                          return virtualstation;

                                                      },

                                                      OnSuccess: OnSuccess,
                                                      OnError:   OnError);

        }

        #endregion


        #region CreateVirtualEVSE(this ChargingStation, EVSEId = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

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
                                             EnergyMeter_Id                    EnergyMeterId,
                                             EVSEAdminStatusTypes              InitialAdminStatus        = EVSEAdminStatusTypes.Operational,
                                             EVSEStatusTypes                   InitialStatus             = EVSEStatusTypes.Available,
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
                                              newevse => {

                                                  var virtualevse = new VirtualEVSE(newevse.Id,
                                                                                    ChargingStation.RemoteChargingStation as VirtualChargingStation,
                                                                                    EnergyMeterId,
                                                                                    null,
                                                                                    null,
                                                                                    InitialAdminStatus,
                                                                                    InitialStatus,
                                                                                    MaxAdminStatusListSize,
                                                                                    MaxStatusListSize);

                                                  VirtualEVSEConfigurator?.Invoke(virtualevse);

                                                  return virtualevse;

                                              },

                                              OnSuccess: OnSuccess,
                                              OnError:   OnError);

        }

        #endregion

        #region CreateVirtualEVSE(this ChargingStation, EVSEId = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

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
                                             EnergyMeter_Id                    EnergyMeterId,
                                             PgpSecretKeyRing                  SecretKeyRing             = null,
                                             PgpPublicKeyRing                  PublicKeyRing             = null,
                                             EVSEAdminStatusTypes              InitialAdminStatus        = EVSEAdminStatusTypes.Operational,
                                             EVSEStatusTypes                   InitialStatus             = EVSEStatusTypes.Available,
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
                                              newevse => {

                                                  var virtualevse = new VirtualEVSE(newevse.Id,
                                                                                    ChargingStation.RemoteChargingStation as VirtualChargingStation,
                                                                                    EnergyMeterId,
                                                                                    SecretKeyRing,
                                                                                    PublicKeyRing,
                                                                                    InitialAdminStatus,
                                                                                    InitialStatus,
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
