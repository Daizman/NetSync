
namespace CreateFolder
{
    partial class wndCreateFolder
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pMenu = new System.Windows.Forms.Panel();
            this.bChoosePath = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bCreate = new System.Windows.Forms.Button();
            this.lFolderPath = new System.Windows.Forms.Label();
            this.fbdPath = new System.Windows.Forms.FolderBrowserDialog();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.bChoosePath);
            this.pMenu.Controls.Add(this.bCancel);
            this.pMenu.Controls.Add(this.bCreate);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pMenu.Location = new System.Drawing.Point(0, 93);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(451, 43);
            this.pMenu.TabIndex = 0;
            // 
            // bChoosePath
            // 
            this.bChoosePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bChoosePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bChoosePath.Location = new System.Drawing.Point(128, 0);
            this.bChoosePath.Margin = new System.Windows.Forms.Padding(10);
            this.bChoosePath.Name = "bChoosePath";
            this.bChoosePath.Size = new System.Drawing.Size(192, 43);
            this.bChoosePath.TabIndex = 2;
            this.bChoosePath.Text = "Указать путь";
            this.bChoosePath.UseVisualStyleBackColor = true;
            this.bChoosePath.Click += new System.EventHandler(this.bChoosePath_Click);
            // 
            // bCancel
            // 
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Location = new System.Drawing.Point(320, 0);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(131, 43);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bCreate
            // 
            this.bCreate.Dock = System.Windows.Forms.DockStyle.Left;
            this.bCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCreate.Location = new System.Drawing.Point(0, 0);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(128, 43);
            this.bCreate.TabIndex = 0;
            this.bCreate.Text = "Создать";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // lFolderPath
            // 
            this.lFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lFolderPath.AutoSize = true;
            this.lFolderPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lFolderPath.Location = new System.Drawing.Point(224, 27);
            this.lFolderPath.Name = "lFolderPath";
            this.lFolderPath.Size = new System.Drawing.Size(0, 22);
            this.lFolderPath.TabIndex = 1;
            // 
            // wndCreateFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 136);
            this.Controls.Add(this.lFolderPath);
            this.Controls.Add(this.pMenu);
            this.Name = "wndCreateFolder";
            this.Text = "Создание папки";
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.Button bChoosePath;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bCreate;
        private System.Windows.Forms.Label lFolderPath;
        private System.Windows.Forms.FolderBrowserDialog fbdPath;
    }
}

