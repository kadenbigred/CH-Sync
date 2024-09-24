using System;
using System.IO;

class Program
{
    public class GlobalVariables
    {
        public static List<string> songNames = new List<string> { };
        public static List<string> songPaths = new List<string> { };
        public static List<string> songNamesFromInput = new List<string> { };
        public static List<string> songDifferences = new List<string> { };
        public static string outputPath;
        public static string inputPath;
        public static string songsPath;



    }

    static void Main()
    {

        //Names of the files generated
        string songNameFileName = "SongNames";
        string songPathFileName = "SongPaths";
        string differencesFileName = "Differences";
        string clonedSongsDirectoryName = "DifferentSongs";

        //Current directory of the exe file
        string exeDirectory = Directory.GetCurrentDirectory();

        //Temporary arrays for searching input and output folders
        string[] outputPathArray = Array.Empty<string>();
        string[] inputPathArray = Array.Empty<string>(); ;

        //Type of operation the program will execute, Read or Transfer
        int operationType;

        outputPathArray = Directory.GetDirectories(exeDirectory, "Output", SearchOption.AllDirectories);
        inputPathArray = Directory.GetDirectories(exeDirectory, "Input", SearchOption.AllDirectories);

        //See if input and output directories are found
        if (outputPathArray.Length == 0)
        {
            Console.WriteLine("Could not find Output folder, did you move the exe?");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return;
        }
        else
        {
            GlobalVariables.outputPath = outputPathArray[0];
        }

        if (inputPathArray.Length == 0)
        {
            Console.WriteLine("Could not find Input folder, did you move the exe?");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return;
        }
        else
        {
            GlobalVariables.inputPath = inputPathArray[0];
        }

        if (Directory.Exists(GlobalVariables.outputPath + @"\" + clonedSongsDirectoryName))
        {
            Console.Write("Found existing " + clonedSongsDirectoryName + " folder in " + Path.GetFileName(GlobalVariables.outputPath) + ". Would you like to delete it? (y/n): ");

            string deleteCloned = Console.ReadLine();
            if (deleteCloned.ToLower() == "y")
            {
                try
                {
                    Directory.Delete(GlobalVariables.outputPath + @"\" + clonedSongsDirectoryName, true);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error: Access denied to file.");
                    Console.WriteLine("You may be able to delete the file manually.");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine("An error occurred: " + ex.Message);
                    Console.WriteLine("You may be able to delete the file manually.");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }

            }
            Console.Clear();
        }


        //Ask the user for their song directory
        Console.WriteLine("What is the directory of the folder containing your songs?: ");
        GlobalVariables.songsPath = Console.ReadLine();
        Console.Clear();

        //Ask the user if they would like to read or transfer
        Console.WriteLine("What action would you like do to:");
        Console.WriteLine("1. Read");
        Console.WriteLine("2. Compare");
        Console.WriteLine(" ");
        Console.Write("(Type the number): ");
        Int32.TryParse(Console.ReadLine(), out operationType);
        Console.Clear();
        Console.WriteLine("Finding local songs...");
        Console.Clear();




        //Find the song folder
        FindSongFile(GlobalVariables.songsPath);

        //Create SongNames file
        CreateTextFile(GlobalVariables.outputPath + @"\" + songNameFileName + ".txt", GlobalVariables.songNames);
        //Create SongPaths file
        CreateTextFile(GlobalVariables.outputPath + @"\" + songPathFileName + ".txt", GlobalVariables.songPaths);


        if (operationType == 2)
        {
            Console.WriteLine("");
            Console.WriteLine("Comparing local songs with input...");

            //Converts the SongNames File into a List
            InputToList(GlobalVariables.inputPath + @"\" + songNameFileName + ".txt");
            //Compares local songs and songs from input
            CompareLists(GlobalVariables.songNames, GlobalVariables.songNamesFromInput);
            Console.WriteLine("");

            if (GlobalVariables.songDifferences.Count == 0)
            {
                Console.WriteLine("Found no differences in libraries, either something was done wrong or your libraries are already synced.");
            }
            else
            {
                Console.WriteLine("Found " + GlobalVariables.songDifferences.Count + " differences:");

                //Prompts the user if they would like to log the found differences to a text file
                Console.Write("Would you like to log these differences to a text file? (y/n): ");
                string logDiff = Console.ReadLine();
                if (logDiff.ToLower() == "y")
                {
                    CreateTextFile(GlobalVariables.outputPath + @"\" + differencesFileName + ".txt", GlobalVariables.songDifferences);
                }

                //Prompts the user if they would like to clone the different songs to output
                Console.WriteLine("");
                Console.WriteLine("Would you like to copy files that are found to be currently installed and different to a new directory? (y/n): ");
                Console.Write("(Note: this may use significant disk space depending on the amount of songs) ");
                string copyFiles = Console.ReadLine();
                if (copyFiles.ToLower() == "y")
                {

                    CopyAvailableDifferences(GlobalVariables.outputPath + @"\" + songNameFileName + ".txt", GlobalVariables.outputPath + @"\" + songPathFileName + ".txt", clonedSongsDirectoryName);
                }
            }






        }

        Console.WriteLine("");
        Console.WriteLine("Press any key to exit.");

        Console.ReadKey();

    }
    static void FindSongFile(string path)
    {
        try
        {
            // Get all directories in the current path
            string[] directories = Directory.GetDirectories(path);

            // Check each directory
            foreach (string dir in directories)
            {
                // Search for the song.ini file in the current directory
                string[] files = Directory.GetFiles(dir, "song.ini");

                foreach (string file in files)
                {
                    // Find and print the line containing "name ="
                    string nameLine = FindNameLineInFile(file);
                    if (nameLine != null)
                    {
                        GlobalVariables.songNames.Add(nameLine);
                        GlobalVariables.songPaths.Add(file.Substring(0, file.Length - 9));
                    }
                }

                // Recursively search in subdirectories
                FindSongFile(dir);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Access denied to: " + path);
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Directory not found: " + path);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static string FindNameLineInFile(string filePath)
    {
        try
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Search for the line containing "name ="
            foreach (string line in lines)
            {
                if (line.StartsWith("name =", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(7); // Return the line if it starts with "name =", remove the beginning of the string
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not read from file" + filePath + ":" + ex.Message);
        }

        return null; // Return null if not found or an error occurred
    }

    static void CreateTextFile(string path, List<string> list)
    {
        string fileName = path;

        try
        {
            // Check if file already exists. If yes, delete it.
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                    Console.WriteLine("Deleted existing " + Path.GetFileName(fileName) + " file");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            // Create a new file
            using (StreamWriter sw = File.CreateText(fileName))
            {
                foreach (string item in list)
                {
                    sw.WriteLine(item);
                }
                Console.WriteLine("Created " + Path.GetFileName(fileName) + " in " + Path.GetFileName(Path.GetDirectoryName(fileName) + " folder"));
            }

        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }

    static void InputToList(string path)
    {
        try
        {
            // Read all lines from the file into an array
            string[] lines = File.ReadAllLines(path);

            // Convert the array to a List<string>
            GlobalVariables.songNamesFromInput = new List<string>(lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void CompareLists(List<string> listA, List<string> listB)
    {
        // Find differences and add them to the differences list
        GlobalVariables.songDifferences.AddRange(listA.Except(listB)); // Items in listA but not in listB
        GlobalVariables.songDifferences.AddRange(listB.Except(listA)); // Items in listB but not in listA
    }

    static void CopyAvailableDifferences(string localPath, string pathsPath, string outputDirectoryName)
    {

        //Creates list to store the paths of the different songs
        List<string> differencePaths = new List<string> { };

        try
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(localPath);
            string[] pathLines = File.ReadAllLines(pathsPath);

            // Goes through differences list and local files list to find paths of mismatched songs
            for (int i = 0; i < lines.Length; i++)
            {
                for (int x = 0; x < GlobalVariables.songDifferences.Count; x++)
                {
                    if (lines[i] == GlobalVariables.songDifferences[x])
                    {
                        differencePaths.Add(pathLines[i]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not read from file" + localPath + ":" + ex.Message);
        }


        //Path of the folder the cloned songs will be stored in 
        string clonesPath = GlobalVariables.outputPath + @"\" + outputDirectoryName;
        //Create DifferentSongs directory in Output folder
        Directory.CreateDirectory(clonesPath);

        Console.WriteLine("Copying files...");
        //Go through list of paths and clone songs to DifferentSongs directory
        for (int i = 0; i < differencePaths.Count; i++)
        {
            foreach (var file in Directory.GetFiles(differencePaths[i]))
            {
                Directory.CreateDirectory(clonesPath + @"\" + Path.GetFileName(differencePaths[i]));
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(clonesPath + @"\" + Path.GetFileName(differencePaths[i]), fileName);
                File.Copy(file, destFile, true); // Overwrite if the file already exists
            }

            // Copy subdirectories and their contents
            foreach (var directory in Directory.GetDirectories(differencePaths[i]))
            {
                string directoryName = Path.GetFileName(directory);
                string destDirectory = Path.Combine(differencePaths[i], directoryName);
            }
        }
        if (differencePaths.Count == 0)
        {
            Console.WriteLine("");
            Console.WriteLine("No files copied, you must not have any installed songs the other user doesnt have.");
            Console.WriteLine("Try repeating the process the opposite way around.");
        }
        else
        {
            Console.WriteLine("Successfully copied " + differencePaths.Count + " files to " + outputDirectoryName + " subfolder in " + Path.GetFileName(GlobalVariables.outputPath) + " folder.");
        }



    }

}

