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
      
      public function MainTimeline()
      {
         super();
         addFrameScript(0,frame1);
      }
      
      internal function frame1() : *
      {
         m_main = null;
         if(!m_main)
         {
            m_main = new CharaSele();
         }
         m_main.Initialize(this);
         stop();
      }
   }
}
