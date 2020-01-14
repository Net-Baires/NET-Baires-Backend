﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetBaires.Data.DomainEvents;

namespace NetBaires.Data
{
    public class GroupCodeBadge : Entity
    {
        public GroupCode GroupCode { get; set; }
        public Badge Badge { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public GroupCodeBadge(GroupCode groupCode, Badge badge)
        {
            GroupCode = groupCode;
            Badge = badge;
        }

        public GroupCodeBadge()
        {

        }
    }

    public class GroupCode : Entity
    {
        public string Code { get; set; }
        public string Detail { get; set; }
        public bool Open { get; set; } = true;
        public List<GroupCodeMember> Members { get; set; } = new List<GroupCodeMember>();
        public int EventId { get; set; }
        public List<GroupCodeBadge> GroupCodeBadges { get; set; } = new List<GroupCodeBadge>();
        public Event Event { get; set; }

        public GroupCode(string detail)
        {
            Open = false;
            Detail = detail;
            Code = RandomHelper.RandomString(8);
        }

        public void AddMember(Member member, string code)
        {
            if (Members.All(x => x.MemberId != member.Id) && code.ToUpper() == Code.ToUpper())
                Members.Add(new GroupCodeMember
                {
                    Member = member,
                    GroupCode = this
                });
        }
        public void AddMember(Member member)
        {
            if (!Members.Any(x => x.MemberId == member.Id))
                Members.Add(new GroupCodeMember
                {
                    Member = member,
                    GroupCode = this
                });
        }
        public void RemoveMember(Member member)
        {
            if (Members.Any(x => x.MemberId == member.Id))
                Members.RemoveAll(x => x.MemberId == member.Id);
        }

        public DomainResponse AssignBadge(Badge badge)
        {
            foreach (var groupCodeMember in Members)
            {
                badge.Members.Add(new BadgeMember
                {
                    BadgeId = badge.Id,
                    MemberId = groupCodeMember.MemberId
                });
                AddDomainEvent(new AssignedBadgeToAttendance(groupCodeMember.Member, badge));
            }
            GroupCodeBadges.Add(new GroupCodeBadge(this, badge));
            return DomainResponse.Ok();
        }
    }
}