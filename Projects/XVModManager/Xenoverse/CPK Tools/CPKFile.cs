
using System;
using System.Linq;
using System.Text;
using System.IO;


namespace Xenoverse
{
	public class CPKFile{
		
		public CPK cpk;
		public Boolean isInit = false;
	    public string extractPath { get; set; }
	      BinaryReader br;

		public CPKFile(){
			
			
		}

		  public byte[] GetFileBytesFromCPK(string file){
     		
        	for (int i = 0; i < cpk.FileTable.Count;i++){
        		
     		 	if (cpk.FileTable[i].DirName + @"/" + cpk.FileTable[i].FileName == file){
        			     br.BaseStream.Seek((long)cpk.FileTable[i].FileOffset, SeekOrigin.Begin);
                    string isComp = Encoding.ASCII.GetString(br.ReadBytes(8));
                    br.BaseStream.Seek((long)cpk.FileTable[i].FileOffset, SeekOrigin.Begin);
					int size = Int32.Parse((cpk.FileTable[i].ExtractSize ?? cpk.FileTable[i].FileSize).ToString());
                    byte[] chunk = br.ReadBytes(Int32.Parse(cpk.FileTable[i].FileSize.ToString()));
                    if (isComp == "CRILAYLA")
                    {	
                       chunk = cpk.DecompressCRILAYLA(chunk, size);
                    }
                       return chunk;
         	}

        	}
     		 return null;
        
        }
		
		public void ExtractAll(){
			
			for (int i = 0; i < cpk.FileTable.Count;i++){
				
                    if(cpk.FileTable[i].DirName != null ){ // actual file and not a header
                    
					ExtractByIndex(i);
                    
                 }
			}
		}
		public void ExtractByIndex(int index){
			
			     br.BaseStream.Seek((long)cpk.FileTable[index].FileOffset, SeekOrigin.Begin); // sets the pointer at file offset
                    string isComp = Encoding.ASCII.GetString(br.ReadBytes(8)); // Read Compression Type
                      br.BaseStream.Seek((long)cpk.FileTable[index].FileOffset, SeekOrigin.Begin);  // return to original pos after readubg CompType

                    byte[] chunk = br.ReadBytes(Int32.Parse(cpk.FileTable[index].FileSize.ToString()));           
        
                 
                  
                       int size = Int32.Parse((cpk.FileTable[index].ExtractSize ?? cpk.FileTable[index].FileSize).ToString());
                       if (isComp == "CRILAYLA"){ // Is Compression CRILAYLA?
                       	 chunk = cpk.DecompressCRILAYLA(chunk, size); // decompress the chunk
                       }
                       Directory.CreateDirectory(extractPath + cpk.FileTable[index].DirName.ToString());
                       File.WriteAllBytes(string.Format("{0}{1}/{2}",extractPath,cpk.FileTable[index].DirName.ToString(),cpk.FileTable[index].FileName.ToString()),chunk); // write chunk;
                 

		}
		
		
	}

}
