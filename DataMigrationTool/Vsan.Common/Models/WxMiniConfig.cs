using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Models
{
    /// <summary>
    /// 微信小程序配置
    /// </summary>
    public class WxMiniConfig
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; } = "wxb83c76151924068e";
        /// <summary>
        /// App秘钥
        /// </summary>
        public string AppSecret { get; set; } = "a8ad08f9cc61bf99a4a197d227235305";

        /// <summary>
        /// 微信消息模板
        /// </summary>
        public  Dictionary<string,WxMessageTemplate> MessageTemplates =new Dictionary<string, WxMessageTemplate>
        {
            ["LiZhi"]=
            new WxMessageTemplate
            {
                Code="LiZhi",//离职通知	
                TemplateId="oK-gUbVmq7DeziISRUiu9zF7FC1K470fRsN2VqT9wZE",
                Page="/pages/index/index?type=wode",
                ParamCount=4//离职人员、离职岗位、离职公司、离职日期
            },
            ["YaoQingJieGuo"]=
            new WxMessageTemplate
            {
                Code="YaoQingJieGuo",//邀请结果提醒	
                TemplateId="2OjfeHdaiZ8LghZp8HVAD3R_syQfaSFBg1alU2K4Yhs",
                Page="/pages/index/index?type=yqhy",
                ParamCount=4//邀请结果、受邀者、注册时间、奖励
            },
            ["MianShiYaoQing"]=
            new WxMessageTemplate
            {
                Code="MianShiYaoQing",//面试邀请通知	
                TemplateId="iS6riiKn2-0FftYYHJwnFU_WnrKAWM_Rq8TSOuM8nGQ",
                Page="/pages/index/index?type=yptz",
                ParamCount=5,//邀约公司、公司地址、面试时间、岗位名称、姓名
            },
            ["ShuiDianZhangDan"]=
            new WxMessageTemplate
            {
                Code="ShuiDianZhangDan",//水电账单通知	
                TemplateId="nGmZXlDYLn5oNq3UITSer-hkfW6cr6ArCBZ7Vs97vvo",
                Page="/pages/index/index?type=sdf",
                ParamCount=2//用量周期、费用合计
            },
            ["KaoQin"]=
            new WxMessageTemplate
            {
                Code="KaoQin",//考勤通知
                TemplateId="DmOfHOigp2cEwniqL9qwN43jPIcr9t7YysE_tMN0owo",
                Page="/pages/index/index?type=wdkq",
                ParamCount=2//考勤时间、姓名
            },
            ["XinZi"]=
             new WxMessageTemplate
            {
                Code="XinZi",//薪资明细通知	
                TemplateId="H5ijIJdy7v0OBoHyMvVD7c2jZAg_jOktIO4GAZIC52A",
                Page="/pages/index/index?type=xzcx",
                ParamCount=2//工资月份、门店名称
            },
            ["RuZhi"]=
              new WxMessageTemplate
            {
                Code="RuZhi",//入职确认通知	
                TemplateId="Lrt__raeqSrwi51U64pi-sltBUv12OJqDUxiGailUMM",
                Page="/pages/index/index?type=wode",
                ParamCount=3//入职时间、职位名称、用户姓名
            },
            ["TiXianShiBai"]=
               new WxMessageTemplate
            {
                Code="TiXianShiBai",//提现失败通知	
                TemplateId="hKpZqWV7VoIUIWcAvwKQBMVo7NfUGnfzNvqj54C3xyk",
                Page="/pages/index/index?type=wdqb",
                ParamCount=4//原因、备注、提现金额、提现时间
            },
            ["TiXianDaoZhang"]=
               new WxMessageTemplate
            {
                Code="TiXianDaoZhang",//提现到账通知	
                TemplateId="ou4vXN1_1SrsCukr0hV_lNhbYMZ-uhV91k4zJnVJV2s",
                Page="/pages/index/index?type=wdqb",
                ParamCount=4//到账时间、到账金额、提现至、备注
            },

        };



    }
}
