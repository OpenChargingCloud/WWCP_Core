/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.SMM
{

    /// <summary>
    /// A smart meter model.
    /// </summary>
    public class SmartMeterModel
    {

        #region Data

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this smart meter model.
        /// </summary>
        public SmartMeterModel_Id         Id                { get; }

        /// <summary>
        /// The unique identification of the smart meter manufacturer.
        /// </summary>
        public SmartMeterManufacturer_Id  ManufacturerId    { get; }

        /// <summary>
        /// The multi-language name of this smart meter model.
        /// </summary>
        public I18NString                 Name              { get; }

        /// <summary>
        /// The multi-language description of this smart meter model.
        /// </summary>
        public I18NString                 Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter model.
        /// </summary>
        /// <param name="Id">An unique identification of this smart meter model.</param>
        /// <param name="Name">A multi-language name of this smart meter model.</param>
        /// <param name="Description">A multi-language description of this smart meter model.</param>
        public SmartMeterModel(SmartMeterModel_Id?  Id            = null,
                               I18NString?          Name          = null,
                               I18NString?          Description   = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique smart meter model identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? SmartMeterModel_Id.NewRandom();
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


        #region Clone

        /// <summary>
        /// Clone this smart meter model.
        /// </summary>
        public SmartMeterModel Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models for equality.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SmartMeterModel SmartMeterModel1,
                                           SmartMeterModel SmartMeterModel2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SmartMeterModel1, SmartMeterModel2))
                return true;

            // If one is null, but not both, return false.
            if (SmartMeterModel1 is null || SmartMeterModel2 is null)
                return false;

            return SmartMeterModel1.Equals(SmartMeterModel2);

        }

        #endregion

        #region Operator != (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models for inequality.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SmartMeterModel SmartMeterModel1,
                                           SmartMeterModel SmartMeterModel2)

            => !(SmartMeterModel1 == SmartMeterModel2);

        #endregion

        #region Operator <  (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SmartMeterModel SmartMeterModel1,
                                          SmartMeterModel SmartMeterModel2)
        {

            if (SmartMeterModel1 is null)
                throw new ArgumentNullException(nameof(SmartMeterModel1), "The given smart meter model 1 must not be null!");

            return SmartMeterModel1.CompareTo(SmartMeterModel2) < 0;

        }

        #endregion

        #region Operator <= (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SmartMeterModel SmartMeterModel1,
                                           SmartMeterModel SmartMeterModel2)

            => !(SmartMeterModel1 > SmartMeterModel2);

        #endregion

        #region Operator >  (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SmartMeterModel SmartMeterModel1,
                                          SmartMeterModel SmartMeterModel2)
        {

            if (SmartMeterModel1 is null)
                throw new ArgumentNullException(nameof(SmartMeterModel1), "The given smart meter model 1 must not be null!");

            return SmartMeterModel1.CompareTo(SmartMeterModel2) > 0;

        }

        #endregion

        #region Operator >= (SmartMeterModel1, SmartMeterModel2)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="SmartMeterModel1">A smart meter model.</param>
        /// <param name="SmartMeterModel2">Another smart meter model.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SmartMeterModel SmartMeterModel1,
                                           SmartMeterModel SmartMeterModel2)

            => !(SmartMeterModel1 < SmartMeterModel2);

        #endregion

        #endregion

        #region IComparable<SmartMeterModel> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="Object">A smart meter model to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterModel smartMeterManufacturer
                   ? CompareTo(smartMeterManufacturer)
                   : throw new ArgumentException("The given object is not a smart meter model!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterModel)

        /// <summary>
        /// Compares two smart meter models.
        /// </summary>
        /// <param name="SmartMeterModel">A smart meter model to compare with.</param>
        public Int32 CompareTo(SmartMeterModel SmartMeterModel)
        {

            if (SmartMeterModel is null)
                throw new ArgumentNullException(nameof(SmartMeterModel), "The given smart meter model must not be null!");

            var c = Id.         CompareTo(SmartMeterModel.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(SmartMeterModel.Name);

            //if (c == 0)
            //    c = Description.CompareTo(SmartMeterModel.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<SmartMeterModel> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter models for equality.
        /// </summary>
        /// <param name="Object">A smart meter model to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterModel smartMeterManufacturer &&
                   Equals(smartMeterManufacturer);

        #endregion

        #region Equals(SmartMeterModel)

        /// <summary>
        /// Compares two smart meter models for equality.
        /// </summary>
        /// <param name="SmartMeterModel">A smart meter model to compare with.</param>
        public Boolean Equals(SmartMeterModel SmartMeterModel)

            => SmartMeterModel is not null &&

               Id.         Equals(SmartMeterModel.Id)   &&
               Name.       Equals(SmartMeterModel.Name) &&
               Description.Equals(SmartMeterModel.Description);

        #endregion

        #endregion

        #region GetHashCode()

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
