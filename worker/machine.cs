using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace worker
{
    class machine
    {
        private string path, id;

        public machine(string path)
        {
            int idLength = 4;
            this.path = path;
            this.id = path.Substring(path.Length - idLength);
        }
    }
}
