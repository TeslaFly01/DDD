using System;
using System.ServiceModel;

namespace DDD.Utility
{
    public class WcfHelper
    {
        //wcf 调用帮助类
        /*
         try{    client.Close();}catch{    client.Abort();}
         */
        public static TReturn UseService<TChannel, TReturn>(Func<TChannel, TReturn> func)
        {
            var chanFactory = new ChannelFactory<TChannel>("*");
            TChannel channel = chanFactory.CreateChannel();
            TReturn result = func(channel);
            try { ((IClientChannel)channel).Close(); }
            catch { ((IClientChannel)channel).Abort(); }
            return result;
        }
    }
}
