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

using org.GraphDefined.Vanaheimr.Illias;

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class AddEVSEsResult : AEnititiesResult<AddEVSEResult, IEVSE, EVSE_Id>
    {

        #region Constructor(s)

        public AddEVSEsResult(PushDataResultTypes          Result,
                              IEnumerable<AddEVSEResult>?  SuccessfulEVSEs   = null,
                              IEnumerable<AddEVSEResult>?  RejectedEVSEs     = null,
                              IId?                         AuthId            = null,
                              Object?                      SendPOIData       = null,
                              EventTracking_Id?            EventTrackingId   = null,
                              I18NString?                  Description       = null,
                              IEnumerable<Warning>?        Warnings          = null,
                              TimeSpan?                    Runtime           = null)

            : base(Result,
                   SuccessfulEVSEs,
                   RejectedEVSEs,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        //public static AddEVSEsResult Success(IEnumerable<AEnitityResult<IEVSE, EVSE_Id>>  AddEVSEResults,
        //                                     EventTracking_Id                             EventTrackingId)

        //    => new (AddEVSEResults,
        //            EventTrackingId,
        //            true,
        //            null,
        //            null);

        //public static AddEVSEsResult NoOperation(IEnumerable<AEnitityResult<IEVSE, EVSE_Id>>  AddEVSEResults,
        //                                         EventTracking_Id                             EventTrackingId,
        //                                         String                                       Description)

        //    => new (AddEVSEResults,
        //            EventTrackingId,
        //            true,
        //            null,
        //            I18NString.Create(
        //                Languages.en,
        //                Description
        //            ));


        //public static AddEVSEsResult Failed(IEnumerable<AEnitityResult<IEVSE, EVSE_Id>>  AddEVSEResults,
        //                                    EventTracking_Id                             EventTrackingId,
        //                                    String                                       Description)

        //    => new (AddEVSEResults,
        //            EventTrackingId,
        //            false,
        //            null,
        //            I18NString.Create(
        //                Languages.en,
        //                Description
        //            ));

        #region (static) NoOperation

        public static AddEVSEsResult

            NoOperation(IEnumerable<IEVSE>     RejectedEVSEs,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        EventTracking_Id?      EventTrackingId   = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

            {

                EventTrackingId ??= EventTracking_Id.New;

                return new (PushDataResultTypes.NoOperation,
                            Array.Empty<AddEVSEResult>(),
                            RejectedEVSEs.Select(evse => AddEVSEResult.NoOperation(evse,
                                                                                   EventTrackingId,
                                                                                   AuthId,
                                                                                   SendPOIData)),
                            AuthId,
                            SendPOIData,
                            EventTrackingId,
                            Description,
                            Warnings,
                            Runtime);

            }

        #endregion


    }

}
