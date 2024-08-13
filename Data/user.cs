using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMonitor.Data
{
    public class user
    {
        private string userId;
        private string password;
        private string role;
        private string name;
        private string taiwanName;
        private string official;

        public user(string userId, string password, string role, string name, string taiwanName, string official)
        {
            this.userId = userId;
            this.password = password;
            this.role = role;
            this.name = name;
            this.taiwanName = taiwanName;
            this.official = official;
        }
        
        public string getUserId()
        {
            return this.userId;
        }

        public string getRole()
        {
            return this.role;
        }

        public string getName()
        {
            return this.name;
        }
        public string getTaiwanName()
        {
            return this.taiwanName;
        }
        public string getOfficial()
        {
            return this.official;
        }
    }
}
