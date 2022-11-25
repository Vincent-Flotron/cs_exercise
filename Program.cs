using System.Text.RegularExpressions;
// Author:          Vincent Flotron
// Start date:      2022-11-25 23h30
// First Realease:  2022-11-26 02h35



/* (1) */
string inputpath = "Input.txt";
string outputpath = "Output.txt";
string correctOutputpath = "Correct Output.txt";

// Open and read all the file
string content = File.ReadAllText(inputpath);

/* (2) */
DateTime strt = DateTime.Now;   // to measure the time

// Extract the datas from the input file
List<string> values = ExtractDatasFromString(content
    , new Regex("(?:\"PA)(?<postnb>\\d+?)(?::proALPHA:)(?<nb>[^\":\\r\\n]+)(?:\")"));

// Sort the datas
values.Sort();
values.Reverse();

// Measure the time used for the step 2
TimeSpan dt = DateTime.Now - strt;

/* (3, 4) */
// Write the ouptut file
WriteFile(outputpath, values, dt);

// Compare the outputed file with the Correct Output.txt
if(!AreFilesTheSame(correctOutputpath, outputpath)){
    return 1;
}


Console.WriteLine("Finished");
return 0;




//#################################################################################################
// Functions
//#############################################################################################
static List<string> ExtractDatasFromString(string content, Regex reg)
{
    MatchCollection matches = reg.Matches(content);

    List<string> values = new List<string>(matches.Count);
    foreach(Match m in matches){
        values.Add(m.Groups[2].Value+m.Groups[1].Value);
    }
    return values;
}

static void WriteFile(string outputpath, List<string> values, TimeSpan dt)
{
    using (StreamWriter strW = new StreamWriter(File.Open(outputpath
        , FileMode.Create, FileAccess.Write))){

        strW.WriteLine("Zeit " + dt.ToString("hh\\:mm\\:ss\\:ffff"));
        foreach(string line in values){
            strW.WriteLine(line);
        }
    }
}

static bool AreFilesTheSame(string file1, string file2){
    bool areTheSame = true;
    
    using(StreamReader strRFile1 = new StreamReader(File.OpenRead(file1))){
        using(StreamReader strRFile2 = new StreamReader(File.OpenRead(file2))){
            strRFile1.ReadLine();
            strRFile2.ReadLine();
            string? output1Line = null;
            string? output2Line = null;
            int i = 1;
            do{
                output1Line = strRFile1.ReadLine();
                output2Line = strRFile2.ReadLine();
                i++;
                if(output2Line != output1Line){
                    Console.WriteLine(i + ") file1 ("+output1Line+") != file2 ("+output2Line+")");
                    areTheSame = false;
                }
            }while(output1Line != null && output2Line != null);
        }
    }

    return areTheSame;
}




