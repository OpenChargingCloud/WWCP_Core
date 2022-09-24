/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An abstract e-mobility entity having crypto capabilities.
    /// </summary>
    public abstract class ACryptoEMobilityEntity<TId,
                                                 TAdminStatus,
                                                 TStatus> : AEMobilityEntity<TId,
                                                                             TAdminStatus,
                                                                             TStatus>,
                                                            ICryptoEntity<TId>

        where TId          : IId
        where TAdminStatus : IComparable
        where TStatus      : IComparable

    {

        #region Properties

        public I18NString              Name                     { get; }
        public IRoamingNetwork         RoamingNetwork           { get; }

        public String                  EllipticCurve            { get; }
        public X9ECParameters          ECP                      { get; }
        public ECDomainParameters      ECSpec                   { get; }
        public FpCurve                 C                        { get; }
        public ECPrivateKeyParameters  PrivateKey               { get; protected set; }
        public PublicKeyCertificates   PublicKeyCertificates    { get; protected set; }

        /// <summary>
        /// The cryptographical signature of this entity.
        /// </summary>
        public Signature?              Signature                { get; protected set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract crypto entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        protected ACryptoEMobilityEntity(TId                         Id,
                                         I18NString                  Name,
                                         IRoamingNetwork             RoamingNetwork,
                                         String                      EllipticCurve                = "P-256",
                                         ECPrivateKeyParameters?     PrivateKey                   = null,
                                         PublicKeyCertificates?      PublicKeyCertificates        = null,

                                         Timestamped<TAdminStatus>?  InitialAdminStatus           = null,
                                         Timestamped<TStatus>?       InitialStatus                = null,
                                         UInt16                      MaxAdminStatusScheduleSize   = DefaultMaxAdminStatusScheduleSize,
                                         UInt16                      MaxStatusScheduleSize        = DefaultMaxStatusScheduleSize,

                                         String?                     DataSource                   = null,
                                         DateTime?                   LastChange                   = null,

                                         JObject?                    CustomData                   = null,
                                         UserDefinedDictionary?      InternalData                 = null)

            : base(Id,
                   InitialAdminStatus,
                   InitialStatus,
                   MaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            this.Name                   = Name;
            this.RoamingNetwork         = RoamingNetwork;
            this.EllipticCurve          = EllipticCurve ?? "P-256";
            this.ECP                    = ECNamedCurveTable.GetByName(this.EllipticCurve);
            this.ECSpec                 = new ECDomainParameters(ECP.Curve, ECP.G, ECP.N, ECP.H, ECP.GetSeed());
            this.C                      = (FpCurve) ECSpec.Curve;
            this.PrivateKey             = PrivateKey;
            this.PublicKeyCertificates  = PublicKeyCertificates;

        }

        #endregion


        #region Sign()

        public Signature Sign()

            => null;// new Signature("");

        #endregion


    }

}
