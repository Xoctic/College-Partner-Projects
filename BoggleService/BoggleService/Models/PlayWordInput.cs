using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    public class PlayWordInput
    {
        public PlayWordInput(string u, string w)
        {
            userToken = u;
            word = w;
        }

        public string userToken { get; set; }
        public string word { get; set; }
    }
}