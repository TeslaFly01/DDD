using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Data;

namespace DDD.Utility
{
    /// <summary>
    /// DropDownListTree下拉列表框树状
    /// </summary>
    public class DropDownListTree
    {
        private System.Web.UI.WebControls.DropDownList selp;
        private DataTable _dt;
        private string _pid;
        private string _id;
        private string _name;
        private string _root;
        private int _rootvalue;

        /// <summary>
        /// 树型下拉控件类
        /// </summary>
        /// <param name="selp">要绑定的服务器端运行的下拉框</param>
        /// <param name="_dt">具有无限级分类特性的数据集</param>
        /// <param name="_pid">字段名：父级ID</param>
        /// <param name="_id">字段名：标识id</param>
        /// <param name="_name">字段名：名称</param>
        /// <param name="_root">根的Text</param>
        /// <param name="_rootvalue">根的Value</param>
        public DropDownListTree(DropDownList selp, DataTable _dt, string _pid, string _id, string _name, string _root, int _rootvalue)
        {
            this.selp = selp;
            this._dt = _dt;
            this._pid = _pid;
            this._id = _id;
            this._name = _name;
            this._root = _root;
            this._rootvalue = _rootvalue;
        }


        public void bind_select()//绑定方法
        {
            this.selp.Items.Clear();
            this.selp.Items.Add(new ListItem("├" + this._root, this._rootvalue.ToString()));
            bindtoSelect(this._rootvalue, this._dt, "├");
        }
        protected void bindtoSelect(int pid, DataTable dt, string header)//递归
        {
            //DataRow[] rows = dt.Select(this._pid + "=" + pid);
            DataRow[] rows = dt.Select(this._pid + "=" + " '" + pid.ToString() + "'");
            header += "┴";
            foreach (DataRow row in rows)
            {
                this.selp.Items.Add(new ListItem(header + Convert.ToString(row[this._name]), Convert.ToString(row[this._id])));
                bindtoSelect(Convert.ToInt32(row[this._id]), dt, header);
            }
        }
    }
}
