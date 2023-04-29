package action_script
{
   public class CharaList
   {
      
      public static var VarTypeCode:int = 0;
      
      public static var VarTypeMid:int = 1;
      
      public static var VarTypeModelPreset:int = 2;
      
      public static var VarTypeUnlockIndex:int = 3;
      
      public static var VarTypeVoiceIdList:int = 4;
      
      public static var VarTypeNum:int = 5;
      
      public static var InvalidCode:String = "";
      
      public static var AvatarCode:String = "HUM";
      
      public static var Dlc3CodeChara0:String = "DLC3_0";
      
      public static var Dlc3CodeChara1:String = "DLC3_1";
      
      public static var Dlc3CodeChara2:String = "DLC3_2";
      
      public static var CharaListDlc0_0:Array = [[[AvatarCode,0,0,0,[-1,-1]],[AvatarCode,1,0,0,[-1,-1]],[AvatarCode,2,0,0,[-1,-1]],[AvatarCode,3,0,0,[-1,-1]],[AvatarCode,4,0,0,[-1,-1]],[AvatarCode,5,0,0,[-1,-1]],[AvatarCode,6,0,0,[-1,-1]],[AvatarCode,7,0,0,[-1,-1]]],[["TRX",2,0,0,[92,93]],["TRX",5,0,1,[92,93]],["TRX",6,0,2,[92,93]],["TRX",0,0,3,[92,93]],["TRX",1,0,4,[92,93]],["TRX",8,0,5,[-1,-1]]],[["TRS",0,0,0,[90,91]],["TRS",1,0,1,[90,91]]],[["GOK",0,0,0,[46,47]],["GOK",1,0,1,[46,47]],["GOK",2,0,2,[46,47]],["GOK",3,0,3,[46,47]],["GOK",8,0,4,[46,47]],["GOK",11,0,5,[46,47]],["GNY",2,0,2,[46,47]],["GOK",4,0,7,[46,47]],["GOK",7,0,8,[46,47]],["GOK",5,0,9,[46,47]],["GOK",6,0,10,[46,47]],["GOK",9,0,11,[46,47]],["GOK",13,0,12,[46,47]],["GOK",10,0,13,[46,47]],["GOK",15,0,14,[46,47]]],[["GOD",0,0,0,[44,45]]],[["GTG",0,0,0,[50,51]],["GTG",0,1,1,[50,51]]],[["GHS",0,0,0,[36,37]],["GHS",1,0,1,[36,37]],["GHS",2,0,2,[36,37]],["GHS",3,0,3,[36,37]]],[["GHM",0,0,0,[34,35]],["GHM",1,0,1,[34,35]]],[["GHL",0,0,1,[32,33]],["GHL",1,0,0,[32,33]],["GHL",1,1,2,[32,33]],["GHL",3,0,3,[32,32]],["GHL",4,0,4,[-1,-1]]],[["PIC",0,0,0,[70,71]],["PIC",1,0,1,[70,71]],["PIC",2,0,2,[70,71]],["PIC",3,0,3,[70,71]],["PIC",0,1,4,[70,71]]],[["KLL",0,0,0,[56,57]],["KLL",1,0,1,[56,57]],["KLL",2,0,2,[56,57]],["KLL",3,0,3,[56,57]],["KLL",3,1,4,[56,57]]],[["YMC",0,0,0,[108,109]],["YMC",0,1,1,[108,109]],["YMC",1,0,2,[108,109]]],[["TSH",0,0,0,[96,97]],["TSH",0,1,1,[96,97]],["TSH",1,0,2,[96,97]]],[["RAD",0,0,0,[72,73]],["RAD",1,0,1,[72,73]],["RAD",0,1,2,[72,73]]],[["SBM",3,0,0,[80,81]],["SBM",0,0,1,[80,81]],["SBM",1,0,2,[80,81]],["SBM",2,0,3,[80,81]],["SBM",4,0,4,[80,81]],["SBM",5,0,5,[80,81]]],[["VGT",1,0,0,[102,103]],["VGT",0,0,1,[102,103]],["VGT",3,0,2,[102,103]],["VGT",4,0,3,[102,103]],["VGT",5,0,4,[102,103]],["VGT",7,0,5,[102,103]],["VGT",8,0,7,[102,103]],["VGT",9,0,8,[102,103]],["VGT",10,0,6,[102,103]],["VGT",13,0,9,[-1,-1]]]];
      
      public static var CharaListDlc0_1:Array = [[["NAP",0,0,0,[62,63]],["NAP",1,0,1,[62,63]],["NAP",2,0,2,[62,63]]]];
      
      public static var CharaListDlc1_0:Array = [[["NAP",0,0,0,[62,63]],["NAP",1,0,1,[62,63]],["NAP",2,0,2,[62,63]],["NAP",0,1,3,[62,63]]]];
      
      public static var CharaListDlc0_2:Array = [[["APL",2,0,0,[0,1]],["APL",0,0,1,[0,1]],["APL",1,0,2,[66,67]],["APL",3,0,3,[0,1]],["APL",4,0,4,[66,67]]],[["GNY",0,0,0,[42,43]],["GNY",1,0,1,[42,43]],["GOK",14,0,15,[42,43]],["GNY",3,0,3,[42,43]]],[["RCM",0,0,0,[74,75]],["RCM",1,0,1,[74,75]]],[["RSB",0,0,0,[76,77]],["RSB",1,0,1,[64,65]],["RSB",2,0,2,[76,77]],["RSB",3,0,3,[64,65]]],[["JES",0,0,0,[54,55]],["JES",1,0,1,[54,55]]],[["BAT",0,0,0,[2,3]],["BAT",0,1,1,[2,3]]],[["GRD",0,0,0,[48,49]],["GRD",0,1,1,[48,49]]],[["FRZ",0,0,0,[24,25]],["FRZ",1,0,1,[24,25]]],[["FR4",0,0,0,[22,23]]],[["FR5",0,0,0,[22,23]],["FR5",3,0,1,[-1,-1]]],[["CL3",0,0,0,[18,19]],["CL3",0,1,1,[18,19]]],[["CL4",1,0,0,[18,19]],["CL4",2,0,1,[-1,-1]]],[["CLJ",0,0,0,[20,21]],["CLJ",0,1,1,[20,21]],["CLJ",0,2,2,[20,21]]],[["G17",0,0,0,[26,27]],["G17",0,1,1,[26,27]]],[["G18",0,0,0,[28,29]],["G18",0,1,1,[28,29]],["G18",1,0,2,[28,29]]],[["GTX",0,0,0,[52,53]],["GTX",0,1,1,[52,53]],["GTX",0,2,2,[52,53]],["GTX",1,0,3,[-1,-1]]],[["BUL",0,0,0,[10,11]],["BUL",0,1,1,[10,11]],["BUZ",0,0,0,[10,11]]],[["BUM",0,0,0,[12,13]]],[["BUS",0,0,0,[14,15]],["BUS",1,0,1,[-1,-1]]],[["STN",1,0,0,[60,61]]],[["VDL",0,0,0,[98,99]],["VDL",1,0,1,[98,99]],["VDL",2,0,3,[98,99]],["VDL",3,0,4,[98,99]],["VDL",4,0,5,[98,98]],["VDL",5,0,2,[98,99]]],[["VTO",0,0,0,[104,105]],["VTO",0,1,1,[104,105]],["VTO",0,2,2,[104,105]]],[["BLS",0,0,0,[6,7]]],[["WIS",0,0,0,[106,107]]],[["BDK",0,0,0,[4,5]],["BDK",1,0,1,[4,5]],["BDK",0,1,2,[4,5]]],[["BRL",0,0,0,[8,9]],["BRL",1,0,1,[-1,-1]]]];
      
      public static var CharaListDlc2_0:Array = [[["MIR",0,0,0,[58,59]],["MIR",0,1,1,[58,59]],["MIR",0,2,2,[58,59]],["MIR",0,3,3,[58,59]]],[["TOW",0,0,0,[88,89]]]];
      
      public static var CharaListDlc1_1:Array = [[["GKG",0,0,0,[40,41]],["GKG",1,0,1,[40,41]],["GKG",2,0,2,[40,41]]],[["TRX",7,0,6,[94,95]]],[["PAN",0,0,0,[68,69]],["PAN",0,1,1,[68,69]]]];
      
      public static var CharaListDlc0_3:Array = [[["GK4",0,0,0,[38,39]]]];
      
      public static var CharaListFirstOther:Array = [[["VG4",0,1,0,[100,101]],["VG4",0,0,1,[100,101]]]];
      
      public static var CharaListDlc0_4:Array = [[["GGT",0,0,0,[30,31]]],[["S17",0,0,0,[78,79]]]];
      
      public static var CharaListDlc2_1:Array = [[["SD4",0,0,0,[86,87]]],[["SD3",0,0,0,[84,85]]]];
      
      public static var CharaListDlc0_5:Array = [[["SD1",0,0,0,[82,83]]]];
      
      public static var CharaListDlc3:Array = [[[Dlc3CodeChara0,0,0,0,[116,117]]],[[Dlc3CodeChara1,0,0,0,[112,113]]],[[Dlc3CodeChara2,0,0,0,[114,115]]]];
      
      public static var CharaListFirstJa:Array = [[["JCO",0,0,0,[110,111]]]];
       
      
      public function CharaList()
      {
         super();
      }
      
      public static function GetList(param1:int, param2:int, param3:int, param4:int, param5:*) : Array
      {
         var _loc6_:Array = CharaListDlc0_0;
         if(param1)
         {
            _loc6_ = _loc6_.concat(CharaListDlc1_0);
         }
         else
         {
            _loc6_ = _loc6_.concat(CharaListDlc0_1);
         }
         _loc6_ = _loc6_.concat(CharaListDlc0_2);
         if(param2)
         {
            _loc6_ = _loc6_.concat(CharaListDlc2_0);
         }
         if(param1)
         {
            _loc6_ = _loc6_.concat(CharaListDlc1_1);
         }
         _loc6_ = _loc6_.concat(CharaListDlc0_3);
         if(param5)
         {
            _loc6_ = _loc6_.concat(CharaListFirstOther);
         }
         _loc6_ = _loc6_.concat(CharaListDlc0_4);
         if(param2)
         {
            _loc6_ = _loc6_.concat(CharaListDlc2_1);
         }
         _loc6_ = _loc6_.concat(CharaListDlc0_5);
         if(param3)
         {
            _loc6_ = _loc6_.concat(CharaListDlc3);
         }
         if(param4)
         {
            _loc6_ = _loc6_.concat(CharaListFirstJa);
         }
         var _loc7_:int = 3 - _loc6_.length % 3;
         var _loc8_:int = 0;
         _loc8_ = 0;
         while(_loc7_ > _loc8_)
         {
            _loc6_.concat([[[InvalidCode,0,0,0]]]);
            _loc8_++;
         }
         return _loc6_;
      }
   }
}
