package action_script
{
   import flash.display.MovieClip;
   import flash.events.TimerEvent;
   import flash.utils.Timer;
   
   public class CountDownTimer
   {
      
      private static const DigitMax:uint = 3;
       
      
      private var m_mc_counter:MovieClip = null;
      
      private var m_mc_base:MovieClip = null;
      
      private var m_timer:Timer = null;
      
      private var m_cb_func_end:Function = null;
      
      private var m_warn_count:uint;
      
      public function CountDownTimer(param1:uint = 9)
      {
         super();
         this.m_mc_counter = null;
         this.m_mc_base = null;
         this.m_timer = null;
         this.m_cb_func_end = null;
         this.m_warn_count = param1;
      }
      
      public function Destroy() : void
      {
         this.destroyTimer();
         this.m_mc_base = null;
         this.m_mc_counter = null;
      }
      
      public function Initialize(param1:MovieClip, param2:MovieClip = null) : void
      {
         this.m_mc_counter = param1;
         this.m_mc_base = param2;
         this.m_timer = null;
         this.m_cb_func_end = null;
         this.m_mc_counter.visible = false;
         if(this.m_mc_base)
         {
            this.m_mc_base.visible = false;
         }
      }
      
      public function Start(param1:int, param2:Function = null) : void
      {
         if(param1 < 0)
         {
            return;
         }
         param1 = this.capMaxTime(param1);
         this.initDigit(param1);
         if(this.m_timer)
         {
            this.destroyTimer();
            this.setMcTimer(param1,"move");
            this.createTimer(param1,param2);
            return;
         }
         this.m_mc_counter.visible = true;
         this.setMcTimer(param1,"start");
         if(this.m_mc_base)
         {
            this.m_mc_base.visible = true;
            this.m_mc_base.gotoAndPlay("start");
         }
         this.createTimer(param1,param2);
      }
      
      public function End() : void
      {
         this.destroyTimer();
         if(this.m_mc_base)
         {
            this.m_mc_base.gotoAndPlay("end");
         }
         this.setMcTimer(-1,"end");
      }
      
      public function Stop() : void
      {
         if(this.m_timer)
         {
            this.m_timer.removeEventListener(TimerEvent.TIMER,this.countTimer);
            this.m_timer.removeEventListener(TimerEvent.TIMER_COMPLETE,this.compTimer);
         }
      }
      
      public function GetTime() : int
      {
         if(!this.m_timer)
         {
            return -1;
         }
         var _loc1_:int = this.m_timer.repeatCount - this.m_timer.currentCount;
         return _loc1_;
      }
      
      private function capMaxTime(param1:int) : int
      {
         var _loc2_:int = Math.pow(10,DigitMax);
         if(_loc2_ <= param1)
         {
            param1 = _loc2_ - 1;
         }
         return param1;
      }
      
      private function initDigit(param1:int) : void
      {
         var _loc2_:int = DigitMax;
         var _loc3_:int = 10;
         var _loc4_:int = 1;
         while(_loc4_ < DigitMax)
         {
            if(param1 < _loc3_)
            {
               _loc2_ = _loc4_;
               break;
            }
            _loc3_ = _loc3_ * 10;
            _loc4_++;
         }
         this.m_mc_counter.gotoAndStop(DigitMax + 1 - _loc2_);
      }
      
      private function createTimer(param1:int, param2:Function = null) : void
      {
         if(this.m_timer)
         {
            return;
         }
         this.m_cb_func_end = param2;
         this.m_timer = new Timer(1000,param1);
         this.m_timer.addEventListener(TimerEvent.TIMER,this.countTimer);
         this.m_timer.addEventListener(TimerEvent.TIMER_COMPLETE,this.compTimer);
         this.m_timer.start();
      }
      
      private function destroyTimer() : void
      {
         if(!this.m_timer)
         {
            return;
         }
         this.m_timer.stop();
         this.m_timer.removeEventListener(TimerEvent.TIMER,this.countTimer);
         this.m_timer.removeEventListener(TimerEvent.TIMER_COMPLETE,this.compTimer);
         this.m_timer = null;
         this.m_cb_func_end = null;
      }
      
      private function countTimer(param1:TimerEvent) : void
      {
         var _loc2_:int = this.m_timer.repeatCount - this.m_timer.currentCount;
         this.setMcTimer(_loc2_,"move");
         this.initDigit(_loc2_);
      }
      
      private function compTimer(param1:TimerEvent) : void
      {
         this.m_cb_func_end();
         this.Stop();
      }
      
      private function setMcTimer(param1:int, param2:String) : void
      {
         var _loc3_:int = 0;
         var _loc4_:* = param1 <= this.m_warn_count;
         var _loc5_:int = param1;
         var _loc6_:int = 1;
         while(_loc6_ <= DigitMax)
         {
            if(0 <= _loc5_)
            {
               if(this.m_mc_counter["nmb_" + _loc6_])
               {
                  _loc3_ = _loc5_ % 10 + 1;
                  if(_loc4_)
                  {
                     _loc3_ = _loc3_ + 10;
                  }
                  this.m_mc_counter["nmb_" + _loc6_].nmb01.gotoAndStop(_loc3_);
               }
               _loc5_ = _loc5_ / 10;
            }
            if(param2)
            {
               if(this.m_mc_counter["nmb_" + _loc6_])
               {
                  this.m_mc_counter["nmb_" + _loc6_].gotoAndPlay(param2);
               }
            }
            _loc6_++;
         }
      }
   }
}
