using Fanex.BetDetails.Lucky3.Criteria;
using Fanex.Razorbill.Module.Report.Criteria.Lucky3;
using NSubstitute;
using NUnit.Framework;

namespace Report.Tests.Criteria.Lucky3
{

        public class GetLucky3DetailCriteriaTest
        {
            private IGetLucky3DetailParameter parameter;
            private GetLucky3DetailCriteria criteria;

            [SetUp]
            public void SetUp()
            {
                parameter = Substitute.For<IGetLucky3DetailParameter>();
                criteria = new GetLucky3DetailCriteria(parameter);
            }

            [Test]
            public void CustId_Always_ReturnCustomerIdOfParameter()
            {
                //// Arrange
                var expectedId = 12345;
                parameter.CustomerId.Returns(expectedId);

                //// Act
                var act = criteria.CustId;

                //// Assert
                Assert.AreEqual(expectedId, act);
            }

            [Test]
            public void TransactionId_Always_ReturnTransactionIdOfParameter()
            {
                //// Arrange
                var expectedId = 12345;
                parameter.TransactionId.Returns(expectedId);

                //// Act
                var act = criteria.TransId;

                //// Assert
                Assert.AreEqual(expectedId, act);
            }

            [Test]
            public void GetSettingKey_Always_ReturnCorrectKey()
            {
                //// Arrange            
                var expectedKey = "Acc_BetList_Lucky3_Detail";

                //// Act
                var act = criteria.GetSettingKey();

                //// Assert
                Assert.AreEqual(expectedKey, act);
            }
        }
}
