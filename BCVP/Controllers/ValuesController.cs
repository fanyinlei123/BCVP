﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BCVP.Common.HttpContextUser;
using BCVP.Filter;
using BCVP.IServices;
using BCVP.Model;
using BCVP.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Controllers
{
    /// <summary>
    /// Values控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(Roles = "Admin,Client")]
    //[Authorize(Policy = "SystemOrAdmin")]
    //[Authorize(PermissionNames.Permission)]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private IMapper _mapper;
        private readonly IAdvertisementServices _advertisementServices;
        private readonly Love _love;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;
        private readonly IUser _user;
        readonly IBlogArticleServices _blogArticleServices;

        /// <summary>
        /// ValuesController
        /// </summary>
        /// <param name="blogArticleServices"></param>
        /// <param name="mapper"></param>
        /// <param name="advertisementServices"></param>
        /// <param name="love"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="user"></param>
        public ValuesController(IBlogArticleServices blogArticleServices, IMapper mapper, IAdvertisementServices advertisementServices, Love love, IRoleModulePermissionServices roleModulePermissionServices, IUser user)
        {
            // 测试 Authorize 和 mapper
            _mapper = mapper;
            _advertisementServices = advertisementServices;
            _love = love;
            _roleModulePermissionServices = roleModulePermissionServices;
            // 测试 Httpcontext
            _user = user;
            // 测试AOP加载顺序，配合 return
            _blogArticleServices = blogArticleServices;
        }
        /// <summary>
        /// Get方法
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageModel<ResponseEnum>> Get()
        {
            var data = new MessageModel<ResponseEnum>();

            /*
             *  测试 sql 更新
             * 
             * 【SQL参数】：@bID:5
             *  @bsubmitter:laozhang619
             *  @IsDeleted:False
             * 【SQL语句】：UPDATE `BlogArticle`  SET
             *  `bsubmitter`=@bsubmitter,`IsDeleted`=@IsDeleted  WHERE `bID`=@bID
             */
            var updateSql = await _blogArticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", IsDeleted = false, bID = 5 });


            // 测试模拟异常，全局异常过滤器拦截
            var i = 0;
            var d = 3 / i;


            // 测试 AOP 缓存
            var blogArticles = await _blogArticleServices.GetBlogs();


            // 测试多表联查
            var roleModulePermissions = await _roleModulePermissionServices.QueryMuchTable();


            // 测试多个异步执行时间
            var roleModuleTask = _roleModulePermissionServices.Query();
            var listTask = _advertisementServices.Query();
            var ad = await roleModuleTask;
            var list = await listTask;


            // 测试service层返回异常
            _advertisementServices.ReturnExp();

            Love love = null;
            love.SayLoveU();

            return data;
        }
        /// <summary>
        /// Get(int id)方法
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        //[TypeFilter(typeof(DeleteSubscriptionCache),Arguments =new object[] { "1"})]
        [TypeFilter(typeof(UseServiceDIAttribute), Arguments = new object[] { "laozhang" })]
        public ActionResult<string> Get(int id)
        {
            var loveu = _love.SayLoveU();

            return "value";
        }

        /// <summary>
        /// 测试参数是必填项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/values/RequiredPara")]
        public string RequiredP([Required]string id)
        {
            return id;
        }


        /// <summary>
        /// 通过 HttpContext 获取用户信息
        /// </summary>
        /// <param name="ClaimType">声明类型，默认 jti </param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/values/UserInfo")]
        public MessageModel<List<string>> GetUserInfo(string ClaimType = "jti")
        {
            var getUserInfoByToken = _user.GetUserInfoFromToken(ClaimType);
            return new MessageModel<List<string>>()
            {
                success = _user.IsAuthenticated(),
                msg = _user.IsAuthenticated() ? _user.Name.ObjToString() : "未登录",
                response = _user.GetClaimValueByType(ClaimType)
            };
        }

        /// <summary>
        /// to redirect by route template name.
        /// </summary>
        [HttpGet("/api/custom/go-destination")]
        [AllowAnonymous]
        public void Source()
        {
            var url = Url.RouteUrl("Destination_Route");
            Response.Redirect(url);
        }

        /// <summary>
        /// route with template name.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/custom/destination", Name = "Destination_Route")]
        [AllowAnonymous]
        public string Destination()
        {
            return "555";
        }


        /// <summary>
        /// 测试 post 一个对象 + 独立参数
        /// </summary>
        /// <param name="blogArticle">model实体类参数</param>
        /// <param name="id">独立参数</param>
        [HttpPost]
        [AllowAnonymous]
        public object Post([FromBody]  BlogArticle blogArticle, int id)
        {
            return Ok(new { success = true, data = blogArticle, id = id });
        }


        /// <summary>
        /// 测试 post 参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TestPostPara")]
        [AllowAnonymous]
        public object TestPostPara(string name)
        {
            return Ok(new { success = true, name = name });
        }


        /// <summary>
        /// Put方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// Delete方法
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
