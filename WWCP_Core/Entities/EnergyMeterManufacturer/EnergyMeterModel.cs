/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using cloud.charging.open.protocols.WWCP.POI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A energy meter model.
    /// </summary>
    public class EnergyMeterModel
    {

        #region Data

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this energy meter model.
        /// </summary>
        public EnergyMeterModel_Id         Id                { get; }

        /// <summary>
        /// The unique identification of the energy meter manufacturer.
        /// </summary>
        public EnergyMeterManufacturer_Id  ManufacturerId    { get; }

        /// <summary>
        /// The multi-language name of this energy meter model.
        /// </summary>
        public I18NString                  Name              { get; }

        /// <summary>
        /// The multi-language description of this energy meter model.
        /// </summary>
        public I18NString                  Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter model.
        /// </summary>
        /// <param name="Id">An unique identification of this energy meter model.</param>
        /// <param name="Name">A multi-language name of this energy meter model.</param>
        /// <param name="Description">A multi-language description of this energy meter model.</param>
        public EnergyMeterModel(EnergyMeterModel_Id?  Id            = null,
                               I18NString?          Name          = null,
                               I18NString?          Description   = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique energy meter model identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? EnergyMeterModel_Id.NewRandom();
            this.Name             = Name        ?? I18NString.Empty;
            this.Description      = Description ?? I18NString.Empty;

            unchecked
            {

                hashCode = this.Id.         GetHashCode() * 5 ^
                           this.Name.       GetHashCode() * 3 ^
                           this.Description.GetHashCode();

            }

        }

        #endregion


        #region Clone()

        /// <summary>
        /// Clone this energy meter model.
        /// </summary>
        public EnergyMeterModel Clone()

            => new (
                   Id.Clone()
               );

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models for equality.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterModel EnergyMeterModel1,
                                           EnergyMeterModel EnergyMeterModel2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EnergyMeterModel1, EnergyMeterModel2))
                return true;

            // If one is null, but not both, return false.
            if (EnergyMeterModel1 is null || EnergyMeterModel2 is null)
                return false;

            return EnergyMeterModel1.Equals(EnergyMeterModel2);

        }

        #endregion

        #region Operator != (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models for inequality.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterModel EnergyMeterModel1,
                                           EnergyMeterModel EnergyMeterModel2)

            => !(EnergyMeterModel1 == EnergyMeterModel2);

        #endregion

        #region Operator <  (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterModel EnergyMeterModel1,
                                          EnergyMeterModel EnergyMeterModel2)
        {

            if (EnergyMeterModel1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterModel1), "The given energy meter model 1 must not be null!");

            return EnergyMeterModel1.CompareTo(EnergyMeterModel2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterModel EnergyMeterModel1,
                                           EnergyMeterModel EnergyMeterModel2)

            => !(EnergyMeterModel1 > EnergyMeterModel2);

        #endregion

        #region Operator >  (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterModel EnergyMeterModel1,
                                          EnergyMeterModel EnergyMeterModel2)
        {

            if (EnergyMeterModel1 is null)
                throw new ArgumentNullException(nameof(EnergyMeterModel1), "The given energy meter model 1 must not be null!");

            return EnergyMeterModel1.CompareTo(EnergyMeterModel2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterModel1, EnergyMeterModel2)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="EnergyMeterModel1">A energy meter model.</param>
        /// <param name="EnergyMeterModel2">Another energy meter model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterModel EnergyMeterModel1,
                                           EnergyMeterModel EnergyMeterModel2)

            => !(EnergyMeterModel1 < EnergyMeterModel2);

        #endregion

        #endregion

        #region IComparable<EnergyMeterModel> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="Object">A energy meter model to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterModel smartMeterManufacturer
                   ? CompareTo(smartMeterManufacturer)
                   : throw new ArgumentException("The given object is not a energy meter model!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterModel)

        /// <summary>
        /// Compares two energy meter models.
        /// </summary>
        /// <param name="EnergyMeterModel">A energy meter model to compare with.</param>
        public Int32 CompareTo(EnergyMeterModel EnergyMeterModel)
        {

            if (EnergyMeterModel is null)
                throw new ArgumentNullException(nameof(EnergyMeterModel), "The given energy meter model must not be null!");

            var c = Id.         CompareTo(EnergyMeterModel.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(EnergyMeterModel.Name);

            //if (c == 0)
            //    c = Description.CompareTo(EnergyMeterModel.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterModel> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter models for equality.
        /// </summary>
        /// <param name="Object">A energy meter model to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterModel smartMeterManufacturer &&
                   Equals(smartMeterManufacturer);

        #endregion

        #region Equals(EnergyMeterModel)

        /// <summary>
        /// Compares two energy meter models for equality.
        /// </summary>
        /// <param name="EnergyMeterModel">A energy meter model to compare with.</param>
        public Boolean Equals(EnergyMeterModel EnergyMeterModel)

            => EnergyMeterModel is not null &&

               Id.         Equals(EnergyMeterModel.Id)   &&
               Name.       Equals(EnergyMeterModel.Name) &&
               Description.Equals(EnergyMeterModel.Description);

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
