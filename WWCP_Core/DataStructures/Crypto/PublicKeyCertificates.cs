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

using System;
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class PublicKeyCertificates
    {

        public IEnumerable<PublicKeyCertificate> Certificates { get; }


        public PublicKeyCertificates(IEnumerable<PublicKeyCertificate> Certificates)
        {
            this.Certificates = Certificates;
        }



        #region Implicitly convert PublicKeyCertificate -> PublicKeyCertificates

        /// <summary>
        /// Implicitly convert a public key certificate into a list of public key certificates.
        /// </summary>
        /// <param name="Certificate">A public key certificate.</param>
        public static implicit operator PublicKeyCertificates(PublicKeyCertificate Certificate)

            => new PublicKeyCertificates(new PublicKeyCertificate[] { Certificate });

        #endregion


    }

}
