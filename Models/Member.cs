using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Projekt
{
    [NotMapped]
    [Serializable]
    public class Member
    {
        private string name = string.Empty;
        private string role = string.Empty;
        private string info = string.Empty;
        private string replik1 = string.Empty;
        private string replik2 = string.Empty;
        private string replik3 = string.Empty;

        public Member() { }
        public Member(string nm, string ro, string inn)
        {
            name = nm;
            role = ro;
            info = inn;
        }

        public string Name { get { return name; } }
        public string Role { get { return role; } }
        public string Info { get { return info; } }

        public string Replik1 { set { replik1 = value; } get { return replik1; } }
        public string Replik2 { set { replik2 = value; } get { return replik2; } }
        public string Replik3 { set { replik3 = value; } get { return replik3; } }

    }
}
