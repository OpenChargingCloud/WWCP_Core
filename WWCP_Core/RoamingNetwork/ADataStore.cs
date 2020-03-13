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
        where TData : IId<TId>
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
        public IRoamingNetwork                  RoamingNetwork          { get; }


        private readonly List<RoamingNetworkInfo> _RoamingNetworkInfos;

        /// <summary>
        /// Roaming network informations.
        /// </summary>
        public IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos
            => _RoamingNetworkInfos;



        /// <summary>
        /// Whether to write data to the log file.
        /// </summary>
        public Boolean                          DisableLogfiles         { get; }

        /// <summary>
        /// The path to all log files.
        /// </summary>
        public String                           LogFilePath             { get; }

        /// <summary>
        /// A delegate for creating the log file.
        /// </summary>
        public Func<RoamingNetwork_Id, String>  LogfileNameCreator      { get; }

        /// <summary>
        /// Whether to reload log file data to restart.
        /// </summary>
        public Boolean                          ReloadDataOnStart       { get; }

        /// <summary>
        /// The log file search pattern for reloading old log files.
        /// </summary>
        public Func<RoamingNetwork_Id, String>  LogfileSearchPattern    { get; }




        public Boolean                          DisableNetworkSync      { get; }






        public DNSClient DNSClient { get; }

        #endregion

        #region Constructor(s)

        public ADataStore(IRoamingNetwork                       RoamingNetwork,
                          IEnumerable<RoamingNetworkInfo>       RoamingNetworkInfos    = null,

                          Boolean                               DisableLogfiles        = false,
                          String                                LogFilePath            = null,
                          Func<RoamingNetwork_Id, String>       LogFileNameCreator     = null,
                          Boolean                               ReloadDataOnStart      = true,
                          Func<RoamingNetwork_Id, String>       LogfileSearchPattern   = null,
                          Func<String, String, JObject, TData>  LogFileParser          = null,
                     //     Action<TData>                         AddFunc                = null,

                          Boolean                               DisableNetworkSync     = false,

                          DNSClient                             DNSClient              = null)

        {

            this.InternalData                   = new Dictionary<TId, TData>();

            this.RoamingNetwork                 = RoamingNetwork       ?? throw new ArgumentNullException(nameof(RoamingNetwork),          "The given roaming network must not be null or empty!");

            this._RoamingNetworkInfos           = RoamingNetworkInfos != null
                                                      ? new List<RoamingNetworkInfo>(RoamingNetworkInfos)
                                                      : new List<RoamingNetworkInfo>();


            this.DisableLogfiles                = DisableLogfiles;
            this.ReloadDataOnStart              = ReloadDataOnStart;

            if (!DisableLogfiles)
            {

                this.LogFilePath                = LogFilePath?.Trim();
                if (this.LogFilePath.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(LogFilePath), "The given log file path must not be null or empty!");

                this.LogfileNameCreator         = LogFileNameCreator   ?? throw new ArgumentNullException(nameof(LogFileNameCreator),    "The given log file name creator must not be null or empty!");

                if (this.ReloadDataOnStart)
                    this.LogfileSearchPattern   = LogfileSearchPattern ?? throw new ArgumentNullException(nameof(LogfileSearchPattern),  "The given log file search pattern must not be null or empty!");

            }


            this.DisableNetworkSync             = DisableNetworkSync;


            this.DNSClient                      = DNSClient ?? new DNSClient(SearchForIPv4DNSServers: true,
                                                                             SearchForIPv6DNSServers: false);

            if (this.ReloadDataOnStart)
                ReloadData(this.LogFilePath,
                           LogfileSearchPattern(this.RoamingNetwork.Id),
                           LogFileParser);

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
                    File.AppendAllText(LogFilePath + LogfileNameCreator(RoamingNetwork.Id), data);
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


        #region ReloadData(LogfileSearchPattern, ParserFunc, AddFunc)

        protected void ReloadData(String                                Path,
                                  String                                LogfileSearchPattern,
                                  Func<String, String, JObject, TData>  ParserFunc)
        {

            if (Path?.Trim().IsNullOrEmpty() == true)
                Path = Directory.GetCurrentDirectory();

            if (ParserFunc == null)
                return;

            try
            {

                JObject JSON     = null;
                String  command  = null;

                foreach (var filename in Directory.EnumerateFiles(Path,
                                                                  LogfileSearchPattern,
                                                                  SearchOption.TopDirectoryOnly).
                                                   OrderBy       (filename => filename))
                {

                    try
                    {

                        TData session = default;

                        File.ReadLines(filename).
                            ForEachCounted((line, counter) => {
                                if (line.IsNeitherNullNorEmpty() && !line.StartsWith("//") && !line.StartsWith("#"))
                                {
                                    try
                                    {

                                        JSON     = JObject.Parse(line);
                                        command  = JSON["command"].Value<String>();
                                        session  = ParserFunc(filename,
                                                              command,
                                                              JSON);

                                        switch (command)
                                        {

                                            case "authStart":
                                            case "remoteStart":
                                                InternalData.Add(session.Id, session);
                                                break;

                                            case "authStop":
                                                InternalData[session.Id] = session;
                                                break;

                                            default:
                                                DebugX.Log("Unknown command '" + command + "' in '" + filename + "' line " + counter + "!");
                                                break;

                                        }

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
