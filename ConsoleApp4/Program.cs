using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser("{\"ad\":\"Ali\",\"durum\":true,\"yas\":32}");
            var obj = parser.ParseJObject();
        }
    }
}
