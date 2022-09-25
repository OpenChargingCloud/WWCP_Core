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
    /// Unit tests for roaming network identifications.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkIdTests
    {

        #region RoamingNetworkIdStringConstructorTest()

        /// <summary>
        /// A test for the RoamingNetworkId string constructor.
        /// </summary>
        [Test]
        public void RoamingNetworkIdStringConstructorTest()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            Assert.AreEqual("TEST", roamingNetworkId.ToString());
            Assert.AreEqual(4,      roamingNetworkId.Length);
        }

        #endregion

        #region RoamingNetworkIdRoamingNetworkIdConstructorTest()

        /// <summary>
        /// A test for the RoamingNetworkId RoamingNetworkId constructor.
        /// </summary>
        [Test]
        public void RoamingNetworkIdRoamingNetworkIdConstructorTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = roamingNetworkId1.Clone;
            Assert.AreEqual(roamingNetworkId1.ToString(), roamingNetworkId2.ToString());
            Assert.AreEqual(roamingNetworkId1.Length,     roamingNetworkId2.Length);
            Assert.AreEqual(roamingNetworkId1,            roamingNetworkId2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsTrue(roamingNetworkId1 == roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.IsTrue(roamingNetworkId1 == roamingNetworkId2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("234");
            Assert.IsFalse(roamingNetworkId1 == roamingNetworkId2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsFalse(roamingNetworkId1 != roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.IsFalse(roamingNetworkId1 != roamingNetworkId2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsTrue(roamingNetworkId1 != roamingNetworkId2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsTrue(roamingNetworkId1 != roamingNetworkId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsFalse(roamingNetworkId1 < roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsTrue(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsTrue(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsTrue(roamingNetworkId1 <= roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsFalse(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.IsFalse(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsFalse(roamingNetworkId1 > roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsTrue(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.IsTrue(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.IsTrue(roamingNetworkId1 >= roamingNetworkId1);
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
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsFalse(roamingNetworkId1 >= roamingNetworkId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsFalse(roamingNetworkId1 >= roamingNetworkId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
        }

        #endregion


        #region CompareToNonRoamingNetworkIdTest()

        /// <summary>
        /// A test for CompareTo a non-RoamingNetworkId.
        /// </summary>
        [Test]
        public void CompareToNonRoamingNetworkIdTest()
        {

            var roamingNetworkId  = RoamingNetwork_Id.Parse("TEST");
            var text              = "TEST";

            Assert.Throws<ArgumentException>(() => { var x = roamingNetworkId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) > 0);
        }

        #endregion


        #region EqualsNonRoamingNetworkIdTest()

        /// <summary>
        /// A test for equals a non-RoamingNetworkId.
        /// </summary>
        [Test]
        public void EqualsNonRoamingNetworkIdTest()
        {
            var roamingNetworkId  = RoamingNetwork_Id.Parse("TEST");
            var text              = "TEST";
            Assert.IsFalse(roamingNetworkId.Equals(text));
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.IsTrue(roamingNetworkId1.Equals(roamingNetworkId2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.IsFalse(roamingNetworkId1.Equals(roamingNetworkId2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = RoamingNetwork_Id.Parse("555").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("555").GetHashCode();
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
            var hashCode1 = RoamingNetwork_Id.Parse("001").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("002").GetHashCode();
            Assert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion


        #region RoamingNetworkIdsAndNUnitTest()

        /// <summary>
        /// Tests RoamingNetworkIds in combination with NUnit.
        /// </summary>
        [Test]
        public void RoamingNetworkIdsAndNUnitTest()
        {

            var a = RoamingNetwork_Id.Parse("111");
            var b = RoamingNetwork_Id.Parse("222");
            var c = RoamingNetwork_Id.Parse("111");

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

        }

        #endregion

        #region RoamingNetworkIdsInHashSetTest()

        /// <summary>
        /// Test RoamingNetworkIds within a HashSet.
        /// </summary>
        [Test]
        public void RoamingNetworkIdsInHashSetTest()
        {

            var a = RoamingNetwork_Id.Parse("111");
            var b = RoamingNetwork_Id.Parse("222");
            var c = RoamingNetwork_Id.Parse("111");

            var _HashSet = new HashSet<RoamingNetwork_Id>();
            Assert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            Assert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            Assert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            Assert.AreEqual(2, _HashSet.Count);

        }

        #endregion

    }

}
