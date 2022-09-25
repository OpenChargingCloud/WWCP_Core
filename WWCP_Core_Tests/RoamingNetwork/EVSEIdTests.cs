/*
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
    /// Unit tests for the EVSE identifications.
    /// </summary>
    [TestFixture]
    public class EVSEIdTests
    {


        private readonly ChargingStationOperator_Id ChargingStationOperatorId  = ChargingStationOperator_Id.Parse("DE*GEF");
        private readonly ChargingPool_Id            ChargingPoolId             = ChargingPool_Id.           Parse("DE*GEF*P1234");
        private readonly ChargingStation_Id         ChargingStationId          = ChargingStation_Id.        Parse("DE*GEF*S1234*5678");


        #region EVSEId_StringConstructorTest1()

        /// <summary>
        /// A test for the EVSE_Id string constructor.
        /// </summary>
        [Test]
        public void EVSEId_StringConstructorTest1()
        {
            var evseId = EVSE_Id.Parse(ChargingStationOperatorId, "9012");
            Assert.AreEqual("DE*GEF*E9012", evseId.ToString());
            Assert.AreEqual(12,             evseId.Length);
        }

        #endregion

        #region EVSEId_StringConstructorTest2()

        /// <summary>
        /// A test for the EVSE_Id string constructor.
        /// </summary>
        [Test]
        public void EVSEId_StringConstructorTest2()
        {
            var evseId = EVSE_Id.Parse(ChargingPoolId, "9012");
            Assert.AreEqual("DE*GEF*E1234*9012", evseId.ToString());
            Assert.AreEqual(17,                  evseId.Length);
        }

        #endregion

        #region EVSEId_StringConstructorTest3()

        /// <summary>
        /// A test for the EVSE_Id string constructor.
        /// </summary>
        [Test]
        public void EVSEId_StringConstructorTest3()
        {
            var evseId = EVSE_Id.Parse(ChargingStationId, "9012");
            Assert.AreEqual("DE*GEF*E1234*5678*9012", evseId.ToString());
            Assert.AreEqual(22,                       evseId.Length);
        }

        #endregion


        #region op_Equality_Equals_Test()

        /// <summary>
        /// A test for the equality operator equals.
        /// </summary>
        [Test]
        public void op_Equality_Equals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1 == evseId2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsFalse(evseId1 == evseId2);
        }

        #endregion


        #region op_Inequality_Equals_Test()

        /// <summary>
        /// A test for the inequality operator equals.
        /// </summary>
        [Test]
        public void op_Inequality_Equals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsFalse(evseId1 != evseId2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsTrue(evseId1 != evseId2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "5");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "23");
            Assert.IsTrue(evseId1 != evseId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var poolId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1234");
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
            var poolId1 = EVSE_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = EVSE_Id.Parse(ChargingStationOperatorId, "111");
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
            var poolId1 = EVSE_Id.Parse(ChargingStationOperatorId, "111");
            var poolId2 = EVSE_Id.Parse(ChargingStationOperatorId, "222");
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
            var poolId1 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            var poolId2 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
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
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsFalse(evseId1 < evseId2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            Assert.IsFalse(evseId1 < evseId2);
        }

        #endregion


        #region op_SmallerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Equals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1 <= evseId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsTrue(evseId1 <= evseId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            Assert.IsTrue(evseId1 <= evseId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsFalse(evseId1 <= evseId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            Assert.IsFalse(evseId1 <= evseId2);
        }

        #endregion


        #region op_Bigger_Equals_Test()

        /// <summary>
        /// A test for the bigger operator equals.
        /// </summary>
        [Test]
        public void op_Bigger_Equals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsFalse(evseId1 > evseId2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsFalse(evseId1 > evseId2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            Assert.IsFalse(evseId1 > evseId2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1 > evseId2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            Assert.IsTrue(evseId1 > evseId2);
        }

        #endregion


        #region op_BiggerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Equals_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1 >= evseId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsFalse(evseId1 >= evseId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            Assert.IsFalse(evseId1 >= evseId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1 >= evseId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            Assert.IsTrue(evseId1 >= evseId2);
        }

        #endregion


        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsTrue(evseId1.CompareTo(evseId2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "005");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "023");
            Assert.IsTrue(evseId1.CompareTo(evseId2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1.CompareTo(evseId2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1.CompareTo(evseId2) > 0);
        }

        #endregion


        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            Assert.IsTrue(evseId1.Equals(evseId2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var evseId1 = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var evseId2 = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            Assert.IsFalse(evseId1.Equals(evseId2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = EVSE_Id.Parse(ChargingStationOperatorId, "5").GetHashCode();
            var hashCode2 = EVSE_Id.Parse(ChargingStationOperatorId, "5").GetHashCode();
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
            var hashCode1 = EVSE_Id.Parse(ChargingStationOperatorId, "1").GetHashCode();
            var hashCode2 = EVSE_Id.Parse(ChargingStationOperatorId, "2").GetHashCode();
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion


        #region EVSEIds_AndNUnitTest()

        /// <summary>
        /// Tests EVSE_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void EVSEIds_AndNUnitTest()
        {

            var a = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var b = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var c = EVSE_Id.Parse(ChargingStationOperatorId, "1");

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

        }

        #endregion

        #region EVSEIds_InHashSetTest()

        /// <summary>
        /// Test EVSE_Ids within a HashSet.
        /// </summary>
        [Test]
        public void EVSEIds_InHashSetTest()
        {

            var a = EVSE_Id.Parse(ChargingStationOperatorId, "1");
            var b = EVSE_Id.Parse(ChargingStationOperatorId, "2");
            var c = EVSE_Id.Parse(ChargingStationOperatorId, "1");

            var _HashSet = new HashSet<EVSE_Id>();
            Assert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            Assert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            Assert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            Assert.AreEqual(2, _HashSet.Count);

        }

        #endregion


        #region EVSEId_FromStationId()

        /// <summary>
        /// Test EVSE identification generated from a charging station identification.
        /// </summary>
        [Test]
        public void EVSEId_FromStationId()
        {

            Assert.AreEqual("DE*GEF*E1234*5678*AAAA",     ChargingStationId.                                   CreateEVSEId("AAAA").ToString());
            Assert.AreEqual("DEGEFE12345678AAAA",         ChargingStation_Id.Parse("DEGEFS12345678").          CreateEVSEId("AAAA").ToString());
            Assert.AreEqual("DE*GEF*EVSE*1234*5678*AAAA", ChargingStation_Id.Parse("DE*GEF*STATION*1234*5678").CreateEVSEId("AAAA").ToString());
            Assert.AreEqual("DEGEFEVSE12345678AAAA",      ChargingStation_Id.Parse("DEGEFSTATION12345678").    CreateEVSEId("AAAA").ToString());

            Assert.AreEqual("DE*GEF*E1234*5678",          ChargingStationId.                                   CreateEVSEId().ToString());
            Assert.AreEqual("DEGEFE12345678",             ChargingStation_Id.Parse("DEGEFS12345678").          CreateEVSEId().ToString());
            Assert.AreEqual("DE*GEF*EVSE*1234*5678",      ChargingStation_Id.Parse("DE*GEF*STATION*1234*5678").CreateEVSEId().ToString());
            Assert.AreEqual("DEGEFEVSE12345678",          ChargingStation_Id.Parse("DEGEFSTATION12345678").    CreateEVSEId().ToString());

        }

        #endregion

        #region EVSEId_OptionalEquals()

        /// <summary>
        /// Test the equality of EVSE identifications having different formats/optional elements.
        /// </summary>
        [Test]
        public void EVSEId_OptionalEquals()
        {

            Assert.IsTrue(EVSE_Id.Parse("DE*GEF*E1234*AAAA") == EVSE_Id.Parse("DEGEFE1234AAAA"));

        }

        #endregion


    }

}
