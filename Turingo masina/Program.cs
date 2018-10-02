using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Turingo_masina
{
    class Code
    {
        public string currentState { get; set; }
        public char currentSymbol { get; set; }
        public char newSymbol { get; set; }
        public char direction { get; set; }
        public string newState { get; set; }
    }

    class Program
    {
        static List<int> heads = new List<int>();
        static List<char[]> inputs = new List<char[]>();
        static List<List<Code>> code = new List<List<Code>>();
        static List<string> currentMachineState = new List<string>();

        static void Main(string[] args)
        {
            ReadFile();
            RunMachines();
        }

        static void ReadFile()
        {
            Console.Write("Juostu skaicius: ");
            int count = int.Parse(Console.ReadLine());

            for (int i = 0; i < count; i++)
            {
                code.Add(new List<Code>());
                currentMachineState.Add("0");
                Console.Write((i + 1) + " failo pavadinimas: ");
                string path = Console.ReadLine();

                if (!File.Exists(path))
                    Exit("Toks failas neegzistuoja");

                using (StreamReader reader = new StreamReader(path))
                {
                    heads.Add(int.Parse(reader.ReadLine()));
                    heads[i]--;

                    inputs.Add(reader.ReadLine().ToCharArray());

                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string[] line = reader.ReadLine().Split(' ');
                            
                            code[i].Add(new Code()
                            {
                                currentState = line[0],
                                currentSymbol = Convert.ToChar(line[1]),
                                newSymbol = Convert.ToChar(line[2]),
                                direction = Convert.ToChar(line[3]),
                                newState = line[4]
                            }); 
                        }
                        catch
                        {
                            Exit("Blogas kodo formatas");
                        }
                    }
                }
                
            }
        }

        static void Exit(string error)
        {
            Console.Clear();
            Console.WriteLine(error);
            Console.ReadLine();
            Environment.Exit(0);
        }


        static bool programRunning()
        {
            for (int i = 0; i < code.Count; i++)
            {
                if (currentMachineState[i].ToLower() != "halt")
                    return true;
            }
            return false;
        }

        static void RunMachines()
        {

            while (true)
            {
                
                for (int j = 0; j < code.Count; j++)
                {
                    if (currentMachineState[j].ToLower() != "halt")
                    {
                        bool stateFound = false;
                        for (int i = 0; i < code[j].Count; i++)
                        {
                            if ((code[j][i].currentState == "*" || code[j][i].currentState == currentMachineState[j]) && (code[j][i].currentSymbol == inputs[j][heads[j]] || code[j][i].currentSymbol == '*'))
                            {
                                stateFound = true;

                                if (code[j][i].newSymbol != '*')
                                    inputs[j][heads[j]] = code[j][i].newSymbol;

                                if (code[j][i].direction.ToString().ToLower() == "r")
                                    heads[j]++;

                                if (code[j][i].direction.ToString().ToLower() == "l")
                                    heads[j]--;

                                if (code[j][i].newState != "*")
                                    currentMachineState[j] = code[j][i].newState;

                                Console.WriteLine(new string(inputs[j]));
                                break;
                            }
                        }

                        //if not found
                        if (!stateFound)
                            Exit(String.Format(new string(inputs[j]) + "\n\nNera instrukciju simboliui {0} busenoje {1}. {2} Masina", inputs[j][heads[j]], currentMachineState[j], j+1));
                    }
                }
            }
        }
    }
}