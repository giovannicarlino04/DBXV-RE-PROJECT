package action_script
{
   import flash.display.Bitmap;
   import flash.display.MovieClip;
   import flash.events.Event;
   import flash.events.KeyboardEvent;
   import flash.external.ExternalInterface;
   import flash.ui.Keyboard;
   
   public class CharaSele
   {
      
      private static const ButtonMax:int = 2;
      
      public static const PlayerNumFri:int = 2;
      
      public static const PlayerNumEnm:int = 3;
      
      public static const PlayerMax:int = 1 + PlayerNumFri + PlayerNumEnm;
      
      public static const SkillMax:int = 7;
      
      public static const CharaVarIndexNum:int = 16;
      
      public static const CharacterMax:int = 99;
      
      public static const ReceiveType_FlagUseCancel:int = 0;
      
      public static const ReceiveType_PlayerFriNum:int = 1;
      
      public static const ReceiveType_PlayerEnmNum:int = 2;
      
      public static const ReceiveType_PartyNpcNum:int = 3;
      
      public static const ReceiveType_FlagSelectAvatar:int = 4;
      
      public static const ReceiveType_FlagLocalBattle:int = 5;
      
      public static const ReceiveType_Flag2pController:int = 6;
      
      public static const ReceiveType_Str2pController:int = 7;
      
      public static const ReceiveType_Time:int = 8;
      
      public static const ReceiveType_CharaNameStr:int = 9;
      
      public static const ReceiveType_TarismanHeaderStr:int = 10;
      
      public static const ReceiveType_TarismanNameStr:int = 11;
      
      public static const ReceiveType_ImageStrStart:int = 12;
      
      public static const ReceiveType_ImageStrEnd:int = ReceiveType_ImageStrStart + CharacterMax - 1;
      
      public static const ReceiveType_UnlockVarStart:int = ReceiveType_ImageStrEnd + 1;
      
      public static const ReceiveType_UnlockVarEnd:int = ReceiveType_UnlockVarStart + CharaVarIndexNum * CharacterMax - 1;
      
      public static const ReceiveType_KeyStrL1:int = ReceiveType_UnlockVarEnd + 1;
      
      public static const ReceiveType_KeyStrR1:int = ReceiveType_KeyStrL1 + 1;
      
      public static const ReceiveType_KeyStrL2:int = ReceiveType_KeyStrR1 + 1;
      
      public static const ReceiveType_KeyStrR2:int = ReceiveType_KeyStrL2 + 1;
      
      public static const ReceiveType_KeyStrRU:int = ReceiveType_KeyStrR2 + 1;
      
      public static const ReceiveType_KeyStrRD:int = ReceiveType_KeyStrRU + 1;
      
      public static const ReceiveType_KeyStrRL:int = ReceiveType_KeyStrRD + 1;
      
      public static const ReceiveType_KeyStrRR:int = ReceiveType_KeyStrRL + 1;
      
      public static const ReceiveType_SkillNameStrStart:int = ReceiveType_KeyStrRR + 1;
      
      public static const ReceiveType_SkillNameStrEnd:int = ReceiveType_SkillNameStrStart + SkillMax - 1;
      
      public static const ReceiveType_FlagDlc1:int = ReceiveType_SkillNameStrEnd + 1;
      
      public static const ReceiveType_FlagDlc2:int = ReceiveType_FlagDlc1 + 1;
      
      public static const ReceiveType_FlagDlc3:int = ReceiveType_FlagDlc2 + 1;
      
      public static const ReceiveType_FlagFirstJa:int = ReceiveType_FlagDlc3 + 1;
      
      public static const ReceiveType_FlagFirstOther:int = ReceiveType_FlagFirstJa + 1;
      
      public static const ReceiveType_ImageStrNpcStart:int = ReceiveType_FlagFirstOther + 1;
      
      public static const ReceiveType_ImageStrNpcEnd:int = ReceiveType_ImageStrNpcStart + PlayerNumFri - 1;
      
      public static const ReceiveType_Num:int = ReceiveType_ImageStrNpcEnd + 1;
      
      private static const SendType_SelectCode:int = 0;
      
      private static const SendType_SelectVariation:int = 1;
      
      private static const SendType_SelectMid:int = 2;
      
      private static const SendType_SelectModelPreset:int = 3;
      
      public static const SendType_CurrentPlayerIndex:int = 4;
      
      public static const SendType_RequestCharaInfo:int = 5;
      
      public static const SendType_RequestImageStr:int = 6;
      
      public static const SendType_RequestUnlock:int = 7;
      
      public static const SendType_RequestDecide:int = 8;
      
      public static const SendType_RequestSetFlagSkill:int = 9;
      
      private static const SendType_SelectUnlockIndex:int = 10;
      
      private static const SendType_RequestPlayVoice:int = 11;
      
      private static const IndexNumRow:int = 3;
      
      private static const IndexNumColumn:int = 7;
      
      private static const PlayerIndexOwn:int = 0;
      
      private static const PlayerIndexFriStart:int = PlayerIndexOwn + 1;
      
      private static const PlayerIndexFriEnd:int = PlayerIndexFriStart + PlayerNumFri - 1;
      
      private static const PlayerIndexEnmStart:int = PlayerIndexFriEnd + 1;
      
      private static const PlayerIndexEnmEnd:int = PlayerIndexEnmStart + PlayerNumEnm - 1;
      
      private static const PlayerTeamTypeOwn:int = 0;
      
      private static const PlayerTeamTypeFri:int = 1;
      
      private static const PlayerTeamTypeEnm:int = 2;
      
      private static const PlayerTeamTypeInvalid:int = -1;
      
      private static var SelectInfoTypeListIndex:int = 0;
      
      private static var SelectInfoTypeVarIndex:int = 1;
      
      private static var SelectInfoTypeNum:int = 2;
       
      
      private var m_callback:Callback = null;
      
      private var m_timeline:MovieClip = null;
      
      private var m_timer:CountDownTimer = null;
      
      private var m_current_player_index:int;
      
      private var m_select_info:Array;
      
      private var m_select_row:int;
      
      private var m_select_column:int;
      
      private var m_select_column_start:int;
      
      private var m_select_icon_row:int;
      
      private var m_select_icon_column:int;
      
      private var m_select_var:int;
      
      private var m_flag_skill:Boolean;
      
      private var m_chara_face:Array;
      
      private var m_chara_face_npc:Array;
      
      private var m_flag_unlock:Array;
      
      private var m_flag_change_player:Boolean;
      
      private var m_flag_decide:Boolean;
      
      private var m_flag_exit:Boolean;
      
      private var m_skill_str_width_default:Number;
      
      private var m_skill_str_scalex_default:Number;
      
      private var m_chara_list_num:int = 0;
      
      private var m_chara_num_column:int = 0;
      
      private var m_chara_list:Array;
      
      private var m_dlc3_chara_name:Array;
      
      public function CharaSele()
      {
         super();
         this.m_callback = new Callback(ReceiveType_Num);
         this.m_timeline = null;
         this.m_timer = new CountDownTimer();
         this.m_current_player_index = 0;
         this.m_select_info = new Array(PlayerMax);
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         _loc1_ = 0;
         while(this.m_select_info.length > _loc1_)
         {
            this.m_select_info[_loc1_] = new Array(SelectInfoTypeNum);
            this.m_select_info[_loc1_][SelectInfoTypeListIndex] = 0;
            this.m_select_info[_loc1_][SelectInfoTypeVarIndex] = 0;
            _loc1_++;
         }
         this.m_select_row = 0;
         this.m_select_column = 0;
         this.m_select_column_start = 0;
         this.m_select_icon_row = -1;
         this.m_select_icon_column = -1;
         this.m_select_var = 0;
         this.m_flag_skill = false;
         this.m_flag_change_player = false;
         this.m_flag_decide = false;
         this.m_flag_exit = false;
         this.m_skill_str_width_default = 0;
         this.m_skill_str_scalex_default = 1;
         this.m_chara_list = null;
         this.m_chara_list_num = 0;
         this.m_chara_num_column = 0;
         this.m_dlc3_chara_name = [CharaList.InvalidCode,CharaList.InvalidCode,CharaList.InvalidCode];
         try
         {
            ExternalInterface.addCallback("ForcingCancel",this.pushKeyCancel);
            return;
         }
         catch(e:Error)
         {
            return;
         }
      }
      
      public function Initialize(param1:MovieClip) : void
      {
         this.m_timeline = param1;
         this.m_timeline.visible = false;
         this.m_timeline.cha_frame.visible = false;
         this.m_timeline.cha_skill.visible = false;
         this.m_timeline.cha_select.visible = false;
         this.m_timeline.cha_parameter.visible = false;
         this.m_timeline.cha_arrow.visible = false;
         this.m_timeline.cha_select_cur.visible = false;
         this.m_timeline.press2P.visible = false;
         this.m_timer.Initialize(this.m_timeline.timer.nest._CMN_M_B_mc_timer,null);
         this.m_current_player_index = 0;
         this.m_select_row = 0;
         this.m_select_column = 0;
         this.m_select_column_start = 0;
         this.m_select_var = 0;
         this.m_select_icon_row = -1;
         this.m_select_icon_column = -1;
         this.m_flag_skill = false;
         this.m_flag_change_player = false;
         this.m_flag_decide = false;
         this.m_skill_str_width_default = this.m_timeline.cha_skill.inact_skill.skill01.sys_skill.width;
         this.m_skill_str_scalex_default = this.m_timeline.cha_skill.inact_skill.skill01.sys_skill.scaleX;
         this.m_chara_list = null;
         this.m_chara_list_num = 0;
         this.m_chara_num_column = 0;
         this.SetDlc3Name("GGK","GVG","GFR");
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.waitDlcInfo);
      }
      
      private function waitDlcInfo(param1:Event) : void
      {
         var _loc2_:int = 0;
         if(!this.m_callback)
         {
            return;
         }
         if(!this.m_callback.GetUserDataValidFlag(ReceiveType_FlagDlc1))
         {
            return;
         }
         var _loc3_:int = this.m_callback.GetUserDataInt(ReceiveType_FlagDlc1);
         this.m_chara_list = CharaList.GetList(_loc3_);
         _loc2_ = 0;
         while(this.m_chara_list.length > _loc2_)
         {
            if(this.m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara0)
            {
               this.m_chara_list[_loc2_][0][0] = this.m_dlc3_chara_name[0];
            }
            else if(this.m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara1)
            {
               this.m_chara_list[_loc2_][0][0] = this.m_dlc3_chara_name[1];
            }
            else if(this.m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara2)
            {
               this.m_chara_list[_loc2_][0][0] = this.m_dlc3_chara_name[2];
            }
            _loc2_++;
         }
         this.m_chara_list_num = this.m_chara_list.length;
         this.m_chara_num_column = (this.m_chara_list_num - 1) / IndexNumRow + 1;
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitDlcInfo);
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.requestUnlock);
      }
      
      public function SetDlc3Name(param1:String, param2:String, param3:String) : void
      {
         this.m_dlc3_chara_name[0] = param1;
         this.m_dlc3_chara_name[1] = param2;
         this.m_dlc3_chara_name[2] = param3;
      }
      
      private function requestUnlock(param1:Event) : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:Array = null;
         var _loc4_:Boolean = false;
         var _loc5_:int = 0;
         var _loc6_:Array = null;
         var _loc7_:String = null;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         _loc11_ = 0;
         // Modify the loop condition to use the desired value (99)
         while (99 > _loc11_)
         {
            _loc2_ = false;
            _loc3_ = this.getCharaInfo(_loc11_);
            if (!_loc3_)
            {
               _loc12_ = 0;
               while (CharaVarIndexNum > _loc12_)
               {
                  this.SetUserDataInt(ReceiveType_UnlockVarStart + _loc11_ * CharaVarIndexNum + _loc12_, 0);
                  _loc12_++;
               }
               this.SetUserDataString(ReceiveType_ImageStrStart + _loc11_, "");
            }
            else if (_loc3_.length <= _loc12_)
            {
               _loc12_ = 0;
               while (CharaVarIndexNum > _loc12_)
               {
                  this.SetUserDataInt(ReceiveType_UnlockVarStart + _loc11_ * CharaVarIndexNum + _loc12_, 0);
                  _loc12_++;
               }
               this.SetUserDataString(ReceiveType_ImageStrStart + _loc11_, "");
            }
            else
            {
               _loc4_ = false;
               _loc12_ = CharaVarIndexNum - 1;
               while (0 <= _loc12_)
               {
                  _loc5_ = _loc11_ * CharaVarIndexNum + _loc12_;
                  if (_loc3_.length <= _loc12_)
                  {
                     this.SetUserDataInt(ReceiveType_UnlockVarStart + _loc5_, 0);
                  }
                  else
                  {
                     _loc6_ = _loc3_[_loc12_];
                     _loc7_ = _loc6_[CharaList.VarTypeCode];
                     if (_loc7_ == CharaList.InvalidCode)
                     {
                        this.SetUserDataInt(ReceiveType_UnlockVarStart + _loc5_, 0);
                     }
                     else
                     {
                        _loc8_ = _loc6_[CharaList.VarTypeMid];
                        _loc2_ = true;
                        if (!this.m_callback.GetUserDataValidFlag(ReceiveType_UnlockVarStart + _loc5_))
                        {
                           _loc9_ = _loc6_[CharaList.VarTypeUnlockIndex];
                           _loc10_ = _loc12_;
                           this.m_callback.CallbackUserDataString("user", SendType_SelectCode, _loc7_);
                           this.m_callback.CallbackUserData("user", SendType_SelectUnlockIndex, _loc9_);
                           this.m_callback.CallbackUserData("user", SendType_SelectVariation, _loc10_);
                           this.m_callback.CallbackUserData("user", SendType_SelectMid, _loc8_);
                           this.m_callback.CallbackUserData("user", SendType_RequestUnlock, _loc5_);
                           this.m_callback.CallbackUserData("user", SendType_RequestImageStr, _loc11_);
                        }
                     }
                  }
                  _loc12_--;
               }
               if (!_loc2_)
               {
                  this.SetUserDataString(ReceiveType_ImageStrStart + _loc11_, "");
               }
            }
            _loc11_++;
         }
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME, this.requestUnlock);
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME, this.waitUnlock);
      }

      
      private function waitUnlock(param1:Event) : void
      {
         var _loc7_:* = undefined;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         _loc6_ = 0;
         while(true)
         {
            if(this.m_select_info.length <= _loc6_)
            {
               this.m_current_player_index = PlayerIndexOwn;
               _loc7_ = this.m_select_info[this.m_current_player_index][SelectInfoTypeListIndex];
               this.m_select_row = this.calcIconIndexRow(_loc7_);
               this.m_select_column = this.calcIconIndexColumn(_loc7_);
               this.m_select_var = this.m_select_info[this.m_current_player_index][SelectInfoTypeVarIndex];
               this.sendCharaInfo(this.m_current_player_index);
               this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitUnlock);
               this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.waitStart);
               return;
            }
            _loc2_ = -1;
            _loc5_ = 0;
            for(; CharacterMax > _loc5_; _loc5_++)
            {
               _loc3_ = PlayerIndexOwn + _loc5_;
               _loc3_ = _loc3_ % CharacterMax;
               _loc4_ = this.m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(_loc6_ != PlayerIndexOwn)
               {
                  if(!_loc4_)
                  {
                     if(this.checkAvatar(_loc3_))
                     {
                        continue;
                     }
                  }
               }
               if(!this.checkUnlockChara(_loc3_))
               {
                  continue;
               }
               _loc2_ = _loc3_;
               break;
            }
            if(_loc2_ < 0)
            {
               break;
            }
            this.m_select_row = this.calcIconIndexRow(_loc2_);
            this.m_select_column = this.calcIconIndexColumn(_loc2_);
            this.m_current_player_index = _loc6_;
            this.m_select_var = 0;
            this.setSelectChara();
            _loc6_++;
         }
      }
      
      private function waitStart(param1:Event) : void
      {
         var _loc2_:int = 0;
         _loc2_ = 0;
         while(ReceiveType_Num > _loc2_)
         {
            if(!this.m_callback.GetUserDataValidFlag(_loc2_))
            {
               return;
            }
            _loc2_++;
         }
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitStart);
         this.startMain();
      }
      
      private function ResetIcons() : void
      {
         var _loc1_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrL2);
         var _loc2_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrR2);
         var _loc3_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrRU);
         var _loc4_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrRD);
         var _loc5_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrRL);
         var _loc6_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrRR);
         this.m_timeline.cha_skill.inact_skill.skill01.sys_com01.htmlText = _loc2_ + "+" + _loc3_;
         this.m_timeline.cha_skill.inact_skill.skill02.sys_com01.htmlText = _loc2_ + "+" + _loc5_;
         this.m_timeline.cha_skill.inact_skill.skill03.sys_com01.htmlText = _loc2_ + "+" + _loc6_;
         this.m_timeline.cha_skill.inact_skill.skill04.sys_com01.htmlText = _loc2_ + "+" + _loc4_;
         this.m_timeline.cha_skill.inact_skill.skill05.sys_com01.htmlText = _loc1_ + "+" + _loc3_;
         this.m_timeline.cha_skill.inact_skill.skill06.sys_com01.htmlText = _loc1_ + "+" + _loc5_;
         this.m_timeline.cha_skill.inact_skill.skill07.sys_com01.htmlText = _loc1_ + "+" + _loc4_;
         var _loc7_:String = this.m_callback.GetUserDataString(ReceiveType_TarismanHeaderStr);
         this.m_timeline.cha_skill.inact_skill.skill08.sys_com01.htmlText = _loc7_;
         this.m_timeline.cha_parameter.nest_clothes.sys_clothes.htmlText = "";
         var _loc8_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrL1);
         this.m_timeline.cha_parameter.nest_clothes.sys_l.htmlText = _loc8_;
         var _loc9_:String = this.m_callback.GetUserDataString(ReceiveType_KeyStrR1);
         this.m_timeline.cha_parameter.nest_clothes.sys_r.htmlText = _loc9_;
      }
      
      private function startMain() : void
      {
         var _loc1_:int = 0;
         var _loc2_:MovieClip = null;
         var _loc3_:int = 0;
         var _loc4_:MovieClip = null;
         var _loc5_:Array = null;
         var _loc6_:Bitmap = null;
         var _loc7_:String = null;
         var _loc8_:String = null;
         var _loc9_:String = null;
         var _loc10_:Bitmap = null;
         var _loc11_:MovieClip = null;
         var _loc12_:int = 0;
         var _loc13_:MovieClip = null;
         var _loc14_:int = 0;
         this.m_timeline.visible = true;
         this.m_timeline.cha_frame.cmn_CMN_M_frame.visible = true;
         this.m_timeline.cha_skill.visible = false;
         this.m_timeline.cha_select.visible = true;
         this.m_timeline.cha_parameter.visible = true;
         this.m_timeline.cha_arrow.visible = true;
         var _loc15_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc16_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         if(_loc15_ == 0 && _loc16_ == 0)
         {
            this.m_timeline.cha_parameter.nest_ready.visible = false;
            this.m_timeline.cha_parameter.ready_base.visible = false;
         }
         else
         {
            this.m_timeline.cha_parameter.nest_ready.visible = true;
            this.m_timeline.cha_parameter.ready_base.visible = true;
         }
         var _loc17_:int = this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
         if(_loc17_)
         {
            this.m_timeline.cha_parameter.icon_1P.play();
            this.m_timeline.cha_parameter.icon_1P.visible = true;
            this.m_timeline.cha_parameter.icon_2P.play();
            this.m_timeline.cha_parameter.icon_2P.visible = false;
            this.m_timeline.press2P.gotoAndPlay("start");
            this.m_timeline.press2P.visible = true;
            this.m_timeline.press2P.nest.sys.sys.htmlText = this.m_callback.GetUserDataString(ReceiveType_Str2pController);
         }
         else
         {
            this.m_timeline.cha_parameter.icon_1P.stop();
            this.m_timeline.cha_parameter.icon_1P.visible = false;
            this.m_timeline.cha_parameter.icon_2P.stop();
            this.m_timeline.cha_parameter.icon_2P.visible = false;
            this.m_timeline.press2P.stop();
            this.m_timeline.press2P.visible = false;
         }
         var _loc18_:MovieClip = this.getReadyIconMc(0);
         _loc18_.visible = true;
         _loc14_ = 0;
         while(PlayerNumFri > _loc14_)
         {
            _loc1_ = PlayerIndexFriStart + _loc14_;
            _loc2_ = this.getReadyIconMc(_loc1_);
            if(_loc14_ < _loc15_)
            {
               _loc2_.visible = true;
            }
            else
            {
               _loc2_.visible = false;
            }
            _loc14_++;
         }
         _loc14_ = 0;
         while(PlayerNumEnm > _loc14_)
         {
            _loc3_ = PlayerIndexEnmStart + _loc14_;
            _loc4_ = this.getReadyIconMc(_loc3_);
            if(_loc14_ < _loc16_)
            {
               _loc4_.visible = true;
            }
            else
            {
               _loc4_.visible = false;
            }
            _loc14_++;
         }
         this.m_timeline.cha_parameter.sys_charaName.sys_charaName.htmlText = "";
         if(this.m_timeline.cha_parameter.sys_skill_header)
         {
            this.m_timeline.cha_parameter.sys_skill_header.sys_skill_header.htmlText = "";
         }
         this.setSelectChara();
         this.m_chara_face = new Array(this.m_chara_list_num);
         _loc14_ = 0;
         while(this.m_chara_list_num > _loc14_)
         {
            _loc5_ = this.getCharaInfo(_loc14_);
            _loc6_ = null;
            if(_loc5_ && _loc5_.length > 0)
            {
               _loc7_ = _loc5_[0][CharaList.VarTypeCode];
               _loc8_ = this.m_callback.GetUserDataString(ReceiveType_ImageStrStart + _loc14_);
               if(_loc7_ != CharaList.InvalidCode)
               {
                  _loc6_ = new Bitmap(null);
                  IggyFunctions.setTextureForBitmap(_loc6_,_loc8_);
                  _loc6_.scaleX = 256 / _loc6_.width;
                  _loc6_.scaleY = 128 / _loc6_.height;
               }
            }
            this.m_chara_face[_loc14_] = _loc6_;
            _loc14_++;
         }
         this.m_chara_face_npc = new Array(PlayerNumFri);
         _loc14_ = 0;
         while(PlayerNumFri > _loc14_)
         {
            _loc9_ = this.m_callback.GetUserDataString(ReceiveType_ImageStrNpcStart + _loc14_);
            _loc10_ = new Bitmap(null);
            IggyFunctions.setTextureForBitmap(_loc10_,_loc9_);
            _loc10_.scaleX = 256 / _loc10_.width;
            _loc10_.scaleY = 128 / _loc10_.height;
            this.m_chara_face_npc[_loc14_] = _loc10_;
            _loc14_++;
         }
         var _loc19_:MovieClip = this.m_timeline.cha_select_cur;
         _loc19_.icn_lock.visible = false;
         this.ResetIcons();
         _loc14_ = 0;
         while(PlayerMax > _loc14_)
         {
            if(_loc14_ == this.m_current_player_index)
            {
               this.setReadyIcon(_loc14_,true,false);
            }
            else
            {
               _loc12_ = this.m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
               if(0 <= _loc14_ - 1 && _loc14_ - 1 < _loc12_)
               {
                  this.setReadyIcon(_loc14_,false,true);
               }
               else
               {
                  this.setReadyIcon(_loc14_,false,false);
               }
            }
            _loc11_ = this.getReadyIconMc(_loc14_);
            if(PlayerIndexOwn == _loc14_)
            {
               _loc11_.cmn_icn_you.visible = true;
            }
            else
            {
               _loc11_.cmn_icn_you.visible = false;
            }
            _loc14_++;
         }
         _loc14_ = 0;
         while(CharaVarIndexNum > _loc14_)
         {
            _loc13_ = this.getMcChamyset(_loc14_);
            if(!_loc13_)
            {
            }
            _loc14_++;
         }
         var _loc20_:int = this.m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc20_ <= 0)
         {
            this.m_timeline.timer.visible = false;
         }
         else
         {
            this.m_timeline.timer.visible = true;
            this.m_timer.Start(_loc20_,this.cbFuncEndTimer);
         }
         if(this.m_chara_num_column <= IndexNumColumn)
         {
            this.m_timeline.cha_arrow.visible = false;
         }
         this.m_timeline.cha_select.gotoAndPlay("start");
         this.m_timeline.cha_parameter.gotoAndPlay("start");
         this.m_timeline.cha_frame.cmn_CMN_M_frame.gotoAndPlay("start");
         this.m_timeline.cha_arrow.gotoAndPlay("start");
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.waitMain);
      }
      
      private function waitMain(param1:Event) : void
      {
         if(this.m_timeline.cha_parameter.currentFrame != Utility.GetLabelEndFrame(this.m_timeline.cha_parameter,"start"))
         {
            return;
         }
         this.setSelectChara();
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitMain);
         this.m_timeline.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.checkKey);
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.main);
      }
      
      private function main(param1:Event) : void
      {
         var _loc15_:Boolean = false;
         var _loc16_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:Array = null;
         var _loc5_:Array = null;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:int = this.m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc10_ > 0)
         {
            if(this.m_timer)
            {
               _loc2_ = this.m_timer.GetTime();
               if(_loc2_ <= _loc10_)
               {
                  this.m_timeline.timer.nest._CMN_M_B_mc_timer.visible = true;
               }
               else
               {
                  this.m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
               }
            }
         }
         this.updateLocalBattle();
         this.updateCharaIcon();
         var _loc11_:String = this.m_callback.GetUserDataString(ReceiveType_CharaNameStr);
         this.m_timeline.cha_parameter.sys_charaName.sys_charaName.text = _loc11_;
         this.updateSkill();
         var _loc12_:MovieClip = this.m_timeline.cha_select;
         var _loc13_:MovieClip = _loc12_["chara_icn_set0" + (this.m_select_icon_column + 1)];
         var _loc14_:MovieClip = _loc13_["nest_charaselect0" + (this.m_select_icon_row + 1)];
         if(!this.m_flag_change_player)
         {
            return;
         }
         this.m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.checkKey);
         if(this.m_flag_decide)
         {
            if(this.m_timeline.cha_parameter.currentLabel == "start" || this.m_timeline.cha_parameter.currentLabel == "wait")
            {
               this.m_timeline.cha_parameter.gotoAndPlay("push");
               _loc3_ = this.m_select_info[this.m_current_player_index][SelectInfoTypeListIndex];
               _loc4_ = this.getCharaInfo(_loc3_);
               if(_loc4_)
               {
                  _loc5_ = _loc4_[this.m_select_var][CharaList.VarTypeVoiceIdList];
                  _loc6_ = Math.floor(Math.random() * _loc5_.length);
                  _loc7_ = _loc5_[_loc6_];
                  this.m_callback.CallbackUserData("user",SendType_RequestPlayVoice,_loc7_);
               }
            }
            if(this.m_timeline.cha_parameter.currentFrame < Utility.GetLabelEndFrame(this.m_timeline.cha_parameter,"push"))
            {
               return;
            }
         }
         if(this.m_timeline.cha_parameter.currentLabel == "end_comp")
         {
            this.m_timeline.cha_parameter.nest_clothes.visible = false;
            if(this.m_flag_exit)
            {
               this.end();
               return;
            }
            this.changeCurrentPlayer(this.m_flag_decide);
            this.updatePage();
            this.setSelectChara();
            this.sendCharaInfo(this.m_current_player_index);
            this.m_flag_decide = false;
            this.m_flag_change_player = false;
            _loc15_ = false;
            if(this.m_current_player_index < 0 || PlayerMax <= this.m_current_player_index)
            {
               _loc15_ = true;
            }
            if(_loc15_)
            {
               this.end();
               return;
            }
            _loc16_ = this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
            if(!_loc16_)
            {
               if(_loc10_ > 0)
               {
                  this.m_timer.Start(_loc10_ + 1,this.cbFuncEndTimer);
                  this.m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
               }
            }
            this.m_timeline.cha_parameter.gotoAndPlay("start");
            this.m_timeline.cha_parameter.nest_clothes.visible = true;
            this.m_timeline.stage.addEventListener(KeyboardEvent.KEY_DOWN,this.checkKey);
            return;
         }
         if(this.m_timeline.cha_parameter.currentLabel == "end")
         {
            return;
         }
         this.m_timeline.cha_parameter.gotoAndPlay("end");
         if(!this.m_flag_exit)
         {
            _loc8_ = -1;
            if(this.m_timer)
            {
               _loc8_ = this.m_timer.GetTime();
            }
            this.m_callback.CallbackUserData("user",SendType_RequestDecide,_loc8_);
         }
      }
      
      private function end() : void
      {
         this.m_timeline.cha_select.gotoAndPlay("end");
         this.m_timeline.cha_arrow.gotoAndPlay("end");
         this.m_timeline.cha_skill.gotoAndPlay("end");
         this.m_timeline.cha_frame.cmn_CMN_M_frame.gotoAndPlay("end");
         this.m_timeline.cha_select_cur.visible = false;
         this.m_timer.End();
         this.m_timeline.timer.gotoAndPlay("end");
         var _loc1_:int = this.m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc1_ > 0)
         {
            this.m_timer.Stop();
         }
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.main);
         this.m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.checkKey);
         if(this.m_current_player_index < 0)
         {
            this.m_callback.CallbackCancel();
         }
         else
         {
            this.m_callback.CallbackDecide(1);
         }
         this.m_timeline.stage.addEventListener(Event.ENTER_FRAME,this.waitEnd);
      }
      
      private function waitEnd(param1:Event) : void
      {
         if(this.m_timeline.cha_select.currentFrame != Utility.GetLabelEndFrame(this.m_timeline.cha_select,"end"))
         {
            return;
         }
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitEnd);
         this.m_callback.CallbackExit();
      }
      
      private function cbFuncEndTimer() : void
      {
         this.pushKeyDecide();
      }
      
      private function updateLocalBattle() : void
      {
         var _loc1_:int = this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
         if(!_loc1_)
         {
            return;
         }
         var _loc2_:int = this.m_callback.GetUserDataInt(ReceiveType_Time);
         var _loc3_:int = this.m_callback.GetUserDataInt(ReceiveType_Flag2pController);
         if(_loc3_)
         {
            if(this.m_timeline.press2P.currentLabel != "end" && this.m_timeline.press2P.currentLabel != "end_comp")
            {
               this.m_timeline.press2P.gotoAndPlay("end");
               if(_loc2_ >= 0)
               {
                  if(this.m_current_player_index != 0)
                  {
                     if(this.m_timer)
                     {
                        this.m_timer.Start(_loc2_ + 1,this.cbFuncEndTimer);
                        this.m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
                     }
                  }
               }
            }
         }
         else if(this.m_timeline.press2P.currentLabel != "start" && this.m_timeline.press2P.currentLabel != "wait")
         {
            this.m_timeline.press2P.gotoAndPlay("start");
            this.m_timeline.press2P.nest.sys.sys.htmlText = this.m_callback.GetUserDataString(ReceiveType_Str2pController);
         }
         switch(this.m_current_player_index)
         {
            case 0:
               if(!this.m_timeline.cha_parameter.icon_1P.visible)
               {
                  this.m_timeline.cha_parameter.icon_1P.gotoAndPlay("start");
               }
               this.m_timeline.cha_parameter.icon_1P.visible = true;
               this.m_timeline.cha_parameter.icon_2P.visible = false;
               break;
            case 3:
               this.m_timeline.cha_parameter.icon_1P.visible = false;
               if(!this.m_timeline.cha_parameter.icon_2P.visible)
               {
                  this.m_timeline.cha_parameter.icon_2P.gotoAndPlay("start");
                  if(_loc2_ >= 0)
                  {
                     if(this.m_timer)
                     {
                        if(_loc3_)
                        {
                           this.m_timer.Start(_loc2_ + 1,this.cbFuncEndTimer);
                           this.m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
                        }
                        else
                        {
                           this.m_timer.Stop();
                        }
                     }
                  }
               }
               this.m_timeline.cha_parameter.icon_2P.visible = true;
               break;
            default:
               this.m_timeline.cha_parameter.icon_1P.visible = false;
               this.m_timeline.cha_parameter.icon_2P.visible = false;
         }
      }
      
      private function getCharaInfo(param1:int) : Array
      {
         if(param1 < 0)
         {
            return null;
         }
         if(this.m_chara_list_num <= param1)
         {
            return null;
         }
         if(this.m_chara_list[param1][0] < 0)
         {
            return null;
         }
         return this.m_chara_list[param1];
      }
      
      public function GetSelectVarInfo(param1:int) : Array
      {
         if(param1 < 0 || PlayerMax <= param1)
         {
            return null;
         }
         var _loc2_:int = this.m_select_info[param1][SelectInfoTypeListIndex];
         var _loc3_:Array = this.getCharaInfo(_loc2_);
         if(!_loc3_)
         {
            return null;
         }
         var _loc4_:int = this.m_select_info[param1][SelectInfoTypeVarIndex];
         _loc4_ = _loc4_ + _loc3_.length;
         _loc4_ = _loc4_ % _loc3_.length;
         return _loc3_[_loc4_];
      }
      
      private function getReadyIconMc(param1:int) : MovieClip
      {
         var _loc2_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc3_:int = 0;
         if(_loc2_ < param1)
         {
            _loc3_ = param1;
         }
         else
         {
            _loc3_ = _loc2_ - param1;
         }
         var _loc4_:MovieClip = this.m_timeline.cha_parameter.nest_ready["btnact_rb0" + (_loc3_ + 1)];
         return _loc4_;
      }
      
      private function calcCharaListIndex(param1:int, param2:int) : int
      {
         return param1 + param2 * IndexNumRow;
      }
      
      private function calcIconIndexRow(param1:int) : int
      {
         if(param1 < 0 || this.m_chara_list_num <= param1)
         {
            return -1;
         }
         return param1 % IndexNumRow;
      }
      
      private function calcIconIndexColumn(param1:int) : int
      {
         if(param1 < 0 || this.m_chara_list_num <= param1)
         {
            return -1;
         }
         return param1 / IndexNumRow;
      }
      
      private function getVarIndexNum(param1:int) : int
      {
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         _loc2_ = 0;
         while(CharaVarIndexNum > _loc2_)
         {
            if(this.checkUnlockVar(param1,_loc2_))
            {
               _loc3_++;
            }
            _loc2_++;
         }
         return _loc3_;
      }
      
      private function updatePage() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:MovieClip = null;
         var _loc4_:Boolean = false;
         var _loc5_:Boolean = false;
         if(this.m_select_column < this.m_select_column_start)
         {
            this.m_select_column_start = this.m_select_column;
            _loc4_ = true;
            _loc5_ = true;
         }
         else if(this.m_select_column_start + IndexNumColumn <= this.m_select_column)
         {
            this.m_select_column_start = this.m_select_column - IndexNumColumn + 1;
            _loc4_ = true;
         }
         if(!_loc4_)
         {
            return;
         }
         var _loc6_:MovieClip = this.m_timeline.cha_select;
         _loc2_ = -1;
         while(IndexNumColumn + 1 > _loc2_)
         {
            _loc3_ = _loc6_["chara_icn_set0" + (_loc2_ + 1)];
            if(_loc5_)
            {
               _loc3_.gotoAndPlay("push_l");
               this.m_timeline.cha_arrow.spinbtn_l.cmn_CMN_M_B_mc_spinbtn_l.gotoAndPlay("push");
            }
            else
            {
               _loc3_.gotoAndPlay("push_r");
               this.m_timeline.cha_arrow.spinbtn_r.cmn_CMN_M_B_mc_spinbtn_r.gotoAndPlay("push");
            }
            _loc2_++;
         }
      }
      
      private function changeLR(param1:Boolean) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Boolean = false;
         var _loc4_:int = 0;
         while(true)
         {
            if(param1)
            {
               this.m_select_column--;
               if(this.m_select_column < 0)
               {
                  this.m_select_column = this.m_chara_num_column - 1;
                  this.m_select_row--;
                  this.m_select_row = this.m_select_row + IndexNumRow;
                  this.m_select_row = this.m_select_row % IndexNumRow;
               }
            }
            else
            {
               this.m_select_column++;
               if(this.m_chara_num_column <= this.m_select_column)
               {
                  this.m_select_column = 0;
                  this.m_select_row++;
                  this.m_select_row = this.m_select_row + IndexNumRow;
                  this.m_select_row = this.m_select_row % IndexNumRow;
               }
            }
            _loc2_ = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
            if(this.m_current_player_index != PlayerIndexOwn)
            {
               _loc4_ = this.m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(!_loc4_)
               {
                  if(this.checkAvatar(_loc2_))
                  {
                     continue;
                  }
               }
            }
            _loc3_ = this.checkUnlockChara(_loc2_);
            if(!_loc3_)
            {
               continue;
            }
            break;
         }
         this.m_select_var = 0;
      }
      
      private function changeUD(param1:Boolean) : void
      {
         var _loc2_:int = 0;
         var _loc3_:Boolean = false;
         var _loc4_:int = 0;
         while(true)
         {
            if(param1)
            {
               this.m_select_row--;
               if(this.m_select_row < 0)
               {
                  this.m_select_row = IndexNumRow - 1;
                  this.m_select_column--;
                  this.m_select_column = this.m_select_column + this.m_chara_num_column;
                  this.m_select_column = this.m_select_column % this.m_chara_num_column;
               }
            }
            else
            {
               this.m_select_row++;
               if(IndexNumRow <= this.m_select_row)
               {
                  this.m_select_row = 0;
                  this.m_select_column++;
                  this.m_select_column = this.m_select_column + this.m_chara_num_column;
                  this.m_select_column = this.m_select_column % this.m_chara_num_column;
               }
            }
            _loc2_ = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
            if(this.m_current_player_index != PlayerIndexOwn)
            {
               _loc4_ = this.m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(!_loc4_)
               {
                  if(this.checkAvatar(_loc2_))
                  {
                     continue;
                  }
               }
            }
            _loc3_ = this.checkUnlockChara(_loc2_);
            if(!_loc3_)
            {
               continue;
            }
            break;
         }
         this.m_select_var = 0;
      }
      
      private function changeVar(param1:Boolean) : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:int = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
         var _loc4_:Array = this.getCharaInfo(_loc3_);
         if(!_loc4_)
         {
            return;
         }
         while(true)
         {
            if(param1)
            {
               this.m_select_var--;
            }
            else
            {
               this.m_select_var++;
            }
            this.m_select_var = this.m_select_var + _loc4_.length;
            this.m_select_var = this.m_select_var % _loc4_.length;
            _loc2_ = this.checkUnlockVar(_loc3_,this.m_select_var);
            if(!_loc2_)
            {
               continue;
            }
            break;
         }
      }
      
      private function checkPlayerTeamType(param1:int) : int
      {
         var _loc2_:int = PlayerTeamTypeInvalid;
         if(param1 == PlayerIndexOwn)
         {
            _loc2_ = PlayerTeamTypeOwn;
         }
         else if(PlayerIndexFriStart <= param1 && param1 <= PlayerIndexFriEnd)
         {
            _loc2_ = PlayerTeamTypeFri;
         }
         else if(PlayerIndexEnmStart <= param1 && param1 <= PlayerIndexEnmEnd)
         {
            _loc2_ = PlayerTeamTypeEnm;
         }
         return _loc2_;
      }
      
      private function setReadyIcon(param1:int, param2:Boolean, param3:Boolean) : void
      {
         var _loc4_:int = 0;
         var _loc5_:Boolean = false;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         if(param1 < 0 || PlayerMax <= param1)
         {
            return;
         }
         var _loc10_:MovieClip = this.getReadyIconMc(param1);
         if(param3)
         {
            _loc4_ = this.checkPlayerTeamType(param1);
            _loc5_ = false;
            _loc6_ = param1 - 1;
            switch(_loc4_)
            {
               case PlayerTeamTypeOwn:
                  _loc10_.gotoAndStop("blue_team");
                  break;
               case PlayerTeamTypeFri:
                  _loc10_.gotoAndStop("blue_team");
                  _loc7_ = this.m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
                  _loc8_ = this.m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
                  if(0 <= _loc6_ && _loc6_ < _loc8_)
                  {
                     _loc5_ = true;
                  }
                  break;
               case PlayerTeamTypeEnm:
                  _loc10_.gotoAndStop("red_team");
            }
            _loc10_.sys_ready.text = "OK";
            if(_loc5_)
            {
               this.setImageFriendNpc(_loc10_.icn_chara_lit.chara_img,_loc6_);
            }
            else
            {
               _loc9_ = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
               this.setImage(_loc10_.icn_chara_lit.chara_img,_loc9_,true);
            }
         }
         else
         {
            _loc10_.gotoAndStop("off");
            _loc10_.sys_ready.text = "---";
         }
         if(param2)
         {
            _loc10_.btnact_ready.gotoAndPlay("on");
         }
         else
         {
            _loc10_.btnact_ready.gotoAndPlay("off");
         }
      }
      
      private function changeCurrentPlayer(param1:Boolean) : void
      {
         var _loc2_:Boolean = false;
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         this.setReadyIcon(this.m_current_player_index,false,param1);
         var _loc9_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc10_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         do
         {
            if(param1)
            {
               this.m_current_player_index++;
            }
            else
            {
               this.m_current_player_index--;
            }
            _loc2_ = false;
            _loc3_ = this.checkPlayerTeamType(this.m_current_player_index);
            switch(_loc3_)
            {
               case PlayerTeamTypeFri:
                  _loc4_ = this.m_current_player_index - PlayerIndexFriStart;
                  _loc5_ = this.m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
                  if(_loc5_ > _loc4_)
                  {
                     _loc2_ = false;
                  }
                  else if(_loc9_ > _loc4_)
                  {
                     _loc2_ = true;
                  }
                  break;
               case PlayerTeamTypeEnm:
                  _loc6_ = this.m_current_player_index - PlayerIndexEnmStart;
                  if(_loc10_ > _loc6_)
                  {
                     _loc2_ = true;
                  }
                  break;
               case PlayerTeamTypeOwn:
               default:
                  _loc2_ = true;
            }
         }
         while(!_loc2_);
         
         this.setReadyIcon(this.m_current_player_index,true,false);
         if(0 <= this.m_current_player_index && this.m_current_player_index < PlayerMax)
         {
            _loc7_ = this.m_select_info[this.m_current_player_index][SelectInfoTypeListIndex];
            this.m_select_row = this.calcIconIndexRow(_loc7_);
            this.m_select_column = this.calcIconIndexColumn(_loc7_);
            this.m_select_var = this.m_select_info[this.m_current_player_index][SelectInfoTypeVarIndex];
         }
      }
      
      private function setSelectChara() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         var _loc4_:MovieClip = null;
         var _loc5_:Boolean = false;
         if(this.m_current_player_index < 0 || PlayerMax <= this.m_current_player_index)
         {
            return;
         }
         var _loc6_:int = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
         var _loc7_:Array = this.getCharaInfo(_loc6_);
         var _loc8_:int = -1;
         if(_loc7_)
         {
            _loc8_ = _loc7_.length;
            _loc1_ = 0;
            while(_loc8_ > _loc1_)
            {
               _loc3_ = this.m_select_var + _loc1_;
               _loc3_ = _loc3_ % _loc8_;
               if(!this.checkUnlockVar(_loc6_,_loc3_))
               {
                  _loc1_++;
                  continue;
               }
               this.m_select_var = _loc3_;
               break;
            }
         }
         var _loc9_:int = this.m_select_row;
         var _loc10_:int = this.m_select_column - this.m_select_column_start;
         var _loc11_:MovieClip = this.m_timeline.cha_select;
         var _loc12_:MovieClip = _loc11_["chara_icn_set0" + (_loc10_ + 1)];
         var _loc13_:MovieClip = _loc12_["nest_charaselect0" + (_loc9_ + 1)];
         this.m_select_icon_row = _loc9_;
         this.m_select_icon_column = _loc10_;
         _loc1_ = 0;
         while(CharaVarIndexNum > _loc1_)
         {
            _loc4_ = this.getMcChamyset(_loc1_);
            if(_loc4_)
            {
               _loc5_ = this.checkUnlockVar(_loc6_,_loc1_);
               _loc4_.btnact_off.visible = true;
               if(_loc1_ < _loc8_)
               {
                  if(_loc5_)
                  {
                     if(_loc1_ == this.m_select_var)
                     {
                        if(_loc4_.currentLabel != "on")
                        {
                           _loc4_.gotoAndPlay("on");
                           _loc4_.btnact_ef.visible = true;
                           _loc4_.btnact_on.visible = true;
                           _loc4_.btnact_off.visible = false;
                        }
                     }
                     else
                     {
                        _loc4_.gotoAndPlay("off");
                        _loc4_.btnact_ef.visible = false;
                        _loc4_.btnact_on.visible = false;
                        _loc4_.btnact_off.visible = true;
                     }
                  }
                  else
                  {
                     _loc4_.gotoAndPlay("lock");
                     _loc4_.btnact_ef.visible = false;
                     _loc4_.btnact_on.visible = false;
                     _loc4_.btnact_off.visible = true;
                  }
               }
               else
               {
                  _loc4_.gotoAndPlay("off");
                  _loc4_.btnact_ef.visible = false;
                  _loc4_.btnact_on.visible = false;
                  _loc4_.btnact_off.visible = false;
               }
            }
            _loc1_++;
         }
         var _loc14_:int = this.getVarIndexNum(_loc6_);
         var _loc15_:* = false;
         if(_loc14_ > 1)
         {
            _loc15_ = true;
         }
         this.m_timeline.cha_parameter.nest_clothes.sys_r.visible = _loc15_;
         this.m_timeline.cha_parameter.nest_clothes.sys_l.visible = _loc15_;
         this.m_select_info[this.m_current_player_index][SelectInfoTypeListIndex] = _loc6_;
         this.m_select_info[this.m_current_player_index][SelectInfoTypeVarIndex] = this.m_select_var;
      }
      
      public function sendCharaInfo(param1:int) : void
      {
         if(param1 < 0)
         {
            return;
         }
         if(PlayerMax <= param1)
         {
            return;
         }
         var _loc2_:Array = this.GetSelectVarInfo(param1);
         if(!_loc2_)
         {
            this.SetUserDataString(CharaSele.ReceiveType_CharaNameStr,"???");
            return;
         }
         var _loc3_:String = _loc2_[CharaList.VarTypeCode];
         var _loc4_:int = this.m_select_info[param1][SelectInfoTypeVarIndex];
         var _loc5_:int = _loc2_[CharaList.VarTypeMid];
         var _loc6_:int = _loc2_[CharaList.VarTypeModelPreset];
         this.m_callback.CallbackUserData("user",SendType_CurrentPlayerIndex,param1);
         this.m_callback.CallbackUserDataString("user",SendType_SelectCode,_loc3_);
         this.m_callback.CallbackUserData("user",SendType_SelectVariation,_loc4_);
         this.m_callback.CallbackUserData("user",SendType_SelectMid,_loc5_);
         this.m_callback.CallbackUserData("user",SendType_SelectModelPreset,_loc6_);
         this.m_callback.CallbackUserData("user",SendType_RequestCharaInfo,0);
         this.m_callback.SetUserDataValidFlag(ReceiveType_CharaNameStr,false);
      }
      
      private function setImage(param1:MovieClip, param2:int, param3:Boolean) : void
      {
         var _loc4_:Bitmap = null;
         var _loc5_:String = null;
         if(!param1)
         {
            return;
         }
         while(param1.numChildren > 0)
         {
            param1.removeChildAt(0);
         }
         if(!this.checkUnlockChara(param2))
         {
            return;
         }
         if(this.m_chara_face[param2])
         {
            _loc4_ = this.m_chara_face[param2];
            if(param3)
            {
               _loc5_ = this.m_callback.GetUserDataString(ReceiveType_ImageStrStart + param2);
               _loc4_ = new Bitmap(null);
               IggyFunctions.setTextureForBitmap(_loc4_,_loc5_);
               _loc4_.scaleX = 256 / _loc4_.width;
               _loc4_.scaleY = 128 / _loc4_.height;
            }
            param1.addChild(_loc4_);
         }
      }
      
      private function setImageFriendNpc(param1:MovieClip, param2:int) : void
      {
         var _loc3_:Bitmap = null;
         if(!param1)
         {
            return;
         }
         if(param2 < 0 || PlayerNumFri <= param2)
         {
            return;
         }
         while(param1.numChildren > 0)
         {
            param1.removeChildAt(0);
         }
         if(this.m_chara_face_npc[param2])
         {
            _loc3_ = this.m_chara_face_npc[param2];
            param1.addChild(_loc3_);
         }
      }
      
      private function updateCharaIcon() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:MovieClip = null;
         var _loc4_:MovieClip = null;
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:MovieClip = null;
         var _loc8_:MovieClip = this.m_timeline.cha_select;
         _loc2_ = -1;
         while(IndexNumColumn + 1 > _loc2_)
         {
            _loc3_ = _loc8_["chara_icn_set0" + (_loc2_ + 1)];
            _loc1_ = 0;
            while(IndexNumRow > _loc1_)
            {
               _loc4_ = _loc3_["nest_charaselect0" + (_loc1_ + 1)];
               if(_loc4_)
               {
                  _loc5_ = this.calcCharaListIndex(_loc1_,_loc2_ + this.m_select_column_start);
                  this.setImage(_loc4_.chara_img,_loc5_,false);
                  if(this.checkNoChara(_loc5_))
                  {
                     _loc4_.visible = false;
                  }
                  else if(this.checkUnlockChara(_loc5_))
                  {
                     _loc4_.visible = true;
                     _loc6_ = this.m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
                     if(!_loc6_ && this.m_current_player_index != PlayerIndexOwn && this.checkAvatar(_loc5_))
                     {
                        _loc4_.gotoAndPlay("grayout");
                     }
                     else if(this.m_select_icon_column == _loc2_ && this.m_select_icon_row == _loc1_)
                     {
                        _loc7_ = this.m_timeline.cha_select_cur;
                        _loc7_.visible = true;
                        _loc7_.x = _loc8_.x + _loc3_.x + _loc4_.x;
                        _loc7_.y = _loc8_.y + _loc3_.y + _loc4_.y;
                        this.setImage(_loc7_.chara_sel.chara_img,_loc5_,false);
                        if(this.m_flag_decide)
                        {
                           if(_loc7_.currentLabel != "push")
                           {
                              _loc7_.gotoAndPlay("push");
                           }
                        }
                        else if(_loc7_.currentLabel != "on")
                        {
                           _loc7_.gotoAndPlay("on");
                        }
                     }
                     else
                     {
                        _loc4_.gotoAndPlay("off");
                     }
                  }
                  else
                  {
                     _loc4_.visible = true;
                     _loc4_.gotoAndPlay("lock");
                  }
               }
               _loc1_++;
            }
            _loc2_++;
         }
         var _loc9_:MovieClip = _loc8_["chara_icn_set00"];
         if(this.m_select_column_start <= 0)
         {
            _loc9_.visible = false;
         }
         else
         {
            _loc9_.visible = true;
         }
         var _loc10_:MovieClip = _loc8_["chara_icn_set0" + (IndexNumColumn + 1)];
         if(this.m_chara_num_column <= this.m_select_column_start + IndexNumColumn)
         {
            _loc10_.visible = false;
         }
         else
         {
            _loc10_.visible = true;
         }
      }
      
      private function updateSkill() : void
      {
         var _loc1_:String = null;
         var _loc2_:Number = NaN;
         var _loc3_:Number = NaN;
         var _loc4_:String = null;
         var _loc5_:Number = NaN;
         var _loc6_:Number = NaN;
         var _loc7_:int = 0;
         this.ResetIcons();
         var _loc8_:MovieClip = this.m_timeline.cha_skill;
         if(this.m_flag_skill)
         {
            if(_loc8_.currentFrame > Utility.GetLabelEndFrame(_loc8_,"wait"))
            {
               _loc8_.gotoAndPlay("start");
               _loc8_.visible = true;
            }
            _loc7_ = 0;
            while(SkillMax > _loc7_)
            {
               _loc4_ = this.m_callback.GetUserDataString(ReceiveType_SkillNameStrStart + _loc7_);
               _loc8_.inact_skill["skill0" + (_loc7_ + 1)].sys_skill.scaleX = this.m_skill_str_scalex_default;
               _loc8_.inact_skill["skill0" + (_loc7_ + 1)].sys_skill.autoSize = "left";
               _loc8_.inact_skill["skill0" + (_loc7_ + 1)].sys_skill.htmlText = _loc4_;
               _loc5_ = _loc8_.inact_skill["skill0" + (_loc7_ + 1)].sys_skill.width;
               _loc6_ = this.m_skill_str_scalex_default;
               if(this.m_skill_str_width_default < _loc5_)
               {
                  _loc6_ = this.m_skill_str_width_default / _loc5_;
               }
               _loc8_.inact_skill["skill0" + (_loc7_ + 1)].sys_skill.scaleX = _loc6_;
               _loc7_++;
            }
            _loc1_ = this.m_callback.GetUserDataString(ReceiveType_TarismanNameStr);
            _loc8_.inact_skill.skill08.sys_skill.scaleX = this.m_skill_str_scalex_default;
            _loc8_.inact_skill.skill08.sys_skill.autoSize = "left";
            _loc8_.inact_skill.skill08.sys_skill.htmlText = _loc1_;
            _loc2_ = _loc8_.inact_skill.skill08.sys_skill.width;
            _loc3_ = this.m_skill_str_scalex_default;
            if(this.m_skill_str_width_default < _loc2_)
            {
               _loc3_ = this.m_skill_str_width_default / _loc2_;
            }
            _loc8_.inact_skill.skill08.sys_skill.scaleX = _loc3_;
         }
         else if(_loc8_.currentFrame <= Utility.GetLabelEndFrame(_loc8_,"wait"))
         {
            _loc8_.gotoAndPlay("end");
         }
      }
      
      private function getMcChamyset(param1:int) : MovieClip
      {
         var _loc2_:MovieClip = null;
         if(param1 < 9)
         {
            _loc2_ = this.m_timeline.cha_parameter.nest_clothes["btnact_chamyset_0" + (param1 + 1)];
         }
         else
         {
            _loc2_ = this.m_timeline.cha_parameter.nest_clothes["btnact_chamyset_" + (param1 + 1)];
         }
         return _loc2_;
      }
      
      private function checkUnlockVar(param1:int, param2:int) : Boolean
      {
         if(param1 < 0 || this.m_chara_list_num <= param1)
         {
            return true;
         }
         if(param2 < 0 || CharaVarIndexNum <= param2)
         {
            return true;
         }
         var _loc3_:int = param1 * CharaVarIndexNum + param2;
         return true;
      }
      
      private function checkUnlockChara(param1:*) : Boolean
      {
         if(param1 < 0 || this.m_chara_list_num <= param1)
         {
            return true;
         }
         var _loc2_:int = 0;
         _loc2_ = 0;
         while(CharaVarIndexNum > _loc2_)
         {
            if(this.checkUnlockVar(param1,_loc2_))
            {
               return true;
            }
            _loc2_++;
         }
         return true;
      }
      
      private function checkAvatar(param1:int) : Boolean
      {
         var _loc2_:Array = this.getCharaInfo(param1);
         if(!_loc2_)
         {
            return false;
         }
         if(_loc2_.length <= 0)
         {
            return false;
         }
         var _loc3_:String = _loc2_[0][CharaList.VarTypeCode];
         if(_loc3_ != CharaList.AvatarCode)
         {
            return false;
         }
         return true;
      }
      
      private function checkNoChara(param1:int) : Boolean
      {
         var _loc2_:Array = this.getCharaInfo(param1);
         if(!_loc2_)
         {
            return true;
         }
         if(_loc2_.length <= 0)
         {
            return true;
         }
         var _loc3_:String = _loc2_[0][CharaList.VarTypeCode];
         if(_loc3_ == CharaList.InvalidCode)
         {
            return true;
         }
         return false;
      }
      
      private function pushLeft() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeLR(true);
         this.updatePage();
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushRight() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeLR(false);
         this.updatePage();
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushUp() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeUD(true);
         this.updatePage();
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushDown() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeUD(false);
         this.updatePage();
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushKeyL() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeVar(true);
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushKeyR() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.changeVar(false);
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushKeySkill() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         if(this.m_flag_skill)
         {
            this.m_flag_skill = false;
            this.m_callback.CallbackUserData("user",SendType_RequestSetFlagSkill,0);
         }
         else
         {
            this.m_flag_skill = true;
            this.m_callback.CallbackUserData("user",SendType_RequestSetFlagSkill,1);
         }
      }
      
      private function pushKeyRandom() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         _loc1_ = 0;
         while(this.m_chara_list_num > _loc1_)
         {
            if(this.checkUnlockChara(_loc1_))
            {
               _loc2_++;
            }
            _loc1_++;
         }
         if(this.m_current_player_index != PlayerIndexOwn)
         {
            _loc2_--;
         }
         var _loc3_:int = 0;
         if(_loc2_ > 1)
         {
            _loc3_ = Math.floor(Math.random() * _loc2_);
         }
         _loc1_ = 0;
         while(_loc3_ > _loc1_)
         {
            this.changeUD(false);
            this.updatePage();
            _loc1_++;
         }
         var _loc4_:int = this.calcCharaListIndex(this.m_select_row,this.m_select_column);
         var _loc5_:int = this.getVarIndexNum(_loc4_);
         var _loc6_:int = 0;
         if(_loc5_ > 1)
         {
            _loc6_ = Math.floor(Math.random() * _loc5_);
         }
         _loc1_ = 0;
         while(_loc6_ > _loc1_)
         {
            this.changeVar(false);
            _loc1_++;
         }
         this.setSelectChara();
         this.sendCharaInfo(this.m_current_player_index);
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function pushKeyDecide() : void
      {
         if(this.m_current_player_index != PlayerIndexOwn && this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !this.m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         this.m_flag_change_player = true;
         this.m_flag_decide = true;
         this.m_callback.CallbackSe(this.m_callback.SeTypeDecide);
      }
      
      private function pushKeyCancel() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = this.m_callback.GetUserDataInt(ReceiveType_FlagUseCancel);
         if(!_loc2_)
         {
            _loc1_ = this.m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
            if(_loc1_)
            {
               return;
            }
            if(this.m_current_player_index <= PlayerIndexOwn)
            {
               return;
            }
         }
         this.m_flag_change_player = true;
         this.m_flag_decide = false;
         this.m_callback.CallbackSe(this.m_callback.SeTypeCancel);
      }
      
      private function pushStart() : void
      {
         var _loc1_:int = this.m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         if(_loc1_ > 0)
         {
            return;
         }
         if(this.m_current_player_index == PlayerIndexOwn)
         {
            return;
         }
         this.m_flag_change_player = true;
         this.m_flag_decide = false;
         this.m_flag_exit = true;
         this.m_callback.CallbackSe(this.m_callback.SeTypeCarsol);
      }
      
      private function checkKey(param1:KeyboardEvent) : void
      {
         if(this.m_flag_decide)
         {
            return;
         }
         switch(param1.keyCode)
         {
            case Keyboard.ENTER:
               this.pushKeyDecide();
               break;
            case Keyboard.ESCAPE:
               this.pushKeyCancel();
               break;
            case Keyboard.LEFT:
               this.pushLeft();
               break;
            case Keyboard.RIGHT:
               this.pushRight();
               break;
            case Keyboard.UP:
               this.pushUp();
               break;
            case Keyboard.DOWN:
               this.pushDown();
               break;
            case Keyboard.DELETE:
               this.pushKeyL();
               break;
            case Keyboard.PAGE_DOWN:
               this.pushKeyR();
               break;
            case 88:
               this.pushKeySkill();
               break;
            case 89:
               this.pushKeyRandom();
               break;
            case Keyboard.SPACE:
               this.pushStart();
         }
      }
      
      public function SetUserDataInt(param1:int, param2:int) : *
      {
         this.m_callback.AddCallbackSetUserDataInt(param1,param2);
      }
      
      public function SetUserDataString(param1:int, param2:String) : *
      {
         this.m_callback.AddCallbackSetUserDataString(param1,param2);
      }
      
      public function TestDestroy() : void
      {
         this.m_callback = null;
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.requestUnlock);
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitUnlock);
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitStart);
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitMain);
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.main);
         this.m_timeline.stage.removeEventListener(Event.ENTER_FRAME,this.waitEnd);
         this.m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,this.checkKey);
         this.m_timeline.visible = false;
         this.m_timeline = null;
         this.m_timer.Destroy();
         this.m_timer = null;
      }
      
      public function TestCheckChangeSelect() : Boolean
      {
         return !this.m_callback.GetUserDataValidFlag(ReceiveType_CharaNameStr);
      }
      
      public function TestGetCharaList() : Array
      {
         return this.m_chara_list;
      }
      
      public function TestGetCharaVarInfo() : Array
      {
         return this.GetSelectVarInfo(this.m_current_player_index);
      }
      
      public function TestGetCurrentPlayerIndex() : int
      {
         return this.m_current_player_index;
      }
   }
}
