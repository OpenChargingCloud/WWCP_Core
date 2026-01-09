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

using NUnit.Framework;
using NUnit.Framework.Legacy;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Unit tests for charging station identifications.
    /// </summary>
    [TestFixture]
    public class ChargingStationIdTests
    {

        private readonly ChargingStationOperator_Id ChargingStationOperatorId  = ChargingStationOperator_Id.Parse("DE*GEF");
        private readonly ChargingPool_Id            ChargingPoolId             = ChargingPool_Id.           Parse("DE*GEF*P1234");


        #region Parse_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void Parse_ChargingStationOperatorId_Test()
        {

            var stationId = ChargingStation_Id.TryParse(ChargingStationOperatorId, "1234");

            ClassicAssert.IsNotNull(stationId);

            if (stationId.HasValue)
            {
                ClassicAssert.AreEqual("DE*GEF*S1234", stationId.Value.ToString());
                ClassicAssert.AreEqual(12,             stationId.Value.Length);
            }

        }

        #endregion

        #region Parse_ChargingPoolId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void Parse_ChargingPoolId_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            ClassicAssert.AreEqual("DE*GEF*S1234*5678", stationId.ToString());
            ClassicAssert.AreEqual(17,                  stationId.Length);
        }

        #endregion


        #region TryParse_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParse_ChargingStationOperatorId_Test()
        {

            var stationId = ChargingStation_Id.TryParse(ChargingStationOperatorId, "1234");
            ClassicAssert.IsNotNull(stationId);

            if (stationId is not null)
            {
                ClassicAssert.AreEqual("DE*GEF*S1234", stationId.Value.ToString());
                ClassicAssert.AreEqual(12,             stationId.Value.Length);
            }

        }

        #endregion

        #region TryParse_ChargingPoolId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParse_ChargingPoolId_Test()
        {

            var stationId = ChargingStation_Id.TryParse(ChargingPoolId, "5678");
            ClassicAssert.IsNotNull(stationId);

            if (stationId is not null)
            {
                ClassicAssert.AreEqual("DE*GEF*S1234*5678", stationId.Value.ToString());
                ClassicAssert.AreEqual(17,                  stationId.Value.Length);
            }

        }

        #endregion


        #region TryParseOut_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParseOut_ChargingStationOperatorId_Test()
        {
            ClassicAssert.IsTrue(ChargingStation_Id.TryParse(ChargingStationOperatorId, "1234", out var stationId));
            ClassicAssert.AreEqual("DE*GEF*S1234", stationId.ToString());
            ClassicAssert.AreEqual(12,             stationId.Length);
        }

        #endregion

        #region TryParseOut_ChargingPoolId_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParseOut_ChargingPoolId_Test()
        {
            ClassicAssert.IsTrue(ChargingStation_Id.TryParse(ChargingPoolId, "5678", out var stationId));
            ClassicAssert.AreEqual("DE*GEF*S1234*5678", stationId.ToString());
            ClassicAssert.AreEqual(17,                  stationId.Length);
        }

        #endregion


        #region Parse_Small_S_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void Parse_Small_S_Test()
        {
            Assert.Throws<ArgumentException>(() => { var evseId = ChargingStation_Id.Parse("DE*GEF*station*1234*5678"); });
        }

        #endregion

        #region TryParse_Small_S_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParse_Small_S_Test()
        {
            ClassicAssert.IsNull(ChargingStation_Id.TryParse("DE*GEF*station*1234*5678"));
        }

        #endregion

        #region TryParseOut_Small_S_Test()

        /// <summary>
        /// A test for parsing charging station identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Small_S_Test()
        {
            ClassicAssert.IsFalse(ChargingStation_Id.TryParse("DE*GEF*station*1234*5678", out _));
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging station identifications.
        /// </summary>
        [Test]
        public void Clone_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingStationOperatorId, "5678");
            var stationId2 = stationId1.Clone();
            ClassicAssert.AreEqual(stationId1.ToString(), stationId2.ToString());
            ClassicAssert.AreEqual(stationId1.Length, stationId2.Length);
            ClassicAssert.AreEqual(stationId1, stationId2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsTrue(stationId == stationId);
            #pragma warning restore
        }

        #endregion

        #region op_Equality_Equals_Test()

        /// <summary>
        /// A test for the equality operator equals.
        /// </summary>
        [Test]
        public void op_Equality_Equals_Test()
        {

            var stationId1a = ChargingStation_Id.Parse(ChargingPoolId, "1111");
            var stationId2a = ChargingStation_Id.Parse(ChargingPoolId, "1111");
            ClassicAssert.IsTrue(stationId1a == stationId2a);

            var stationId1b = ChargingStation_Id.Parse(ChargingPoolId, "aaaa");
            var stationId2b = ChargingStation_Id.Parse(ChargingPoolId, "aaaa");
            ClassicAssert.IsTrue(stationId1b == stationId2b);

            var stationId1c = ChargingStation_Id.Parse(ChargingPoolId, "AAAA");
            var stationId2c = ChargingStation_Id.Parse(ChargingPoolId, "AAAA");
            ClassicAssert.IsTrue(stationId1c == stationId2c);

            var stationId1d = ChargingStation_Id.Parse(ChargingPoolId, "aaaa");
            var stationId2d = ChargingStation_Id.Parse(ChargingPoolId, "AAAA");
            ClassicAssert.IsTrue(stationId1d == stationId2d);

            var stationId1e = ChargingStation_Id.Parse("DE*GEF*STATION*abcd*1234");
            var stationId2e = ChargingStation_Id.Parse("De*GeF*StAtIoN*ABCD*1234");
            ClassicAssert.IsTrue(stationId1e == stationId2e);

        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "8765");
            ClassicAssert.IsFalse(stationId1 == stationId2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsFalse(stationId != stationId);
            #pragma warning restore
        }

        #endregion

        #region op_Inequality_Equals_Test()

        /// <summary>
        /// A test for the inequality operator equals.
        /// </summary>
        [Test]
        public void op_Inequality_Equals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            ClassicAssert.IsFalse(stationId1 != stationId2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsTrue(stationId1 != stationId2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsTrue(stationId1 != stationId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsFalse(stationId < stationId);
            #pragma warning restore
        }

        #endregion

        #region op_Smaller_Equals_Test()

        /// <summary>
        /// A test for the smaller operator equals.
        /// </summary>
        [Test]
        public void op_Smaller_Equals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsFalse(stationId1 < stationId2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsTrue(stationId1 < stationId2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsTrue(stationId1 < stationId2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsFalse(stationId1 < stationId2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            ClassicAssert.IsFalse(stationId1 < stationId2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsTrue(stationId <= stationId);
            #pragma warning restore
        }

        #endregion

        #region op_SmallerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Equals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            ClassicAssert.IsTrue(stationId1 <= stationId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsTrue(stationId1 <= stationId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsTrue(stationId1 <= stationId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsFalse(stationId1 <= stationId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            ClassicAssert.IsFalse(stationId1 <= stationId2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsFalse(stationId > stationId);
            #pragma warning restore
        }

        #endregion

        #region op_Bigger_Equals_Test()

        /// <summary>
        /// A test for the bigger operator equals.
        /// </summary>
        [Test]
        public void op_Bigger_Equals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsFalse(stationId1 > stationId2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsFalse(stationId1 > stationId2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsFalse(stationId1 > stationId2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsTrue(stationId1 > stationId2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            ClassicAssert.IsTrue(stationId1 > stationId2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            #pragma warning disable
            ClassicAssert.IsTrue(stationId >= stationId);
            #pragma warning restore
        }

        #endregion

        #region op_BiggerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Equals_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            ClassicAssert.IsTrue(stationId1 >= stationId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsFalse(stationId1 >= stationId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsFalse(stationId1 >= stationId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsTrue(stationId1 >= stationId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            ClassicAssert.IsTrue(stationId1 >= stationId2);
        }

        #endregion


        #region CompareToNonChargingStationIdTest()

        /// <summary>
        /// A test for CompareTo a non-ChargingStation_Id.
        /// </summary>
        [Test]
        public void CompareToNonChargingStationIdTest()
        {

            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var text      = "DE*GEF*S1234*5678";

            Assert.Throws<ArgumentException>(() => { var x = stationId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsTrue(stationId1.CompareTo(stationId2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "005");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "023");
            ClassicAssert.IsTrue(stationId1.CompareTo(stationId2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsTrue(stationId1.CompareTo(stationId2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsTrue(stationId1.CompareTo(stationId2) > 0);
        }

        #endregion


        #region EqualsNonChargingStation_IdTest()

        /// <summary>
        /// A test for equals a non-ChargingStation_Id.
        /// </summary>
        [Test]
        public void EqualsNonChargingStation_IdTest()
        {
            var stationId = ChargingStation_Id.Parse(ChargingPoolId, "5678");
            var text      = "DE*GEF*S1234*5678";
            ClassicAssert.IsFalse(stationId.Equals(text));
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            ClassicAssert.IsTrue(stationId1.Equals(stationId2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var stationId1 = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var stationId2 = ChargingStation_Id.Parse(ChargingPoolId, "222");
            ClassicAssert.IsFalse(stationId1.Equals(stationId2));
        }

        #endregion

        #region OptionalEquals_Test()

        /// <summary>
        /// Test the equality of charging station identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void OptionalEquals_Test()
        {
            ClassicAssert.IsTrue(ChargingStation_Id.Parse("DE*GEF*S1234*AAAA")   == ChargingStation_Id.Parse("DEGEFS1234AAAA"));
            ClassicAssert.IsTrue(ChargingStation_Id.Parse("DE*GEF*S12*34*AA*AA") == ChargingStation_Id.Parse("DEGEFS1234AAAA"));
        }

        #endregion


        #region GetHashCode_Equals_Test()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCode_Equals_Test()
        {
            var hashCode1 = ChargingStation_Id.Parse(ChargingPoolId, "555").GetHashCode();
            var hashCode2 = ChargingStation_Id.Parse(ChargingPoolId, "555").GetHashCode();
            ClassicAssert.AreEqual(hashCode1, hashCode2);
        }

        #endregion

        #region GetHashCode_NotEquals_Test()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCode_NotEquals_Test()
        {
            var hashCode1 = ChargingStation_Id.Parse(ChargingPoolId, "001").GetHashCode();
            var hashCode2 = ChargingStation_Id.Parse(ChargingPoolId, "002").GetHashCode();
            ClassicAssert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion

        #region GetHashCode_OptionalEquals_Test()

        /// <summary>
        /// Test the equality of charging station identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void GetHashCode_OptionalEquals_Test()
        {
            var hashCode1 = ChargingStation_Id.Parse("DE*GEF*S1234*AAAA").GetHashCode();
            var hashCode2 = ChargingStation_Id.Parse("DEGEFS1234AAAA").   GetHashCode();
            ClassicAssert.AreEqual(hashCode1, hashCode2);
        }

        #endregion


        #region ChargingStationIdsAndNUnitTest()

        /// <summary>
        /// Tests ChargingStation_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void ChargingStationIdsAndNUnitTest()
        {

            var a = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var b = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var c = ChargingStation_Id.Parse(ChargingPoolId, "111");

            ClassicAssert.AreEqual(a, a);
            ClassicAssert.AreEqual(b, b);
            ClassicAssert.AreEqual(c, c);

            ClassicAssert.AreEqual(a, c);
            ClassicAssert.AreNotEqual(a, b);
            ClassicAssert.AreNotEqual(b, c);

        }

        #endregion

        #region ChargingStationIdsInHashSetTest()

        /// <summary>
        /// Test ChargingStation_Ids within a HashSet.
        /// </summary>
        [Test]
        public void ChargingStationIdsInHashSetTest()
        {

            var a = ChargingStation_Id.Parse(ChargingPoolId, "111");
            var b = ChargingStation_Id.Parse(ChargingPoolId, "222");
            var c = ChargingStation_Id.Parse(ChargingPoolId, "111");

            var _HashSet = new HashSet<ChargingStation_Id>();
            ClassicAssert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            ClassicAssert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            ClassicAssert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            ClassicAssert.AreEqual(2, _HashSet.Count);

        }

        #endregion


        #region ChargingPoolId_CreateStationId()

        /// <summary>
        /// Test charging station identification generated from a charging pool identification.
        /// </summary>
        [Test]
        public void ChargingPoolId_CreateStationId()
        {

            ClassicAssert.AreEqual("DE*GEF*S1234*AAAA",        ChargingPoolId.                           CreateStationId("AAAA").ToString());
            ClassicAssert.AreEqual("DEGEFS1234AAAA",           ChargingPool_Id.Parse("DEGEFP1234").      CreateStationId("AAAA").ToString());
            ClassicAssert.AreEqual("DE*GEF*STATION*0001*AAAA", ChargingPool_Id.Parse("DE*GEF*POOL*0001").CreateStationId("AAAA").ToString());

            ClassicAssert.AreEqual("DE*GEF*S1234",             ChargingPoolId.                           CreateStationId().ToString());
            ClassicAssert.AreEqual("DEGEFS1234",               ChargingPool_Id.Parse("DEGEFP1234").      CreateStationId().ToString());
            ClassicAssert.AreEqual("DE*GEF*STATION*0001",      ChargingPool_Id.Parse("DE*GEF*POOL*0001").CreateStationId().ToString());

        }

        #endregion


    }

}
