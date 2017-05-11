using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TestBotAPI.Controllers {
    public class LineChatController : ApiController {
        [HttpPost]
        public IHttpActionResult POST() {
            string ChannelAccessToken = "zZqwp8/TJhZyXzrMdtstYXJV4xAiieUTotHC275jw9tcZF9YDdMYYzSLloWbR7YkGOg73uyzjdyapMD/ArsX5cvZOdY3FosaKxiJY27F89vp/OiPmjOrJgtmaMg1WGJp5XXn/da2ivg1QR8bc2hqRAdB04t89/1O/w1cDnyilFU=";

            try {
                //取得 http Post RawData(should be JSON)
                string postData = Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = Utility.Parsing(postData);
                //回覆訊息
                foreach (Event e in ReceivedMessage.events) {
                    if (e.source.type == "user") {
                        LineUserInfo user = Utility.GetUserInfo(e.source.userId, ChannelAccessToken);
                        string Message;
                        switch (e.type) {
                            case "follow":
                                Message = "สวัสดีคุณ" + user.displayName + "\nลงทะเบียนรับการแจ้งเตือนเอกสารธุรกรรมผ่านช่องทางไลน์ พิมพ์ \"#R\"";
                                Utility.ReplyMessage(e.replyToken, Message, ChannelAccessToken);
                                break;
                            case "message":
                                switch (e.message.type) {
                                    case "text":
                                        if (e.message.text.ToLower() == "#r") {
                                            var actions = new List<TemplateActionBase>();
                                            actions.Add(new MessageActon() {
                                                label = "Message",
                                                text = "Text"
                                            });

                                            actions.Add(new UriActon() {
                                                label = "Uri",
                                                uri = new Uri("http://dev.tks.co.th/ChannelOne/Home/Register?refer=")
                                            });

                                            actions.Add(new PostbackActon() {
                                                label = "postback",
                                                data = " ABC DEF = AAA = 111 & "
                                            });
                                            var C = new List<Column>();
                                            C.Add(new Column() {
                                                thumbnailImageUrl = new Uri("https://dev.tks.co.th/ChannelOne/Content/img/headTemplateTh.png"),
                                                text = "กด “ลงทะเบียน” เพื่อไปหน้าลงทะเบียน",
                                                actions = actions
                                            });
                                            C.Add(new Column() {
                                                thumbnailImageUrl = new Uri("https://dev.tks.co.th/ChannelOne/Content/img/headTemplateEn.png"),
                                                text = "Press “Register” to continue",
                                                actions = actions
                                            });
                                            var carousel = new CarouselTemplate() {
                                                altText = "กดลงทะเบียนรับการแจ้งเตือนเอกสารธุระกรรมผ่านช่องทางไลน์",
                                                columns = C
                                            };
                                            Utility.PushTemplateMessage(user.userId, carousel, ChannelAccessToken);
                                        } else {
                                            Message = "Bot : " + e.message.text;
                                            Utility.ReplyMessage(e.replyToken, Message, ChannelAccessToken);
                                        }
                                        break;
                                    default:
                                        Message = "ขอบคุณสำหรับข้อความ!􀄃􀄄blush􏿿\nขออภัย เราไม่สามารถตอบกลับผู้ใช้ เป็นส่วนตัวได้จากบัญชีนี้้􀄃􀄑hm􏿿";
                                        Utility.ReplyMessage(e.replyToken, Message, ChannelAccessToken);
                                        break;
                                }
                                break;
                        }
                    }
                    //string Message;
                    //Message = "GG:" + e.message.text;
                    //isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, Message, ChannelAccessToken);
                }
                //回覆用戶

                //回覆API OK
                return Ok();
            } catch (Exception ex) {
                return Ok();
            }
        }
    }
}
