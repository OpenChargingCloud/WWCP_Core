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

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Asn1.X9;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The common interface of an entity having one or multiple unique
    /// identification(s) and crypto methods.
    /// </summary>
    public interface ICryptoEntity : IEntity
    {

        String?                  EllipticCurve            { get; }
        X9ECParameters?          ECP                      { get; }
        ECDomainParameters?      ECSpec                   { get; }
        FpCurve?                 C                        { get; }
        ECPrivateKeyParameters?  PrivateKey               { get; }
        PublicKeyCertificates?   PublicKeyCertificates    { get; }

        Signature?               Signature                { get; }

        Signature Sign();

    }

    /// <summary>
    /// The common generic interface of an entity having one or multiple unique
    /// identification(s) and crypto methods.
    /// </summary>
    /// <typeparam name="TId">THe type of the unique identificator.</typeparam>
    public interface ICryptoEntity<TId> : IEntity<TId>,
                                          ICryptoEntity

        where TId : IId

    { }

}
