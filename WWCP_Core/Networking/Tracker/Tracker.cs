/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
 * This file is part of WWCP Tracker <https://github.com/OpenChargingCloud/WWCP_Tracker>
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
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json.Linq;
using org.GraphDefined.WWCP.Net;

#endregion

namespace org.GraphDefined.WWCP.Networking
{

    //public static class WWCPTrackerExtentions
    //{

    //    public static Tracker AttachTracker(this WWCP_HTTPAPI     HTTPAPI,
    //                                        Tracker_Id            Id,
    //                                        I18NString            Description  = null,
    //                                        HTTPPath?              URIPrefix    = null)
    //    {

    //        return new Tracker(Id,
    //                           Description,
    //                           HTTPAPI?.HTTPServer,
    //                           URIPrefix ?? Tracker.DefaultURIPrefix);

    //    }

    //}


    /// <summary>
    /// Create a new WWCP tracker client.
    /// </summary>
    public class Tracker
    {

        #region Data

        /// <summary>
        /// The default URI prefix of the WWCP tracker API.
        /// </summary>
        public  static readonly HTTPPath DefaultURIPrefix       = HTTPPath.Parse("/tracker");

        private ConcurrentDictionary<RoamingNetwork_Id, List<RoamingNetworkInfo>> _LocalRoamingNetworks;

        private Dictionary<String, String> _Logins;

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this tracker client.
        /// </summary>
        public Tracker_Id                                   Id              { get; }

        /// <summary>
        /// An optional description of this tracker client.
        /// </summary>
        public I18NString                                   Description     { get; }


        /// <summary>
        /// The HTTP server of the WWCP tracker.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer      { get; }

        /// <summary>
        /// The common URI prefix of the HTTP server of the WWCP tracker.
        /// </summary>
        public HTTPPath                                      URIPrefix       { get; }

        /// <summary>
        /// The DNS client used by the tracker.
        /// </summary>
        public DNSClient                                    DNSClient       { get; }

        #endregion

        #region Constructor(s)

        public Tracker(Tracker_Id                                   Id,
                       I18NString                                   Description,
                       HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                       HTTPPath?                                     URIPrefix = null)
        {

            #region Initial checks

            if (Id == null || Id.ToString().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id),          "The given tracker client identification must not be null or empty!");

            if (HTTPServer == null)
                throw new ArgumentNullException(nameof(HTTPServer),  "The given HTTP server must not be null!");

            #endregion

            this.Id                     = Id;
            this.Description            = Description ?? I18NString.Empty;
            this.HTTPServer             = HTTPServer;
            this.URIPrefix              = URIPrefix   ?? DefaultURIPrefix;
            this._Logins                = new Dictionary<String, String>();
            this._LocalRoamingNetworks  = new ConcurrentDictionary<RoamingNetwork_Id, List<RoamingNetworkInfo>>();

            RegisterURITemplates();

        }

        #endregion

        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region GET  /

            // -----------------------------------------------------------------------
            // curl -v -X GET -H "Accept: application/json" \
            //      http://127.0.0.1:3004/tracker
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URIPrefix,
                                         HTTPContentType.JSON_UTF8,
                                         HTTPDelegate: Request => {

                                             var Now   = DateTime.UtcNow;
                                             var JSON  = _LocalRoamingNetworks.
                                                             Select (kvp => {

                                                                var LocalRoamingNetwork = kvp.Value.First(rni => rni != null)?.RoamingNetwork;

                                                                return JSONObject.Create(new JProperty("id",                kvp.Key.  ToString()),
                                                                                         LocalRoamingNetwork != null
                                                                                            ? new JProperty("description",  LocalRoamingNetwork?.Description?.ToJSON())
                                                                                            : null,
                                                                                         new JProperty("rendezvousPoints",  JSONArray.Create(
                                                                                             kvp.Value.
                                                                                                 Select(rni => JSONObject.Create(
                                                                                                                   new JProperty("priority",      rni.priority),
                                                                                                                   new JProperty("weight",        rni.weight),
                                                                                                                   new JProperty("hostname",      rni.hostname),
                                                                                                                   new JProperty("port",          rni.port.        ToString()),
                                                                                                                   new JProperty("transport",     rni.transport.   ToString()),
                                                                                                                   new JProperty("uriPrefix",     rni.uriPrefix),
                                                                                                                   new JProperty("contentType",   rni.contentType. ToString()),
                                                                                                                   new JProperty("protocol",      rni.protocolType.ToString()),
                                                                                                                   new JProperty("expiresAfter",  rni.ExpiredAfter.ToIso8601()),
                                                                                                                   new JProperty("publicKeys",    new JArray()),
                                                                                                                   new JProperty("signature",     "...")
                                                                                                               )))),
                                                                                         LocalRoamingNetwork != null
                                                                                             ? new JProperty("statistics",        JSONObject.Create(
                                                                                                   new JProperty("EVSEOperators",                 LocalRoamingNetwork?.ChargingStationOperators.Count()),
                                                                                                   new JProperty("EVServiceProviders",            LocalRoamingNetwork?.eMobilityProviders.Count()),
                                                                                                   new JProperty("EMPRoamingProviders",           LocalRoamingNetwork?.EMPRoamingProviders.Count()),
                                                                                                   new JProperty("EVSEOperatorRoamingProviders",  LocalRoamingNetwork?.ChargingStationOperatorRoamingProviders.Count()),
                                                                                                   new JProperty("chargingPools",                 LocalRoamingNetwork?.ChargingPools.   Count()),
                                                                                                   new JProperty("chargingStations",              LocalRoamingNetwork?.ChargingStations.Count()),
                                                                                                   new JProperty("EVSEs",                         LocalRoamingNetwork?.EVSEs.           Count())
                                                                                               ))
                                                                                             : null
                                                                                        );
                                                             });


                                             return Task.FromResult<HTTPResponse>(
                                                 new HTTPResponse.Builder(Request) {
                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     Server          = HTTPServer.DefaultServerName,
                                                     Date            = DateTime.Now,
                                                     ContentType     = HTTPContentType.JSON_UTF8,
                                                     Content         = JSONObject.Create(
                                                                           new JProperty("id",               Id.ToString()),
                                                                           new JProperty("description",      Description.ToJSON()),
                                                                           new JProperty("timestamp",        Now.ToIso8601()),
                                                                           new JProperty("roamingNetworks",  new JArray(JSON)),
                                                                           new JProperty("publicKeys",       new JArray()),
                                                                           new JProperty("signature",        "...")
                                                                       ).ToUTF8Bytes(),
                                                     Connection      = "close"
                                                 });

                                         });

            #endregion

            #region ANNOUNCE  /

            // -----------------------------------------------------------------------
            // curl -v -X NOTIFY -H "Content-Type: application/json" \
            //                   -H "Accept:       application/json" \
            //      -d "{ \"StatusList\":  { \
            //              \"2014-10-13T22:14:01.862Z\": \"OutOfService\", \
            //              \"2014-10-13T21:32:15.386Z\": \"Charging\"  \
            //          }" \
            //      http://127.0.0.1:3004/ext/BoschEBike/EVSEs/49*822*483*1/NotClosed
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.ANNOUNCE,
                                         URIPrefix,
                                         HTTPContentType.JSON_UTF8,
                                         HTTPDelegate: Request => {

                                             //SendEVSEDoorNotClosedNotifyLog(Request);

                                             #region Check HTTP Basic Authentication

                                             if (Request.Authorization          == null               ||
                                                 !_Logins.ContainsKey(Request.Authorization.Username) ||
                                                  _Logins[Request.Authorization.Username] != Request.Authorization.Password)
                                             {

                                                 return Task.FromResult<HTTPResponse>(
                                                  //SendEVSEDoorNotClosedNotifiedLog(
                                                     new HTTPResponse.Builder(Request) {
                                                         HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                                                         WWWAuthenticate  = @"Basic realm=""WWCP tracker""",
                                                         Server           = HTTPServer.DefaultServerName,
                                                         Date             = DateTime.Now,
                                                         Connection       = "close"
                                                     });

                                             }

                                             #endregion

                                             //#region Check EVSEId parameter

                                             //HTTPResponse _HTTPResponse;
                                             //EVSE_Id       EVSEId;

                                             //if (!Request.ParseEVSEId(HTTPServer.DefaultServerName,
                                             //                         out EVSEId,
                                             //                         out _HTTPResponse))
                                             //    return SendEVSEDoorNotClosedNotifiedLog(_HTTPResponse);

                                             //#endregion

                                             //#region Parse JSON

                                             //JSONWrapper JSON       = null;
                                             //Object                      JSONToken  = null;

                                             //if (!Request.TryParseJObjectRequestBody(out JSON, out _HTTPResponse))
                                             //    return SendEVSEDoorNotClosedNotifiedLog(_HTTPResponse);

                                             //// {
                                             ////   "context" : "NO CONTEXT",
                                             ////   "EventId" : "bd27b678-b3af-42b1-8dff-0af1cf3564ed",
                                             ////   "Timestamp" : "2016-02-14T10:17:13GMT",
                                             ////   "Event" : {
                                             ////     "code" : 600,
                                             ////     "eventName" : "EVSE Not Closed"
                                             ////   },
                                             ////   "EVSEId" : "49*822*483*1"
                                             //// }

                                             //#region Parse EventId    [mandatory]

                                             //String EventId = null;

                                             //if (!JSON.ParseMandatory("EventId",
                                             //                         "Unique event identification",
                                             //                         HTTPServer.DefaultServerName,
                                             //                         out EventId,
                                             //                         Request,
                                             //                         out _HTTPResponse))

                                             //    return SendEVSEDoorNotClosedNotifiedLog(_HTTPResponse);

                                             //#endregion

                                             //#region Parse Timestamp  [mandatory]

                                             //DateTime EventTimestamp;

                                             //if (!JSON.ParseMandatory(Enumeration.Create("Timestamp", "TimeStamp"),
                                             //                         "The time stamp of the event",
                                             //                         HTTPServer.DefaultServerName,
                                             //                         out EventTimestamp,
                                             //                         Request,
                                             //                         out _HTTPResponse))

                                             //    return SendEVSEDoorNotClosedNotifiedLog(_HTTPResponse);

                                             //#endregion

                                             //#endregion


                                             //var Task = SendEVSEDoorNotClosedNotification(Request.Timestamp,
                                             //                                             Request.CancellationToken,
                                             //                                             Request.EventTrackingId,
                                             //                                             MapIncomingEVSEIds != null ? MapIncomingEVSEIds(EVSEId) : EVSEId,
                                             //                                             EventId,
                                             //                                             EventTimestamp);

                                             //Task.Wait(TimeSpan.FromSeconds(60));


                                             return Task.FromResult<HTTPResponse>(// SendEVSEDoorNotClosedNotifiedLog(
                                                 new HTTPResponse.Builder(Request) {
                                                     HTTPStatusCode  = HTTPStatusCode.OK,
                                                     Server          = HTTPServer.DefaultServerName,
                                                     Date            = DateTime.Now,
                                                     Connection      = "close"
                                                 });

                                         });

            #endregion

        }

        #endregion


        #region Add/Remove roaming network

        #region AddRoamingNetworkInfos(params RoamingNetworkInfos)

        public Tracker AddRoamingNetworkInfos(params RoamingNetworkInfo[] RoamingNetworkInfos)
        {

            foreach (var roamingnetworkinfo in RoamingNetworkInfos)
            {

                List<RoamingNetworkInfo> RoamingNetworkList;

                if (_LocalRoamingNetworks.TryGetValue(roamingnetworkinfo.RoamingNetworkId, out RoamingNetworkList))
                {
                    lock (RoamingNetworkList)
                    {
                        RoamingNetworkList.Add(roamingnetworkinfo);
                    }
                }

                else
                    _LocalRoamingNetworks.TryAdd(roamingnetworkinfo.RoamingNetworkId,
                                                 new List<RoamingNetworkInfo> { roamingnetworkinfo });

            }

            //RoamingNetwork _RoamingNetwork = null;

                //// Automagically add/remove roaming networks which will be added/removed later...
                //RoamingNetworks.OnRoamingNetworkAddition.OnNotification += (roamingnetworks, roamingnetwork) => this._LocalRoamingNetworks.TryAdd   (roamingnetwork.Id, roamingnetwork);
                //RoamingNetworks.OnRoamingNetworkRemoval. OnNotification += (roamingnetworks, roamingnetwork) => this._LocalRoamingNetworks.TryRemove(roamingnetwork.Id, out _RoamingNetwork);

                //foreach (var __RoamingNetwork in RoamingNetworks)
                //    if (!_LocalRoamingNetworks.TryAdd(__RoamingNetwork.Id, __RoamingNetwork))
                //        throw new ArgumentException("Roaming network '" + __RoamingNetwork.Id + "' could not be added!");

            return this;

        }

        #endregion


        #region RemoveRoamingNetwork(RoamingNetwork)

        public Tracker RemoveRoamingNetwork(RoamingNetwork RoamingNetwork)
        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            #endregion

            //RoamingNetwork _RoamingNetwork = null;

            //if (!_LocalRoamingNetworks.TryRemove(RoamingNetwork.Id, out _RoamingNetwork))
            //    throw new ArgumentException("The given roaming network could not be removed!");

            return this;

        }

        #endregion

        #region RemoveRoamingNetwork(RoamingNetworkId)

        public Tracker RemoveRoamingNetwork(RoamingNetwork_Id RoamingNetworkId)
        {

            #region Initial checks

            if (RoamingNetworkId == null)
                throw new ArgumentNullException(nameof(RoamingNetworkId), "The given roaming network identification must not be null!");

            #endregion

            //RoamingNetwork _RoamingNetwork = null;

            //if (!_LocalRoamingNetworks.TryRemove(RoamingNetworkId, out _RoamingNetwork))
            //    throw new ArgumentException("The given roaming network could not be removed!");

            return this;

        }

        #endregion

        #endregion


    }

}
