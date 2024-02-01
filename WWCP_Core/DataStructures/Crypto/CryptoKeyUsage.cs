/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Crypto key usages within WWCP.
    /// </summary>
    public static class WWCPCryptoKeyUsage
    {

        /// <summary>
        /// Identity
        /// </summary>
        public static CryptoKeyUsage Identity
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/identity");

        /// <summary>
        /// Identity Group (Membership)
        /// </summary>
        public static CryptoKeyUsage IdentityGroup
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/identityGroup");

        /// <summary>
        /// Encryption
        /// </summary>
        public static CryptoKeyUsage Encryption
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/encryption");

        /// <summary>
        /// Signature
        /// </summary>
        public static CryptoKeyUsage Signature
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/signature");




        /// <summary>
        /// Register Networking Nodes
        /// </summary>
        public static CryptoKeyUsage RegisterNetworkingNodes
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/networking/nodes/register");

        /// <summary>
        /// Revoke Networking Nodes
        /// </summary>
        public static CryptoKeyUsage RevokeNetworkingNodes
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/networking/nodes/revoke");



        /// <summary>
        /// Register Metering Calibration Law Agencies
        /// </summary>
        public static CryptoKeyUsage RegisterMeteringCalibrationLawAgencies
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/MeteringCalibrationLawAgencies/register");

        /// <summary>
        /// Revoke Metering Calibration Law Agencies
        /// </summary>
        public static CryptoKeyUsage RevokeMeteringCalibrationLawAgencies
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/MeteringCalibrationLawAgencies/revoke");



        /// <summary>
        /// Register Smart Meter Manufacturers
        /// </summary>
        public static CryptoKeyUsage RegisterSmartMeterManufacturers
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/smartMeters/manufacturers/register");

        /// <summary>
        /// Revoke Smart Meter Manufacturers
        /// </summary>
        public static CryptoKeyUsage RevokeSmartMeterManufacturers
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/smartMeters/manufacturers/revoke");



        /// <summary>
        /// Register Charging Station Manufacturers
        /// </summary>
        public static CryptoKeyUsage RegisterChargingStationManufacturers
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/chargingStations/manufacturers/register");

        /// <summary>
        /// Revoke Charging Station Manufacturers
        /// </summary>
        public static CryptoKeyUsage RevokeChargingStationManufacturers
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/chargingStations/manufacturers/revoke");




        /// <summary>
        /// Signature for the Measuring Instruments Directive (MID)
        /// </summary>
        public static CryptoKeyUsage MeasuringInstrumentsDirective
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/MeasuringInstrumentsDirective/signature");

        /// <summary>
        /// Signature for the German Calibration Law: Type Approval (Module B)
        /// </summary>
        public static CryptoKeyUsage GermanCalibrationLaw_TypeApproval
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/GermanCalibrationLaw/TypeApproval/signature");

        /// <summary>
        /// Signature for the German Calibration Law: Quality Assurance (Module D)
        /// </summary>
        public static CryptoKeyUsage GermanCalibrationLaw_QualityAssurance
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/GermanCalibrationLaw/QualityAssurance/signature");

        /// <summary>
        /// Signature for the Office of Weights and Measures (Eichamt)
        /// </summary>
        public static CryptoKeyUsage Eichamt
            => CryptoKeyUsage.Lookup("https://open.charging.cloud/contexts/crypto/keyUsages/WeightsAndMeasuresOffice/signature");

    }

}
