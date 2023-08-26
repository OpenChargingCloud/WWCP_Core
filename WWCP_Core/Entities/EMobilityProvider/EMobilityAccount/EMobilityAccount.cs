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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A electric mobility account.
    /// </summary>
    public class EMobilityAccount : AInternalData,
                                    IHasId<EMobilityAccount_Id>,
                                    IEquatable<EMobilityAccount>, IComparable<EMobilityAccount>, IComparable
    {

        #region Properties

        /// <summary>
        /// The unique electric mobility account identification.
        /// </summary>
        [Mandatory]
        public EMobilityAccount_Id  Id      { get; }

        /// <summary>
        /// The offical (multi-language) name of the e-mobility account.
        /// </summary>
        [Mandatory]
        public I18NString           Name    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new parking sensor having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the parking sensor.</param>
        public EMobilityAccount(EMobilityAccount_Id  Id,
                                I18NString?          Name   = null)

            : base(null,
                   null,
                   Timestamp.Now)

        {


            this.Id    = Id;
            this.Name  = Name ?? new I18NString(Languages.en, Id.ToString());

        }

        #endregion


        #region Operator overloading

        #region Operator == (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EMobilityAccount EMobilityAccount1,
                                           EMobilityAccount EMobilityAccount2)
        {

            if (Object.ReferenceEquals(EMobilityAccount1, EMobilityAccount2))
                return true;

            if (EMobilityAccount1 is null || EMobilityAccount2 is null)
                return false;

            return EMobilityAccount1.Equals(EMobilityAccount2);

        }

        #endregion

        #region Operator != (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EMobilityAccount EMobilityAccount1,
                                           EMobilityAccount EMobilityAccount2)

            => !(EMobilityAccount1 == EMobilityAccount2);

        #endregion

        #region Operator <  (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EMobilityAccount EMobilityAccount1,
                                          EMobilityAccount EMobilityAccount2)

            => EMobilityAccount1 is null
                   ? throw new ArgumentNullException(nameof(EMobilityAccount1), "The given EMobilityAccount must not be null!")
                   : EMobilityAccount1.CompareTo(EMobilityAccount2) < 0;

        #endregion

        #region Operator <= (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EMobilityAccount EMobilityAccount1,
                                           EMobilityAccount EMobilityAccount2)

            => !(EMobilityAccount1 > EMobilityAccount2);

        #endregion

        #region Operator >  (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EMobilityAccount EMobilityAccount1,
                                          EMobilityAccount EMobilityAccount2)

            => EMobilityAccount1 is null
                   ? throw new ArgumentNullException(nameof(EMobilityAccount1), "The given EMobilityAccount must not be null!")
                   : EMobilityAccount1.CompareTo(EMobilityAccount2) > 0;

        #endregion

        #region Operator >= (EMobilityAccount1, EMobilityAccount2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityAccount1">An electric mobility account.</param>
        /// <param name="EMobilityAccount2">Another electric mobility account.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EMobilityAccount EMobilityAccount1,
                                           EMobilityAccount EMobilityAccount2)

            => !(EMobilityAccount1 < EMobilityAccount2);

        #endregion

        #endregion

        #region IComparable<EMobilityAccount> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two electric mobility accounts.
        /// </summary>
        /// <param name="Object">An electric mobility account to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMobilityAccount eMobilityAccount
                   ? CompareTo(eMobilityAccount)
                   : throw new ArgumentException("The given object is not an electric mobility account!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityAccount)

        /// <summary>
        /// Compares two electric mobility accounts.
        /// </summary>
        /// <param name="EMobilityAccount">An electric mobility account to compare with.</param>
        public Int32 CompareTo(EMobilityAccount? EMobilityAccount)
        {

            if (EMobilityAccount is null)
                throw new ArgumentNullException(nameof(EMobilityAccount), "The given electric mobility account must not be null!");

            var c = Id.CompareTo(EMobilityAccount.Id);

            //if (c == 0)
            //    c = Name.CompareTo(EMobilityAccount.Name);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EMobilityAccount> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two electric mobility accounts for equality.
        /// </summary>
        /// <param name="Object">An electric mobility account to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityAccount eMobilityAccount &&
                   Equals(eMobilityAccount);

        #endregion

        #region Equals(EMobilityAccount)

        /// <summary>
        /// Compares two electric mobility accounts for equality.
        /// </summary>
        /// <param name="EMobilityAccount">An electric mobility account to compare with.</param>
        public Boolean Equals(EMobilityAccount? EMobilityAccount)

            => EMobilityAccount is not null &&

               Id.  Equals(EMobilityAccount.Id) &&
               Name.Equals(EMobilityAccount.Name);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

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
