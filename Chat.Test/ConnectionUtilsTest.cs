using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat.Model;
using Chat.Model.BasicTypes;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Test
{
    [TestClass]
    public class ConnectionUtilsTest
    {
        [TestMethod]
        public void ConnectionTest()
        {
            IPEndPoint endPoint1 = new IPEndPoint(IPAddress.Loopback, 65000);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Loopback, 65001);
            ConnectionUtils utils1 = new ConnectionUtils(endPoint1);
            ConnectionUtils utils2 = new ConnectionUtils(endPoint2);
            Assert.IsTrue(utils1.recieveSocketConnected & utils1.sendSocketConnected & utils2.recieveSocketConnected & utils2.sendSocketConnected);
        }
    }
}
