using BCVP.Common;
using BCVP.Controllers;
using BCVP.IRepository;
using BCVP.IServices;
using BCVP.Model.Models;
using Moq;
using Xunit;
using System;
using Microsoft.Extensions.Logging;
using Autofac;
using System.Linq;

namespace BCVP.Tests
{
    public class ClaimsController_Should
    {
        ClaimsController claimsController;



        public ClaimsController_Should()
        {
            claimsController = new ClaimsController();
        }

        [Fact]
        public void GetTest()
        {
            var data = claimsController.Get();
            Assert.True(data.Any());
        }
        [Fact]
        public void GetDetailsTest()
        {
            object blogs =claimsController.Get(1);

            Assert.NotNull(blogs);
        }

    }
}
