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

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Unit tests for charging pool identifications.
    /// </summary>
    [TestFixture]
    public class ChargingPoolIdTests
    {

        private readonly ChargingStationOperator_Id ChargingStationOperatorId = ChargingStationOperator_Id.Parse("DE*GEF");


        #region Parse_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void Parse_ChargingStationOperatorId_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.That(poolId.ToString(), Is.EqualTo("DE*GEF*P1234"));
            Assert.That(poolId.Length, Is.EqualTo(12));
        }

        #endregion

        #region TryParse_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void TryParse_ChargingStationOperatorId_Test()
        {

            var poolId = ChargingPool_Id.TryParse(ChargingStationOperatorId, "1234");
            Assert.That(poolId, Is.Not.Null);

            if (poolId is not null)
            {
                Assert.That(poolId.Value.ToString(), Is.EqualTo("DE*GEF*P1234"));
                Assert.That(poolId.Value.Length, Is.EqualTo(12));
            }

        }

        #endregion

        #region TryParseOut_ChargingStationOperatorId_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void TryParseOut_ChargingStationOperatorId_Test()
        {
            Assert.That(ChargingPool_Id.TryParse(ChargingStationOperatorId, "1234", out var poolId), Is.True);
            Assert.That(poolId.ToString(), Is.EqualTo("DE*GEF*P1234"));
            Assert.That(poolId.Length, Is.EqualTo(12));
        }

        #endregion


        #region Parse_Small_P_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void Parse_Small_P_Test()
        {
            Assert.Throws<ArgumentException>(() => { var evseId = ChargingStation_Id.Parse("DE*GEF*pool*1234"); });
        }

        #endregion

        #region TryParse_Small_P_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void TryParse_Small_P_Test()
        {
            Assert.That(ChargingStation_Id.TryParse("DE*GEF*pool*1234"), Is.Null);
        }

        #endregion

        #region TryParseOut_Small_P_Test()

        /// <summary>
        /// A test for parsing charging pool identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Small_P_Test()
        {
            Assert.That(ChargingStation_Id.TryParse("DE*GEF*pool*1234", out _), Is.False);
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging pool identifications.
        /// </summary>
        [Test]
        public void Clone_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "5678");
            var poolId2 = poolId1.Clone();
            Assert.That(poolId2.ToString(), Is.EqualTo(poolId1.ToString()));
            Assert.That(poolId2.Length, Is.EqualTo(poolId1.Length));
            Assert.That(poolId2, Is.EqualTo(poolId1));
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId == poolId, Is.True);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.That(poolId1 == poolId2, Is.True);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "234");
            Assert.That(poolId1 == poolId2, Is.False);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId != poolId, Is.False);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.That(poolId1 != poolId2, Is.False);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1 != poolId2, Is.True);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1 != poolId2, Is.True);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId < poolId, Is.False);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 < poolId2, Is.False);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1 < poolId2, Is.True);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1 < poolId2, Is.True);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 < poolId2, Is.False);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            Assert.That(poolId1 < poolId2, Is.False);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId <= poolId, Is.True);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.That(poolId1 <= poolId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1 <= poolId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1 <= poolId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 <= poolId2, Is.False);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            Assert.That(poolId1 <= poolId2, Is.False);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId > poolId, Is.False);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 > poolId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1 > poolId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1 > poolId2, Is.False);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 > poolId2, Is.True);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            Assert.That(poolId1 > poolId2, Is.True);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.That(poolId >= poolId, Is.True);
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
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.That(poolId1 >= poolId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1 >= poolId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1 >= poolId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1 >= poolId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            Assert.That(poolId1 >= poolId2, Is.True);
        }

        #endregion


        #region CompareToNonChargingPool_IdTest()

        /// <summary>
        /// A test for CompareTo a non-ChargingPool_Id.
        /// </summary>
        [Test]
        public void CompareToNonChargingPool_IdTest()
        {

            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var text   = "DE*GEF*P1234";

            Assert.Throws<ArgumentException>(() => { var x = poolId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1.CompareTo(poolId2) < 0, Is.True);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "023");
            Assert.That(poolId1.CompareTo(poolId2) < 0, Is.True);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1.CompareTo(poolId2) == 0, Is.True);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1.CompareTo(poolId2) > 0, Is.True);
        }

        #endregion


        #region EqualsNonChargingPool_IdTest()

        /// <summary>
        /// A test for equals a non-ChargingPool_Id.
        /// </summary>
        [Test]
        public void EqualsNonChargingPool_IdTest()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var text   = "DE*GEF*P1234";
            Assert.That(poolId.Equals(text), Is.False);
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            Assert.That(poolId1.Equals(poolId2), Is.True);
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            Assert.That(poolId1.Equals(poolId2), Is.False);
        }

        #endregion

        #region OptionalEquals_Test()

        /// <summary>
        /// Test the equality of charging pool identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void OptionalEquals_Test()
        {
            Assert.That(ChargingPool_Id.Parse("DE*GEF*P1234")  == ChargingPool_Id.Parse("DEGEFP1234"), Is.True);
            Assert.That(ChargingPool_Id.Parse("DE*GEF*P12*34") == ChargingPool_Id.Parse("DEGEFP1234"), Is.True);
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "555").GetHashCode();
            var hashCode2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "555").GetHashCode();
            Assert.That(hashCode2, Is.EqualTo(hashCode1));
        }

        #endregion

        #region GetHashCodeNotEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeNotEqualTest()
        {
            var hashCode1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "001").GetHashCode();
            var hashCode2 = ChargingPool_Id.Parse(ChargingStationOperatorId, "002").GetHashCode();
            Assert.That(hashCode2, Is.Not.EqualTo(hashCode1));
        }

        #endregion

        #region GetHashCode_OptionalEquals_Test()

        /// <summary>
        /// Test the equality of charging station identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void GetHashCode_OptionalEquals_Test()
        {
            var hashCode1 = ChargingPool_Id.Parse("DE*GEF*P1234").GetHashCode();
            var hashCode2 = ChargingPool_Id.Parse("DEGEFP1234").  GetHashCode();
            Assert.That(hashCode2, Is.EqualTo(hashCode1));
        }

        #endregion


        #region ChargingPool_IdsAndNUnitTest()

        /// <summary>
        /// Tests ChargingPool_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void ChargingPool_IdsAndNUnitTest()
        {

            var a = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var b = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var c = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");

            // Each id equals itself: what Equals is asked here, on purpose.
#pragma warning disable NUnit2009
            Assert.That(a, Is.EqualTo(a));
            Assert.That(b, Is.EqualTo(b));
            Assert.That(c, Is.EqualTo(c));
#pragma warning restore NUnit2009

            Assert.That(c, Is.EqualTo(a));
            Assert.That(b, Is.Not.EqualTo(a));
            Assert.That(c, Is.Not.EqualTo(b));

        }

        #endregion

        #region ChargingPool_IdsInHashSetTest()

        /// <summary>
        /// Test ChargingPool_Ids within a HashSet.
        /// </summary>
        [Test]
        public void ChargingPool_IdsInHashSetTest()
        {

            var a = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");
            var b = ChargingPool_Id.Parse(ChargingStationOperatorId, "222");
            var c = ChargingPool_Id.Parse(ChargingStationOperatorId, "111");

            var _HashSet = new HashSet<ChargingPool_Id>();
            Assert.That(_HashSet.Count, Is.EqualTo(0));

            _HashSet.Add(a);
            Assert.That(_HashSet.Count, Is.EqualTo(1));

            _HashSet.Add(b);
            Assert.That(_HashSet.Count, Is.EqualTo(2));

            _HashSet.Add(c);
            Assert.That(_HashSet.Count, Is.EqualTo(2));

        }

        #endregion


        #region ChargingStationOperatorId_CreateChargingPoolId()

        /// <summary>
        /// Test charging pool identification generated from a charging station operator identification.
        /// </summary>
        [Test]
        public void ChargingStationOperatorId_CreateChargingPoolId()
        {
            Assert.That(ChargingStationOperator_Id.Parse("DEGEF").CreatePoolId("1234").ToString(), Is.EqualTo("DEGEFP1234"));
            Assert.That(ChargingStationOperator_Id.Parse("DE*GEF").CreatePoolId("1234").ToString(), Is.EqualTo("DE*GEF*P1234"));
        }

        #endregion


    }

}
