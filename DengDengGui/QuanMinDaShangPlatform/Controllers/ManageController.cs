using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;
using QuanMinDaShangPlatform.Models.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace QuanMinDaShangPlatform.Controllers
{
    [Authorize(Roles = "admin")]
    public class ManageController : Controller
    {
        private IQMDSTemplateRepository qMDSTemplateRepository;
        private ICompanyTemplateRepository companyTemplateRepository;
        private IEmployeeTemplateRelationRepository employeeTemplateRelationRepository;
        private IDsInfoRepository dsInfoRepository;
        public ManageController(IQMDSTemplateRepository qMDSTemplateRepository, ICompanyTemplateRepository companyTemplateRepository, IEmployeeTemplateRelationRepository employeeTemplateRelationRepository, IDsInfoRepository dsInfoRepository)
        {
            this.qMDSTemplateRepository = qMDSTemplateRepository;
            this.companyTemplateRepository = companyTemplateRepository;
            this.employeeTemplateRelationRepository = employeeTemplateRelationRepository;
            this.dsInfoRepository = dsInfoRepository;
        }
       
        [HttpGet("gettemplate")]
        public IActionResult SetTemplate()
        {
            return View();
        }

        /// <summary>
        /// 添加打赏模板
        /// </summary>
        /// <param name="tQMDSTemplate">打赏模板信息</param>
        /// <returns></returns>
        [HttpPost("addqmdstemplate")]
        public IActionResult AddQMDSTemplate(TQMDSTemplate tQMDSTemplate)
        {
            var companyID= HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;
            tQMDSTemplate.CreaterID = companyID;
            tQMDSTemplate.CompanyID = companyID;
            try
            {
                var result = qMDSTemplateRepository.AddQMDSTemplate(tQMDSTemplate);
                return new JsonResult(new BackData<bool> { Result = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 删除打赏模板信息
        /// </summary>
        /// <param name="id">打赏模板ID</param>
        /// <returns></returns>
        [HttpDelete("deleteqmdstemplate")]
        public IActionResult DeleteQMDSTemplate(int id)
        {
            try
            {
                var result = qMDSTemplateRepository.DeleteQMDSTemplate(id);
                return new JsonResult(new BackData<bool> { Result = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 修改打赏模板信息
        /// </summary>
        /// <param name="tQMDSTemplate">模板信息内容</param>
        /// <returns></returns>
        [HttpPut("modifyqmdstemplate")]
        public IActionResult ModifyQMDSTemplate(TQMDSTemplate tQMDSTemplate)
        {
            try
            {
                var result = qMDSTemplateRepository.ModifyQMDSTemplate(tQMDSTemplate);
                return new JsonResult(new BackData<bool> { Result = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 查询所有模板信息内容
        /// </summary>
        /// <returns></returns>
        [HttpGet("templatemanage")]
        public IActionResult TemplateManage()
        {
            return View();
        }

        #region 查询所有人员
        /// <summary>
        /// 查询所有模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("querytemplates")]
        public IActionResult QueryTemplates()
        {
            var companyID=HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;

            var result = qMDSTemplateRepository.QureyTemplates(companyID);
            if (result != null && result.Count() > 0)
            {
                return new JsonResult(new BackData<IEnumerable<TQMDSTemplate>> { Result = true, Data = result });
            }
            else
            {
                return new JsonResult(new BackData<TQMDSTemplate> { Result = false, ErrMeg = "还未添加模板信息" });
            }
        }
        #endregion

        /// <summary>
        /// 通过ID查询模板信息ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet("querytemplatebyid")]
        public IActionResult QueryTemplateByID(int id)
        {
            var result = qMDSTemplateRepository.QueryTemplateByTemplateID(id);
            if (result != null && result.Count() > 0)
            {
                return new JsonResult(new BackData<IEnumerable<TQMDSTemplate>> { Result = true, Data = result });
            }
            else
            {
                return new JsonResult(new BackData<IEnumerable<TQMDSTemplate>> { Result = false, ErrMeg = "未找到打赏模板" });
            }
        }

        /// <summary>
        /// 添加公司打赏模板关系
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation">公司打赏模板关系</param>
        /// <returns></returns>
        [HttpPost("addcompanytemplate")]
        public IActionResult AddCompanyTemplate(int id)
        {
            try
            {
               
                var companyID = HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;

                var ret = employeeTemplateRelationRepository.AddEmployeeTemplateRelation(companyID, id);
                return new JsonResult(new BackData<bool> { Result = ret });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }

        }

        /// <summary>
        ///修改公司打赏模板信息
        /// </summary>
        /// <param name="tQMDSCompanyTemplateRelation">公司打赏模板信息</param>
        /// <returns></returns>
        [HttpPut("modifycompanytemplate")]
        public IActionResult ModifyCompanyTemplate(TQMDSCompanyTemplateRelation tQMDSCompanyTemplateRelation)
        {
            try
            {
                var result = companyTemplateRepository.ModifyCompanyTemplateRelation(tQMDSCompanyTemplateRelation);
                var ret = employeeTemplateRelationRepository.DeleteEmployeeTemplateRelationsByComponyID(tQMDSCompanyTemplateRelation.CompanyID, tQMDSCompanyTemplateRelation.TemplateID);
                return new JsonResult(new BackData<bool> { Result = true });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 修改人员打赏模板关系表
        /// </summary>
        /// <param name="tQMDSEmployeeTemplateRelation">人员打赏模板关系</param>
        /// <returns></returns>
        [HttpPut("modifyemployeetemplaterelation")]
        public IActionResult ModifyEmpolyeeTemplateRelation(TQMDSEmployeeTemplateRelation tQMDSEmployeeTemplateRelation)
        {
            try
            {
                var result = employeeTemplateRelationRepository.ModifyEmployeeTemplateRelation(tQMDSEmployeeTemplateRelation);
                return new JsonResult(new BackData<bool> { Result = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 通过人员打赏模板ID删除人员打赏模板关系内容
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("deleteemployeetemplaterelation")]
        public IActionResult DeleteEmplouyeeTemplateRelation(int id)
        {
            try
            {
                var result = employeeTemplateRelationRepository.DeleteEmployeeTemplateRelation(id);
                return new JsonResult(new BackData<bool> { Result = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        [HttpGet("getcompanytemplate")]
        public IActionResult CompanyTemplate()
        {
            return View();
        }

        /// <summary>
        /// 获取公司所有人的打赏设置
        /// </summary>
        /// <returns></returns>
        [HttpGet("getemployeetemplate")]
        public IActionResult GetEmployeeTemplateMsg()
        {
            try
            {
                var companyID = HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;
                var result = employeeTemplateRelationRepository.QueryEmployeeTemplateRelation(companyID);
                return new JsonResult(new BackData<dynamic> { Result = true, Data = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 获取人员打赏信息通过单关系ID
        /// </summary>
        /// <param name="id">人员模板关系</param>
        /// <returns></returns>
        [HttpGet("getemployeedsset")]
        public IActionResult GetEmployeeDsSet(int id)
        {
            try
            {
                var result = employeeTemplateRelationRepository.QueryEmployeeDsSetByID(id);
                return new JsonResult(new BackData<dynamic> { Result = true, Data = result });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 后台查询员工被打上明细页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("employeeinfomanage")]
        public IActionResult EmployeeInfoManage()
        {
            return View();
        }

        /// <summary>
        /// 后台员工通过员工信息获取被打赏明细信息
        /// </summary>
        /// <param name="index">分页索引</param>
        /// <param name="employeeid">人员ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet("getemployeeinfobyemployeeid")]
        public IActionResult GetEmployeeInfoByEmployeeID(int index, string employeeid, string start, string end)
        {
            try
            {
                decimal totalFee = 0.00m;
                var result = dsInfoRepository.QueryEmployeeInfoByEmployeeID(index, employeeid, start, end, out totalFee);
                return new JsonResult(new BackData<dynamic> { Result = true, Data = result, ErrMeg = totalFee.ToString() }, new Newtonsoft.Json.JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 公司查看人员被打赏信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("companyemployeedsinfo")]
        public IActionResult CompanyEmployDsInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取公司人员的打赏明细
        /// </summary>
        /// <param name="index">分页索引</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet("getcompanyemployeedsinfo")]
        public IActionResult GetCompanyEmployeeDsInfo(int index,string start, string end)
        {
            try
            {
                var companyID= HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;
                decimal totalFee = 0m;
                var result = dsInfoRepository.QueryCompanyEmployeeDsInfoByCompanyID(index, companyID, start, end, out totalFee);
                return new JsonResult(new BackData<dynamic> { Result = true, Data = result, ErrMeg = totalFee.ToString("0.00") }, new Newtonsoft.Json.JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }
    }
}
