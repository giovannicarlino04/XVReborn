package action_script
{
   import flash.external.ExternalInterface;
   import flash.display.Bitmap;

   public class XVPatcher
   { 
      public static const XV_PATCHER_AS3_TAG = "XV_PATCHER_AS3_V1";
      
      public function XVPatcher()
      {
         super();
      }
      
      public static function HelloWorld() : String
      {
         try 
		 {
			return ExternalInterface.call("XVPHelloWorld");
		 } catch (e: Error)
		 {
		 }
		 
		 return "This doesn't work";
      }
	  
	  public static function GetSlotsData() : String
	  {
		try
		{
			return ExternalInterface.call("XVPGetSlots");
		} catch(e: Error)
		{
		}
		
		return "{}";
	  }
	  
   }
}