/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Mockup <http://www.github.com/eMI3/Mockup>
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using org.GraphDefined.eMI3;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.eMI3.IO.XML
{

    public static class XMLExport
    {

        public static readonly XNamespace NS_eMI3             = "http://eMI3.de/XML";
        public static readonly XNamespace NS_EVSPool          = "http://eMI3.de/XML/EVSPool";
        public static readonly XNamespace NS_ChargingStation  = "http://eMI3.de/XML/ChargingStation";
        public static readonly XNamespace NS_EVSE             = "http://eMI3.de/XML/EVSE";
        public static readonly XNamespace NS_SocketOutlet     = "http://eMI3.de/XML/SocketOutlet";


        public static readonly String eMI3_Root     = "eMI3";
        public static readonly String eMI3_Version  = "version";



        public static XDocument Generate(XElement Element)
        {

            var XMLRoot = new XElement(NS_eMI3 + eMI3_Root,
                              new XAttribute(NS_eMI3 + eMI3_Version, "0.1"),
                              new XAttribute(XNamespace.Xmlns + "eMI3",            NS_eMI3.NamespaceName),
                              new XAttribute(XNamespace.Xmlns + "EVSPool",          NS_EVSPool.NamespaceName),
                              new XAttribute(XNamespace.Xmlns + "ChargingStation",  NS_ChargingStation.NamespaceName),
                              new XAttribute(XNamespace.Xmlns + "EVSE",             NS_EVSE.NamespaceName),
                              new XAttribute(XNamespace.Xmlns + "SocketOutlet",     NS_SocketOutlet.NamespaceName),
                              Element);

            return new XDocument(
                new XDeclaration("1.0", "utf-8", null), XMLRoot);

        }


        #region ToXML(this Object, Namespace, ElementName)

        public static XElement ToXML(this Object Object, XNamespace Namespace, String ElementName)
        {
            return new XElement(Namespace + ElementName, Object.ToString());
        }

        #endregion

        #region ToXML(this DateTime, Namespace, ElementName)

        public static XElement ToXML(this DateTime DateTime, XNamespace Namespace, String ElementName, String Format = "yyyyMMdd HHmmss")
        {
            return new XElement(Namespace + ElementName, DateTime.ToUniversalTime().ToString(Format));
        }

        #endregion

        #region ToXML(this I8N, Namespace, ElementName)

        public static readonly String I8N_Root      = "I8N";
        public static readonly String I8N_Language  = "lang";

        public static XElement ToXML(this I8NString I8N, XNamespace Namespace, String ElementName)
        {

            return new XElement(Namespace + ElementName,
                                I8N.Select(v => new XElement(NS_eMI3 + I8N_Root,
                                     new XAttribute(NS_eMI3 + I8N_Language, v.Language),
                                     v.Value)));

        }

        #endregion

        #region ToXML(this Location, Namespace, ElementName)

        public static readonly String Geo_Model     = "model";
        public static readonly String Geo_Latitude  = "latitude";
        public static readonly String Geo_Longitude = "longitude";
        public static readonly String Geo_Altitude  = "altitude";

        public static XElement ToXML(this GeoCoordinate Location, XNamespace Namespace, String ElementName)
        {

            if (Location.Longitude.Value == 0 && Location.Latitude.Value == 0)
                return null;

            return new XElement(Namespace + ElementName,
                       (Location.Projection != GravitationalModel.WGS84) ? new XAttribute(NS_eMI3 + Geo_Model,    Location.Projection)    : null,
                       new XElement(NS_eMI3 + Geo_Latitude,  Location.Latitude),
                       new XElement(NS_eMI3 + Geo_Longitude, Location.Longitude),
                       (Location.Altitude.Value != 0.0)                  ? new XElement  (NS_eMI3 + Geo_Altitude, Location.Altitude) : null
                   );

        }

        #endregion


        #region ToXML(this Pool)

        public static readonly String NS_EVSPool_Enumeration       = "Enumeration";
        public static readonly String NS_EVSPool_Root              = "Instance";
        public static readonly String NS_EVSPool_Id                = "Id";
        public static readonly String NS_EVSPool_Timestamp         = "Timestamp";
        public static readonly String NS_EVSPool_Name              = "Name";
        public static readonly String NS_EVSPool_Description       = "Description";
        public static readonly String NS_EVSPool_LocationLanguage  = "LocationLanguage";

        public static XElement ToXML(this ChargingPool Pool)
        {

            return new XElement(NS_EVSPool + NS_EVSPool_Root,
                        Pool.Id.              ToXML(NS_EVSPool, NS_EVSPool_Id),
                        Pool.LastChange.       ToXML(NS_EVSPool, NS_EVSPool_Timestamp),
                        Pool.Name.            ToXML(NS_EVSPool, NS_EVSPool_Name),
                        (!Pool.Description.IsEmpty) ?
                        Pool.Description.     ToXML(NS_EVSPool, NS_EVSPool_Description)      : null,
                        (Pool.LocationLanguage != Languages.undef) ?
                        Pool.LocationLanguage.ToXML(NS_EVSPool, NS_EVSPool_LocationLanguage) : null,
                        Pool.PoolLocation.    ToXML(NS_EVSPool, "PoolLocation"),
                        Pool.EntranceLocation.ToXML(NS_EVSPool, "EntranceLocation")
                   );

        }

        #endregion


        public static XDocument ToXML(this ChargingStation Station)
        {

            var _XDocument = new XDocument();




            return _XDocument;

        }

        public static XDocument ToXML(this EVSE EVSE)
        {

            var _XDocument = new XDocument();




            return _XDocument;

        }

        public static XDocument ToXML(this SocketOutlet Socket)
        {

            var _XDocument = new XDocument();




            return _XDocument;

        }

    }

}
