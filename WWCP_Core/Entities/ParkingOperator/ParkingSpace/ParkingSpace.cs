﻿/*
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

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A parking space.
    /// </summary>
    public class ParkingSpace : AEMobilityEntity<ParkingSpace_Id,
                                                 ParkingSpaceAdminStatusTypes,
                                                 ParkingSpaceStatusTypes>,
                                IEquatable<ParkingSpace>, IComparable<ParkingSpace>, IComparable
    {

        #region Data

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The official (multi-language) name of the parking space.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                SetProperty<I18NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the parking space.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                SetProperty<I18NString>(ref _Description, value);
            }

        }

        #endregion

        #region OSM_WayId

        private String _OSM_WayId;

        /// <summary>
        /// OSM Node Id.
        /// </summary>
        [Optional]
        public String OSM_WayId
        {

            get
            {
                return _OSM_WayId;
            }

            set
            {
                SetProperty<String>(ref _OSM_WayId, value);
            }

        }

        #endregion

        #region Geometry

        private List<GeoCoordinate> _Geometry;

        /// <summary>
        /// An optional polygon geometry of the parking space.
        /// </summary>
        [Optional]
        public List<GeoCoordinate> Geometry
        {
            get
            {
                return _Geometry;
            }
        }

        #endregion

        // status := free, ocupied, reserved, not accessible

        // fee := double

        // fee unit := "€/h"

        // Opening hours

        // restrictions := EV only, must be plugged in, disabled persons only

        #region Sensors

        private ReactiveSet<String> _Sensors;

        /// <summary>
        /// Parking sensors at the parking space.
        /// </summary>
        [Optional]
        public ReactiveSet<String> Sensors
        {
            get
            {
                return _Sensors;
            }
        }

        #endregion

        #endregion

        #region Links

        #region ChargingStations

        private ReactiveSet<ChargingStation> _ChargingStations;

        /// <summary>
        /// Charging stations reachable from this parking space.
        /// </summary>
        [Optional]
        public ReactiveSet<ChargingStation> ChargingStations
        {
            get
            {
                return _ChargingStations;
            }
        }

        #endregion

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        #region (internal) ParkingSpace()

        /// <summary>
        /// Create a new parking space having a random identification.
        /// </summary>
        internal ParkingSpace()
            : this(ParkingSpace_Id.New)
        { }

        #endregion

        #region (internal) ParkingSpace(Id)

        /// <summary>
        /// Create a new parking space having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the parking space.</param>
        public ParkingSpace(ParkingSpace_Id  Id)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The unique identification of the parking space must not be null!");

            #endregion

            #region Init data and properties

            this._Name              = new I18NString(Languages.en, Id.ToString());
            this._Description       = new I18NString();
            this._Geometry          = new List<GeoCoordinate>();
            this._ChargingStations  = new ReactiveSet<ChargingStation>();

            #endregion

        }

        #endregion

        #endregion


        #region IComparable<ParkingSpace> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a service plan.
            var ServicePlan = Object as ParkingSpace;
            if ((Object) ServicePlan == null)
                throw new ArgumentException("The given object is not a service plan!");

            return CompareTo(ServicePlan);

        }

        #endregion

        #region CompareTo(ParkingSpace)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpace">A service plan object to compare with.</param>
        public Int32 CompareTo(ParkingSpace ParkingSpace)
        {

            if ((Object) ParkingSpace == null)
                throw new ArgumentNullException("The given service plan must not be null!");

            return Id.CompareTo(ParkingSpace.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ParkingSpace> Members

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

            // Check if the given object is a service plan.
            var ParkingSpace = Object as ParkingSpace;
            if ((Object) ParkingSpace == null)
                return false;

            return this.Equals(ParkingSpace);

        }

        #endregion

        #region Equals(ParkingSpace)

        /// <summary>
        /// Compares two service plans for equality.
        /// </summary>
        /// <param name="ParkingSpace">A service plan to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingSpace ParkingSpace)
        {

            if ((Object) ParkingSpace == null)
                return false;

            return Id.Equals(ParkingSpace.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "eMI3 charging service plan: " + Id.ToString();
        }

        #endregion

    }

}
