using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using QRCoder;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuanMinDaShangPlatform.Controllers
{
    /// <summary>
    /// 商户注册控制类
    /// </summary>
    [Authorize(Roles = "admin")]
    public class CommercialController : Controller
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _log;
        /// <summary>
        /// 商户注册业务接口
        /// </summary>
        ICommercialRepository _commercialRepository;

        readonly AppSettings _setting;

     
        /// <summary>
        /// 字段构造
        /// </summary>
        /// <param name="commercialRepository"></param>
        public CommercialController(AppSettings appseting,ICommercialRepository commercialRepository)
        {
            _commercialRepository = commercialRepository;
            _log = LogManager.GetCurrentClassLogger();
            _setting = appseting;
        }

        #region 注册

        /// <summary>
        /// 注册页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("comregister")]
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 提交注册
        /// </summary>
        /// <param name="company">公司</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="gcid">集团ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("comregister")]
        public IActionResult AddRegister(T_QMDS_Company company, T_QMDS_GroupCompany groupCompany, string username, string password)
        {
            try
            {
                if (_commercialRepository.CheckUser(username))
                {
                    return new JsonResult(new { result = 2, message = "用户名已存在！" });
                }
                if (groupCompany.GCName != null)
                {
                    groupCompany.ID = _commercialRepository.ProduceID(1);
                    company.GCID = groupCompany.ID;
                }
                company.ID = _commercialRepository.ProduceID(2);
                company.CreaterID = company.ID;

                var flag = _commercialRepository.Register(company, groupCompany, username, password);
                if (flag)
                {
                    return new JsonResult(new { result = 1, message = "注册成功！" });
                }
                else
                {
                    return new JsonResult(new { result = 0, message = "注册失败！" });
                }
            }
            catch (Exception exc)
            {
                _log.Fatal($"注册失败：错误信息{exc.Message}");
                return new JsonResult(new { result = -1, messge = exc.Message });
            }

        }
        #endregion

        #region  上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="env">服务端内部数据对象</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("uploadimage")]
        public JsonResult UploadImg([FromServices]IHostingEnvironment env)
        {
            var files = HttpContext.Request.Form.Files;

            var list = new List<string>();
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName);
                var imgPath = @"/upload/image/" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + new Random().Next(100) + ext;
                var stream = file.OpenReadStream();
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);


                var filestream = new FileStream(env.WebRootPath + imgPath, FileMode.CreateNew, FileAccess.ReadWrite);
                filestream.Write(bytes, 0, bytes.Length);
                filestream.Flush();
                filestream.Dispose();
                list.Add(imgPath);

            }
            return new JsonResult(list, new Newtonsoft.Json.JsonSerializerSettings());

        }
        #endregion


        #region 登录

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("comlogin")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            //判断是否验证
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                _log.Info($"返回地址:{returnUrl}");
                //把返回地址保存在前台的hide表单中
                ViewBag.returnUrl = returnUrl;
            }
            ViewBag.error = null;
            await HttpContext.SignOutAsync("loginvalidate");
            return View();
        }
        /// <summary>
        /// 实现登录
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="password"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("comlogin")]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            try
            {
                //后台登录
                var companyUser = _commercialRepository.Login(username, password);
                if (companyUser.Count > 0)
                {

                    await SetCookie(username, companyUser[0].CompanyName, companyUser[0].CompanyID);
                    return new RedirectResult( "/showaddemployee");
                }
                else
                {
                    ViewBag.fanme = username;
                    ViewBag.password = password;
                    ViewBag.error = "账号或密码错误！";
                    return View();
                }
            }
            catch (Exception exc)
            {

                ViewBag.error = $"登录失败：{exc.Message}";
                _log.Fatal($"登录失败：{exc.Message}");
                return View();
            }
        }
        /// <summary>
        /// 创建Cookie
        /// </summary>
        /// <param name="user">当前用户</param>
        [AllowAnonymous]
        public async Task SetCookie(string userName, string companyName, string CompanyID)
        {

            var claims = new Claim[]
                {
                    new Claim(ClaimTypes.UserData,companyName),
                    new Claim(ClaimTypes.Role,"admin"),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Sid,CompanyID)
                 };
            await HttpContext.SignOutAsync("loginvalidate");
            await HttpContext.SignInAsync("loginvalidate", new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookie")));
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
        #endregion

        #region 人员相关
        /// <summary>
        /// 人员展示页
        /// </summary>
        /// <returns></returns>
        [HttpGet("showaddemployee")]
        public IActionResult ShowAddEmployee()
        {
            return View();
        }
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("addemployee")]
        public IActionResult AddEmployee([FromServices]IHostingEnvironment env,T_QMDS_Employee employee, string password)
        {
            try
            {
                //验证手机
                if (_commercialRepository.CheckPhone(employee.Phone))
                {
                    return new JsonResult(0);
                }
                employee.ENumber = _commercialRepository.YGNumber();
                employee.ID = _commercialRepository.ProduceID(3);
                QRCode(env, employee.ID, employee.ENumber);
                _commercialRepository.AddEmployee(employee, password);

                return new JsonResult(new { result = 1, message = "添加人员成功！" });

            }
            catch (Exception exc)
            {
                _log.Fatal($"添加人员失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "添加人员失败！" });
            }
        }

        public void QRCode([FromServices]IHostingEnvironment env,string id,string ygNmuber)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"http://{_setting.DomainName}/getopenid/{id}?redirectUrl =wxpay", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save($@"{env.WebRootPath}\upload\myimage\{ygNmuber}.jpg", System.DrawingCore.Imaging.ImageFormat.Jpeg);
            
        }
        /// <summary>
        /// 查询公司下全部员工
        /// </summary>
        /// <returns></returns>
        [HttpGet("getemployees")]
        public IActionResult GetEmployees(int pageIndex, int pageRow)
        {
            try
            {
                //获取商户ID
                var companyID = HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;
                //var companyID = "gs20180124000005";
                var list = _commercialRepository.GetEmployees(companyID, pageIndex, pageRow);
                return new JsonResult(new { result = 1, message = "查询人员成功！", data = list, companyID });
            }
            catch (Exception exc)
            {
                _log.Fatal($"添加人员失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "查询人员失败！" });
            }
        }
        /// <summary>
        /// 查询公司下全部员工总数
        /// </summary>
        /// <param name="pageRow">当前页的行数</param>
        /// <returns></returns>
        [HttpPost("query/advisescount")]
        public string QueryEmployeesCount(int pageRow)
        {
            return _commercialRepository.QueryEmployeesCount(pageRow);
        }
        /// <summary>
        /// 修改人员
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("modifyemployee")]
        public IActionResult ModifyEmployee(T_QMDS_Employee employee)
        {
            try
            {
                var phone = _commercialRepository.GetEmployeeByID(employee.ID)[0].Phone;
                if (phone != employee.Phone&&_commercialRepository.CheckPhone(employee.Phone)) //验证手机
                {
                    return new JsonResult(0);
                }
                _commercialRepository.ModifyEmployee(employee);
                return new JsonResult(new { result = 1, message = "修改人员成功！" });

            }
            catch (Exception exc)
            {
                _log.Fatal($"添加人员失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "修改人员失败！" });
            }
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        [HttpPost("deleteemployee")]
        public IActionResult DeleteEmployee(string id)
        {
            try
            {
                _commercialRepository.DeleteEmployee(id);
                return new JsonResult(new { result = 1, message = "删除人员成功！" });

            }
            catch (Exception exc)
            {

                _log.Fatal($"添加人员失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "删除人员失败！" });
            }
        }
        /// <summary>
        /// 查询人员详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getemployeebyid")]
        public IActionResult Get(string id)
        {
            try
            {
                var money = _commercialRepository.GetEmployeeDsMoneyID(id);
                var employee = _commercialRepository.GetEmployeeByID(id);
                return new JsonResult(new { result = 1, data = employee, message = "查询人员成功！", money });

            }
            catch (Exception exc)
            {

                _log.Fatal($"添加人员失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "查询人员失败！" });
            }
        }
        #endregion

        #region 公司相关

        /// <summary>
        /// 商户（公司）页面
        /// </summary>
        /// <returns></returns>
       
        [HttpGet("showcompany")]
        public IActionResult ShowCompany()
        {
            return View();
        }
        /// <summary>
        /// 查询商户
        /// </summary>
        /// <param name="companyID">商户（公司）ID</param>
        /// <returns></returns>
        [HttpGet("getcompanybyid")]
        public IActionResult GetCompanyByID()
        {
            try
            {
                var companyID =  HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.Sid).Value;
                var company = _commercialRepository.GetCompanByID(companyID);
                return new JsonResult(new { result = 1, data = company, message = "查询商户成功！" , companyID});
            }
            catch (Exception exc)
            {
                _log.Fatal($"查询商户失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "查询商户失败！" });
            }
        }
        /// <summary>
        /// 修改公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost("modifycompany")]
        public IActionResult ModifyCompany(T_QMDS_Company company)
        {
            try
            {
                 _commercialRepository.ModifyCompany(company);
                return new JsonResult(new { result = 1,  message = "修改商户成功！" });
            }
            catch (Exception exc)
            {
                _log.Fatal($"修改商户失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "修改商户失败！" });
            }
        }
        #endregion

        /// <summary>
        /// 打赏用户协议
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("agreementindex")]
        public IActionResult AgreementIndex()
        {
            return View();
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <returns></returns>
        [HttpGet("modifypassword")]
        public IActionResult ShowModifyPassword()
        {
            ViewBag.CompanyName= HttpContext.User.Claims.SingleOrDefault(item => item.Type == ClaimTypes.UserData).Value;
            return View();
        }

        /// <summary>
        /// 修改商户密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("modifypassword")]
        public IActionResult ModifyPassword(string username, string password)
        {
            try
            {
                _commercialRepository.ModifyPassword(username, password);
                return new JsonResult(new { result = 1, message = "修改商户密码成功！" });
            }
            catch (Exception exc)
            {

                _log.Fatal($"修改商户密码失败：{exc.Message}");
                return new JsonResult(new { result = 0, message = "修改商户密码失败！" });
            }
        }
    }
}
