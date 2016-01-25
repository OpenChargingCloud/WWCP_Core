/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System;

#endregion

namespace org.GraphDefined.WWCP
{


    /// <summary>
    /// A delegate called whenever the dynamic status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    public delegate void OnStatusChangedDelegate(DateTime Timestamp, IRemoteEVSE EVSE, Timestamped<EVSEStatusType> OldEVSEStatus, Timestamped<EVSEStatusType> NewEVSEStatus);


    /// <summary>
    /// A delegate called whenever the admin status of the EVSE changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EVSE">The EVSE.</param>
    /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
    /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
    public delegate void OnAdminStatusChangedDelegate(DateTime Timestamp, IRemoteEVSE EVSE, Timestamped<EVSEAdminStatusType> OldEVSEStatus, Timestamped<EVSEAdminStatusType> NewEVSEStatus);




    /// <summary>
    /// A delegate called whenever a charging reservation was created.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="Sender">The sender of the event.</param>
    /// <param name="Reservation">The new charging reservation.</param>
    public delegate void OnNewReservationDelegate(DateTime Timestamp, Object Sender, ChargingReservation Reservation);

    /// <summary>
    /// A delegate called whenever a charging reservation was cancelled.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this cancellation was detected.</param>
    /// <param name="Sender">The sender of the event.</param>
    /// <param name="Reservation">The cancelled charging reservation.</param>
    /// <param name="ReservationCancellation">The reason for the cancellation.</param>
    public delegate void OnReservationCancelledDelegate(DateTime Timestamp, Object Sender, ChargingReservation Reservation, ChargingReservationCancellation ReservationCancellation);

    /// <summary>
    /// A delegate called whenever a charging session was created.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="Sender">The sender of the event.</param>
    /// <param name="ChargingSession">The new charging session.</param>
    public delegate void OnNewChargingSessionDelegate(DateTime Timestamp, Object Sender, ChargingSession ChargingSession);

    /// <summary>
    /// A delegate called whenever a charge detail record was created.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="Sender">The sender of the event.</param>
    /// <param name="ChargeDetailRecord">The new charge detail record.</param>
    public delegate void OnNewChargeDetailRecordDelegate(DateTime Timestamp, Object Sender, ChargeDetailRecord ChargeDetailRecord);

}