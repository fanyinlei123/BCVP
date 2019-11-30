﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCVP.IRepository.UnitOfWork;
using BCVP.IServices;
using BCVP.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TransactionController : ControllerBase
    {
        private readonly IPasswordLibServices _passwordLibServices;
        private readonly IGuestbookServices _guestbookServices;
        private readonly IUnitOfWork _unitOfWork;


        public TransactionController(IUnitOfWork unitOfWork, IPasswordLibServices passwordLibServices, IGuestbookServices guestbookServices)
        {
            _unitOfWork = unitOfWork;
            _passwordLibServices = passwordLibServices;
            _guestbookServices = guestbookServices;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            List<string> returnMsg = new List<string>() { };
            try
            {
                returnMsg.Add($"Begin Transaction");

                _unitOfWork.BeginTran();
                var passwords = await _passwordLibServices.Query(d=>d.IsDeleted==false);
                returnMsg.Add($"first time : the count of passwords is :{passwords.Count}");


                returnMsg.Add($"insert a data into the table PasswordLib now.");
                var insertPassword = await _passwordLibServices.Add(new PasswordLib()
                {
                    IsDeleted = false,
                    plAccountName = "aaa",
                    plCreateTime = DateTime.Now
                });


                passwords = await _passwordLibServices.Query(d => d.IsDeleted == false);
                returnMsg.Add($"second time : the count of passwords is :{passwords.Count}");
                returnMsg.Add($" ");

                //......

                var guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"first time : the count of guestbooks is :{guestbooks.Count}");

                int ex = 0;
                returnMsg.Add($"There's an exception!!");
                returnMsg.Add($" ");
                int throwEx = 1 / ex;

                var insertGuestbook = await _guestbookServices.Add(new Guestbook()
                {
                    username = "bbb",
                    blogId = 1,
                    createdate = DateTime.Now,
                    isshow = true
                });

                guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"first time : the count of guestbooks is :{guestbooks.Count}");
                returnMsg.Add($" ");

                _unitOfWork.CommitTran();
            }
            catch (Exception)
            {
                _unitOfWork.RollbackTran();
                var passwords = await _passwordLibServices.Query();
                returnMsg.Add($"third time : the count of passwords is :{passwords.Count}");

                var guestbooks = await _guestbookServices.Query();
                returnMsg.Add($"third time : the count of guestbooks is :{guestbooks.Count}");
            }

            return returnMsg;
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<bool> Get(int id)
        {
            return await _guestbookServices.TestTranInRepository();
        }

        // POST: api/Transaction
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _guestbookServices.TestTranInRepositoryAOP();
        }
    }
}
