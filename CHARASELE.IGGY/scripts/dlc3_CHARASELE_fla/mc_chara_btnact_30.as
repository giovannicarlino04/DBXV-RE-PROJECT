package dlc3_CHARASELE_fla
{
   import flash.display.MovieClip;
   
   public dynamic class mc_chara_btnact_30 extends MovieClip
   {
       
      
      public var chara_img:MovieClip;
      
      public var chara_icn:MovieClip;
      
      public var chara_mask:MovieClip;
      
      public function mc_chara_btnact_30()
      {
         super();
         addFrameScript(8,frame9,58,frame59,69,frame70,84,frame85);
      }
      
      internal function frame9() : *
      {
         stop();
      }
      
      internal function frame59() : *
      {
         gotoAndPlay("on");
      }
      
      internal function frame70() : *
      {
         stop();
      }
      
      internal function frame85() : *
      {
         stop();
      }
   }
}