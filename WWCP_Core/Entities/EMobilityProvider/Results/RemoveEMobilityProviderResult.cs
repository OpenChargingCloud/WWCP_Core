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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of a delete e-mobility provider request.
    /// </summary>
    public class DeleteEMobilityProviderResult : AEnitityResult<IEMobilityProvider, EMobilityProvider_Id>
    {

        #region Properties

        public IEMobilityProvider?  EMobilityProvider
            => Entity;

        public IRoamingNetwork?     RoamingNetwork    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteEMobilityProviderResult(IEMobilityProvider     EMobilityProvider,
                                             CommandResult          Result,
                                             EventTracking_Id?      EventTrackingId   = null,
                                             IId?                   SenderId          = null,
                                             Object?                Sender            = null,
                                             IRoamingNetwork?       RoamingNetwork    = null,
                                             I18NString?            Description       = null,
                                             IEnumerable<Warning>?  Warnings          = null,
                                             TimeSpan?              Runtime           = null)

            : base(EMobilityProvider,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork = RoamingNetwork;

        }


        public DeleteEMobilityProviderResult(EMobilityProvider_Id   EMobilityProviderId,
                                             CommandResult          Result,
                                             EventTracking_Id?      EventTrackingId   = null,
                                             IId?                   SenderId          = null,
                                             Object?                Sender            = null,
                                             IRoamingNetwork?       RoamingNetwork    = null,
                                             I18NString?            Description       = null,
                                             IEnumerable<Warning>?  Warnings          = null,
                                             TimeSpan?              Runtime           = null)

            : base(EMobilityProviderId,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork = RoamingNetwork;

        }

        #endregion


        #region (static) AdminDown      (ChargingStation,   ...)

        public static DeleteEMobilityProviderResult

            AdminDown(IEMobilityProvider     EMobilityProvider,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   SenderId          = null,
                      Object?                Sender            = null,
                      IRoamingNetwork?       RoamingNetwork    = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (ChargingStation,   ...)

        public static DeleteEMobilityProviderResult

            NoOperation(IEMobilityProvider     EMobilityProvider,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IRoamingNetwork?       RoamingNetwork    = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (ChargingStation,   ...)

        public static DeleteEMobilityProviderResult

            Enqueued(IEMobilityProvider     EMobilityProvider,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   SenderId          = null,
                     Object?                Sender            = null,
                     IRoamingNetwork?       RoamingNetwork    = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (ChargingStation,   ...)

        public static DeleteEMobilityProviderResult

            Success(IEMobilityProvider     EMobilityProvider,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IRoamingNetwork?       RoamingNetwork    = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(ChargingStation, ...)

        public static DeleteEMobilityProviderResult

            CanNotBeRemoved(IEMobilityProvider     EMobilityProvider,
                            EventTracking_Id?      EventTrackingId   = null,
                            IId?                   SenderId          = null,
                            Object?                Sender            = null,
                            IRoamingNetwork?       RoamingNetwork    = null,
                            I18NString?            Description       = null,
                            IEnumerable<Warning>?  Warnings          = null,
                            TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (ChargingStation,   Description, ...)

        public static DeleteEMobilityProviderResult

            ArgumentError(IEMobilityProvider     EMobilityProvider,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IRoamingNetwork?       RoamingNetwork    = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError  (ChargingStationId, Description, ...)

        public static DeleteEMobilityProviderResult

            ArgumentError(EMobilityProvider_Id   EMobilityProviderId,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IRoamingNetwork?       RoamingNetwork    = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EMobilityProviderId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStation,   Description, ...)

        public static DeleteEMobilityProviderResult

            Error(IEMobilityProvider     EMobilityProvider,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IRoamingNetwork?       RoamingNetwork    = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStation,   Exception,   ...)

        public static DeleteEMobilityProviderResult

            Error(IEMobilityProvider     EMobilityProvider,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IRoamingNetwork?       RoamingNetwork    = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout        (ChargingStation,   Timeout,     ...)

        public static DeleteEMobilityProviderResult

            Timeout(IEMobilityProvider     EMobilityProvider,
                    TimeSpan               Timeout,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IRoamingNetwork?       RoamingNetwork    = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.Timeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (ChargingStation,   Timeout,     ...)

        public static DeleteEMobilityProviderResult

            LockTimeout(IEMobilityProvider     EMobilityProvider,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IRoamingNetwork?       RoamingNetwork    = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EMobilityProvider,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
