using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectDataStructure;
using ProjectDataStructure.Model;
using System.IO;

namespace ConsoleApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            DocManagement dm = new DocManagement(print, delateAfterSeconds: 60);
            string limit = "_________________________________________________________________________________";
            string input;

            do
            {
                //Menu
                Console.WriteLine($"{limit}\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("press 1 for IsDuplicated\n");
                Console.WriteLine("press 2 for ShowDocumentDetail\n");
                Console.WriteLine("press 3 for ShowPermutationDetail\n");
                Console.WriteLine("press 4 for FindClosest\n");
                Console.WriteLine("press 5 for IsDuplicated a document of text\n");
                Console.WriteLine("press N to To Exit \n");
                Console.WriteLine(limit);

                input = Console.ReadLine();
                string answer;

                Console.ForegroundColor = ConsoleColor.Green;

                //action after selection made on menu
                switch (input)
                {
                    case "1":
                        Console.WriteLine("You select IsDuplicated, Enter a string to be checked..");
                        PrintIsDuplicated(dm.IsDuplicated(Console.ReadLine()));
                        break;

                    case "2":
                        Console.WriteLine("You select ShowDocumentDetail, Enter a string to be know the Document details..");
                        dm.ShowDocumentDetail(Console.ReadLine(), out answer);
                        Console.WriteLine(answer);
                        break;

                    case "3":
                        Console.WriteLine("You select ShowPermutationDetail, Enter a string to be know the Permutation details..");
                        dm.ShowPermutationDetail(Console.ReadLine(), out answer);
                        Console.WriteLine(answer);
                        break;

                    case "4":
                        Console.WriteLine("You select FindClosest, Enter a string to find the closest permutation.. ");
                        dm.FindClosest(Console.ReadLine(), out answer);
                        Console.WriteLine(answer);
                        break;
                    case "5":
                        Console.WriteLine("You select FindClosest for a document of .txt, Enter a PATH to be know all Document details..");
                        foreach (string path in Directory.GetFiles(Console.ReadLine()))//@"C:/Users/Owner/Documents/Visual Studio 2015/Projects/ProjectDataStructure/Txt"
                        {

                            // The files used in this example are created in the topic
                            // How to: Write to a Text File. You can change the path and
                            // file name to substitute text files of your own.
                            string text = System.IO.File.ReadAllText(path);

                            // Display the file contents to the console. Variable text is a string.
                            System.Console.WriteLine("Contents of the document = {0}", text);
                            PrintIsDuplicated(dm.IsDuplicated(text));
                            Console.WriteLine();

                        }
                        break;

                    default:
                        if (input.ToLower() != "n") Console.WriteLine("Wrong choice.");
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;

            } while (input.ToLower() != "n");

        }

        //print a text (use in delegate)
        private static void print(string msg)
        {
            Console.WriteLine(msg);
        }

        //print a text for IsDuplicated
        private static void PrintIsDuplicated(Duplicated text)
        {
            switch (text)
            {
                case (Duplicated.DocExist):
                    Console.WriteLine("The Document exist. The permutation have been created..");
                    break;

                case (Duplicated.NoExist):
                    Console.WriteLine("The Document didn't exist. It have been created..");
                    break;

                case (Duplicated.PermExist):
                    Console.WriteLine("The Document exist. The permutation have been updated..");
                    break;

                case (Duplicated.Error):
                    Console.WriteLine("The input text isn't valid..");

                    break;
                default:
                    break;
            }
        }

       
    }
}
