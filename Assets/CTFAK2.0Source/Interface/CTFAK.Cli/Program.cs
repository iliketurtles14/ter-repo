using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;
using CTFAK;
using CTFAK.Core.CCN.Chunks.Banks.SoundBank;
using CTFAK.EXE;
using CTFAK.FileReaders;
using CTFAK.Tools;
using CTFAK.Utils;

public class Program
{
    public static IFileReader gameParser;
    public static string builddate = "7/17/23";
    public static bool didToolArg = false;

    // Public lists to store files
    public static List<Bitmap> ImageList = new List<Bitmap>();
    public static List<SoundItem> SoundList = new List<SoundItem>();

    public static void Main(string[] args)
    {
        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule != null)
        {
            var pathToExe = processModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            Directory.SetCurrentDirectory(pathToContentRoot);
        }
        CTFAK.CTFAKCore.Init();
        ASCIIArt.SetStatus("Idle");
        Directory.CreateDirectory("Plugins");
        Directory.CreateDirectory("Dumps");
        ASCIIArt.DrawArt2();
        ASCIIArt.DrawArt();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("by 1987kostya and Yunivers");
        Console.ResetColor();
        Thread.Sleep(700);
        Console.Clear();

        ASCIIArt.DrawArt();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("by 1987kostya and Yunivers");
        Console.WriteLine($"Running {builddate} build.\n");

        // Read game path from config.ini
        string configFilePath = "config.ini";
        string path = ReadGamePathFromConfig(configFilePath);

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Console.WriteLine("ERROR: File not found or invalid path in config.ini");
            return;
        }

        // Set CTFAKCore.path
        CTFAKCore.path = path;

        var types = Assembly.GetAssembly(typeof(ExeFileReader)).GetTypes();

        List<IFileReader> availableReaders = new List<IFileReader>();

        if (Path.GetExtension(path) == ".exe")
            gameParser = new ExeFileReader();
        else if (Path.GetExtension(path) == ".apk")
        {
            if (File.Exists(Path.GetTempPath() + "application.ccn"))
                File.Delete(Path.GetTempPath() + "application.ccn");
            path = ApkFileReader.ExtractCCN(path);
            gameParser = new CCNFileReader();
        }
        else if (Path.GetExtension(path) == ".mfa")
            gameParser = new MFAFileReader();
        else
        {
            foreach (var rawType in types)
                if (rawType.GetInterface(typeof(IFileReader).FullName) != null)
                    availableReaders.Add((IFileReader)Activator.CreateInstance(rawType));
            foreach (var item in Directory.GetFiles("Plugins", "*.dll"))
            {
                var newAsm = Assembly.LoadFrom(Path.GetFullPath(item));
                foreach (var pluginType in newAsm.GetTypes())
                    if (pluginType.GetInterface(typeof(IFileReader).FullName) != null)
                        availableReaders.Add((IFileReader)Activator.CreateInstance(pluginType));
            }
            gameParser = availableReaders.FirstOrDefault();
        }

        var readStopwatch = new Stopwatch();
        readStopwatch.Start();
        ASCIIArt.DrawArt();
        ASCIIArt.SetStatus("Reading game");
        Console.WriteLine($"Reading game with \"{gameParser.Name}\"");
        gameParser.PatchMethods();
        gameParser.LoadGame(path);
        IFileReader game = gameParser.Copy();
        readStopwatch.Stop();

        ASCIIArt.DrawArt();
        Console.WriteLine($"Reading finished in {readStopwatch.Elapsed.TotalSeconds} seconds");

        // Save images and sounds to public lists
        ImageList.AddRange(gameParser.getGameData().Images.Items.Select(image => image.Value.bitmap));
        SoundList.AddRange(gameParser.getGameData().Sounds.Items);
        Console.WriteLine("Files have been saved to public lists.");

        string mapName = "ClickteamMemoryMap";

        // Convert SoundList to raw sound data
        List<byte[]> rawSoundList = SoundList.Select(sound => sound.Data).ToList();

        string data = SerializeData(ImageList, rawSoundList);

        using (MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen(mapName, 1024 * 1024 * 10)) // 10 MB
        {
            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Data written to memory-mapped file.");
                Console.ReadLine(); // Keep the program running for Unity to connect
            }
        }
    }

    static string ReadGamePathFromConfig(string configFilePath)
    {
        IniFile ini = new IniFile(configFilePath);
        string path = ini.Read("GameFolderPath", "Settings");
        Console.WriteLine(path);
        return path.Trim('"'); // Remove surrounding quotes if present
    }

    static string SerializeData(List<Bitmap> images, List<byte[]> sounds)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");

        // Convert images to Base64 strings
        sb.Append("\"ImageList\":[");
        for (int i = 0; i < images.Count; i++)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                images[i].Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                string base64Image = Convert.ToBase64String(ms.ToArray());
                sb.Append("\"" + base64Image + "\"");
                if (i < images.Count - 1) sb.Append(",");
            }
        }
        sb.Append("],");

        // Convert sounds to Base64
        sb.Append("\"SoundList\":[");
        for (int i = 0; i < sounds.Count; i++)
        {
            string base64Sound = Convert.ToBase64String(sounds[i]);
            sb.Append("\"" + base64Sound + "\"");
            if (i < sounds.Count - 1) sb.Append(",");
        }
        sb.Append("]");

        sb.Append("}");
        return sb.ToString();
    }
}