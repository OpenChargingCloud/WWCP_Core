/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Unit tests for charging station operator identifications.
    /// </summary>
    [TestFixture]
    public class ChargingStationOperatorIdTests
    {

        #region Parse_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test1()
        {
            var csoId = ChargingStationOperator_Id.Parse("DEGEF");
            ClassicAssert.AreEqual("DEGEF", csoId.ToString());
            ClassicAssert.AreEqual(5,       csoId.Length);
        }

        #endregion

        #region Parse_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test2()
        {
            var csoId = ChargingStationOperator_Id.Parse("DE*GEF");
            ClassicAssert.AreEqual("DE*GEF", csoId.ToString());
            ClassicAssert.AreEqual(6,        csoId.Length);
        }

        #endregion

        #region Parse_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test3()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            ClassicAssert.AreEqual("DE*GEF", csoId.ToString());
            ClassicAssert.AreEqual(6,        csoId.Length);
        }

        #endregion


        #region TryParse_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test1()
        {

            var csoId = ChargingStationOperator_Id.TryParse("DEGEF");
            ClassicAssert.IsNotNull(csoId);

            if (csoId is not null)
            {
                ClassicAssert.AreEqual("DEGEF", csoId.Value.ToString());
                ClassicAssert.AreEqual(5,       csoId.Value.Length);
            }

        }

        #endregion

        #region TryParse_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test2()
        {

            var csoId = ChargingStationOperator_Id.TryParse("DE*GEF");
            ClassicAssert.IsNotNull(csoId);

            if (csoId is not null)
            {
                ClassicAssert.AreEqual("DE*GEF", csoId.Value.ToString());
                ClassicAssert.AreEqual(6,        csoId.Value.Length);
            }

        }

        #endregion

        #region TryParse_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test3()
        {

            var csoId = ChargingStationOperator_Id.TryParse(Country.Germany, "GEF");
            ClassicAssert.IsNotNull(csoId);

            if (csoId is not null)
            {
                ClassicAssert.AreEqual("DE*GEF", csoId.Value.ToString());
                ClassicAssert.AreEqual(6,        csoId.Value.Length);
            }

        }

        #endregion


        #region TryParseOut_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test1()
        {
            ClassicAssert.IsTrue(ChargingStationOperator_Id.TryParse("DEGEF", out var csoId));
            ClassicAssert.AreEqual("DEGEF", csoId.ToString());
            ClassicAssert.AreEqual(5,       csoId.Length);
        }

        #endregion

        #region TryParseOut_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test2()
        {
            ClassicAssert.IsTrue(ChargingStationOperator_Id.TryParse("DE*GEF", out var csoId));
            ClassicAssert.AreEqual("DE*GEF", csoId.ToString());
            ClassicAssert.AreEqual(6,        csoId.Length);
        }

        #endregion

        #region TryParseOut_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test3()
        {
            ClassicAssert.IsTrue(ChargingStationOperator_Id.TryParse(Country.Germany, "GEF", out var csoId));
            ClassicAssert.AreEqual("DE*GEF", csoId.ToString());
            ClassicAssert.AreEqual(6,        csoId.Length);
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging station operator identifications.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdChargingStationOperator_IdConstructorTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = csoId1.Clone();
            ClassicAssert.AreEqual(csoId1.ToString(), csoId2.ToString());
            ClassicAssert.AreEqual(csoId1.Length,     csoId2.Length);
            ClassicAssert.AreEqual(csoId1,            csoId2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsTrue(csoId == csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            ClassicAssert.IsTrue(csoId1 == csoId2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "234");
            ClassicAssert.IsFalse(csoId1 == csoId2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsFalse(csoId != csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            ClassicAssert.IsFalse(csoId1 != csoId2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsTrue(csoId1 != csoId2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsTrue(csoId1 != csoId2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsFalse(csoId < csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsFalse(csoId1 < csoId2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsTrue(csoId1 < csoId2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsTrue(csoId1 < csoId2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsFalse(csoId1 < csoId2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            ClassicAssert.IsFalse(csoId1 < csoId2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsTrue(csoId <= csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            ClassicAssert.IsTrue(csoId1 <= csoId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsTrue(csoId1 <= csoId2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsTrue(csoId1 <= csoId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsFalse(csoId1 <= csoId2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            ClassicAssert.IsFalse(csoId1 <= csoId2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsFalse(csoId > csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsFalse(csoId1 > csoId2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsFalse(csoId1 > csoId2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsFalse(csoId1 > csoId2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsTrue(csoId1 > csoId2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            ClassicAssert.IsTrue(csoId1 > csoId2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            ClassicAssert.IsTrue(csoId >= csoId);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            ClassicAssert.IsTrue(csoId1 >= csoId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsFalse(csoId1 >= csoId2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsFalse(csoId1 >= csoId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsTrue(csoId1 >= csoId2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            ClassicAssert.IsTrue(csoId1 >= csoId2);
        }

        #endregion


        #region CompareToNonChargingStationOperator_IdTest()

        /// <summary>
        /// A test for CompareTo a non-ChargingStationOperator_Id.
        /// </summary>
        [Test]
        public void CompareToNonChargingStationOperator_IdTest()
        {

            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var text  = "DE*GEF";

            Assert.Throws<ArgumentException>(() => { var x = csoId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsTrue(csoId1.CompareTo(csoId2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            ClassicAssert.IsTrue(csoId1.CompareTo(csoId2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsTrue(csoId1.CompareTo(csoId2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsTrue(csoId1.CompareTo(csoId2) > 0);
        }

        #endregion


        #region EqualsNonChargingStationOperator_IdTest()

        /// <summary>
        /// A test for equals a non-ChargingStationOperator_Id.
        /// </summary>
        [Test]
        public void EqualsNonChargingStationOperator_IdTest()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var text  = "DE*GEF";
            ClassicAssert.IsFalse(csoId.Equals(text));
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            ClassicAssert.IsTrue(csoId1.Equals(csoId2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            ClassicAssert.IsFalse(csoId1.Equals(csoId2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "555").GetHashCode();
            var hashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "555").GetHashCode();
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
            var hashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "001").GetHashCode();
            var hashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "002").GetHashCode();
            ClassicAssert.AreNotEqual(hashCode1, hashCode2);
        }

        #endregion


        #region ChargingStationOperator_IdsAndNUnitTest()

        /// <summary>
        /// Tests ChargingStationOperator_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdsAndNUnitTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "111");

            ClassicAssert.AreEqual(a, a);
            ClassicAssert.AreEqual(b, b);
            ClassicAssert.AreEqual(c, c);

            ClassicAssert.AreEqual(a, c);
            ClassicAssert.AreNotEqual(a, b);
            ClassicAssert.AreNotEqual(b, c);

        }

        #endregion

        #region ChargingStationOperator_IdsInHashSetTest()

        /// <summary>
        /// Test ChargingStationOperator_Ids within a HashSet.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdsInHashSetTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "111");

            var _HashSet = new HashSet<ChargingStationOperator_Id>();
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
