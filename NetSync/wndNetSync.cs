﻿using AddFriend;
using CreateFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolsLib;
using ToolsLib.Model;

namespace NetSync
{
    public partial class wndNetSync : Form
    {
        private User _user;
        private readonly IPAddress _ip;
        private readonly string _host;
        private const int _port = 11000;
        private const string _dumpExt = ".ubd";
        private readonly string _userBackupFilePath;
        private readonly CancellationTokenSource _cancellationToken;

        public wndNetSync()
        {
            InitializeComponent();

            _cancellationToken = new CancellationTokenSource();

            _host = Dns.GetHostName();
            _ip = Dns.GetHostEntry(_host).AddressList[0];

            _userBackupFilePath = GetUserBackupFile();
            if (_userBackupFilePath == "")
            {
                _userBackupFilePath = "User" + _dumpExt;
                GenerateUser();
            }
            else
            {
                RestoreUser(_userBackupFilePath);
            }
            tbPublicKey.Text = _user.PublicKey;

            SetButtons();
            SetWatcher();
            FillFolderSpace();
        }

        private void RestoreUser(string file)
        {
            try
            {
                var restJson = Dumper.Restore(file);
                _user = JsonConvert.DeserializeObject<User>(restJson);
            }
            catch
            {
                GenerateUser();
            }
        }

        private void GenerateUser()
        {
            _user = new User();
        }

        private string GetUserBackupFile()
        {
            var allDirFiles = Directory.GetFiles(Environment.CurrentDirectory);
            foreach (var file in allDirFiles)
            {
                if (file.Contains(_dumpExt))
                {
                    return file;
                }
            }
            return "";
        }

        private void SetButtons()
        {
            bAddFile.Enabled = _user.UserDirectory.Path != "";
            bDelFile.Enabled = _user.UserDirectory.Path != "" && lbFolderSpace.SelectedIndex >= 0;
            bCreateDir.Enabled = _user.UserDirectory.Path == "";
            bAddFriend.Enabled = _user.UserDirectory.Path != "";
        }

        private void FillFolderSpace()
        {
            if (_user.UserDirectory.Path == "")
            {
                return;
            }
            lbFolderSpace.Items.Clear();
            var allDirFiles = Directory.GetFiles(_user.UserDirectory.Path);
            foreach (var file in allDirFiles)
            {
                lbFolderSpace.Items.Add(file);
            }
        }

        private void FolderChanged(object sender, FileSystemEventArgs e)
        {
            FillFolderSpace();
        }

        private void SetWatcher()
        {
            if(_user.UserDirectory.Path == "")
            {
                return;
            }
            fswTracker.Path = _user.UserDirectory.Path;
            fswTracker.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            fswTracker.Changed += FolderChanged;
            fswTracker.Created += FolderChanged;
            fswTracker.Deleted += FolderChanged;
            fswTracker.Renamed += FolderChanged;

            fswTracker.IncludeSubdirectories = true;
            fswTracker.EnableRaisingEvents = true;
        }

        private void wndNetSync_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dumper.Dump(JsonConvert.SerializeObject(_user), _userBackupFilePath);
        }

        private void bAddFile_Click(object sender, EventArgs e)
        {
            if (ofdAdd.ShowDialog() == DialogResult.OK)
            {
                var lastPt = ofdAdd.FileName.Split('\\').Last();
                File.Copy(ofdAdd.FileName, Path.Combine(_user.UserDirectory.Path, lastPt), true);
            }
            SetButtons();
        }

        private void bDelFile_Click(object sender, EventArgs e)
        {
            var file = lbFolderSpace.SelectedItem;
            if (file is null)
            {
                return;
            }
            File.Delete((string)file);
            SetButtons();
        }

        private void bAddFriend_Click(object sender, EventArgs e)
        {
            var addUserToDir = new wndAddFriend();
            addUserToDir.ShowDialog();
            if (addUserToDir.DialogResult == DialogResult.OK)
            {
                var friendKey = addUserToDir.FriendPublicKey;
                SendFriendRq(friendKey);
            }
        }

        private void bCreateDir_Click(object sender, EventArgs e)
        {
            var addFolderDialog = new wndCreateFolder();
            if (addFolderDialog.ShowDialog() == DialogResult.OK)
            {
                _user.UserDirectory.Path = addFolderDialog.SelectedPath;
                SetWatcher();
                FillFolderSpace();
                SetButtons();
            }
        }

        private void SendFriendRq(string friendKey)
        {

        }

        private void lbFolderSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtons();
        }
    }
}
