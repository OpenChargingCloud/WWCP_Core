/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    public class SendCDRsResult
    {

        #region Properties

        public IId                              AuthorizatorId                { get; }

        public SendCDRsResultType               Status                        { get; }

        public IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords   { get; }

        public String                           Description                   { get; }

        public String                           AdditionalInfo                { get; }

        public TimeSpan?                        Runtime                       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new send charge detail records result.
        /// </summary>
        /// <param name="Status">The status of the request.</param>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="RejectedChargeDetailRecords">An enumeration of rejected charge detail records.</param>
        /// <param name="Description">An optional description of the send charge detail records result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private SendCDRsResult(SendCDRsResultType               Status,
                               IId                              AuthorizatorId,
                               IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords  = null,
                               String                           Description                  = null,
                               TimeSpan?                        Runtime                      = null)
        {

            this.Status                       = Status;
            this.AuthorizatorId               = AuthorizatorId;
            this.RejectedChargeDetailRecords  = RejectedChargeDetailRecords;
            this.Description                  = Description;
            this.Runtime                      = Runtime;

        }

        #endregion


        #region (static) NotForwared(AuthorizatorId, RejectedChargeDetailRecords = null, Description = null, Runtime = null)

        public static SendCDRsResult NotForwared(IId                              AuthorizatorId,
                                                 IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords  = null,
                                                 String                           Description                  = null,
                                                 TimeSpan?                        Runtime                      = null)

            => new SendCDRsResult(SendCDRsResultType.NotForwared,
                                  AuthorizatorId,
                                  RejectedChargeDetailRecords,
                                  Description,
                                  Runtime);

        #endregion

        #region (static) Partly     (AuthorizatorId, RejectedChargeDetailRecords = null, Description = null, Runtime = null)

        public static SendCDRsResult Partly(IId                              AuthorizatorId,
                                            IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords  = null,
                                            String                           Description                  = null,
                                            TimeSpan?                        Runtime                      = null)

            => new SendCDRsResult(SendCDRsResultType.Partly,
                                  AuthorizatorId,
                                  RejectedChargeDetailRecords,
                                  Description,
                                  Runtime);

        #endregion

        #region (static) Enqueued   (AuthorizatorId, Description = null, Runtime = null)

        public static SendCDRsResult Enqueued(IId        AuthorizatorId,
                                              String     Description  = null,
                                              TimeSpan?  Runtime      = null)

            => new SendCDRsResult(SendCDRsResultType.Enqueued,
                                  AuthorizatorId,
                                  Description: Description,
                                  Runtime:     Runtime);

        #endregion

        #region (static) Forwarded  (AuthorizatorId, Runtime = null)

        public static SendCDRsResult Forwarded(IId        AuthorizatorId,
                                               TimeSpan?  Runtime  = null)

            => new SendCDRsResult(SendCDRsResultType.Forwarded,
                                  AuthorizatorId,
                                  Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId, Description = null, Runtime = null)

        public static SendCDRsResult InvalidSessionId(IId        AuthorizatorId,
                                                      String     Description  = null,
                                                      TimeSpan?  Runtime      = null)

            => new SendCDRsResult(SendCDRsResultType.InvalidSessionId,
                                  AuthorizatorId,
                                  Description: Description ?? "Invalid session identification!",
                                  Runtime:     Runtime);

        #endregion

        #region (static) UnknownEVSE(AuthorizatorId, Description = null, Runtime = null)

        public static SendCDRsResult UnknownEVSE(IId        AuthorizatorId,
                                                 String     Description  = null,
                                                 TimeSpan?  Runtime      = null)

            => new SendCDRsResult(SendCDRsResultType.UnknownEVSE,
                                  AuthorizatorId,
                                  Description: Description,
                                  Runtime:     Runtime);

        #endregion

        #region (static) OutOfService(AuthorizatorId, Description = null, Runtime = null)

        public static SendCDRsResult OutOfService(IId        AuthorizatorId,
                                                  String     Description  = null,
                                                  TimeSpan?  Runtime      = null)

            => new SendCDRsResult(SendCDRsResultType.OutOfService,
                                  AuthorizatorId,
                                  Description: Description,
                                  Runtime:     Runtime);

        #endregion

        #region (static) Error(AuthorizatorId, RejectedChargeDetailRecords = null, Description = null, Runtime = null)

        public static SendCDRsResult Error(IId                              AuthorizatorId,
                                           IEnumerable<ChargeDetailRecord>  RejectedChargeDetailRecords  = null,
                                           String                           Description                  = null,
                                           TimeSpan?                        Runtime                      = null)

            => new SendCDRsResult(SendCDRsResultType.Error,
                                  AuthorizatorId,
                                  RejectedChargeDetailRecords,
                                  Description,
                                  Runtime);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Status + " via " + AuthorizatorId;

        #endregion

    }


    public enum SendCDRsResultType
    {

        NotForwared,

        Partly,

        Enqueued,

        Forwarded,

        InvalidSessionId,

        UnknownEVSE,

        OutOfService,

        Error

    }

}
