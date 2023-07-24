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
      
      public static const CharacterMax:int = 64;
      
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
         m_callback = new Callback(ReceiveType_Num);
         m_timeline = null;
         m_timer = new CountDownTimer();
         m_current_player_index = 0;
         m_select_info = new Array(PlayerMax);
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         _loc1_ = 0;
         while(m_select_info.length > _loc1_)
         {
            m_select_info[_loc1_] = new Array(SelectInfoTypeNum);
            m_select_info[_loc1_][SelectInfoTypeListIndex] = 0;
            m_select_info[_loc1_][SelectInfoTypeVarIndex] = 0;
            _loc1_++;
         }
         m_select_row = 0;
         m_select_column = 0;
         m_select_column_start = 0;
         m_select_icon_row = -1;
         m_select_icon_column = -1;
         m_select_var = 0;
         m_flag_skill = false;
         m_flag_change_player = false;
         m_flag_decide = false;
         m_flag_exit = false;
         m_skill_str_width_default = 0;
         m_skill_str_scalex_default = 1;
         m_chara_list = null;
         m_chara_list_num = 0;
         m_chara_num_column = 0;
         m_dlc3_chara_name = [CharaList.InvalidCode,CharaList.InvalidCode,CharaList.InvalidCode];
         try
         {
            ExternalInterface.addCallback("ForcingCancel",pushKeyCancel);
            return;
         }
         catch(e:Error)
         {
            return;
         }
      }
      
      public function Initialize(param1:MovieClip) : void
      {
         m_timeline = param1;
         m_timeline.visible = false;
         m_timeline.cha_frame.visible = false;
         m_timeline.cha_skill.visible = false;
         m_timeline.cha_select.visible = false;
         m_timeline.cha_parameter.visible = false;
         m_timeline.cha_arrow.visible = false;
         m_timeline.cha_select_cur.visible = false;
         m_timeline.press2P.visible = false;
         m_timer.Initialize(m_timeline.timer.nest._CMN_M_B_mc_timer,null);
         m_current_player_index = 0;
         m_select_row = 0;
         m_select_column = 0;
         m_select_column_start = 0;
         m_select_var = 0;
         m_select_icon_row = -1;
         m_select_icon_column = -1;
         m_flag_skill = false;
         m_flag_change_player = false;
         m_flag_decide = false;
         m_skill_str_width_default = m_timeline.cha_skill.inact_skill.skill01.sys_skill.width;
         m_skill_str_scalex_default = m_timeline.cha_skill.inact_skill.skill01.sys_skill.scaleX;
         m_chara_list = null;
         m_chara_list_num = 0;
         m_chara_num_column = 0;
         SetDlc3Name("GGK","GVG","GFR");
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,waitDlcInfo);
      }
      
      private function waitDlcInfo(param1:Event) : void
      {
         var _loc2_:int = 0;
         if(!m_callback)
         {
            return;
         }
         if(!m_callback.GetUserDataValidFlag(ReceiveType_FlagDlc1))
         {
            return;
         }
         if(!m_callback.GetUserDataValidFlag(ReceiveType_FlagDlc2))
         {
            return;
         }
         if(!m_callback.GetUserDataValidFlag(ReceiveType_FlagDlc3))
         {
            return;
         }
         if(!m_callback.GetUserDataValidFlag(ReceiveType_FlagFirstJa))
         {
            return;
         }
         if(!m_callback.GetUserDataValidFlag(ReceiveType_FlagFirstOther))
         {
            return;
         }
         var _loc3_:int = m_callback.GetUserDataInt(ReceiveType_FlagDlc1);
         var _loc4_:int = m_callback.GetUserDataInt(ReceiveType_FlagDlc2);
         var _loc5_:int = m_callback.GetUserDataInt(ReceiveType_FlagDlc3);
         var _loc6_:int = m_callback.GetUserDataInt(ReceiveType_FlagFirstJa);
         var _loc7_:int = m_callback.GetUserDataInt(ReceiveType_FlagFirstOther);
         m_chara_list = CharaList.GetList(_loc3_); //Fixed to use only one parameter
         _loc2_ = 0;
         while(m_chara_list.length > _loc2_)
         {
            if(m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara0)
            {
               m_chara_list[_loc2_][0][0] = m_dlc3_chara_name[0];
            }
            else if(m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara1)
            {
               m_chara_list[_loc2_][0][0] = m_dlc3_chara_name[1];
            }
            else if(m_chara_list[_loc2_][0][0] == CharaList.Dlc3CodeChara2)
            {
               m_chara_list[_loc2_][0][0] = m_dlc3_chara_name[2];
            }
            _loc2_++;
         }
         m_chara_list_num = m_chara_list.length;
         m_chara_num_column = (m_chara_list_num - 1) / IndexNumRow + 1;
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitDlcInfo);
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,requestUnlock);
      }
      
      public function SetDlc3Name(param1:String, param2:String, param3:String) : void
      {
         m_dlc3_chara_name[0] = param1;
         m_dlc3_chara_name[1] = param2;
         m_dlc3_chara_name[2] = param3;
      }
      
      private function requestUnlock(param1:Event) : void
      {
         var _loc4_:Boolean = false;
         var _loc5_:Array = null;
         var _loc6_:Boolean = false;
         var _loc7_:int = 0;
         var _loc8_:Array = null;
         var _loc9_:String = null;
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         _loc2_ = 0;
         while(CharacterMax > _loc2_)
         {
            _loc4_ = false;
            _loc5_ = getCharaInfo(_loc2_);
            if(!_loc5_)
            {
               _loc3_ = 0;
               while(CharaVarIndexNum > _loc3_)
               {
                  SetUserDataInt(ReceiveType_UnlockVarStart + _loc2_ * CharaVarIndexNum + _loc3_,0);
                  _loc3_++;
               }
               SetUserDataString(ReceiveType_ImageStrStart + _loc2_,"");
            }
            else if(_loc5_.length <= _loc3_)
            {
               _loc3_ = 0;
               while(CharaVarIndexNum > _loc3_)
               {
                  SetUserDataInt(ReceiveType_UnlockVarStart + _loc2_ * CharaVarIndexNum + _loc3_,0);
                  _loc3_++;
               }
               SetUserDataString(ReceiveType_ImageStrStart + _loc2_,"");
            }
            else
            {
               _loc6_ = false;
               _loc3_ = CharaVarIndexNum - 1;
               while(0 <= _loc3_)
               {
                  _loc7_ = _loc2_ * CharaVarIndexNum + _loc3_;
                  if(_loc5_.length <= _loc3_)
                  {
                     SetUserDataInt(ReceiveType_UnlockVarStart + _loc7_,0);
                  }
                  else
                  {
                     _loc8_ = _loc5_[_loc3_];
                     _loc9_ = _loc8_[CharaList.VarTypeCode];
                     if(_loc9_ == CharaList.InvalidCode)
                     {
                        SetUserDataInt(ReceiveType_UnlockVarStart + _loc7_,0);
                     }
                     else
                     {
                        _loc10_ = _loc8_[CharaList.VarTypeMid];
                        _loc4_ = true;
                        if(!m_callback.GetUserDataValidFlag(ReceiveType_UnlockVarStart + _loc7_))
                        {
                           _loc11_ = _loc8_[CharaList.VarTypeUnlockIndex];
                           _loc12_ = _loc3_;
                           m_callback.CallbackUserDataString("user",SendType_SelectCode,_loc9_);
                           m_callback.CallbackUserData("user",SendType_SelectUnlockIndex,_loc11_);
                           m_callback.CallbackUserData("user",SendType_SelectVariation,_loc12_);
                           m_callback.CallbackUserData("user",SendType_SelectMid,_loc10_);
                           m_callback.CallbackUserData("user",SendType_RequestUnlock,_loc7_);
                           m_callback.CallbackUserData("user",SendType_RequestImageStr,_loc2_);
                        }
                     }
                  }
                  _loc3_--;
               }
               if(!_loc4_)
               {
                  SetUserDataString(ReceiveType_ImageStrStart + _loc2_,"");
               }
            }
            _loc2_++;
         }
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,requestUnlock);
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,waitUnlock);
      }
      
      private function waitUnlock(param1:Event) : void
      {
         var _loc5_:int = 0;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         _loc3_ = 0;
         while(true)
         {
            if(m_select_info.length <= _loc3_)
            {
               m_current_player_index = PlayerIndexOwn;
               var _loc4_:* = m_select_info[m_current_player_index][SelectInfoTypeListIndex];
               m_select_row = calcIconIndexRow(_loc4_);
               m_select_column = calcIconIndexColumn(_loc4_);
               m_select_var = m_select_info[m_current_player_index][SelectInfoTypeVarIndex];
               sendCharaInfo(m_current_player_index);
               m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitUnlock);
               m_timeline.stage.addEventListener(Event.ENTER_FRAME,waitStart);
               return;
            }
            _loc5_ = -1;
            _loc2_ = 0;
            for(; CharacterMax > _loc2_; _loc2_++)
            {
               _loc6_ = PlayerIndexOwn + _loc2_;
               _loc6_ = _loc6_ % CharacterMax;
               _loc7_ = m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(_loc3_ != PlayerIndexOwn)
               {
                  if(!_loc7_)
                  {
                     if(checkAvatar(_loc6_))
                     {
                        continue;
                     }
                  }
               }
               if(!checkUnlockChara(_loc6_))
               {
                  continue;
               }
               _loc5_ = _loc6_;
               break;
            }
            if(_loc5_ < 0)
            {
               break;
            }
            m_select_row = calcIconIndexRow(_loc5_);
            m_select_column = calcIconIndexColumn(_loc5_);
            m_current_player_index = _loc3_;
            m_select_var = 0;
            setSelectChara();
            _loc3_++;
         }
      }
      
      private function waitStart(param1:Event) : void
      {
         var _loc2_:int = 0;
         _loc2_ = 0;
         while(ReceiveType_Num > _loc2_)
         {
            if(!m_callback.GetUserDataValidFlag(_loc2_))
            {
               return;
            }
            _loc2_++;
         }
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitStart);
         startMain();
      }
      
      private function ResetIcons() : void
      {
         var _loc1_:String = m_callback.GetUserDataString(ReceiveType_KeyStrL2);
         var _loc2_:String = m_callback.GetUserDataString(ReceiveType_KeyStrR2);
         var _loc3_:String = m_callback.GetUserDataString(ReceiveType_KeyStrRU);
         var _loc4_:String = m_callback.GetUserDataString(ReceiveType_KeyStrRD);
         var _loc5_:String = m_callback.GetUserDataString(ReceiveType_KeyStrRL);
         var _loc6_:String = m_callback.GetUserDataString(ReceiveType_KeyStrRR);
         m_timeline.cha_skill.inact_skill.skill01.sys_com01.htmlText = _loc2_ + "+" + _loc3_;
         m_timeline.cha_skill.inact_skill.skill02.sys_com01.htmlText = _loc2_ + "+" + _loc5_;
         m_timeline.cha_skill.inact_skill.skill03.sys_com01.htmlText = _loc2_ + "+" + _loc6_;
         m_timeline.cha_skill.inact_skill.skill04.sys_com01.htmlText = _loc2_ + "+" + _loc4_;
         m_timeline.cha_skill.inact_skill.skill05.sys_com01.htmlText = _loc1_ + "+" + _loc3_;
         m_timeline.cha_skill.inact_skill.skill06.sys_com01.htmlText = _loc1_ + "+" + _loc5_;
         m_timeline.cha_skill.inact_skill.skill07.sys_com01.htmlText = _loc1_ + "+" + _loc4_;
         var _loc7_:String = m_callback.GetUserDataString(ReceiveType_TarismanHeaderStr);
         m_timeline.cha_skill.inact_skill.skill08.sys_com01.htmlText = _loc7_;
         m_timeline.cha_parameter.nest_clothes.sys_clothes.htmlText = "";
         var _loc8_:String = m_callback.GetUserDataString(ReceiveType_KeyStrL1);
         m_timeline.cha_parameter.nest_clothes.sys_l.htmlText = _loc8_;
         var _loc9_:String = m_callback.GetUserDataString(ReceiveType_KeyStrR1);
         m_timeline.cha_parameter.nest_clothes.sys_r.htmlText = _loc9_;
      }
      
      private function startMain() : void
      {
         var _loc8_:int = 0;
         var _loc9_:MovieClip = null;
         var _loc10_:int = 0;
         var _loc11_:MovieClip = null;
         var _loc12_:Array = null;
         var _loc13_:Bitmap = null;
         var _loc14_:String = null;
         var _loc15_:String = null;
         var _loc16_:String = null;
         var _loc17_:Bitmap = null;
         var _loc18_:MovieClip = null;
         var _loc19_:int = 0;
         var _loc20_:MovieClip = null;
         var _loc1_:int = 0;
         m_timeline.visible = true;
         m_timeline.cha_frame.cmn_CMN_M_frame.visible = true;
         m_timeline.cha_skill.visible = false;
         m_timeline.cha_select.visible = true;
         m_timeline.cha_parameter.visible = true;
         m_timeline.cha_arrow.visible = true;
         var _loc2_:int = m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc3_:int = m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         if(_loc2_ == 0 && _loc3_ == 0)
         {
            m_timeline.cha_parameter.nest_ready.visible = false;
            m_timeline.cha_parameter.ready_base.visible = false;
         }
         else
         {
            m_timeline.cha_parameter.nest_ready.visible = true;
            m_timeline.cha_parameter.ready_base.visible = true;
         }
         var _loc4_:int = m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
         if(_loc4_)
         {
            m_timeline.cha_parameter.icon_1P.play();
            m_timeline.cha_parameter.icon_1P.visible = true;
            m_timeline.cha_parameter.icon_2P.play();
            m_timeline.cha_parameter.icon_2P.visible = false;
            m_timeline.press2P.gotoAndPlay("start");
            m_timeline.press2P.visible = true;
            m_timeline.press2P.nest.sys.sys.htmlText = m_callback.GetUserDataString(ReceiveType_Str2pController);
         }
         else
         {
            m_timeline.cha_parameter.icon_1P.stop();
            m_timeline.cha_parameter.icon_1P.visible = false;
            m_timeline.cha_parameter.icon_2P.stop();
            m_timeline.cha_parameter.icon_2P.visible = false;
            m_timeline.press2P.stop();
            m_timeline.press2P.visible = false;
         }
         var _loc5_:MovieClip = getReadyIconMc(0);
         _loc5_.visible = true;
         _loc1_ = 0;
         while(PlayerNumFri > _loc1_)
         {
            _loc8_ = PlayerIndexFriStart + _loc1_;
            _loc9_ = getReadyIconMc(_loc8_);
            if(_loc1_ < _loc2_)
            {
               _loc9_.visible = true;
            }
            else
            {
               _loc9_.visible = false;
            }
            _loc1_++;
         }
         _loc1_ = 0;
         while(PlayerNumEnm > _loc1_)
         {
            _loc10_ = PlayerIndexEnmStart + _loc1_;
            _loc11_ = getReadyIconMc(_loc10_);
            if(_loc1_ < _loc3_)
            {
               _loc11_.visible = true;
            }
            else
            {
               _loc11_.visible = false;
            }
            _loc1_++;
         }
         m_timeline.cha_parameter.sys_charaName.sys_charaName.htmlText = "";
         if(m_timeline.cha_parameter.sys_skill_header)
         {
            m_timeline.cha_parameter.sys_skill_header.sys_skill_header.htmlText = "";
         }
         setSelectChara();
         m_chara_face = new Array(m_chara_list_num);
         _loc1_ = 0;
         while(m_chara_list_num > _loc1_)
         {
            _loc12_ = getCharaInfo(_loc1_);
            _loc13_ = null;
            if(_loc12_ && _loc12_.length > 0)
            {
               _loc14_ = _loc12_[0][CharaList.VarTypeCode];
               _loc15_ = m_callback.GetUserDataString(ReceiveType_ImageStrStart + _loc1_);
               if(_loc14_ != CharaList.InvalidCode)
               {
                  _loc13_ = new Bitmap(null);
                  IggyFunctions.setTextureForBitmap(_loc13_,_loc15_);
                  _loc13_.scaleX = 256 / _loc13_.width;
                  _loc13_.scaleY = 128 / _loc13_.height;
               }
            }
            m_chara_face[_loc1_] = _loc13_;
            _loc1_++;
         }
         m_chara_face_npc = new Array(PlayerNumFri);
         _loc1_ = 0;
         while(PlayerNumFri > _loc1_)
         {
            _loc16_ = m_callback.GetUserDataString(ReceiveType_ImageStrNpcStart + _loc1_);
            _loc17_ = new Bitmap(null);
            IggyFunctions.setTextureForBitmap(_loc17_,_loc16_);
            _loc17_.scaleX = 256 / _loc17_.width;
            _loc17_.scaleY = 128 / _loc17_.height;
            m_chara_face_npc[_loc1_] = _loc17_;
            _loc1_++;
         }
         var _loc6_:MovieClip = m_timeline.cha_select_cur;
         _loc6_.icn_lock.visible = false;
         ResetIcons();
         _loc1_ = 0;
         while(PlayerMax > _loc1_)
         {
            if(_loc1_ == m_current_player_index)
            {
               setReadyIcon(_loc1_,true,false);
            }
            else
            {
               _loc19_ = m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
               if(0 <= _loc1_ - 1 && _loc1_ - 1 < _loc19_)
               {
                  setReadyIcon(_loc1_,false,true);
               }
               else
               {
                  setReadyIcon(_loc1_,false,false);
               }
            }
            _loc18_ = getReadyIconMc(_loc1_);
            if(PlayerIndexOwn == _loc1_)
            {
               _loc18_.cmn_icn_you.visible = true;
            }
            else
            {
               _loc18_.cmn_icn_you.visible = false;
            }
            _loc1_++;
         }
         _loc1_ = 0;
         while(CharaVarIndexNum > _loc1_)
         {
            _loc20_ = getMcChamyset(_loc1_);
            if(_loc20_)
            {
            }
            _loc1_++;
         }
         var _loc7_:int = m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc7_ <= 0)
         {
            m_timeline.timer.visible = false;
         }
         else
         {
            m_timeline.timer.visible = true;
            m_timer.Start(_loc7_,cbFuncEndTimer);
         }
         if(m_chara_num_column <= IndexNumColumn)
         {
            m_timeline.cha_arrow.visible = false;
         }
         m_timeline.cha_select.gotoAndPlay("start");
         m_timeline.cha_parameter.gotoAndPlay("start");
         m_timeline.cha_frame.cmn_CMN_M_frame.gotoAndPlay("start");
         m_timeline.cha_arrow.gotoAndPlay("start");
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,waitMain);
      }
      
      private function waitMain(param1:Event) : void
      {
         if(m_timeline.cha_parameter.currentFrame != Utility.GetLabelEndFrame(m_timeline.cha_parameter,"start"))
         {
            return;
         }
         setSelectChara();
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitMain);
         m_timeline.stage.addEventListener(KeyboardEvent.KEY_DOWN,checkKey);
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,main);
      }
      
      private function main(param1:Event) : void
      {
         var _loc10_:int = 0;
         var _loc11_:int = 0;
         var _loc12_:Array = null;
         var _loc13_:Array = null;
         var _loc14_:int = 0;
         var _loc15_:int = 0;
         var _loc16_:int = 0;
         var _loc2_:int = 0;
         var _loc3_:int = m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc3_ > 0)
         {
            if(m_timer)
            {
               _loc10_ = m_timer.GetTime();
               if(_loc10_ <= _loc3_)
               {
                  m_timeline.timer.nest._CMN_M_B_mc_timer.visible = true;
               }
               else
               {
                  m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
               }
            }
         }
         updateLocalBattle();
         updateCharaIcon();
         var _loc4_:String = m_callback.GetUserDataString(ReceiveType_CharaNameStr);
         m_timeline.cha_parameter.sys_charaName.sys_charaName.text = _loc4_;
         updateSkill();
         var _loc5_:MovieClip = m_timeline.cha_select;
         var _loc6_:MovieClip = _loc5_["chara_icn_set0" + (m_select_icon_column + 1)];
         var _loc7_:MovieClip = _loc6_["nest_charaselect0" + (m_select_icon_row + 1)];
         if(!m_flag_change_player)
         {
            return;
         }
         m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,checkKey);
         if(m_flag_decide)
         {
            if(m_timeline.cha_parameter.currentLabel == "start" || m_timeline.cha_parameter.currentLabel == "wait")
            {
               m_timeline.cha_parameter.gotoAndPlay("push");
               _loc11_ = m_select_info[m_current_player_index][SelectInfoTypeListIndex];
               _loc12_ = getCharaInfo(_loc11_);
               if(_loc12_)
               {
                  _loc13_ = _loc12_[m_select_var][CharaList.VarTypeVoiceIdList];
                  _loc14_ = Math.floor(Math.random() * _loc13_.length);
                  _loc15_ = _loc13_[_loc14_];
                  m_callback.CallbackUserData("user",SendType_RequestPlayVoice,_loc15_);
               }
            }
            if(m_timeline.cha_parameter.currentFrame < Utility.GetLabelEndFrame(m_timeline.cha_parameter,"push"))
            {
               return;
            }
         }
         if(m_timeline.cha_parameter.currentLabel == "end_comp")
         {
            m_timeline.cha_parameter.nest_clothes.visible = false;
            if(m_flag_exit)
            {
               end();
               return;
            }
            changeCurrentPlayer(m_flag_decide);
            updatePage();
            setSelectChara();
            sendCharaInfo(m_current_player_index);
            m_flag_decide = false;
            m_flag_change_player = false;
            var _loc8_:Boolean = false;
            if(m_current_player_index < 0 || PlayerMax <= m_current_player_index)
            {
               _loc8_ = true;
            }
            if(_loc8_)
            {
               end();
               return;
            }
            var _loc9_:int = m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
            if(!_loc9_)
            {
               if(_loc3_ > 0)
               {
                  m_timer.Start(_loc3_ + 1,cbFuncEndTimer);
                  m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
               }
            }
            m_timeline.cha_parameter.gotoAndPlay("start");
            m_timeline.cha_parameter.nest_clothes.visible = true;
            m_timeline.stage.addEventListener(KeyboardEvent.KEY_DOWN,checkKey);
            return;
         }
         if(m_timeline.cha_parameter.currentLabel == "end")
         {
            return;
         }
         m_timeline.cha_parameter.gotoAndPlay("end");
         if(!m_flag_exit)
         {
            _loc16_ = -1;
            if(m_timer)
            {
               _loc16_ = m_timer.GetTime();
            }
            m_callback.CallbackUserData("user",SendType_RequestDecide,_loc16_);
         }
      }
      
      private function end() : void
      {
         m_timeline.cha_select.gotoAndPlay("end");
         m_timeline.cha_arrow.gotoAndPlay("end");
         m_timeline.cha_skill.gotoAndPlay("end");
         m_timeline.cha_frame.cmn_CMN_M_frame.gotoAndPlay("end");
         m_timeline.cha_select_cur.visible = false;
         m_timer.End();
         m_timeline.timer.gotoAndPlay("end");
         var _loc1_:int = m_callback.GetUserDataInt(ReceiveType_Time);
         if(_loc1_ > 0)
         {
            m_timer.Stop();
         }
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,main);
         m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,checkKey);
         if(m_current_player_index < 0)
         {
            m_callback.CallbackCancel();
         }
         else
         {
            m_callback.CallbackDecide(1);
         }
         m_timeline.stage.addEventListener(Event.ENTER_FRAME,waitEnd);
      }
      
      private function waitEnd(param1:Event) : void
      {
         if(m_timeline.cha_select.currentFrame != Utility.GetLabelEndFrame(m_timeline.cha_select,"end"))
         {
            return;
         }
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitEnd);
         m_callback.CallbackExit();
      }
      
      private function cbFuncEndTimer() : void
      {
         pushKeyDecide();
      }
      
      private function updateLocalBattle() : void
      {
         var _loc1_:int = m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
         if(!_loc1_)
         {
            return;
         }
         var _loc2_:int = m_callback.GetUserDataInt(ReceiveType_Time);
         var _loc3_:int = m_callback.GetUserDataInt(ReceiveType_Flag2pController);
         if(_loc3_)
         {
            if(m_timeline.press2P.currentLabel != "end" && m_timeline.press2P.currentLabel != "end_comp")
            {
               m_timeline.press2P.gotoAndPlay("end");
               if(_loc2_ >= 0)
               {
                  if(m_current_player_index != 0)
                  {
                     if(m_timer)
                     {
                        m_timer.Start(_loc2_ + 1,cbFuncEndTimer);
                        m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
                     }
                  }
               }
            }
         }
         else if(m_timeline.press2P.currentLabel != "start" && m_timeline.press2P.currentLabel != "wait")
         {
            m_timeline.press2P.gotoAndPlay("start");
            m_timeline.press2P.nest.sys.sys.htmlText = m_callback.GetUserDataString(ReceiveType_Str2pController);
         }
         switch(m_current_player_index)
         {
            case 0:
               if(!m_timeline.cha_parameter.icon_1P.visible)
               {
                  m_timeline.cha_parameter.icon_1P.gotoAndPlay("start");
               }
               m_timeline.cha_parameter.icon_1P.visible = true;
               m_timeline.cha_parameter.icon_2P.visible = false;
               break;
            case 3:
               m_timeline.cha_parameter.icon_1P.visible = false;
               if(!m_timeline.cha_parameter.icon_2P.visible)
               {
                  m_timeline.cha_parameter.icon_2P.gotoAndPlay("start");
                  if(_loc2_ >= 0)
                  {
                     if(m_timer)
                     {
                        if(_loc3_)
                        {
                           m_timer.Start(_loc2_ + 1,cbFuncEndTimer);
                           m_timeline.timer.nest._CMN_M_B_mc_timer.visible = false;
                        }
                        else
                        {
                           m_timer.Stop();
                        }
                     }
                  }
               }
               m_timeline.cha_parameter.icon_2P.visible = true;
               break;
            default:
               m_timeline.cha_parameter.icon_1P.visible = false;
               m_timeline.cha_parameter.icon_2P.visible = false;
         }
      }
      
      private function getCharaInfo(param1:int) : Array
      {
         if(param1 < 0)
         {
            return null;
         }
         if(m_chara_list_num <= param1)
         {
            return null;
         }
         if(m_chara_list[param1][0] < 0)
         {
            return null;
         }
         return m_chara_list[param1];
      }
      
      public function GetSelectVarInfo(param1:int) : Array
      {
         if(param1 < 0 || PlayerMax <= param1)
         {
            return null;
         }
         var _loc2_:int = m_select_info[param1][SelectInfoTypeListIndex];
         var _loc3_:Array = getCharaInfo(_loc2_);
         if(!_loc3_)
         {
            return null;
         }
         var _loc4_:int = m_select_info[param1][SelectInfoTypeVarIndex];
         _loc4_ = _loc4_ + _loc3_.length;
         _loc4_ = _loc4_ % _loc3_.length;
         return _loc3_[_loc4_];
      }
      
      private function getReadyIconMc(param1:int) : MovieClip
      {
         var _loc2_:int = m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc3_:int = 0;
         if(_loc2_ < param1)
         {
            _loc3_ = param1;
         }
         else
         {
            _loc3_ = _loc2_ - param1;
         }
         var _loc4_:MovieClip = m_timeline.cha_parameter.nest_ready["btnact_rb0" + (_loc3_ + 1)];
         return _loc4_;
      }
      
      private function calcCharaListIndex(param1:int, param2:int) : int
      {
         return param1 + param2 * IndexNumRow;
      }
      
      private function calcIconIndexRow(param1:int) : int
      {
         if(param1 < 0 || m_chara_list_num <= param1)
         {
            return -1;
         }
         return param1 % IndexNumRow;
      }
      
      private function calcIconIndexColumn(param1:int) : int
      {
         if(param1 < 0 || m_chara_list_num <= param1)
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
            if(checkUnlockVar(param1,_loc2_))
            {
               _loc3_++;
            }
            _loc2_++;
         }
         return _loc3_;
      }
      
      private function updatePage() : void
      {
         var _loc3_:int = 0;
         var _loc4_:int = 0;
         var _loc6_:MovieClip = null;
         var _loc1_:Boolean = false;
         var _loc2_:Boolean = false;
         if(m_select_column < m_select_column_start)
         {
            m_select_column_start = m_select_column;
            _loc1_ = true;
            _loc2_ = true;
         }
         else if(m_select_column_start + IndexNumColumn <= m_select_column)
         {
            m_select_column_start = m_select_column - IndexNumColumn + 1;
            _loc1_ = true;
         }
         if(!_loc1_)
         {
            return;
         }
         var _loc5_:MovieClip = m_timeline.cha_select;
         _loc4_ = -1;
         while(IndexNumColumn + 1 > _loc4_)
         {
            _loc6_ = _loc5_["chara_icn_set0" + (_loc4_ + 1)];
            if(_loc2_)
            {
               _loc6_.gotoAndPlay("push_l");
               m_timeline.cha_arrow.spinbtn_l.cmn_CMN_M_B_mc_spinbtn_l.gotoAndPlay("push");
            }
            else
            {
               _loc6_.gotoAndPlay("push_r");
               m_timeline.cha_arrow.spinbtn_r.cmn_CMN_M_B_mc_spinbtn_r.gotoAndPlay("push");
            }
            _loc4_++;
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
               m_select_column--;
               if(m_select_column < 0)
               {
                  m_select_column = m_chara_num_column - 1;
                  m_select_row--;
                  m_select_row = m_select_row + IndexNumRow;
                  m_select_row = m_select_row % IndexNumRow;
               }
            }
            else
            {
               m_select_column++;
               if(m_chara_num_column <= m_select_column)
               {
                  m_select_column = 0;
                  m_select_row++;
                  m_select_row = m_select_row + IndexNumRow;
                  m_select_row = m_select_row % IndexNumRow;
               }
            }
            _loc2_ = calcCharaListIndex(m_select_row,m_select_column);
            if(m_current_player_index != PlayerIndexOwn)
            {
               _loc4_ = m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(!_loc4_)
               {
                  if(checkAvatar(_loc2_))
                  {
                     continue;
                  }
               }
            }
            _loc3_ = checkUnlockChara(_loc2_);
            if(!_loc3_)
            {
               continue;
            }
            break;
         }
         m_select_var = 0;
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
               m_select_row--;
               if(m_select_row < 0)
               {
                  m_select_row = IndexNumRow - 1;
                  m_select_column--;
                  m_select_column = m_select_column + m_chara_num_column;
                  m_select_column = m_select_column % m_chara_num_column;
               }
            }
            else
            {
               m_select_row++;
               if(IndexNumRow <= m_select_row)
               {
                  m_select_row = 0;
                  m_select_column++;
                  m_select_column = m_select_column + m_chara_num_column;
                  m_select_column = m_select_column % m_chara_num_column;
               }
            }
            _loc2_ = calcCharaListIndex(m_select_row,m_select_column);
            if(m_current_player_index != PlayerIndexOwn)
            {
               _loc4_ = m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
               if(!_loc4_)
               {
                  if(checkAvatar(_loc2_))
                  {
                     continue;
                  }
               }
            }
            _loc3_ = checkUnlockChara(_loc2_);
            if(!_loc3_)
            {
               continue;
            }
            break;
         }
         m_select_var = 0;
      }
      
      private function changeVar(param1:Boolean) : void
      {
         var _loc4_:Boolean = false;
         var _loc2_:int = calcCharaListIndex(m_select_row,m_select_column);
         var _loc3_:Array = getCharaInfo(_loc2_);
         if(!_loc3_)
         {
            return;
         }
         while(true)
         {
            if(param1)
            {
               m_select_var--;
            }
            else
            {
               m_select_var++;
            }
            m_select_var = m_select_var + _loc3_.length;
            m_select_var = m_select_var % _loc3_.length;
            _loc4_ = checkUnlockVar(_loc2_,m_select_var);
            if(!_loc4_)
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
         var _loc5_:int = 0;
         var _loc6_:Boolean = false;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         if(param1 < 0 || PlayerMax <= param1)
         {
            return;
         }
         var _loc4_:MovieClip = getReadyIconMc(param1);
         if(param3)
         {
            _loc5_ = checkPlayerTeamType(param1);
            _loc6_ = false;
            _loc7_ = param1 - 1;
            switch(_loc5_)
            {
               case PlayerTeamTypeOwn:
                  _loc4_.gotoAndStop("blue_team");
                  break;
               case PlayerTeamTypeFri:
                  _loc4_.gotoAndStop("blue_team");
                  _loc8_ = m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
                  _loc9_ = m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
                  if(0 <= _loc7_ && _loc7_ < _loc9_)
                  {
                     _loc6_ = true;
                  }
                  break;
               case PlayerTeamTypeEnm:
                  _loc4_.gotoAndStop("red_team");
            }
            _loc4_.sys_ready.text = "OK";
            if(_loc6_)
            {
               setImageFriendNpc(_loc4_.icn_chara_lit.chara_img,_loc7_);
            }
            else
            {
               _loc10_ = calcCharaListIndex(m_select_row,m_select_column);
               setImage(_loc4_.icn_chara_lit.chara_img,_loc10_,true);
            }
         }
         else
         {
            _loc4_.gotoAndStop("off");
            _loc4_.sys_ready.text = "---";
         }
         if(param2)
         {
            _loc4_.btnact_ready.gotoAndPlay("on");
         }
         else
         {
            _loc4_.btnact_ready.gotoAndPlay("off");
         }
      }
      
      private function changeCurrentPlayer(param1:Boolean) : void
      {
         var _loc5_:Boolean = false;
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:int = 0;
         var _loc2_:int = 0;
         setReadyIcon(m_current_player_index,false,param1);
         var _loc3_:int = m_callback.GetUserDataInt(ReceiveType_PlayerFriNum);
         var _loc4_:int = m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         do
         {
            if(param1)
            {
               m_current_player_index++;
            }
            else
            {
               m_current_player_index--;
            }
            _loc5_ = false;
            _loc6_ = checkPlayerTeamType(m_current_player_index);
            switch(_loc6_)
            {
               case PlayerTeamTypeFri:
                  _loc7_ = m_current_player_index - PlayerIndexFriStart;
                  _loc8_ = m_callback.GetUserDataInt(ReceiveType_PartyNpcNum);
                  if(_loc8_ > _loc7_)
                  {
                     _loc5_ = false;
                  }
                  else if(_loc3_ > _loc7_)
                  {
                     _loc5_ = true;
                  }
                  break;
               case PlayerTeamTypeEnm:
                  _loc9_ = m_current_player_index - PlayerIndexEnmStart;
                  if(_loc4_ > _loc9_)
                  {
                     _loc5_ = true;
                  }
                  break;
               case PlayerTeamTypeOwn:
               default:
                  _loc5_ = true;
            }
         }
         while(!_loc5_);
         
         setReadyIcon(m_current_player_index,true,false);
         if(0 <= m_current_player_index && m_current_player_index < PlayerMax)
         {
            _loc10_ = m_select_info[m_current_player_index][SelectInfoTypeListIndex];
            m_select_row = calcIconIndexRow(_loc10_);
            m_select_column = calcIconIndexColumn(_loc10_);
            m_select_var = m_select_info[m_current_player_index][SelectInfoTypeVarIndex];
         }
      }
      
      private function setSelectChara() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc13_:int = 0;
         var _loc14_:MovieClip = null;
         var _loc15_:Boolean = false;
         if(m_current_player_index < 0 || PlayerMax <= m_current_player_index)
         {
            return;
         }
         var _loc3_:int = calcCharaListIndex(m_select_row,m_select_column);
         var _loc4_:Array = getCharaInfo(_loc3_);
         var _loc5_:int = -1;
         if(_loc4_)
         {
            _loc5_ = _loc4_.length;
            _loc1_ = 0;
            while(_loc5_ > _loc1_)
            {
               _loc13_ = m_select_var + _loc1_;
               _loc13_ = _loc13_ % _loc5_;
               if(!checkUnlockVar(_loc3_,_loc13_))
               {
                  _loc1_++;
                  continue;
               }
               m_select_var = _loc13_;
               break;
            }
         }
         var _loc6_:int = m_select_row;
         var _loc7_:int = m_select_column - m_select_column_start;
         var _loc8_:MovieClip = m_timeline.cha_select;
         var _loc9_:MovieClip = _loc8_["chara_icn_set0" + (_loc7_ + 1)];
         var _loc10_:MovieClip = _loc9_["nest_charaselect0" + (_loc6_ + 1)];
         m_select_icon_row = _loc6_;
         m_select_icon_column = _loc7_;
         _loc1_ = 0;
         while(CharaVarIndexNum > _loc1_)
         {
            _loc14_ = getMcChamyset(_loc1_);
            if(_loc14_)
            {
               _loc15_ = checkUnlockVar(_loc3_,_loc1_);
               _loc14_.btnact_off.visible = true;
               if(_loc1_ < _loc5_)
               {
                  if(_loc15_)
                  {
                     if(_loc1_ == m_select_var)
                     {
                        if(_loc14_.currentLabel != "on")
                        {
                           _loc14_.gotoAndPlay("on");
                           _loc14_.btnact_ef.visible = true;
                           _loc14_.btnact_on.visible = true;
                           _loc14_.btnact_off.visible = false;
                        }
                     }
                     else
                     {
                        _loc14_.gotoAndPlay("off");
                        _loc14_.btnact_ef.visible = false;
                        _loc14_.btnact_on.visible = false;
                        _loc14_.btnact_off.visible = true;
                     }
                  }
                  else
                  {
                     _loc14_.gotoAndPlay("lock");
                     _loc14_.btnact_ef.visible = false;
                     _loc14_.btnact_on.visible = false;
                     _loc14_.btnact_off.visible = true;
                  }
               }
               else
               {
                  _loc14_.gotoAndPlay("off");
                  _loc14_.btnact_ef.visible = false;
                  _loc14_.btnact_on.visible = false;
                  _loc14_.btnact_off.visible = false;
               }
            }
            _loc1_++;
         }
         var _loc11_:int = getVarIndexNum(_loc3_);
         var _loc12_:* = false;
         if(_loc11_ > 1)
         {
            _loc12_ = true;
         }
         m_timeline.cha_parameter.nest_clothes.sys_r.visible = _loc12_;
         m_timeline.cha_parameter.nest_clothes.sys_l.visible = _loc12_;
         m_select_info[m_current_player_index][SelectInfoTypeListIndex] = _loc3_;
         m_select_info[m_current_player_index][SelectInfoTypeVarIndex] = m_select_var;
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
         var _loc2_:Array = GetSelectVarInfo(param1);
         if(!_loc2_)
         {
            SetUserDataString(CharaSele.ReceiveType_CharaNameStr,"???");
            return;
         }
         var _loc3_:String = _loc2_[CharaList.VarTypeCode];
         var _loc4_:int = m_select_info[param1][SelectInfoTypeVarIndex];
         var _loc5_:int = _loc2_[CharaList.VarTypeMid];
         var _loc6_:int = _loc2_[CharaList.VarTypeModelPreset];
         m_callback.CallbackUserData("user",SendType_CurrentPlayerIndex,param1);
         m_callback.CallbackUserDataString("user",SendType_SelectCode,_loc3_);
         m_callback.CallbackUserData("user",SendType_SelectVariation,_loc4_);
         m_callback.CallbackUserData("user",SendType_SelectMid,_loc5_);
         m_callback.CallbackUserData("user",SendType_SelectModelPreset,_loc6_);
         m_callback.CallbackUserData("user",SendType_RequestCharaInfo,0);
         m_callback.SetUserDataValidFlag(ReceiveType_CharaNameStr,false);
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
         if(!checkUnlockChara(param2))
         {
            return;
         }
         if(m_chara_face[param2])
         {
            _loc4_ = m_chara_face[param2];
            if(param3)
            {
               _loc5_ = m_callback.GetUserDataString(ReceiveType_ImageStrStart + param2);
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
         if(m_chara_face_npc[param2])
         {
            _loc3_ = m_chara_face_npc[param2];
            param1.addChild(_loc3_);
         }
      }
      
      private function updateCharaIcon() : void
      {
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         var _loc6_:MovieClip = null;
         var _loc7_:MovieClip = null;
         var _loc8_:int = 0;
         var _loc9_:int = 0;
         var _loc10_:MovieClip = null;
         var _loc3_:MovieClip = m_timeline.cha_select;
         _loc2_ = -1;
         while(IndexNumColumn + 1 > _loc2_)
         {
            _loc6_ = _loc3_["chara_icn_set0" + (_loc2_ + 1)];
            _loc1_ = 0;
            while(IndexNumRow > _loc1_)
            {
               _loc7_ = _loc6_["nest_charaselect0" + (_loc1_ + 1)];
               if(_loc7_)
               {
                  _loc8_ = calcCharaListIndex(_loc1_,_loc2_ + m_select_column_start);
                  setImage(_loc7_.chara_img,_loc8_,false);
                  if(checkNoChara(_loc8_))
                  {
                     _loc7_.visible = false;
                  }
                  else if(checkUnlockChara(_loc8_))
                  {
                     _loc7_.visible = true;
                     _loc9_ = m_callback.GetUserDataInt(ReceiveType_FlagSelectAvatar);
                     if(!_loc9_ && m_current_player_index != PlayerIndexOwn && checkAvatar(_loc8_))
                     {
                        _loc7_.gotoAndPlay("grayout");
                     }
                     else if(m_select_icon_column == _loc2_ && m_select_icon_row == _loc1_)
                     {
                        _loc10_ = m_timeline.cha_select_cur;
                        _loc10_.visible = true;
                        _loc10_.x = _loc3_.x + _loc6_.x + _loc7_.x;
                        _loc10_.y = _loc3_.y + _loc6_.y + _loc7_.y;
                        setImage(_loc10_.chara_sel.chara_img,_loc8_,false);
                        if(m_flag_decide)
                        {
                           if(_loc10_.currentLabel != "push")
                           {
                              _loc10_.gotoAndPlay("push");
                           }
                        }
                        else if(_loc10_.currentLabel != "on")
                        {
                           _loc10_.gotoAndPlay("on");
                        }
                     }
                     else
                     {
                        _loc7_.gotoAndPlay("off");
                     }
                  }
                  else
                  {
                     _loc7_.visible = true;
                     _loc7_.gotoAndPlay("lock");
                  }
               }
               _loc1_++;
            }
            _loc2_++;
         }
         var _loc4_:MovieClip = _loc3_["chara_icn_set00"];
         if(m_select_column_start <= 0)
         {
            _loc4_.visible = false;
         }
         else
         {
            _loc4_.visible = true;
         }
         var _loc5_:MovieClip = _loc3_["chara_icn_set0" + (IndexNumColumn + 1)];
         if(m_chara_num_column <= m_select_column_start + IndexNumColumn)
         {
            _loc5_.visible = false;
         }
         else
         {
            _loc5_.visible = true;
         }
      }
      
      private function updateSkill() : void
      {
         var _loc3_:String = null;
         var _loc4_:Number = NaN;
         var _loc5_:Number = NaN;
         var _loc6_:String = null;
         var _loc7_:Number = NaN;
         var _loc8_:Number = NaN;
         var _loc1_:int = 0;
         ResetIcons();
         var _loc2_:MovieClip = m_timeline.cha_skill;
         if(m_flag_skill)
         {
            if(_loc2_.currentFrame > Utility.GetLabelEndFrame(_loc2_,"wait"))
            {
               _loc2_.gotoAndPlay("start");
               _loc2_.visible = true;
            }
            _loc1_ = 0;
            while(SkillMax > _loc1_)
            {
               _loc6_ = m_callback.GetUserDataString(ReceiveType_SkillNameStrStart + _loc1_);
               _loc2_.inact_skill["skill0" + (_loc1_ + 1)].sys_skill.scaleX = m_skill_str_scalex_default;
               _loc2_.inact_skill["skill0" + (_loc1_ + 1)].sys_skill.autoSize = "left";
               _loc2_.inact_skill["skill0" + (_loc1_ + 1)].sys_skill.htmlText = _loc6_;
               _loc7_ = _loc2_.inact_skill["skill0" + (_loc1_ + 1)].sys_skill.width;
               _loc8_ = m_skill_str_scalex_default;
               if(m_skill_str_width_default < _loc7_)
               {
                  _loc8_ = m_skill_str_width_default / _loc7_;
               }
               _loc2_.inact_skill["skill0" + (_loc1_ + 1)].sys_skill.scaleX = _loc8_;
               _loc1_++;
            }
            _loc3_ = m_callback.GetUserDataString(ReceiveType_TarismanNameStr);
            _loc2_.inact_skill.skill08.sys_skill.scaleX = m_skill_str_scalex_default;
            _loc2_.inact_skill.skill08.sys_skill.autoSize = "left";
            _loc2_.inact_skill.skill08.sys_skill.htmlText = _loc3_;
            _loc4_ = _loc2_.inact_skill.skill08.sys_skill.width;
            _loc5_ = m_skill_str_scalex_default;
            if(m_skill_str_width_default < _loc4_)
            {
               _loc5_ = m_skill_str_width_default / _loc4_;
            }
            _loc2_.inact_skill.skill08.sys_skill.scaleX = _loc5_;
         }
         else if(_loc2_.currentFrame <= Utility.GetLabelEndFrame(_loc2_,"wait"))
         {
            _loc2_.gotoAndPlay("end");
         }
      }
      
      private function getMcChamyset(param1:int) : MovieClip
      {
         var _loc2_:MovieClip = null;
         if(param1 < 9)
         {
            _loc2_ = m_timeline.cha_parameter.nest_clothes["btnact_chamyset_0" + (param1 + 1)];
         }
         else
         {
            _loc2_ = m_timeline.cha_parameter.nest_clothes["btnact_chamyset_" + (param1 + 1)];
         }
         return _loc2_;
      }
      
      private function checkUnlockVar(param1:int, param2:int) : Boolean
      {
         if(param1 < 0 || m_chara_list_num <= param1)
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
         if(param1 < 0 || m_chara_list_num <= param1)
         {
            return true;
         }
         var _loc2_:int = 0;
         _loc2_ = 0;
         while(CharaVarIndexNum > _loc2_)
         {
            if(checkUnlockVar(param1,_loc2_))
            {
               return true;
            }
            _loc2_++;
         }
         return true;
      }
      
      private function checkAvatar(param1:int) : Boolean
      {
         var _loc2_:Array = getCharaInfo(param1);
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
         var _loc2_:Array = getCharaInfo(param1);
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
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeLR(true);
         updatePage();
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushRight() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeLR(false);
         updatePage();
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushUp() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeUD(true);
         updatePage();
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushDown() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeUD(false);
         updatePage();
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushKeyL() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeVar(true);
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushKeyR() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         changeVar(false);
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushKeySkill() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         if(m_flag_skill)
         {
            m_flag_skill = false;
            m_callback.CallbackUserData("user",SendType_RequestSetFlagSkill,0);
         }
         else
         {
            m_flag_skill = true;
            m_callback.CallbackUserData("user",SendType_RequestSetFlagSkill,1);
         }
      }
      
      private function pushKeyRandom() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         var _loc1_:int = 0;
         var _loc2_:int = 0;
         _loc1_ = 0;
         while(m_chara_list_num > _loc1_)
         {
            if(checkUnlockChara(_loc1_))
            {
               _loc2_++;
            }
            _loc1_++;
         }
         if(m_current_player_index != PlayerIndexOwn)
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
            changeUD(false);
            updatePage();
            _loc1_++;
         }
         var _loc4_:int = calcCharaListIndex(m_select_row,m_select_column);
         var _loc5_:int = getVarIndexNum(_loc4_);
         var _loc6_:int = 0;
         if(_loc5_ > 1)
         {
            _loc6_ = Math.floor(Math.random() * _loc5_);
         }
         _loc1_ = 0;
         while(_loc6_ > _loc1_)
         {
            changeVar(false);
            _loc1_++;
         }
         setSelectChara();
         sendCharaInfo(m_current_player_index);
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function pushKeyDecide() : void
      {
         if(m_current_player_index != PlayerIndexOwn && m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle) && !m_callback.GetUserDataInt(ReceiveType_Flag2pController))
         {
            return;
         }
         m_flag_change_player = true;
         m_flag_decide = true;
         m_callback.CallbackSe(m_callback.SeTypeDecide);
      }
      
      private function pushKeyCancel() : void
      {
         var _loc2_:int = 0;
         var _loc1_:int = m_callback.GetUserDataInt(ReceiveType_FlagUseCancel);
         if(!_loc1_)
         {
            _loc2_ = m_callback.GetUserDataInt(ReceiveType_FlagLocalBattle);
            if(_loc2_)
            {
               return;
            }
            if(m_current_player_index <= PlayerIndexOwn)
            {
               return;
            }
         }
         m_flag_change_player = true;
         m_flag_decide = false;
         m_callback.CallbackSe(m_callback.SeTypeCancel);
      }
      
      private function pushStart() : void
      {
         var _loc1_:int = m_callback.GetUserDataInt(ReceiveType_PlayerEnmNum);
         if(_loc1_ > 0)
         {
            return;
         }
         if(m_current_player_index == PlayerIndexOwn)
         {
            return;
         }
         m_flag_change_player = true;
         m_flag_decide = false;
         m_flag_exit = true;
         m_callback.CallbackSe(m_callback.SeTypeCarsol);
      }
      
      private function checkKey(param1:KeyboardEvent) : void
      {
         if(m_flag_decide)
         {
            return;
         }
         switch(param1.keyCode)
         {
            case Keyboard.ENTER:
               pushKeyDecide();
               break;
            case Keyboard.ESCAPE:
               pushKeyCancel();
               break;
            case Keyboard.LEFT:
               pushLeft();
               break;
            case Keyboard.RIGHT:
               pushRight();
               break;
            case Keyboard.UP:
               pushUp();
               break;
            case Keyboard.DOWN:
               pushDown();
               break;
            case Keyboard.DELETE:
               pushKeyL();
               break;
            case Keyboard.PAGE_DOWN:
               pushKeyR();
               break;
            case 88:
               pushKeySkill();
               break;
            case 89:
               pushKeyRandom();
               break;
            case Keyboard.SPACE:
               pushStart();
         }
      }
      
      public function SetUserDataInt(param1:int, param2:int) : *
      {
         m_callback.AddCallbackSetUserDataInt(param1,param2);
      }
      
      public function SetUserDataString(param1:int, param2:String) : *
      {
         m_callback.AddCallbackSetUserDataString(param1,param2);
      }
      
      public function TestDestroy() : void
      {
         m_callback = null;
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,requestUnlock);
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitUnlock);
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitStart);
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitMain);
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,main);
         m_timeline.stage.removeEventListener(Event.ENTER_FRAME,waitEnd);
         m_timeline.stage.removeEventListener(KeyboardEvent.KEY_DOWN,checkKey);
         m_timeline.visible = false;
         m_timeline = null;
         m_timer.Destroy();
         m_timer = null;
      }
      
      public function TestCheckChangeSelect() : Boolean
      {
         return !m_callback.GetUserDataValidFlag(ReceiveType_CharaNameStr);
      }
      
      public function TestGetCharaList() : Array
      {
         return m_chara_list;
      }
      
      public function TestGetCharaVarInfo() : Array
      {
         return GetSelectVarInfo(m_current_player_index);
      }
      
      public function TestGetCurrentPlayerIndex() : int
      {
         return m_current_player_index;
      }
   }
}
