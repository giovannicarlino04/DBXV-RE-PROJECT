using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public struct IGGYFlashHeader32
{
    public byte[] Signature;
    public int MainOffset;
    public int As3NamesSectionOffset;
    public int As3CodeOffset;
    public int NamesOffset;
    public int LastSection;
}

public class Program
{
    public static List<string> ExtractAS3Scripts(byte[] data)
    {
        List<string> scripts = new List<string>();

        using (StreamReader reader = new StreamReader(new MemoryStream(data)))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("package"))
                {
                    scripts.Add(line);
                }
            }
        }

        return scripts;
    }

    public static IGGYFlashHeader32 Read(byte[] data, int offset)
    {
        IGGYFlashHeader32 hdr = new IGGYFlashHeader32();

        hdr.Signature = data.Skip(offset).Take(4).ToArray();
        hdr.MainOffset = BitConverter.ToInt32(data, offset + 4);
        hdr.As3NamesSectionOffset = BitConverter.ToInt32(data, offset + 8);
        hdr.As3CodeOffset = BitConverter.ToInt32(data, offset + 12);
        hdr.NamesOffset = BitConverter.ToInt32(data, offset + 16);
        hdr.LastSection = BitConverter.ToInt32(data, offset + 20);

        return hdr;
    }

    public byte[]? As3CodeSection { get; set; }
    public byte[]? As3NamesSection { get; set; }

    public bool LoadFlashData32(byte[] data)
    {
        IGGYFlashHeader32 hdr = Read(data, 0);

        this.As3CodeSection = data.Skip(hdr.MainOffset).ToArray();
        this.As3NamesSection = data.Skip(hdr.MainOffset + hdr.As3NamesSectionOffset).ToArray();

        return true;
    }

    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: iggy_file.exe <file>");
            return;
        }

        string filename = args[0];

        try
        {
            string scriptsFolder = Path.Combine(Path.GetDirectoryName(filename), "scripts");
            if (!Directory.Exists(scriptsFolder))
            {
                Directory.CreateDirectory(scriptsFolder);
            }

            using (var file = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[file.Length];
                file.Read(data, 0, data.Length);

                Program iggyFile = new Program();
                IGGYFlashHeader32 hdr = Read(data, 0);

                byte[] swfBlob = data.Skip(hdr.MainOffset + hdr.As3NamesSectionOffset + hdr.As3CodeOffset).ToArray();

                File.WriteAllBytes(Path.GetDirectoryName(filename) + @"/CHARASELE.swf", swfBlob);

                List<string> scripts = ExtractAS3Scripts(File.ReadAllBytes(Path.GetDirectoryName(filename) + @"/CHARASELE.swf"));

                foreach (string script in scripts)
                {
                    string scriptFile = Path.Combine(scriptsFolder, script + ".as");
                    using (StreamWriter writer = new StreamWriter(scriptFile))
                    {
                        writer.WriteLine(script);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.Read();
        }
    }
}
