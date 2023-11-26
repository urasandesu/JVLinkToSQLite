namespace JVLinkToSQLite
{
    partial class SetupForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSetUIProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSetUIProperties
            // 
            this.buttonSetUIProperties.Location = new System.Drawing.Point(10, 10);
            this.buttonSetUIProperties.Name = "buttonSetUIProperties";
            this.buttonSetUIProperties.Size = new System.Drawing.Size(199, 45);
            this.buttonSetUIProperties.TabIndex = 0;
            this.buttonSetUIProperties.Text = "設定";
            this.buttonSetUIProperties.UseVisualStyleBackColor = true;
            this.buttonSetUIProperties.Click += new System.EventHandler(this.buttonSetUIProperties_Click);
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 66);
            this.Controls.Add(this.buttonSetUIProperties);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "初期化モード";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupForm_FormClosed);
            this.Load += new System.EventHandler(this.SetupForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSetUIProperties;
    }
}

