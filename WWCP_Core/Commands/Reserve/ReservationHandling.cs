/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Defines if a reservation can be used for consecutive charging sessions.
    /// </summary>
    public struct ReservationHandling
    {

        #region Properties

        /// <summary>
        /// The reservation should not end after the remote stop operation.
        /// </summary>
        public Boolean    IsKeepAlive
            => KeepAliveTime.HasValue;

        /// <summary>
        /// The time span in which the reservation can be used
        /// for additional charging sessions.
        /// </summary>
        public TimeSpan?  KeepAliveTime     { get; }

        /// <summary>
        /// The timestamp when the reservation will expire.
        /// </summary>
        public DateTime   EndTime           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new reservation handling.
        /// </summary>
        /// <param name="KeepAliveTime">The timespan in which the reservation can be used for additional charging sessions.</param>
        private ReservationHandling(TimeSpan KeepAliveTime)
        {

            if (KeepAliveTime.TotalSeconds > 0)
            {
                this.KeepAliveTime  = KeepAliveTime;
                this.EndTime        = DateTime.UtcNow + KeepAliveTime;
            }

            else
            {
                this.KeepAliveTime  = null;
                this.EndTime        = DateTime.UtcNow;
            }

        }

        #endregion


        #region (static) Close

        /// <summary>
        /// The reservation will end with the remote stop operation.
        /// </summary>
        public static ReservationHandling Close

            => new ReservationHandling(TimeSpan.Zero);

        #endregion

        #region (static) KeepAlive(KeepAliveTime)

        /// <summary>
        /// The reservation can be used for additional charging sessions
        /// within the given time span.
        /// </summary>
        /// <param name="KeepAliveTime">The time span in which the reservation can be used for additional charging sessions.</param>
        public static ReservationHandling KeepAlive(TimeSpan KeepAliveTime)

            => new ReservationHandling(KeepAliveTime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => KeepAliveTime.HasValue
                   ? KeepAliveTime.Value.TotalSeconds + " secs"
                   : "close";

        #endregion

    }

}
