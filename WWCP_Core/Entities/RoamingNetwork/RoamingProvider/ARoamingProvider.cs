///*
// * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace org.GraphDefined.WWCP
//{

//    /// <summary>
//    /// A e-Mobility Roaming Provider (EMRP).
//    /// </summary>
//    public abstract class ARoamingProvider : ABaseEMobilityEntity<RoamingProvider_Id>,
//                                             IEquatable<AEMPRoamingProvider>, IComparable<AEMPRoamingProvider>, IComparable
//    {

//        #region Properties

//        #region Name

//        private readonly I18NString _Name;

//        /// <summary>
//        /// The offical (multi-language) name of the roaming provider.
//        /// </summary>
//        [Mandatory]
//        public I18NString Name
//            => _Name;

//        #endregion

//        #region RoamingNetwork

//        protected readonly RoamingNetwork _RoamingNetwork;

//        /// <summary>
//        /// The attached roaming network.
//        /// </summary>
//        public RoamingNetwork RoamingNetwork
//            => _RoamingNetwork;

//        #endregion

//        #region AuthorizatorId

//        //private readonly Authorizator_Id _AuthorizatorId;

//        ///// <summary>
//        ///// The unique identification of the authorizator of this roaming provider.
//        ///// </summary>
//        //public Authorizator_Id AuthorizatorId
//        //    => _AuthorizatorId;

//        #endregion

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new e-Mobility Roaming Provider (EMRP) having the given
//        /// unique roaming provider identification, internationalized name
//        /// and attached roaming network.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="RoamingNetwork">The attached roaming network.</param>
//        internal ARoamingProvider(RoamingProvider_Id  Id,
//                                  I18NString          Name,
//                                  RoamingNetwork      RoamingNetwork)

//            : base(Id,
//                   RoamingNetwork)

//        {

//            #region Initial Checks

//            if (Name.IsNullOrEmpty())
//                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

//            if (RoamingNetwork == null)
//                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

//            #endregion

//            this._Name            = Name;
//            this._RoamingNetwork  = RoamingNetwork;
//            //this._AuthorizatorId  = Authorizator_Id.Parse(Id.ToString());

//        }

//        #endregion


//        #region IComparable<RoamingProvider> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException("The given object must not be null!");

//            // Check if the given object is a roaming provider.
//            var RoamingProvider = Object as AEMPRoamingProvider;
//            if ((Object) RoamingProvider == null)
//                throw new ArgumentException("The given object is not a roaming provider!");

//            return CompareTo(RoamingProvider);

//        }

//        #endregion

//        #region CompareTo(RoamingProvider)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="RoamingProvider">A roaming provider object to compare with.</param>
//        public Int32 CompareTo(AEMPRoamingProvider RoamingProvider)
//        {

//            if ((Object) RoamingProvider == null)
//                throw new ArgumentNullException("The given roaming provider must not be null!");

//            return Id.CompareTo(RoamingProvider.Id);

//        }

//        #endregion

//        #endregion

//        #region IEquatable<RoamingProvider> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            // Check if the given object is a roaming provider.
//            var RoamingProvider = Object as AEMPRoamingProvider;
//            if ((Object) RoamingProvider == null)
//                return false;

//            return this.Equals(RoamingProvider);

//        }

//        #endregion

//        #region Equals(RoamingProvider)

//        /// <summary>
//        /// Compares two roaming provider for equality.
//        /// </summary>
//        /// <param name="RoamingProvider">A roaming provider to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(AEMPRoamingProvider RoamingProvider)
//        {

//            if ((Object) RoamingProvider == null)
//                return false;

//            return Id.Equals(RoamingProvider.Id);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Get the hashcode of this object.
//        /// </summary>
//        public override Int32 GetHashCode()
//            => Id.GetHashCode();

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a string representation of this object.
//        /// </summary>
//        public override String ToString()
//            => Id.ToString();

//        #endregion

//    }

//}

