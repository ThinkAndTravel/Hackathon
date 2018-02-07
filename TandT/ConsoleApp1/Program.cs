using System;
using BLL;
using MongoManager;
using MongoManager.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DataManager A = new DataManager();
            List<string> list = new List<string>();
            
            list.Add("t1:hERWNyirmmxVFZh");
            DataManager.Users.AddActiveTasks("u1:q",list.ToArray());

        }
    }
}