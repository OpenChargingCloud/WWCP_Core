/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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
using System.Xml.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An abstract base class for all SOAP clients.
    /// </summary>
    public abstract class ASOAPClient : IDisposable
    {

        #region Data

        /// <summary>
        /// The default timeout for upstream queries.
        /// </summary>
        public static readonly TimeSpan DefaultQueryTimeout  = TimeSpan.FromSeconds(180);

        /// <summary>
        /// The default HTTP user agent.
        /// </summary>
        public const String DefaultHTTPUserAgent = "GraphDefined SOAP Client";

        #endregion

        #region Properties

        /// <summary>
        /// A unqiue identification of this client.
        /// </summary>
        public String            ClientId                { get; }

        public String            Hostname                { get; }

        public IPPort            TCPPort                 { get; }

        public String            HTTPVirtualHost         { get; }

        public String            UserAgent               { get; }

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        public TimeSpan          RequestTimeout          { get; }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient         DNSClient               { get; }

        //   public X509Certificate2  ServerCert              { get; }

        public RemoteCertificateValidationCallback RemoteCertificateValidator { get; }

        public X509Certificate ClientCert { get; }

        //        public LocalCertificateSelectionCallback ClientCertificateSelector { get; set; }

        public Boolean          UseTLS                  { get; set; }

        #endregion

        #region Events

        #region OnException

        /// <summary>
        /// An event fired whenever an exception occured.
        /// </summary>
        public event OnExceptionDelegate OnException;

        #endregion

        #region OnHTTPError

        /// <summary>
        /// A delegate called whenever a HTTP error occured.
        /// </summary>
        public delegate void OnHTTPErrorDelegate(DateTime Timestamp, Object Sender, HTTPResponse HttpResponse);

        /// <summary>
        /// An event fired whenever a HTTP error occured.
        /// </summary>
        public event OnHTTPErrorDelegate OnHTTPError;

        #endregion

        #region OnSOAPError

        /// <summary>
        /// A delegate called whenever a SOAP error occured.
        /// </summary>
        public delegate void OnSOAPErrorDelegate(DateTime Timestamp, Object Sender, XElement SOAPXML);

        /// <summary>
        /// An event fired whenever a SOAP error occured.
        /// </summary>
        public event OnSOAPErrorDelegate OnSOAPError;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an abstract SOAP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname to connect to.</param>
        /// <param name="TCPPort">The TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="UserAgent">An optional HTTP user agent to use.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public ASOAPClient(String                               ClientId,
                           String                               Hostname,
                           IPPort                               TCPPort,
                           RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                           X509Certificate                      ClientCert                  = null,
                           String                               HTTPVirtualHost             = null,
                           String                               UserAgent                   = DefaultHTTPUserAgent,
                           TimeSpan?                            QueryTimeout                = null,
                           DNSClient                            DNSClient                   = null)
        {

            #region Initial checks

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname), "The given parameter must not be null or empty!");

            if (TCPPort == null)
                throw new ArgumentNullException(nameof(TCPPort),  "The given parameter must not be null!");

            #endregion

            this.ClientId                    = ClientId;
            this.Hostname                    = Hostname;
            this.TCPPort                     = TCPPort;

            this.RemoteCertificateValidator  = RemoteCertificateValidator;
            this.ClientCert                  = ClientCert;

            this.HTTPVirtualHost             = (HTTPVirtualHost != null)
                                                    ? HTTPVirtualHost
                                                    : Hostname;

            this.UserAgent                   = UserAgent;

            this.RequestTimeout              = QueryTimeout != null
                                                  ? QueryTimeout.Value
                                                  : DefaultQueryTimeout;

            this.DNSClient                   = (DNSClient == null)
                                                  ? new DNSClient()
                                                  : DNSClient;

        }

        #endregion


        #region (protected) SendSOAPError(Timestamp, Sender, SOAPXML)

        /// <summary>
        /// Notify that an HTTP error occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the error received.</param>
        /// <param name="Sender">The sender of this error message.</param>
        /// <param name="SOAPXML">The SOAP fault/error.</param>
        protected void SendSOAPError(DateTime  Timestamp,
                                     Object    Sender,
                                     XElement  SOAPXML)
        {

            DebugX.Log("AOICPUpstreamService => SOAP Fault: " + SOAPXML != null ? SOAPXML.ToString() : "<null>");

            var OnSOAPErrorLocal = OnSOAPError;
            if (OnSOAPErrorLocal != null)
                OnSOAPErrorLocal(Timestamp, Sender, SOAPXML);

        }

        #endregion

        #region (protected) SendHTTPError(Timestamp, Sender, HttpResponse)

        /// <summary>
        /// Notify that an HTTP error occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the error received.</param>
        /// <param name="Sender">The sender of this error message.</param>
        /// <param name="HttpResponse">The HTTP response related to this error message.</param>
        protected void SendHTTPError(DateTime      Timestamp,
                                     Object        Sender,
                                     HTTPResponse  HttpResponse)
        {

            DebugX.Log("AOICPUpstreamService => HTTP Status Code: " + HttpResponse != null ? HttpResponse.HTTPStatusCode.ToString() : "<null>");

            var OnHTTPErrorLocal = OnHTTPError;
            if (OnHTTPErrorLocal != null)
                OnHTTPErrorLocal(Timestamp, Sender, HttpResponse);

        }

        #endregion

        #region (protected) SendException(Timestamp, Sender, Exception)

        /// <summary>
        /// Notify that an exception occured.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the exception.</param>
        /// <param name="Sender">The sender of this exception.</param>
        /// <param name="Exception">The exception itself.</param>
        protected void SendException(DateTime   Timestamp,
                                     Object     Sender,
                                     Exception  Exception)
        {

            DebugX.Log("AOICPUpstreamService => Exception: " + Exception.Message);

            var OnExceptionLocal = OnException;
            if (OnExceptionLocal != null)
                OnExceptionLocal(Timestamp, Sender, Exception);

        }

        #endregion


        #region IsHubjectError(XML)

        //protected Boolean IsHubjectError(XElement                             XML,
        //                                 out OICPException                    OICPException,
        //                                 Action<DateTime, Object, Exception>  OnError)
        //{

        //    #region Initial checks

        //    if (OnError == null)
        //        throw new ArgumentNullException("The given OnError-delegate must not be null!");

        //    #endregion

        //    StatusCode _StatusCode = null;

        //    if (StatusCode.TryParse(XML, out _StatusCode))
        //    {
        //        OICPException = new OICPException(_StatusCode);
        //        OnError(DateTime.Now, XML, OICPException);
        //        return true;
        //    }

        //    OICPException = null;

        //    return false;

        //}

        #endregion


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public virtual void Dispose()
        { }

        #endregion

    }

}
