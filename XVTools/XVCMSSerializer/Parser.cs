using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

public class Parser
{
    private string saveLocation;
    private byte[] rawBytes;
    private byte[] bytes;
    private CMS_File cmsFile;

    public Parser(string location, bool writeXml = false)
    {
        saveLocation = location;
        rawBytes = File.ReadAllBytes(location);
        bytes = new byte[rawBytes.Length];
        Array.Copy(rawBytes, bytes, rawBytes.Length);
        cmsFile = new CMS_File();
    }

    public void Parse()
    {
        int count = BitConverter.ToInt32(rawBytes, 8);
        int offset = BitConverter.ToInt32(rawBytes, 12);
        cmsFile.CMS_Entries.Clear();

        for (int i = 0; i < count; i++)
        {
            CMS_Entry entry = new CMS_Entry();
            entry.Index = BitConverter.ToInt32(rawBytes, offset).ToString();
            entry.Str_04 = GetString(rawBytes, offset + 4);
            entry.I_08 = BitConverter.ToInt64(rawBytes, offset + 8);
            entry.I_16 = BitConverter.ToInt32(rawBytes, offset + 16);
            entry.I_20 = BitConverter.ToUInt16(rawBytes, offset + 20);
            entry.I_22 = BitConverter.ToUInt16(rawBytes, offset + 22);
            entry.I_24 = BitConverter.ToUInt16(rawBytes, offset + 24);
            entry.I_26 = BitConverter.ToUInt16(rawBytes, offset + 26);
            entry.I_28 = BitConverter.ToInt32(rawBytes, offset + 28);
            entry.Str_32 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 32));
            entry.Str_36 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 36));
            entry.Str_44 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 44));
            entry.Str_48 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 48));
            entry.Str_56 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 56));
            entry.Str_60 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 60));
            entry.Str_64 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 64));
            entry.Str_68 = GetString(rawBytes, BitConverter.ToInt32(rawBytes, offset + 68));

            cmsFile.CMS_Entries.Add(entry);
            offset += 80;
        }
    }

    private string GetString(byte[] data, int offset)
    {
        int endOffset = offset;
        while (endOffset < data.Length && data[endOffset] != 0)
        {
            endOffset++;
        }
        return Encoding.UTF8.GetString(data, offset, endOffset - offset);
    }

    public void SaveToXml(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement root = xmlDoc.DocumentElement;
        xmlDoc.InsertBefore(xmlDeclaration, root);

        XmlElement cmsElement = xmlDoc.CreateElement("CMS");
        xmlDoc.AppendChild(cmsElement);

        foreach (var entry in cmsFile.CMS_Entries)
        {
            XmlElement entryElement = xmlDoc.CreateElement("Entry");
            entryElement.SetAttribute("ID", entry.Index);
            cmsElement.AppendChild(entryElement);

            XmlElement str_04_element = xmlDoc.CreateElement("Str_04");
            str_04_element.InnerText = entry.Str_04;
            entryElement.AppendChild(str_04_element);

            XmlElement i_08_element = xmlDoc.CreateElement("I_08");
            i_08_element.InnerText = entry.I_08.ToString();
            entryElement.AppendChild(i_08_element);

            XmlElement i_16_element = xmlDoc.CreateElement("I_16");
            i_16_element.InnerText = entry.I_16.ToString();
            entryElement.AppendChild(i_16_element);

            XmlElement i_20_element = xmlDoc.CreateElement("I_20");
            i_20_element.InnerText = entry.I_20.ToString();
            entryElement.AppendChild(i_20_element);

            XmlElement i_22_element = xmlDoc.CreateElement("I_22");
            i_22_element.InnerText = entry.I_22.ToString();
            entryElement.AppendChild(i_22_element);

            XmlElement i_24_element = xmlDoc.CreateElement("I_24");
            i_24_element.InnerText = entry.I_24.ToString();
            entryElement.AppendChild(i_24_element);

            XmlElement i_26_element = xmlDoc.CreateElement("I_26");
            i_26_element.InnerText = entry.I_26.ToString();
            entryElement.AppendChild(i_26_element);

            XmlElement i_28_element = xmlDoc.CreateElement("I_28");
            i_28_element.InnerText = entry.I_28.ToString();
            entryElement.AppendChild(i_28_element);

            XmlElement str_32_element = xmlDoc.CreateElement("Str_32");
            str_32_element.InnerText = entry.Str_32;
            entryElement.AppendChild(str_32_element);

            XmlElement str_36_element = xmlDoc.CreateElement("Str_36");
            str_36_element.InnerText = entry.Str_36;
            entryElement.AppendChild(str_36_element);

            XmlElement str_44_element = xmlDoc.CreateElement("Str_44");
            str_44_element.InnerText = entry.Str_44;
            entryElement.AppendChild(str_44_element);

            XmlElement str_48_element = xmlDoc.CreateElement("Str_48");
            str_48_element.InnerText = entry.Str_48.Trim();
            entryElement.AppendChild(str_48_element);

            XmlElement str_56_element = xmlDoc.CreateElement("Str_56");
            str_56_element.InnerText = entry.Str_56;
            entryElement.AppendChild(str_56_element);

            XmlElement str_60_element = xmlDoc.CreateElement("Str_60");
            str_60_element.InnerText = entry.Str_60;
            entryElement.AppendChild(str_60_element);

            XmlElement str_64_element = xmlDoc.CreateElement("Str_64");
            str_64_element.InnerText = entry.Str_64;
            entryElement.AppendChild(str_64_element);

            XmlElement str_68_element = xmlDoc.CreateElement("Str_68");
            str_68_element.InnerText = entry.Str_68;
            entryElement.AppendChild(str_68_element);
        }

        xmlDoc.Save(path);
    }

    public void RemoveInvalidCharactersFromXml(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        foreach (XmlNode node in xmlDoc.DocumentElement.SelectNodes("//*[text()]"))
        {
            string cleanedText = RemoveInvalidCharacters(node.InnerText);
            cleanedText = cleanedText.Replace("#CMS", "");

            // Rimuovi i caporali aggiunti dalla rimozione dei caratteri non validi
            cleanedText = cleanedText.Trim();

            node.InnerText = cleanedText;
        }

        xmlDoc.Save(path);
    }

    public string RemoveInvalidCharacters(string input)
    {
        StringBuilder output = new StringBuilder();
        foreach (char c in input)
        {
            if (XmlConvert.IsXmlChar(c) && c != '�' && !char.IsControl(c))
            {
                output.Append(c);
            }
        }

        return output.ToString();
    }

    public void DeserializeToCMS(string xmlPath, string cmsPath)
    {
        throw new NotImplementedException("Function not yet implemented");
    }

    static void Main(string[] args)
    {
        if (args[0].EndsWith(".cms"))
        {
            // Esempio di utilizzo:
            Parser parser = new Parser(args[0], writeXml: true);
            parser.Parse();
            parser.SaveToXml("char_model_spec.xml");
            parser.RemoveInvalidCharactersFromXml("char_model_spec.xml");
            Console.WriteLine("CMS file parsed successfully.");
        }
        else if (args[0].EndsWith(".xml"))
        {
            Parser parser = new Parser(args[0]);
            parser.DeserializeToCMS(args[0], "char_model_spec.cms");
            Console.WriteLine("XML file serialized successfully.");
        }
        else
        {
            Console.WriteLine("Unknown File Type");
        }
    }
}

