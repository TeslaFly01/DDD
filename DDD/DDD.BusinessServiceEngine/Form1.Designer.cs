namespace DDD.BusinessServiceEngine
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.richtxt_Info = new System.Windows.Forms.RichTextBox();
            this.btn_Satrt = new System.Windows.Forms.Button();
            this.lbl_State = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richtxt_Info
            // 
            this.richtxt_Info.Location = new System.Drawing.Point(12, 12);
            this.richtxt_Info.Name = "richtxt_Info";
            this.richtxt_Info.Size = new System.Drawing.Size(497, 317);
            this.richtxt_Info.TabIndex = 0;
            this.richtxt_Info.Text = "";
            // 
            // btn_Satrt
            // 
            this.btn_Satrt.Location = new System.Drawing.Point(530, 12);
            this.btn_Satrt.Name = "btn_Satrt";
            this.btn_Satrt.Size = new System.Drawing.Size(119, 51);
            this.btn_Satrt.TabIndex = 1;
            this.btn_Satrt.Text = "开始";
            this.btn_Satrt.UseVisualStyleBackColor = true;
            this.btn_Satrt.Click += new System.EventHandler(this.btn_Satrt_Click);
            // 
            // lbl_State
            // 
            this.lbl_State.AutoSize = true;
            this.lbl_State.Location = new System.Drawing.Point(27, 346);
            this.lbl_State.Name = "lbl_State";
            this.lbl_State.Size = new System.Drawing.Size(41, 12);
            this.lbl_State.TabIndex = 2;
            this.lbl_State.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 458);
            this.Controls.Add(this.lbl_State);
            this.Controls.Add(this.btn_Satrt);
            this.Controls.Add(this.richtxt_Info);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richtxt_Info;
        private System.Windows.Forms.Button btn_Satrt;
        private System.Windows.Forms.Label lbl_State;
    }
}

