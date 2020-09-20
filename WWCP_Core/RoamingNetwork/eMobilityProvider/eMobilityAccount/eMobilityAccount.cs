/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A electric mobility account.
    /// </summary>
    public class eMobilityAccount : AEMobilityEntity<eMobilityAccount_Id>,
                                    IEquatable<eMobilityAccount>, IComparable<eMobilityAccount>, IComparable
    {

        #region Data

        #endregion

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the e-mobility account.
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


        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        #region (internal) eMobilityAccount(Id)

        /// <summary>
        /// Create a new parking sensor having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the parking sensor.</param>
        internal eMobilityAccount(eMobilityAccount_Id  Id)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The unique identification of the e-mobility account must not be null!");

            #endregion

            #region Init data and properties

            this._Name              = new I18NString(Languages.eng, Id.ToString());

            #endregion

        }

        #endregion

        #endregion


        #region IComparable<eMobilityAccount> Members

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
            var ServicePlan = Object as eMobilityAccount;
            if ((Object) ServicePlan == null)
                throw new ArgumentException("The given object is not a service plan!");

            return CompareTo(ServicePlan);

        }

        #endregion

        #region CompareTo(eMobilityAccount)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityAccount">A service plan object to compare with.</param>
        public Int32 CompareTo(eMobilityAccount eMobilityAccount)
        {

            if ((Object) eMobilityAccount == null)
                throw new ArgumentNullException("The given service plan must not be null!");

            return Id.CompareTo(eMobilityAccount.Id);

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityAccount> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a service plan.
            var eMobilityAccount = Object as eMobilityAccount;
            if ((Object) eMobilityAccount == null)
                return false;

            return this.Equals(eMobilityAccount);

        }

        #endregion

        #region Equals(eMobilityAccount)

        /// <summary>
        /// Compares two service plans for equality.
        /// </summary>
        /// <param name="eMobilityAccount">A service plan to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityAccount eMobilityAccount)
        {

            if ((Object) eMobilityAccount == null)
                return false;

            return Id.Equals(eMobilityAccount.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
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
