
namespace AddFriend
{
    partial class wndAddFriend
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
            this.bAddFriend = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.gbPublicKey = new System.Windows.Forms.GroupBox();
            this.tbPublicKey = new System.Windows.Forms.TextBox();
            this.pMenu.SuspendLayout();
            this.gbPublicKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.bCancel);
            this.pMenu.Controls.Add(this.bAddFriend);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pMenu.Location = new System.Drawing.Point(0, 241);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(520, 38);
            this.pMenu.TabIndex = 0;
            // 
            // bAddFriend
            // 
            this.bAddFriend.Dock = System.Windows.Forms.DockStyle.Left;
            this.bAddFriend.Location = new System.Drawing.Point(0, 0);
            this.bAddFriend.Name = "bAddFriend";
            this.bAddFriend.Size = new System.Drawing.Size(238, 38);
            this.bAddFriend.TabIndex = 0;
            this.bAddFriend.Text = "Предоставить доступ";
            this.bAddFriend.UseVisualStyleBackColor = true;
            this.bAddFriend.Click += new System.EventHandler(this.bAddFriend_Click);
            // 
            // bCancel
            // 
            this.bCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bCancel.Location = new System.Drawing.Point(288, 0);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(232, 38);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Отмена";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // gbPublicKey
            // 
            this.gbPublicKey.Controls.Add(this.tbPublicKey);
            this.gbPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPublicKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbPublicKey.Location = new System.Drawing.Point(0, 0);
            this.gbPublicKey.Name = "gbPublicKey";
            this.gbPublicKey.Size = new System.Drawing.Size(520, 241);
            this.gbPublicKey.TabIndex = 1;
            this.gbPublicKey.TabStop = false;
            this.gbPublicKey.Text = "Публичный ключ:";
            // 
            // tbPublicKey
            // 
            this.tbPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPublicKey.Location = new System.Drawing.Point(3, 20);
            this.tbPublicKey.Multiline = true;
            this.tbPublicKey.Name = "tbPublicKey";
            this.tbPublicKey.Size = new System.Drawing.Size(514, 218);
            this.tbPublicKey.TabIndex = 0;
            this.tbPublicKey.TextChanged += new System.EventHandler(this.tbPublicKey_TextChanged);
            // 
            // wndAddFriend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 279);
            this.Controls.Add(this.gbPublicKey);
            this.Controls.Add(this.pMenu);
            this.Name = "wndAddFriend";
            this.Text = "Добавить пользователя к папке";
            this.pMenu.ResumeLayout(false);
            this.gbPublicKey.ResumeLayout(false);
            this.gbPublicKey.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bAddFriend;
        private System.Windows.Forms.GroupBox gbPublicKey;
        private System.Windows.Forms.TextBox tbPublicKey;
    }
}

