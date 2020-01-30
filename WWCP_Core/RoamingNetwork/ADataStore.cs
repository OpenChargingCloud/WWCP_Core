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
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.WWCP.Networking;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP
{

    public abstract class ADataStore<TId, TData> : IEnumerable<TData>
    {

        #region Data

        protected        readonly  Dictionary<TId, TData>  InternalData;

        private          readonly  Object                  Lock  = new Object();

        private   static readonly  Char                    RS    = (Char) 30;

        #endregion

        #region Properties

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        public IRoamingNetwork                  RoamingNetwork      { get; }

        /// <summary>
        /// The name of the logfile.
        /// </summary>
        public String                           LogfileName         { get; }

        Boolean DisableLogfiles { get; }


        private readonly List<RoamingNetworkInfo> _RoamingNetworkInfos;

        /// <summary>
        /// Roaming network informations.
        /// </summary>
        public IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos
            => _RoamingNetworkInfos;

        Boolean DisableNetworkSync { get; }


        public DNSClient DNSClient { get; }

        #endregion

        #region Constructor(s)

        public ADataStore(IRoamingNetwork                  RoamingNetwork,
                          Func<RoamingNetwork_Id, String>  LogFileNameCreator,
                          Boolean                          DisableLogfiles       = false,
                          IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos   = null,
                          Boolean                          DisableNetworkSync    = false,
                          DNSClient                        DNSClient             = null)
        {

            this.InternalData            = new Dictionary<TId, TData>();

            this.RoamingNetwork          = RoamingNetwork ?? throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null or empty!");

            if (LogFileNameCreator == null)
                throw new ArgumentNullException(nameof(LogFileNameCreator), "The given LogFileNameCreator delegate must not be null or empty!");

            this.LogfileName             = LogFileNameCreator(RoamingNetwork.Id);

            this.DisableLogfiles         = DisableLogfiles;

            this._RoamingNetworkInfos    = RoamingNetworkInfos != null
                                               ? new List<RoamingNetworkInfo>(RoamingNetworkInfos)
                                               : new List<RoamingNetworkInfo>();

            this.DisableNetworkSync      = DisableNetworkSync;

            this.DNSClient               = DNSClient ?? new DNSClient(SearchForIPv4DNSServers: true,
                                                                      SearchForIPv6DNSServers: false);

        }

        #endregion


        #region Methods on InternalData

        public Boolean ContainsKey(TId Id)
            => InternalData.ContainsKey(Id);

        public Boolean TryGet(TId Id, out TData Data)
            => InternalData.TryGetValue(Id, out Data);

        #endregion


        #region (protected) LogIt(Command, Id)

        protected void LogIt(String Command, IId Id)
        {
            logIt(Command, Id);
        }

        #endregion

        #region (protected) LogIt(Command, Id, PropertyKey, JSONObject)

        protected void LogIt(String Command, IId Id, String PropertyKey, JObject JSONObject)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), "The given property key must not be null or empty!");

            logIt(Command, Id, PropertyKey, JSONObject);

        }

        #endregion

        #region (protected) LogIt(Command, Id, PropertyKey, JSONArray)

        protected void LogIt(String Command, IId Id, String PropertyKey, JArray JSONArray)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), "The given property key must not be null or empty!");

            logIt(Command, Id, PropertyKey, JSONArray);

        }

        #endregion

        #region (private)   logIt(Command, Id, PropertyKey = null, JSON = null)

        private void logIt(String Command, IId Id, String PropertyKey = null, Object JSON = null)
        {

            Command     = Command?.    Trim();
            PropertyKey = PropertyKey?.Trim();

            if (Command.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Command), "The given command must not be null or empty!");

            var data = JSONObject.Create(

                           new JProperty("timestamp",  DateTime.UtcNow.ToIso8601()),
                           new JProperty("id",         Id.ToString()),
                           new JProperty("command",    Command),

                           PropertyKey != null
                               ? new JProperty(PropertyKey,  JSON)
                               : null

                       ).ToString(Newtonsoft.Json.Formatting.None) +
                         Environment.NewLine;

            if (!DisableLogfiles)
            {
                lock (Lock)
                {
                    File.AppendAllText(LogfileName, data);
                }
            }

            if (!DisableNetworkSync)
            {
                lock (Lock)
                {
                    foreach (var networkInfo in RoamingNetworkInfos)
                    {

                        var client = new TCPClient(RemoteHost:         networkInfo.hostname,
                                                   RemotePort:         networkInfo.port,
                                                   UseTLS:             TLSUsage.NoTLS,
                                                   ConnectionTimeout:  TimeSpan.FromSeconds(5),
                                                   DNSClient:          DNSClient);

                        client.Connect();

                        if (client.TCPStream != null) 
                        {

                            if (client.TCPStream.CanWrite)
                                client.TCPStream.Write(data.ToUTF8Bytes());

                            if (client.TCPStream.CanRead)
                            {

                                client.TCPStream.ReadTimeout = 5000; // msec

                                var message            = new StringBuilder();
                                var buffer             = new Byte[4096];
                                var numberOfBytesRead  = 0;

                                do
                                {

                                    numberOfBytesRead = client.TCPStream.Read(buffer, 0, buffer.Length);

                                    message.Append(Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead));

                                    if (message.ToString() == "ack")
                                        break;

                                } while (client.TCPStream.DataAvailable);

                            }

                        }

                    }
                }
            }

        }

        #endregion


        #region ReloadData(LogfileSearchPattern, ParserFunc)

        protected void ReloadData(String                   LogfileSearchPattern,
                                  Action<String, JObject>  ParserFunc)
        {

            if (ParserFunc == null)
                return;

            try
            {

                JObject JSON = null;

                foreach (var filename in Directory.EnumerateFiles(Directory.GetCurrentDirectory(),
                                                                  LogfileSearchPattern,
                                                                  SearchOption.TopDirectoryOnly).
                                                   OrderBy   (filename => filename))
                {

                    try
                    {

                        File.ReadLines(filename).
                        ForEachCounted((line, counter) => {
                            if (line.IsNeitherNullNorEmpty() && !line.StartsWith("//") && !line.StartsWith("#"))
                            {
                                try
                                {

                                    JSON = JObject.Parse(line);

                                    ParserFunc(JSON["command"].Value<String>(),
                                               JSON);

                                }
                                catch (Exception e)
                                {
                                    DebugX.Log("Could not parse data in '" + filename + "' line "+ counter + ": " + e.Message);
                                }
                            }
                        });

                    }
                    catch (Exception e)
                    {
                        DebugX.Log("Could not parse logfile '" + filename + "': " + e.Message);
                    }

                }

            }
            catch (Exception e)
            {
                DebugX.Log("Could not reload data: " + e.Message);
            }

        }

        #endregion


        #region IEnumerable<TData>

        public IEnumerator<TData> GetEnumerator()
            => InternalData.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => InternalData.Values.GetEnumerator();

        #endregion

    }

}
