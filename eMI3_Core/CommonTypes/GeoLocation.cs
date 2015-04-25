///*
// * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
// * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;

//#endregion

//namespace org.GraphDefined.eMI3
//{

//    public enum GravitationalModel_old
//    {
//        WGS84,
//        EGM96,
//        EGM2008
//    }

//    /// <summary>
//    /// A geographical location.
//    /// </summary>
//    public class GeoLocation_old
//    {

//        #region Properties

//        public Boolean              IsValid
//        {
//            get
//            {
//                return (Latitude != 0.0 && Longitude != 0.0);
//            }
//        }


//        /// <summary>
//        /// The gravitational model.
//        /// </summary>
//        public GravitationalModel_old   Projection       { get; set; }

//        /// <summary>
//        /// The latitude.
//        /// </summary>
//        public Double               Latitude    { get; set; }

//        /// <summary>
//        /// The longitude.
//        /// </summary>
//        public Double               Longitude   { get; set; }

//        /// <summary>
//        /// The altitude.
//        /// </summary>
//        public Double               Altitude    { get; set; }

//        #endregion

//        #region Constructor(s)

//        #region GeoLocation()

//        /// <summary>
//        /// Generate a new geographical location.
//        /// </summary>
//        public GeoLocation_old()
//        {
//            this.Projection     = GravitationalModel_old.WGS84;
//            this.Altitude       = 0.0;
//        }

//        #endregion

//        #endregion


//        public static GeoLocation_old Create(Double Latitude, Double Longitude)
//        {
//            return new GeoLocation_old()
//            {
//                Latitude  = Latitude,
//                Longitude = Longitude
//            };
//        }


//        #region SetModel(Value)

//        /// <summary>
//        /// Set the gravitational model.
//        /// </summary>
//        /// <param name="Value">A gravitational model.</param>
//        public GeoLocation_old SetModel(GravitationalModel_old Value)
//        {

//            this.Projection = Value;

//            return this;

//        }

//        #endregion

//        #region SetLatitude(Value)

//        /// <summary>
//        /// Set the latitude.
//        /// </summary>
//        /// <param name="Value">The latitude.</param>
//        public GeoLocation_old SetLatitude(Double Value)
//        {

//            this.Latitude = Value;

//            return this;

//        }

//        #endregion

//        #region SetLongitude(Value)

//        /// <summary>
//        /// Set the longitude.
//        /// </summary>
//        /// <param name="Value">The longitude.</param>
//        public GeoLocation_old SetLongitude(Double Value)
//        {

//            this.Longitude = Value;

//            return this;

//        }

//        #endregion

//        #region SetLatLng(Latitude, Longitude)

//        /// <summary>
//        /// Set the latitude.
//        /// </summary>
//        /// <param name="Latitude">The latitude.</param>
//        /// <param name="Longitude">The longitude.</param>
//        public GeoLocation_old SetLatLng(Double Latitude, Double Longitude)
//        {

//            this.Latitude  = Latitude;
//            this.Longitude = Longitude;

//            return this;

//        }

//        #endregion

//        #region SetAltitude(Value)

//        /// <summary>
//        /// Set the altitude.
//        /// </summary>
//        /// <param name="Value">The altitude.</param>
//        public GeoLocation_old SetAltitude(Double Value)
//        {

//            this.Altitude = Value;

//            return this;

//        }

//        #endregion


//        #region GetHashCode()

//        /// <summary>
//        /// Return the HashCode of this object.
//        /// </summary>
//        /// <returns>The HashCode of this object.</returns>
//        public override Int32 GetHashCode()
//        {
//            return this.ToString().GetHashCode();
//        }

//        #endregion

//        #region ToString()

//        /// <summary>
//        /// Return a string represtentation of this object.
//        /// </summary>
//        public override String ToString()
//        {
//            return Latitude + " " + Longitude + " " + Altitude;
//        }

//        #endregion

//    }

//}
