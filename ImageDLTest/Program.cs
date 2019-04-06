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
            ImgBrdProcessor processor = new ImgBrdProcessor();
            
            Directory.Exists(@"D:\Pictures\New folder\");


            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World! Lets get yiffy!");
            try
            {

                
                var result = processor.E621Search(searchtags);
                //Test();              
                
                Console.WriteLine(result.Result.Count);
                
                Parallel.ForEach(processor.files, new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                    file => 
                    {
                        WebClient wc = new WebClient();
                       
                        wc.DownloadFile(file.File_url, @"D:\Pictures\New folder\"+file.filename);
                        


                        Console.WriteLine("Download complete");
                        wc.Dispose();
                    });
                    
            }
            catch (Exception e)
            {

                Console.WriteLine($" I dont know chief! ");
                Console.WriteLine(e.Message);
            }
          
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
      
    }
       
   }

