using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using leo.cap.study.Data;
using Microsoft.AspNetCore.Mvc;

namespace leo.cap.study.Controllers
{
    public class PublishController : Controller
    {
        private readonly ICapPublisher _capBus;

        public PublishController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        //不使用事务
        [Route("~/without/transaction")]
        public IActionResult WithoutTransaction(string msg="发送消息")
        {
            _capBus.Publish("xxx.services.show.time", msg);
            _capBus.Publish("leo.services.showmsg", msg, "leo.services.callback");
            return Ok();
        }

        //Ado.Net 中使用事务，自动提交
        //[Route("~/adonet/transaction")]
        //public IActionResult AdonetWithTransaction()
        //{
        //    using (var connection = new MySqlConnection(ConnectionString))
        //    {
        //        using (var transaction = connection.BeginTransaction(_capBus, autoCommit: true))
        //        {
        //            //业务代码

        //            _capBus.Publish("xxx.services.show.time", DateTime.Now);
        //        }
        //    }
        //    return Ok();
        //}

        //EntityFramework 中使用事务，自动提交
        [Route("~/ef/transaction")]
        public IActionResult EntityFrameworkWithTransaction([FromServices]AppDbContext dbContext)
        {
            using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
            {
                //业务代码

                _capBus.Publish("xxx.services.show.time", DateTime.Now);
            }
            return Ok("发送成功");
        }




        [CapSubscribe("xxx.services.show.time")]
        public void CheckReceivedMessage(string msg)
        {
            Console.WriteLine(DateTime.Now+":"+msg);
        }

        [CapSubscribe("leo.services.callback")]
        public void ReceivedMessageCallback(string content)
        {
            Console.WriteLine(DateTime.Now + ":" + "回调"+ content);
        }
    }
}