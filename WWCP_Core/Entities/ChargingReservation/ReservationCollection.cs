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

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class ChargingReservationCollection : IId<ChargingReservation_Id>,
                                                 IHasId<ChargingReservation_Id>,
                                                 IEnumerable<ChargingReservation>
    {

        #region Data

        private readonly List<ChargingReservation> chargingReservations;

        #endregion

        #region Properties

        public ChargingReservation_Id       Id    { get; }

        public EVSE_Id?                     EVSEId
            => chargingReservations.LastOrDefault()?.EVSEId;

        public ChargingStation_Id?          ChargingStationId
            => chargingReservations.LastOrDefault()?.ChargingStationId;

        public ChargingPool_Id?             ChargingPoolId
            => chargingReservations.LastOrDefault()?.ChargingPoolId;

        public ChargingStationOperator_Id?  ChargingStationOperatorId
            => chargingReservations.LastOrDefault()?.ChargingStationOperatorId;

        public CSORoamingProvider_Id?       EMPRoamingProviderId
            => chargingReservations.LastOrDefault()?.EMPRoamingProviderId;

        public EMPRoamingProvider_Id?       CSORoamingProviderId
            => chargingReservations.LastOrDefault()?.CSORoamingProviderId;

        public ulong Length
            => throw new NotImplementedException();

        public bool IsNullOrEmpty
            => throw new NotImplementedException();

        #endregion

        #region Constructor(s)

        public ChargingReservationCollection(ChargingReservation_Id Id)
        {
            this.Id = Id;
            this.chargingReservations  = new List<ChargingReservation>();
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

            if (Reservation is null)
                throw new ArgumentNullException(nameof(Reservation), "The given charging reservation must not be null!");

            if (Reservation.Id != Id)
                throw new ArgumentException("The given charging reservation identification '" + Reservation.Id + "' does not match!", nameof(Reservation));

            lock (chargingReservations)
            {
                chargingReservations.Add(Reservation);
            }

            return this;

        }

        public ChargingReservationCollection Add(IEnumerable<ChargingReservation> Reservations)
        {

            chargingReservations.AddRange(Reservations);

            return this;

        }

        public ChargingReservationCollection UpdateLast(ChargingReservation Reservation)
        {

            chargingReservations.RemoveAt(chargingReservations.Count - 1);
            chargingReservations.Add(Reservation);

            return this;

        }

        public IEnumerator<ChargingReservation> GetEnumerator()
            => chargingReservations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => chargingReservations.GetEnumerator();

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(ChargingReservation_Id other)
            => Id.CompareTo(other);

        public Boolean Equals(ChargingReservation_Id other)
            => Id.Equals(other);

    }

}
