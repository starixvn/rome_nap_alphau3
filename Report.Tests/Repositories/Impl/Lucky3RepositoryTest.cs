using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.Models;
using Fanex.NAP.Framework.Data;
using Fanex.Razorbill.Module.Report.Criteria.Lucky3;
using Fanex.Razorbill.Module.Report.Repositories.Impl;
using NSubstitute;
using NUnit.Framework;

namespace Report.Tests.Repositories.Impl
{
    public class Lucky3RepositoryTest
    {
        private IDynamicRepository dynamicRepository;
        private Lucky3Repository repository;

        [SetUp]
        public void SetUp()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            repository = new Lucky3Repository(dynamicRepository);
        }

        [Test]
        public void GetLucky3Detail_Alway_CallFetchDynamicRepository()
        {
            //// Arrange
            var parameter = new GetLucky3DetailParameter();

            //// Act
            repository.GetLucky3Detail(parameter);

            //// Assert
            dynamicRepository.Received().Fetch<Lucky3DetailModel>(Arg.Any<GetLucky3DetailCriteria>());
        }
    }
}
