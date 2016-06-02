using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DDD.BusinessServiceEngine.Common;
using DDD.Utility;

namespace DDD.BusinessServiceEngine
{
    public partial class Form1 : Form
    {
        static int _SleepQuery; //查询间隔指程序每一次处理完一定数据（每查询数）后，休眠多少毫秒再去查询本地数据库，数据变化不多时不要设得太小，设置范围最小1000（1000毫秒=1秒）

        static string _ServiceAddr = string.Empty;//服务基地址
        static string _authKey = string.Empty;//简单身份key
        static bool _InSettingConfig = false; //导入配置信息标志 - 导入过就不必再次导入


        static int _remindQueryCount;//每一次从数据库查询的商家未发起活动服务提醒的数量
        static int _remainStandbyCount;//提交提醒服务列表小于剩余多少数量时，需要再次查询数据库取得新增队列记录。该设置与_XddNotProjectRemindQueryCount 相对应
        static int _reminTaskNum;//提醒服务工作线程数量

        private static bool _noticeFlag = false;//提醒服务运行标志
        private static DateTime? _noticeDateTime = null;//提醒服务时间
        static ProducerConsumer<string> _producerConsumer = null;
        static object _noticeLock = new object();
        const int _GetQueryRetryTimes = 9;//获取数据库队列失败连续重试次数
        static int _SleepPost;//服务提交间隔指每次向服务器提交后再次提交的等待时间，设置范围必须大于等于100毫秒, 可以根据服务器情况快慢调整

        public Form1()
        {
            InitializeComponent();
        }

        #region 写日志
        private void AddMsg(string msg)
        {
            richtxt_Info.AppendText(msg + "\n");
        }

        private void Logging(string msg, string stackTrace = "")
        {
            new Task(() =>
            {
                msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg;
                Invoke(new Action(() => AddMsg(msg)));
                LogFileHelper.InsertMsg(msg + (string.IsNullOrWhiteSpace(stackTrace) ? "" : stackTrace));
            }).Start();
        }
        #endregion

        #region 导入配置信息
        private void AppConfigInput()
        {
            if (!_InSettingConfig)
            {
                try
                {
                    _ServiceAddr = ConfigurationManager.AppSettings["ServiceAddr"].ToString();
                    _authKey = ConfigurationManager.AppSettings["authKey"].ToString();

                    _SleepQuery = int.Parse(ConfigurationManager.AppSettings["SleepQuery"].ToString());
                    _SleepPost = int.Parse(ConfigurationManager.AppSettings["SleepPost"].ToString());

                    _remainStandbyCount = int.Parse(ConfigurationManager.AppSettings["RemindRemainStandbyCount"]);
                    _remindQueryCount = int.Parse(ConfigurationManager.AppSettings["RemindQueryCount"]);
                    _reminTaskNum = int.Parse(ConfigurationManager.AppSettings["RemindTaskNum"]);

                    _InSettingConfig = true;
                    Logging("导入配置信息成功!");
                }
                catch
                {
                    Logging("导入配置信息出错!");
                    throw new InvalidOperationException("导入配置信息出错!");
                }
            }
        }
        #endregion

        #region 格式化状态信息、按钮状态
        private void SetlblState(Label lblName, string text, Color color)
        {
            lblName.Text = text;
            lblName.ForeColor = color;
        }

        private void SetbtnState(Button btnName, string text, bool enable)
        {
            btnName.Text = text;
            btnName.Enabled = enable;
        }
        #endregion

        private void btn_Satrt_Click(object sender, EventArgs e)
        {
            if (!_noticeFlag)
            {
                _noticeFlag = true;
                _noticeDateTime = DateTime.Now;
                Logging("启动[提醒]服务");
                SetbtnState(btn_Satrt, "停止[提醒]服务", true);
                SetlblState(lbl_State, "[提醒]扫描服务运行中(" + _noticeDateTime + ")", Color.Green);

                //重置提醒扫描状态：正在扫描--> 等到扫描
                var resetStandbyTask = new Task<bool>(() =>
                {
                    var isReset = false;
                    try
                    {
                        isReset = ResetAllRemindScan();
                    }
                    catch
                    {
                    }
                    return isReset;
                });
                resetStandbyTask.Start();
                resetStandbyTask.Wait();
                var isSuccess = resetStandbyTask.Result;
                if (isSuccess)
                {
                    Logging("重置\"扫描中\"的[提醒]扫描服务为等待扫描成功！");
                }
                else
                {
                    Logging("[提醒]扫描服务启动失败：重置\"扫描中\"的[提醒]扫描服务为等待扫描失败！");

                    _noticeFlag = false;
                    SetbtnState(btn_Satrt, "启动[提醒]扫描服务", true);
                    SetlblState(lbl_State, "[提醒]扫描服务停止", Color.Red);
                    return;
                }

                //初始化相关变量、工作线程
                //等到重置“发送中”任务结束后，才能执行下面任务：
                //获取[未提醒]列表数据,为[发起提醒]生产者提供生产方法
                var getRetryTimes = 0;
                Func<List<string>> ProduceFunc = () =>
                {
                    //休眠获取[提醒]列表数据，主要为了间断查询。
                    Thread.Sleep(_SleepQuery);
                    var unList = new List<string>();
                    try
                    {
                        unList = GetTaskList();
                        if (unList.Count > 0)
                        {
                            Logging("获取[提醒]队列成功！");
                        }
                        getRetryTimes = 0;
                    }
                    catch (Exception ex)
                    {
                        getRetryTimes = getRetryTimes + 1;
                        if (getRetryTimes >= _GetQueryRetryTimes)
                        {
                            _noticeFlag = false;
                            Logging(
                                "[提醒]服务意外停止：获取[未提醒]队列连续失败" + getRetryTimes + "次！|| 错误原因：" +
                                (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()),
                                (string.IsNullOrWhiteSpace(ex.StackTrace) ? "" : ex.StackTrace));
                            ChangeAllServerState();
                            if (_producerConsumer != null)
                            {
                                _producerConsumer.Stop();
                            }
                        }
                        else
                        {
                            Logging(
                                "获取[未提醒]队列连续失败" + getRetryTimes + "次！|| 错误原因：" +
                                (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()),
                                (string.IsNullOrWhiteSpace(ex.StackTrace) ? "" : ex.StackTrace));
                        }
                    }
                    return unList;
                };

                int consumeRetryTimes = 0;
                //为[未提醒]消费者提供消费方法
                Action<string> notNoticeAction = item =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        long durTime = -1;
                        var durTimeUnit = "毫秒";
                        var durTimer = new Stopwatch();
                        durTimer.Start();
                        try
                        {
                            //执行活动提交服务
                            var result = PostRemind(item);
                            durTimer.Stop();
                            durTime = durTimer.ElapsedMilliseconds;
                            if (durTime >= 1000)
                            {
                                durTimeUnit = "秒";
                                durTime = durTime / 1000;
                            }
                            lock (_noticeLock)
                            {
                                consumeRetryTimes = 0;
                            }
                            Logging("[未提醒]提交成功." + "(商家ID:" + item + "，运行结果:" + (result ? "提醒成功" : "提醒失败") + "，耗时:" +
                                    durTime.ToString() + durTimeUnit + ")");
                        }
                        catch (Exception ex)
                        {
                            lock (_noticeLock)
                            {
                                consumeRetryTimes++;
                                if (consumeRetryTimes >= _GetQueryRetryTimes)
                                {
                                    _noticeFlag = false;
                                    Logging(
                                        "[未提醒]服务意外停止:提交提醒服务连续失败" + consumeRetryTimes + "次![其中包含ID：" + item +
                                        "]  || 错误原因：" +
                                        (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()),
                                        (string.IsNullOrWhiteSpace(ex.StackTrace) ? "" : ex.StackTrace));
                                    ChangeAllServerState();
                                    if (_producerConsumer != null)
                                    {
                                        _producerConsumer.Stop();
                                    }
                                }
                                else
                                {
                                    Logging(
                                        "[未提醒]提交提醒服务连续失败" + consumeRetryTimes + "次![其中包含商家ID:" + item +
                                        "] || 错误原因：" +
                                        (ex.InnerException == null ? ex.Message : ex.InnerException.ToString()),
                                        (string.IsNullOrWhiteSpace(ex.StackTrace) ? "" : ex.StackTrace));
                                }
                            }
                        }
                        Thread.Sleep(_SleepPost);
                    }
                };
                //构造生产消费模式，初始化如生产方法，消费方法，指定消费者数量
                _producerConsumer = new ProducerConsumer<string>(ProduceFunc,
                    notNoticeAction, _remainStandbyCount,
                    _reminTaskNum);
                _producerConsumer.Start();
            }
            else //停止
            {
                _noticeFlag = false;
                if (_producerConsumer != null)
                {
                    _producerConsumer.Stop();
                }
                Logging("手动停止[未提醒]扫描服务");

                SetbtnState(btn_Satrt, "启动[提醒]扫描服务", true);
                SetlblState(lbl_State, "[提醒]扫描服务停止", Color.Red);
            }
        }

        //重置提醒扫描状态：正在扫描--> 等到扫描
        private bool ResetAllRemindScan()
        {
            var url = _ServiceAddr + "NotNotice/ResetAllNotNoticeRemindScan";
            string json = string.Empty;
            HttpWebResponse response = null;
            Stream dataStream = null;
            StreamReader sr = null;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 1000 * 60 * 3; //重置扫描标志服务超时：3分钟
                request.Headers.Add("Authorization", _authKey);
                request.ContentType = "application/json";
                request.ContentLength = 0;
                response = (HttpWebResponse)request.GetResponse();//开始执行鸟
                dataStream = response.GetResponseStream();
                sr = new StreamReader(dataStream, Encoding.UTF8);
                json = sr.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                var foo = js.Deserialize<bool>(json);
                return foo;
            }
            catch
            {
                throw new InvalidOperationException("重置[提醒]队列扫描状态为“等待扫描”失败！");
            }
            finally
            {
                if (sr != null) { sr.Close(); }
                if (dataStream != null) { dataStream.Close(); }
                if (response != null) { response.Close(); }
            }

        }

        //取得远程队列
        private List<string> GetTaskList()
        {
            var url = _ServiceAddr + "NotNotice/GetNotNoticeTaskList/" +
                      _remindQueryCount.ToString();
            string json = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 1000 * 60 * 5;
                request.Headers.Add("Authorization", _authKey);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string code = response.ContentType;
                    code = code.Split('=')[1];//得到编码 
                    using (Stream stream = response.GetResponseStream())
                    {
                        var sr = new StreamReader(stream, Encoding.GetEncoding(code));
                        json = sr.ReadToEnd();
                    }
                }
                if (json == "[]") { return new List<string>(); }
                var js = new JavaScriptSerializer();
                var foo = js.Deserialize<List<string>>(json);
                return foo;
            }
            catch
            {
                throw new InvalidOperationException("获取[提醒]队列连续失败");
            }
        }

        private void ChangeAllServerState()
        {
            new Task(() => this.Invoke(new Action(() =>
            {
                SetbtnState(btn_Satrt, "启动[提醒]扫描服务", true);
                SetlblState(lbl_State, "[提醒]扫描服务停止", Color.Red);
            }))).Start();
        }

        //提交提醒执行
        private bool PostRemind(string corpId)
        {
            var url = _ServiceAddr + "NotNotice/RunNotProjectRemind/" + corpId; //get方式
            string json = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 1000 * 60 * 3;
                request.Headers.Add("Authorization", _authKey);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//开始执行鸟
                {
                    string code = response.ContentType;
                    code = code.Split('=')[1];//得到编码 
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(code));
                        json = sr.ReadToEnd();
                    }
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                var foo = js.Deserialize<bool>(json);
                return foo;
            }
            catch (Exception)
            {
                throw new Exception("提交[提醒]服务失败！");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                AppConfigInput();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(@"启动失败：" + ex.Message, @"错误");
                this.Close();
            }
        }

    }
}
