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
        static char[] input;
        static int head;
        static List<Code> code = new List<Code>();

        //tiuringo masina prasideda su 0 state'u
        static string currentMachineState = "0";

        static void Main(string[] args)
        {
            Console.Write("Duomenu failas: ");
            ReadFile(Console.ReadLine());
            Console.WriteLine("1. Paleisti masina pilnu pajegumu.\n2. Paleisti masina step by step rezimu");
            int masina = int.Parse(Console.ReadLine());
            Console.Clear();
            RunMachine(masina);
        }

        static void ReadFile(string path)
        {
            if (!File.Exists(path))
                Exit("Toks failas neegzistuoja");

            using (StreamReader reader = new StreamReader(path))
            {
                head = int.Parse(reader.ReadLine());
                input = reader.ReadLine().ToCharArray();

                while (!reader.EndOfStream)
                {
                    try
                    {
                        string[] line = reader.ReadLine().Split(' ');
                        if (line.Length == 5)
                        {
                            code.Add(new Code()
                            {
                                currentState = line[0],
                                currentSymbol = Convert.ToChar(line[1]),
                                newSymbol = Convert.ToChar(line[2]),
                                direction = Convert.ToChar(line[3]),
                                newState = line[4]
                            });
                        }
                    }
                    catch
                    {
                        Exit("Blogas kodo formatas");
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

        static void RunMachine(int n)
        {
            //head's position
            int index = head - 1;
            //run the machine until it reaches halt state
            while (currentMachineState.ToLower() != "halt")
            {
                bool stateFound = false;
                //Search for the current state's instructions for the symbol
                for (int i = 0; i < code.Count; i++)
                {
                    if ((code[i].currentState == "*" || code[i].currentState == currentMachineState) && (code[i].currentSymbol == input[index] || code[i].currentSymbol == '*'))
                    {
                        //Console.WriteLine("Dabartine busena: " + currentMachineState);
                        Console.WriteLine(new string(input));
                        if (n != 1)
                        {
                            for (int j = 1; j < index; j++)
                                Console.Write(" ");
                            Console.Write("^");
                            Thread.Sleep(700);
                        }
                        stateFound = true;

                        if (code[i].newSymbol != '*')
                            input[index] = code[i].newSymbol;

                        if (code[i].direction.ToString().ToLower() == "r")
                            index++;

                        else if (code[i].direction.ToString().ToLower() == "l")
                            index--;

                        if (index < 0 || index >= input.Length)
                            Exit(new string(input) + "\n\nMasina isejo is juostos ribu");

                        if (code[i].newState != "*")
                            currentMachineState = code[i].newState;

                        break;
                    }
                }

                //if not found
                if (!stateFound)
                    Exit(String.Format(new string(input) + "\n\nNera instrukciju simboliui {0} busenoje {1}", input[index], currentMachineState));
            }

            Console.WriteLine(new string(input));
            Console.ReadLine();
        }
    }
}