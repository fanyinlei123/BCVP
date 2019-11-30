using BCVP.Common;
using BCVP.Controllers;
using BCVP.IRepository;
using BCVP.IServices;
using BCVP.Model.Models;
using Moq;
using Xunit;
using System;
using Autofac;

namespace BCVP.Tests
{
    public class Redis_Should
    {
        private IRedisCacheManager _redisCacheManager;
        DI_Test dI_Test = new DI_Test();

        public Redis_Should()
        {
            //var container = dI_Test.DICollections();
            //_redisCacheManager = container.Resolve<IRedisCacheManager>();

        }

        [Fact]
        public void Connect_Redis_Test()
        {

            //var redisBlogCache = _redisCacheManager.Get<object>("Redis.Blog");

            //Assert.Null(redisBlogCache);
        }

    }
}
