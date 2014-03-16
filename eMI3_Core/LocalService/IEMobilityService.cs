/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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
using System.Text;

#endregion

namespace org.emi3group.LocalService
{

    /// <summary>
    /// The common interface for all E-Mobility services.
    /// </summary>
    public interface IEMobilityService
    {

        #region Properties

        String AuthorizatorId { get; }

        #endregion

        #region RFID Authorization

        AUTHSTARTResult AuthorizeStart(EVSEOperator_Id  OperatorId,
                                       EVSE_Id          EVSEId,
                                       String           PartnerSessionId,
                                       String           UID);

        AUTHSTOPResult  AuthorizeStop (EVSEOperator_Id  OperatorId,
                                       EVSE_Id          EVSEId,
                                       String           SessionId,
                                       String           PartnerSessionId,
                                       String           UID);

        SENDCDRResult   SendCDR       (EVSE_Id          EVSEId,
                                       String           SessionId,
                                       String           PartnerSessionId,
                                       String           PartnerProductId,
                                       String           UID,
                                       DateTime         ChargeStart,
                                       DateTime         ChargeEnd,
                                       DateTime?        SessionStart    = null,
                                       DateTime?        SessionEnd      = null,
                                       Double?          MeterValueStart = null,
                                       Double?          MeterValueEnd   = null);

        #endregion

    }

}
