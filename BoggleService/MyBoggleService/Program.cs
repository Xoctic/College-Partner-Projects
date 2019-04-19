using BoggleService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoggleService
{
    class Program
    {
        static void Main()
        {
            BoggleController controller = new BoggleController();
            String userToken = controller.PostRegister("Joe");
            Console.WriteLine(userToken);

            // This is our way of preventing the main thread from            
            // exiting while the server is in use            
            Console.ReadLine();
        }

        private StringSocketListener server = new StringSocketListener();
    }
}
