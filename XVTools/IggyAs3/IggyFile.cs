using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IggyAs3
{
    public class IggyFile
    {
        public class IggyHeader
        {
            public uint Signature { get; set; }
            public uint Version { get; set; }
            public byte[] Platform { get; set; }
            public uint Unk0C { get; set; }
            public byte[] Reserved { get; set; }
            public uint NumSubfiles { get; set; }
            public uint Width { get; set; }
            public uint Height { get; set; }
        }

        public class IggySubFileEntry
        {
            public uint Id { get; set; }
            public uint Size { get; set; }
            public uint Size2 { get; set; }
            public uint Offset { get; set; }
        }

        public IggyHeader Header { get; set; }
        public List<IggySubFileEntry> SubFiles { get; set; }
        public List<byte> FlashData { get; set; }
        public List<byte> AS3CodeSection { get; set; }
        public List<byte> IndexDataSection { get; set; }
        public List<byte> MainSection { get; set; }

        public uint SWFVersion { get; set; }

        public IggyFile()
        {
            Header = new IggyHeader();
            SubFiles = new List<IggySubFileEntry>();
            FlashData = new List<byte>();
            AS3CodeSection = new List<byte>();
            IndexDataSection = new List<byte>();
            MainSection = new List<byte>();
            SWFVersion = 10;
        }

        public bool Load(Stream stream)
        {
            try
            {
                using (var br = new BinaryReader(stream))
                {
                    var header = new IggyHeader
                    {
                        Signature = br.ReadUInt32(),
                        Version = br.ReadUInt32(),
                        Platform = br.ReadBytes(4),
                        Unk0C = br.ReadUInt32(),
                        Reserved = br.ReadBytes(0xC),
                        NumSubfiles = br.ReadUInt32()
                    };

                    Header = header;

                    var subFiles = new List<IggySubFileEntry>();
                    for (int i = 0; i < Header.NumSubfiles; i++)
                    {
                        var subFile = new IggySubFileEntry
                        {
                            Id = br.ReadUInt32(),
                            Size = br.ReadUInt32(),
                            Size2 = br.ReadUInt32(),
                            Offset = br.ReadUInt32()
                        };
                        subFiles.Add(subFile);
                    }

                    SubFiles = subFiles;

                    for (int i = 0; i < SubFiles.Count; i++)
                    {
                        Console.WriteLine($"SubFile - ID: 0x{SubFiles[i].Id:X}, Size: {SubFiles[i].Size}, Offset: 0x{SubFiles[i].Offset:X}");
                    }

                    if (SubFiles.Count < 2)
                    {
                        Console.WriteLine("Not all sections are available in the iggy file.");
                        return false;
                    }

                    var flashDataOffset = SubFiles[0].Offset;
                    var flashDataSize = SubFiles[0].Size;
                    var as3CodeSectionOffset = SubFiles[1].Offset;
                    var as3CodeSectionSize = SubFiles[1].Size;

                    Console.WriteLine($"FlashData offset: 0x{flashDataOffset:X}, FlashData size: {flashDataSize}");
                    Console.WriteLine($"AS3CodeSection offset: 0x{as3CodeSectionOffset:X}, AS3CodeSection size: {as3CodeSectionSize}");

                    // Move the stream to the correct positions for reading the sections.
                    br.BaseStream.Seek(flashDataOffset, SeekOrigin.Begin);
                    FlashData = new List<byte>(br.ReadBytes((int)flashDataSize));

                    br.BaseStream.Seek(as3CodeSectionOffset, SeekOrigin.Begin);
                    AS3CodeSection = new List<byte>(br.ReadBytes((int)as3CodeSectionSize));

                    // Validate if all the data has been read correctly.
                    if (FlashData.Count != flashDataSize || AS3CodeSection.Count != as3CodeSectionSize)
                    {
                        Console.WriteLine("Failed to read data from iggy file.");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }
        }

        public byte[] Save()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        // Write the header.
                        bw.Write(Header.Signature);
                        bw.Write(Header.Version);
                        bw.Write(Header.Platform);
                        bw.Write(Header.Unk0C);
                        bw.Write(Header.Reserved);
                        bw.Write(SubFiles.Count); // Set NumSubfiles based on the number of sections.

                        // Store the offset and size of the AS3 code section.
                        uint as3CodeSectionOffset = (uint)((uint)ms.Position + SubFiles.Count * 16);
                        uint as3CodeSectionSize = (uint)AS3CodeSection.Count;

                        // Write the subfile entries (including the AS3 code section entry).
                        for (int i = 0; i < SubFiles.Count; i++)
                        {
                            var subFileEntry = new IggySubFileEntry
                            {
                                Id = SubFiles[i].Id,
                                Size = i == 0 ? (uint)FlashData.Count : as3CodeSectionSize,
                                Size2 = i == 0 ? (uint)FlashData.Count : as3CodeSectionSize,
                                Offset = i == 0 ? 0 : as3CodeSectionOffset
                            };

                            bw.Write(subFileEntry.Id);
                            bw.Write(subFileEntry.Size);
                            bw.Write(subFileEntry.Size2);
                            bw.Write(subFileEntry.Offset);
                        }

                        // Write the FlashData.
                        bw.Write(FlashData.ToArray());

                        // Write the AS3CodeSection.
                        bw.Write(AS3CodeSection.ToArray());

                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return null;
            }
        }
        private void WriteSubFileEntry(BinaryWriter bw, uint id, uint size, uint size2, uint offset)
        {
            bw.Write(id);
            bw.Write(size);
            bw.Write(size2);
            bw.Write(offset);
        }
    }
}
