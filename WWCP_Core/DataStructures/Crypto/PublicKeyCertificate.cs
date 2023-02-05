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

using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Mail;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class PublicKeyCertificate
    {

        #region Properties

        private List<PublicKeyLifetime> _PublicKeys;
        public IEnumerable<PublicKeyLifetime>           PublicKeys
            => _PublicKeys;

        public Certificate_Id                           Id                            { get; }
        public String                                   Name                          { get; }
        public I18NString                               Description                   { get; }
        public SimpleEMailAddress?                      EMail                         { get; }
        public String                                   Web                           { get; }
        public JObject                                  Artwork                       { get; }
        public JObject                                  Operations                    { get; }
        public I18NString                               Comment                       { get; }
        public IEnumerable<ChargingStationOperator_Id>  ChargingStationOperatorIds    { get; }
        public ChargingStationOperator_Id?              ChargingStationOperatorId     { get; }
        public ChargingPool_Id?                         ChargingPoolId                { get; }
        public ChargingStation_Id?                      ChargingStationId             { get; }
        public EVSE_Id?                                 EVSEId                        { get; }
        public EnergyMeter_Id?                          EnergyMeterId                 { get; }
        public I18NString                               RegulatoryReferences          { get; }

        private List<CertificateSignature> _CertificateSignatures;
        public IEnumerable<CertificateSignature>        CertificateSignatures
            => _CertificateSignatures;

        #endregion

        #region Constructor(s)

        #region PublicKeyCertificate(PublicKey, ...)

        public PublicKeyCertificate(PublicKeyLifetime                        PublicKey,

                                    Certificate_Id?                          Id                           = null,
                                    String                                   Name                         = null,
                                    I18NString                               Description                  = null,
                                    SimpleEMailAddress?                      EMail                        = null,
                                    String                                   Web                          = null,
                                    JObject                                  Artwork                      = null,
                                    JObject                                  Operations                   = null,
                                    I18NString                               Comment                      = null,
                                    IEnumerable<ChargingStationOperator_Id>  ChargingStationOperatorIds   = null,
                                    ChargingStationOperator_Id?              ChargingStationOperatorId    = null,
                                    ChargingPool_Id?                         ChargingPoolId               = null,
                                    ChargingStation_Id?                      ChargingStationId            = null,
                                    EVSE_Id?                                 EVSEId                       = null,
                                    EnergyMeter_Id?                          EnergyMeterId                = null,
                                    I18NString                               RegulatoryReferences         = null,

                                    IEnumerable<CertificateSignature>        CertificateSignatures        = null,
                                    Boolean                                  SelfSign                     = false)

            : this(new PublicKeyLifetime[] { PublicKey },

                   Id,
                   Name,
                   Description,
                   EMail,
                   Web,
                   Artwork,
                   Operations,
                   Comment,
                   ChargingStationOperatorIds,
                   ChargingStationOperatorId,
                   ChargingPoolId,
                   ChargingStationId,
                   EVSEId,
                   EnergyMeterId,
                   RegulatoryReferences,

                   CertificateSignatures,
                   SelfSign)

        { }

        #endregion

        #region PublicKeyCertificate(PublicKeys, ...)

        public PublicKeyCertificate(IEnumerable<PublicKeyLifetime>           PublicKeys,

                                    Certificate_Id?                          Id                           = null,
                                    String                                   Name                         = null,
                                    I18NString                               Description                  = null,
                                    SimpleEMailAddress?                      EMail                        = null,
                                    String                                   Web                          = null,
                                    JObject                                  Artwork                      = null,
                                    JObject                                  Operations                   = null,
                                    I18NString                               Comment                      = null,
                                    IEnumerable<ChargingStationOperator_Id>  ChargingStationOperatorIds   = null,
                                    ChargingStationOperator_Id?              ChargingStationOperatorId    = null,
                                    ChargingPool_Id?                         ChargingPoolId               = null,
                                    ChargingStation_Id?                      ChargingStationId            = null,
                                    EVSE_Id?                                 EVSEId                       = null,
                                    EnergyMeter_Id?                          EnergyMeterId                = null,
                                    I18NString                               RegulatoryReferences         = null,

                                    IEnumerable<CertificateSignature>        CertificateSignatures        = null,
                                    Boolean                                  SelfSign                     = false)
        {

            this._PublicKeys                 = PublicKeys.SafeAny()
                                                   ? new List<PublicKeyLifetime>(PublicKeys)
                                                   : new List<PublicKeyLifetime>();

            this.Id                          = Id            ?? Certificate_Id.NewRandom(); //ToDo: Use better randomness!
            this.Name                        = Name;
            this.Description                 = Description   ?? I18NString.Empty;
            this.EMail                       = EMail;
            this.Web                         = Web;
            this.Artwork                     = Artwork;
            this.Operations                  = Operations;
            this.Comment                     = Comment;
            this.ChargingStationOperatorIds  = ChargingStationOperatorIds;
            this.ChargingStationOperatorId   = ChargingStationOperatorId;
            this.ChargingPoolId              = ChargingPoolId;
            this.ChargingStationId           = ChargingStationId;
            this.EVSEId                      = EVSEId;
            this.EnergyMeterId               = EnergyMeterId;
            this.RegulatoryReferences        = RegulatoryReferences;

            this._CertificateSignatures      = CertificateSignatures.SafeAny()
                                                   ? new List<CertificateSignature>(CertificateSignatures)
                                                   : new List<CertificateSignature>();

        }

        #endregion

        #endregion

        public PublicKeyCertificate AddCertificateSignature(CertificateSignature Signature)
        {

            if (Signature                 != null &&
                Signature.SignerPublicKey != null &&
                Signature.SignerPublicKey != null)
            {
                _CertificateSignatures.Add(Signature);
            }

            return this;

        }

    }


}
