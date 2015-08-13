/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charge detail record for a charging session.
    /// </summary>
    public class ChargeDetailRecord : IEquatable <ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Data

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status history.
        /// </summary>
        public const UInt16 DefaultStationStatusHistorySize = 50;

        #endregion

        #region Properties

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// The charging session identification from the Authorize Start request.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #region PartnerSessionId

        private readonly ChargingSession_Id _PartnerSessionId;

        [Optional]
        public ChargingSession_Id PartnerSessionId
        {
            get
            {
                return _PartnerSessionId;
            }
        }

        #endregion

        #region PartnerProductId

        private readonly ChargingProduct_Id _PartnerProductId;

        [Optional]
        public ChargingProduct_Id PartnerProductId
        {
            get
            {
                return _PartnerProductId;
            }
        }

        #endregion

        #region ChargingPoolId

        private readonly ChargingPool_Id _ChargingPoolId;

        [Optional]
        public ChargingPool_Id ChargingPoolId
        {
            get
            {
                return _ChargingPoolId;
            }
        }

        #endregion

        #region ChargingStationId

        private readonly ChargingStation_Id _ChargingStationId;

        [Optional]
        public ChargingStation_Id ChargingStationId
        {
            get
            {
                return _ChargingStationId;
            }
        }

        #endregion

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        [Optional]
        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        [Optional]
        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #region eMAId

        private readonly eMA_Id _eMAId;

        [Optional]
        public eMA_Id eMAId
        {
            get
            {
                return _eMAId;
            }
        }

        #endregion

        #region ParkingTime

        private readonly StartEndTime? _ParkingTime;

        [Mandatory]
        public StartEndTime? ParkingTime
        {
            get
            {
                return _ParkingTime;
            }
        }

        #endregion

        #region SessionTime

        private readonly StartEndTime? _SessionTime;

        [Mandatory]
        public StartEndTime? SessionTime
        {
            get
            {
                return _SessionTime;
            }
        }

        #endregion

        #region ChargingTime

        private readonly StartEndTime? _ChargingTime;

        [Mandatory]
        public StartEndTime? ChargingTime
        {
            get
            {
                return _ChargingTime;
            }
        }

        #endregion

        #region ConsumedEnergy

        private readonly Double? _ConsumedEnergy;

        [Optional]
        public Double? ConsumedEnergy
        {
            get
            {
                return _ConsumedEnergy;
            }
        }

        #endregion

        #region MeterValues

        private readonly IEnumerable<Timestamped<Double>> _MeterValues;

        [Optional]
        public IEnumerable<Timestamped<Double>> MeterValues
        {
            get
            {
                return _MeterValues;
            }
        }

        #endregion

        #region MeteringSignature

        private readonly String _MeteringSignature;

        [Optional]
        public String MeteringSignature
        {
            get
            {
                return _MeteringSignature;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charge detail record for the given charging session (identification).
        /// </summary>
        /// <param name="SessionId">The charging session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">An unqiue identification for the consumed charging product.</param>

        /// <param name="EVSEId">An EVSE identification.</param>
        
        /// <param name="SessionStart">The timestamp of the session start.</param>
        /// <param name="SessionEnd">The timestamp of the session end.</param>
        /// <param name="AuthToken">An optional (RFID) user identification.</param>
        /// <param name="eMAId">An optional e-Mobility account identification.</param>
        /// <param name="PartnerSessionId">An optional partner session identification.</param>
        /// <param name="ChargingTime">Optional timestamps of the charging start/stop.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="HubOperatorId">An optional identification of the hub operator.</param>
        /// <param name="HubProviderId">An optional identification of the hub provider.</param>
        public ChargeDetailRecord(ChargingSession_Id                SessionId,
                                  ChargingSession_Id                PartnerSessionId   = null,
                                  ChargingProduct_Id                PartnerProductId   = null,
                                  ChargingPool_Id                   ChargingPoolId     = null,
                                  ChargingStation_Id                ChargingStationId  = null,
                                  EVSE_Id                           EVSEId             = null,
                                  Auth_Token                        AuthToken          = null,
                                  eMA_Id                            eMAId              = null,
                                  StartEndTime?                     ParkingTime        = null,
                                  StartEndTime?                     SessionTime        = null,
                                  StartEndTime?                     ChargingTime       = null,
                                  Double?                           ConsumedEnergy     = null,
                                  IEnumerable<Timestamped<Double>>  MeterValues        = null,
                                  String                            MeteringSignature  = null)

        {

            #region Initial checks

            if (SessionId == null)
                throw new ArgumentNullException("Id", "The charging session identification must not be null!");

            #endregion

            this._SessionId         = SessionId;
            this._PartnerSessionId  = PartnerSessionId;
            this._PartnerProductId  = PartnerProductId;

        }

        #endregion


        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charge detail record.
            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record!");

            return CompareTo(ChargeDetailRecord);

        }

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentNullException("The given charge detail record must not be null!");

            return _SessionId.CompareTo(ChargeDetailRecord._SessionId);

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

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

            // Check if the given object is a charge detail record.
            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                return false;

            return this.Equals(ChargeDetailRecord);

        }

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                return false;

            return _SessionId.Equals(ChargeDetailRecord._SessionId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _SessionId.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _SessionId.ToString();
        }

        #endregion

    }

}
