using System;

namespace Compass.Andro.Dtos.Plans
{
    public class IssueDto : BaseDto
    {
        #region 基本属性
        //todo：记录问题时向相关方发送电子邮件
        public Guid MainPlanId { get; set; }//关联主计划
        public Guid ReporterId { get; set; }//记录问题的人
        //问题描述
        public IssueTitle_e Title { get; set; }//标题
        public string Content { get; set; }//内容
        public string ContentUrl { get; set; }//上传附件，多文件 
        #endregion


        #region 状态属性
        public Guid? ResponderId { get; set; }//解决问题的责任人，由项目经理指定
        public DateTime? Deadline { get; set; }//责令日期

        //todo：问题解决时向相关方发送电子邮件
        public string Solution { get; set; }//解决方案
        public string SolutionUrl { get; set; }//上传得附件，多文件
        public DateTime? CloseTime { get; set; }//问题解决的时间
        public bool IsClosed { get; set; }//是否结束 
        #endregion



        #region 查询后扩展的属性
        public UserDto Reporter { get; set; }
        public UserDto Responder { get; set; }
        #endregion
    }
}

