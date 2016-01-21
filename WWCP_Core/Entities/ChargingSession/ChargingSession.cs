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
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging session
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingSession : AEMobilityEntity<ChargingSession_Id>,
                                   IEquatable<ChargingSession>, IComparable<ChargingSession>, IComparable
    {

        #region Data

        #endregion

        #region Properties

        #region EVSEOperator

        private readonly EVSEOperator _EVSEOperator;

        /// <summary>
        /// The Electric Vehicle Supply Equipment Operator for this charging session.
        /// </summary>
        [Optional]
        public EVSEOperator EVSEOperator
        {
            get
            {
                return _EVSEOperator;
            }
        }

        #endregion

        #region ChargingPool

        private readonly ChargingPool _ChargingPool;

        /// <summary>
        /// The charging pool for this charging session.
        /// </summary>
        [Optional]
        public ChargingPool ChargingPool
        {
            get
            {
                return _ChargingPool;
            }
        }

        #endregion

        #region ChargingStation

        private readonly ChargingStation _ChargingStation;

        /// <summary>
        /// The charging station for this charging session.
        /// </summary>
        [Optional]
        public ChargingStation ChargingStation
        {
            get
            {
                return _ChargingStation;
            }
        }

        #endregion

        #region ChargingStationId

        private readonly ChargingStation_Id _ChargingStationId;

        /// <summary>
        /// The charging station identification for this charging session.
        /// </summary>
        [Optional]
        public ChargingStation_Id ChargingStationId
        {
            get
            {
                return _ChargingStationId;
            }
        }

        #endregion

        #region EVSE

        private readonly EVSE _EVSE;

        /// <summary>
        /// The Electric Vehicle Supply Equipments (EVSE) for this charging session.
        /// </summary>
        [Optional]
        public EVSE EVSE
        {
            get
            {
                return _EVSE;
            }
        }

        #endregion

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        /// <summary>
        /// The Electric Vehicle Supply Equipment identification for this charging session.
        /// </summary>
        [Optional]
        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion


        #region ChargingReservation

        private readonly ChargingReservation _ChargingReservation;

        /// <summary>
        /// An optional charging reservation for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation ChargingReservation
        {
            get
            {
                return _ChargingReservation;
            }
        }

        #endregion

        #region ChargingReservationId

        private readonly ChargingReservation_Id _ChargingReservationId;

        /// <summary>
        /// An optional charging reservation identification for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation_Id ChargingReservationId
        {
            get
            {
                return _ChargingReservationId;
            }
        }

        #endregion

        #region EVSP

        private readonly EVSP _EVSP;

        /// <summary>
        /// The e-Mobility service provider for this charging session.
        /// </summary>
        [Optional]
        public EVSP EVSP
        {
            get
            {
                return _EVSP;
            }
        }

        #endregion

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        /// <summary>
        /// The e-Mobility service provider identification for this charging session.
        /// </summary>
        [Optional]
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion


        #region ChargingProductId

        private readonly ChargingProduct_Id _ChargingProductId;

        /// <summary>
        /// The charging product selected for this charging session.
        /// </summary>
        [Optional]
        public ChargingProduct_Id ChargingProductId
        {
            get
            {
                return _ChargingProductId;
            }
        }

        #endregion


        #region ReservationTime

        private StartEndDateTime? _ReservationTime;

        /// <summary>
        /// Optional timestamps when the reservation started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime? ReservationTime
        {
            get
            {
                return _ReservationTime;
            }
        }

        #endregion

        #region ParkingTime

        private StartEndDateTime? _ParkingTime;

        /// <summary>
        /// Optional timestamps when the parking started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime? ParkingTime
        {
            get
            {
                return _ParkingTime;
            }
        }

        #endregion

        #region SessionTime

        private StartEndDateTime? _SessionTime;

        /// <summary>
        /// Optional timestamps when the charging session started and ended.
        /// </summary>
        [Mandatory]
        public StartEndDateTime? SessionTime
        {
            get
            {
                return _SessionTime;
            }
        }

        #endregion

        #region ChargingTime

        private StartEndDateTime? _ChargingTime;

        /// <summary>
        /// Optional timestamps when the charging started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime? ChargingTime
        {
            get
            {
                return _ChargingTime;
            }
        }

        #endregion


        #region EnergyMeterId

        private readonly EnergyMeter_Id _EnergyMeterId;

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        public EnergyMeter_Id EnergyMeterId
        {
            get
            {
                return _EnergyMeterId;
            }
        }

        #endregion

        #region EnergyMeterValues

        private readonly List<Timestamped<Double>> _EnergyMeterValues;

        /// <summary>
        /// An optional enumeration of intermediate energy meter values.
        /// This values indicate the consumed energy between the current
        /// and the last timestamp in watt-hours [Wh].
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<Double>> EnergyMeterValues
        {
            get
            {
                return _EnergyMeterValues;
            }
        }

        #endregion

        #region ConsumedEnergy

        /// <summary>
        /// The current amount of energy consumed while charging in [kWh].
        /// </summary>
        [Mandatory]
        public Double ConsumedEnergy
        {
            get
            {

                return _EnergyMeterValues.
                           Select(metervalue => metervalue.Value).
                           Sum() / 1000;

            }
        }

        #endregion


        private readonly IAuthServices _AuthService;

        public IAuthServices AuthService
        {
            get
            {
                return _AuthService;
            }
        }


        private readonly IOperatorRoamingService _OperatorRoamingService;

        public IOperatorRoamingService OperatorRoamingService
        {
            get
            {
                return _OperatorRoamingService;
            }
        }


        private Boolean _RemoveMe;

        public Boolean RemoveMe
        {

            get
            {
                return _RemoveMe;
            }

            set
            {
                _RemoveMe = value;
            }

        }

        #endregion

        #region Events

        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        /// <param name="Timestamp">The current timestamp.</param>
        /// <param name="ChargingSession">The unique charging session identification.</param>
        /// <param name="EnergyMeterValue">A timestamped energy meter value.</param>
        public delegate void OnNewEnergyMeterValueDelegate(DateTime Timestamp, ChargingSession ChargingSession, Timestamped<Double> EnergyMeterValue);

        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        public event OnNewEnergyMeterValueDelegate OnNewEnergyMeterValue;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="EVSE">The Electric Vehicle Supply Equipment (EVSE) for this charging session.</param>
        /// <param name="ChargingStation">The charging station for this charging session.</param>
        /// <param name="ChargingProductId">An optional charging product selected for this charging session.</param>
        /// <param name="ChargingReservation">An optional charging reservation for this charging session.</param>
        /// 
        /// <param name="ReservationStartTime">Optional timestamp when the reservation started.</param>
        /// <param name="ParkingStartTime">Optional timestamp when the parking started.</param>
        /// <param name="SessionStartTime">Optional timestamp when the charging session started.</param>
        /// <param name="ChargingStartTime">Optional timestamp when the charging started.</param>
        /// 
        /// <param name="EnergyMeterId">An optional unique identification of the energy meter.</param>
        /// <param name="EnergyMeterValueStart">The optional start value of the energy meter used for charging.</param>
        /// 
        /// <param name="AuthService">The authentication service of this charging session.</param>
        /// <param name="OperatorRoamingService">The roaming provider of this charging session.</param>
        internal ChargingSession(ChargingSession_Id       Id,
                                 EVSEOperator             EVSEOperator            = null,
                                 ChargingPool             ChargingPool            = null,
                                 ChargingStation          ChargingStation         = null,
                                 ChargingStation_Id       ChargingStationId       = null,
                                 EVSE                     EVSE                    = null,
                                 EVSE_Id                  EVSEId                  = null,

                                 ChargingReservation      Reservation             = null,
                                 ChargingReservation_Id   ReservationId           = null,
                                 EVSP                     Provider                = null,
                                 EVSP_Id                  ProviderId              = null,

                                 ChargingProduct_Id       ChargingProductId       = null,

                                 DateTime?                ReservationStartTime    = null,
                                 DateTime?                ParkingStartTime        = null,
                                 DateTime?                SessionStartTime        = null,
                                 DateTime?                ChargingStartTime       = null,

                                 EnergyMeter_Id           EnergyMeterId           = null,
                                 Timestamped<Double>?     EnergyMeterValueStart   = null,

                                 IAuthServices            AuthService             = null,
                                 IOperatorRoamingService  OperatorRoamingService  = null)

            : base(Id)

        {

            #region Initial checks

            if (Id   == null)
                throw new ArgumentNullException("Id",    "The given charging session identification must not be null!");

      //      if (EVSE == null)
      //          throw new ArgumentNullException("EVSE",  "The given EVSE must not be null!");

            #endregion

            this._EVSEOperator            = EVSEOperator;
            this._ChargingPool            = ChargingPool;
            this._ChargingStation         = ChargingStation;
            this._ChargingStationId       = ChargingStationId;
            this._EVSE                    = EVSE;
            this._EVSEId                  = EVSEId;

            this._ChargingReservation     = Reservation;
            this._ChargingReservationId   = ReservationId;
            this._EVSP                    = Provider;
            this._ProviderId                  = ProviderId;

            this._ChargingProductId       = ChargingProductId;

            this._ReservationTime         = ReservationStartTime. HasValue ? new StartEndDateTime(ReservationStartTime.Value)                : new StartEndDateTime?();
            this._ParkingTime             = ParkingStartTime.     HasValue ? new StartEndDateTime(ParkingStartTime.    Value)                : new StartEndDateTime?();
            this._SessionTime             = SessionStartTime.     HasValue ? new StartEndDateTime(SessionStartTime.    Value)                : new StartEndDateTime?();
            this._ChargingTime            = ChargingStartTime.    HasValue ? new StartEndDateTime(ChargingStartTime.   Value)                : new StartEndDateTime?();

            this._EnergyMeterId           = EnergyMeterId;
            this._EnergyMeterValues       = EnergyMeterValueStart.HasValue ? new List<Timestamped<Double>>() { EnergyMeterValueStart.Value } : new List<Timestamped<Double>>();

            this._AuthService             = AuthService;
            this._OperatorRoamingService  = OperatorRoamingService;

        }

        #endregion


        #region IComparable<ChargingSession> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging session.
            var ChargingSession = Object as ChargingSession;
            if ((Object) ChargingSession == null)
                throw new ArgumentException("The given object is not a charging session!");

            return CompareTo(ChargingSession);

        }

        #endregion

        #region CompareTo(ChargingSession)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSession">A charging session object to compare with.</param>
        public Int32 CompareTo(ChargingSession ChargingSession)
        {

            if ((Object) ChargingSession == null)
                throw new ArgumentNullException("The given charging session must not be null!");

            return Id.CompareTo(ChargingSession.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingSession> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a charging session.
            var ChargingSession = Object as ChargingSession;
            if ((Object) ChargingSession == null)
                return false;

            return this.Equals(ChargingSession);

        }

        #endregion

        #region Equals(ChargingSession)

        /// <summary>
        /// Compares two charging sessions for equality.
        /// </summary>
        /// <param name="ChargingSession">A charging session to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSession ChargingSession)
        {

            if ((Object) ChargingSession == null)
                return false;

            return Id.Equals(ChargingSession.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
