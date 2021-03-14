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
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolsLib;
using ToolsLib.Interfaces;
using ToolsLib.Model;

namespace NetSync
{
    public partial class wndNetSync : Form
    {
        private User _user;
        private readonly IPAddress _ip;
        private readonly string _ipStr;
        private readonly IPHostEntry _host;
        private const int _port = 11000;
        private const string _dumpExt = ".ubd";
        private readonly string _userBackupFilePath;
        private readonly CancellationTokenSource _cancellationToken;
        private UdpClient _reciv;
        private List<string> _curFriendsIps;

        public wndNetSync()
        {
            InitializeComponent();

            _curFriendsIps = new List<string>();

            _cancellationToken = new CancellationTokenSource();

            _host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in _host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ip = ip;
                    _ipStr = _ip.ToString();
                }
            }

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
            Run();
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
            try
            {
                Stop();
            }
            finally
            {
            }
            if (_user is null)
            {
                return;
            }
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
                if (_user.Friends.Contains(friendKey))
                {
                    MessageBox.Show("Пользователь уже в списке друзей");
                    return;
                }
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
            var addrTemplate = "192.168.0.";
            var frRq = new Request(UserRequestType.FRIENDRQ, friendKey);
            var rqJson = JsonConvert.SerializeObject(frRq);
            for (var i = 0; i < 193; i++)
            {
                var curIp = addrTemplate + i.ToString();
                if (curIp != _ipStr)
                {
                    Send(rqJson, IPAddress.Parse(curIp));
                }
            }
        }

        private void SendFriendCheck(string friendKey)
        { 
            
        }

        private void Send(string data, IPAddress ip)
        {
            var client = new UdpClient();
            var end = new IPEndPoint(ip, _port);
            if (!string.IsNullOrEmpty(data))
            {
                var dBytes = Encoding.UTF8.GetBytes(data);
                client.Send(dBytes, dBytes.Length, end);
            }
        }

        public void Run()
        {
            try
            {
                var receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            _reciv.Close();
            _cancellationToken.Cancel();
        }

        private Request GetRequestFromMessage(string msg)
        {
            try 
            {
                return JsonConvert.DeserializeObject<Request>(msg);
            }
            catch 
            {
                return new Request(UserRequestType.ERROR);
            }
        }

        private void UpdateFolder()
        { 
            
        }

        private void UpdateFriendsList()
        {
            lbFriends.Items.Clear();
            foreach(var frIp in _curFriendsIps)
            {
                lbFriends.Items.Add(frIp);
            }
        }

        private void ReceiveMessage()
        {
            try
            {
                _reciv = new UdpClient(_port); // UdpClient для получения данных
                IPEndPoint remoteIp = null; // адрес входящего подключения
                while (true)
                {
                    var data = _reciv.Receive(ref remoteIp); // получаем данные
                    var message = Encoding.UTF8.GetString(data);
                    var decodedRq = GetRequestFromMessage(message);
                    var answerRq = new Request();
                    var answerRqJson = "";

                    switch(decodedRq.Type)
                    {
                        case UserRequestType.HAVEDIR:
                            MessageBox.Show("У пользователя уже есть папка");
                            break;
                        case UserRequestType.DENIERQ:
                            MessageBox.Show("У пользователя уже есть папка");
                            break;
                        case UserRequestType.ACCEPTRQ:
                            MessageBox.Show("Пользователь принял запрос дружбы");
                            _user.Friends.Add(decodedRq.MainData);
                            _curFriendsIps.Add(remoteIp.ToString());
                            answerRq.Type = UserRequestType.FRIENDFINALACCEPT;
                            answerRq.MainData = _user.PublicKey;
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            UpdateFriendsList();
                            break;
                        case UserRequestType.FRIENDRQ:
                            if (decodedRq.MainData == _user.PublicKey)
                            {
                                if (_user.UserDirectory.Path != "")
                                {
                                    answerRq.Type = UserRequestType.HAVEDIR;
                                    answerRqJson = JsonConvert.SerializeObject(answerRq);
                                    Send(answerRqJson, remoteIp.Address);
                                }
                                else
                                {
                                    var quest = MessageBox.Show($"Пользователь {remoteIp} хочет добавить вас в друзья, принять?",
                                                                "Запрос.",
                                                                MessageBoxButtons.YesNo);
                                    if (quest == DialogResult.Yes)
                                    {
                                       /* var addFolderDialog = new wndCreateFolder("Выберите место для копии папки у себя");
                                        if (addFolderDialog.ShowDialog() == DialogResult.OK)
                                        {
                                            _user.UserDirectory.Path = addFolderDialog.SelectedPath;
                                            SetWatcher();
                                            FillFolderSpace();
                                            SetButtons();
                                        }*/
                                        answerRq.Type = UserRequestType.ACCEPTRQ;
                                        answerRq.MainData = _user.PublicKey;
                                        answerRqJson = JsonConvert.SerializeObject(answerRq);
                                        Send(answerRqJson, remoteIp.Address);
                                    }
                                    else
                                    {
                                        answerRq.Type = UserRequestType.DENIERQ;
                                        answerRqJson = JsonConvert.SerializeObject(answerRq);
                                        Send(answerRqJson, remoteIp.Address);
                                    }
                                }
                            }
                            break;
                        case UserRequestType.FRIENDCHECK:
                            break;
                        case UserRequestType.FRIENDFINALACCEPT:
                            _user.Friends.Add(decodedRq.MainData);
                            _curFriendsIps.Add(remoteIp.ToString());
                            UpdateFolder();
                            UpdateFriendsList();
                            break;
                        case UserRequestType.ERROR:
                            MessageBox.Show("Произошла ошибка при обработке сообщения");
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void lbFolderSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtons();
        }
    }
}
