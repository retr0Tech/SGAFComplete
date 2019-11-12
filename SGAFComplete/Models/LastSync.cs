using System;
using SQLite;

namespace SGAFComplete.Models
{
    public class LastSync
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string lastSync { get;  set; }
    }
}
