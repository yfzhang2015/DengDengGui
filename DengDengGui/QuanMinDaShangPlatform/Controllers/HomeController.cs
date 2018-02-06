using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using QuanMinDaShangPlatform.Models;
using System.DrawingCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using QuanMinDaShangPlatform.Models.Entity;
using QuanMinDaShangPlatform.Models.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace QuanMinDaShangPlatform.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        Logger _log;
        DataModel _db;
        /// <summary>
        /// 配置文件
        /// </summary>
        readonly AppSettings _setting;


        private IDsInfoRepository dsInfoRepository;

        private ICommercialRepository commercialRepository;

        private IEmployeeTemplateRelationRepository employeeTemplateRelationRepository;
        public HomeController(AppSettings appSettings, IDsInfoRepository dsInfoRepository, ICommercialRepository commercialRepository, IEmployeeTemplateRelationRepository employeeTemplateRelationRepository)
        {
            _setting = appSettings;
            _db = new DataModel(_setting.DBConnection);
            _log = LogManager.GetCurrentClassLogger();
            this.dsInfoRepository = dsInfoRepository;
            this.commercialRepository = commercialRepository;
            this.employeeTemplateRelationRepository = employeeTemplateRelationRepository;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            return new RedirectResult("/showaddemployee");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("qrcode")]
        public IActionResult QRCode([FromServices]IHostingEnvironment env, string url, string id)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"http://{_setting.DomainName}/getopenid/{id}?redirectUrl =wxpay", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save($@"{env.WebRootPath}\images\{id}.jpg", System.DrawingCore.Imaging.ImageFormat.Jpeg);
            return Ok();
        }

        [HttpGet("test")]
        public void Test()
        {
            var userAgent = (HttpContext.Request.Headers as Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders).HeaderUserAgent.ToString().ToLower();

        }

        [HttpGet("wxpay")]
        public IActionResult WxPay(string openid, string employeeid = "ry20180124000015")
        {

            var employeeList = employeeTemplateRelationRepository.QueryEmployeeDsSetMessage(employeeid).ToList();
            if (employeeList!=null&&employeeList.Count>0)
            {

            switch (employeeList[0].DSTemplateType)
            {
                case 0:
                    #region 自由输入模式
                    var str = $@"<div class='container-fluid'>
			<div class='row'>
				<div class='rewardBox'>
					<div class='rewardTitle'>{employeeList[0].CompanyName}</div>				
					<div class='rewardComment container'>{employeeList[0].Message}</div>
					<div class='container'>
						<div class='starZan'>
							<div class='star'><p>打赏</p><P>{employeeList[0].Count}</p><p>次</p></div>
						</div>						
					</div>
					<div class='rewardPic'>
						<div class='rewardPicBox'>
							<div class='headSculpture' id='backimg'></div>
							<div class='shade'>
								<h1>{employeeList[0].EName}</h1>
                                <input type='hidden' id='img' value='{employeeList[0].Picture}'/>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>

		<div class='container-fluid'>
			<div class='rewardPay'>
				<input type='number' placeholder='请输入金额' id='money'/>
				<form action = '/ShowWeiXinPay' method='get' id='form'>
				<input type='hidden'name='openid' value='{openid}'/>
				<input type = 'hidden' name='fAmt' id='famt'/>
				<input type = 'hidden' name='employeeid'  value='{employeeid}'/>
			</form>
			</div>
		</div>
		<div class='container-fluid'>
			<div class='rewardPaySure'>
				<input type='button' value='确认支付' onclick='SubmitOrder()'/>
			</div>
		</div>";
                    ViewData["data"] = str;
                    #endregion
                    break;
                case 1:
                    #region 多额固定模式
                    var str1 = $@"
<div class='container-fluid'>
			<div class='row'>
				<div class='rewardBox'>
					<div class='rewardTitle'>{employeeList[0].CompanyName}</div>				
					<div class='rewardComment container'>{employeeList[0].Message}</div>
					<div class='container'>
						<div class='starZan'>
							<div class='star'><p>打赏</p><P>{employeeList[0].Count}</p><p>次</p></div>
						</div>						
					</div>
					<div class='rewardPic'>
						<div class='rewardPicBox'>
							<div class='headSculpture' id='backimg'></div>
							<div class='shade'>
								<h1>{employeeList[0].EName}</h1>
                                <input type='hidden' id='img' value='{employeeList[0].Picture}'/>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
<div class='container-fluid'>
			<div class='rewardPay'>
<div class='row'>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS1).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS2).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS3).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
                       
                        <form action = '/ShowWeiXinPay' method='get' id='form'>
				<input type='hidden'name='openid' value='{openid}'/>
				<input type = 'hidden' name='fAmt' id='famt'/>
				<input type = 'hidden' name='employeeid'  value='{employeeid}'/>
			</form>
					</div>
</div>
</div>";
                    ViewData["data"] = str1;
                    #endregion
                    break;
                case 2:
                    var str3 = $@"
<div class='container-fluid'>
			<div class='row'>
				<div class='rewardBox'>
					<div class='rewardTitle'>{employeeList[0].CompanyName}</div>				
					<div class='rewardComment container'>{employeeList[0].Message}</div>
					<div class='container'>
						<div class='starZan'>
							<div class='star'><p>打赏</p><P>{employeeList[0].Count}</p><p>次</p></div>
						</div>						
					</div>
					<div class='rewardPic'>
						<div class='rewardPicBox'>
							<div class='headSculpture' id='backimg'></div>
							<div class='shade'>
								<h1>{employeeList[0].EName}</h1>
                                <input type='hidden' id='img' value='{employeeList[0].Picture}'/>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
<div class='container-fluid'>
			<div class='rewardPay'>
<div class='row'>
						
						<div class='col-xs-12 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS1).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
						
                       
                        <form action = '/ShowWeiXinPay' method='get' id='form'>
				<input type='hidden'name='openid' value='{openid}'/>
				<input type = 'hidden' name='fAmt' id='famt'/>
				<input type = 'hidden' name='employeeid'  value='{employeeid}'/>
			</form>
					</div>
</div>
</div>";
                    ViewData["data"] = str3;
                    break;
                case 3:
                    #region 自由多金额模式
                    var str2 = $@"
<div class='container-fluid'>
			<div class='row'>
				<div class='rewardBox'>
					<div class='rewardTitle'>{employeeList[0].CompanyName}</div>				
					<div class='rewardComment container'>{employeeList[0].Message}</div>
					<div class='container'>
						<div class='starZan'>
							<div class='star'><p>打赏</p><P>{employeeList[0].Count}</p><p>次</p></div>
						</div>						
					</div>
					<div class='rewardPic'>
						<div class='rewardPicBox'>
							<div class='headSculpture' id='backimg'></div>
							<div class='shade'>
								<h1>{employeeList[0].EName}</h1>
                                <input type='hidden' id='img' value='{employeeList[0].Picture}'/>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
<div class='container-fluid'>
			<div class='rewardPay'>
<div class='row'>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS1).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS2).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
						<div class='col-xs-4 choiceTally'>
							<input class='singleBox' type='button' value='{Convert.ToDecimal(employeeList[0].DS3).ToString("0.00")}' onclick='SubmiteBtns(this)'/>
						</div>
                        <input type='number' value='请输入金额' style='margin-top: 1rem;' id='payfee'/>
                       <input type='button' value='确认支付' style='margin-top: 1rem;' onclick='SurePay()'/>
                        <form action = '/ShowWeiXinPay' method='get' id='form'>
				<input type='hidden'name='openid' value='{openid}'/>
				<input type = 'hidden' name='fAmt' id='famt'/>
				<input type = 'hidden' name='employeeid'  value='{employeeid}'/>
			</form>
					</div>
</div>
</div>";
                    ViewData["data"] = str2;
                    #endregion
                    break;
                default:
                    break;
            }
            }
            else
            {
                ViewData["data"] = "<h1>请为员工使用模板</h1>";
            }


            return View();
        }

        [HttpGet("dscomplete")]
        public IActionResult DSComplete()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string redirectUrl, string code = null, string openID = null)
        {
            if (openID == null)
            {
                return Redirect("/getopenid/employee?redirectUrl=" + redirectUrl);
            }
            var verifyEmployList = dsInfoRepository.QueryEmployeeByOpenID(openID);
            if (verifyEmployList.Count() > 0)
            {
                return RedirectToAction($"ShowEmployeeDsInfo", new { openID });
            }
            else
            {
                return RedirectToAction($"Login", new { openID });

            }

        }

        [HttpGet("employeelogin")]
        public IActionResult Login(string openID)
        {
            ViewData["openid"] = openID;
            return View();
        }

        /// <summary>
        /// 验证员工
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost("verifyemployee")]
        public IActionResult VerifyEmployee(string number, string password)
        {
            try
            {
                var employUserList = dsInfoRepository.QueryEmployeeByPhoneAndPssword(number, password);
                if (employUserList.Count() > 0)
                {
                    return new JsonResult(new BackData<IEnumerable<T_QMDS_EmployeeUser>>{ Result=true,Data=employUserList});
                }
                else
                {
                    return new JsonResult(new BackData<bool> { Result = false,ErrMeg="未登记过" });
                }
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result = false, ErrMeg = exc.Message });
            }
        }

        /// <summary>
        /// 绑定员工的openID
        /// </summary>
        /// <param name="openID">openID</param>
        /// <param name="employeeID">人员ID</param>
        /// <returns></returns>
        [HttpPost("bindopenid")]
        public IActionResult BindEmployee(string openid,string employeeID)
        {
            try
            {
                var result=dsInfoRepository.BindEmployeeOpenID(openid,employeeID);
                return new JsonResult(new BackData<bool> { Result=result});
            }
            catch (Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result=false,ErrMeg=exc.Message});
            }
        }

        [HttpGet("showregistrationcategory")]
        public IActionResult ShowEmployeeDsInfo(string openID)
        {
            ViewData["openid"] = openID;
            return View();
        }

        /// <summary>
        /// 员工通过自己的openID
        /// </summary>
        /// <param name="index">查询索引</param>
        /// <param name="openID">openID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet("getemployeeinfo")]
        public IActionResult EmployeeInfo(int index,string openID,string start,string end)
        {
            try
            {
                decimal totalFee = 0m;
                var result = dsInfoRepository.QueryEmployeeInfo(index,openID,start,end,out totalFee);
                return new JsonResult(new BackData<dynamic> { Result=true,Data=result,ErrMeg=totalFee.ToString("0.00")},new Newtonsoft.Json.JsonSerializerSettings { DateFormatString="yyyy-MM-dd HH:mm:ss"});
            }
            catch(Exception exc)
            {
                return new JsonResult(new BackData<bool> { Result=false,ErrMeg=exc.Message});
            }
        }


        #region 获取GetOpenid
        /// <summary>
        /// 获取GetOpenid
        /// </summary>
        /// <param name="redirectUrl">路由</param>
        /// <param name="code">微信code</param>
        /// <returns></returns>
        [HttpGet("getopenid/{employeeid}")]
        public IActionResult GetOpenID(string redirectUrl, string code = null, string employeeid = null)
        {
            var jsApiPay = new JsApiPay();
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                var url = jsApiPay.GetOpenidAndAccessToken($@"{_setting.DomainName}/getopenid/{employeeid}?redirectUrl=" + redirectUrl, code);

                if (string.IsNullOrEmpty(url))
                {
                    if (employeeid == "employee")
                    {
                        return Redirect($"/login?openID={jsApiPay.openid}");
                    }
                    return Redirect($"/wxpay?openid={jsApiPay.openid}&employeeid={employeeid}");
                }
                else
                {
                    return Redirect(url);
                }

            }
            catch (Exception exc)
            {
                return NotFound();
            }
        }
        #endregion

        #region 微信支付建立订单付款信息
        /// <summary>
        /// 微信支付建立订单付款信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [HttpPost("showregistrationmessage")]
        public IActionResult Product(string openid, decimal fAmt, int employeeid)
        {
            var orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //支付调用
            if (!string.IsNullOrEmpty(openid))
            {
                //支付金额
                //var fAmt = Convert.ToDecimal(1);
                if (fAmt == 0)
                {
                    return View();
                }
                else
                {
                    string url = $"http://{_setting.DomainName}/ShowWeiXinPay?openid=" + openid + "&total_fee=" + Convert.ToInt32(fAmt * 100) + "&orderNo=" + orderNo + "&employeeid=" + employeeid;
                    return Redirect(url);
                }

            }
            else
            {
                ViewData["error"] = "页面缺少参数，请返回重试";
                return View();
            }
        }

        #endregion

        #region 微信支付

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="total_fee">支付费用</param>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        [HttpGet("ShowWeiXinPay")]
        public IActionResult ShowWeiXInPay(string openid, string orderNo = "asdfsdgbadfgsdf", string employeeid = null, decimal fAmt = 0)
        {
            orderNo = commercialRepository.ProduceID(4);
            var userAgent = (HttpContext.Request.Headers as Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.FrameRequestHeaders).HeaderUserAgent.ToString().ToLower();
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openid))
            {
                ViewData["error"] = "页面传参出错,请返回重试";
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                return View();
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            var jsApiPay = new JsApiPay();
            jsApiPay.openid = openid;
            jsApiPay.total_fee = Convert.ToInt32(fAmt * 100);
            jsApiPay.notice_url = $"{_setting.DomainName}/registerresult";
            jsApiPay.out_trade_no = orderNo;
            jsApiPay.body = "奖励金";
            jsApiPay.attach = employeeid;
            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                var wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                ViewData["wxJsApiParam"] = wxJsApiParam;
            }
            catch (Exception ex)
            {
                ViewData["error"] = $"下单失败，请返回重试,{ex.Message}";
            }
            return View();
        }
        #endregion

        #region 微信回调
        /// <summary>
        /// 微信回调
        /// </summary>
        /// <returns></returns>
        [HttpPost("registerresult")]
        public string RegisterResult()
        {
            _log.Info($"------直接挂号方法调方法-------");

            var bytes = new Byte[Convert.ToInt32(Request.ContentLength)];
            try
            {
                Request.Body.Read(bytes, 0, bytes.Length);
                var content = System.Text.Encoding.UTF8.GetString(bytes);
                var backData = new WxPayData();
                backData.FromXml(content);
                var return_code = backData.GetValue("return_code")?.ToString().ToUpper();
                var result_code = backData.GetValue("result_code")?.ToString().ToUpper();
                //var Return_Code = backData.GetValue("return_code")?.ToString().ToUpper();
                var out_order_no = backData.GetValue("out_trade_no")?.ToString().ToUpper();
                var total_fee = backData.GetValue("total_fee")?.ToString();
                var openid = backData.GetValue("openid")?.ToString();
                var attach = backData.GetValue("attach")?.ToString();
                var transanctionID = backData.GetValue("transaction_id")?.ToString();
                if (return_code.ToUpper() == "SUCCESS" && result_code == "SUCCESS")
                {
                    var result = dsInfoRepository.QueryDsInfoRepository(out_order_no);
                    if (result == null || result.Count() < 1)
                    {
                        var dsInfo = new TQMDSDsInfo();
                        dsInfo.DsOpenID = openid;
                        dsInfo.EmployeeID = attach;
                        dsInfo.DsMoney = Convert.ToDecimal(total_fee) / 100;
                        dsInfo.OrderNo = out_order_no;
                        dsInfo.TransanctionID = transanctionID;
                        dsInfoRepository.AddDsInfoRepository(dsInfo);
                    }

                    var returnXML = @"<xml>
<return_code><![CDATA[SUCCESS]]></return_code>
<return_msg><![CDATA[OK]]></return_msg>
</xml>";
                    return returnXML;
                }
                else
                {
                    var returnXML = $@"<xml>
<return_code><![CDATA[FAIL]]></return_code>
<return_msg><![CDATA[]]></return_msg>
</xml>";
                    return returnXML;
                }
            }
            catch (Exception exc)
            {
                _log.Fatal($"未知的异常:{exc.Message}");
                var returnXML = $@"<xml>
<return_code><![CDATA[FAIL]]></return_code>
<return_msg><![CDATA[]]></return_msg>
</xml>";
                return returnXML;
            }
        }
    }
    #endregion
}
