/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using cloud.charging.open.protocols.WWCP.Networking;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace cloud.charging.open.protocols.WWCP
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

        public ChargeDetailRecordsStore(RoamingNetwork_Id                 RoamingNetworkId,
                                        IEnumerable<RoamingNetworkInfo>?  RoamingNetworkInfos   = null,
                                        Boolean                           DisableLogfiles       = false,
                                        Boolean                           ReloadDataOnStart     = true,

                                        Boolean                           DisableNetworkSync    = false,
                                        String?                           LoggingPath           = null,
                                        DNSClient?                        DNSClient             = null)

            : base(Name:                  nameof(ChargeDetailRecordsStore),
                   RoamingNetworkId:      RoamingNetworkId,
                   StringIdParser:        ChargingSession_Id.TryParse,

                   CommandProcessor:      (logfilename,
                                           lineNumber,
                                           remoteSocket,
                                           timestamp,
                                           id,
                                           command,
                                           json,
                                           internalData) => false,

                   DisableLogfiles:       DisableLogfiles,
                   LogFilePathCreator:    roamingNetworkId => Path.Combine(LoggingPath ?? AppContext.BaseDirectory, "ChargeDetailRecords"),
                   LogFileNameCreator:    roamingNetworkId => String.Concat("ChargeDetailRecords-",
                                                                            roamingNetworkId, "-",
                                                                            Environment.MachineName, "_",
                                                                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now.Year, "-", org.GraphDefined.Vanaheimr.Illias.Timestamp.Now.Month.ToString("D2"),
                                                                            ".log"),
                   ReloadDataOnStart:     ReloadDataOnStart,
                   LogfileSearchPattern:  roamingNetworkId => "ChargeDetailRecords-" + roamingNetworkId + "-" + Environment.MachineName + "_",

                   RoamingNetworkInfos:   RoamingNetworkInfos,
                   DisableNetworkSync:    DisableNetworkSync,
                   DNSClient:             DNSClient)

        { }

        #endregion


        #region New   (NewChargeDetailRecord)

        public async Task New(ChargeDetailRecord NewChargeDetailRecord)
        {

            if (!InternalData.ContainsKey(NewChargeDetailRecord.SessionId))
                InternalData.TryAdd(NewChargeDetailRecord.SessionId, new ChargeDetailRecordCollection(NewChargeDetailRecord));

            else
                InternalData[NewChargeDetailRecord.SessionId].Add(NewChargeDetailRecord);

            await LogIt("new",
                        NewChargeDetailRecord.SessionId,
                        "chargeDetailRecords",
                        new JArray(NewChargeDetailRecord.ToJSON()));

        }

        #endregion

        #region New   (NewChargeDetailRecords)

        public async Task New(IEnumerable<ChargeDetailRecord> NewChargeDetailRecords)
        {
            foreach (var chargeDetailRecord in NewChargeDetailRecords)
                await New(chargeDetailRecord);
        }

        #endregion


        #region Sent   (SendCDRResults)

        public async Task Sent(IEnumerable<SendCDRResult> SendCDRResults)
        {
            foreach (var sendCDRResult in SendCDRResults)
                await Sent(sendCDRResult);
        }

        #endregion

        #region Sent   (SendCDRResults)

        public async Task Sent(SendCDRResult SendCDRResult)
        {

            if (!InternalData.ContainsKey(SendCDRResult.ChargeDetailRecord.SessionId))
                InternalData.TryAdd(SendCDRResult.ChargeDetailRecord.SessionId,
                                    new ChargeDetailRecordCollection(SendCDRResult.ChargeDetailRecord));

            await LogIt("sent",
                        SendCDRResult.ChargeDetailRecord.SessionId,
                        "sendChargeDetailRecordResult",
                        SendCDRResult.ToJSON(Embedded: true));

        }

        #endregion

        #region Sent   (SendCDRsResults)

        public void Sent(SendCDRsResult SendCDRsResult)
        {

            foreach (var cdrr in SendCDRsResult.ResultMap)
            {
                if (!InternalData.ContainsKey(cdrr.ChargeDetailRecord.SessionId))
                    InternalData.TryAdd(cdrr.ChargeDetailRecord.SessionId,
                                        new ChargeDetailRecordCollection(cdrr.ChargeDetailRecord));
            }

            //LogIt("sent",
            //      SendCDRResult.ChargeDetailRecord.SessionId,
            //      "sendChargeDetailRecordResult",
            //      SendCDRResult.ToJSON());

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
