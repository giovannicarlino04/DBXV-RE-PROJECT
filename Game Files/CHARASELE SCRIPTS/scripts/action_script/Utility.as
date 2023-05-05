package action_script
{
   import flash.display.MovieClip;
   import flash.text.TextField;
   import flash.text.TextFieldAutoSize;
   import flash.text.TextFormat;
   import flash.text.TextFormatAlign;
   
   public class Utility
   {
       
      
      public function Utility()
      {
         super();
      }
      
      public static function GetLabelEndFrame(param1:MovieClip, param2:String) : int
      {
         var _loc3_:int = param1.totalFrames - 1;
         var _loc4_:int = 0;
         while(_loc4_ < param1.currentLabels.length)
         {
            if(param1.currentLabels[_loc4_].name == param2 && _loc4_ + 1 < param1.currentLabels.length)
            {
               _loc3_ = param1.currentLabels[_loc4_ + 1].frame - 1;
            }
            _loc4_++;
         }
         return _loc3_;
      }
      
      public static function SetAutoSizedText(param1:TextField, param2:String) : void
      {
         var _loc3_:Number = NaN;
         var _loc4_:String = null;
         var _loc5_:MovieClip = param1.parent as MovieClip;
         if(!_loc5_)
         {
            return;
         }
         if(_loc5_.user_add_property_default_width == undefined)
         {
            _loc3_ = param1.width;
            _loc4_ = param1.getTextFormat().align;
            _loc5_.user_add_property_default_width = _loc3_;
            _loc5_.user_add_property_default_align = _loc4_;
            param1.autoSize = param1.getTextFormat().align;
         }
         SetSizedText(param1,param2,_loc5_.user_add_property_default_width,_loc5_.user_add_property_default_align);
      }
      
      public static function SetSizedText(param1:TextField, param2:String, param3:Number, param4:String) : void
      {
         var _loc5_:* = undefined;
         var _loc6_:TextField = null;
         param1.autoSize = param4;
         if(param4 == TextFormatAlign.RIGHT || param4 == TextFormatAlign.CENTER)
         {
            _loc5_ = param1.defaultTextFormat;
            _loc6_ = new TextField();
            _loc6_.defaultTextFormat = new TextFormat(_loc5_.font,_loc5_.size,_loc5_.color,_loc5_.bold,_loc5_.italic,_loc5_.underline);
            _loc6_.autoSize = param4;
            _loc6_.embedFonts = param1.embedFonts;
            _loc6_.scaleX = 1;
            _loc6_.text = param2;
            _loc6_.width = param3;
            if(param3 < _loc6_.width)
            {
               param1.autoSize = TextFieldAutoSize.LEFT;
            }
            else
            {
               param1.autoSize = TextFieldAutoSize.NONE;
            }
         }
         param1.scaleX = 1;
         param1.htmlText = param2;
         param1.width = param3;
         if(param3 < param1.width)
         {
            param1.scaleX = param3 / param1.width;
         }
      }
      
      public static function SetAutoSizedhtmlText(param1:TextField, param2:String, param3:Number = 0) : void
      {
         var _loc4_:Number = NaN;
         var _loc5_:String = null;
         var _loc6_:MovieClip = param1.parent as MovieClip;
         if(!_loc6_)
         {
            return;
         }
         if(_loc6_.user_add_property_default_width == undefined)
         {
            _loc4_ = param1.width;
            _loc5_ = param1.getTextFormat().align;
            _loc6_.user_add_property_default_width = _loc4_;
            _loc6_.user_add_property_default_align = _loc5_;
            param1.autoSize = param1.getTextFormat().align;
            param1.wordWrap = false;
         }
         SetSizedhtmlText(param1,param2,_loc6_.user_add_property_default_width + param3,_loc6_.user_add_property_default_align);
      }
      
      public static function SetSizedhtmlText(param1:TextField, param2:String, param3:Number, param4:String) : void
      {
         var _loc5_:* = undefined;
         var _loc6_:TextField = null;
         param1.autoSize = param4;
         if(param4 == TextFormatAlign.RIGHT || param4 == TextFormatAlign.CENTER)
         {
            _loc5_ = param1.defaultTextFormat;
            _loc6_ = new TextField();
            _loc6_.defaultTextFormat = new TextFormat(_loc5_.font,_loc5_.size,_loc5_.color,_loc5_.bold,_loc5_.italic,_loc5_.underline);
            _loc6_.autoSize = param4;
            _loc6_.embedFonts = param1.embedFonts;
            _loc6_.scaleX = 1;
            _loc6_.htmlText = param2;
            _loc6_.width = param3;
            if(param3 < _loc6_.width)
            {
               param1.autoSize = TextFieldAutoSize.LEFT;
            }
            else
            {
               param1.autoSize = TextFieldAutoSize.NONE;
            }
         }
         param1.scaleX = 1;
         param1.htmlText = param2;
         param1.width = param3;
         if(param3 < param1.width)
         {
            param1.scaleX = param3 / param1.width;
         }
      }
      
      public static function GetNotReplacedString(param1:String, param2:Boolean = false) : String
      {
         if(param2)
         {
            return "\t" + param1;
         }
         return param1 + "\t";
      }
      
      public static function ReplaceStringForHtmlText(param1:String) : String
      {
         var _loc2_:String = null;
         var _loc3_:Boolean = false;
         var _loc4_:int = 0;
         var _loc5_:Array = [{
            "src":"<",
            "dst":"&lt;"
         },{
            "src":">",
            "dst":"&gt;"
         },{
            "src":"&",
            "dst":"&amp;"
         },{
            "src":"\"",
            "dst":"&quot;"
         },{
            "src":"\'",
            "dst":"&apos;"
         }];
         var _loc6_:String = "";
         var _loc7_:int = 0;
         while(_loc7_ < param1.length)
         {
            _loc2_ = param1.charAt(_loc7_);
            _loc3_ = false;
            _loc4_ = 0;
            while(_loc4_ < _loc5_.length)
            {
               if(_loc2_ == _loc5_[_loc4_].src)
               {
                  _loc6_ = _loc6_ + _loc5_[_loc4_].dst;
                  _loc3_ = true;
                  break;
               }
               _loc4_++;
            }
            if(!_loc3_)
            {
               _loc6_ = _loc6_ + _loc2_;
            }
            _loc7_++;
         }
         return _loc6_;
      }
   }
}
