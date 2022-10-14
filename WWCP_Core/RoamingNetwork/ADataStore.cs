/*
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

using System.Collections;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An generic data store.
    /// </summary>
    /// <typeparam name="TId">The type of the identificators.</typeparam>
    /// <typeparam name="TData">The type of the stored data.</typeparam>
    public abstract class ADataStore<TId, TData> : IEnumerable<TData>
        where TId:   IId
        where TData: IHasId<TId>
    {

        #region Data

        /// <summary>
        /// The internal data store.
        /// </summary>
        protected      readonly  Dictionary<TId, TData>                                                     InternalData;

        private        readonly  Func<String, IPSocket?, String, JObject, Dictionary<TId, TData>, Boolean>  CommandProcessor;

        private        readonly  Object                                                                     Lock  = new Object();

        private static readonly  Char                                                                       RS    = (Char) 30;


        private TCPServer Server;

        #endregion

        #region Properties

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        public RoamingNetwork_Id?                RoamingNetworkId        { get; }


        /// <summary>
        /// Whether to write data to the log file.
        /// </summary>
        public Boolean                           DisableLogfiles         { get; }

        /// <summary>
        /// The path to all log files.
        /// </summary>
        public String                            LogFilePath             { get; }

        /// <summary>
        /// A delegate for creating the log file.
        /// </summary>
        public Func<RoamingNetwork_Id?, String>  LogfileNameCreator      { get; }

        /// <summary>
        /// Whether to reload log file data to restart.
        /// </summary>
        public Boolean                           ReloadDataOnStart       { get; }

        /// <summary>
        /// The log file search pattern for reloading old log files.
        /// </summary>
        public Func<RoamingNetwork_Id?, String>  LogfileSearchPattern    { get; }


        /// <summary>
        /// Whether to disable network synchronization.
        /// </summary>
        public Boolean                           DisableNetworkSync      { get; }


        public Node_Id                           NodeId                  { get; }

        private readonly List<RoamingNetworkInfo> _RoamingNetworkInfos;

        /// <summary>
        /// Roaming network informations.
        /// </summary>
        public IEnumerable<RoamingNetworkInfo>   RoamingNetworkInfos
            => _RoamingNetworkInfos;


        public RoamingNetworkInfo RoamingNetworkInfo
            => RoamingNetworkInfos.SafeWhere(roamingNetworkInfo => roamingNetworkInfo.NodeId == NodeId).FirstOrDefault();


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient                        DNSClient               { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a generic data store.
        /// </summary>
        /// <param name="DNSClient">The DNS client defines which DNS servers to use.</param>
        public ADataStore(Func<String, IPSocket?, String, JObject, Dictionary<TId, TData>, Boolean>?  CommandProcessor       = null,

                          Boolean                                                                     DisableLogfiles        = false,
                          Func<RoamingNetwork_Id?, String>?                                           LogFilePathCreator     = null,
                          Func<RoamingNetwork_Id?, String>?                                           LogFileNameCreator     = null,
                          Boolean                                                                     ReloadDataOnStart      = true,
                          Func<RoamingNetwork_Id?, String>?                                           LogfileSearchPattern   = null,

                          RoamingNetwork_Id?                                                          RoamingNetworkId       = null,
                          IEnumerable<RoamingNetworkInfo>?                                            RoamingNetworkInfos    = null,
                          Boolean                                                                     DisableNetworkSync     = false,
                          DNSClient?                                                                  DNSClient              = null)

        {

            this.NodeId                        = Node_Id.Parse(Environment.MachineName);

            this.InternalData                  = new Dictionary<TId, TData>();

            this.RoamingNetworkId              = RoamingNetworkId;
            this._RoamingNetworkInfos          = RoamingNetworkInfos != null
                                                     ? new List<RoamingNetworkInfo>(RoamingNetworkInfos)
                                                     : new List<RoamingNetworkInfo>();

            this.CommandProcessor              = CommandProcessor     ?? throw new ArgumentNullException(nameof(CommandProcessor),      "The given command processor must not be null or empty!");

            this.DisableLogfiles               = DisableLogfiles;
            this.ReloadDataOnStart             = ReloadDataOnStart;

            if (!DisableLogfiles)
            {

                this.LogFilePath               = LogFilePathCreator(this.RoamingNetworkId)?.Trim() ?? AppContext.BaseDirectory;
                Directory.CreateDirectory(this.LogFilePath);

                this.LogfileNameCreator        = LogFileNameCreator   ?? throw new ArgumentNullException(nameof(LogFileNameCreator),    "The given log file name creator must not be null or empty!");
                this.LogfileSearchPattern      = LogfileSearchPattern;

                if (this.ReloadDataOnStart && this.LogfileSearchPattern == null)
                    throw new ArgumentNullException(nameof(LogfileSearchPattern), "The given log file search pattern must not be null or empty!");

            }

            this.DisableNetworkSync            = DisableNetworkSync;
            this.DNSClient                     = DNSClient ?? new DNSClient(SearchForIPv4DNSServers: true,
                                                                            SearchForIPv6DNSServers: false);

            if (RoamingNetworkInfo is not null)
            {

                this.Server = new TCPServer(Port:               RoamingNetworkInfo.port,
                                            ConnectionTimeout:  TimeSpan.FromSeconds(20),
                                            Autostart:          true);

                this.Server.OnNotification += (connection) => {

                    try
                    {

                        var LastDataReceivedAt = Timestamp.Now;

                        do
                        {

                            try
                            {

                                var data = connection.ReadLine(MaxInitialWaitingTime:  TimeSpan.FromSeconds(5),
                                                               __ReadTimeout:          TimeSpan.FromSeconds(10));
                                if (data.IsNotNullOrEmpty())
                                {

                                    Console.WriteLine("Received '" + data + "' from '" + connection.RemoteSocket.ToString() + "'!");

                                    LastDataReceivedAt = Timestamp.Now;

                                    if (data == "BYE!")
                                        connection.Close();

                                    else
                                    {

                                        try
                                        {

                                            var JSON     = JObject.Parse(data);
                                            var command  = JSON["command"]?.Value<String>();

                                            if (command.IsNotNullOrEmpty())
                                            {

                                                if (CommandProcessor(null,
                                                                     connection.RemoteSocket,
                                                                     command,
                                                                     JSON,
                                                                     InternalData))
                                                {
                                                    LogToDisc(data);
                                                }

                                                connection.WriteLineToResponseStream("ack");

                                            }

                                        }
                                        catch (Exception)
                                        { }

                                    }

                                }

                            }
                            catch (Exception)
                            { }

                        } while (!connection.IsClosed && Timestamp.Now - LastDataReceivedAt < TimeSpan.FromSeconds(10));

                    }
                    catch (Exception)
                    { }

                    try
                    {

                        if (!connection.IsClosed)
                            connection.Close();

                    } catch (Exception)
                    { }

                    Console.WriteLine("Connection '" + connection.RemoteSocket.ToString() + "' closed!");

                };

            }

            if (this.ReloadDataOnStart)
                LoadLogFiles(LogFilePath,
                             LogfileSearchPattern(this.RoamingNetworkId));

        }

        #endregion


        #region Methods on InternalData

        public Boolean ContainsKey(TId Id)
            => InternalData.ContainsKey(Id);

        public Boolean TryGet(TId Id, out TData Data)
            => InternalData.TryGetValue(Id, out Data);

        #endregion


        public void LoadLogfiles()
        {
            LoadLogFiles(LogFilePath,
                         LogfileSearchPattern(this.RoamingNetworkId));
        }


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

            Command      = Command?.    Trim();
            PropertyKey  = PropertyKey?.Trim();

            if (Command.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Command), "The given command must not be null or empty!");

            var data = JSONObject.Create(

                           new JProperty("timestamp",  Timestamp.Now.ToIso8601()),
                           new JProperty("id",         Id.ToString()),
                           new JProperty("command",    Command),

                           PropertyKey != null
                               ? new JProperty(PropertyKey, JSON)
                               : null

                       ).ToString(Newtonsoft.Json.Formatting.None);

            #endregion

            LogToDisc(data);
            LogToNetwork(data);

        }

        #endregion


        #region (private)   LogToDisc(data)

        private void LogToDisc(String data)
        {
            if (!DisableLogfiles && data?.Trim().IsNotNullOrEmpty() == true)
            {
                lock (Lock)
                {
                    File.AppendAllText(LogFilePath + LogfileNameCreator(RoamingNetworkId), data.Trim() + Environment.NewLine);
                }
            }
        }

        #endregion

        #region LogToNetwork(data)

        private void LogToNetwork(String data)
        {

            if (!DisableNetworkSync && _RoamingNetworkInfos.SafeAny() && data?.Trim().IsNotNullOrEmpty() == true)
            {
                lock (Lock)
                {
                    foreach (var networkInfo in RoamingNetworkInfos.Where(networkInfo => networkInfo.NodeId != NodeId))
                    {

                        TCPClient client = null;

                        if (networkInfo.IPAddress != null)
                            client = new TCPClient(IPAddress:          networkInfo.IPAddress,
                                                   RemotePort:         networkInfo.port,
                                                   UseTLS:             TLSUsage.NoTLS,
                                                   ConnectionTimeout:  TimeSpan.FromSeconds(5));

                        else if (networkInfo.hostname.IsNotNullOrEmpty())
                            client = new TCPClient(RemoteHost:         networkInfo.hostname,
                                                   RemotePort:         networkInfo.port,
                                                   UseTLS:             TLSUsage.NoTLS,
                                                   ConnectionTimeout:  TimeSpan.FromSeconds(5),
                                                   DNSClient:          DNSClient);

                        if (client != null)
                        {

                            client.Connect();

                            if (client.TCPStream != null) 
                            {

                                try
                                {

                                    if (client.TCPStream.CanWrite)
                                        client.TCPStream.Write((data.Trim() + Environment.NewLine).ToUTF8Bytes());

                                    //if (client.TCPStream.CanRead)
                                    //{

                                    //    client.TCPStream.ReadTimeout = 5000; // msec

                                    //    var message = new StringBuilder();
                                    //    var buffer = new Byte[4096];
                                    //    var numberOfBytesRead = 0;

                                    //    do
                                    //    {

                                    //        numberOfBytesRead = client.TCPStream.Read(buffer, 0, buffer.Length);

                                    //        message.Append(Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead));

                                    //        if (message.ToString() == "ack")
                                    //            break;

                                    //    } while (client.TCPStream.DataAvailable);

                                    //}

                                }
                                catch (Exception e)
                                {

                                }

                            }

                        }

                    }
                }
            }

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
                                                                 null,
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
