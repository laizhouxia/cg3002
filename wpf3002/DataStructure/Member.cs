using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace wpf3002.DataStructure
{
    [DataContract]
    public class Member
    {
        [DataMember(Name = "username")]
        public String username { get; set; }

        [DataMember(Name = "phone")]
        public String phone { get; set; }

        [DataMember(Name = "birthday")]
        public String birthday { get; set; }

        [DataMember(Name = "credits")]
        public double credits { get; set; }

        public Member(String _username, String _phone, String _birthday, double _credits)
        {
            username = _username;
            phone = _phone;
            birthday = _birthday;
            credits = _credits;
        }
    }
}
