/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 <http://www.github.com/GraphDefined/eMI3>
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

#region Usings

using System;
using System.Collections.Generic;

using NUnit.Framework;

using org.GraphDefined.WWCP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.UnitTests
{

    /// <summary>
    /// Unit tests for the EVSE_Id class.
    /// </summary>
    [TestFixture]
    public class EVSE_IdTests
    {

        #region EVSE_IdStringConstructorTest()

        /// <summary>
        /// A test for the EVSE_Id string constructor.
        /// </summary>
        [Test]
        public void EVSE_IdStringConstructorTest()
        {
            var _EVSE_Id = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "123");
            Assert.AreEqual("123", _EVSE_Id.ToString());
            Assert.AreEqual(3,     _EVSE_Id.Length);
        }

        #endregion


        #region op_Equality_BothNull_Test()

        /// <summary>
        /// A test for the equality operator both null.
        /// </summary>
        [Test]
        public void op_Equality_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsTrue(_EVSE_Id1 == _EVSE_Id2);
        }

        #endregion

        #region op_Equality_Equals_Test()

        /// <summary>
        /// A test for the equality operator equals.
        /// </summary>
        [Test]
        public void op_Equality_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1 == _EVSE_Id2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsFalse(_EVSE_Id1 == _EVSE_Id2);
        }

        #endregion


        #region op_Inequality_BothNull_Test()

        /// <summary>
        /// A test for the inequality operator both null.
        /// </summary>
        [Test]
        public void op_Inequality_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsFalse(_EVSE_Id1 != _EVSE_Id2);
        }

        #endregion

        #region op_Inequality_Equals_Test()

        /// <summary>
        /// A test for the inequality operator equals.
        /// </summary>
        [Test]
        public void op_Inequality_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsFalse(_EVSE_Id1 != _EVSE_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsTrue(_EVSE_Id1 != _EVSE_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsTrue(_EVSE_Id1 != _EVSE_Id2);
        }

        #endregion


        #region op_Smaller_BothNull_Test()

        /// <summary>
        /// A test for the smaller operator both null.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void op_Smaller_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsFalse(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion

        #region op_Smaller_Equals_Test()

        /// <summary>
        /// A test for the smaller operator equals.
        /// </summary>
        [Test]
        public void op_Smaller_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsFalse(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsTrue(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsTrue(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsFalse(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            Assert.IsFalse(_EVSE_Id1 < _EVSE_Id2);
        }

        #endregion


        #region op_SmallerOrEqual_BothNull_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator both null.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void op_SmallerOrEqual_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsFalse(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsTrue(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsTrue(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsFalse(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            Assert.IsFalse(_EVSE_Id1 <= _EVSE_Id2);
        }

        #endregion


        #region op_Bigger_BothNull_Test()

        /// <summary>
        /// A test for the bigger operator both null.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void op_Bigger_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsFalse(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion

        #region op_Bigger_Equals_Test()

        /// <summary>
        /// A test for the bigger operator equals.
        /// </summary>
        [Test]
        public void op_Bigger_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsFalse(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsFalse(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsFalse(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            Assert.IsTrue(_EVSE_Id1 > _EVSE_Id2);
        }

        #endregion


        #region op_BiggerOrEqual_BothNull_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator both null.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void op_BiggerOrEqual_BothNull_Test()
        {
            EVSE_Id _EVSE_Id1 = null;
            EVSE_Id _EVSE_Id2 = null;
            Assert.IsFalse(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Equals_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsFalse(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsFalse(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            Assert.IsTrue(_EVSE_Id1 >= _EVSE_Id2);
        }

        #endregion


        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsTrue(_EVSE_Id1.CompareTo(_EVSE_Id2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "23");
            Assert.IsTrue(_EVSE_Id1.CompareTo(_EVSE_Id2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1.CompareTo(_EVSE_Id2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1.CompareTo(_EVSE_Id2) > 0);
        }

        #endregion


        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            Assert.IsTrue(_EVSE_Id1.Equals(_EVSE_Id2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var _EVSE_Id1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var _EVSE_Id2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            Assert.IsFalse(_EVSE_Id1.Equals(_EVSE_Id2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var _SensorHashCode1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5").GetHashCode();
            var _SensorHashCode2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "5").GetHashCode();
            Assert.AreEqual(_SensorHashCode1, _SensorHashCode2);
        }

        #endregion

        #region GetHashCodeNotEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeNotEqualTest()
        {
            var _SensorHashCode1 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1").GetHashCode();
            var _SensorHashCode2 = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2").GetHashCode();
            Assert.AreNotEqual(_SensorHashCode1, _SensorHashCode2);
        }

        #endregion


        #region EVSE_IdsAndNUnitTest()

        /// <summary>
        /// Tests EVSE_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void EVSE_IdsAndNUnitTest()
        {

            var a = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var b = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var c = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

        }

        #endregion

        #region EVSE_IdsInHashSetTest()

        /// <summary>
        /// Test EVSE_Ids within a HashSet.
        /// </summary>
        [Test]
        public void EVSE_IdsInHashSetTest()
        {

            var a = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");
            var b = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "2");
            var c = EVSE_Id.Parse(EVSEOperator_Id.Parse(Country.Germany, "822"), "1");

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

    }

}
