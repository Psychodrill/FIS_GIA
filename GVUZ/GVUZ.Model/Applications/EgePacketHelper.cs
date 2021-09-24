using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;

namespace GVUZ.Model.Applications
{
   internal static class EgePacketHelper
   {  
       public static EgePacket GetEgePacket(string clientUserName, params EgeQuery[] queries)
       {
           return new EgePacket(clientUserName, AppSettings.Get("EgeCheckLogin", ""), AppSettings.Get("EgeCheckPassword", ""), queries);
       }
    }
}
