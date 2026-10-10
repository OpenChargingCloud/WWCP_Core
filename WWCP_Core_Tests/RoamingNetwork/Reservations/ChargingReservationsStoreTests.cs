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

using NUnit.Framework;

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Reservations
{

    /// <summary>
    /// The store of a roaming network's charging reservations.
    /// </summary>
    /// <remarks>
    /// A charging pool that reserved at its remote pool updates the reservation
    /// in the roaming network's store - and does so before the roaming network
    /// has stored it, which happens only once the answer has come back up to it.
    /// So an update of an id the store does not know yet is the ordinary case,
    /// not a mistake: it used to throw a NullReferenceException, which the pool
    /// caught and answered as a failed reservation.
    ///
    /// The pool sets its id; these tests set the end time instead, because
    /// only WWCP_Core may set a reservation's pool id.
    /// </remarks>
    [TestFixture]
    public class ChargingReservationsStoreTests
    {

        #region Data

        private static readonly ChargingStationOperator_Id  operatorId  = ChargingStationOperator_Id.Parse("DE*GEF");
        private static readonly DateTimeOffset              endTime     = new (2026, 10, 4, 12, 0, 0, TimeSpan.Zero);

        #endregion

        #region (private static) NewStore()

        /// <summary>
        /// A store that writes no log file and syncs with nobody.
        /// </summary>
        private static ChargingReservationsStore NewStore()

            => new (RoamingNetwork_Id.Parse("TEST"),
                    DisableLogfiles:     true,
                    ReloadDataOnStart:   false,
                    DisableNetworkSync:  true);

        #endregion

        #region (private static) NewReservation(Suffix)

        private static ChargingReservation NewReservation(String Suffix)

            => new (ChargingReservation_Id.Parse(operatorId, Suffix),
                    Timestamp:                DateTimeOffset.UtcNow,
                    StartTime:                DateTimeOffset.UtcNow,
                    Duration:                 TimeSpan.FromMinutes(15),
                    ConsumedReservationTime:  TimeSpan.Zero,
                    ReservationLevel:         ChargingReservationLevel.EVSE);

        #endregion


        #region UpdatingAllOfAReservationNotStoredYetChangesNothing()

        /// <summary>
        /// What a charging pool does after it reserved: update every stored
        /// version of a reservation the store does not have yet.
        /// </summary>
        [Test]
        public async Task UpdatingAllOfAReservationNotStoredYetChangesNothing()
        {

            var store = NewStore();
            var id    = ChargingReservation_Id.Parse(operatorId, "unknown");

            await store.UpdateAll(id, reservation => reservation.EndTime = endTime);

            Assert.That(store.ContainsKey(id), Is.False);

        }

        #endregion

        #region UpdatingTheLatestOfAReservationNotStoredYetChangesNothing()

        [Test]
        public async Task UpdatingTheLatestOfAReservationNotStoredYetChangesNothing()
        {

            var store = NewStore();
            var id    = ChargingReservation_Id.Parse(operatorId, "unknown");

            await store.UpdateLatest(id, reservation => reservation.EndTime = endTime);

            Assert.That(store.ContainsKey(id), Is.False);

        }

        #endregion

        #region AStoredReservationIsUpdated()

        /// <summary>
        /// Updating a reservation the store has reaches it.
        /// </summary>
        [Test]
        public async Task AStoredReservationIsUpdated()
        {

            var store        = NewStore();
            var reservation  = NewReservation("known");

            await store.NewOrUpdate(reservation);
            await store.UpdateAll(reservation.Id, stored => stored.EndTime = endTime);

            Assert.That(store.Get(reservation.Id)?.Last().EndTime, Is.EqualTo(endTime));

        }

        #endregion

    }

}
