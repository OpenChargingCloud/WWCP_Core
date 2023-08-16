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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an update charging pool request.
    /// </summary>
    public class UpdateChargingPoolResult : AEnitityResult<IChargingPool, ChargingPool_Id>
    {

        #region Properties

        public IChargingPool?             ChargingPool
            => Object;

        public IChargingStationOperator?  ChargingStationOperator    { get; internal set; }

        #endregion

        #region Constructor(s)

        public UpdateChargingPoolResult(IChargingPool              ChargingPool,
                                        CommandResult        Result,
                                        EventTracking_Id?          EventTrackingId           = null,
                                        IId?                       AuthId                    = null,
                                        Object?                    SendPOIData               = null,
                                        IChargingStationOperator?  ChargingStationOperator   = null,
                                        I18NString?                Description               = null,
                                        IEnumerable<Warning>?      Warnings                  = null,
                                        TimeSpan?                  Runtime                   = null)

            : base(ChargingPool,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStationOperator = ChargingStationOperator;

        }

        public UpdateChargingPoolResult(ChargingPool_Id            ChargingPoolId,
                                        CommandResult        Result,
                                        EventTracking_Id?          EventTrackingId           = null,
                                        IId?                       AuthId                    = null,
                                        Object?                    SendPOIData               = null,
                                        IChargingStationOperator?  ChargingStationOperator   = null,
                                        I18NString?                Description               = null,
                                        IEnumerable<Warning>?      Warnings                  = null,
                                        TimeSpan?                  Runtime                   = null)

            : base(ChargingPoolId,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStationOperator = ChargingStationOperator;

        }

        #endregion


        #region (static) AdminDown    (ChargingPool, ...)

        public static UpdateChargingPoolResult

            AdminDown(IChargingPool              ChargingPool,
                      EventTracking_Id?          EventTrackingId           = null,
                      IId?                       AuthId                    = null,
                      Object?                    SendPOIData               = null,
                      IChargingStationOperator?  ChargingStationOperator   = null,
                      I18NString?                Description               = null,
                      IEnumerable<Warning>?      Warnings                  = null,
                      TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (ChargingPool, ...)

        public static UpdateChargingPoolResult

            NoOperation(IChargingPool              ChargingPool,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       AuthId                    = null,
                        Object?                    SendPOIData               = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        I18NString?                Description               = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (ChargingPool, ...)

        public static UpdateChargingPoolResult

            Enqueued(IChargingPool              ChargingPool,
                     EventTracking_Id?          EventTrackingId           = null,
                     IId?                       AuthId                    = null,
                     Object?                    SendPOIData               = null,
                     IChargingStationOperator?  ChargingStationOperator   = null,
                     I18NString?                Description               = null,
                     IEnumerable<Warning>?      Warnings                  = null,
                     TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success      (ChargingPool, ...)

        public static UpdateChargingPoolResult

            Success(IChargingPool              ChargingPool,
                    EventTracking_Id?          EventTrackingId           = null,
                    IId?                       AuthId                    = null,
                    Object?                    SendPOIData               = null,
                    IChargingStationOperator?  ChargingStationOperator   = null,
                    I18NString?                Description               = null,
                    IEnumerable<Warning>?      Warnings                  = null,
                    TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(ChargingPool,   Description, ...)

        public static UpdateChargingPoolResult

            ArgumentError(IChargingPool              ChargingPool,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId           = null,
                          IId?                       AuthId                    = null,
                          Object?                    SendPOIData               = null,
                          IChargingStationOperator?  ChargingStationOperator   = null,
                          IEnumerable<Warning>?      Warnings                  = null,
                          TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError(ChargingPoolId, Description, ...)

        public static UpdateChargingPoolResult

            ArgumentError(ChargingPool_Id            ChargingPoolId,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId           = null,
                          IId?                       AuthId                    = null,
                          Object?                    SendPOIData               = null,
                          IChargingStationOperator?  ChargingStationOperator   = null,
                          IEnumerable<Warning>?      Warnings                  = null,
                          TimeSpan?                  Runtime                   = null)

                => new (ChargingPoolId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingPool,   Description, ...)

        public static UpdateChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId           = null,
                  IId?                       AuthId                    = null,
                  Object?                    SendPOIData               = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingPool,   Exception,   ...)

        public static UpdateChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       AuthId                    = null,
                  Object?                    SendPOIData               = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (Timeout, ...)

        public static UpdateChargingPoolResult

            LockTimeout(IChargingPool              ChargingPool,
                        TimeSpan                   Timeout,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       AuthId                    = null,
                        Object?                    SendPOIData               = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStationOperator,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
