using ProjectDataStructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Xaml;

namespace ProjectDataStructure
{
    public static class StringArrayComparer
    {
        public static int Compare(string[] x, string[] y)
        {
            if (x.Length == 0 && y.Length == 0) return 0;
            if (y.Length == 0) return string.Compare(x[0], "");
            if (x.Length == 0) return string.Compare("", y[0]);

            for (int i = 0; i < Math.Min(x.Length, y.Length); i++)
            {
                if (string.Compare(x[i], y[i]) != 0) return string.Compare(x[i], y[i]);
            }

            if (x.Length > y.Length)
            {
                return string.Compare(x[Math.Min(x.Length, y.Length)], "");
            }
            else if (x.Length < y.Length)
            {
                return string.Compare("", y[Math.Min(x.Length, y.Length)]);
            }
            else
            {
                return 0;
            }

        }
    }



    public class DocManagement
    {
        //alow to print a message when delate old document
        private delegate void PrintData(string msg);
        private PrintData DataPrinter;

        //Data Structure declaration
        private Hash<string, Document> DocHash;
        internal static Queue<TimeData> DateQueue;

        //timer
        private System.Timers.Timer aTimer = new System.Timers.Timer();
        private TimeSpan timeS;

        //define max and min word that can take a document
        private int minWord, maxWord;

        //ctor
        public DocManagement(Action<string> dataMsg, int minWord = 5, int maxWord = 10, int delateAfterSeconds = 10)
        {
            DataPrinter = new PrintData(dataMsg);
            this.minWord = minWord; this.maxWord = maxWord;
            DocHash = new Hash<string, Document>(9);
            DateQueue = new Queue<TimeData>();

            timeS = new TimeSpan(0, 0, 0, delateAfterSeconds);

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = delateAfterSeconds * 1000;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            DeleteOld();
        }


        public Duplicated IsDuplicated(string text)
        {
            string[] arrTextPermut = StringTraitment(text);
            string tmp;
            if (!IsValid(arrTextPermut, out tmp))
            {
                return Duplicated.Error;
            }

            string permutText = arrToString(arrTextPermut);
            Array.Sort(arrTextPermut);
            string docText = arrToString(arrTextPermut);
            //find the doc
            Document doc;
            if (DocHash.GetValue(docText, out doc))
            {
                //find the perm
                Permutation perm;
                if (doc.PermExist(permutText, out perm))
                {
                    //update permutaion
                    doc.countPermutation++;
                    perm.Update();
                    return Duplicated.PermExist;
                }
                else
                {
                    //create perm
                    doc.NewPermutation(permutText);
                    return Duplicated.DocExist;
                }

            }
            else
            {
                //create a doc
                doc = new Document(docText);
                doc.NewPermutation(permutText);
                DocHash.Add(docText, doc);
                return Duplicated.NoExist;
            }
        }

        public bool ShowDocumentDetail(string searchedString, out string description)
        {
            string[] arrTextDoc = StringTraitment(searchedString);
            if (!IsValid(arrTextDoc, out description)) return false; //return false if the string isn't valid

            //format the string
            Array.Sort(arrTextDoc);
            string docText = arrToString(arrTextDoc);

            //look for the doc
            Document doc;
            if (DocHash.GetValue(docText, out doc))
            {
                description = $"Number of document : {doc.countPermutation}\nNumber of differrent Permutation : {doc.tree.count}";
                return true;
            }
            else
            {
                description = $"Number of document : 0\nNumber of differrent Permutation : 0";
                return false;
            }
        }

        //look for Permutation
        public bool ShowPermutationDetail(string searchedString, out string description)
        {
            //check the validity of the text
            string[] arrTextPermut = StringTraitment(searchedString);
            if (!IsValid(arrTextPermut, out description)) return false;
            
            //format the string
            string permutText = arrToString(arrTextPermut);
            Array.Sort(arrTextPermut);
            string docText = arrToString(arrTextPermut);

            //look for Doc
            Document doc;
            if (DocHash.GetValue(docText, out doc))
            {
                //look for perm
                Permutation perm;
                if (doc.PermExist(permutText, out perm))
                {
                    description = $"Number of permutation : {perm.count}\nLast update of the Permutation : {perm.timeD.dateStamp}";
                    return true;
                }
                else
                {
                    description = $"Document Exist, but not this permutation";
                    return false;
                }

            }
            else
            {
                description = $"Neither the document nor the permutation exists.";
                return false;

            }
        }

        //look for closest
        public Matching FindClosest(string searchedString, out string description)
        {
            //check validity of hte string
            string[] arrTextPermut = StringTraitment(searchedString);
            if (!IsValid(arrTextPermut, out description)) return Matching.Error;
            
            //format the string
            string permutText = arrToString(arrTextPermut);
            Array.Sort(arrTextPermut);
            string docText = arrToString(arrTextPermut);

            //look for Doc
            Document doc;
            if (DocHash.GetValue(docText, out doc))
            {
                //look for perm
                Permutation perm;
                Matching test = doc.FindClosest(permutText, out perm);
                if (test == Matching.Match)
                {
                    description = $"Exact Match have been founded : {perm.permutation}";
                    return Matching.Match;
                }
                else if (test == Matching.Closest)
                {
                    description = $"Aproximatif Match have been founded : {perm.permutation}";
                    return Matching.Closest;
                }
                else
                {
                    description = $"No Match have been founded!";
                    return Matching.NoMatch;
                }

            }
            else
            {
                description = $"Neither the document nor the permutation exists.";
                return Matching.NoMatch;

            }
        }
        //check string validity
        private bool IsValid(string[] arr, out string answer)
        {
            if (arr.Length < minWord)
            {
                answer = $"your document doesn't contain enough words";
                return false;
            }
            else if (arr.Length > maxWord)
            {
                answer = $"your document contain to much words";
                return false;
            }
            else { answer = string.Empty; return true; }
        }

        //delete Old Document
        private void DeleteOld()
        {
            DataPrinter("Delete old Document"); // invok.. print this string when DeleteOld Activated
            TimeData tData;
            int countPerm = 0;
            int countDoc = 0;

            //DeQueue till it delete all old perm and document
            while (DateQueue.LastElement(out tData))
            {
                if (tData.dateStamp < DateTime.Now - timeS)
                {
                    DateQueue.DeQueue(out tData);

                    //format the string to find the old permutation
                    string permutationString = tData.permutation;
                    string[] docString = permutationString.Split(' ');
                    Array.Sort(docString);
                    string docText = arrToString(docString);

                    //find doc
                    Document doc;
                    if (DocHash.GetValue(docText, out doc))
                    {
                        //find perm
                        Permutation perm;
                        if (doc.PermExist(permutationString, out perm))
                        {
                            //remove perm from the Doc BST
                            doc.tree.Remove(perm, out perm);
                            doc.countPermutation -= perm.count;
                            countPerm++;

                            //remove doc if all Doc's perm have been deleted
                            if (doc.countPermutation < 1)
                            {
                                DocHash.Remove(doc.mainPermutation, out doc);
                                countDoc++;
                            }
                        }
                    }
                }
                else
                {
                    //return how much doc and perm have been delated
                    if (countDoc > 0)
                        DataPrinter($"{countDoc} Document have been deleted");
                    if (countPerm > 0)
                        DataPrinter($"{countPerm} Permutation have been deleted");
                    break;
                }
            }

        }

        //transform Array to String
        static internal string arrToString(string[] arr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != arr.Length - 1)
                {
                    sb.Append($"{arr[i]} ");
                }
                else
                {
                    sb.Append($"{arr[i]}");

                }
            }
            return sb.ToString();
        }

        //transform the string to remove all non-accepted char, and transform to array
        static internal string[] StringTraitment(string sentence)
        {
            sentence = sentence.ToLower();
            StringBuilder sb = new StringBuilder(sentence);
            for (int i = 0; i < sb.Length; i++)
            {
                if (!char.IsLetter(sb[i]) && !char.IsWhiteSpace(sb[i])) sb.Replace(sb[i], ' ', i, 1);
                if (i > 0 && char.IsWhiteSpace(sb[i]) && char.IsWhiteSpace(sb[i - 1])) { i--; sb.Remove(i, 1); };

            }
            if (sb.Length > 0)
                if (char.IsWhiteSpace(sb[0])) sb.Remove(0, 1);
            if (sb.Length > 0)
                if (char.IsWhiteSpace(sb[sb.Length - 1])) sb.Remove(sb.Length - 1, 1);

            string[] arr = sb.ToString().Split(' ');
            return arr;
        }
    }
}

