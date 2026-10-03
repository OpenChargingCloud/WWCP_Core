/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// Self-signed certificates for TLS between a test and its server: one for
    /// the server, and one per client, named by its common name.
    /// </summary>
    /// <remarks>
    /// Each stands alone, without a CA, and is known to the other side by its
    /// thumbprint. A certificate that came with a CA would make Windows put the
    /// CA into the user's certificate store, as a client's TLS context does with
    /// the CAs it sends along. These are kept out of every store: a test that
    /// is done disposes of them, and with them of their private keys.
    /// </remarks>
    internal static class TestCertificates
    {

        #region Server()

        /// <summary>
        /// A certificate for a TLS server on localhost.
        /// </summary>
        public static X509Certificate2 Server()
        {

            using var key    = RSA.Create(2048);

            var request      = new CertificateRequest("CN=localhost", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var names        = new SubjectAlternativeNameBuilder();

            names.AddDnsName  ("localhost");
            names.AddIpAddress(System.Net.IPAddress.Loopback);

            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension([ new Oid("1.3.6.1.5.5.7.3.1") ], false));
            request.CertificateExtensions.Add(names.Build());

            return Usable(request);

        }

        #endregion

        #region Client(CommonName)

        /// <summary>
        /// A certificate for a TLS client named by the given common name.
        /// </summary>
        /// <param name="CommonName">The common name of the client, e.g. the identity of a charging station.</param>
        public static X509Certificate2 Client(String CommonName)
        {

            using var key    = RSA.Create(2048);

            var request      = new CertificateRequest($"CN={CommonName}", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, false));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension([ new Oid("1.3.6.1.5.5.7.3.2") ], false));

            return Usable(request);

        }

        #endregion


        #region (private static) Usable(Request)

        /// <summary>
        /// The self-signed certificate of the given request, with a private key
        /// SChannel can use: on Windows a key held in memory alone is turned
        /// down, so the certificate goes out to PKCS#12 and comes back in.
        /// </summary>
        private static X509Certificate2 Usable(CertificateRequest Request)
        {

            using var created = Request.CreateSelfSigned(
                                    DateTimeOffset.UtcNow.AddMinutes(-5),
                                    DateTimeOffset.UtcNow.AddDays(1)
                                );

            return X509CertificateLoader.LoadPkcs12(
                       created.Export(X509ContentType.Pfx),
                       null,
                       X509KeyStorageFlags.Exportable
                   );

        }

        #endregion

    }

}
