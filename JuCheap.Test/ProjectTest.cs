using System;
using System.Diagnostics;
using JuCheap.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JuCheap.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (var i = 0; i < 10; i++)
            {
                Trace.Write(SnowFlake.NewId());
                Trace.Write(Environment.NewLine);
            }
        }
    }
}
