/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.Networking
{

    public enum TransportTypes
    {

        undefined,

        UDP,
        TCP,
        HTTP,
        TLS,
        HTTPS

    }

    public enum ProtocolTypes
    {

        undefined,

        WWCP,
        OICP,
        OCPI

    }

    public static class Ext
    {

        public static RoamingNetworkInfo Create(this RoamingNetwork    RoamingNetwork,
                                                Tracker_Id             TrackerId,
                                                NetworkServiceNode_Id  NodeId,
                                                DateTime               NotBefore,
                                                DateTime               NotAfter,

                                                Byte                   priority,
                                                Byte                   weight,
                                                IIPAddress             IPAddress,
                                                String                 hostname,
                                                IPPort                 port,
                                                TransportTypes         transport,
                                                String                 URLPrefix,
                                                HTTPContentType        contentType,
                                                ProtocolTypes          protocolType,
                                                IEnumerable<String>    PublicKeys)
        {

            return new RoamingNetworkInfo(TrackerId,
                                          NodeId,
                                          "",
                                          NotBefore,
                                          NotAfter,
                                          RoamingNetwork.Id,

                                          priority,
                                          weight,
                                          IPAddress,
                                          hostname,
                                          port,
                                          transport,
                                          URLPrefix,
                                          contentType,
                                          protocolType,
                                          PublicKeys);

        }

    }

    public class RoamingNetworkInfo : IEquatable <RoamingNetworkInfo>,
                                      IComparable<RoamingNetworkInfo>
    {

        #region Properties

        /// <summary>
        /// The identification of a remote tracker which sent this information.
        /// </summary>
        public Tracker_Id             TrackerId           { get; }


        public NetworkServiceNode_Id  NodeId              { get; }

        /// <summary>
        /// An URI for the roaming network.
        /// </summary>
        public String                 IncomingURL         { get; }

        /// <summary>
        /// When this information becomes valid.
        /// </summary>
        public DateTime               NotBefore           { get; }

        /// <summary>
        /// When this information expires.
        /// </summary>
        public DateTime               NotAfter            { get; }



        /// <summary>
        /// The local roaming network.
        /// </summary>
        public RoamingNetwork         RoamingNetwork      { get; }

        /// <summary>
        /// The identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id      RoamingNetworkId    { get; }



        /// <summary>
        /// An URI for the roaming network.
        /// </summary>
        public String                 AnnouncedURI        { get; }


        public Byte                   priority            { get; }

        public Byte                   weight              { get; }

        public IIPAddress             IPAddress           { get; }

        public String                 hostname            { get; }

        public IPPort                 port                { get; }

        public TransportTypes         transport           { get; }

        public String                 URLPrefix           { get; }

        public HTTPContentType        contentType         { get; }

        public ProtocolTypes          protocolType        { get; }

        public IEnumerable<String>    publicKeys          { get; }

        #endregion

        #region Constructor(s)

        #region RoamingNetworkInfo(..., RoamingNetworkId, ...)

        public RoamingNetworkInfo(Tracker_Id            TrackerId,
                                  NetworkServiceNode_Id               NodeId,
                                  String                IncomingURL,
                                  DateTime              NotBefore,
                                  DateTime              NotAfter,

                                  RoamingNetwork_Id     RoamingNetworkId,
                                  Byte                  priority,
                                  Byte                  weight,
                                  IIPAddress            IPAddress,
                                  String                hostname,
                                  IPPort                port,
                                  TransportTypes        transport,
                                  String                URLPrefix,
                                  HTTPContentType       contentType,
                                  ProtocolTypes         protocolType,
                                  IEnumerable<String>   PublicKeys)
        {

            this.TrackerId         = TrackerId;
            this.NodeId            = NodeId;
            this.IncomingURL       = IncomingURL;
            this.NotBefore         = NotBefore;
            this.NotAfter          = NotAfter;

            this.RoamingNetworkId  = RoamingNetworkId;

            this.priority          = priority;
            this.weight            = weight;
            this.hostname          = hostname;
            this.IPAddress         = IPAddress;
            this.port              = port;
            this.transport         = transport;
            this.URLPrefix         = URLPrefix;
            this.contentType       = contentType;
            this.protocolType      = protocolType;
            this.publicKeys        = PublicKeys;

            this.AnnouncedURI      = String.Concat(transport, "://", hostname, ":", port, URLPrefix, "_", contentType);

        }

        #endregion

        #region RoamingNetworkInfo(..., RoamingNetwork, ...)

        public RoamingNetworkInfo(Tracker_Id            TrackerId,
                                  NetworkServiceNode_Id               NodeId,
                                  String                IncomingURL,
                                  DateTime              NotBefore,
                                  DateTime              NotAfter,

                                  RoamingNetwork        RoamingNetwork,
                                  Byte                  priority,
                                  Byte                  weight,
                                  IIPAddress            IPAddress,
                                  String                hostname,
                                  IPPort                port,
                                  TransportTypes        transport,
                                  String                URLPrefix,
                                  HTTPContentType       contentType,
                                  ProtocolTypes         protocolType,
                                  IEnumerable<String>   PublicKeys)

            : this(TrackerId,
                   NodeId,
                   IncomingURL,
                   NotBefore,
                   NotAfter,

                   RoamingNetwork.Id,
                   priority,
                   weight,
                   IPAddress,
                   hostname,
                   port,
                   transport,
                   URLPrefix,
                   contentType,
                   protocolType,
                   PublicKeys)

        {

            this.RoamingNetwork  = RoamingNetwork;

        }

        #endregion

        #endregion



        #region Operator overloading

        #region Operator == (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RemoteRoamingNetworkInfo1 == null) || ((Object) RemoteRoamingNetworkInfo2 == null))
                return false;

            return RemoteRoamingNetworkInfo1.Equals(RemoteRoamingNetworkInfo2);

        }

        #endregion

        #region Operator != (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {
            return !(RemoteRoamingNetworkInfo1 == RemoteRoamingNetworkInfo2);
        }

        #endregion

        #region Operator <  (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {

            if ((Object) RemoteRoamingNetworkInfo1 == null)
                throw new ArgumentNullException("The given RemoteRoamingNetworkInfo1 must not be null!");

            return RemoteRoamingNetworkInfo1.CompareTo(RemoteRoamingNetworkInfo2) < 0;

        }

        #endregion

        #region Operator <= (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {
            return !(RemoteRoamingNetworkInfo1 > RemoteRoamingNetworkInfo2);
        }

        #endregion

        #region Operator >  (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {

            if ((Object) RemoteRoamingNetworkInfo1 == null)
                throw new ArgumentNullException("The given RemoteRoamingNetworkInfo1 must not be null!");

            return RemoteRoamingNetworkInfo1.CompareTo(RemoteRoamingNetworkInfo2) > 0;

        }

        #endregion

        #region Operator >= (RemoteRoamingNetworkInfo1, RemoteRoamingNetworkInfo2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo1">A RemoteRoamingNetworkInfo.</param>
        /// <param name="RemoteRoamingNetworkInfo2">Another RemoteRoamingNetworkInfo.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (RoamingNetworkInfo RemoteRoamingNetworkInfo1, RoamingNetworkInfo RemoteRoamingNetworkInfo2)
        {
            return !(RemoteRoamingNetworkInfo1 < RemoteRoamingNetworkInfo2);
        }

        #endregion

        #endregion

        #region IComparable<RemoteRoamingNetworkInfo> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            // Check if the given object is an RemoteRoamingNetworkInfo.
            var RemoteRoamingNetworkInfo = Object as RoamingNetworkInfo;
            if ((Object) RemoteRoamingNetworkInfo == null)
                throw new ArgumentException("The given object is not a RemoteRoamingNetworkInfo!");

            return CompareTo(RemoteRoamingNetworkInfo);

        }

        #endregion

        #region CompareTo(RemoteRoamingNetworkInfo)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkInfo RemoteRoamingNetworkInfo)
        {

            if ((Object) RemoteRoamingNetworkInfo == null)
                throw new ArgumentNullException(nameof(RemoteRoamingNetworkInfo), "The given RemoteRoamingNetworkInfo must not be null!");

            var result = RoamingNetworkId.CompareTo(RemoteRoamingNetworkInfo.RoamingNetworkId);

            if (result == 0)
                return String.Compare(AnnouncedURI, RemoteRoamingNetworkInfo.AnnouncedURI, StringComparison.Ordinal);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<RemoteRoamingNetworkInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an RemoteRoamingNetworkInfo.
            var RemoteRoamingNetworkInfo = Object as RoamingNetworkInfo;
            if ((Object) RemoteRoamingNetworkInfo == null)
                return false;

            return this.Equals(RemoteRoamingNetworkInfo);

        }

        #endregion

        #region Equals(RemoteRoamingNetworkInfo)

        /// <summary>
        /// Compares two RemoteRoamingNetworkInfos for equality.
        /// </summary>
        /// <param name="RemoteRoamingNetworkInfo">A RemoteRoamingNetworkInfo to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkInfo RemoteRoamingNetworkInfo)
        {

            if ((Object) RemoteRoamingNetworkInfo == null)
                return false;

            if (!RoamingNetworkId.Equals(RemoteRoamingNetworkInfo.RoamingNetworkId))
                return false;

            return AnnouncedURI.Equals(RemoteRoamingNetworkInfo.AnnouncedURI);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return RoamingNetworkId.GetHashCode() * 17 ^ AnnouncedURI.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{RoamingNetworkId} @ {AnnouncedURI}";

        #endregion


    }

}
