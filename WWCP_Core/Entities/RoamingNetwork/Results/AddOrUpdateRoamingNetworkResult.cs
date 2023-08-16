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
    /// The result of an add or update roaming network request.
    /// </summary>
    public class AddOrUpdateRoamingNetworkResult : AEnitityResult<IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Properties

        public IRoamingNetwork?  RoamingNetwork
            => Object;

        public AddedOrUpdated?   AddedOrUpdated     { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateRoamingNetworkResult(IRoamingNetwork        RoamingNetwork,
                                               CommandResult    Result,
                                               EventTracking_Id?      EventTrackingId   = null,
                                               IId?                   AuthId            = null,
                                               Object?                SendPOIData       = null,
                                               AddedOrUpdated?        AddedOrUpdated    = null,
                                               I18NString?            Description       = null,
                                               IEnumerable<Warning>?  Warnings          = null,
                                               TimeSpan?              Runtime           = null)

            : base(RoamingNetwork,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.AddedOrUpdated = AddedOrUpdated;

        }

        #endregion


        #region (static) AdminDown    (RoamingNetwork, ...)

        public static AddOrUpdateRoamingNetworkResult

            AdminDown(IRoamingNetwork        RoamingNetwork,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   AuthId            = null,
                      Object?                SendPOIData       = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (RoamingNetwork, ...)

        public static AddOrUpdateRoamingNetworkResult

            NoOperation(IRoamingNetwork        RoamingNetwork,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (RoamingNetwork, ...)

        public static AddOrUpdateRoamingNetworkResult

            Enqueued(IRoamingNetwork        RoamingNetwork,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added        (RoamingNetwork, ...)

        public static AddOrUpdateRoamingNetworkResult

            Added(IRoamingNetwork        RoamingNetwork,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  I18NString?            Description       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated      (RoamingNetwork, ...)

        public static AddOrUpdateRoamingNetworkResult

            Updated(IRoamingNetwork        RoamingNetwork,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(RoamingNetwork, Description, ...)

        public static AddOrUpdateRoamingNetworkResult

            ArgumentError(IRoamingNetwork        RoamingNetwork,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (RoamingNetwork, Description, ...)

        public static AddOrUpdateRoamingNetworkResult

            Error(IRoamingNetwork        RoamingNetwork,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (RoamingNetwork, Exception,   ...)

        public static AddOrUpdateRoamingNetworkResult

            Error(IRoamingNetwork        RoamingNetwork,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (RoamingNetwork, Timeout,     ...)

        public static AddOrUpdateRoamingNetworkResult

            LockTimeout(IRoamingNetwork        RoamingNetwork,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
