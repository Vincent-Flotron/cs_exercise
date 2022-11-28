using System.Text.RegularExpressions;
// Author:          Vincent Flotron
// Start date:      2022-11-25 23h30
// First Realease:  2022-11-26 02h35

Print("Started.");

/* (1) */
string inputpath = "";
string outputpath = "";

if(!DealingWithArgsOk(args, out inputpath, out outputpath)) return 0;

// Open and read all the file
Print("Read the input file \"" + inputpath + "\".");
string content = File.ReadAllText(inputpath);

/* (2) */
DateTime strt = DateTime.Now;   // to measure the time
Print("Start measuring the time: " + strt.ToString("hh\\:mm\\:ss\\.ffff") + ".");

// Extract the datas from the input file
Print("Extract the datas.");
MatchCollection matches = ExtractDatasFromString(content);

// Reshape the datas
Print("Reshape.");
List<string> values = ReshapeDatas(matches);

// Sort the datas
Print("Sort.");
values.Sort();
values.Reverse();

// Measure the time used for the step 2
TimeSpan dt = DateTime.Now - strt;
Print("Stop measuring the time. dt:  " + dt.ToString("hh\\:mm\\:ss\\.ffff") + ".");

/* (3, 4) */
// Write the ouptut file
Print("Write the output file \"" + outputpath + "\".");
WriteFile(outputpath, values, dt);

// // Compare the outputed file with the Correct Output.txt
// if(!AreFilesTheSame("Correct Output.txt", outputpath)){
//     return 1;
// }

Print("Finished.");
return 0;


//#################################################################################################
// Functions
//#############################################################################################
static MatchCollection ExtractDatasFromString(string content){
    Regex reg = new Regex("(?:\"PA)(?<postnb>\\d+?)(?::proALPHA:)(?<nb>[^\":\\r\\n]+)(?:\")");
    MatchCollection matches = reg.Matches(content);

    return matches;
}

static void WriteFile(string outputpath, List<string> values, TimeSpan dt){
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

List<string> ReshapeDatas(MatchCollection matches){
    List<string> values = new List<string>(matches.Count);
    foreach(Match m in matches){
        values.Add(m.Groups[2].Value+m.Groups[1].Value);
    }
    return values;
}

bool DealingWithArgsOk(string[] args, out string inputpath, out string outputpath)
{
    inputpath = "Input.txt";
    outputpath = "Output.txt";

    // No args. Use default parameters
    if(args.Length == 0)
        return true;

    // Help
    if(args[0]=="--help" || args[0]=="-h"){
        DisplayHelp();
        return false;
    }
    // Path of input and output files
    else if(args[0]!="" && args[1]!=""){
        inputpath = args[0];
        outputpath = args[1];
    }
    // Bad arguments
    else{
        DisplayHelp();
        return false;
    }

    return true;
}

void DisplayHelp(){
    Console.WriteLine("cs_exercise [--help]");
    Console.WriteLine("cs_exercise [full_input_file_path] [full_output_file_path]");
    Console.WriteLine("Exemples: ");
    Console.WriteLine("\tcs_exercise --help");
    Console.WriteLine("\tcs_exercise inputfile1.txt outputfile1.txt");
}

void Print(string txt){
    Console.WriteLine(DateTime.Now.ToString("yyyy\\-MM\\-dd\\ hh\\:mm\\:ss\\:ffff") + ": " + txt);
}

