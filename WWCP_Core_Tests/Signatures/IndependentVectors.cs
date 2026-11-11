/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// Test vectors from an implementation other than this library: Node.js v26.12.1,
    /// whose Number::toString is V8's and whose ECDSA is OpenSSL's. Made by vectors.mjs;
    /// the numbers start with the bit patterns of RFC 8785 Appendix B.
    /// </summary>
    internal static class IndependentVectors
    {

        /// <summary>
        /// An IEEE 754 double as 16 hex digits, and the text ECMAScript writes for it.
        /// </summary>
        public static readonly String[][] Numbers = [
            [ "0000000000000000", "0" ],
            [ "8000000000000000", "0" ],
            [ "0000000000000001", "5e-324" ],
            [ "8000000000000001", "-5e-324" ],
            [ "7fefffffffffffff", "1.7976931348623157e+308" ],
            [ "ffefffffffffffff", "-1.7976931348623157e+308" ],
            [ "4340000000000000", "9007199254740992" ],
            [ "c340000000000000", "-9007199254740992" ],
            [ "4430000000000000", "295147905179352830000" ],
            [ "44b52d02c7e14af5", "9.999999999999997e+22" ],
            [ "44b52d02c7e14af6", "1e+23" ],
            [ "44b52d02c7e14af7", "1.0000000000000001e+23" ],
            [ "444b1ae4d6e2ef4e", "999999999999999700000" ],
            [ "444b1ae4d6e2ef4f", "999999999999999900000" ],
            [ "444b1ae4d6e2ef50", "1e+21" ],
            [ "3eb0c6f7a0b5ed8c", "9.999999999999997e-7" ],
            [ "3eb0c6f7a0b5ed8d", "0.000001" ],
            [ "41b3de4355555553", "333333333.3333332" ],
            [ "41b3de4355555554", "333333333.33333325" ],
            [ "41b3de4355555555", "333333333.3333333" ],
            [ "41b3de4355555556", "333333333.3333334" ],
            [ "41b3de4355555557", "333333333.33333343" ],
            [ "becbf647612f3696", "-0.0000033333333333333333" ],
            [ "43143ff3c1cb0959", "1424953923781206.2" ],
            [ "d3ec979db8d53dba", "-1.9085211304793906e+96" ],
            [ "f9aa650c63c4a101", "-1.1697231347495056e+278" ],
            [ "394516960e6c4d7c", "8.122889639245819e-33" ],
            [ "041d3fdd240da41b", "7.503469887453153e-289" ],
            [ "ad9d07a899edbf0e", "-5.7004229161755356e-89" ],
            [ "ee92920d532008c5", "-4.296162991553797e+224" ],
            [ "53fef6f40f0bc6f0", "4.1337572264992134e+96" ],
            [ "d57cf0f67dda897f", "-6.48210375231053e+103" ],
            [ "970077993bf2ada2", "-6.884178787132854e-198" ],
            [ "6bd463710c7d64c9", "2.681149622023233e+211" ],
            [ "87e4d5ccb36eefa4", "-1.232453768097441e-270" ],
            [ "2c856c08b53dfd23", "3.209309047357615e-94" ],
            [ "a02f8a524d405d76", "-1.1761922042010476e-153" ],
            [ "d290983ce7c3790d", "-5.281868567879496e+89" ],
            [ "55110d2bfe623b98", "5.967328308408258e+101" ],
            [ "aad7320339b46307", "-2.5890630837331077e-102" ],
            [ "befa174f5050628a", "-0.00002488236807005271" ],
            [ "2e44639448bd4991", "8.199488740392323e-86" ],
            [ "709d372585915ecc", "2.9028775909648332e+234" ],
            [ "41f5c9f74cb85f2b", "5848921291.523234" ],
            [ "aedac526130d90de", "-5.512027773082932e-83" ],
            [ "d2cd339144de1a55", "-7.435546803515395e+90" ],
            [ "6bb44739d0fb4d40", "6.66663902359118e+210" ],
            [ "c3ca80c85526d58f", "-3819493020108791300" ],
            [ "18016e17f367fc72", "4.775419129008928e-193" ],
            [ "280d49f71d456f59", "9.291699123647616e-116" ],
            [ "46102091c9a63af4", "3.194325917621068e+29" ],
            [ "0b582c4da0e2ea33", "5.151756047717288e-254" ],
            [ "d6b090854528f946", "-3.890251238530712e+109" ],
            [ "3bcaf46351030c9d", "1.1415734270764951e-20" ],
            [ "1b06a042ab939be8", "1.7448604344293137e-178" ],
            [ "1d61f14ff8ba0117", "3.8034563145802984e-167" ],
            [ "4859ac926d071b5a", "3.494578669994209e+40" ],
            [ "b9c89d664b9af621", "-2.4272108328273597e-30" ],
            [ "72a0a00c2ef4241c", "1.4189693383426867e+244" ],
            [ "aee080abaa87be3b", "-6.795854706220075e-83" ],
            [ "dfb37d1f84fa36ae", "-1.0207092982431069e+153" ],
            [ "999f3f74f3c96fe5", "-2.87265329965044e-185" ],
            [ "b57448b5dc9bc790", "-3.388401635762541e-51" ],
            [ "df077621ed9a059f", "-5.999883367692228e+149" ],
            [ "d0f6a1989dcf5f42", "-1.0733641653295619e+82" ],
            [ "be58aa309e86fde9", "-2.2970889109036988e-8" ],
            [ "f9db9297c1b5ba44", "-9.775391633583058e+278" ],
            [ "df2bc94ac554fb43", "-2.842356379976802e+150" ],
            [ "9a4d115df1fce916", "-5.4727472208931944e-182" ],
            [ "564802532a4c642d", "4.405183615439001e+107" ],
            [ "6bf0f235f6b87038", "8.913897795323134e+211" ],
            [ "7f4948e2c6170327", "1.3871505771312888e+305" ],
            [ "b63fa5d09db6682a", "-2.165424496979793e-47" ],
            [ "1b5868e14696a6b1", "6.023720002602509e-177" ],
            [ "40077d65ad999d6c", "2.93622909188314" ],
            [ "157b389e605cc14b", "3.39148343275732e-205" ],
            [ "3dd316b1b040b07e", "6.944409983095547e-11" ],
            [ "6b96875d2bab0975", "1.851630746943481e+210" ],
            [ "04ecde75bb4235e0", "6.066882918247435e-285" ],
            [ "f3ff9bfe962519af", "-5.657857724107478e+250" ],
            [ "2f76eab6aa85d612", "4.831879328854536e-80" ],
            [ "b69020d0469b1079", "-7.062622223431305e-46" ],
            [ "9d0fed8bd0426d94", "-1.0575063766176094e-168" ],
            [ "d2d0db993e953053", "-8.584989873444925e+90" ],
            [ "444b1ae4d6e2ef50", "1e+21" ],
            [ "4415af1d78b58c40", "100000000000000000000" ],
            [ "3e7ad7f29abcaf48", "1e-7" ],
            [ "3eb0c6f7a0b5ed8d", "0.000001" ],
            [ "3fb999999999999a", "0.1" ],
            [ "405edd2f1a9fbe77", "123.456" ],
            [ "bfe0000000000000", "-0.5" ],
            [ "4340000000000000", "9007199254740992" ],
            [ "4012000000000000", "4.5" ],
            [ "3f60624dd2f1a9fc", "0.002" ],
            [ "3a53ce9a36f23c10", "1e-27" ],
            [ "46293e5939a08cea", "1e+30" ],
            [ "41b3de4355555555", "333333333.3333333" ],
        ];

        /// <summary>
        /// alg, crv, x, y, JWS signing input, signature (r||s, base64url) made by Node, RFC 7638 thumbprint.
        /// </summary>
        public static readonly String[][] Signatures = [
            [ "ES256", "P-256", "EtENhitacYUulF2SQzo9arHJNMC4Sdo-JV09WkUNs_k", "urMY6zG7NeJ6PRY61zrZ8DXaU_8838L8XYWDU6Lq0DY", "eyJhbGciOiJFUzI1NiJ9.eyJoZWxsbyI6IndvcmxkIn0", "_OqC-hRCD9SFzU-GBaPZ9hPRFd93AweePkGu9Xd7Df9zUBPoqHg6nZmJRgzz-04gRGGGr2HkMz0SkbE_-y0tow", "4Q-krWpi5nlsYTEFq2GfVwlb56vfn8-KjAFFWPrCtTo" ],
            [ "ES384", "P-384", "VrlIx7-HZ2inBCk0ghR201qBqM5hJS1f2hl86X0oS-9rgndkivE9C7rxz7YXsQyb", "OChdIGv1yN__gOJkxAwNEMvYw8C75kwTC_zEvP0EuE7lZO27PEOLcpxLzhD37B03", "eyJhbGciOiJFUzM4NCJ9.eyJoZWxsbyI6IndvcmxkIn0", "CdodrmVJ56CetZi_gx3ZLDQEgNntsWeye04_RTFLI2qLZ5sqqZyT9-e3j5cJwWCCqmVfEWIUHGS4owuWbO5F42t7gdB2UMalMKnO5OWbM6k41-kkhLxxFiIi6apfPadJ", "QGGfLr1U-c05ra0SZjypLY186AdDFttA5zpAguHw8Ho" ],
            [ "ES512", "P-521", "AUwdynOQKZlSY0kckuqTRff3SmfhQpRI-oSATDCh7-Ni9l9QSS3AnkP3kZF4Z_V7ydhcO-SlHsLu-LGf8Ig_XmHT", "AFAbMeEMsgcFufmIk1K5UrzXJBZphedt1aAdkO2LgS6IwJS3tJ4I1PjH9Q4rQ0wqdaSbBxFLRgGPPrcaSl8WwRV8", "eyJhbGciOiJFUzUxMiJ9.eyJoZWxsbyI6IndvcmxkIn0", "ALldx4YNm6MOqUCvYPsWhOu5ZthLsByKU_8U8wla7wDcj48qZaKQp3srNG2BMrzkxPJN6xJSlzUIw0mZKOdpxHPqAFuP_Ipl9zaIo771IujPp3nE_zXLFKBuNcwGKBw1X2GQ77u0zTQ1Gk3swwF9OOEv0G701fNmNnAwAB1qXEM7Sxab", "-rD0p85n4v_L0aN1m9NW52Bh0ahwk7ISmFN7XEen-po" ],
        ];

        /// <summary>
        /// The thumbprint Node computes for the key of RFC 7515 Appendix A.3.
        /// </summary>
        public const String RFC7515A3Thumbprint = "oKIywvGUpTVTyxMQ3bwIIeQUudfr_CkLMjCE19ECD-U";

    }

}
