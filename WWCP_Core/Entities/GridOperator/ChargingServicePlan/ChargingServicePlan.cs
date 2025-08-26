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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A electric vehicle charging service plan (EVCSP).
    /// </summary>
    public class ChargingServicePlan : AInternalData,
                                       IHasId<ChargingServicePlan_Id>,
                                       IEquatable<ChargingServicePlan>, IComparable<ChargingServicePlan>, IComparable
    {

        #region Data

        #endregion

        #region Properties

        /// <summary>
        /// The unique electric vehicle charging service plan identification.
        /// </summary>
        [Mandatory]
        public ChargingServicePlan_Id  Id             { get; }

        /// <summary>
        /// The (multi-language) name of the electric vehicle charging service plan identification.
        /// </summary>
        [Mandatory]
        public I18NString              Name           { get; }

        /// <summary>
        /// The optional (multi-language) description of the electric vehicle charging service plan identification.
        /// </summary>
        [Optional]
        public I18NString?             Description    { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new electric vehicle charging service plan having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the service plan.</param>
        /// <param name="Name">The (multi-language) name of the electric vehicle charging service plan identification.</param>
        /// <param name="Description">An optional (multi-language) description of the electric vehicle charging service plan identification.</param>
        internal ChargingServicePlan(ChargingServicePlan_Id?  Id            = null,
                                     I18NString?              Name          = null,
                                     I18NString?              Description   = null)

            : base(null,
                   null,
                   Timestamp.Now)

        {

            this.Id           = Id   ?? ChargingServicePlan_Id.NewRandom;
            this.Name         = Name ?? new I18NString(Languages.en,
                                                       "Charging Service Plan " + this.Id.ToString());
            this.Description  = Description;

        }

        #endregion


        #region IComparable<ChargingServicePlan> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a service plan.
            var ServicePlan = Object as ChargingServicePlan;
            if ((Object) ServicePlan is null)
                throw new ArgumentException("The given object is not a service plan!");

            return CompareTo(ServicePlan);

        }

        #endregion

        #region CompareTo(ChargingServicePlan)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingServicePlan">A service plan object to compare with.</param>
        public Int32 CompareTo(ChargingServicePlan ChargingServicePlan)
        {

            if ((Object) ChargingServicePlan is null)
                throw new ArgumentNullException("The given service plan must not be null!");

            return Id.CompareTo(ChargingServicePlan.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingServicePlan> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            // Check if the given object is a service plan.
            var ChargingServicePlan = Object as ChargingServicePlan;
            if ((Object) ChargingServicePlan is null)
                return false;

            return this.Equals(ChargingServicePlan);

        }

        #endregion

        #region Equals(ChargingServicePlan)

        /// <summary>
        /// Compares two service plans for equality.
        /// </summary>
        /// <param name="ChargingServicePlan">A service plan to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingServicePlan ChargingServicePlan)
        {

            if ((Object) ChargingServicePlan is null)
                return false;

            return Id.Equals(ChargingServicePlan.Id);

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
