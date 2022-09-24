﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;

using Org.BouncyCastle.Bcpg.OpenPgp;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.Mail;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate called whenever a property of the given object changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the event.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="Sender">The changed object.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnPropertyChangedDelegate(DateTime Timestamp, EventTracking_Id EventTrackingId, Object Sender, String PropertyName, Object OldValue, Object NewValue);


    /// <summary>
    /// A delegate called whenever a charging station should be signed.
    /// </summary>
    public delegate Signature ChargingStationSignatureDelegate(ChargingStation ChargingStation, PgpSecretKey SecretKey);

    /// <summary>
    /// A delegate called whenever a charging pool should be signed.
    /// </summary>
    public delegate Signature ChargingPoolSignatureDelegate(ChargingPool ChargingPool, PgpSecretKey SecretKey);

    /// <summary>
    /// A delegate called whenever a charging station operator should be signed.
    /// </summary>
    public delegate Signature ChargingStationOperatorSignatureDelegate(ChargingStationOperator ChargingStationOperator, PgpSecretKey SecretKey);



    #region OnEVSEStatusDiffEMailCreator

    public delegate EMail OnEVSEStatusDiffEMailCreatorDelegate(EVSEStatusDiff EVSEStatusDiff);

    #endregion

}
