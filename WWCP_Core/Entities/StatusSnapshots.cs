/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    // The (admin) status values live in WWCP_POI2, which knows no entities;
    // their snapshots of WWCP Core entities live here, one class per status type.

    /// <summary>
    /// The snapshot of a ChargingPoolAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingPoolAdminStatusUpdateSnapshots
    {

        extension(ChargingPoolAdminStatusUpdate)
        {

            #region (static) Snapshot(ChargingPool, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current charging pool admin status.
            /// </summary>
            /// <param name="ChargingPool">A charging pool.</param>
            /// <param name="DataSource">An optional data source or context for the charging pool admin status update.</param>
            public static ChargingPoolAdminStatusUpdate Snapshot(IChargingPool  ChargingPool,
                                                                 Context?       DataSource   = null)

                => new (ChargingPool.Id,
                        ChargingPool.AdminStatus,
                        ChargingPool.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingPoolStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingPoolStatusSnapshots
    {

        extension(ChargingPoolStatus)
        {

            #region (static) Snapshot(ChargingPool)

            /// <summary>
            /// Take a snapshot of the current charging pool status.
            /// </summary>
            /// <param name="ChargingPool">A charging pool.</param>
            public static ChargingPoolStatus Snapshot(ChargingPool ChargingPool)

                => new (ChargingPool.Id,
                        ChargingPool.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingPoolStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingPoolStatusUpdateSnapshots
    {

        extension(ChargingPoolStatusUpdate)
        {

            #region (static) Snapshot(ChargingPool, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current charging pool status.
            /// </summary>
            /// <param name="ChargingPool">A charging pool.</param>
            /// <param name="DataSource">An optional data source or context for the EVSE admin status update.</param>
            public static ChargingPoolStatusUpdate Snapshot(IChargingPool  ChargingPool,
                                                            Context?       DataSource   = null)

                => new (ChargingPool.Id,
                        ChargingPool.Status,
                        ChargingPool.StatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingStationAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingStationAdminStatusUpdateSnapshots
    {

        extension(ChargingStationAdminStatusUpdate)
        {

            #region (static) Snapshot(ChargingStation, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current charging station admin status.
            /// </summary>
            /// <param name="ChargingStation">A charging station.</param>
            /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
            public static ChargingStationAdminStatusUpdate Snapshot(IChargingStation  ChargingStation,
                                                                    Context?          DataSource   = null)

                => new (ChargingStation.Id,
                        ChargingStation.AdminStatus,
                        ChargingStation.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingStationOperatorAdminStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingStationOperatorAdminStatusSnapshots
    {

        extension(ChargingStationOperatorAdminStatus)
        {

            #region (static) Snapshot(ChargingStationOperator)

            /// <summary>
            /// Take a snapshot of the current charging station operator admin status.
            /// </summary>
            /// <param name="ChargingStationOperator">A charging station operator.</param>
            public static ChargingStationOperatorAdminStatus Snapshot(ChargingStationOperator ChargingStationOperator)

                => new (ChargingStationOperator.Id,
                        ChargingStationOperator.AdminStatus);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingStationOperatorAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingStationOperatorAdminStatusUpdateSnapshots
    {

        extension(ChargingStationOperatorAdminStatusUpdate)
        {

            #region (static) Snapshot(ChargingStationOperator, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current charging station operator admin status.
            /// </summary>
            /// <param name="ChargingStationOperator">A charging station operator.</param>
            /// <param name="DataSource">An optional data source or context for the charging station operator admin status update.</param>
            public static ChargingStationOperatorAdminStatusUpdate Snapshot(IChargingStationOperator  ChargingStationOperator,
                                                                            String?                   DataSource   = null)

                => new (ChargingStationOperator.Id,
                        ChargingStationOperator.AdminStatus,
                        ChargingStationOperator.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingStationStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingStationStatusSnapshots
    {

        extension(ChargingStationStatus)
        {

            #region (static) Snapshot(ChargingStation)

            /// <summary>
            /// Take a snapshot of the current charging station status.
            /// </summary>
            /// <param name="ChargingStation">A charging station.</param>
            public static ChargingStationStatus Snapshot(ChargingStation ChargingStation)

                => new (ChargingStation.Id,
                        ChargingStation.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a ChargingStationStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class ChargingStationStatusUpdateSnapshots
    {

        extension(ChargingStationStatusUpdate)
        {

            #region (static) Snapshot(ChargingStation, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current charging station status.
            /// </summary>
            /// <param name="ChargingStation">A charging station.</param>
            /// <param name="DataSource">An optional data source or context for the charging station status update.</param>
            public static ChargingStationStatusUpdate Snapshot(IChargingStation  ChargingStation,
                                                               Context?          DataSource   = null)

                => new (ChargingStation.Id,
                        ChargingStation.Status,
                        ChargingStation.StatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EMobilityProviderAdminStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class EMobilityProviderAdminStatusSnapshots
    {

        extension(EMobilityProviderAdminStatus)
        {

            #region (static) Snapshot(EMobilityProvider)

            /// <summary>
            /// Take a snapshot of the current e-mobility provider admin status.
            /// </summary>
            /// <param name="EMobilityProvider">A e-mobility provider.</param>
            public static EMobilityProviderAdminStatus Snapshot(EMobilityProvider EMobilityProvider)

                => new (EMobilityProvider.Id,
                        EMobilityProvider.AdminStatus.Value,
                        EMobilityProvider.AdminStatus.Timestamp);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EMobilityProviderAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EMobilityProviderAdminStatusUpdateSnapshots
    {

        extension(EMobilityProviderAdminStatusUpdate)
        {

            #region (static) Snapshot(EMobilityProvider)

            /// <summary>
            /// Take a snapshot of the current e-mobility provider admin status.
            /// </summary>
            /// <param name="EMobilityProvider">A e-mobility provider.</param>
            public static EMobilityProviderAdminStatusUpdate Snapshot(IEMobilityProvider EMobilityProvider)

                => new (EMobilityProvider.Id,
                        EMobilityProvider.AdminStatus,
                        EMobilityProvider.AdminStatusSchedule().Skip(1).FirstOrDefault());

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EMobilityProviderStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class EMobilityProviderStatusSnapshots
    {

        extension(EMobilityProviderStatus)
        {

            #region (static) Snapshot(EMobilityProvider)

            /// <summary>
            /// Take a snapshot of the current e-mobility provider status.
            /// </summary>
            /// <param name="EMobilityProvider">A e-mobility provider.</param>
            public static EMobilityProviderStatus Snapshot(EMobilityProvider EMobilityProvider)

                => new (EMobilityProvider.Id,
                        EMobilityProvider.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EMobilityProviderStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EMobilityProviderStatusUpdateSnapshots
    {

        extension(EMobilityProviderStatusUpdate)
        {

            #region (static) Snapshot(EMobilityProvider)

            /// <summary>
            /// Take a snapshot of the current e-mobility provider status.
            /// </summary>
            /// <param name="EMobilityProvider">A e-mobility provider.</param>
            public static EMobilityProviderStatusUpdate Snapshot(IEMobilityProvider EMobilityProvider)

                => new (EMobilityProvider.Id,
                        EMobilityProvider.Status,
                        EMobilityProvider.StatusSchedule().Skip(1).FirstOrDefault());

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EVSEAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EVSEAdminStatusUpdateSnapshots
    {

        extension(EVSEAdminStatusUpdate)
        {

            #region (static) Snapshot(EVSE, Context = null)

            /// <summary>
            /// Take a snapshot of the current EVSE admin status.
            /// </summary>
            /// <param name="EVSE">An EVSE.</param>
            /// <param name="Context">An optional data source or context for the EVSE admin status update.</param>
            public static EVSEAdminStatusUpdate Snapshot(IEVSE     EVSE,
                                                         Context?  Context   = null)

                => new (EVSE.Id,
                        EVSE.AdminStatus,
                        EVSE.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        Context);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EVSEStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class EVSEStatusSnapshots
    {

        extension(EVSEStatus)
        {

            #region (static) Snapshot(EVSE)

            /// <summary>
            /// Take a snapshot of the current EVSE status.
            /// </summary>
            /// <param name="EVSE">An EVSE.</param>
            public static EVSEStatus Snapshot(EVSE EVSE)

                => new (EVSE.Id,
                        EVSE.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EVSEStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EVSEStatusUpdateSnapshots
    {

        extension(EVSEStatusUpdate)
        {

            #region (static) Snapshot(EVSE, Context = null)

            /// <summary>
            /// Take a snapshot of the current EVSE status.
            /// </summary>
            /// <param name="EVSE">An EVSE.</param>
            /// <param name="Context">An optional data source or context for the EVSE status update.</param>
            public static EVSEStatusUpdate Snapshot(IEVSE     EVSE,
                                                    Context?  Context   = null)

                => new (EVSE.Id,
                        EVSE.Status,
                        EVSE.StatusSchedule().Skip(1).FirstOrDefault(),
                        Context);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EnergyMeterAdminStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class EnergyMeterAdminStatusSnapshots
    {

        extension(EnergyMeterAdminStatus)
        {

            #region (static) Snapshot(EnergyMeter)

            /// <summary>
            /// Take a snapshot of the current energy meter admin status.
            /// </summary>
            /// <param name="EnergyMeter">A energy meter.</param>
            public static EnergyMeterAdminStatus Snapshot(EnergyMeter EnergyMeter)

                => new (EnergyMeter.Id,
                        EnergyMeter.AdminStatus);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EnergyMeterAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EnergyMeterAdminStatusUpdateSnapshots
    {

        extension(EnergyMeterAdminStatusUpdate)
        {

            #region (static) Snapshot(EnergyMeter, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current energy meter admin status.
            /// </summary>
            /// <param name="EnergyMeter">A energy meter.</param>
            /// <param name="DataSource">An optional data source or context for the EVSE status update.</param>
            public static EnergyMeterAdminStatusUpdate Snapshot(IEnergyMeter  EnergyMeter,
                                                                Context?      DataSource   = null)

                => new (EnergyMeter.Id,
                        EnergyMeter.AdminStatus,
                        EnergyMeter.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EnergyMeterStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class EnergyMeterStatusSnapshots
    {

        extension(EnergyMeterStatus)
        {

            #region (static) Snapshot(EnergyMeter)

            /// <summary>
            /// Take a snapshot of the current energy meter status.
            /// </summary>
            /// <param name="EnergyMeter">A energy meter.</param>
            public static EnergyMeterStatus Snapshot(EnergyMeter EnergyMeter)

                => new (EnergyMeter.Id,
                        EnergyMeter.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a EnergyMeterStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class EnergyMeterStatusUpdateSnapshots
    {

        extension(EnergyMeterStatusUpdate)
        {

            #region (static) Snapshot(EnergyMeter, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current energy meter status.
            /// </summary>
            /// <param name="EnergyMeter">An energy meter.</param>
            /// <param name="DataSource">An optional data source or context for the energy meter status update.</param>
            public static EnergyMeterStatusUpdate Snapshot(IEnergyMeter  EnergyMeter,
                                                               Context?          DataSource   = null)

                => new (EnergyMeter.Id,
                        EnergyMeter.Status,
                        EnergyMeter.StatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a GridOperatorAdminStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class GridOperatorAdminStatusSnapshots
    {

        extension(GridOperatorAdminStatus)
        {

            #region (static) Snapshot(GridOperator)

            /// <summary>
            /// Take a snapshot of the current grid operator admin status.
            /// </summary>
            /// <param name="GridOperator">A grid operator.</param>
            public static GridOperatorAdminStatus Snapshot(GridOperator GridOperator)

                => new (GridOperator.Id,
                        GridOperator.AdminStatus);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a GridOperatorAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class GridOperatorAdminStatusUpdateSnapshots
    {

        extension(GridOperatorAdminStatusUpdate)
        {

            #region (static) Snapshot(GridOperator, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current grid operator admin status.
            /// </summary>
            /// <param name="GridOperator">A grid operator.</param>
            /// <param name="DataSource">An optional data source or context for the grid operator admin status update.</param>
            public static GridOperatorAdminStatusUpdate Snapshot(IGridOperator  GridOperator,
                                                                            String?                   DataSource   = null)

                => new (GridOperator.Id,
                        GridOperator.AdminStatus,
                        GridOperator.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a GridOperatorStatus, taken from its WWCP Core entity.
    /// </summary>
    public static class GridOperatorStatusSnapshots
    {

        extension(GridOperatorStatus)
        {

            #region (static) Snapshot(GridOperator)

            /// <summary>
            /// Take a snapshot of the current grid operator status.
            /// </summary>
            /// <param name="GridOperator">A grid operator.</param>
            public static GridOperatorStatus Snapshot(GridOperator GridOperator)

                => new (GridOperator.Id,
                        GridOperator.Status);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a GridOperatorStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class GridOperatorStatusUpdateSnapshots
    {

        extension(GridOperatorStatusUpdate)
        {

            #region (static) Snapshot(GridOperator, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current grid operator status.
            /// </summary>
            /// <param name="GridOperator">A grid operator.</param>
            /// <param name="DataSource">An optional data source or context for the grid operator status update.</param>
            public static GridOperatorStatusUpdate Snapshot(IGridOperator  GridOperator,
                                                                       String?                   DataSource   = null)

                => new (GridOperator.Id,
                        GridOperator.Status,
                        GridOperator.StatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a RoamingNetworkAdminStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class RoamingNetworkAdminStatusUpdateSnapshots
    {

        extension(RoamingNetworkAdminStatusUpdate)
        {

            #region (static) Snapshot(RoamingNetwork, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current roaming network admin status.
            /// </summary>
            /// <param name="RoamingNetwork">A roaming network.</param>
            /// <param name="DataSource">An optional data source or context for the roaming network admin status update.</param>
            public static RoamingNetworkAdminStatusUpdate Snapshot(IRoamingNetwork  RoamingNetwork,
                                                                   String?          DataSource   = null)

                => new (RoamingNetwork.Id,
                        RoamingNetwork.AdminStatus,
                        RoamingNetwork.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }


    /// <summary>
    /// The snapshot of a RoamingNetworkStatusUpdate, taken from its WWCP Core entity.
    /// </summary>
    public static class RoamingNetworkStatusUpdateSnapshots
    {

        extension(RoamingNetworkStatusUpdate)
        {

            #region (static) Snapshot(RoamingNetwork, DataSource = null)

            /// <summary>
            /// Take a snapshot of the current roaming network status.
            /// </summary>
            /// <param name="RoamingNetwork">A roaming network.</param>
            /// <param name="DataSource">An optional data source or context for the roaming network status update.</param>
            public static RoamingNetworkStatusUpdate Snapshot(IRoamingNetwork  RoamingNetwork,
                                                              String?          DataSource   = null)

                => new (RoamingNetwork.Id,
                        RoamingNetwork.Status,
                        RoamingNetwork.StatusSchedule().Skip(1).FirstOrDefault(),
                        DataSource);

            #endregion

        }

    }

}
