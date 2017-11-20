using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LucySpreadsheetConverter
{
    class Program
    {
        static List<string[]> allContent = new List<string[]>();

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting...");

            string sourceNameFile = "C:\\Dev\\ThreeSixty\\LucySpreadsheetConverter\\files\\source.csv";

            System.Console.WriteLine("Reading Name File " + sourceNameFile);

            string[] sourceNames = null; // = System.IO.File.ReadAllLines(sourceNameFile);
            try
            {
                sourceNames = System.IO.File.ReadAllLines(sourceNameFile);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("ERROR - Could not read file!");
            }

            System.Console.WriteLine(string.Format("Read {0} lines from {1}", sourceNames.Length, sourceNameFile));


            int i=1;
            foreach (string line in sourceNames)
            {

                var content = line.Split(new char[] { ',' });
                allContent.Add(content);
                //System.Console.WriteLine(string.Format("Line {0} - {1}", i++, content.Length));
            }

            // Process all teh files
            string[] filesToConvert = System.IO.Directory.GetFiles("C:\\Dev\\ThreeSixty\\LucySpreadsheetConverter\\files\\to Convert");
            foreach (string file in filesToConvert)
            {
                System.Console.WriteLine(string.Format("Processing file {0}", file));

                // Create the new file
                var newFile = System.IO.File.CreateText("C:\\Dev\\ThreeSixty\\LucySpreadsheetConverter\\files\\Converted\\output1.csv");
      //          var failFile = System.IO.File.CreateText("C:\\Dev\\ThreeSixty\\LucySpreadsheetConverter\\files\\Converted\\failed.csv");

                var allLines = System.IO.File.ReadAllLines(file);
                foreach (string p in allLines)
                {
                    // split the line up
                    List<string> tokens = new List<string>();
                    TokenizeString(ref tokens, p, ',', false, false, false, true);

//                    string[] content = p.Split(new char[] { ',' });

                    //System.Console.WriteLine("Looking up " + tokens[2]);


                    string[] source = Lookup(tokens[2].ToLower());

                    string newLine = ""; //source[1] + "," + source[2] + "," + source[3] + "," + source[9] + "," + p;
//                        tokens.Inser
                    if (source != null)
                    {
                         newLine = source[1] + "," + source[2] + "," + source[3] + "," + source[9] + "," + p;
//                        tokens.Insert(0, source[9]);
  //                      tokens.Insert(0, source[3]);
    //                    tokens.Insert(0, source[2]);
      //                  tokens.Insert(0, source[1]);

                    }
                    else
                    {
                        newLine = ",,,," + p;
//                        for (int j = 0;  j < 4; j++)
  //                          tokens.Insert(0, "");
                        
                 //       string fLine =  string.Join(",", tokens);
                  //      failFile.WriteLine(fLine);

                    }

       //             string newLine =  string.Join(",", tokens);
                    newFile.WriteLine(newLine);
                    

                }
                newFile.Close();
        //        failFile.Close();
                
            }

            System.Console.ReadKey();
        }

        static string[] Lookup(string emailSearch)
        {
            foreach(string[] source in allContent)
            {
                if (string.IsNullOrEmpty(source[0]))
                    continue;

                string realEmail = source[0].ToLower();
                if (emailSearch.Contains(realEmail))
                {
                    //System.Console.WriteLine("Found the name : " + source[0]);
                    return source;
                }
            }
            System.Console.WriteLine("Failed to find email - " + emailSearch);
            return null;
        }

        public static void TokenizeString(
        ref List<string> aTokens,
        string sSource,
        char Delimiter,
        bool bTrimSpaces,
        bool bIgnoreEscapeChar,
        bool bIgnoreBlanks,
        bool bRemoveQuotes)		// Split string but remove quotes that were found
        {
            int i = 0;
            bool bInQuotes = false;
            int nStart = i;
            string sToken = "";
            bool bStartToken = true;

            while (i < sSource.Length)
            {
                if (sSource[i] == '"')
                {
                    if (bInQuotes)
                        bInQuotes = false;
                    else if (bStartToken)
                        bInQuotes = true;
                }
                bStartToken = false; // Only check first character for "

                if (bInQuotes)
                {
                    i++;
                    continue;
                }
                if (sSource[i] == Delimiter)
                {
                    if (!bIgnoreEscapeChar)
                    {
                        if (i > 0 && sSource[i - 1] == '\\')
                        {
                            i++;
                            continue;
                        }
                    }

                    sToken = sSource.Substring(nStart, i - nStart);
                    if (bRemoveQuotes && sToken.Length > 0)
                    {
                        if (sToken[0] == '"' && sToken[sToken.Length - 1] == '"')
                        {
                            string c = sToken.Substring(1, sToken.Length - 2);
                            sToken = c;
                        }
                    }
                    if (bTrimSpaces)
                    {
                        sToken.Trim();
                    }
                    if (sToken.Length > 0 || !bIgnoreBlanks)
                        aTokens.Add(sToken);
                    nStart = ++i;
                    bStartToken = true;
                    continue;
                }
                i++;
            }
            sToken = sSource.Substring(nStart);
            if (sToken.Length > 0 || !bIgnoreBlanks)
                aTokens.Add(sToken);
        }

    }
}
