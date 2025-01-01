/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a delete roaming network request.
    /// </summary>
    public class DeleteRoamingNetworkResult : AEnitityResult<IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Properties

        public IRoamingNetwork?  RoamingNetwork
            => Entity;

        #endregion

        #region Constructor(s)

        public DeleteRoamingNetworkResult(IRoamingNetwork        RoamingNetwork,
                                          CommandResult    Result,
                                          EventTracking_Id?      EventTrackingId   = null,
                                          IId?                   SenderId          = null,
                                          Object?                Sender            = null,
                                          I18NString?            Description       = null,
                                          IEnumerable<Warning>?  Warnings          = null,
                                          TimeSpan?              Runtime           = null)

            : base(RoamingNetwork,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        { }


        public DeleteRoamingNetworkResult(RoamingNetwork_Id      RoamingNetworkId,
                                          CommandResult    Result,
                                          EventTracking_Id?      EventTrackingId   = null,
                                          IId?                   SenderId          = null,
                                          Object?                Sender            = null,
                                          I18NString?            Description       = null,
                                          IEnumerable<Warning>?  Warnings          = null,
                                          TimeSpan?              Runtime           = null)

            : base(RoamingNetworkId,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown      (RoamingNetwork, ...)

        public static DeleteRoamingNetworkResult

            AdminDown(IRoamingNetwork        RoamingNetwork,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   SenderId          = null,
                      Object?                Sender            = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (RoamingNetwork, ...)

        public static DeleteRoamingNetworkResult

            NoOperation(IRoamingNetwork        RoamingNetwork,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (RoamingNetwork, ...)

        public static DeleteRoamingNetworkResult

            Enqueued(IRoamingNetwork        RoamingNetwork,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   SenderId          = null,
                     Object?                Sender            = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (RoamingNetwork, ...)

        public static DeleteRoamingNetworkResult

            Success(IRoamingNetwork        RoamingNetwork,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(RoamingNetwork, ...)

        public static DeleteRoamingNetworkResult

            CanNotBeRemoved(IRoamingNetwork        RoamingNetwork,
                            EventTracking_Id?      EventTrackingId   = null,
                            IId?                   SenderId          = null,
                            Object?                Sender            = null,
                            I18NString?            Description       = null,
                            IEnumerable<Warning>?  Warnings          = null,
                            TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (RoamingNetwork, Description, ...)

        public static DeleteRoamingNetworkResult

            ArgumentError(IRoamingNetwork        RoamingNetwork,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (RoamingNetwork, Description, ...)

        public static DeleteRoamingNetworkResult

            Error(IRoamingNetwork        RoamingNetwork,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (RoamingNetwork, Exception,   ...)

        public static DeleteRoamingNetworkResult

            Error(IRoamingNetwork        RoamingNetwork,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout        (RoamingNetwork, Timeout,     ...)

        public static DeleteRoamingNetworkResult

            Timeout(IRoamingNetwork        RoamingNetwork,
                    TimeSpan               Timeout,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Timeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (RoamingNetwork, Timeout,     ...)

        public static DeleteRoamingNetworkResult

            LockTimeout(IRoamingNetwork        RoamingNetwork,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
