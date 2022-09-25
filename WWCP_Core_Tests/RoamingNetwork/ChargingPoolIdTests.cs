﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.tests.roamingNetwork
{

    /// <summary>
    /// Unit tests for charging pool identifications.
    /// </summary>
    [TestFixture]
    public class ChargingPoolIdTests
    {

        private readonly ChargingStationOperator_Id ChargingStationOperatorId = ChargingStationOperator_Id.Parse("DE*GEF");


        #region ChargingPool_IdStringConstructorTest()

        /// <summary>
        /// A test for the ChargingPool_Id string constructor.
        /// </summary>
        [Test]
        public void ChargingPool_IdStringConstructorTest()
        {
            var poolId = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            Assert.AreEqual("DE*GEF*P1234", poolId.ToString());
            Assert.AreEqual(12,             poolId.Length);
        }

        #endregion

        #region ChargingPool_IdChargingPool_IdConstructorTest()

        /// <summary>
        /// A test for the ChargingPool_Id ChargingPool_Id constructor.
        /// </summary>
        [Test]
        public void ChargingPool_IdChargingPool_IdConstructorTest()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var poolId2 = poolId1.Clone;
            Assert.AreEqual(poolId1.ToString(), poolId2.ToString());
            Assert.AreEqual(poolId1.Length,     poolId2.Length);
            Assert.AreEqual(poolId1,            poolId2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsTrue(poolId1 == poolId1);
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
            Assert.IsTrue(poolId1 == poolId2);
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
            Assert.IsFalse(poolId1 == poolId2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsFalse(poolId1 != poolId1);
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
            Assert.IsFalse(poolId1 != poolId2);
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
            Assert.IsTrue(poolId1 != poolId2);
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
            Assert.IsTrue(poolId1 != poolId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsFalse(poolId1 < poolId1);
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
            Assert.IsFalse(poolId1 < poolId2);
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
            Assert.IsTrue(poolId1 < poolId2);
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
            Assert.IsTrue(poolId1 < poolId2);
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
            Assert.IsFalse(poolId1 < poolId2);
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
            Assert.IsFalse(poolId1 < poolId2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsTrue(poolId1 <= poolId1);
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
            Assert.IsTrue(poolId1 <= poolId2);
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
            Assert.IsTrue(poolId1 <= poolId2);
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
            Assert.IsTrue(poolId1 <= poolId2);
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
            Assert.IsFalse(poolId1 <= poolId2);
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
            Assert.IsFalse(poolId1 <= poolId2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsFalse(poolId1 > poolId1);
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
            Assert.IsFalse(poolId1 > poolId2);
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
            Assert.IsFalse(poolId1 > poolId2);
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
            Assert.IsFalse(poolId1 > poolId2);
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
            Assert.IsTrue(poolId1 > poolId2);
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
            Assert.IsTrue(poolId1 > poolId2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var poolId1 = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            #pragma warning disable
            Assert.IsTrue(poolId1 >= poolId1);
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
            Assert.IsTrue(poolId1 >= poolId2);
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
            Assert.IsFalse(poolId1 >= poolId2);
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
            Assert.IsFalse(poolId1 >= poolId2);
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
            Assert.IsTrue(poolId1 >= poolId2);
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
            Assert.IsTrue(poolId1 >= poolId2);
        }

        #endregion


        #region CompareToNonChargingPool_IdTest()

        /// <summary>
        /// A test for CompareTo a non-ChargingPool_Id.
        /// </summary>
        [Test]
        public void CompareToNonChargingPool_IdTest()
        {

            var poolId  = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var text   = "DE*GEF";

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
            Assert.IsTrue(poolId1.CompareTo(poolId2) < 0);
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
            Assert.IsTrue(poolId1.CompareTo(poolId2) < 0);
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
            Assert.IsTrue(poolId1.CompareTo(poolId2) == 0);
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
            Assert.IsTrue(poolId1.CompareTo(poolId2) > 0);
        }

        #endregion


        #region EqualsNonChargingPool_IdTest()

        /// <summary>
        /// A test for equals a non-ChargingPool_Id.
        /// </summary>
        [Test]
        public void EqualsNonChargingPool_IdTest()
        {
            var poolId  = ChargingPool_Id.Parse(ChargingStationOperatorId, "1234");
            var text   = "DE*GEF";
            Assert.IsFalse(poolId.Equals(text));
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
            Assert.IsTrue(poolId1.Equals(poolId2));
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
            Assert.IsFalse(poolId1.Equals(poolId2));
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
            Assert.AreEqual(hashCode1, hashCode2);
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
            Assert.AreNotEqual(hashCode1, hashCode2);
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

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

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
            Assert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            Assert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            Assert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            Assert.AreEqual(2, _HashSet.Count);

        }

        #endregion


        #region ChargingPoolId_OptionalEquals()

        /// <summary>
        /// Test the equality of charging pool identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void ChargingPoolId_OptionalEquals()
        {

            Assert.IsTrue(ChargingPool_Id.Parse("DE*GEF*P1234*AAAA") == ChargingPool_Id.Parse("DEGEFP1234AAAA"));

        }

        #endregion

    }

}
