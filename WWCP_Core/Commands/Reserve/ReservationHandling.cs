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

using System;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Defines if a reservation can be used for consecutive charging sessions.
    /// </summary>
    public class ReservationHandling
    {

        #region Properties

        #region IsKeepAlive

        /// <summary>
        /// The reservation should not end after the remote stop operation.
        /// </summary>
        public Boolean IsKeepAlive
        {
            get
            {
                return _KeepAliveTime.HasValue;
            }
        }

        #endregion

        #region KeepAliveTime

        private readonly TimeSpan? _KeepAliveTime;

        /// <summary>
        /// The time span in which the reservation can be used
        /// for additional charging sessions.
        /// </summary>
        public TimeSpan? KeepAliveTime
        {
            get
            {
                return _KeepAliveTime;
            }
        }

        #endregion

        #region EndTime

        private readonly DateTime _EndTime;

        /// <summary>
        /// The timestamp when the reservation will expire.
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return _EndTime;
            }
        }

        #endregion

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
                this._KeepAliveTime  = KeepAliveTime;
                this._EndTime        = DateTime.Now + KeepAliveTime;
            }

            else
            {
                this._KeepAliveTime  = null;
                this._EndTime        = DateTime.Now;
            }

        }

        #endregion


        #region (static) Close

        /// <summary>
        /// The reservation will end with the remote stop operation.
        /// </summary>
        public static ReservationHandling Close
        {
            get
            {
                return new ReservationHandling(TimeSpan.Zero);
            }
        }

        #endregion

        #region (static) KeepAlive(KeepAliveTime)

        /// <summary>
        /// The reservation can be used for additional charging sessions
        /// within the given time span.
        /// </summary>
        /// <param name="KeepAliveTime">The time span in which the reservation can be used for additional charging sessions.</param>
        /// <returns></returns>
        public static ReservationHandling KeepAlive(TimeSpan KeepAliveTime)
        {
            return new ReservationHandling(KeepAliveTime);
        }

        #endregion

    }

}
