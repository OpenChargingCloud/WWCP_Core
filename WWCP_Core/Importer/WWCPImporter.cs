﻿/*
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

using System.Text;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace cloud.charging.open.protocols.WWCP.Importer
{

    public delegate Task<T> GetDataDelegate<T>(DateTime         Timestamp,
                                               WWCPImporter<T>  WWCPImporter,
                                               DateTime         LastRuntimestamp,
                                               UInt64           LastRunId,
                                               DNSClient        DNSClient)

        where T : class;


    public delegate IEnumerable<ImporterForwardingInfo> CreateForwardingTableDelegate<T>(WWCPImporter<T>                                                 Importer,
                                                                                         T                                                               Input,
                                                                                         IEnumerable<ChargingStationOperator>                            AllChargingStationOperators,
                                                                                         Func<ChargingStation_Id, IEnumerable<ChargingStationOperator>>  ChargingStationOperatorSelector,
                                                                                         Func<ChargingStation_Id, ChargingStationOperator>               DefaultChargingStationOperator)

        where T : class;


    /// <summary>
    /// Import data into the WWCP in-memory datastructures.
    /// </summary>
    /// <typeparam name="T">The type of data which will be processed on every update run.</typeparam>
    public class WWCPImporter<T>
        where T: class
    {

        #region Data

        private                  Boolean                            Started;
        //private readonly         Object                             ImporterRunLock;
        private static readonly  SemaphoreSlim                      ImporterRunSemaphore = new (1, 1);
        private readonly         Timer                              ImporterRunTimer;

        /// <summary>
        /// The default time span between importer runs.
        /// </summary>
        public  readonly static  TimeSpan                           DefaultImportEvery                = TimeSpan.FromMinutes(1);

        private readonly         Action<WWCPImporter<T>, T>         OnStartup;
        private readonly         GetDataDelegate<T>                 GetData;
        private readonly         Action<WWCPImporter<T>, T>         OnEveryRun;
      //  private readonly         CreateForwardingTableDelegate<T>   CreateForwardingTable;

        /// <summary>
        /// The default number of cached data imports.
        /// </summary>
        public  const            UInt32                             DefaultMaxNumberOfCachedImports   = 3;

        private readonly         Action<WWCPImporter<T>, T>         OnShutdown;

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this WWCP importer.
        /// </summary>
        public String                                                          Id                                  { get; }

        /// <summary>
        /// The prefix of the importer forwarding files.
        /// </summary>
        //public String                                                          ForwardingFilenamePrefix            { get; }


        public IEnumerable<ChargingStationOperator>                            AllChargingStationOperators         { get; }

        public Func<ChargingStation_Id, IEnumerable<ChargingStationOperator>>  GetChargingStationOperators         { get; }

        public Func<ChargingStation_Id, ChargingStationOperator>               GetDefaultChargingStationOperator   { get; }

        public Func<ChargingStation_Id, IEnumerable<RoamingNetwork_Id>>        RoamingNetworkIds
            => operators => GetChargingStationOperators(operators).Select(cso => cso.RoamingNetwork.Id);


        /// <summary>
        /// The time span between importer runs.
        /// </summary>
        public TimeSpan                                                        ImportEvery                         { get; }

        #region LastRunId

        private UInt64 lastRunId;

        public UInt64 LastRunId
            => lastRunId;

        #endregion

        #region LastRunTimestamp

        public DateTime  LastRunTimestamp    { get; private set; }

        #endregion


        #region AllForwardingInfos

        //private readonly Dictionary<ChargingStation_Id, ImporterForwardingInfo> _AllForwardingInfos;

        //public IEnumerable<ImporterForwardingInfo> AllForwardingInfos

        //    => _AllForwardingInfos.
        //           OrderBy(AllForwardingInfos => AllForwardingInfos.Key).
        //           Select (AllForwardingInfos => AllForwardingInfos.Value);

        #endregion

        //public Boolean Get(ChargingStation_Id ChargingStationId, out ImporterForwardingInfo ImporterForwardingInfo)

        //    => _AllForwardingInfos.TryGetValue(ChargingStationId, out ImporterForwardingInfo);


        #region ImportedData

        private List<Timestamped<T>> _ImportedData;

        public IEnumerable<Timestamped<T>> ImportedData

            => _ImportedData;

        #endregion

        /// <summary>
        /// The number of cached data imports.
        /// </summary>
        public UInt32                                                          MaxNumberOfCachedDataImports     { get; }

        public DNSClient                                                       DNSClient                        { get; }

        #endregion

        #region Events

        #region OnNewForwardingInformation

        /// <summary>
        /// A delegate called whenever a new forwarding information was added.
        /// </summary>
        public delegate Task OnNewForwardingInformationDelegate(DateTime                Timestamp,
                                                                WWCPImporter<T>         Importer,
                                                                ImporterForwardingInfo  ForwardingInfo);

        /// <summary>
        /// An event fired whenever a new forwarding information was added.
        /// </summary>
        public event OnNewForwardingInformationDelegate OnNewForwardingInformation;

        #endregion

        #region OnForwardingInformationChanged

        /// <summary>
        /// A delegate called whenever a new forwarding information was added.
        /// </summary>
        public delegate Task OnForwardingInformationChangedDelegate(DateTime                Timestamp,
                                                                    WWCPImporter<T>         Importer,
                                                                    ImporterForwardingInfo  OldForwardingInfo,
                                                                    ImporterForwardingInfo  NewForwardingInfo);

        /// <summary>
        /// An event fired whenever a new forwarding information was added.
        /// </summary>
        public event OnForwardingInformationChangedDelegate OnForwardingInformationChanged;

        #endregion

        #region OnForwardingChanged

        /// <summary>
        /// A delegate called whenever a new forwarding information was changed.
        /// </summary>
        public delegate Task OnForwardingChangedDelegate(DateTime                Timestamp,
                                                         WWCPImporter<T>         Sender,
                                                         ImporterForwardingInfo  ForwardingInfo,
                                                         RoamingNetwork_Id?      OldRN,
                                                         RoamingNetwork_Id?      NewRN);

        /// <summary>
        /// An event fired whenever a new forwarding information was changed.
        /// </summary>
        public event OnForwardingChangedDelegate OnForwardingChanged;

        #endregion


        #region OnStarted

        public delegate Task OnStartedDelegate(DateTime         Timestamp,
                                               WWCPImporter<T>  Importer,
                                               String           Message);

        public event OnStartedDelegate OnStarted;

        #endregion

        #region OnStartFinished

        public delegate Task OnStartFinishedDelegate(DateTime         Timestamp,
                                                     WWCPImporter<T>  Importer,
                                                     String           Message);

        public event OnStartFinishedDelegate OnStartFinished;

        #endregion

        #region OnLoadForwardingDataFromFileStarted

        public delegate Task OnLoadForwardingDataFromFileStartedDelegate(DateTime         Timestamp,
                                                                         WWCPImporter<T>  Importer,
                                                                         String           FileName);

        public event OnLoadForwardingDataFromFileStartedDelegate OnLoadForwardingDataFromFileStarted;

        #endregion

        #region OnLoadForwardingDataFromFileFinished

        public delegate Task OnLoadForwardingDataFromFileFinishedDelegate(DateTime         Timestamp,
                                                                          WWCPImporter<T>  Importer,
                                                                          String           FileName,
                                                                          UInt64           NumberOfForwardingInfosLoaded);

        public event OnLoadForwardingDataFromFileFinishedDelegate OnLoadForwardingDataFromFileFinished;

        #endregion

        #region OnFinished

        public delegate Task OnFinishedDelegate(DateTime         Timestamp,
                                                WWCPImporter<T>  Importer,
                                                String           Message);

        public event OnFinishedDelegate OnFinished;

        #endregion

        #region OnImportFailed

        public delegate Task OnImportFailedDelegate(DateTime         Timestamp,
                                                    WWCPImporter<T>  Importer,
                                                    String           Category,
                                                    Exception?       Exception         = null,
                                                    String?          Description       = null,
                                                    DateTime?        ExportTimestamp   = null);

        public event OnImportFailedDelegate  OnImportFailed;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP importer.
        /// </summary>
        public WWCPImporter(String                                                           Id,
                            //String?                                                          ForwardingFilenamePrefix            = null,

                            IEnumerable<ChargingStationOperator>?                            AllChargingStationOperators         = null,
                            Func<ChargingStation_Id, IEnumerable<ChargingStationOperator>>?  GetChargingStationOperators         = null,
                            Func<ChargingStation_Id, ChargingStationOperator>?               GetDefaultChargingStationOperator   = null,
                            //CreateForwardingTableDelegate<T>?                                CreateForwardingTable               = null,

                            Action<WWCPImporter<T>, T>?                                      OnStartup                           = null,
                            TimeSpan?                                                        ImportEvery                         = null,
                            GetDataDelegate<T>?                                              GetData                             = null,
                            Action<WWCPImporter<T>, T>?                                      OnEveryRun                          = null,
                            UInt32                                                           MaxNumberOfCachedDataImports        = DefaultMaxNumberOfCachedImports,
                            Action<WWCPImporter<T>, T>?                                      OnShutdown                          = null,

                            DNSClient?                                                       DNSClient                           = null)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id),                        "The given importer identification must not be null or empty!");

            //if (ForwardingFilenamePrefix.IsNullOrEmpty())
            //    throw new ArgumentNullException(nameof(ForwardingFilenamePrefix),  "The given importer config filename prefix must not be null or empty!");

            if (GetData == null)
                throw new ArgumentNullException(nameof(GetData),                   "The given GetData delegate must not be null!");

            if (OnEveryRun == null)
                throw new ArgumentNullException(nameof(OnEveryRun),                "The given OnEveryRun delegate must not be null!");

            #endregion

            this.Id                                 = Id;

            //this.ForwardingFilenamePrefix           = ForwardingFilenamePrefix.IsNotNullOrEmpty()
            //                                              ? ForwardingFilenamePrefix
            //                                              : Id + "_forwardings_";

            this.ImportEvery                        = ImportEvery ?? DefaultImportEvery;

            this.OnStartup                          = OnStartup;
            this.GetData                            = GetData;
            this.OnEveryRun                         = OnEveryRun;
            //this.CreateForwardingTable              = CreateForwardingTable;
            this.MaxNumberOfCachedDataImports       = MaxNumberOfCachedDataImports;
            this.OnShutdown                         = OnShutdown;

            this.AllChargingStationOperators        = AllChargingStationOperators;
            this.GetChargingStationOperators        = GetChargingStationOperators;
            this.GetDefaultChargingStationOperator  = GetDefaultChargingStationOperator;
            //this._AllForwardingInfos                = new Dictionary<ChargingStation_Id, ImporterForwardingInfo>();
            this._ImportedData                      = new List<Timestamped<T>>();

            this.Started                            = false;
            //this.ImporterRunLock                    = new Object();
            this.ImporterRunTimer                   = new Timer(ImporterRun);

            this.DNSClient                          = DNSClient;

        }

        #endregion


        #region (protected) SendImportFailed(Timestamp, Category, Exception, Description = null, ExportTimestamp = null)

        protected void SendImportFailed(DateTime    Timestamp,
                                        String      Category,
                                        Exception?  Exception         = null,
                                        String?     Description       = null,
                                        DateTime?   ExportTimestamp   = null)
        {

            OnImportFailed?.Invoke(Timestamp,
                                   this,
                                   Category,
                                   Exception,
                                   Description,
                                   ExportTimestamp);

        }

        #endregion

        #region SendOnForwardingChanged(Timestamp, ForwardingInfo, OldRN, NewRN)

        //public void SendOnForwardingChanged(DateTime                Timestamp,
        //                                    ImporterForwardingInfo  ForwardingInfo,
        //                                    RoamingNetwork_Id?      OldRN,
        //                                    RoamingNetwork_Id?      NewRN)
        //{

        //    SaveForwardingDataToFile();

        //    OnForwardingChanged?.Invoke(Timestamp, this, ForwardingInfo, OldRN, NewRN);

        //}

        #endregion


        #region (private) LoadForwardingDataFromFile()

        //private async Task<WWCPImporter<T>> LoadForwardingDataFromFile()
        //{

        //    try
        //    {

        //        var CurrentDirectory  = Directory.GetCurrentDirectory();
        //        var ConfigFilename    = Directory.EnumerateFiles(CurrentDirectory).
        //                                            Select           (file => file.Remove(0, CurrentDirectory.Length + 1)).
        //                                            Where            (file => file.StartsWith(ForwardingFilenamePrefix, StringComparison.Ordinal)).
        //                                            OrderByDescending(file => file).
        //                                            FirstOrDefault();

        //        var InputFile         = ConfigFilename.IsNotNullOrEmpty()
        //                                    ? ConfigFilename
        //                                    : ForwardingFilenamePrefix + ".json";

        //        if (File.Exists(InputFile))
        //        {

        //            #region Try to read JSON from file...

        //            JObject JSONConfig;

        //            try
        //            {
        //                JSONConfig = JObject.Parse(File.ReadAllText(InputFile));
        //            }
        //            catch (Exception)
        //            {
        //                throw new ApplicationException("Could not read '" + InputFile + "'!");
        //            }

        //            #endregion

        //            try
        //            {

        //                foreach (var CurrentRoamingNetwork in JSONConfig)
        //                {

        //                    var CurrentRoamingNetworkId  = RoamingNetwork_Id.Parse(CurrentRoamingNetwork.Key);

        //                    //var CurrentEVSEOperator      = ChargingStationOperators.FirstOrDefault(evseoperator => evseoperator.RoamingNetwork.Id == CurrentRoamingNetworkId);

        //                    //if (CurrentEVSEOperator == null)
        //                    //    throw new ApplicationException("Could not find any charging station operator for roaming network '" + CurrentRoamingNetworkId + "'!");

        //                    var CurrentRoamingNetworkJObject = CurrentRoamingNetwork.Value as JObject;

        //                    if (CurrentRoamingNetworkJObject != null)
        //                    {
        //                        foreach (var ChargingStationGroups in CurrentRoamingNetworkJObject)
        //                        {

        //                            switch (ChargingStationGroups.Key.ToLower())
        //                            {

        //                                #region ValidChargingStations

        //                                case "validchargingstations":

        //                                    (ChargingStationGroups.Value as JObject).GetEnumerator().
        //                                        ConsumeAll().
        //                                        OrderBy(KVP => KVP.Key).
        //                                        ForEach(StationConfig => {

        //                                            ChargingStation_Id ChargingStationId;

        //                                            if (ChargingStation_Id.TryParse(StationConfig.Key, out ChargingStationId))
        //                                            {


        //                                                var CurrentEVSEOperator = GetChargingStationOperators(ChargingStationId)?.
        //                                                                                FirstOrDefault(cso => cso                   != null &&
        //                                                                                                    cso.RoamingNetwork.Id == CurrentRoamingNetworkId);

        //                                                if (CurrentEVSEOperator != null)
        //                                                {

        //                                                    JToken JSONToken2;
        //                                                    String PhoneNumber = null;
        //                                                    var CurrentSettings = StationConfig.Value as JObject;

        //                                                    #region PhoneNumber

        //                                                    //if (CurrentSettings.TryGetValue("PhoneNumber", out JSONToken2))
        //                                                    //{
        //                                                    //    PhoneNumber = JSONToken2.Value<String>();
        //                                                    //}

        //                                                    #endregion

        //                                                    #region AdminStatus

        //                                                    var AdminStatus = ChargingStationAdminStatusTypes.Operational;

        //                                                    if (CurrentSettings.TryGetValue("Adminstatus", out JSONToken2) &&
        //                                                        !Enum.TryParse(JSONToken2.Value<String>(), true, out AdminStatus))

        //                                                        DebugX.Log("Invalid admin status '" + JSONToken2.Value<String>() + "' for charging station '" + ChargingStationId + "'!");

        //                                                    #endregion

        //                                                    #region Group

        //                                                    if (CurrentSettings.TryGetValue("Group", out JSONToken2))
        //                                                    {

        //                                                        var JV = JSONToken2 as JValue;
        //                                                        var JA = JSONToken2 as JArray;

        //                                                        var GroupList = JV != null
        //                                                                            ? new String[] { JV.Value<String>() }
        //                                                                            : JA != null
        //                                                                                ? JA.AsEnumerable().Select(v => v.Value<String>())
        //                                                                                : null;

        //                                                        if (GroupList != null)
        //                                                        {
        //                                                            foreach (var GroupId in GroupList)
        //                                                            {
        //                                                                CurrentEVSEOperator.
        //                                                                    GetOrCreateChargingStationGroup(GroupId,
        //                                                                                                    I18NString.Create(Languages.de, GroupId)).
        //                                                                    Add(ChargingStationId);
        //                                                            }
        //                                                        }

        //                                                    }

        //                                                    #endregion

        //                                                    //if (!_AllForwardingInfos.ContainsKey(ChargingStationId))
        //                                                    //{

        //                                                    //    _AllForwardingInfos.Add(ChargingStationId,
        //                                                    //                            new ImporterForwardingInfo(
        //                                                    //                                OnChangedCallback:          SendOnForwardingChanged,
        //                                                    //                                ChargingStationOperators:   GetChargingStationOperators(ChargingStationId),
        //                                                    //                                StationId:                  ChargingStationId,
        //                                                    //                                StationName:                "",
        //                                                    //                                StationServiceTag:          "",
        //                                                    //                                StationAddress:             null,//new Address(),
        //                                                    //                                StationGeoCoordinate:       null,
        //                                                    //                                PhoneNumber:                PhoneNumber,
        //                                                    //                                AdminStatus:                AdminStatus,
        //                                                    //                                Created:                    Timestamp.Now,
        //                                                    //                                OutOfService:               true,
        //                                                    //                                ForwardedToOperator:        CurrentEVSEOperator)
        //                                                    //                            );

        //                                                    //}

        //                                                }

        //                                            }

        //                                        });

        //                                    break;

        //                                #endregion

        //                            }

        //                        }

        //                    }

        //                }

        //            }

        //            catch (Exception e)
        //            {
        //                DebugX.Log("LoadForwardingDataFromFile failed: " + e.Message);
        //            }

        //        }

        //        else
        //            throw new ApplicationException("Config file '" + ForwardingFilenamePrefix + "' does not exist!");


        //        OnLoadForwardingDataFromFileFinished?.Invoke(Timestamp.Now,
        //                                                        this,
        //                                                        InputFile,
        //                                                        (UInt64) _AllForwardingInfos.Count);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    return this;

        //}

        #endregion

        #region (private) SaveForwardingDataToFile()

        //private async Task<WWCPImporter<T>> SaveForwardingDataToFile()
        //{

        //    try
        //    {

        //        var Now              = Timestamp.Now;

        //        var _ConfigFilename  = String.Concat(ForwardingFilenamePrefix,     "_",
        //                                                Now.Year,                  "-",
        //                                                Now.Month. ToString("D2"), "-",
        //                                                Now.Day.   ToString("D2"), "_",
        //                                                Now.Hour.  ToString("D2"), "-",
        //                                                Now.Minute.ToString("D2"), "-",
        //                                                Now.Second.ToString("D2"),
        //                                                ".json");

        //        var JSON = new JObject(

        //                        _AllForwardingInfos.

        //                            GroupBy(kvp     => kvp.Value.ForwardedToRoamingNetworkId,
        //                                    kvp     => kvp.Value).

        //                            Where  (RNGroup => RNGroup.Key.ToString() != "-").
        //                            Select (RNGroup => new JProperty(RNGroup.Key.ToString(),
        //                                                        new JObject(

        //                                                            new JProperty("ValidChargingStations", new JObject(
        //                                                                RNGroup.Select(FwdInfo =>
        //                                                                    new JProperty(FwdInfo.StationId.ToString(), new JObject(
        //                                                                            new JProperty("PhoneNumber", FwdInfo.PhoneNumber),
        //                                                                            new JProperty("AdminStatus", FwdInfo.AdminStatus.Value.ToString())
        //                                                                        ))
        //                                                                )
        //                                                            ))

        //                                                    ))).
        //                            ToArray()
        //                    );



        //        //var JSON = new JObject(
        //        //    RoamingNetworkIds.Select(RNId => new JProperty(RNId.ToString(),
        //        //                                          new JObject(

        //        //                                              new JProperty("ValidChargingStations", new JObject(
        //        //                                                        EVSEOperators.
        //        //                                                            Where(evseoperator => evseoperator.RoamingNetwork.Id == RNId).
        //        //                                                            First().
        //        //                                                            ValidEVSEIds.
        //        //                                                            Select(EVSEId => new JProperty(EVSEId.ToString(), new JObject()))
        //        //                                                  )),

        //        //                                              new JProperty("ValidEVSEIds",   new JObject(
        //        //                                                        EVSEOperators.
        //        //                                                            Where(evseoperator => evseoperator.RoamingNetwork.Id == RNId).
        //        //                                                            First().
        //        //                                                            ValidEVSEIds.
        //        //                                                            Select(EVSEId => new JProperty(EVSEId.ToString(), new JObject()))
        //        //                                                  )),

        //        //                                              new JProperty("InvalidEVSEIds", new JObject(
        //        //                                                        EVSEOperators.
        //        //                                                            Where(evseoperator => evseoperator.RoamingNetwork.Id == RNId).
        //        //                                                            First().
        //        //                                                            InvalidEVSEIds.
        //        //                                                            Select(EVSEId => new JProperty(EVSEId.ToString(), new JObject()))
        //        //                                                  ))

        //        //                                          )))

        //        //);

        //        File.WriteAllText(_ConfigFilename, JSON.ToString(), Encoding.UTF8);

        //    }

        //    catch (Exception e)
        //    {
        //        DebugX.Log("SaveForwardingDataInFile failed: " + e.Message);
        //    }


        //    return this;

        //}

        #endregion


        #region AddOrUpdateForwardingInfos(ForwardingInfos, MarkAllOutOfService = true)

        //public WWCPImporter<T> AddOrUpdateForwardingInfos(IEnumerable<ImporterForwardingInfo>  ForwardingInfos,
        //                                                  Boolean                              MarkAllOutOfService = true)
        //{

        //    //lock (_AllForwardingInfos)
        //    //{

        //        // Mark ForwardingInfos as 'OutOfService', to detect which are no longer within the XML...
        //        if (MarkAllOutOfService)
        //            _AllForwardingInfos.Values.ForEach(FwdInfo => FwdInfo.OutOfService = true);

        //        ImporterForwardingInfo ExistingForwardingInfo;
        //        Boolean ForwardingInformationChanged = false;

        //        foreach (var ForwardingInfo in ForwardingInfos)
        //        {

        //            #region An existing forwarding information was found: Look for changes...

        //            if (_AllForwardingInfos.TryGetValue(ForwardingInfo.StationId, out ExistingForwardingInfo))
        //            {

        //                ForwardingInformationChanged = false;

        //                #region StationNameChanged...

        //                if (ExistingForwardingInfo.StationName != ForwardingInfo.StationName)
        //                {
        //                    ExistingForwardingInfo.StationName  = ForwardingInfo.StationName;
        //                    ForwardingInformationChanged        = true;
        //                }

        //                #endregion

        //                #region StationServiceTag changed...

        //                if (ExistingForwardingInfo.StationServiceTag != ForwardingInfo.StationServiceTag)
        //                {
        //                    ExistingForwardingInfo.StationServiceTag  = ForwardingInfo.StationServiceTag;
        //                    ForwardingInformationChanged              = true;
        //                }

        //                #endregion

        //                #region StationAddress changed...

        //                if (ExistingForwardingInfo.StationAddress != ForwardingInfo.StationAddress)
        //                {
        //                    ExistingForwardingInfo.StationAddress  = ForwardingInfo.StationAddress;
        //                    ForwardingInformationChanged           = true;
        //                }

        //                #endregion

        //                #region StationGeoCoordinate changed...

        //                if (ExistingForwardingInfo.StationGeoCoordinate != ForwardingInfo.StationGeoCoordinate)
        //                {
        //                    ExistingForwardingInfo.StationGeoCoordinate  = ForwardingInfo.StationGeoCoordinate;
        //                    ForwardingInformationChanged                 = true;
        //                }

        //                #endregion

        //                ExistingForwardingInfo.UpdateTimestamp();

        //                if (ForwardingInformationChanged)
        //                    OnForwardingInformationChanged?.Invoke(Timestamp.Now,
        //                                                           this,
        //                                                           ExistingForwardingInfo, //ToDo: Send a clone of the previous forwarding info!
        //                                                           ExistingForwardingInfo);

        //            }

        //            #endregion

        //            #region ...or a new one was created!

        //            else
        //            {

        //                _AllForwardingInfos.Add(ForwardingInfo.StationId, ForwardingInfo);

        //                OnNewForwardingInformation?.Invoke(Timestamp.Now,
        //                                                   this,
        //                                                   ForwardingInfo);

        //            }

        //            #endregion

        //        }

        //    //}

        //    return this;

        //}

        #endregion


        #region Start()

        /// <summary>
        /// Start the WWCP importer.
        /// </summary>
        public async Task<WWCPImporter<T>> Start()
        {

            var success = await ImporterRunSemaphore.WaitAsync(TimeSpan.FromSeconds(10));

            if (success)
            {

                try
                {

                    if (!Started)
                    {

                        #region OnStarted

                        var startTime = Timestamp.Now;

                        LastRunTimestamp = startTime;
                        lastRunId        = 0;

                        OnStarted?.Invoke(startTime,
                                          this,
                                          "Importer started");

                        #endregion

                        //await LoadForwardingDataFromFile();

                        var firstData = await GetData(Timestamp:         Timestamp.Now,
                                                      WWCPImporter:      this,
                                                      LastRuntimestamp:  LastRunTimestamp,
                                                      LastRunId:         lastRunId,
                                                      DNSClient:         DNSClient);

                        if (firstData is not null)
                        {

                            //AddOrUpdateForwardingInfos(CreateForwardingTable(this,
                            //                                                 firstData,
                            //                                                 AllChargingStationOperators,
                            //                                                 GetChargingStationOperators,
                            //                                                 GetDefaultChargingStationOperator));

                            OnStartup (this, firstData);
                            OnEveryRun(this, firstData);

                        }

                        #region OnStartFinished

                        var endTime = Timestamp.Now;

                        OnStartFinished?.Invoke(endTime,
                                                this,
                                                "Importer finished its startup!");

                        #endregion

                        ImporterRunTimer.Change(TimeSpan.FromSeconds(1),
                                                ImportEvery);

                        Started = true;

                        //await SaveForwardingDataToFile();

                    }

                }
                catch (Exception e)
                {
                    DebugX.Log("Starting the WWCP Importer '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }
                finally
                {
                    ImporterRunSemaphore.Release();
                }

            }

            return this;

        }

        #endregion


        #region (private, Timer) ImporterRun(Status)

        private async void ImporterRun(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            var success = await ImporterRunSemaphore.WaitAsync(TimeSpan.FromSeconds(10));

            if (success)
            {

                #region Debug info

                DebugX.LogT("WWCP importer '" + Id + "' run started!");

                var startTime = Timestamp.Now;

                #endregion

                try
                {

                    #region Remove ForwardingInfos older than 7 days...

                    //var Now       = Timestamp.Now;

                    //var ToRemove  = _AllForwardingInfos.
                    //                    Where (ForwardingInfo => ForwardingInfo.Value.LastTimeSeen + TimeSpan.FromDays(7) < Now).
                    //                    Select(ForwardingInfo => ForwardingInfo.Key).
                    //                    ToList();

                    //ToRemove.ForEach(ForwardingInfo => _AllForwardingInfos.Remove(ForwardingInfo));

                    #endregion

                    var data = await GetData(Timestamp:         Timestamp.Now,
                                             WWCPImporter:      this,
                                             LastRuntimestamp:  LastRunTimestamp,
                                             LastRunId:         lastRunId++,
                                             DNSClient:         DNSClient);

                    LastRunTimestamp = startTime;

                    if (data is not null)
                    {

                        //ToDo: Handle XML parser exceptions...
                        //AddOrUpdateForwardingInfos(CreateForwardingTable(this,
                        //                                                 data,
                        //                                                 AllChargingStationOperators,
                        //                                                 GetChargingStationOperators,
                        //                                                 GetDefaultChargingStationOperator));

                        //if (_ImportedData.Count >= MaxNumberOfCachedDataImports)
                        //{
                        //    var succ = _ImportedData.Remove(_ImportedData[0]);
                        //    DebugX.LogT("Importer Count = " + _ImportedData.Count + " " + succ);
                        //}

                        //// Save the imported data for later review...
                        //_ImportedData.Add(new Timestamped<T>(ImporterTask.Result));

                        // Update ForwardingInfos
                        OnEveryRun?.Invoke(this, data);

                        OnFinished?.Invoke(Timestamp.Now,
                                           this,
                                           "WWCP importer '" + Id + "' finished after " + (Timestamp.Now - startTime).TotalSeconds + " seconds!");

                    }

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.LogT("WWCP importer '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    ImporterRunSemaphore.Release();
                }

            }

            else
                DebugX.LogT("WWCP importer '" + Id + "' skipped!");

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("WWCP importer '", Id, "': "); //, _AllForwardingInfos.Count, " forwarding infos");

        #endregion

    }

}
