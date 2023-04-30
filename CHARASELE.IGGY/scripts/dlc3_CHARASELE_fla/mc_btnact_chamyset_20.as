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
         addFrameScript(8,this.frame9,58,this.frame59,66,this.frame67);
      }
      
      function frame9() : *
      {
         stop();
      }
      
      function frame59() : *
      {
         gotoAndPlay("on");
      }
      
      function frame67() : *
      {
         stop();
      }
   }
}
