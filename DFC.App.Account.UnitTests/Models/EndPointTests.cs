using DFC.App.Account.Models;
using NUnit.Framework;

namespace DFC.App.Account.UnitTests.Models
{
    class EndPointTests
    {
        [Test]
        public void CreateEndPoint()
        {
            var endPoint = new EndPoint();
            endPoint.Action = "Action";
            endPoint.Controller = "Controller";
            endPoint.Methods = "Methods";
        }
    }
}