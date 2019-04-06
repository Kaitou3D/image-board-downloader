using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using ImageBoardProcessor.Clients;
using ImageBoardProcessor.Models;
using ImageBoardProcessor.Processors;
using System.Net;
using System.IO;
using System.Net.Http;

namespace ImageDLTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<string> searchtags = new List<string>() { "nawka", "cum" };
           
            
            Directory.Exists(@"D:\Pictures\New folder\");

            Process();

            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World! Lets get yiffy!");
            
          
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        static async void Process()
        {
            try
            {
                ImgBrdProcessor processor = new ImgBrdProcessor();
                Query q = new Query(ImageBoardProcessor.Enumerations.QueryType.Rule34);
                q.tag0 = "renamon";
                q.tag1 = "male//female";
                Console.WriteLine("We're searching");
                var result = await processor.Rule34Search(q);
                Console.WriteLine("We got them!");
                foreach (var item in result)
                {
                    foreach ( var post in item.post)
                    {
                        Console.WriteLine(post.file_url);
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine($" I dont know chief! ");
                Console.WriteLine(e.Message);
            }
        }
      
    }
       
   }

