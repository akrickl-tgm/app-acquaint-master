using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquaint.Data
{
    public static class DBConnectionBase
    {
        public static string servername { get; set; } = "wi-gate.technikum-wien.at";
        public static uint port { get; set; } = 60637;
        public static string dbname { get; set; } = "lexiecom";
        public static string username { get; set; } = "remote";
        public static string pw { get; set; } = "MDHfst4-"; 
    }
}
