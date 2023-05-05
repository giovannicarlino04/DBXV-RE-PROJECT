package dlc3_CHARASELE_fla
{
   import action_script.CharaSele;
   import flash.display.MovieClip;
   
   public dynamic class MainTimeline extends MovieClip
   {
       
      
      public var cha_skill:MovieClip;
      
      public var press2P:MovieClip;
      
      public var cha_select_cur:MovieClip;
      
      public var cha_frame:MovieClip;
      
      public var cha_parameter:MovieClip;
      
      public var timer:MovieClip;
      
      public var cha_arrow:MovieClip;
      
      public var cha_select:MovieClip;
      
      public var m_main:CharaSele;
      
      public var aaa1:_CMN_M_frame_49;
      
      public var aaa2:mc_btnact_chamyset_20;
      
      public var aaa3:mc_cha_arrow_45;
      
      public var aaa4:mc_cha_cursol_35;
      
      public var aaa5:mc_cha_parameter_1;
      
      public var aaa6:mc_cha_select_28;
      
      public var aaa7:mc_cha_skill_40;
      
      public var aaa8:mc_chara_btnact_30;
      
      public var aaa9:mc_chara_set_29;
      
      public var aaa10:mc_ready_btnact_enm_8;
      
      public var aaa11:mc_ready_btnact_fri_16;
      
      public var aaa12:mc_ready_nest_enm_7;
      
      public var aaa13:mc_ready_nest_fri_15;
      
      public var aaa14:mc_timer_53;
      
      public var aaa15:sys_2Ppress_50;
      
      public function MainTimeline()
      {
         super();
         addFrameScript(0,this.frame1);
      }
      
      function frame1() : *
      {
         this.m_main = null;
         if(!this.m_main)
         {
            this.m_main = new CharaSele();
         }
         this.m_main.Initialize(this);
         stop();
      }
   }
}
