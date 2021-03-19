using AddFriend;
using CreateFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolsLib;
using System.Windows.Threading;
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
        private bool _is_receiving = false;
        private Dictionary<string, string> _curFriendsIps;

        private Dispatcher _thisDisp;

        public wndNetSync()
        {
            InitializeComponent();

            _thisDisp = Dispatcher.CurrentDispatcher;

            _curFriendsIps = new Dictionary<string, string>();

            _cancellationToken = new CancellationTokenSource();

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", _port);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                _ip = endPoint.Address;
                _ipStr = endPoint.Address.ToString();
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

        private void StartReceiving()
        {
            _is_receiving = true;
        }

        private void StopReceiving()
        {
            _is_receiving = false;
        }

        private void RestoreFolderFromFriend()
        {
            if (_curFriendsIps.Count > 0) 
            {
                Console.WriteLine("IN_RESTORE");
                var rq = new Request();
                rq.Type = UserRequestType.IWANTUPDATEFOLDER;
                var rqJson = JsonConvert.SerializeObject(rq);
                Send(rqJson, IPAddress.Parse(_curFriendsIps.Values.ToArray()[0].Split(':')[0]));
            }
        }

        private void RestoreUser(string file)
        {
            try
            {
                var restJson = Dumper.Restore(file);
                _user = JsonConvert.DeserializeObject<User>(restJson);
                foreach (var fr in _user.Friends)
                {
                    SendFriendCheck(fr);
                }
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
            var allDirFiles = Directory.GetFiles(_user.UserDirectory.Path, "*", SearchOption.AllDirectories);
            foreach (var file in allDirFiles)
            {
                lbFolderSpace.Items.Add(file);
            }
        }

        private void FolderChanged(object sender, FileSystemEventArgs e)
        {
            if (_is_receiving) return;
            Console.WriteLine("IN_FOLDER_CHANGE");
            FillFolderSpace();
            NotifyFriends(e.ChangeType.ToString());
        }

        private void NotifyFriends(string chType)
        {
            var rq = new Request(UserRequestType.IUPDATEDFOLDER);
            var uFiles = new DirectoryFiles(_user.UserDirectory.Path);
            rq.MainData = JsonConvert.SerializeObject(uFiles);
            var jsonRq = JsonConvert.SerializeObject(rq);
            foreach (var fr in _curFriendsIps)
            {
                Console.WriteLine("I NOTIFY FRIEND: " + fr.Value.Split(':')[0] + " because: " + chType);
                Send(jsonRq, IPAddress.Parse(fr.Value.Split(':')[0]));
            }
        }

        private void SetWatcher()
        {
            if(_user.UserDirectory.Path == "")
            {
                return;
            }
            fswTracker.Path = _user.UserDirectory.Path;
            fswTracker.IncludeSubdirectories = true;
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
                if (ofdAdd.FileName.Contains(_user.UserDirectory.Path)) return;
                var lastPt = ofdAdd.FileName.Split('\\').Last();
                var dirPath = _user.UserDirectory.Path.Split('\\');
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
                if (_user.PublicKey == friendKey)
                {
                    MessageBox.Show("Нельзя добавить себя в друзья");
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
            var addrTemplate = "192.168.0.";
            var frRq = new Request(UserRequestType.FRIENDCHECK, friendKey);
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

        private void PingFriends()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("PINGFRIENDS");
                _thisDisp.Invoke(lbFriends.Items.Clear);
                foreach (var fr in _user.Friends)
                {
                    SendFriendCheck(fr);
                }
            }
        }

        public void Run()
        {
            try
            {
                var receiveTask = new Task(ReceiveMessage, _cancellationToken.Token);
                var pingTask = new Task(PingFriends, _cancellationToken.Token);
                receiveTask.Start();
                pingTask.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
            _reciv.Close();
            _reciv.Dispose();
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

        private Tuple<string[], string[]> GetBasesForUpdateFolder(string[] myFiles, DirectoryFiles reciveFiles)
        {
            var myFilesLen = myFiles.Length;
            var myBasePathLen = _user.UserDirectory.Path.Length;
            for (var i = 0; i < myFilesLen; i++)
            {
                myFiles[i] = myFiles[i].Substring(myBasePathLen + 1);
            }

            var reciveFilesData = reciveFiles.DirFiles.Keys.ToArray();
            var reciveFilesDataLen = reciveFilesData.Length;
            var reciveFilesBaseLen = reciveFiles.BasePath.Length;
            for (var i = 0; i < reciveFilesDataLen; i++)
            {
                reciveFilesData[i] = reciveFilesData[i].Substring(reciveFilesBaseLen + 1);
            }

            return new Tuple<string[], string[]>(myFiles, reciveFilesData);
        }

        private void UpdateFolder(DirectoryFiles files, bool fullChanges=false)
        {
            var myFiles = Directory.GetFiles(_user.UserDirectory.Path, "*", SearchOption.AllDirectories);
            var basesForUpd = GetBasesForUpdateFolder(myFiles, files);
            var filesCount = myFiles.Length;
            fswTracker.Path = "/";

            Console.WriteLine("IN_UPDATE_FOLDER");
            if (fullChanges) 
            {
                Console.WriteLine("IN_FULL_CHANGES");
                if (filesCount != files.DirFiles.Count)
                {
                    var iDeleted = false;
                    foreach (var file in basesForUpd.Item1)
                    {
                        if (!basesForUpd.Item2.Contains(file))
                        {
                            iDeleted = true;
                            File.Delete(Path.Combine(_user.UserDirectory.Path, file));
                        }
                    }
                    if (iDeleted)
                    {
                        Console.WriteLine("SET_TRACKER_BACK_DEL");
                        fswTracker.Path = _user.UserDirectory.Path;
                        _thisDisp.Invoke(FillFolderSpace);
                        return;
                    }
                }

                var iRenamed = false;
                for (var i = 0; i < filesCount; i++)
                {
                    if (!basesForUpd.Item2.Contains(basesForUpd.Item1[i]) && basesForUpd.Item1[i] != basesForUpd.Item2[i])
                    {
                        File.Delete(Path.Combine(_user.UserDirectory.Path, basesForUpd.Item1[i]));

                        var f = File.Create(Path.Combine(_user.UserDirectory.Path, basesForUpd.Item2[i]));
                        var fileData = files.DirFiles[Path.Combine(files.BasePath, basesForUpd.Item2[i])];
                        f.Write(fileData, 0, fileData.Length);
                        f.Close();
                        iRenamed = true;
                    }
                }
                if (iRenamed)
                {
                    Console.WriteLine("SET_TRACKER_BACK_RENAME");
                    fswTracker.Path = _user.UserDirectory.Path;
                    _thisDisp.Invoke(FillFolderSpace);
                    return;
                }
            }
            foreach (var file in basesForUpd.Item2)
            {
                Console.WriteLine("BASE_RESTORE");
                var newFPath = Path.Combine(_user.UserDirectory.Path, file);

                Directory.CreateDirectory(Path.GetDirectoryName(newFPath));

                var fs = new StreamWriter(newFPath);
                fs.Write("");
                fs.Close();
                
                var f = File.Exists(newFPath) ? File.Open(newFPath, FileMode.Open) : File.Create(newFPath);
                
                var fileData = files.DirFiles[Path.Combine(files.BasePath, file)];
                f.Write(fileData, 0, fileData.Length);
                f.Close();
            }
            Console.WriteLine("SET_TRACKER_BACK_JUST");
            fswTracker.Path = _user.UserDirectory.Path;
            _thisDisp.Invoke(FillFolderSpace);
        }

        private void UpdateFriendsList()
        {
            lbFriends.Items.Clear();
            foreach (var frIp in _curFriendsIps.Values)
            {
                lbFriends.Items.Add(frIp.Split(':')[0]);
            }
        }

        private void CreateCopyFolder()
        {
            var addFolderDialog = new wndCreateFolder("Выберите место для копии папки у себя");
            if (addFolderDialog.ShowDialog() == DialogResult.OK)
            {
                _user.UserDirectory.Path = addFolderDialog.SelectedPath;
                SetWatcher();
                FillFolderSpace();
                SetButtons();
            }
        }

        private void ReceiveMessage()
        {
            var cancelWaitTask = Task.Run(() =>
            {
                using (var resetEvent = new ManualResetEvent(false))
                {
                    _cancellationToken.Token.Register(() => resetEvent.Set());
                    resetEvent.WaitOne();
                }
            });
            try
            {
                _reciv = new UdpClient(_port); // UdpClient для получения данных
                IPEndPoint remoteIp = null; // адрес входящего подключения
                while (!_cancellationToken.Token.IsCancellationRequested)
                {
                    if (cancelWaitTask.IsCompleted)
                    {
                        break;
                    }
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
                            MessageBox.Show("Пользователь отклонил запрос дружбы");
                            break;
                        case UserRequestType.ACCEPTRQ:
                            MessageBox.Show("Пользователь принял запрос дружбы");
                            _user.Friends.Add(decodedRq.MainData);
                            if (_curFriendsIps.ContainsKey(decodedRq.MainData))
                            {
                                _curFriendsIps[decodedRq.MainData] = remoteIp.ToString();
                            }
                            else
                            {
                                _curFriendsIps.Add(decodedRq.MainData, remoteIp.ToString());
                            }
                            answerRq.Type = UserRequestType.FRIENDFINALACCEPT;
                            answerRq.MainData = _user.PublicKey;
                            answerRq.AdditionalInfo = JsonConvert.SerializeObject(_curFriendsIps);
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            _thisDisp.Invoke(UpdateFriendsList);
                            answerRq.Type = UserRequestType.IWANTUPDATEFOLDER;
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            answerRq.Type = UserRequestType.IHAVENEWFRIEND;
                            answerRq.MainData = decodedRq.MainData;
                            answerRq.AdditionalInfo = remoteIp.ToString();
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            foreach (var friend in _curFriendsIps)
                            {
                                if (friend.Key != decodedRq.MainData && friend.Key != decodedRq.MainData)
                                {
                                    Send(answerRqJson, IPAddress.Parse(friend.Value.Split(':')[0]));
                                }
                            }
                            break;
                        case UserRequestType.IHAVENEWFRIEND:
                            if (_curFriendsIps.ContainsKey(decodedRq.MainData))
                            {
                                _curFriendsIps[decodedRq.MainData] = decodedRq.AdditionalInfo;
                            }
                            else
                            {
                                _curFriendsIps.Add(decodedRq.MainData, decodedRq.AdditionalInfo);
                            }
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
                                        _thisDisp.Invoke(CreateCopyFolder);
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
                            if (decodedRq.MainData == _user.PublicKey) 
                            {
                                answerRq.Type = UserRequestType.FRIENDCHECKANSWER;
                                answerRq.MainData = _user.PublicKey;
                                answerRqJson = JsonConvert.SerializeObject(answerRq);
                                Send(answerRqJson, remoteIp.Address);
                            }
                            break;
                        case UserRequestType.FRIENDCHECKANSWER:
                            bool iWasEmpty = _curFriendsIps.Count == 0;
                            if (_curFriendsIps.ContainsKey(decodedRq.MainData))
                            {
                                _curFriendsIps[decodedRq.MainData] = remoteIp.ToString();
                            }
                            else
                            {
                                _curFriendsIps.Add(decodedRq.MainData, remoteIp.ToString());
                            }
                            answerRq.Type = UserRequestType.FRIENDCHECKANSWERFINAL;
                            answerRq.MainData = _user.PublicKey;
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            _thisDisp.Invoke(UpdateFriendsList);
                            if (iWasEmpty)
                            {
                                _thisDisp.Invoke(StartReceiving);
                                RestoreFolderFromFriend();
                                _thisDisp.Invoke(StopReceiving);
                            }
                            break;
                        case UserRequestType.FRIENDCHECKANSWERFINAL:
                            iWasEmpty = _curFriendsIps.Count == 0;
                            if (_curFriendsIps.ContainsKey(decodedRq.MainData))
                            {
                                _curFriendsIps[decodedRq.MainData] = remoteIp.ToString();
                            }
                            else
                            {
                                _curFriendsIps.Add(decodedRq.MainData, remoteIp.ToString());
                            }
                            if (iWasEmpty)
                            {
                                _thisDisp.Invoke(StartReceiving);
                                RestoreFolderFromFriend();
                                _thisDisp.Invoke(StopReceiving);
                            }
                            _thisDisp.Invoke(UpdateFriendsList);
                            break;
                        case UserRequestType.IWANTUPDATEFOLDER:
                            var uFiles = new DirectoryFiles(_user.UserDirectory.Path);
                            answerRq.Type = UserRequestType.IWANTSENDFOLDER;
                            answerRq.MainData = JsonConvert.SerializeObject(uFiles);
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            break;
                        case UserRequestType.IWANTSENDFOLDER:
                            _thisDisp.Invoke(StartReceiving);
                            UpdateFolder(JsonConvert.DeserializeObject<DirectoryFiles>(decodedRq.MainData));
                            _thisDisp.Invoke(StopReceiving);
                            break;
                        case UserRequestType.FRIENDFINALACCEPT:
                            _user.Friends.Add(decodedRq.MainData);
                            if (_curFriendsIps.ContainsKey(decodedRq.MainData))
                            {
                                _curFriendsIps[decodedRq.MainData] = remoteIp.ToString();
                            }
                            else
                            {
                                _curFriendsIps.Add(decodedRq.MainData, remoteIp.ToString());
                            }
                            var newFriends = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedRq.AdditionalInfo);
                            foreach (var frien in newFriends)
                            {
                                if (!_curFriendsIps.ContainsKey(frien.Key) && frien.Key != _user.PublicKey)
                                {
                                    _curFriendsIps.Add(frien.Key, frien.Value);
                                }
                            }
                            _thisDisp.Invoke(UpdateFriendsList);
                            answerRq.Type = UserRequestType.IWANTUPDATEFOLDER;
                            answerRqJson = JsonConvert.SerializeObject(answerRq);
                            Send(answerRqJson, remoteIp.Address);
                            break;
                        case UserRequestType.IUPDATEDFOLDER:
                            _thisDisp.Invoke(StartReceiving);
                            UpdateFolder(JsonConvert.DeserializeObject<DirectoryFiles>(decodedRq.MainData), true);
                            _thisDisp.Invoke(StopReceiving);
                            break;
                        case UserRequestType.ERROR:
                            MessageBox.Show("Произошла ошибка при обработке сообщения");
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void lbFolderSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtons();
        }
    }
}
