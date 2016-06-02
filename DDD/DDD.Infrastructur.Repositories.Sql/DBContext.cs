using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Model.Entities.Admin;

namespace DDD.Infrastructur.Repositories.Sql
{
    public class DBContext : DbContext
    {
        private const string CONNECTION_STRING = "DbConn";
        //static SqlConnection Sqlconn_test = new SqlConnection(@"Server=192.168.1.208\sql2k8;DataBase=wquan;Uid=wquan;pwd=wquan!@#");
        public DBContext()
            : base(CONNECTION_STRING)
        {
            Database.SetInitializer<DBContext>(null);//数据库结构修改后，不自动重建数据库，避免数据丢失 (不需要根据实体生成数据库时，检查是否存在同名的数据库)
            Database.CommandTimeout = 120;//默认是30s
        }
        //base(Sqlconn_wquan, true) { } 

        //动态数据库链接字符串
        public DBContext(string DbConn)
            : base(DbConn)
        {
            Database.SetInitializer<DBContext>(null);
            Database.CommandTimeout = 120;//默认是30s
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<SystemAdmin> SystemAdmin { get; set; }
        public DbSet<AdminAction> AdminAction { get; set; }
        public DbSet<AdminLog> AdminLog { get; set; }
        public DbSet<AdminModule> AdminModule { get; set; }
        public DbSet<AdminRole> AdminRole { get; set; }
        public DbSet<AdminRole_Module> AdminRole_Module { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 移除EF的表名公约   
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // 移除对MetaData表的查询验证 
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            //关闭外键级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //用户和部门多对一关系
            modelBuilder.Entity<UserInfo>().HasRequired(c => c.Department).WithMany().HasForeignKey(a => a.DeptId);

            //系统管理员与管理员角色的多对多关联
            modelBuilder.Entity<SystemAdmin>()
           .HasMany(u => u.AdminRoles)
           .WithMany(r => r.SystemAdmins)
           .Map(m =>
           {
               m.ToTable("Admin_Role");
               m.MapLeftKey("SAID");
               m.MapRightKey("ARID");
           });

            modelBuilder.Entity<AdminRole>().HasMany(a => a.AdminRole_Modules).WithRequired(p => p.adminRole);
            modelBuilder.Entity<AdminModule>().HasMany(a => a.AdminRole_Modules).WithRequired(p => p.adminModule);

            //管理员功能模块自身主外键关系
            modelBuilder.Entity<AdminModule>().HasRequired(a => a.FAdminModule).WithMany().HasForeignKey(a => a.FID);

        }
    }
}
