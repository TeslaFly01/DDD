using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.MainModule.Admin;
using DDD.Domain.Model.Entities.Admin;
using DDD.Utility;

namespace DDD.Infrastructur.Repositories.Sql.MainModule.Admin
{
    public class SystemAdminRepository : RepositoryBase<SystemAdmin>, ISystemAdminRepository
    {
        public SystemAdminRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Enable(int said, bool isEnable)
        {
            var Enablesysadmin = new SystemAdmin() { SAID = said, IsEnable = isEnable, CurrentIP = "null", LastIP = "null", SAMobileNo = "null", SAPwd = "null", SAName = "null", SANickName = "null", Email = "cc@cc.cc" };
            base.dbset.Attach(Enablesysadmin);
            var updateEntry = ((IObjectContextAdapter)base.DataContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(Enablesysadmin);
            updateEntry.SetModifiedProperty("IsEnable");
        }


        public void UpdateSysAdmin(SystemAdmin admin)
        {
            SystemAdmin entity = base.GetByKey(admin.SAID);
            DbEntityEntry<SystemAdmin> entry = base.DataContext.Entry(entity);
            entity.AdminRoles.Clear();//导航属性必须先清空
            entry.Collection(a => a.AdminRoles).CurrentValue = admin.AdminRoles;

            entry.Property(a => a.SANickName).CurrentValue = admin.SANickName;
            entry.Property(a => a.SASex).CurrentValue = admin.SASex;
            entry.Property(a => a.SAMobileNo).CurrentValue = admin.SAMobileNo;
            entry.Property(a => a.Email).CurrentValue = admin.Email;
            entry.Property(a => a.IsEnable).CurrentValue = admin.IsEnable;
            entry.Property(a => a.SAPwd).CurrentValue = admin.SAPwd;

            //entity.SASex = admin.SASex;
            //Update(entity, x => x.SANickName, x => x.SASex, x => x.SAMobileNo, x => x.Email);
        }


        public void EditCurr(SystemAdmin admin)
        {
            base.Update(admin, ec => ec.SANickName, ec => ec.SAMobileNo, ec => ec.SASex, ec => ec.Email);
        }

        public void UpdateLogonInfo(SystemAdmin admin)
        {
            admin.LastIP = admin.CurrentIP;
            admin.LastTime = admin.CurrentTime;
            admin.CurrentIP = IPHelper.getIPAddr();//读取当前ip
            admin.CurrentTime = DateTime.Now;
            admin.LoginTimes = admin.LoginTimes + 1;
            base.Update(admin, le => le.LastIP, le => le.LastTime, le => le.CurrentIP, le => le.CurrentTime, le => le.LoginTimes);
        }


        public void ChangePwd(SystemAdmin admin, string newPwd)
        {
            admin.SAPwd = newPwd;
            base.Update(admin, cg => cg.SAPwd);
        }


        public string GetIPBysysAdmin(int said)
        {
            var result = (from i in base.dbset
                          where i.SAID == said
                          select new { currIp = i.CurrentIP }).FirstOrDefault();
            return result.currIp;
        }
    }
}
