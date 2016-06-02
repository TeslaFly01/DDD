using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace DDD.Utility
{
    /// <summary>
    /// Asp.net服务器控件帮助类
    /// </summary>
    public class CtrlHelper
    {
        public CtrlHelper()
        {

        }

        #region CheckBoxList

        /// <summary>
        /// 清空CheckBoxList选择的项
        /// </summary>
        /// <param name="ckb"></param>
        public static void CLSChkBoxList(CheckBoxList ckb)
        {
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                if (ckb.Items[i].Selected) ckb.Items[i].Selected = false;
            }
        }

        /// <summary>
        /// 全部选择CheckBoxList的项
        /// </summary>
        /// <param name="ckb"></param>
        public static void SelAllChkBoxList(CheckBoxList ckb)
        {
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                if (!ckb.Items[i].Selected) ckb.Items[i].Selected = true;
            }
        }

        /// <summary>
        /// CheckBoxList是否有选择的项
        /// </summary>
        /// <param name="ckb"></param>
        public static bool ChkBoxListHasSeled(CheckBoxList ckb)
        {
            bool re = false;
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                if (ckb.Items[i].Selected)
                {
                    re = true;
                    break;
                }
            }
            return re;
        }

        /// <summary>
        /// 取得CheckBoxList选择的项，以","累加
        /// </summary>
        /// <param name="ckb"></param>
        public static string GetChkBoxListSeled(CheckBoxList ckb)
        {
            string s = "";
            bool flag = false;
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                if (ckb.Items[i].Selected)
                {
                    flag = true;
                    s += ckb.Items[i].Value + ",";
                }
            }
            if (flag) s = s.Substring(0, s.LastIndexOf(","));
            return s;
        }


        /// <summary>
        /// 取得CheckBoxList选择的项，以"SepStr"累加
        /// </summary>
        /// <param name="ckb"></param>
        /// <param name="SepStr">分隔符号：如“,”， “|”等等</param>
        /// <returns></returns>
        public static string GetChkBoxListSeled(CheckBoxList ckb, string SepStr)
        {
            string s = "";
            bool flag = false;
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                if (ckb.Items[i].Selected)
                {
                    flag = true;
                    s += ckb.Items[i].Value + SepStr;
                }
            }
            if (flag) s = s.Substring(0, s.LastIndexOf(SepStr));
            return s;
        }

        /// <summary>
        /// 勾选项value匹配字符数组的项
        /// </summary>
        /// <param name="ckb"></param>
        /// <param name="ids"></param>
        public static void ShowChkBoxListSeled(CheckBoxList ckb, string[] ids)
        {
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                for (int j = 0; j < ids.Length; j++)
                {
                    if (ids[j] == ckb.Items[i].Value)
                    {
                        ckb.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 勾选项text匹配字符数组的项
        /// </summary>
        /// <param name="ckb"></param>
        /// <param name="ids"></param>
        public static void ShowChkBoxListSeledByText(CheckBoxList ckb, string[] ids)
        {
            for (int i = 0; i < ckb.Items.Count; i++)
            {
                for (int j = 0; j < ids.Length; j++)
                {
                    if (ids[j] == ckb.Items[i].Text)
                    {
                        ckb.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region RadioButtonList

        /// <summary>
        /// 选择RadiobuttonList匹配的项
        /// </summary>
        /// <param name="rdo"></param>
        /// <param name="myValue"></param>
        public static void ShowRdoSeled(RadioButtonList rdo, string myValue)
        {
            for (int i = 0; i < rdo.Items.Count; i++)
            {
                if (rdo.Items[i].Value == myValue)
                {
                    rdo.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion

        #region DropdownList

        /// <summary>
        /// 选择DropDownList匹配的项
        /// </summary>
        /// <param name="dpl"></param>
        /// <param name="myValue"></param>
        public static void ShowDplSeled(DropDownList dpl, string myValue)
        {
            for (int i = 0; i < dpl.Items.Count; i++)
            {
                if (dpl.Items[i].Value == myValue)
                {
                    dpl.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// 根据分割字符绑定DropDownList数据项
        /// </summary>
        /// <param name="dpl"></param>
        /// <param name="SepStr"></param>
        public static void DplDataBindBySepStr(DropDownList dpl, string[] SepStr)
        {
            for (int i = 0; i < SepStr.Length; i++)
            {
                dpl.Items.Add(new ListItem(SepStr[i], i.ToString()));
            }
        }

        #endregion

        #region 数据集控件

        /// <summary>
        /// 取得GridView的所有选择的Checkbox的值字符串
        /// </summary>
        /// <param name="dgIDs">返回字符串</param>
        /// <param name="GView"></param>
        /// <param name="chkBoxID">Checkbox的ID</param>
        /// <returns></returns>
        public static bool GetSeledIDs(out string dgIDs, GridView GView, string chkBoxID)
        {
            bool BxsChkd = false;
            dgIDs = "";
            foreach (GridViewRow i in GView.Rows)
            {
                CheckBox deleteChkBxItem = (CheckBox)i.FindControl(chkBoxID);
                if (deleteChkBxItem.Checked)
                {
                    BxsChkd = true;
                    dgIDs += GView.DataKeys[i.RowIndex].Value.ToString() + ",";
                }

            }
            if (BxsChkd) dgIDs = dgIDs.Substring(0, dgIDs.LastIndexOf(","));

            return BxsChkd;
        }
        /// <summary>
        /// 取得DataList的所有选择的Checkbox的值字符串
        /// </summary>
        /// <param name="dgIDs"></param>
        /// <param name="DList"></param>
        /// <param name="chkBoxID"></param>
        /// <returns></returns>
        public static bool GetSeledIDs(out string dgIDs, DataList DList, string chkBoxID)
        {
            bool BxsChkd = false;
            dgIDs = "";
            foreach (DataListItem i in DList.Items)
            {
                CheckBox deleteChkBxItem = (CheckBox)i.FindControl(chkBoxID);
                if (deleteChkBxItem.Checked)
                {
                    BxsChkd = true;
                    dgIDs += DList.DataKeys[i.ItemIndex].ToString() + ",";
                }

            }
            if (BxsChkd) dgIDs = dgIDs.Substring(0, dgIDs.LastIndexOf(","));

            return BxsChkd;
        }

        #endregion
    }
}
