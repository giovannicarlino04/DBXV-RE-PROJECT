package dlc3_CHARASELE_fla
{
   import flash.display.MovieClip;
   
   public dynamic class mc_btnact_chamyset_20 extends MovieClip
   {
       
      
      public var btnact_off:MovieClip;
      
      public var sys_charamyset:MovieClip;
      
      public var btnact_on:MovieClip;
      
      public var btnact_ef:MovieClip;
      
      public function mc_btnact_chamyset_20()
      {
         super();
         addFrameScript(8,frame9,58,frame59,66,frame67);
      }
      
      internal function frame9() : *
      {
         stop();
      }
      
      internal function frame59() : *
      {
         gotoAndPlay("on");
      }
      
      internal function frame67() : *
      {
         stop();
      }
   }
}