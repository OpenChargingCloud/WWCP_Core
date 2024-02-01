/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Unit tests for roaming network identifications.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkIdTests
    {

        #region Parse_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void Parse_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            ClassicAssert.AreEqual("TEST", roamingNetworkId.ToString());
            ClassicAssert.AreEqual(4,      roamingNetworkId.Length);
        }

        #endregion

        #region TryParse_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void TryParse_Test()
        {

            var roamingNetworkId = RoamingNetwork_Id.TryParse("TEST");
            ClassicAssert.IsNotNull(roamingNetworkId);

            if (roamingNetworkId is not null)
            {
                ClassicAssert.AreEqual("TEST", roamingNetworkId.Value.ToString());
                ClassicAssert.AreEqual(4,      roamingNetworkId.Value.Length);
            }

        }

        #endregion

        #region TryParseOut_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test()
        {
            ClassicAssert.IsTrue(RoamingNetwork_Id.TryParse("TEST", out var roamingNetworkId));
            ClassicAssert.AreEqual("TEST", roamingNetworkId.ToString());
            ClassicAssert.AreEqual(4,      roamingNetworkId.Length);
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging station operator identifications.
        /// </summary>
        [Test]
        public void Clone_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = roamingNetworkId1.Clone;
            ClassicAssert.AreEqual(roamingNetworkId1.ToString(), roamingNetworkId2.ToString());
            ClassicAssert.AreEqual(roamingNetworkId1.Length,     roamingNetworkId2.Length);
            ClassicAssert.AreEqual(roamingNetworkId1,            roamingNetworkId2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsTrue(roamingNetworkId == roamingNetworkId);
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
            ClassicAssert.IsTrue(roamingNetworkId1 == roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 == roamingNetworkId2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsFalse(roamingNetworkId != roamingNetworkId);
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
            ClassicAssert.IsFalse(roamingNetworkId1 != roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 != roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 != roamingNetworkId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsFalse(roamingNetworkId < roamingNetworkId);
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
            ClassicAssert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 < roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 < roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 < roamingNetworkId2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsTrue(roamingNetworkId <= roamingNetworkId);
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
            ClassicAssert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 <= roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 <= roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 <= roamingNetworkId2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsFalse(roamingNetworkId > roamingNetworkId);
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
            ClassicAssert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 > roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 > roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 > roamingNetworkId2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            ClassicAssert.IsTrue(roamingNetworkId >= roamingNetworkId);
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
            ClassicAssert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 >= roamingNetworkId2);
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
            ClassicAssert.IsFalse(roamingNetworkId1 >= roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1 >= roamingNetworkId2);
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
            ClassicAssert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0);
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
            ClassicAssert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0);
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
            ClassicAssert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) == 0);
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
            ClassicAssert.IsTrue(roamingNetworkId1.CompareTo(roamingNetworkId2) > 0);
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
            ClassicAssert.IsFalse(roamingNetworkId.Equals(text));
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
            ClassicAssert.IsTrue(roamingNetworkId1.Equals(roamingNetworkId2));
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
            ClassicAssert.IsFalse(roamingNetworkId1.Equals(roamingNetworkId2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = RoamingNetwork_Id.Parse("TEST").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("TEST").GetHashCode();
            ClassicAssert.AreEqual(hashCode1, hashCode2);
        }

        #endregion

        #region GetHashCodeNotEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeNotEqualTest()
        {
            var hashCode1 = RoamingNetwork_Id.Parse("TEST1").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("TEST2").GetHashCode();
            ClassicAssert.AreNotEqual(hashCode1, hashCode2);
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

            ClassicAssert.AreEqual(a, a);
            ClassicAssert.AreEqual(b, b);
            ClassicAssert.AreEqual(c, c);

            ClassicAssert.AreEqual(a, c);
            ClassicAssert.AreNotEqual(a, b);
            ClassicAssert.AreNotEqual(b, c);

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
            ClassicAssert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            ClassicAssert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            ClassicAssert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            ClassicAssert.AreEqual(2, _HashSet.Count);

        }

        #endregion


    }

}
