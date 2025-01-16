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


    public class EVRoamingPartnerInfo
    {

        public EMobilityProvider_Id   EMPId        { get; }
        public I18NString             Name         { get; }
        public DateTime?              NotBefore    { get; }
        public DateTime?              NotAfter     { get; }
        public I18NString             Comment      { get; }

        public EVRoamingPartnerInfo(EMobilityProvider_Id  EMPId,
                                    I18NString?           Name,
                                    DateTime?             NotBefore,
                                    DateTime?             NotAfter,
                                    I18NString?           Comment     = null)
        {

            this.EMPId      = EMPId;
            this.Name       = Name    ?? I18NString.Empty;
            this.NotBefore  = NotBefore;
            this.NotAfter   = NotAfter;
            this.Comment    = Comment ?? I18NString.Empty;

        }


        #region Clone()

        /// <summary>
        /// Clone this root CA information.
        /// </summary>
        public EVRoamingPartnerInfo Clone()

            => new (
                   EMPId.  Clone(),
                   Name.   Clone(),
                   NotBefore,
                   NotAfter,
                   Comment.Clone()
               );

        #endregion

    }

}
