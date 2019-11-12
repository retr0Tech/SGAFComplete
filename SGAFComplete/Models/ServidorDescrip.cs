using System;
using SQLite;

namespace SGAFComplete.Models
{
    public class ServidorDescrip
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int Timeout { get; set; }
        public string ServerIp { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
