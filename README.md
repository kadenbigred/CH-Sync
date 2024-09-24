# CH-Sync
Program for syncing Clone Hero song libraries for easy online play

# How to use:

#####  Note: you may have to repeat these steps again oppositely between each other to get a fully synced library, depending on if you each have songs that the others dont.

### Receiving songs:
1. Open CH Sync.exe\
<sub>You may recieve a popup saying windows protected your PC. This is because this is a fairly new program that is not commonly used on windows and therefore is not recognized by defender. Click "More Info" and "Run Anyway" at the bottom. The source code is public and you may read over it if you do not trust the safety of this program.</sub>

3. You will be prompted to input the path of your songs folder, find it and paste it in the terminal window. Do not include a backslash at the end of the directory. (The default path is ``C:\Users\YOURUSERHERE\Documents\Clone Hero\Songs``)

4. You will be asked if you want to Read or Compare songs, enter "1" to Read. Two files will be created in the output folder, ``SongNames.txt``, and ``SongPaths.txt``.

5. Send ``SongNames.txt`` to the user you would like to recieve songs from, ignore ``SongPaths.txt``.

### Sending songs:

1. The user you will be sending songs to should have sent you a file called ``SongNames.txt``, place this in the Input folder.

2. Open CH Sync.exe\
<sub>You may recieve a popup saying windows protected your PC. This is because this is a fairly new program that is not commonly used on windows and therefore is not recognized by defender. Click "More Info" and "Run Anyway" at the bottom. The source code is public and you may read over it if you do not trust the safety of this program.</sub>

4. You will be prompted to input the path of your songs folder, find it and paste it in the terminal window. Do not include a backslash at the end of the directory. (The default path is ``C:\Users\YOURUSERHERE\Documents\Clone Hero\Songs``)

5. You will be asked if you want to Read or Compare songs, enter "2" to compare.
  
6. You will be told how many differences there are between the songs in your library and the other user's. You will then be asked if you would like to log the mismatched songs to a file. If you type "y", a file named ``Differences.txt`` will be created in the Output folder, it lists the differences between you and the other user's song libraries.

7. You will then be asked if you would like to copy the mismatched songs to a new directory. If you type "y" a new folder called ``DifferentSongs`` will be created in the Output folder. This folder will contain copies of all the songs the other user is missing from your library (These are copies, your personal song folder will not be not modified). You can then send this folder to the other user to easily give them the songs they are missing from you.
