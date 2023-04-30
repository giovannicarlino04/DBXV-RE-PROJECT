package action_script
{
   import flash.display.MovieClip;
   import flash.external.ExternalInterface;
   
   public class Callback
   {
       
      
      public var SeTypeDecide:int;
      
      public var SeTypeCancel:int;
      
      public var SeTypeNg:int;
      
      public var SeTypeCarsol:int;
      
      public var SeTypePageChnage:int;
      
      public var SeTypeWindowOpen:int;
      
      public var SeTypeWindowClose:int;
      
      public var SeTypeChatWindow:int;
      
      public var SeTypeChatText:int;
      
      public var SeTypeChatRefrect:int;
      
      public var SeTypeTitleStart:int;
      
      public var SeTypeThankYou:int;
      
      public var SeTypeCount:int;
      
      public var SeTypeInvalid:int;
      
      public var m_mc_target:MovieClip;
      
      public var m_user_data:Array;
      
      public var m_user_data_valid_flag:Array;
      
      public function Callback(param1:int)
      {
         var _loc2_:int;
         super();
         this.SeTypeInvalid = -1;
         this.SeTypeDecide = 0;
         this.SeTypeCancel = 1;
         this.SeTypeNg = 2;
         this.SeTypeCarsol = 3;
         this.SeTypePageChnage = 4;
         this.SeTypeWindowOpen = 5;
         this.SeTypeWindowClose = 6;
         this.SeTypeChatWindow = 7;
         this.SeTypeChatText = 8;
         this.SeTypeChatRefrect = 9;
         this.SeTypeTitleStart = 10;
         this.SeTypeThankYou = 11;
         this.SeTypeCount = 12;
         this.m_mc_target = null;
         this.m_user_data = new Array(param1);
         this.m_user_data_valid_flag = new Array(param1);
         _loc2_ = 0;
         while(param1 > _loc2_)
         {
            this.m_user_data_valid_flag[_loc2_] = false;
            _loc2_++;
         }
         try
         {
            ExternalInterface.addCallback("GotoAndPlayLabel",this.AddCallbackGotoAndPlayLabel);
            ExternalInterface.addCallback("GotoAndPlayFrame",this.AddCallbackGotoAndPlayFrame);
            ExternalInterface.addCallback("GotoAndStopLabel",this.AddCallbackGotoAndStopLabel);
            ExternalInterface.addCallback("GotoAndStopFrame",this.AddCallbackGotoAndStopFrame);
            ExternalInterface.addCallback("SetUserData",this.AddCallbackSetUserDataInt);
            ExternalInterface.addCallback("SetUserData",this.AddCallbackSetUserDataString);
            return;
         }
         catch(e:Error)
         {
            return;
         }
      }
      
      public function CallbackUserData(param1:String, param2:int, param3:int) : void
      {
         try
         {
            ExternalInterface.call(param1,param2,param3);
            return;
         }
         catch(e:Error)
         {
            return;
         }
      }
      
      public function CallbackUserDataString(param1:String, param2:int, param3:String) : void
      {
         try
         {
            ExternalInterface.call(param1,param2,param3);
            return;
         }
         catch(e:Error)
         {
            return;
         }
      }
      
      public function CallbackCancel() : void
      {
         this.CallbackUserData("cancel",0,0);
      }
      
      public function CallbackDecide(param1:int) : void
      {
         this.CallbackUserData("decide",param1,0);
      }
      
      public function CallbackExit() : void
      {
         this.CallbackUserData("exit",0,0);
      }
      
      public function CallbackSe(param1:int) : void
      {
         this.CallbackUserData("playSe",param1,0);
      }
      
      public function SetCallbacTarget(param1:MovieClip) : void
      {
         this.m_mc_target = param1;
      }
      
      public function GetUserDataInt(param1:int) : int
      {
         if(param1 < 0)
         {
            return 0;
         }
         if(param1 >= this.m_user_data.length)
         {
            return 0;
         }
         return this.m_user_data[param1];
      }
      
      public function GetUserDataString(param1:int) : String
      {
         if(param1 < 0)
         {
            return "";
         }
         if(param1 >= this.m_user_data.length)
         {
            return "";
         }
         return this.m_user_data[param1];
      }
      
      public function SetUserDataValidFlag(param1:int, param2:Boolean) : void
      {
         if(param1 < 0)
         {
            return;
         }
         if(param1 >= this.m_user_data_valid_flag.length)
         {
            return;
         }
         this.m_user_data_valid_flag[param1] = param2;
      }
      
      public function GetUserDataValidFlag(param1:int) : Boolean
      {
         if(param1 < 0)
         {
            return false;
         }
         if(param1 >= this.m_user_data_valid_flag.length)
         {
            return false;
         }
         return this.m_user_data_valid_flag[param1];
      }
      
      public function AddCallbackGotoAndPlayLabel(param1:String) : void
      {
         this.m_mc_target.gotoAndPlay(param1);
      }
      
      public function AddCallbackGotoAndPlayFrame(param1:int) : void
      {
         this.m_mc_target.gotoAndPlay(param1);
      }
      
      public function AddCallbackGotoAndStopLabel(param1:String) : void
      {
         this.m_mc_target.gotoAndStop(param1);
      }
      
      public function AddCallbackGotoAndStopFrame(param1:int) : void
      {
         this.m_mc_target.gotoAndStop(param1);
      }
      
      public function AddCallbackSetUserDataInt(param1:int, param2:int) : void
      {
         if(param1 < 0)
         {
            return;
         }
         if(param1 >= this.m_user_data.length)
         {
            return;
         }
         this.m_user_data[param1] = param2;
         this.m_user_data_valid_flag[param1] = true;
      }
      
      public function AddCallbackSetUserDataString(param1:int, param2:String) : void
      {
         if(param1 < 0)
         {
            return;
         }
         if(param1 >= this.m_user_data.length)
         {
            return;
         }
         this.m_user_data[param1] = param2;
         this.m_user_data_valid_flag[param1] = true;
      }
   }
}
