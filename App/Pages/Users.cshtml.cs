using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Pages
{
    public class UsersModel : PageModel
    {
        public string Param1Str;
        public string Param2Str;
        public string Param3Str;

        //public void OnGet()
        //{
        //    Param1Str = "ee";
        //    Param2Str = "rr";
        //    Param3Str = "xxx";
        //}

        public void OnGet(string param1 = "11", string param2 = "22", string param3 = "33")
        {
            Param1Str = param1;
            Param2Str = param2;
            Param3Str = param3;
        }
    }
}