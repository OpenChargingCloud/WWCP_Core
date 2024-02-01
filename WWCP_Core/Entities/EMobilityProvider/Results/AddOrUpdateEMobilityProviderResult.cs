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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an add or update e-mobility provider request.
    /// </summary>
    public class AddOrUpdateEMobilityProviderResult : AEnitityResult<IEMobilityProvider, EMobilityProvider_Id>
    {

        #region Properties

        public IEMobilityProvider?  EMobilityProvider
            => Entity;

        public IRoamingNetwork?     RoamingNetwork    { get; internal set; }

        public AddedOrUpdated?      AddedOrUpdated    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateEMobilityProviderResult(IEMobilityProvider     EMobilityProvider,
                                                  CommandResult          Result,
                                                  EventTracking_Id?      EventTrackingId   = null,
                                                  IId?                   SenderId          = null,
                                                  Object?                Sender            = null,
                                                  IRoamingNetwork?       RoamingNetwork    = null,
                                                  AddedOrUpdated?        AddedOrUpdated    = null,
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

            this.RoamingNetwork  = RoamingNetwork;
            this.AddedOrUpdated  = AddedOrUpdated;

        }

        #endregion


        #region (static) AdminDown    (ChargingStation, ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (ChargingStation, ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (ChargingStation, ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added        (ChargingStation, ...)

        public static AddOrUpdateEMobilityProviderResult

            Added(IEMobilityProvider     EMobilityProvider,
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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated      (ChargingStation, ...)

        public static AddOrUpdateEMobilityProviderResult

            Updated(IEMobilityProvider     EMobilityProvider,
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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(ChargingStation, Description, ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStation, Description, ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStation, Exception,   ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout      (ChargingStation, Timeout,     ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (ChargingStation, Timeout,     ...)

        public static AddOrUpdateEMobilityProviderResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
