using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SS
{
    public class Controller
    {
        private Interface window;
        private AbstractSpreadsheet model;

        public Controller(Interface _window)
        {
            this.window = window;
            this.model = new Spreadsheet();

        }
    }
}
