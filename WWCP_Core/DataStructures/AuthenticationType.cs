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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extensions methods for authentication type.
    /// </summary>
    public static class AuthenticationTypeExtensions
    {

        #region Parse    (Text)

        /// <summary>
        /// Parse the given text as an authentication type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication type.</param>
        public static AuthenticationType Parse(String Text)
        {

            if (TryParse(Text, out var authenticationType))
                return authenticationType;

            return AuthenticationType.Unknown;

        }

        #endregion

        #region TryParse (Text)

        /// <summary>
        /// Try to parse the given text as an authentication type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication type.</param>
        public static AuthenticationType? TryParse(String Text)
        {

            if (TryParse(Text, out var authenticationType))
                return authenticationType;

            return null;

        }

        #endregion

        #region TryParse (Text, out AuthenticationType)

        /// <summary>
        /// Try to parse the given text as an authentication type.
        /// </summary>
        /// <param name="Text">A text representation of an authentication type.</param>
        /// <param name="AuthenticationType">The parsed authentication type.</param>
        public static Boolean TryParse(String Text, out AuthenticationType AuthenticationType)
        {
            switch (Text.Trim().ToUpper())
            {

                case "LOCAL":
                    AuthenticationType = AuthenticationType.Local;
                    return true;

                case "REMOTE":
                    AuthenticationType = AuthenticationType.Remote;
                    return true;

                default:
                    AuthenticationType = AuthenticationType.Unknown;
                    return false;

            }
        }

        #endregion

        #region AsText   (this AuthenticationType)

        public static String AsText(this AuthenticationType AuthenticationType)

            => AuthenticationType switch {
                   AuthenticationType.Local   => "LOCAL",
                   AuthenticationType.Remote  => "REMOTE",
                   _                          => "unknown"
               };

        #endregion

    }

    public enum AuthenticationType
    {
        Unknown,
        Local,
        Remote
    }

}
