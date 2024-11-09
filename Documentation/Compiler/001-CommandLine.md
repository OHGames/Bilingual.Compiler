# Command Line
The Bilingual Compiler is a command line app that will compile Bilingual files, create/update localization file, 
and add line ids to the files.

## Help Command
You can use the `help` command verb for a list of help info. Using the `--help` flag on any command will show the help info as well.

## Compiling
To compile Bilingual files, the `compile` command verb is used. The `compile` command requires an input directory and an output directory.
The input directory is where all the `.bi` files are located. The output directory is where all the compiled files will be. 
The output directory will mimic the paths of the input directory. So a file in `input/my_dir` will be compiled and placed into `output/my_dir`.
Compiled Bilingual files are formatted in JSON.

You can optionally use the `--pretty` (`-p`), `--bson` (`-b`), and `--change-extension` (`-c`).
- The `--pretty` command will pretty-print the compiled files. Defaults to false.
- The `--bson` command will compile the files into BSON instead of JSON. This will force `--change-extension` to be true. Defaults to false.
- The `--change-extension` command will change the file extension from `.json` to `.bic` to prevent conflict. Defaults to false.

### Compiling Examples
To compile will all default settings, use `--input` and `--output` flags.
```ps
> .\Bilingual.Compiler.exe --input "Path/To/My/BilingualFiles" --output "Path/To/Compiled/Files"
```

To pretty print, use the `--pretty` flag.
```ps
> .\Bilingual.Compiler.exe --input "Path/To/My/BilingualFiles" --output "Path/To/Compiled/Files" --pretty
```

You can also use the shorthand flag names. The following compiles with pretty-print and with the `.bic` file extension.
```ps
> .\Bilingual.Compiler.exe -i "Path/To/My/BilingualFiles" -o "Path/To/Compiled/Files" -p -c
```

## Localizing
The Bilingual.Compiler.exe can also be use to generate localization files. Localization files are `.csv` files that aid in translating the dialogue across languages.
To to use the localizer, use the `localize` command verb. The required flags change based on the function the localizer will perform. 

When generating localization files:
- The `--input` (`-i`) directory is required.
- The `--output` (`-o`) directory is required.
- The `--locale` (`-l`) code is required.
- The `--dont-zip` (`-z`) flag is optional.
- The `--force` flag is optional.

When updating existing localization files:
- The `--input` (`-i`) directory is required.
- The `--locale` (`-l`) code is required.
- The `--update` (`-u`) flag is required and should be present.
- The `--zip-path` (`-p`) file is required.

**Line Ids will always be added when localizing!**

### Generating Localization Files
To generate a localization file, set the options outlined above. To prevent the generation of a zip folder, use the `--dont-zip` flag. 
When `--dont-zip` is true, the `--output` directory must be empty. If the directory is not empty, the user will be prompted to delete everything or abort.
If `--force` is set to true, it will not prompt the user and instead delete the files automatically. **Deleted files cannot be retrieved!**

The `--locale` code is a basic locale code such as "en" or "en_US".

### Updating Localization Files
Add the `--update` flag to update localization files. The `--zip-path` will be the file to update. The `--input` directory will be the `.bi` files to use.
The zip file will be zipped up when finished.