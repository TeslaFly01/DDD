using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.Service;
using DDD.Domain;
using DDD.Domain.MainModule.Test;
using DDD.Domain.MainModule.Test.Specification;
using DDD.Domain.Model.Entities.Admin;
using DDD.Domain.Specification;
using DDD.Infrastructur.Repositories;
using DDD.Utility;

namespace DDD.Application.Service.BusinessService
{
    public class UserInfoService : BaseServices<UserInfo>, IUserInfoService
    {
        private IUserInfoDomainService _userInfoDomainService;
        private IUserInfoRepository _userInfoRepository;
        static object _myLock = new object();//排他锁
        public UserInfoService(IUnitOfWork unitOfWork, IUserInfoRepository repository, IUserInfoDomainService userInfoDomainService, IUserInfoRepository userInfoRepository)
            : base(unitOfWork, repository)
        {
            _userInfoDomainService = userInfoDomainService;
            _userInfoRepository = userInfoRepository;
        }

        public List<UserInfo> GetList(string name, int? sex)
        {
            UserInfoSpecification spec = new UserInfoSpecification(name, sex);
            return _userInfoDomainService.GetList(spec);
        }

        /// <summary>
        /// 重置未提醒扫描状态：正在扫描=>等待扫描
        /// </summary>
        /// <returns></returns>
        public bool ResetNotNoticeScanningToWait()
        {
            try
            {
                using (var scope = TransactionUtilities.CreateTransactionScopeWithNoLock())
                {
                    _userInfoRepository.ChangeScanningToWait();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取未提醒的ID列表
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public List<string> GetNotNoticeList(int topNum)
        {
            return _userInfoDomainService.GetNotNoticeList(topNum);
        }

        /// <summary>
        /// 提交未提醒
        /// </summary>
        /// <param name="strId"></param>
        public void PostNotNotice(string strId)
        {
            var entity = new UserInfo();
            try
            {
                using (var scope = TransactionUtilities.CreateTransactionScopeWithNoLock())
                {
                    var id = int.Parse(strId);
                    entity =
                        _userInfoRepository.GetByCondition(
                            new DirectSpecification<UserInfo>(
                                x => x.ID == id));
                    entity.ScanFlag = 3;
                    //entity.NoticeTime = DateTime.Now;
                    base.Update(entity, x => x.ScanFlag);
                    scope.Complete();
                }
            }
            catch (InvalidOperationException e)
            {
                lock (_myLock)
                {
                    LoggerHelper.Log("【发起提醒】失败，失败原因:" +(e.InnerException == null ? e.Message : e.InnerException.ToString()));
                }
            }
            catch (Exception ex)
            {
                lock (_myLock)
                {
                    LoggerHelper.Log("【未提醒】失败，失败原因:" +(ex.InnerException == null ? ex.Message : ex.InnerException.ToString()));
                    //避免数据库异常下，无法记录错误日志
                    entity.ScanFlag =3;
                    base.Update(entity, x => x.ScanFlag);
                }
            }
        }
    }
}
