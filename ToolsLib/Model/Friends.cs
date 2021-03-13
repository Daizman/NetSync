using Newtonsoft.Json;
using System.Collections.Generic;

namespace ToolsLib.Model
{
    public class Friends
    {
        [JsonProperty("users")]
        private List<User> _users;

        public Friends() 
        {
            _users = new List<User>();
        }

        public Friends(List<User> users)
        {
            _users = users;
        }

        public void AddFriend(User user)
        {
            _users.Add(user);
        }

        public bool CheckUser(User user)
        {
            foreach(var u in _users)
            {
                if (u.PublicKey == user.PublicKey)
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteFriend(User user)
        {
            _users.Remove(user);
        }

        public User this[int index]
        { 
            get
            {
                return _users[index];
            }
            set
            {
                _users[index] = value;
            }
        }
    }
}
