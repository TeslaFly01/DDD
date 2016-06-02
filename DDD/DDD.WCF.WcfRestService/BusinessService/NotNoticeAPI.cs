using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using DDD.Application.Service.BusinessService;
using DDD.WCF.WcfRestService.Common;

namespace DDD.WCF.WcfRestService.BusinessService
{
    [Description("未发起提醒服务API")]
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class NotNoticeAPI
    {
        private readonly IUserInfoService _userInfoService;
        public const string NotNoticeRouteName = "NotNotice";

        public NotNoticeAPI(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }


        /// <summary>
        /// 重置所有未提醒服务扫描标志
        /// </summary>
        /// <returns></returns>
        [Description("重置所有未提醒服务扫描标志")]
        [WebInvoke(UriTemplate = "ResetAllNotNoticeRemindScan", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public bool ResetAllNotNoticeRemindScan()
        {
            try
            {
                return _userInfoService.ResetNotNoticeScanningToWait();
            }
            catch
            {
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [Description("获取需要更新状态的未提醒服务任务队列")]
        [WebGet(UriTemplate = "GetNotNoticeTaskList/{scanNum}", ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetNotNoticeTaskList(string scanNum)
        {
            try
            {
                return _userInfoService.GetNotNoticeList(int.Parse(scanNum));
            }
            catch (Exception e)
            {
                LogFileHelper.InsertMsg("[获取需要更新状态的未提醒服务任务队列]出错" + " || 错误原因：" +
                                        (e.InnerException == null
                                             ? e.Message
                                             : e.InnerException.ToString()) +
                                               (string.IsNullOrWhiteSpace(e.StackTrace) ? "" : " || 堆栈信息：" + e.StackTrace));

                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            }
        }

        [Description("提交未提醒服务")]
        [WebGet(UriTemplate = "RunNotProjectRemind/{id}", ResponseFormat = WebMessageFormat.Json)]
        public bool RunNotProjectRemind(string id)
        {
            try
            {
                _userInfoService.PostNotNotice(id);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception)
            {
                throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}