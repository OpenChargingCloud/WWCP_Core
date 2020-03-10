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
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.Networking;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP
{

    public class ChargeDetailRecordsStore : ADataStore<ChargingSession_Id, ChargeDetailRecordCollection>
    {

        #region Events

        /// <summary>
        /// An event fired whenever a new charge detail record was registered.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        #endregion

        #region Constructor(s)

        public ChargeDetailRecordsStore(IRoamingNetwork                  RoamingNetwork,
                                        IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                                        Boolean                          DisableLogfiles       = false,
                                        Boolean                          ReloadDataOnStart     = true,

                                        Boolean                          DisableNetworkSync    = false,
                                        DNSClient                        DNSClient             = null)

            : base(RoamingNetwork,
                   RoamingNetworkInfos,

                   DisableLogfiles,
                   "ChargeDetailRecords" + Path.DirectorySeparatorChar,
                   roamingNetworkId => String.Concat("ChargeDetailRecords-",
                                                     roamingNetworkId, "-",
                                                     Environment.MachineName, "_",
                                                     DateTime.UtcNow.Year, "-", DateTime.UtcNow.Month.ToString("D2"),
                                                     ".log"),
                   ReloadDataOnStart,
                   roamingNetworkId => "ChargingSessions-" + roamingNetworkId + "-" + Environment.MachineName + "_",
                   (logfilename, command, json) => null,

                   DisableNetworkSync,
                   DNSClient)

        { }

        #endregion


        #region New   (NewChargeDetailRecords)

        public void New(IEnumerable<ChargeDetailRecord> NewChargeDetailRecords)
        {
            foreach (var chargeDetailRecord in NewChargeDetailRecords)
                New(chargeDetailRecord);
        }

        #endregion

        #region New   (NewChargeDetailRecord)

        public void New(ChargeDetailRecord NewChargeDetailRecord)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(NewChargeDetailRecord.SessionId))
                    InternalData.Add(NewChargeDetailRecord.SessionId, new ChargeDetailRecordCollection(NewChargeDetailRecord));

                else
                    InternalData[NewChargeDetailRecord.SessionId].Add(NewChargeDetailRecord);

                LogIt("new",
                      NewChargeDetailRecord.SessionId,
                      "chargeDetailRecords",
                      new JArray(NewChargeDetailRecord.ToJSON()));

            }

        }

        #endregion


        #region Sent   (SendCDRResults)

        public void Sent(IEnumerable<SendCDRResult> SendCDRResults)
        {
            foreach (var sendCDRResult in SendCDRResults)
                Sent(sendCDRResult);
        }

        #endregion

        #region Sent   (SendCDRResults)

        public void Sent(SendCDRResult SendCDRResult)
        {

            lock (InternalData)
            {

                if (!InternalData.ContainsKey(SendCDRResult.ChargeDetailRecord.SessionId))
                    InternalData.Add(SendCDRResult.ChargeDetailRecord.SessionId,
                                     new ChargeDetailRecordCollection(SendCDRResult.ChargeDetailRecord));

                LogIt("sent",
                      SendCDRResult.ChargeDetailRecord.SessionId,
                      "sendChargeDetailRecordResult",
                      SendCDRResult.ToJSON());

            }

        }

        #endregion

        #region Sent   (SendCDRsResults)

        public void Sent(SendCDRsResult SendCDRsResult)
        {

            lock (InternalData)
            {

                foreach (var cdrr in SendCDRsResult.ResultMap)
                {
                    if (!InternalData.ContainsKey(cdrr.ChargeDetailRecord.SessionId))
                        InternalData.Add(cdrr.ChargeDetailRecord.SessionId,
                                         new ChargeDetailRecordCollection(cdrr.ChargeDetailRecord));
                }

                //LogIt("sent",
                //      SendCDRResult.ChargeDetailRecord.SessionId,
                //      "sendChargeDetailRecordResult",
                //      SendCDRResult.ToJSON());

            }

            //if (SendCDRsResult.ResultMap.Count == 1)

        }

        #endregion

 

        #region ReloadData()

        //public void ReloadData()
        //{

        //    ReloadData("ChargeDetailRecords-" + RoamingNetwork.Id,
        //               (command, json) =>
        //    {
        //        switch (command.ToLower())
        //        {

        //            case "new":

        //                if (json["chargeDetailRecords"] is JArray reservations)
        //                {



        //                }

        //                break;

        //        }
        //    });

        //}

        #endregion


    }

}
