using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public interface ILog
    {
       
        void WriteTeam(Team team, string filePath);

        
        void WriteHistory(Team team, string filePath);
    }
}
