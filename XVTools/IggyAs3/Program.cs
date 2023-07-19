using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using IggyAs3;

namespace IggyAs3
{


    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    if (arg.EndsWith(".iggy"))
                    {
                        using (var iggyFile = File.OpenRead(arg))
                        {
                            var iggy = new IggyFile();
                            if (iggy.Load(iggyFile))
                            {

                                foreach (var subFile in iggy.SubFiles)
                                {
                                    Console.WriteLine($"SubFile - ID: 0x{subFile.Id:X}, Size: {subFile.Size}, Offset: 0x{subFile.Offset:X}");
                                }

                                // Salviamo il file SWF solo se FlashData è stato correttamente popolato.
                                if (iggy.FlashData != null)
                                {
                                    byte[] swfData = iggy.Save();

                                    File.WriteAllBytes(arg + ".swf", swfData);
                                    Console.WriteLine("SWF file saved successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("FlashData buffer is null. Unable to save SWF data.");
                                    Console.Read();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Failed to load iggy file.");
                                Console.Read();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                Console.Read();
            }
        }
    }
}