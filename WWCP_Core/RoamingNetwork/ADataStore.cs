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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An generic data store.
    /// </summary>
    /// <typeparam name="TId">The type of the identificators.</typeparam>
    /// <typeparam name="TData">The type of the stored data.</typeparam>
    public abstract class ADataStore<TId, TData> : IEnumerable<TData>
        where TData : IId<TId>
    {

        #region Data

        /// <summary>
        /// The internal data store.
        /// </summary>
        protected      readonly  Dictionary<TId, TData>                                   InternalData;

        private        readonly  Action<String, String, JObject, Dictionary<TId, TData>>  CommandProcessor;

        private        readonly  Object                                                   Lock  = new Object();

        private static readonly  Char                                                     RS    = (Char) 30;


        private TCPServer Server;

        #endregion

        #region Properties

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        public IRoamingNetwork                  RoamingNetwork          { get; }


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


        /// <summary>
        /// Whether to disable network synchronization.
        /// </summary>
        public Boolean                          DisableNetworkSync      { get; }

        private readonly List<RoamingNetworkInfo> _RoamingNetworkInfos;

        /// <summary>
        /// Roaming network informations.
        /// </summary>
        public IEnumerable<RoamingNetworkInfo>  RoamingNetworkInfos
            => _RoamingNetworkInfos;


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient                        DNSClient               { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a generic data store.
        /// </summary>
        /// <param name="RoamingNetwork"></param>
        /// 
        /// <param name="CommandProcessor"></param>
        /// 
        /// <param name="DisableLogfiles"></param>
        /// <param name="LogFilePathCreator"></param>
        /// <param name="LogFileNameCreator"></param>
        /// <param name="ReloadDataOnStart"></param>
        /// <param name="LogfileSearchPattern"></param>
        /// 
        /// <param name="TCPPort"></param>
        /// <param name="RoamingNetworkInfos"></param>
        /// <param name="DisableNetworkSync"></param>
        /// <param name="DNSClient">The DNS client defines which DNS servers to use.</param>
        public ADataStore(IRoamingNetwork                                          RoamingNetwork,

                          Action<String, String, JObject, Dictionary<TId, TData>>  CommandProcessor       = null,

                          Boolean                                                  DisableLogfiles        = false,
                          Func<RoamingNetwork_Id, String>                          LogFilePathCreator     = null,
                          Func<RoamingNetwork_Id, String>                          LogFileNameCreator     = null,
                          Boolean                                                  ReloadDataOnStart      = true,
                          Func<RoamingNetwork_Id, String>                          LogfileSearchPattern   = null,

                          IPPort?                                                  TCPPort                = null,
                          IEnumerable<RoamingNetworkInfo>                          RoamingNetworkInfos    = null,
                          Boolean                                                  DisableNetworkSync     = false,
                          DNSClient                                                DNSClient              = null)

        {

            this.InternalData                  = new Dictionary<TId, TData>();

            this.RoamingNetwork                = RoamingNetwork       ?? throw new ArgumentNullException(nameof(RoamingNetwork),        "The given roaming network must not be null or empty!");

            this._RoamingNetworkInfos          = RoamingNetworkInfos != null
                                                     ? new List<RoamingNetworkInfo>(RoamingNetworkInfos)
                                                     : new List<RoamingNetworkInfo>();

            this.CommandProcessor              = CommandProcessor     ?? throw new ArgumentNullException(nameof(CommandProcessor),      "The given command processor must not be null or empty!");

            this.DisableLogfiles               = DisableLogfiles;
            this.ReloadDataOnStart             = ReloadDataOnStart;

            if (!DisableLogfiles)
            {

                this.LogFilePath               = LogFilePathCreator(this.RoamingNetwork.Id)?.Trim();
                if (this.LogFilePath.IsNullOrEmpty())
                    throw new ArgumentNullException(nameof(LogFilePath), "The given log file path must not be null or empty!");

                this.LogfileNameCreator        = LogFileNameCreator   ?? throw new ArgumentNullException(nameof(LogFileNameCreator),    "The given log file name creator must not be null or empty!");

                if (this.ReloadDataOnStart)
                    this.LogfileSearchPattern  = LogfileSearchPattern ?? throw new ArgumentNullException(nameof(LogfileSearchPattern),  "The given log file search pattern must not be null or empty!");

            }

            this.DisableNetworkSync            = DisableNetworkSync;
            this.DNSClient                     = DNSClient ?? new DNSClient(SearchForIPv4DNSServers: true,
                                                                            SearchForIPv6DNSServers: false);

            if (TCPPort.HasValue)
            {

                this.Server = new TCPServer(Port:               TCPPort.Value,
                                            ConnectionTimeout:  TimeSpan.FromSeconds(20),
                                            Autostart:          true);

                this.Server.OnNotification += (connection) => {

                    try
                    {

                        var LastDataReceivedAt = DateTime.UtcNow;

                        do
                        {

                            try
                            {

                                var data = connection.ReadLine(MaxInitialWaitingTime: TimeSpan.FromSeconds(10),
                                                               __ReadTimeout:         TimeSpan.FromSeconds(20));
                                if (data.IsNotNullOrEmpty())
                                {

                                    Console.WriteLine("Received '" + data + "' from '" + connection.RemoteSocket.ToString() + "'!");

                                    LastDataReceivedAt = DateTime.UtcNow;

                                    ReceiveData(connection.RemoteSocket.ToString(),
                                                JObject.Parse(data));

                                }

                            }
                            catch (Exception e)
                            {
                            }

                        } while (!connection.IsClosed && DateTime.UtcNow - LastDataReceivedAt < TimeSpan.FromSeconds(10));

                    }
                    catch (Exception)
                    { }

                    connection.Close();
                    Console.WriteLine("Connection '" + connection.RemoteSocket.ToString() + "' closed!");

                };

            }

            if (this.ReloadDataOnStart)
                LoadLogFiles(this.LogFilePath,
                             LogfileSearchPattern(this.RoamingNetwork.Id));

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
            LogIt(Command, Id);
        }

        #endregion

        #region (protected) LogIt(Command, Id, PropertyKey, JSONObject)

        protected void LogIt(String Command, IId Id, String PropertyKey, JObject JSONObject)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), "The given property key must not be null or empty!");

            LogIt(Command, Id, PropertyKey, (object)JSONObject);

        }

        #endregion

        #region (protected) LogIt(Command, Id, PropertyKey, JSONArray)

        protected void LogIt(String Command, IId Id, String PropertyKey, JArray JSONArray)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), "The given property key must not be null or empty!");

            LogIt(Command, Id, PropertyKey, (object)JSONArray);

        }

        #endregion

        #region (private)   LogIt(Command, Id, PropertyKey = null, JSON = null)

        private void LogIt(String Command, IId Id, String PropertyKey = null, Object JSON = null)
        {

            #region Prepare data

            Command = Command?.    Trim();
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

            #endregion

            #region Write to log file

            if (!DisableLogfiles)
            {
                lock (Lock)
                {
                    File.AppendAllText(LogFilePath + LogfileNameCreator(RoamingNetwork.Id), data);
                }
            }

            #endregion

            #region Write to other network servers

            if (!DisableNetworkSync && _RoamingNetworkInfos.SafeAny())
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

            #endregion

        }

        #endregion


        #region ReceiveData(RemoteHost, JSON, Parser)

        protected void ReceiveData(String   RemoteHost,
                                   JObject  JSON)
        {

            CommandProcessor(RemoteHost,
                             JSON["command"]?.Value<String>(),
                             JSON,
                             InternalData);

        }

        #endregion

        #region LoadLogFiles(Path, LogfileSearchPattern)

        protected void LoadLogFiles(String  Path,
                                    String  LogfileSearchPattern)
        {

            if (Path?.Trim().IsNullOrEmpty() == true)
                Path = Directory.GetCurrentDirectory();

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

                        File.ReadLines(filename).
                            ForEachCounted((line, counter) => {
                                if (line.IsNeitherNullNorEmpty() && !line.StartsWith("//") && !line.StartsWith("#"))
                                {
                                    try
                                    {

                                        JSON     = JObject.Parse(line);
                                        command  = JSON["command"]?.Value<String>();

                                        switch (command)
                                        {

                                            case "clear":
                                                InternalData.Clear();
                                                break;

                                            default:
                                                CommandProcessor(filename,
                                                                 command,
                                                                 JSON,
                                                                 InternalData);
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
