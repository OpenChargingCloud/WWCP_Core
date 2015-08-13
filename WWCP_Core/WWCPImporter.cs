/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Services.DNS;

#endregion

namespace org.GraphDefined.WWCP.Importer
{

    /// <summary>
    /// Import data into the WWCP in-memory datastructures.
    /// </summary>
    /// <typeparam name="T">The type of data which will be processed on every update run.</typeparam>
    public class WWCPImporter<T>
    {

        #region Data

        private                 Boolean                           Started = false;
        private readonly        Object                            UpdateEVSEDataAndStatusLock;
        private readonly        Timer                             UpdateEVSEStatusTimer;

        private readonly        Func<DNSClient, Task<T>>          DownloadXMLData;
        private readonly        Action<WWCPImporter<T>, Task<T>>  OnFirstRun;
        private readonly        Action<WWCPImporter<T>, Task<T>>  OnEveryRun;

        private readonly static TimeSpan                          DefaultImportEvery  = TimeSpan.FromMinutes(1);

        public  const           UInt16                            DefaultMaxNumberOfCachedXMLExports = 100;

        #endregion

        #region Properties

        #region Id

        private readonly String _Id;

        public String Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region ConfigFilenamePrefix

        private readonly String _ConfigFilenamePrefix;

        public String ConfigFilenamePrefix
        {
            get
            {
                return _ConfigFilenamePrefix;
            }
        }

        #endregion

        #region DNSClient

        private readonly DNSClient _DNSClient;

        public DNSClient DNSClient
        {
            get
            {
                return _DNSClient;
            }
        }

        #endregion

        #region EVSEOperators

        private readonly List<EVSEOperator> _EVSEOperators;

        public IEnumerable<EVSEOperator> EVSEOperators
        {
            get
            {
                return _EVSEOperators;
            }
        }

        #endregion

        #region RoamingNetworkIds

        public IEnumerable<RoamingNetwork_Id> RoamingNetworkIds
        {
            get
            {
                return _EVSEOperators.Select(EVSEOperator => EVSEOperator.RoamingNetwork.Id);
            }
        }

        #endregion

        #region AllForwardingInfos

        private readonly Dictionary<ChargingStation_Id, ImporterForwardingInfo> _AllForwardingInfos;

        public IEnumerable<ImporterForwardingInfo> AllForwardingInfos
        {
            get
            {

                return _AllForwardingInfos.
                           OrderBy(AllForwardingInfos => AllForwardingInfos.Key).
                           Select (AllForwardingInfos => AllForwardingInfos.Value);

            }
        }

        #endregion

        #region XMLExports

        private List<Timestamped<T>> _XMLExports;

        public IEnumerable<Timestamped<T>> XMLExports
        {
            get
            {
                return _XMLExports;
            }
        }

        #endregion

        #region MaxNumberOfCachedXMLExports

        private UInt16 _MaxNumberOfCachedXMLExports;

        public UInt16 MaxNumberOfCachedXMLExports
        {
            get
            {
                return _MaxNumberOfCachedXMLExports;
            }
        }

        #endregion

        #region UpdateEvery

        private readonly TimeSpan _UpdateEvery;

        public TimeSpan UpdateEvery
        {
            get
            {
                return _UpdateEvery;
            }
        }

        #endregion

        #region HTTPImportEvents

        private HTTPEventSource _HTTPImportEvents;

        public HTTPEventSource HTTPImportEvents
        {

            get
            {
                return _HTTPImportEvents;
            }

            set
            {
                if (value != null)
                    _HTTPImportEvents = value;
            }

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// A delegate called whenever a new forwarding information was added.
        /// </summary>
        public delegate void OnForwardingInformationAddedDelegate(DateTime Timestamp, WWCPImporter<T> Sender, IEnumerable<ImporterForwardingInfo> ForwardingInfos);

        /// <summary>
        /// An event fired whenever a new forwarding information was added.
        /// </summary>
        public event OnForwardingInformationAddedDelegate OnForwardingInformationAdded;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP importer.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ConfigFilenamePrefix"></param>
        /// <param name="DNSClient"></param>
        /// <param name="UpdateEvery"></param>
        /// <param name="GetXMLData"></param>
        /// <param name="OnFirstRun"></param>
        /// <param name="OnEveryRun"></param>
        /// <param name="MaxNumberOfCachedXMLExports"></param>
        public WWCPImporter(String                            Id,
                            String                            ConfigFilenamePrefix,
                            DNSClient                         DNSClient                    = null,
                            TimeSpan?                         UpdateEvery                  = null,

                            Func<DNSClient, Task<T>>          GetXMLData                   = null,
                            Action<WWCPImporter<T>, Task<T>>  OnFirstRun                   = null,
                            Action<WWCPImporter<T>, Task<T>>  OnEveryRun                   = null,

                            UInt16                            MaxNumberOfCachedXMLExports  = DefaultMaxNumberOfCachedXMLExports)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException("ImporterId", "The given config file name must not be null or empty!");

            if (ConfigFilenamePrefix.IsNullOrEmpty())
                throw new ArgumentNullException("ConfigFilenamePrefix", "The given config file name must not be null or empty!");

            if (GetXMLData == null)
                throw new ArgumentNullException("GetXMLData", "The given delegate must not be null or empty!");

            if (OnFirstRun == null)
                throw new ArgumentNullException("OnFirstRun", "The given delegate must not be null or empty!");

            if (OnEveryRun == null)
                throw new ArgumentNullException("OnEveryRun", "The given delegate must not be null or empty!");

            #endregion

            this._Id                           = Id;
            this._ConfigFilenamePrefix         = ConfigFilenamePrefix;
            this._DNSClient                    = DNSClient   != null ? DNSClient         : new DNSClient();
            this._UpdateEvery                  = UpdateEvery != null ? UpdateEvery.Value : DefaultImportEvery;

            this.DownloadXMLData               = GetXMLData;
            this.OnFirstRun                    = OnFirstRun;
            this.OnEveryRun                    = OnEveryRun;

            this._MaxNumberOfCachedXMLExports  = MaxNumberOfCachedXMLExports;

            this._EVSEOperators                = new List<EVSEOperator>();
            this._AllForwardingInfos           = new Dictionary<ChargingStation_Id, ImporterForwardingInfo>();
            this._XMLExports                   = new List<Timestamped<T>>();

            // Start not now but veeeeery later!
            UpdateEVSEDataAndStatusLock        = new Object();
            UpdateEVSEStatusTimer              = new Timer(ImportEvery, null, TimeSpan.FromDays(30), _UpdateEvery);

        }

        #endregion


        #region RegisterEVSEOperator(EVSEOperator)

        public WWCPImporter<T> RegisterEVSEOperator(EVSEOperator EVSEOperator)
        {

            _EVSEOperators.Add(EVSEOperator);

            return this;

        }

        #endregion

        #region LoadForwardingDataFromFile()

        public WWCPImporter<T> LoadForwardingDataFromFile()
        {

            lock (_EVSEOperators)
            {

                if (File.Exists(_ConfigFilenamePrefix + ".json"))
                {

                    #region Read JSON from file...

                    JObject JSONConfig;

                    try
                    {
                        JSONConfig = JObject.Parse(File.ReadAllText(_ConfigFilenamePrefix + ".json"));
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException("Could not load '" + _ConfigFilenamePrefix + "'!");
                    }

                    #endregion

                    try
                    {

                        foreach (var CurrentRoamingNetwork in JSONConfig)
                        {

                            var CurrentRoamingNetworkId  = RoamingNetwork_Id.Parse(CurrentRoamingNetwork.Key);

                            var CurrentOperator          = _EVSEOperators.
                                                               Where(evseoperator => evseoperator.RoamingNetwork.Id == CurrentRoamingNetworkId).
                                                               FirstOrDefault();

                            if (CurrentOperator == null)
                                throw new ApplicationException("Could not find any EVSE operator for roaming network '" + CurrentRoamingNetworkId + "'!");

                            if (CurrentRoamingNetwork.Value as JObject != null)
                            {
                                foreach (var EVSELists in CurrentRoamingNetwork.Value as JObject)
                                {

                                    switch (EVSELists.Key.ToLower())
                                    {

                                        case "invalidevseids":

                                            //(EVSELists.Value as JObject).GetEnumerator().
                                            //    ConsumeAll().
                                            //    OrderBy(KVP => KVP.Key).
                                            //    ForEach(EVSEIdConfig => CurrentOperator.InvalidEVSEIds.Add(EVSE_Id.Parse(EVSEIdConfig.Key)));

                                            break;


                                        case "validevseids":

                                            (EVSELists.Value as JObject).GetEnumerator().
                                                ConsumeAll().
                                                OrderBy(KVP => KVP.Key).
                                                ForEach(EVSEIdConfig => {

                                                    var CurrentSettings  = EVSEIdConfig.Value as JObject;

                                                    JToken JSONToken2;
                                                    if (CurrentSettings.TryGetValue("Redirect", out JSONToken2))
                                                    {
                                                        var JS = JSONToken2 as JValue;
                                                        var JP = JSONToken2 as JProperty;
                                                        var JO = JSONToken2 as JObject;
                                                    }

                                                    var EVSEId             = EVSE_Id.Parse(EVSEIdConfig.Key);
                                                    var ChargingStationId  = ChargingStation_Id.Create(EVSEId);

                    //                                CurrentOperator.ValidEVSEIds.Add(EVSEId);

                                                    if (!_AllForwardingInfos.ContainsKey(ChargingStationId))
                                                    {

                                                        _AllForwardingInfos.Add(ChargingStationId, new ImporterForwardingInfo(
                                                                                                       EVSEOperators:           _EVSEOperators,
                                                                                                       EVSEIds:                 new EVSE_Id[] { EVSEId },
                                                                                                       StationId:               ChargingStationId,
                                                                                                       StationName:             "",
                                                                                                       StationServiceTag:       "",
                                                                                                       StationAddress:          new Address(),
                                                                                                       StationGeoCoordinate:    null,
                                                                                                       Created:                 DateTime.Now,
                                                                                                       OutOfService:            true,
                                                                                                       ForwardedToEVSEOperator: CurrentOperator));

                                                 //       _AllForwardingInfos[ChargingStationId].ForwardedToEVSEOperator = CurrentOperator; //CheckForwarding(_AllForwardingInfos[ChargingStationId]);

                                                    }

                                                    else
                                                        _AllForwardingInfos[ChargingStationId].EVSEIds.Add(EVSEId);

                                                });

                                            break;

                                    }

                                }

                            }

                        }

                    }

                    catch (Exception e)
                    {
                        DebugX.Log("LoadForwardingDataFromFile failed: " + e.Message);
                    }

                    //BelectricDriveOperator_QA1.ValidEVSEIds.AddRange(new List<EVSE_Id>());

                }

                else
                    throw new ApplicationException("Config file '" + _ConfigFilenamePrefix + "' does not exist!");

            }

            return this;

        }

        #endregion

        #region SaveForwardingDataInFile()

        public WWCPImporter<T> SaveForwardingDataInFile()
        {

            lock (_EVSEOperators)
            {

                var _ConfigFilename = _ConfigFilenamePrefix + "-" + DateTime.Now.ToUnixTimestamp() + ".json";

                try
                {

                    var JSON = new JObject(
                        RoamingNetworkIds.Select(RNId => new JProperty(RNId.ToString(),
                                                              new JObject(

                                                                  new JProperty("ValidEVSEIds",   new JObject(
                                                                            EVSEOperators.
                                                                                Where(evseoperator => evseoperator.RoamingNetwork.Id == RNId).
                                                                                First().
                                                                                ValidEVSEIds.
                                                                                Select(EVSEId => new JProperty(EVSEId.ToString(), new JObject()))
                                                                      )),

                                                                  new JProperty("InvalidEVSEIds", new JObject(
                                                                            EVSEOperators.
                                                                                Where(evseoperator => evseoperator.RoamingNetwork.Id == RNId).
                                                                                First().
                                                                                InvalidEVSEIds.
                                                                                Select(EVSEId => new JProperty(EVSEId.ToString(), new JObject()))
                                                                      )))))

                        );

                    File.WriteAllText(_ConfigFilename, JSON.ToString(), Encoding.UTF8);

                }

                catch (Exception e)
                {
                    DebugX.Log("SaveForwardingDataInFile failed: " + e.Message);
                }

            }

            return this;

        }

        #endregion


        #region AddOrUpdateForwardingInfos(ForwardingInfos)

        public WWCPImporter<T> AddOrUpdateForwardingInfos(IEnumerable<ImporterForwardingInfo> ForwardingInfos)
        {

            lock (_AllForwardingInfos)
            {

                var NewForwardingInfos = new List<ImporterForwardingInfo>();

                ForwardingInfos.ForEach(ForwardingInfo => {

                    #region An existing forwarding information was found... search for changes...

                    ImporterForwardingInfo ExistingForwardingInfo;

                    if (_AllForwardingInfos.TryGetValue(ForwardingInfo.StationId, out ExistingForwardingInfo))
                    {

                        #region StationNameChanged...

                        if (ExistingForwardingInfo.StationName          != ForwardingInfo.StationName)
                        {

                            _HTTPImportEvents.SubmitSubEvent("StationNameChanged",
                                                             new JObject(
                                                                 new JProperty("Timestamp",          DateTime.Now.ToIso8601()),
                                                                 new JProperty("ChargingStationId",  ExistingForwardingInfo.StationId.ToString()),
                                                                 new JProperty("OldValue",           ExistingForwardingInfo.StationName),
                                                                 new JProperty("NewValue",           ForwardingInfo. StationName)
                                                             ).ToString().
                                                               Replace(Environment.NewLine, ""));

                            ExistingForwardingInfo.StationName = ForwardingInfo.StationName;

                        }

                        #endregion

                        #region StationServiceTag changed...

                        if (ExistingForwardingInfo.StationServiceTag    != ForwardingInfo.StationServiceTag)
                        {

                            _HTTPImportEvents.SubmitSubEvent("StationServiceTagChanged",
                                                             new JObject(
                                                                 new JProperty("Timestamp",          DateTime.Now.ToIso8601()),
                                                                 new JProperty("ChargingStationId",  ExistingForwardingInfo.StationId.ToString()),
                                                                 new JProperty("OldValue",           ExistingForwardingInfo.StationServiceTag),
                                                                 new JProperty("NewValue",           ForwardingInfo. StationServiceTag)
                                                             ).ToString().
                                                               Replace(Environment.NewLine, ""));

                            ExistingForwardingInfo.StationServiceTag = ForwardingInfo.StationServiceTag;

                        }

                        #endregion

                        #region StationAddress changed...

                        if (ExistingForwardingInfo.StationAddress       != ForwardingInfo.StationAddress)
                        {

                            _HTTPImportEvents.SubmitSubEvent("StationAddressChanged",
                                                             new JObject(
                                                                 new JProperty("Timestamp",          DateTime.Now.ToIso8601()),
                                                                 new JProperty("ChargingStationId",  ExistingForwardingInfo.StationId.ToString()),
                                                                 new JProperty("OldValue",           ExistingForwardingInfo.StationAddress.ToString()),
                                                                 new JProperty("NewValue",           ForwardingInfo. StationAddress.ToString())
                                                             ).ToString().
                                                               Replace(Environment.NewLine, ""));

                            ExistingForwardingInfo.StationAddress = ForwardingInfo.StationAddress;

                        }

                        #endregion

                        #region StationGeoCoordinate changed...

                        if (ExistingForwardingInfo.StationGeoCoordinate != ForwardingInfo.StationGeoCoordinate)
                        {

                            _HTTPImportEvents.SubmitSubEvent("StationGeoCoordinateChanged",
                                                             new JObject(
                                                                 new JProperty("Timestamp",          DateTime.Now.ToIso8601()),
                                                                 new JProperty("ChargingStationId",  ExistingForwardingInfo.StationId.ToString()),
                                                                 new JProperty("OldValue",           ExistingForwardingInfo.StationGeoCoordinate.ToString()),
                                                                 new JProperty("NewValue",           ForwardingInfo. StationGeoCoordinate.ToString())
                                                             ).ToString().
                                                               Replace(Environment.NewLine, ""));

                            ExistingForwardingInfo.StationGeoCoordinate = ForwardingInfo.StationGeoCoordinate;

                        }

                        #endregion

                        ExistingForwardingInfo.UpdateTimestamp();

                    }

                    #endregion

                    #region ...or a new one was created.

                    else
                    {

                        NewForwardingInfos.Add(ForwardingInfo);

                        _AllForwardingInfos.Add(ForwardingInfo.StationId, ForwardingInfo);

                    }

                    #endregion

                });

                if (NewForwardingInfos.Any())
                {

                    //ToDo: Implement me!
                    _HTTPImportEvents.SubmitSubEvent("NewForwardingInfos",
                                                     new JObject(
                                                         new JProperty("Timestamp",          DateTime.Now.ToIso8601()),
                                                         new JProperty("ForwardingInfos",    new JArray())
                                                     ).ToString().
                                                       Replace(Environment.NewLine, ""));

                    var OnForwardingInformationAddedLocal = OnForwardingInformationAdded;
                    if (OnForwardingInformationAddedLocal != null)
                        OnForwardingInformationAddedLocal(DateTime.Now, this, NewForwardingInfos);

                }

            }

            return this;

        }

        #endregion

        #region Start()

        /// <summary>
        /// Start the WWCP importer.
        /// </summary>
        public WWCPImporter<T> Start()
        {

            if (Monitor.TryEnter(UpdateEVSEDataAndStatusLock))
            {

                try
                {

                    if (!Started)
                    {

                        OnFirstRun(this, DownloadXMLData(_DNSClient));

                        DebugX.Log("WWCP importer '" + Id + "' Initital import finished!");

                        UpdateEVSEStatusTimer.Change(TimeSpan.FromSeconds(1), UpdateEvery);

                        Started = true;

                    }

                }
                catch (Exception e)
                {
                    DebugX.Log("Starting the WWCP Importer '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEDataAndStatusLock);
                }

            }

            SaveForwardingDataInFile();
            return this;

        }

        #endregion


        #region (private, Timer) ImportEvery(Status)

        private void ImportEvery(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(UpdateEVSEDataAndStatusLock))
            {

                #region Debug info

                #if DEBUG

                DebugX.LogT("WWCP importer '" + Id + "' started!");

                var StopWatch = new Stopwatch();
                StopWatch.Start();

                #endif

                #endregion

                try
                {

                    #region Remove ForwardingInfos older than 7 days...

                    //var Now       = DateTime.Now;

                    //var ToRemove  = _AllForwardingInfos.
                    //                    Where (ForwardingInfo => ForwardingInfo.Value.LastTimeSeen + TimeSpan.FromDays(7) < Now).
                    //                    Select(ForwardingInfo => ForwardingInfo.Key).
                    //                    ToList();

                    //ToRemove.ForEach(ForwardingInfo => _AllForwardingInfos.Remove(ForwardingInfo));

                    #endregion

                    DownloadXMLData(_DNSClient).
                        ContinueWith(XMLTask => {

                        // Save the XML Export for later review...
                        _XMLExports.Add(new Timestamped<T>(XMLTask.Result));

                        if (_XMLExports.Count > _MaxNumberOfCachedXMLExports)
                            _XMLExports.Remove(_XMLExports.First());

                        // Mark ForwardingInfos as 'OutOfService', to detect which are no longer within the XML...
                        lock (_AllForwardingInfos)
                        {
                            _AllForwardingInfos.Values.ForEach(FwdInfo => FwdInfo.OutOfService = true);
                        }

                        // Update ForwardingInfos
                        var OnEveryRunLocal = OnEveryRun;
                        if (OnEveryRunLocal != null)
                            OnEveryRunLocal(this, XMLTask);

                    }).
                    Wait();

                    #region Debug info

                    #if DEBUG

                        StopWatch.Stop();

                        DebugX.LogT("WWCP importer '" + Id + "' finished after " + StopWatch.Elapsed.TotalSeconds + " seconds!");

                    #endif

                    #endregion

                }
                catch (Exception e)
                {
                    DebugX.LogT("WWCP importer '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(UpdateEVSEDataAndStatusLock);
                }

            }

            else
                DebugX.LogT("WWCP importer '" + Id + "' skipped!");

        }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat("WWCP importer '", Id, "': ", _AllForwardingInfos.Count, " forwarding infos");
        }

        #endregion

    }

}
