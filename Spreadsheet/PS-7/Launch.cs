using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS_7
{
    /// <summary>
    /// Main entry point for the application
    /// </summary>
    static class Launch
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Get the application context and run one form inside it
            var context = SpreadsheetApplicationContext.GetContext();
            SpreadsheetApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }

    }

    
}
