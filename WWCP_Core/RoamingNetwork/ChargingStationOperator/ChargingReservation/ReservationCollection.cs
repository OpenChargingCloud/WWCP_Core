/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    public class ChargingReservationCollection : IEnumerable<ChargingReservation>
    {

        #region Data

        private readonly List<ChargingReservation> _Reservations;

        #endregion

        #region Properties

        public ChargingReservation_Id  Id    { get; }

        #endregion

        #region Constructor(s)

        public ChargingReservationCollection(ChargingReservation_Id Id)
        {
            this.Id = Id;
            this._Reservations  = new List<ChargingReservation>();
        }

        public ChargingReservationCollection(ChargingReservation Reservation)
            : this(Reservation.Id)
        {
            Add(Reservation);
        }

        public ChargingReservationCollection(ChargingReservation_Id  Id,
                                             ChargingReservation     Reservation)
            : this(Id)
        {
            Add(Reservation);
        }

        public ChargingReservationCollection(ChargingReservation_Id            Id,
                                             IEnumerable<ChargingReservation>  Reservations)
            : this(Id)
        {
            Add(Reservations);
        }

        #endregion


        public ChargingReservationCollection Add(ChargingReservation Reservation)
        {

            if (Reservation == null)
                throw new ArgumentNullException(nameof(Reservation), "The given charging reservation must not be null!");

            if (Reservation.Id != Id)
                throw new ArgumentException("The given charging reservation identification '" + Reservation.Id + "' does not match!", nameof(Reservation));

            lock (_Reservations)
            {
                _Reservations.Add(Reservation);
            }

            return this;

        }

        public ChargingReservationCollection Add(IEnumerable<ChargingReservation> Reservations)
        {

            _Reservations.AddRange(Reservations);

            return this;

        }

        public ChargingReservationCollection UpdateLast(ChargingReservation Reservation)
        {

            _Reservations.RemoveAt(_Reservations.Count - 1);
            _Reservations.Add(Reservation);

            return this;

        }

        public IEnumerator<ChargingReservation> GetEnumerator()
            => _Reservations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _Reservations.GetEnumerator();

    }

}
