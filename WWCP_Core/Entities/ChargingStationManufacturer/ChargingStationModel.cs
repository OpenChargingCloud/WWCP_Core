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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.CSM
{

    /// <summary>
    /// A charging station model.
    /// </summary>
    public class ChargingStationModel
    {

        #region Data

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this charging station model.
        /// </summary>
        public ChargingStationModel_Id         Id                { get; }

        /// <summary>
        /// The unique identification of the charging station manufacturer.
        /// </summary>
        public ChargingStationManufacturer_Id  ManufacturerId    { get; }

        /// <summary>
        /// The multi-language name of this charging station model.
        /// </summary>
        public I18NString                      Name              { get; }

        /// <summary>
        /// The multi-language description of this charging station model.
        /// </summary>
        public I18NString                      Description       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station model.
        /// </summary>
        /// <param name="Id">An unique identification of this charging station model.</param>
        /// <param name="Name">A multi-language name of this charging station model.</param>
        /// <param name="Description">A multi-language description of this charging station model.</param>
        public ChargingStationModel(ChargingStationModel_Id?  Id            = null,
                                    I18NString?               Name          = null,
                                    I18NString?               Description   = null)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Id), "The given unique charging station model identification must not be null or empty!");

            #endregion

            this.Id               = Id          ?? ChargingStationModel_Id.NewRandom();
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
        /// Clone this charging station model.
        /// </summary>
        public ChargingStationModel Clone

            => new (Id.Clone);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models for equality.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationModel ChargingStationModel1,
                                           ChargingStationModel ChargingStationModel2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationModel1, ChargingStationModel2))
                return true;

            // If one is null, but not both, return false.
            if (ChargingStationModel1 is null || ChargingStationModel2 is null)
                return false;

            return ChargingStationModel1.Equals(ChargingStationModel2);

        }

        #endregion

        #region Operator != (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models for inequality.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationModel ChargingStationModel1,
                                           ChargingStationModel ChargingStationModel2)

            => !(ChargingStationModel1 == ChargingStationModel2);

        #endregion

        #region Operator <  (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationModel ChargingStationModel1,
                                          ChargingStationModel ChargingStationModel2)
        {

            if (ChargingStationModel1 is null)
                throw new ArgumentNullException(nameof(ChargingStationModel1), "The given charging station model 1 must not be null!");

            return ChargingStationModel1.CompareTo(ChargingStationModel2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationModel ChargingStationModel1,
                                           ChargingStationModel ChargingStationModel2)

            => !(ChargingStationModel1 > ChargingStationModel2);

        #endregion

        #region Operator >  (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationModel ChargingStationModel1,
                                          ChargingStationModel ChargingStationModel2)
        {

            if (ChargingStationModel1 is null)
                throw new ArgumentNullException(nameof(ChargingStationModel1), "The given charging station model 1 must not be null!");

            return ChargingStationModel1.CompareTo(ChargingStationModel2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationModel1, ChargingStationModel2)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="ChargingStationModel1">A charging station model.</param>
        /// <param name="ChargingStationModel2">Another charging station model.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationModel ChargingStationModel1,
                                           ChargingStationModel ChargingStationModel2)

            => !(ChargingStationModel1 < ChargingStationModel2);

        #endregion

        #endregion

        #region IComparable<ChargingStationModel> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="Object">A charging station model to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationModel chargingStationManufacturer
                   ? CompareTo(chargingStationManufacturer)
                   : throw new ArgumentException("The given object is not a charging station model!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationModel)

        /// <summary>
        /// Compares two charging station models.
        /// </summary>
        /// <param name="ChargingStationModel">A charging station model to compare with.</param>
        public Int32 CompareTo(ChargingStationModel ChargingStationModel)
        {

            if (ChargingStationModel is null)
                throw new ArgumentNullException(nameof(ChargingStationModel), "The given charging station model must not be null!");

            var c = Id.         CompareTo(ChargingStationModel.Id);

            //if (c == 0)
            //    c = Name.       CompareTo(ChargingStationModel.Name);

            //if (c == 0)
            //    c = Description.CompareTo(ChargingStationModel.Description);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationModel> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station models for equality.
        /// </summary>
        /// <param name="Object">A charging station model to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationModel chargingStationManufacturer &&
                   Equals(chargingStationManufacturer);

        #endregion

        #region Equals(ChargingStationModel)

        /// <summary>
        /// Compares two charging station models for equality.
        /// </summary>
        /// <param name="ChargingStationModel">A charging station model to compare with.</param>
        public Boolean Equals(ChargingStationModel ChargingStationModel)

            => ChargingStationModel is not null &&

               Id.         Equals(ChargingStationModel.Id)   &&
               Name.       Equals(ChargingStationModel.Name) &&
               Description.Equals(ChargingStationModel.Description);

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
