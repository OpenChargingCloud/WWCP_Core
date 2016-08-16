﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Net.Security;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A remote entity attached via a computer network (TCP/IP).
    /// </summary>
    public interface INetworkAttached
    {

        IPTransport                          IPTransport                { get; }
        DNSClient                            DNSClient                  { get; }
        String                               Hostname                   { get; }
        IPPort                               TCPPort                    { get; }
        RemoteCertificateValidationCallback  RemoteCertificateValidator { get; }
        String                               VirtualHost                { get; }
        String                               URIPrefix                  { get; }
        TimeSpan                             RequestTimeout             { get; }

    }

}