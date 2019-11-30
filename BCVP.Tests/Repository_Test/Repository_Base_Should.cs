﻿using BCVP.Model.Models;
using Xunit;
using System;
using BCVP.Repository.Base;
using BCVP.Repository;
using System.Linq;
using BCVP.Common.DB;
using SqlSugar;
using Microsoft.AspNetCore.Hosting;
using Autofac;
using BCVP.Common;
using BCVP.IServices;
using BCVP.IRepository;

namespace BCVP.Tests
{
    public class Repository_Base_Should
    {
        private IBlogArticleRepository baseRepository;
        DI_Test dI_Test = new DI_Test();

        public Repository_Base_Should()
        {

            var container = dI_Test.DICollections();

            baseRepository = container.Resolve<IBlogArticleRepository>();

            //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);
        }


        [Fact]
        public async void Get_Blogs_Test()
        {
            var data = await baseRepository.Query();

            Assert.NotNull(data);
        }

        [Fact]
        public async void Add_Blog_Test()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                bCreateTime = DateTime.Now,
                bUpdateTime = DateTime.Now,
                btitle = "xuint test title",
                bcontent = "xuint test content",
                bsubmitter = "xuint： test repositoryBase add blog",
            };

            var BId = await baseRepository.Add(blogArticle);
            Assert.True(BId > 0);
        }


        [Fact]
        public async void Update_Blog_Test()
        {
            var IsUpd = false;
            var updateModel = (await baseRepository.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(updateModel);

            updateModel.bcontent = "xuint: test repositoryBase content update";
            updateModel.bCreateTime = DateTime.Now;
            updateModel.bUpdateTime = DateTime.Now;

            IsUpd = await baseRepository.Update(updateModel);

            Assert.True(IsUpd);
        }

        [Fact]
        public async void Delete_Blog_Test()
        {
            var IsDel = false;
            var deleteModel = (await baseRepository.Query(d => d.btitle == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            IsDel = await baseRepository.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
