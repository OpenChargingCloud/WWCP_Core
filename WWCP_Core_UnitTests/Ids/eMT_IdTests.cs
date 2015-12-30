/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

//#region Usings

//using System;
//using System.Collections.Generic;

//using NUnit.Framework;

//using org.GraphDefined.WWCP;

//#endregion

//namespace org.GraphDefined.WWCP.UnitTests
//{

//    /// <summary>
//    /// Unit tests for the eMT_Id class.
//    /// </summary>
//    [TestFixture]
//    public class eMT_IdTests
//    {

//        #region eMT_IdEmptyConstructorTest()

//        /// <summary>
//        /// A test for an empty eMT_Id constructor.
//        /// </summary>
//        [Test]
//        public void eMT_IdEmptyConstructorTest()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            var _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1.Length > 0);
//            Assert.IsTrue(_eMT_Id2.Length > 0);
//            Assert.AreNotEqual(_eMT_Id1, _eMT_Id2);
//        }

//        #endregion

//        #region eMT_IdStringConstructorTest()

//        /// <summary>
//        /// A test for the eMT_Id string constructor.
//        /// </summary>
//        [Test]
//        public void eMT_IdStringConstructorTest()
//        {
//            var _eMT_Id = new eMT_Id("123");
//            Assert.AreEqual("123", _eMT_Id.ToString());
//            Assert.AreEqual(3,     _eMT_Id.Length);
//        }

//        #endregion

//        #region eMT_IdeMT_IdConstructorTest()

//        /// <summary>
//        /// A test for the eMT_Id eMT_Id constructor.
//        /// </summary>
//        [Test]
//        public void eMT_IdeMT_IdConstructorTest()
//        {
//            var _eMT_Id1 = eMT_Id.New;
//            var _eMT_Id2 = _eMT_Id1.Clone;
//            Assert.AreEqual(_eMT_Id1.ToString(), _eMT_Id2.ToString());
//            Assert.AreEqual(_eMT_Id1.Length,     _eMT_Id2.Length);
//            Assert.AreEqual(_eMT_Id1,            _eMT_Id2);
//        }

//        #endregion


//        #region NeweMT_IdMethodTest()

//        /// <summary>
//        /// A test for the static neweMT_Id method.
//        /// </summary>
//        [Test]
//        public void NeweMT_IdMethodTest()
//        {
//            var _eMT_Id1 = eMT_Id.New;
//            var _eMT_Id2 = eMT_Id.New;
//            Assert.AreNotEqual(_eMT_Id1, _eMT_Id2);
//        }

//        #endregion


//        #region op_Equality_Null_Test1()

//        /// <summary>
//        /// A test for the equality operator null.
//        /// </summary>
//        [Test]
//        public void op_Equality_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 == _eMT_Id2);
//        }

//        #endregion

//        #region op_Equality_Null_Test2()

//        /// <summary>
//        /// A test for the equality operator null.
//        /// </summary>
//        [Test]
//        public void op_Equality_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsFalse(_eMT_Id1 == _eMT_Id2);
//        }

//        #endregion

//        #region op_Equality_BothNull_Test()

//        /// <summary>
//        /// A test for the equality operator both null.
//        /// </summary>
//        [Test]
//        public void op_Equality_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 == _eMT_Id2);
//        }

//        #endregion

//        #region op_Equality_SameReference_Test()

//        /// <summary>
//        /// A test for the equality operator same reference.
//        /// </summary>
//        [Test]

//        public void op_Equality_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsTrue(_eMT_Id1 == _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_Equality_Equals_Test()

//        /// <summary>
//        /// A test for the equality operator equals.
//        /// </summary>
//        [Test]
//        public void op_Equality_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1 == _eMT_Id2);
//        }

//        #endregion

//        #region op_Equality_NotEquals_Test()

//        /// <summary>
//        /// A test for the equality operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Equality_NotEquals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsFalse(_eMT_Id1 == _eMT_Id2);
//        }

//        #endregion


//        #region op_Inequality_Null_Test1()

//        /// <summary>
//        /// A test for the inequality operator null.
//        /// </summary>
//        [Test]
//        public void op_Inequality_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion

//        #region op_Inequality_Null_Test2()

//        /// <summary>
//        /// A test for the inequality operator null.
//        /// </summary>
//        [Test]
//        public void op_Inequality_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion

//        #region op_Inequality_BothNull_Test()

//        /// <summary>
//        /// A test for the inequality operator both null.
//        /// </summary>
//        [Test]
//        public void op_Inequality_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion

//        #region op_Inequality_SameReference_Test()

//        /// <summary>
//        /// A test for the inequality operator same reference.
//        /// </summary>
//        [Test]
//        public void op_Inequality_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsFalse(_eMT_Id1 != _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_Inequality_Equals_Test()

//        /// <summary>
//        /// A test for the inequality operator equals.
//        /// </summary>
//        [Test]
//        public void op_Inequality_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsFalse(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion

//        #region op_Inequality_NotEquals1_Test()

//        /// <summary>
//        /// A test for the inequality operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Inequality_NotEquals1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsTrue(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion

//        #region op_Inequality_NotEquals2_Test()

//        /// <summary>
//        /// A test for the inequality operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Inequality_NotEquals2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsTrue(_eMT_Id1 != _eMT_Id2);
//        }

//        #endregion


//        #region op_Smaller_Null_Test1()

//        /// <summary>
//        /// A test for the smaller operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Smaller_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_Null_Test2()

//        /// <summary>
//        /// A test for the smaller operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Smaller_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_BothNull_Test()

//        /// <summary>
//        /// A test for the smaller operator both null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Smaller_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_SameReference_Test()

//        /// <summary>
//        /// A test for the smaller operator same reference.
//        /// </summary>
//        [Test]
//        public void op_Smaller_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsFalse(_eMT_Id1 < _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_Smaller_Equals_Test()

//        /// <summary>
//        /// A test for the smaller operator equals.
//        /// </summary>
//        [Test]
//        public void op_Smaller_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsFalse(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_Smaller1_Test()

//        /// <summary>
//        /// A test for the smaller operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Smaller_Smaller1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsTrue(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_Smaller2_Test()

//        /// <summary>
//        /// A test for the smaller operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Smaller_Smaller2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsTrue(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_Bigger1_Test()

//        /// <summary>
//        /// A test for the smaller operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Smaller_Bigger1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("2");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsFalse(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion

//        #region op_Smaller_Bigger2_Test()

//        /// <summary>
//        /// A test for the smaller operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Smaller_Bigger2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("23");
//            var _eMT_Id2 = new eMT_Id("5");
//            Assert.IsFalse(_eMT_Id1 < _eMT_Id2);
//        }

//        #endregion


//        #region op_SmallerOrEqual_Null_Test1()

//        /// <summary>
//        /// A test for the smallerOrEqual operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_SmallerOrEqual_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_Null_Test2()

//        /// <summary>
//        /// A test for the smallerOrEqual operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_SmallerOrEqual_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_BothNull_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator both null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_SmallerOrEqual_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_SameReference_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator same reference.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_SmallerOrEqual_Equals_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator equals.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_SmallerThan1_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_SmallerThan1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_SmallerThan2_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_SmallerThan2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsTrue(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_Bigger1_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_Bigger1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("2");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsFalse(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion

//        #region op_SmallerOrEqual_Bigger2_Test()

//        /// <summary>
//        /// A test for the smallerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_SmallerOrEqual_Bigger2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("23");
//            var _eMT_Id2 = new eMT_Id("5");
//            Assert.IsFalse(_eMT_Id1 <= _eMT_Id2);
//        }

//        #endregion


//        #region op_Bigger_Null_Test1()

//        /// <summary>
//        /// A test for the bigger operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Bigger_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_Null_Test2()

//        /// <summary>
//        /// A test for the bigger operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Bigger_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_BothNull_Test()

//        /// <summary>
//        /// A test for the bigger operator both null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_Bigger_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_SameReference_Test()

//        /// <summary>
//        /// A test for the bigger operator same reference.
//        /// </summary>
//        [Test]
//        public void op_Bigger_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsFalse(_eMT_Id1 > _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_Bigger_Equals_Test()

//        /// <summary>
//        /// A test for the bigger operator equals.
//        /// </summary>
//        [Test]
//        public void op_Bigger_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsFalse(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_Smaller1_Test()

//        /// <summary>
//        /// A test for the bigger operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Bigger_Smaller1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsFalse(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_Smaller2_Test()

//        /// <summary>
//        /// A test for the bigger operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Bigger_Smaller2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsFalse(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_Bigger1_Test()

//        /// <summary>
//        /// A test for the bigger operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Bigger_Bigger1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("2");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion

//        #region op_Bigger_Bigger2_Test()

//        /// <summary>
//        /// A test for the bigger operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_Bigger_Bigger2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("23");
//            var _eMT_Id2 = new eMT_Id("5");
//            Assert.IsTrue(_eMT_Id1 > _eMT_Id2);
//        }

//        #endregion


//        #region op_BiggerOrEqual_Null_Test1()

//        /// <summary>
//        /// A test for the biggerOrEqual operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_BiggerOrEqual_Null_Test1()
//        {
//            var      _eMT_Id1 = new eMT_Id();
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_Null_Test2()

//        /// <summary>
//        /// A test for the biggerOrEqual operator null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_BiggerOrEqual_Null_Test2()
//        {
//            eMT_Id _eMT_Id1 = null;
//            var      _eMT_Id2 = new eMT_Id();
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_BothNull_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator both null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void op_BiggerOrEqual_BothNull_Test()
//        {
//            eMT_Id _eMT_Id1 = null;
//            eMT_Id _eMT_Id2 = null;
//            Assert.IsFalse(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_SameReference_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator same reference.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_SameReference_Test()
//        {
//            var _eMT_Id1 = new eMT_Id();
//            #pragma warning disable
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id1);
//            #pragma warning restore
//        }

//        #endregion

//        #region op_BiggerOrEqual_Equals_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator equals.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_Equals_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_SmallerThan1_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_SmallerThan1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsFalse(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_SmallerThan2_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_SmallerThan2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsFalse(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_Bigger1_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_Bigger1_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("2");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion

//        #region op_BiggerOrEqual_Bigger2_Test()

//        /// <summary>
//        /// A test for the biggerOrEqual operator not-equals.
//        /// </summary>
//        [Test]
//        public void op_BiggerOrEqual_Bigger2_Test()
//        {
//            var _eMT_Id1 = new eMT_Id("23");
//            var _eMT_Id2 = new eMT_Id("5");
//            Assert.IsTrue(_eMT_Id1 >= _eMT_Id2);
//        }

//        #endregion


//        #region CompareToNullTest1()

//        /// <summary>
//        /// A test for CompareTo null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void CompareToNullTest1()
//        {
//            var    _eMT_Id = eMT_Id.New;
//            Object _Object   = null;
//            _eMT_Id.CompareTo(_Object);
//        }

//        #endregion

//        #region CompareToNullTest2()

//        /// <summary>
//        /// A test for CompareTo null.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void CompareToNullTest2()
//        {
//            var      _eMT_Id = eMT_Id.New;
//            eMT_Id _Object   = null;
//            _eMT_Id.CompareTo(_Object);
//        }

//        #endregion

//        #region CompareToNoneMT_IdTest()

//        /// <summary>
//        /// A test for CompareTo a non-eMT_Id.
//        /// </summary>
//        [Test]
//        [ExpectedException(typeof(ArgumentException))]
//        public void CompareToNoneMT_IdTest()
//        {
//            var _eMT_Id = eMT_Id.New;
//            var _Object   = "123";
//            _eMT_Id.CompareTo(_Object);
//        }

//        #endregion

//        #region CompareToSmallerTest1()

//        /// <summary>
//        /// A test for CompareTo smaller.
//        /// </summary>
//        [Test]
//        public void CompareToSmallerTest1()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsTrue(_eMT_Id1.CompareTo(_eMT_Id2) < 0);
//        }

//        #endregion

//        #region CompareToSmallerTest2()

//        /// <summary>
//        /// A test for CompareTo smaller.
//        /// </summary>
//        [Test]
//        public void CompareToSmallerTest2()
//        {
//            var _eMT_Id1 = new eMT_Id("5");
//            var _eMT_Id2 = new eMT_Id("23");
//            Assert.IsTrue(_eMT_Id1.CompareTo(_eMT_Id2) < 0);
//        }

//        #endregion

//        #region CompareToEqualsTest()

//        /// <summary>
//        /// A test for CompareTo equals.
//        /// </summary>
//        [Test]
//        public void CompareToEqualsTest()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1.CompareTo(_eMT_Id2) == 0);
//        }

//        #endregion

//        #region CompareToBiggerTest()

//        /// <summary>
//        /// A test for CompareTo bigger.
//        /// </summary>
//        [Test]
//        public void CompareToBiggerTest()
//        {
//            var _eMT_Id1 = new eMT_Id("2");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1.CompareTo(_eMT_Id2) > 0);
//        }

//        #endregion


//        #region EqualsNullTest1()

//        /// <summary>
//        /// A test for equals null.
//        /// </summary>
//        [Test]
//        public void EqualsNullTest1()
//        {
//            var    _eMT_Id = eMT_Id.New;
//            Object _Object   = null;
//            Assert.IsFalse(_eMT_Id.Equals(_Object));
//        }

//        #endregion

//        #region EqualsNullTest2()

//        /// <summary>
//        /// A test for equals null.
//        /// </summary>
//        [Test]
//        public void EqualsNullTest2()
//        {
//            var      _eMT_Id = eMT_Id.New;
//            eMT_Id _Object   = null;
//            Assert.IsFalse(_eMT_Id.Equals(_Object));
//        }

//        #endregion

//        #region EqualsNoneMT_IdTest()

//        /// <summary>
//        /// A test for equals a non-eMT_Id.
//        /// </summary>
//        [Test]
//        public void EqualsNoneMT_IdTest()
//        {
//            var _eMT_Id = eMT_Id.New;
//            var _Object   = "123";
//            Assert.IsFalse(_eMT_Id.Equals(_Object));
//        }

//        #endregion

//        #region EqualsEqualsTest()

//        /// <summary>
//        /// A test for equals.
//        /// </summary>
//        [Test]
//        public void EqualsEqualsTest()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("1");
//            Assert.IsTrue(_eMT_Id1.Equals(_eMT_Id2));
//        }

//        #endregion

//        #region EqualsNotEqualsTest()

//        /// <summary>
//        /// A test for not-equals.
//        /// </summary>
//        [Test]
//        public void EqualsNotEqualsTest()
//        {
//            var _eMT_Id1 = new eMT_Id("1");
//            var _eMT_Id2 = new eMT_Id("2");
//            Assert.IsFalse(_eMT_Id1.Equals(_eMT_Id2));
//        }

//        #endregion


//        #region GetHashCodeEqualTest()

//        /// <summary>
//        /// A test for GetHashCode
//        /// </summary>
//        [Test]
//        public void GetHashCodeEqualTest()
//        {
//            var _SensorHashCode1 = new eMT_Id("5").GetHashCode();
//            var _SensorHashCode2 = new eMT_Id("5").GetHashCode();
//            Assert.AreEqual(_SensorHashCode1, _SensorHashCode2);
//        }

//        #endregion

//        #region GetHashCodeNotEqualTest()

//        /// <summary>
//        /// A test for GetHashCode
//        /// </summary>
//        [Test]
//        public void GetHashCodeNotEqualTest()
//        {
//            var _SensorHashCode1 = new eMT_Id("1").GetHashCode();
//            var _SensorHashCode2 = new eMT_Id("2").GetHashCode();
//            Assert.AreNotEqual(_SensorHashCode1, _SensorHashCode2);
//        }

//        #endregion


//        #region eMT_IdsAndNUnitTest()

//        /// <summary>
//        /// Tests eMT_Ids in combination with NUnit.
//        /// </summary>
//        [Test]
//        public void eMT_IdsAndNUnitTest()
//        {

//            var a = new eMT_Id("1");
//            var b = new eMT_Id("2");
//            var c = new eMT_Id("1");

//            Assert.AreEqual(a, a);
//            Assert.AreEqual(b, b);
//            Assert.AreEqual(c, c);

//            Assert.AreEqual(a, c);
//            Assert.AreNotEqual(a, b);
//            Assert.AreNotEqual(b, c);

//        }

//        #endregion

//        #region eMT_IdsInHashSetTest()

//        /// <summary>
//        /// Test eMT_Ids within a HashSet.
//        /// </summary>
//        [Test]
//        public void eMT_IdsInHashSetTest()
//        {

//            var a = new eMT_Id("1");
//            var b = new eMT_Id("2");
//            var c = new eMT_Id("1");

//            var _HashSet = new HashSet<eMT_Id>();
//            Assert.AreEqual(0, _HashSet.Count);

//            _HashSet.Add(a);
//            Assert.AreEqual(1, _HashSet.Count);

//            _HashSet.Add(b);
//            Assert.AreEqual(2, _HashSet.Count);

//            _HashSet.Add(c);
//            Assert.AreEqual(2, _HashSet.Count);

//        }

//        #endregion

//    }

//}
