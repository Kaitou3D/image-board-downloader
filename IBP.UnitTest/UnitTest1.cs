using ImageBoardProcessor.Enumerations;
using ImageBoardProcessor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IBP.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void QueryConstructorTest()
        {
            Query query = new Query(QueryType.Rule34);
            Assert.AreEqual(QueryType.Rule34, query.SearchType);
            Console.WriteLine(query.URLbuilder.Uri);
        }
    }
}
