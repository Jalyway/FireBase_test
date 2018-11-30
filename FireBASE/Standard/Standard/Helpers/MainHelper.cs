using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Helpers
{
    public class MainHelper
    {
        public static Firebase.Database.FirebaseClient FirebaseClient
        { get; set; } =
            new Firebase.Database.FirebaseClient("https://xxxx-9e6b3.firebaseio.com");
    }
}
