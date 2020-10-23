using FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerApplication;
using GUI;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void TestMethod1()
        {
            FileWriteRead fileWriteRead = new FileWriteRead();
            fileWriteRead.ReadFromFile();
            Assert.IsNotNull(fileWriteRead.outcomes);
        }

        [TestMethod()]
        public void Test2()
        {
            FileWriteRead fileWriteRead = new FileWriteRead();
            fileWriteRead.WriteToFile("test");
            fileWriteRead.ReadFromFile();
            Assert.AreEqual("test won game 1!", fileWriteRead.outcomes[0]);
        }

        [TestMethod()]
        public void Test3()
        {
            Game game = new Game(null, "test", true);
            Dictionary<string, bool> test = new Dictionary<string, bool>();
            test.Add("a", true);
            test.Add("b", true);
            test.Add("c", true);
            Assert.IsTrue(game.CheckWinner(test));
        }
    }
}
