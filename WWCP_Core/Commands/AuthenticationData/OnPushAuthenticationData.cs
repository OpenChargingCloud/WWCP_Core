/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OICPv2_1.EMP
{

    ///// <summary>
    ///// A delegate called whenever a 'push authentication data' request will be send.
    ///// </summary>
    //public delegate Task OnPushAuthenticationDataRequestHandler (DateTime                                         LogTimestamp,
    //                                                             DateTime                                         RequestTimestamp,
    //                                                             Object                                           Sender,
    //                                                             String                                           SenderId,
    //                                                             EventTracking_Id                                 EventTrackingId,
    //                                                             IEnumerable<AuthorizationIdentification>         AuthorizationIdentifications,
    //                                                             eMobilityProvider_Id                             ProviderId,
    //                                                             ActionType                                       Action,
    //                                                             TimeSpan?                                        RequestTimeout);

    ///// <summary>
    ///// A delegate called whenever a response for a 'push authentication data' request had been received.
    ///// </summary>
    //public delegate Task OnPushAuthenticationDataResponseHandler(DateTime                                         Timestamp,
    //                                                             Object                                           Sender,
    //                                                             String                                           SenderId,
    //                                                             EventTracking_Id                                 EventTrackingId,
    //                                                             IEnumerable<AuthorizationIdentification>         AuthorizationIdentifications,
    //                                                             eMobilityProvider_Id                             ProviderId,
    //                                                             ActionType                                       Action,
    //                                                             TimeSpan?                                        RequestTimeout,
    //                                                             Acknowledgement<PushAuthenticationDataRequest>   Result,
    //                                                             TimeSpan                                         Duration);

}
