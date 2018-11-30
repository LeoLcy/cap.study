using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leo.cap.study.Service
{
    public class TestService
    {
        public interface ISubscriberService
        {
            string CheckReceivedMessage(string msg);
        }

        public class SubscriberService : ISubscriberService, ICapSubscribe
        {
            [CapSubscribe("leo.services.showmsg")]
            public string CheckReceivedMessage(string msg)
            {
                Console.WriteLine($"{DateTime.Now.ToLocalTime()}=>leo.services.showmsg", msg);

                return "回调信息";
            }
        }
    }
}
