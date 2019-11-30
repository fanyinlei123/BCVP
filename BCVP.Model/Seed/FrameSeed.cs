using BCVP.Common;
using BCVP.Common.Helper;
using BCVP.Model.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BCVP.Model.Models
{
    public class FrameSeed
    {

        /// <summary>
        /// 生成Model层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateModels(MyContext myContext)
        {

            try
            {
                myContext.Create_Model_ClassFileByDBTalbe($@"C:\my-file\BCVP.Model", "BCVP.Model.Models", new string[] {  }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 生成IRepository层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateIRepositorys(MyContext myContext)
        {

            try
            {
                myContext.Create_IRepository_ClassFileByDBTalbe($@"C:\my-file\BCVP.IRepository", "BCVP.IRepository", new string[] {  }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 IService 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateIServices(MyContext myContext)
        {

            try
            {
                myContext.Create_IServices_ClassFileByDBTalbe($@"C:\my-file\BCVP.IServices", "BCVP.IServices", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 Repository 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateRepository(MyContext myContext)
        {

            try
            {
                myContext.Create_Repository_ClassFileByDBTalbe($@"C:\my-file\BCVP.Repository", "BCVP.Repository", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 生成 Repository 层
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public static bool CreateServices(MyContext myContext)
        {

            try
            {
                myContext.Create_Repository_ClassFileByDBTalbe($@"C:\my-file\BCVP.Services", "BCVP.Services", new string[] { "Module" }, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
