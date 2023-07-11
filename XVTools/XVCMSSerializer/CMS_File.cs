using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CMS_File
{
    public List<CMS_Entry> CMS_Entries { get; }

    public CMS_File()
    {
        CMS_Entries = new List<CMS_Entry>();
    }

    public byte[] SaveToBytes()
    {
        List<byte> byteList = new List<byte>();
        byteList.AddRange(BitConverter.GetBytes(CMS_Entries.Count));
        byteList.AddRange(BitConverter.GetBytes(0)); // Placeholder for offset

        int offset = 16 + 80 * CMS_Entries.Count;
        foreach (CMS_Entry entry in CMS_Entries)
        {
            byteList.AddRange(BitConverter.GetBytes(int.Parse(entry.Index)));
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_04.PadRight(8, '\x00')));
            byteList.AddRange(BitConverter.GetBytes(entry.I_08));
            byteList.AddRange(BitConverter.GetBytes(entry.I_16));
            byteList.AddRange(BitConverter.GetBytes(entry.I_20));
            byteList.AddRange(BitConverter.GetBytes(entry.I_22));
            byteList.AddRange(BitConverter.GetBytes(entry.I_24));
            byteList.AddRange(BitConverter.GetBytes(entry.I_26));
            byteList.AddRange(BitConverter.GetBytes(entry.I_28));

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_32) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_32));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_36) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_36));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_44) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_44));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_48) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_48));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_56) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_56));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_60) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_60));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_64) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_64));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_68) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_68));
            byteList.Add(0);

            byteList.AddRange(BitConverter.GetBytes(offset));
            offset += Encoding.UTF8.GetByteCount(entry.Str_80) + 1;
            byteList.AddRange(Encoding.UTF8.GetBytes(entry.Str_80));
            byteList.Add(0);
        }

        // Update offset
        byteList.GetRange(4, 4).ToArray().CopyTo(byteList.ToArray(), 4);

        return byteList.ToArray();
    }

    public void SortEntries()
    {
        CMS_Entries.Sort((x, y) => int.Parse(x.Index).CompareTo(int.Parse(y.Index)));
    }

    public CMS_Entry GetEntry(string id)
    {
        return CMS_Entries.Find(entry => entry.Index == id);
    }

    public void SaveBinary(string path)
    {
        File.WriteAllBytes(path, SaveToBytes());
    }

    public string CharaIdToCharCode(int charaId)
    {
        string charaIdStr = charaId.ToString();
        CMS_Entry entry = CMS_Entries.Find(e => e.Index == charaIdStr);
        if (entry != null)
        {
            return entry.Str_04;
        }
        return string.Empty;
    }

    public int CharaCodeToCharaId(string charaCode)
    {
        CMS_Entry entry = CMS_Entries.Find(e => e.Str_04 == charaCode);
        if (entry != null)
        {
            return int.Parse(entry.Index);
        }
        return -1;
    }
}

public class CMS_Entry
{
    public string Index { get; set; }
    public string Str_04 { get; set; }
    public long I_08 { get; set; }
    public int I_16 { get; set; }
    public ushort I_20 { get; set; }
    public ushort I_22 { get; set; }
    public ushort I_24 { get; set; }
    public ushort I_26 { get; set; }
    public int I_28 { get; set; }
    public string Str_32 { get; set; }
    public string Str_36 { get; set; }
    public string Str_44 { get; set; }
    public string Str_48 { get; set; }
    public string Str_56 { get; set; }
    public string Str_60 { get; set; }
    public string Str_64 { get; set; }
    public string Str_68 { get; set; }
    public string Str_80 { get; set; }
}
