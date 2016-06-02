using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace DDD.Utility
{
    /// <summary>
    /// TreeView控件找节点
    /// </summary>
    public class TreeViewHelper
    {
        public static TreeNode FindNode(TreeNode tnParent, TreeNode tnSrch)
        {
            if (tnParent == null) return null;
            if (tnParent.Text == tnSrch.Text && tnParent.Value == tnSrch.Value)
            {
                tnParent.Selected = true;
                return tnParent;
            }
            TreeNode tnRet = null;
            foreach (TreeNode tn in tnParent.ChildNodes)
            {
                tnRet = FindNode(tn, tnSrch);
                if (tnRet != null) break;
            }
            return tnRet;
        }
    }

}
