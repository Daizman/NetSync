
namespace NetSync
{
    partial class wndNetSync
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
            this.pFolderSpace = new System.Windows.Forms.Panel();
            this.lbFolderSpace = new System.Windows.Forms.ListBox();
            this.gbFriends = new System.Windows.Forms.GroupBox();
            this.lbFriends = new System.Windows.Forms.ListBox();
            this.gbMenu = new System.Windows.Forms.GroupBox();
            this.bCreateDir = new System.Windows.Forms.Button();
            this.bAddFriend = new System.Windows.Forms.Button();
            this.bDelFile = new System.Windows.Forms.Button();
            this.bAddFile = new System.Windows.Forms.Button();
            this.gbPublicKey = new System.Windows.Forms.GroupBox();
            this.tbPublicKey = new System.Windows.Forms.TextBox();
            this.fswTracker = new System.IO.FileSystemWatcher();
            this.ofdAdd = new System.Windows.Forms.OpenFileDialog();
            this.pMenu.SuspendLayout();
            this.pFolderSpace.SuspendLayout();
            this.gbFriends.SuspendLayout();
            this.gbMenu.SuspendLayout();
            this.gbPublicKey.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fswTracker)).BeginInit();
            this.SuspendLayout();
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.gbPublicKey);
            this.pMenu.Controls.Add(this.gbMenu);
            this.pMenu.Controls.Add(this.gbFriends);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.pMenu.Location = new System.Drawing.Point(582, 0);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(242, 531);
            this.pMenu.TabIndex = 0;
            // 
            // pFolderSpace
            // 
            this.pFolderSpace.Controls.Add(this.lbFolderSpace);
            this.pFolderSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFolderSpace.Location = new System.Drawing.Point(0, 0);
            this.pFolderSpace.Name = "pFolderSpace";
            this.pFolderSpace.Size = new System.Drawing.Size(582, 531);
            this.pFolderSpace.TabIndex = 1;
            // 
            // lbFolderSpace
            // 
            this.lbFolderSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFolderSpace.FormattingEnabled = true;
            this.lbFolderSpace.Location = new System.Drawing.Point(0, 0);
            this.lbFolderSpace.Name = "lbFolderSpace";
            this.lbFolderSpace.Size = new System.Drawing.Size(582, 531);
            this.lbFolderSpace.TabIndex = 0;
            // 
            // gbFriends
            // 
            this.gbFriends.Controls.Add(this.lbFriends);
            this.gbFriends.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbFriends.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbFriends.Location = new System.Drawing.Point(0, 313);
            this.gbFriends.Name = "gbFriends";
            this.gbFriends.Size = new System.Drawing.Size(242, 218);
            this.gbFriends.TabIndex = 0;
            this.gbFriends.TabStop = false;
            this.gbFriends.Text = "Пользователи с доступом:";
            // 
            // lbFriends
            // 
            this.lbFriends.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFriends.FormattingEnabled = true;
            this.lbFriends.ItemHeight = 18;
            this.lbFriends.Location = new System.Drawing.Point(3, 20);
            this.lbFriends.Name = "lbFriends";
            this.lbFriends.Size = new System.Drawing.Size(236, 195);
            this.lbFriends.TabIndex = 0;
            // 
            // gbMenu
            // 
            this.gbMenu.Controls.Add(this.bAddFile);
            this.gbMenu.Controls.Add(this.bDelFile);
            this.gbMenu.Controls.Add(this.bAddFriend);
            this.gbMenu.Controls.Add(this.bCreateDir);
            this.gbMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbMenu.Location = new System.Drawing.Point(0, 179);
            this.gbMenu.Name = "gbMenu";
            this.gbMenu.Size = new System.Drawing.Size(242, 134);
            this.gbMenu.TabIndex = 1;
            this.gbMenu.TabStop = false;
            this.gbMenu.Text = "Управление директорией";
            // 
            // bCreateDir
            // 
            this.bCreateDir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bCreateDir.Location = new System.Drawing.Point(3, 103);
            this.bCreateDir.Name = "bCreateDir";
            this.bCreateDir.Size = new System.Drawing.Size(236, 28);
            this.bCreateDir.TabIndex = 0;
            this.bCreateDir.Text = "Создать директорию";
            this.bCreateDir.UseVisualStyleBackColor = true;
            this.bCreateDir.Click += new System.EventHandler(this.bCreateDir_Click);
            // 
            // bAddFriend
            // 
            this.bAddFriend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bAddFriend.Location = new System.Drawing.Point(3, 75);
            this.bAddFriend.Name = "bAddFriend";
            this.bAddFriend.Size = new System.Drawing.Size(236, 28);
            this.bAddFriend.TabIndex = 1;
            this.bAddFriend.Text = "Предоставить доступ";
            this.bAddFriend.UseVisualStyleBackColor = true;
            this.bAddFriend.Click += new System.EventHandler(this.bAddFriend_Click);
            // 
            // bDelFile
            // 
            this.bDelFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bDelFile.Location = new System.Drawing.Point(3, 48);
            this.bDelFile.Name = "bDelFile";
            this.bDelFile.Size = new System.Drawing.Size(236, 27);
            this.bDelFile.TabIndex = 2;
            this.bDelFile.Text = "Удалить файл";
            this.bDelFile.UseVisualStyleBackColor = true;
            this.bDelFile.Click += new System.EventHandler(this.bDelFile_Click);
            // 
            // bAddFile
            // 
            this.bAddFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bAddFile.Location = new System.Drawing.Point(3, 21);
            this.bAddFile.Name = "bAddFile";
            this.bAddFile.Size = new System.Drawing.Size(236, 27);
            this.bAddFile.TabIndex = 3;
            this.bAddFile.Text = "Добавить файл";
            this.bAddFile.UseVisualStyleBackColor = true;
            this.bAddFile.Click += new System.EventHandler(this.bAddFile_Click);
            // 
            // gbPublicKey
            // 
            this.gbPublicKey.Controls.Add(this.tbPublicKey);
            this.gbPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPublicKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbPublicKey.Location = new System.Drawing.Point(0, 0);
            this.gbPublicKey.Name = "gbPublicKey";
            this.gbPublicKey.Size = new System.Drawing.Size(242, 179);
            this.gbPublicKey.TabIndex = 2;
            this.gbPublicKey.TabStop = false;
            this.gbPublicKey.Text = "Публичный ключ:";
            // 
            // tbPublicKey
            // 
            this.tbPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPublicKey.Location = new System.Drawing.Point(3, 20);
            this.tbPublicKey.Multiline = true;
            this.tbPublicKey.Name = "tbPublicKey";
            this.tbPublicKey.ReadOnly = true;
            this.tbPublicKey.Size = new System.Drawing.Size(236, 156);
            this.tbPublicKey.TabIndex = 0;
            // 
            // fswTracker
            // 
            this.fswTracker.EnableRaisingEvents = true;
            this.fswTracker.SynchronizingObject = this;
            // 
            // ofdAdd
            // 
            this.ofdAdd.FileName = "openFileDialog1";
            // 
            // wndNetSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 531);
            this.Controls.Add(this.pFolderSpace);
            this.Controls.Add(this.pMenu);
            this.MinimumSize = new System.Drawing.Size(840, 570);
            this.Name = "wndNetSync";
            this.Text = "NetSync";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.wndNetSync_FormClosed);
            this.pMenu.ResumeLayout(false);
            this.pFolderSpace.ResumeLayout(false);
            this.gbFriends.ResumeLayout(false);
            this.gbMenu.ResumeLayout(false);
            this.gbPublicKey.ResumeLayout(false);
            this.gbPublicKey.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fswTracker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.GroupBox gbMenu;
        private System.Windows.Forms.GroupBox gbFriends;
        private System.Windows.Forms.ListBox lbFriends;
        private System.Windows.Forms.Panel pFolderSpace;
        private System.Windows.Forms.ListBox lbFolderSpace;
        private System.Windows.Forms.Button bDelFile;
        private System.Windows.Forms.Button bAddFriend;
        private System.Windows.Forms.Button bCreateDir;
        private System.Windows.Forms.Button bAddFile;
        private System.Windows.Forms.GroupBox gbPublicKey;
        private System.Windows.Forms.TextBox tbPublicKey;
        private System.IO.FileSystemWatcher fswTracker;
        private System.Windows.Forms.OpenFileDialog ofdAdd;
    }
}

