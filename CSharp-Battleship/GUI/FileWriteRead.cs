using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace FileIO
{
    class FileWriteRead

        

       
    {
        public List<string> outcomes { get; set; }


        public void WriteToFile(string playername)
        {

            string filepath = @"C:\Users\jornn\Desktop\csharp repo 2\CSharp-Battleship\CSharp-Battleship\ScoreBoard.txt";
            List<string> lines = new List<string>();
            try
            {
                lines = File.ReadAllLines(filepath).ToList();
            }
            catch
            {
                Console.WriteLine("Wrong filepath");
            }
            int counter = lines.Count + 1;

            lines.Add(playername + " won game " + counter + "!");

            File.WriteAllLines(filepath, lines);

        }

        public void ReadFromFile()
        {
            string filepath = @"C:\Users\jornn\Desktop\csharp repo 2\CSharp-Battleship\CSharp-Battleship\ScoreBoard.txt";
            outcomes = new List<string>();
            outcomes = File.ReadAllLines(filepath).ToList();


        }

        public static void ClearFile()
        {
            string filepath = @"C:\Users\jornn\Desktop\csharp repo 2\CSharp-Battleship\CSharp-Battleship\ScoreBoard.txt";
            List<string> clearScoreBoard = new List<string>();

            File.WriteAllLines(filepath, clearScoreBoard);

        }


    }
}

