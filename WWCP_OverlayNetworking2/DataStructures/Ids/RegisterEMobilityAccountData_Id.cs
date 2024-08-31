/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

namespace cloud.charging.open.protocols.OCPPv2_1
{

    /// <summary>
    /// Extension methods for RegisterEMobilityAccountData identifications.
    /// </summary>
    public static class RegisterEMobilityAccountDataIdExtensions
    {

        /// <summary>
        /// Indicates whether this RegisterEMobilityAccountData identification is null or empty.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId">A RegisterEMobilityAccountData identification.</param>
        public static Boolean IsNullOrEmpty(this RegisterEMobilityAccountData_Id? RegisterEMobilityAccountDataId)
            => !RegisterEMobilityAccountDataId.HasValue || RegisterEMobilityAccountDataId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this RegisterEMobilityAccountData identification is NOT null or empty.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId">A RegisterEMobilityAccountData identification.</param>
        public static Boolean IsNotNullOrEmpty(this RegisterEMobilityAccountData_Id? RegisterEMobilityAccountDataId)
            => RegisterEMobilityAccountDataId.HasValue && RegisterEMobilityAccountDataId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a RegisterEMobilityAccountData.
    /// </summary>
    public readonly struct RegisterEMobilityAccountData_Id : IId<RegisterEMobilityAccountData_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this RegisterEMobilityAccountData identification is null or empty.
        /// </summary>
        public readonly Boolean  IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this RegisterEMobilityAccountData identification is NOT null or empty.
        /// </summary>
        public readonly Boolean  IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the RegisterEMobilityAccountData identification.
        /// </summary>
        public readonly UInt64   Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RegisterEMobilityAccountData identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a RegisterEMobilityAccountData identification.</param>
        private RegisterEMobilityAccountData_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Random()

        /// <summary>
        /// Create a new (random) RegisterEMobilityAccountData identification.
        /// </summary>
        public static RegisterEMobilityAccountData_Id Random()

            => new ($"{UUIDv7.Generate()}");

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given text as a RegisterEMobilityAccountData identification.
        /// </summary>
        /// <param name="Text">A text representation of a RegisterEMobilityAccountData identification.</param>
        public static RegisterEMobilityAccountData_Id Parse(String Text)
        {

            if (TryParse(Text, out var RegisterEMobilityAccountDataId))
                return RegisterEMobilityAccountDataId;

            throw new ArgumentException($"Invalid text representation of a RegisterEMobilityAccountData identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given text as a RegisterEMobilityAccountData identification.
        /// </summary>
        /// <param name="Text">A text representation of a RegisterEMobilityAccountData identification.</param>
        public static RegisterEMobilityAccountData_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var RegisterEMobilityAccountDataId))
                return RegisterEMobilityAccountDataId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out RegisterEMobilityAccountDataId)

        /// <summary>
        /// Try to parse the given text as a RegisterEMobilityAccountData identification.
        /// </summary>
        /// <param name="Text">A text representation of a RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId">The parsed RegisterEMobilityAccountData identification.</param>
        public static Boolean TryParse(String Text, out RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RegisterEMobilityAccountDataId = new RegisterEMobilityAccountData_Id(Text);
                    return true;
                }
                catch
                { }
            }

            RegisterEMobilityAccountDataId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this RegisterEMobilityAccountData identification.
        /// </summary>
        public RegisterEMobilityAccountData_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                           RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => RegisterEMobilityAccountDataId1.Equals(RegisterEMobilityAccountDataId2);

        #endregion

        #region Operator != (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                           RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => !RegisterEMobilityAccountDataId1.Equals(RegisterEMobilityAccountDataId2);

        #endregion

        #region Operator <  (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                          RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => RegisterEMobilityAccountDataId1.CompareTo(RegisterEMobilityAccountDataId2) < 0;

        #endregion

        #region Operator <= (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                           RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => RegisterEMobilityAccountDataId1.CompareTo(RegisterEMobilityAccountDataId2) <= 0;

        #endregion

        #region Operator >  (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                          RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => RegisterEMobilityAccountDataId1.CompareTo(RegisterEMobilityAccountDataId2) > 0;

        #endregion

        #region Operator >= (RegisterEMobilityAccountDataId1, RegisterEMobilityAccountDataId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId1">A RegisterEMobilityAccountData identification.</param>
        /// <param name="RegisterEMobilityAccountDataId2">Another RegisterEMobilityAccountData identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId1,
                                           RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId2)

            => RegisterEMobilityAccountDataId1.CompareTo(RegisterEMobilityAccountDataId2) >= 0;

        #endregion

        #endregion

        #region IComparable<RegisterEMobilityAccountDataId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData identifications.
        /// </summary>
        /// <param name="Object">A RegisterEMobilityAccountData identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId
                   ? CompareTo(RegisterEMobilityAccountDataId)
                   : throw new ArgumentException("The given object is not a RegisterEMobilityAccountData identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RegisterEMobilityAccountDataId)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData identifications.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId">A RegisterEMobilityAccountData identification to compare with.</param>
        public Int32 CompareTo(RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId)

            => String.Compare(InternalId,
                              RegisterEMobilityAccountDataId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RegisterEMobilityAccountDataId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData identifications for equality.
        /// </summary>
        /// <param name="Object">A RegisterEMobilityAccountData identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId &&
                   Equals(RegisterEMobilityAccountDataId);

        #endregion

        #region Equals(RegisterEMobilityAccountDataId)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData identifications for equality.
        /// </summary>
        /// <param name="RegisterEMobilityAccountDataId">A RegisterEMobilityAccountData identification to compare with.</param>
        public Boolean Equals(RegisterEMobilityAccountData_Id RegisterEMobilityAccountDataId)

            => String.Equals(InternalId,
                             RegisterEMobilityAccountDataId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
