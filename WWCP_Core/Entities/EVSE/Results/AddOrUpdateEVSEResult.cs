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
    /// The result of an add or update EVSE request.
    /// </summary>
    public class AddOrUpdateEVSEResult : AEnitityResult<IEVSE, EVSE_Id>
    {

        #region Properties

        public IEVSE?             EVSE
            => Object;

        public IChargingStation?  ChargingStation    { get; internal set; }

        public AddedOrUpdated?    AddedOrUpdated     { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateEVSEResult(IEVSE                  EVSE,
                                     CommandResult    Result,
                                     EventTracking_Id?      EventTrackingId   = null,
                                     IId?                   SenderId          = null,
                                     Object?                Sender            = null,
                                     IChargingStation?      ChargingStation   = null,
                                     AddedOrUpdated?        AddedOrUpdated    = null,
                                     I18NString?            Description       = null,
                                     IEnumerable<Warning>?  Warnings          = null,
                                     TimeSpan?              Runtime           = null)

            : base(EVSE,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStation  = ChargingStation;
            this.AddedOrUpdated   = AddedOrUpdated;

        }

        #endregion


        #region (static) AdminDown    (EVSE, ...)

        public static AddOrUpdateEVSEResult

            AdminDown(IEVSE                  EVSE,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   SenderId          = null,
                      Object?                Sender            = null,
                      IChargingStation?      ChargingStation   = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (EVSE, ...)

        public static AddOrUpdateEVSEResult

            NoOperation(IEVSE                  EVSE,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IChargingStation?      ChargingStation   = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (EVSE, ...)

        public static AddOrUpdateEVSEResult

            Enqueued(IEVSE                  EVSE,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   SenderId          = null,
                     Object?                Sender            = null,
                     IChargingStation?      ChargingStation   = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added        (EVSE, ...)

        public static AddOrUpdateEVSEResult

            Added(IEVSE                  EVSE,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IChargingStation?      ChargingStation   = null,
                  I18NString?            Description       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated      (EVSE, ...)

        public static AddOrUpdateEVSEResult

            Updated(IEVSE                  EVSE,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IChargingStation?      ChargingStation   = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(EVSE, Description, ...)

        public static AddOrUpdateEVSEResult

            ArgumentError(IEVSE                  EVSE,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IChargingStation?      ChargingStation   = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (EVSE, Description, ...)

        public static AddOrUpdateEVSEResult

            Error(IEVSE                  EVSE,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IChargingStation?      ChargingStation   = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (EVSE, Exception,   ...)

        public static AddOrUpdateEVSEResult

            Error(IEVSE                  EVSE,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IChargingStation?      ChargingStation   = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout      (EVSE, Timeout,     ...)

        public static AddOrUpdateEVSEResult

            Timeout(IEVSE                  EVSE,
                    TimeSpan               Timeout,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IChargingStation?      ChargingStation   = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.Timeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (EVSE, Timeout,     ...)

        public static AddOrUpdateEVSEResult

            LockTimeout(IEVSE                  EVSE,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IChargingStation?      ChargingStation   = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EVSE,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStation,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
