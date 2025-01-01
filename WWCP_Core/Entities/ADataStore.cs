/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Channels;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.WWCP.Networking;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class ReloadStatistics(String                              FileNameSearchPattern,
                                  IEnumerable<Tuple<String, UInt64>>  FileStatistics,
                                  UInt64                              NumberOfCommands,
                                  IEnumerable<String>                 Errors,
                                  TimeSpan                            Runtime)
    {

        public String                              FileNameSearchPattern    { get; } = FileNameSearchPattern;
        public IEnumerable<Tuple<String, UInt64>>  FileStatistics           { get; } = FileStatistics;
        public UInt64                              NumberOfCommands         { get; } = NumberOfCommands;
        public IEnumerable<String>                 Errors                   { get; } = Errors;
        public TimeSpan                            Runtime                  { get; } = Runtime;

        public override String ToString()
            => $"{FileNameSearchPattern}: {FileStatistics.Count()} files, {NumberOfCommands} commands, {Errors.Count()} errors, {Runtime.TotalSeconds} seconds runtime";

    }

    public class LogData(String  FileName,
                         String  Data)
    {
        public String  FileName    { get; } = FileName;
        public String  Data        { get; } = Data;

    }


    public delegate Boolean CommandDelegate<TId, TData>(String                            FileName,
                                                        UInt64                            lineCounter,
                                                        IPSocket?                         Socket,
                                                        DateTime                          Timestamp,
                                                        TId                               Id,
                                                        String                            Command,
                                                        JObject                           JSON,
                                                        ConcurrentDictionary<TId, TData>  InternaleData)
        where TId   : struct, IId
        where TData : IHasId<TId>;


    /// <summary>
    /// An generic data store.
    /// </summary>
    /// <typeparam name="TId">The type of the identificators.</typeparam>
    /// <typeparam name="TData">The type of the stored data.</typeparam>
    public abstract class ADataStore<TId, TData> : IEnumerable<TData>
        where TId:   struct, IId
        where TData: IHasId<TId>
    {

        #region Data

        protected        readonly  Func<String, TId?>                StringIdParser;

            /// <summary>
            /// The maximum number of retries to write to a logfile.
            /// </summary>
        public    static readonly  Byte                              MaxRetries                = 5;


        protected        readonly  ConcurrentDictionary<TId, TData>  InternalData              = new();

        private          readonly  CommandDelegate<TId, TData>       CommandProcessor;

        private          readonly  Channel<LogData>                  discChannel;
        private          readonly  Channel<String>                   networkChannel;
        private          readonly  CancellationTokenSource           cancellationTokenSource;

        private          readonly  TCPServer?                        Server;

        /// <summary>
        /// The default maintenance interval.
        /// </summary>
        public           readonly  TimeSpan                          DefaultMaintenanceEvery   = TimeSpan.FromMinutes(1);

        private          readonly  Timer                             MaintenanceTimer;

        protected static readonly  TimeSpan                          SemaphoreSlimTimeout      = TimeSpan.FromSeconds(5);

        protected static readonly  SemaphoreSlim                     MaintenanceSemaphore      = new (1, 1);

        #endregion

        #region Properties

        /// <summary>
        /// The name of the data store.
        /// </summary>
        public String                            Name                        { get; }

        /// <summary>
        /// The attached roaming network.
        /// </summary>
        public RoamingNetwork_Id                 RoamingNetworkId            { get; }


        #region Regular maintenance tasks

        /// <summary>
        /// Whether the reload of the data store is finished.
        /// </summary>
        public Boolean                           ReloadFinished              { get; protected set; }

        /// <summary>
        /// The maintenance interval.
        /// </summary>
        public TimeSpan                          MaintenanceEvery            { get; }

        /// <summary>
        /// Disable all maintenance tasks.
        /// </summary>
        public Boolean                           DisableMaintenanceTasks     { get; set; }

        #endregion

        #region Log files

        /// <summary>
        /// Whether to write data to the log file.
        /// </summary>
        public Boolean                           DisableLogfiles             { get; }

        /// <summary>
        /// The path to all log files.
        /// </summary>
        public String                            LogFilePath                 { get; }

        /// <summary>
        /// A delegate for creating the log file.
        /// </summary>
        public Func<RoamingNetwork_Id?, String>  LogfileNameCreator          { get; }

        /// <summary>
        /// Whether to reload log file data to restart.
        /// </summary>
        public Boolean                           ReloadDataOnStart           { get; }

        /// <summary>
        /// The log file search pattern for reloading old log files.
        /// </summary>
        public Func<RoamingNetwork_Id?, String>  LogfileSearchPattern        { get; }

        #endregion

        #region Networking

        /// <summary>
        /// Whether to disable network synchronization.
        /// </summary>
        public Boolean                           DisableNetworkSync          { get; }


        public NetworkServiceNode_Id             NodeId                      { get; }

        private readonly List<RoamingNetworkInfo> roamingNetworkInfos;

        /// <summary>
        /// Roaming network informations.
        /// </summary>
        public IEnumerable<RoamingNetworkInfo>   RoamingNetworkInfos
            => roamingNetworkInfos;


        public RoamingNetworkInfo? RoamingNetworkInfo
            => RoamingNetworkInfos.SafeWhere(roamingNetworkInfo => roamingNetworkInfo.NodeId == NodeId).FirstOrDefault();


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient                        DNSClient                    { get; }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a generic data store.
        /// </summary>
        /// <param name="DNSClient">The DNS client defines which DNS servers to use.</param>
        /// 
        /// <param name="DisableMaintenanceTasks">Disable all maintenance tasks.</param>
        /// <param name="MaintenanceInitialDelay">The initial delay of the maintenance tasks.</param>
        /// <param name="MaintenanceEvery">The maintenance intervall.</param>
        public ADataStore(String                            Name,
                          RoamingNetwork_Id                 RoamingNetworkId,
                          Func<String, TId?>                StringIdParser,
                          CommandDelegate<TId, TData>       CommandProcessor,

                          Func<RoamingNetwork_Id?, String>  LogFilePathCreator,
                          Func<RoamingNetwork_Id?, String>  LogFileNameCreator,
                          Func<RoamingNetwork_Id?, String>  LogfileSearchPattern,

                          Boolean                           DisableLogfiles           = false,
                          Boolean                           ReloadDataOnStart         = true,

                          Boolean?                          DisableMaintenanceTasks   = false,
                          TimeSpan?                         MaintenanceInitialDelay   = null,
                          TimeSpan?                         MaintenanceEvery          = null,

                          IEnumerable<RoamingNetworkInfo>?  RoamingNetworkInfos       = null,
                          Boolean                           DisableNetworkSync        = false,
                          DNSClient?                        DNSClient                 = null)

        {

            this.Name                     = Name;
            this.NodeId                   = NetworkServiceNode_Id.Parse(Environment.MachineName);

            this.RoamingNetworkId         = RoamingNetworkId;
            this.roamingNetworkInfos      = RoamingNetworkInfos is not null
                                                ? new List<RoamingNetworkInfo>(RoamingNetworkInfos)
                                                : [];

            this.StringIdParser           = StringIdParser;
            this.CommandProcessor         = CommandProcessor;

            this.LogFilePath              = LogFilePathCreator(this.RoamingNetworkId)?.Trim() ?? AppContext.BaseDirectory;
            this.LogfileNameCreator       = LogFileNameCreator;
            this.LogfileSearchPattern     = LogfileSearchPattern;

            this.DisableLogfiles          = DisableLogfiles;
            this.ReloadDataOnStart        = ReloadDataOnStart;

            if (!DisableLogfiles)
                Directory.CreateDirectory(LogFilePath);


            this.DisableNetworkSync       = DisableNetworkSync;
            this.DNSClient                = DNSClient ?? new DNSClient(
                                                             SearchForIPv4DNSServers: true,
                                                             SearchForIPv6DNSServers: false
                                                         );

            if (RoamingNetworkInfo is not null)
            {

                try
                {

                    this.Server = new TCPServer(
                                      Port:               RoamingNetworkInfo.port,
                                      ConnectionTimeout:  TimeSpan.FromSeconds(20),
                                      AutoStart:          true
                                  );

                    this.Server.OnNotification += (eventTrackingId, connection) => {

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

                                        DebugX.Log($"{Name}: Received '{data}' from '{connection.RemoteSocket}'!");

                                        LastDataReceivedAt = Timestamp.Now;

                                        if (data == "BYE!")
                                            connection.Close();

                                        else
                                        {

                                            try
                                            {

                                                var json       = JObject.Parse(data);

                                                var timestamp  =                     json["timestamp"]?.Value<DateTime>();
                                                var id         = this.StringIdParser(json["id"]?.       Value<String>() ?? "");
                                                var command    =                     json["command"]?.  Value<String>();

                                                if (timestamp.HasValue    &&
                                                    id        is not null &&
                                                    command.  IsNotNullOrEmpty())
                                                {

                                                    if (CommandProcessor(null,
                                                                         0,
                                                                         connection.RemoteSocket,
                                                                         timestamp.Value,
                                                                         (TId) id,
                                                                         command,
                                                                         json,
                                                                         InternalData))
                                                    {
                                                        LogToDisc(data).Wait();
                                                    }

                                                    connection.WriteLineToResponseStream("ack");

                                                }

                                            }
                                            catch
                                            { }

                                        }

                                    }

                                }
                                catch
                                { }

                            } while (!connection.IsClosed && Timestamp.Now - LastDataReceivedAt < TimeSpan.FromSeconds(10));

                        }
                        catch
                        { }

                        try
                        {

                            if (!connection.IsClosed)
                                connection.Close();

                        } catch
                        { }

                        Console.WriteLine($"{Name}: Connection '{connection.RemoteSocket}' closed!");

                    };

                }
                catch (Exception e)
                {
                    DebugX.Log(e, "RoamingNetworkInfo TCP server");
                }

            }


            this.cancellationTokenSource = new CancellationTokenSource();

            #region WriteToDisc channel

            this.discChannel = Channel.CreateUnbounded<LogData>();

            _ = Task.Factory.StartNew(async () => {

                do
                {

                    var logData  = await discChannel.Reader.ReadAsync(cancellationTokenSource.Token);
                    var retry    = 0;

                    do
                    {

                        try
                        {

                            File.AppendAllText(
                                logData.FileName,
                                logData.Data + Environment.NewLine,
                                System.Text.Encoding.UTF8
                            );

                            break;

                        }
                        catch (IOException e)
                        {

                            if (e.HResult != -2147024864)
                            {
                                DebugX.LogT($"File access error while logging to '{logData.FileName}' (retry: {retry}): {e.Message}");
                                Thread.Sleep(100);
                            }

                            else
                            {
                                DebugX.LogT($"Could not log to '{logData.FileName}': {e.Message}");
                                break;
                            }

                        }
                        catch (Exception e)
                        {
                            DebugX.LogT($"Could not log to '{logData.FileName}': {e.Message}");
                            break;
                        }

                    }
                    while (retry++ < MaxRetries);

                    if (retry >= MaxRetries)
                        DebugX.LogT($"Could not write to logfile '{logData.FileName}' for {retry} retries!");

                    else if (retry > 0)
                        DebugX.LogT($"Successfully written to logfile '{logData.FileName}' after {retry} retries!");

                }
                while (!cancellationTokenSource.IsCancellationRequested);

            }, cancellationTokenSource.Token);

            #endregion

            #region WriteToNetwork channel

            this.networkChannel = Channel.CreateUnbounded<String>();

            _ = Task.Factory.StartNew(async () => {

                do
                {

                    var logData  = await networkChannel.Reader.ReadAsync(cancellationTokenSource.Token);
                    var retry    = 0;

                    try
                    {

                        foreach (var networkInfo in this.RoamingNetworkInfos.Where(networkInfo => networkInfo.NodeId != NodeId))
                        {

                            do
                            {

                                //ToDo: Use persistent TCPClients!

                                TCPClient? client = null;

                                if (networkInfo.IPAddress is not null)
                                    client = new TCPClient(
                                                 IPAddress:          networkInfo.IPAddress,
                                                 RemotePort:         networkInfo.port,
                                                 UseTLS:             TLSUsage.NoTLS,
                                                 ConnectionTimeout:  TimeSpan.FromSeconds(5)
                                             );

                                else if (networkInfo.hostname.IsNotNullOrEmpty())
                                    client = new TCPClient(
                                                 RemoteHost:         networkInfo.hostname,
                                                 RemotePort:         networkInfo.port,
                                                 UseTLS:             TLSUsage.NoTLS,
                                                 ConnectionTimeout:  TimeSpan.FromSeconds(5),
                                                 DNSClient:          DNSClient
                                             );

                                if (client is not null)
                                {

                                    client.Connect();

                                    if (client.TCPStream is not null)
                                    {

                                        try
                                        {

                                            if (client.TCPStream.CanWrite)
                                                client.TCPStream.Write((logData + Environment.NewLine).ToUTF8Bytes());

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
                                            DebugX.LogT($"Could not log to '{networkInfo}': {e.Message}");
                                            break;
                                        }

                                    }

                                }

                            }
                            while (retry++ < MaxRetries);

                            if (retry >= MaxRetries)
                                DebugX.LogT($"Could not write to logfile '{networkInfo}' for {retry} retries!");

                            else if (retry > 0)
                                DebugX.LogT($"Successfully written to logfile '{networkInfo}' after {retry} retries!");

                        }

                        break;

                    }
                    catch (Exception e)
                    {
                        DebugX.LogT($"Could not log to network: {e.Message}");
                        break;
                    }

                }
                while (!cancellationTokenSource.IsCancellationRequested);

            }, cancellationTokenSource.Token);

            #endregion


            DebugX.Log($"{Name}: Using logfile path: {LogFilePath} {LogfileSearchPattern(RoamingNetworkId)}");
            DebugX.Log($"DisableLogfiles: {this.DisableLogfiles}");
            DebugX.Log($"ReloadDataOnStart: {this.ReloadDataOnStart}");

            if (!DisableLogfiles && this.ReloadDataOnStart)
                LoadLogFiles(LogFilePath,
                             LogfileSearchPattern(this.RoamingNetworkId));


            // Setup Maintenance Task
            this.DisableMaintenanceTasks  = DisableMaintenanceTasks ?? false;
            this.MaintenanceEvery         = MaintenanceEvery        ?? DefaultMaintenanceEvery;
            this.MaintenanceTimer         = new Timer(
                                                DoMaintenanceSync,
                                                this,
                                                MaintenanceInitialDelay ?? this.MaintenanceEvery,
                                                this.MaintenanceEvery
                                            );

        }

        #endregion


        #region Methods on InternalData

        public Boolean ContainsKey(TId Id)
            => InternalData.ContainsKey(Id);

        public Boolean TryGet(TId                             Id,
                              [NotNullWhen(true)] out TData?  Data)

            => InternalData.TryGetValue(Id, out Data);

        public TData? Get(TId Id)
        {

            if (InternalData.TryGetValue(Id, out var data))
                return data;

            return default;

        }

        #endregion


        #region (Timer) DoMaintenance(State)

        private void DoMaintenanceSync(Object? State)
        {
            if (ReloadFinished && !DisableMaintenanceTasks)
                DoMaintenanceAsync(State).ConfigureAwait(false);
        }

        protected internal virtual Task DoMaintenance(Object? State)
            => Task.CompletedTask;

        private async Task DoMaintenanceAsync(Object? State)
        {

            if (await MaintenanceSemaphore.WaitAsync(SemaphoreSlimTimeout).
                                           ConfigureAwait(false))
            {
                try
                {

                    await DoMaintenance(State);

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.LogException(e);

                }
                finally
                {
                    MaintenanceSemaphore.Release();
                }
            }
            else
                DebugX.LogT($"{Name}: Could not aquire the maintenance tasks lock!");

        }

        #endregion


        #region (protected) LogIt         (Command, Id)

        protected async Task LogIt(String  Command,
                                   IId     Id)
        {

            await LogItInternal(
                      Command,
                      Id
                  );

        }

        #endregion

        #region (protected) LogIt         (Command, Id, PropertyKey, JSONObject)

        protected async Task LogIt(String   Command,
                                   IId      Id,
                                   String   PropertyKey,
                                   JObject  JSONObject)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), $"{Name}: The given property key must not be null or empty!");

            await LogItInternal(
                      Command,
                      Id,
                      PropertyKey,
                      JSONObject
                  );

        }

        #endregion

        #region (protected) LogIt         (Command, Id, PropertyKey, JSONArray)

        protected async Task LogIt(String  Command,
                                   IId     Id,
                                   String  PropertyKey,
                                   JArray  JSONArray)
        {

            if (PropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(PropertyKey), $"{Name}: The given property key must not be null or empty!");

            await LogItInternal(
                      Command,
                      Id,
                      PropertyKey,
                      JSONArray
                  );

        }

        #endregion

        #region (private)   LogItInternal (Command, Id, PropertyKey = null, Data = null)

        private async Task LogItInternal(String   Command,
                                         IId      Id,
                                         String?  PropertyKey   = null,
                                         JToken?  Data          = null)
        {

            if (DisableLogfiles && DisableNetworkSync)
                return;

            #region Initial checks

            if (Command.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Command), $"{Name}: The given command must not be null or empty!");

            #endregion

            Command      = Command.     Trim();
            PropertyKey  = PropertyKey?.Trim();

            if (Command.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Command), $"{Name}: The given command must not be null or empty!");

            var logData = JSONObject.Create(

                              new JProperty("timestamp",  Timestamp.Now.ToIso8601()),
                              new JProperty("id",         Id.ToString()),
                              new JProperty("command",    Command),

                              PropertyKey is not null
                                  ? new JProperty(PropertyKey, Data)
                                  : null

                          ).ToString(Newtonsoft.Json.Formatting.None);

            await LogToDisc(logData);
            await LogToNetwork(logData);

        }

        #endregion


        #region (private) LogToDisc(Data)

        private async Task LogToDisc(String Data)
        {
            if (!DisableLogfiles)
                await discChannel.Writer.WriteAsync(
                          new LogData(
                              Path.Combine(LogFilePath, LogfileNameCreator(RoamingNetworkId)),
                              Data
                          )
                      );
        }

        #endregion

        #region (private) LogToNetwork(Data)

        private async Task LogToNetwork(String Data)
        {
            if (!DisableNetworkSync && roamingNetworkInfos.Count > 0)
                await networkChannel.Writer.WriteAsync(
                          Data
                      );
        }

        #endregion


        #region LoadLogFiles()

        public ReloadStatistics LoadLogfiles()

            => LoadLogFiles(LogFilePath,
                            LogfileSearchPattern(RoamingNetworkId));

        #endregion

        #region LoadLogFiles(Path, LogfileSearchPattern)

        protected ReloadStatistics LoadLogFiles(String  Path,
                                                String  LogfileSearchPattern)
        {

            var startTime         = Timestamp.Now;
            var listOfFilenames   = new List<Tuple<String, UInt64>>();
            var numberOfCommands  = 0UL;
            var listOfErrors      = new List<String>();

            if (Path.Trim().IsNullOrEmpty() == true)
                Path = Directory.GetCurrentDirectory();

            try
            {

                var filenames = Directory.EnumerateFiles(Path,
                                                         LogfileSearchPattern,
                                                         SearchOption.TopDirectoryOnly).
                                          OrderBy(filename => filename).
                                          ToArray();

                DebugX.Log($"{Name}: Found {filenames.Length} log files matching '{LogfileSearchPattern}' at: '{Path}'!");

                foreach (var filename in filenames)
                {

                    try
                    {

                        DebugX.Log($"{Name}: Processing logfile '{filename}'!");

                        var lineNumber = 0UL;

                        foreach (var line in File.ReadLines(filename))
                        {

                            lineNumber++;

                            if (line.IsNeitherNullNorEmpty() && !line.StartsWith("//") && !line.StartsWith('#'))
                            {
                                try
                                {

                                    var json       = JObject.Parse(line);

                                    var timestamp  =                json["timestamp"]?.Value<DateTime>();
                                    var id         = StringIdParser(json["id"]?.       Value<String>() ?? "");
                                    var command    =                json["command"]?.  Value<String>();

                                    if (timestamp.HasValue    &&
                                        id        is not null &&
                                        command.  IsNotNullOrEmpty())
                                    {

                                        numberOfCommands++;

                                        switch (command)
                                        {

                                            case "clear":
                                                InternalData.Clear();
                                                break;

                                            default:
                                                CommandProcessor(filename,
                                                                 lineNumber,
                                                                 null,
                                                                 timestamp.Value,
                                                                 (TId) id,
                                                                 command,
                                                                 json,
                                                                 InternalData);
                                                break;

                                        }

                                    }

                                }
                                catch (Exception e)
                                {

                                    var errorMessage = $"Could not parse data in '{filename}' line {lineNumber}: {e.Message}";

                                    listOfErrors.Add(errorMessage);
                                    DebugX.Log(errorMessage);

                                }
                            }

                        }

                        listOfFilenames.Add(new Tuple<String, UInt64>(filename, lineNumber));
                        DebugX.Log($"{Name}: Processed logfile '{filename}' with {lineNumber} lines!");

                    }
                    catch (Exception e)
                    {
                        DebugX.Log($"{Name}: Could not parse logfile '{filename}': {e.Message}");
                    }

                }

            }
            catch (Exception e)
            {
                DebugX.Log($"{Name}: Could not reload data: {e.Message}");
            }


            var statistics = new ReloadStatistics(
                                 LogfileSearchPattern,
                                 listOfFilenames,
                                 numberOfCommands,
                                 listOfErrors,
                                 Timestamp.Now - startTime
                             );

            if (listOfFilenames.Count > 0 && numberOfCommands > 0)
                DebugX.Log($"{Name}: {statistics}");

            ReloadFinished = true;

            return statistics;

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
