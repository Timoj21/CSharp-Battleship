using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileIO
{
    class Program
    {
        static void Main(string[] args)
        {

            WriteToFile("Timo");
            ReadFromFile();


        }

        public static void WriteToFile(string playername)
        {

            string filepath = "ScoreBoard.txt";
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

        public static void ReadFromFile()
        {
            StreamReader reader = new StreamReader("ScoreBoard.txt");
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        public static void ClearFile()
        {
            string filepath = "ScoreBoard.txt";
            List<string> clearScoreBoard = new List<string>();

            File.WriteAllLines(filepath, clearScoreBoard);

        }
    }
}

