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

using System.Diagnostics.CodeAnalysis;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for current types.
    /// </summary>
    public static class CurrentTypesExtensions
    {

        public static Boolean TryParse(String                                 Text,
                                       [NotNullWhen(true)] out CurrentTypes?  CurrentType)
        {

            switch (Text.Trim())
            {

                case "AC_1_PHASE":
                case "AC_1Phase":
                case "AC_OnePhase":
                    CurrentType = CurrentTypes.AC_OnePhase;
                    return true;

                case "AC_3_PHASE":
                case "AC_3Phases":
                case "AC_ThreePhases":
                    CurrentType = CurrentTypes.AC_ThreePhases;
                    return true;

                case "DC":
                    CurrentType = CurrentTypes.DC;
                    return true;

                default:
                    CurrentType = null;
                    return false;

            }

        }

        public static CurrentTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var currentType))
                return currentType;

            return null;

        }


        public static CurrentTypes Reduce(this IEnumerable<CurrentTypes> EnumerationOfCurrentTypes)
        {

            var currentTypes = CurrentTypes.Unspecified;

            foreach (var CurrentType in EnumerationOfCurrentTypes)
                currentTypes |= CurrentType;

            return currentTypes;

        }

        public static IEnumerable<CurrentTypes> ToEnumeration(this CurrentTypes CurrentTypesEnum)

            => Enum.GetValues(typeof(CurrentTypes)).
                    Cast<CurrentTypes>().
                    Where(flag => CurrentTypesEnum.HasFlag(flag) && flag != CurrentTypes.Unspecified);

        public static IEnumerable<String> ToText(this CurrentTypes CurrentTypesEnum)

            => CurrentTypesEnum.ToEnumeration().Select(item => item.ToString());

    }

    /// <summary>
    /// The type of the current at an EVSE.
    /// </summary>
    [Flags]
    public enum CurrentTypes
    {

        /// <summary>
        /// Unknown current type.
        /// </summary>
        Unspecified         = 0,

        /// <summary>
        /// AC with 1 phase.
        /// </summary>
        AC_OnePhase         = 1,

        /// <summary>
        /// AC with 3 phases.
        /// </summary>
        AC_ThreePhases      = 2,

        /// <summary>
        /// Direct current.
        /// </summary>
        DC                  = 4

    }

}
