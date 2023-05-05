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
         addFrameScript(8,this.frame9,58,this.frame59,69,this.frame70,84,this.frame85);
      }
      
      function frame9() : *
      {
         stop();
      }
      
      function frame59() : *
      {
         gotoAndPlay("on");
      }
      
      function frame70() : *
      {
         stop();
      }
      
      function frame85() : *
      {
         stop();
      }
   }
}
