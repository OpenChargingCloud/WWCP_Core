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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A e-mobility roaming provider.
    /// </summary>
    public class EMPRoamingProvider : ARoamingProvider,
                                      IeMobilityRoamingService
    {

        #region Properties

        #region eMobilityRoamingService

        private readonly IeMobilityRoamingService _eMobilityRoamingService;

        public IeMobilityRoamingService eMobilityRoamingService
        {
            get
            {
                return _eMobilityRoamingService;
            }
        }

        #endregion


        public Authorizator_Id AuthorizatorId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RoamingNetwork_Id RoamingNetworkId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Events

        // Client methods (logging)


        // Server methods

        #region OnAuthorizeStart/-Stop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartEVSEDelegate OnAuthorizeStartEVSE
        {

            add
            {
                _eMobilityRoamingService.OnAuthorizeStartEVSE += value;
            }

            remove
            {
                _eMobilityRoamingService.OnAuthorizeStartEVSE -= value;
            }

        }

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStopEVSEDelegate OnAuthorizeStopEVSE
        {

            add
            {
                _eMobilityRoamingService.OnAuthorizeStopEVSE += value;
            }

            remove
            {
                _eMobilityRoamingService.OnAuthorizeStopEVSE -= value;
            }

        }

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate OnChargeDetailRecord
        {

            add
            {
                _eMobilityRoamingService.OnChargeDetailRecord += value;
            }

            remove
            {
                _eMobilityRoamingService.OnChargeDetailRecord -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an e-mobility roaming provider.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        /// <param name="OperatorRoamingService">The attached local or remote EVSE operator roaming service.</param>
        /// <param name="eMobilityRoamingService">The attached local or remote e-mobility roaming service.</param>
        internal EMPRoamingProvider(RoamingProvider_Id        Id,
                                    I18NString                Name,
                                    RoamingNetwork            RoamingNetwork,
                                    IeMobilityRoamingService  eMobilityRoamingService)

            : base(Id, Name, RoamingNetwork)

        {

            #region Initial Checks

            if (eMobilityRoamingService == null)
                throw new ArgumentNullException(nameof(eMobilityRoamingService),  "The given e-mobility roaming service must not be null!");

            #endregion


            this._eMobilityRoamingService  = eMobilityRoamingService;


            #region Link AuthorizeStart/-Stop and SendCDR to the roaming network

            this.OnAuthorizeStartEVSE += (Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          OperatorId,
                                          AuthToken,
                                          EVSEId,
                                          ChargingProductId,
                                          SessionId,
                                          QueryTimeout) => RoamingNetwork.AuthorizeStart(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         AuthToken,
                                                                                         EVSEId,
                                                                                         ChargingProductId,
                                                                                         SessionId,
                                                                                         QueryTimeout);

            this.OnAuthorizeStopEVSE += (Timestamp,
                                         CancellationToken,
                                         EventTrackingId,
                                         OperatorId,
                                         EVSEId,
                                         SessionId,
                                         AuthToken,
                                         QueryTimeout) => RoamingNetwork.AuthorizeStop(Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       OperatorId,
                                                                                       SessionId,
                                                                                       AuthToken,
                                                                                       EVSEId,
                                                                                       QueryTimeout);

            this.OnChargeDetailRecord += (Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          ChargeDetailRecord,
                                          QueryTimeout) => RoamingNetwork.SendChargeDetailRecord(Timestamp,
                                                                                                 CancellationToken,
                                                                                                 EventTrackingId,
                                                                                                 ChargeDetailRecord,
                                                                                                 QueryTimeout);

            #endregion

        }


        #endregion


        #region IComparable<RoamingProvider> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as EMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                throw new ArgumentException("The given object is not a roaming provider!");

            return CompareTo(RoamingProvider);

        }

        #endregion

        #region CompareTo(RoamingProvider)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider object to compare with.</param>
        public Int32 CompareTo(EMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                throw new ArgumentNullException("The given roaming provider must not be null!");

            return Id.CompareTo(RoamingProvider.Id);

        }

        #endregion

        #endregion

        #region IEquatable<RoamingProvider> Members

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

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as EMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                return false;

            return this.Equals(RoamingProvider);

        }

        #endregion

        #region Equals(RoamingProvider)

        /// <summary>
        /// Compares two roaming provider for equality.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                return false;

            return Id.Equals(RoamingProvider.Id);

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

