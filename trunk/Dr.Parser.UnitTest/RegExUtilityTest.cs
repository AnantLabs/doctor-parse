using Dr.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace Dr.Parser.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for RegExUtilityTest and is intended
    ///to contain all RegExUtilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RegExUtilityTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for RegexOptionsParse
        ///</summary>
        [TestMethod()]
        public void RegexOptionsParseTest()
        {
            string list = "IgnoreCase|singleline|IgnoreWhitespace|FirstMatch";

            RegexOptions options = new RegexOptions();
            RegexOptions optionsExpected = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            bool IsFirstMatch = false;
            bool IsFirstMatchExpected = true;

            try
            {
                RegExUtility.RegexOptionsParse(list, ref options, ref IsFirstMatch);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentException);
            }

            Assert.AreEqual(optionsExpected, options);
            Assert.AreEqual(IsFirstMatchExpected, IsFirstMatch);

        }
    }
}
