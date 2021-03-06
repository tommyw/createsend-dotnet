﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace createsend_dotnet
{
    public class Segment
    {
        public string SegmentID { get; set; }

        public Segment(string segmentID)
        {
            SegmentID = segmentID;
        }

        public static string Create(string listID, string title, SegmentRules rules)
        {
            return HttpHelper.Post<Dictionary<string, object>, string, ErrorResult<RuleErrorResults>>(
                string.Format("/segments/{0}.json", listID), null,
                new Dictionary<string, object>() 
                { 
                    { "ListID", listID }, 
                    { "Title", title }, 
                    { "Rules", rules } 
                });
        }

        public void Update(string title, SegmentRules rules)
        {
            HttpHelper.Put<Dictionary<string, object>, string>(
                string.Format("/segments/{0}.json", SegmentID), null,
                new Dictionary<string, object>() 
                { 
                    { "Title", title }, 
                    { "Rules", rules } 
                });
        }

        public void AddRule(string subject, List<string> clauses)
        {
            HttpHelper.Post<Dictionary<string, object>, string>(
                string.Format("/segments/{0}/rules.json", SegmentID), null,
                new Dictionary<string, object>() 
                { 
                    { "Subject", subject }, 
                    { "Clauses", clauses } 
                });
        }

        public PagedCollection<SubscriberDetail> Subscribers(DateTime fromDate, int page, int pageSize, string orderField, string orderDirection)
        {
            NameValueCollection queryArguments = new NameValueCollection();
            queryArguments.Add("date", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
            queryArguments.Add("page", page.ToString());
            queryArguments.Add("pagesize", pageSize.ToString());
            queryArguments.Add("orderfield", orderField);
            queryArguments.Add("orderdirection", orderDirection);

            return HttpHelper.Get<PagedCollection<SubscriberDetail>>(string.Format("/segments/{0}/active.json", SegmentID), queryArguments);
        }

        public SegmentDetail Details()
        {
            return HttpHelper.Get<SegmentDetail>(string.Format("/segments/{0}.json", SegmentID), null);
        }

        public void ClearRules()
        {
            HttpHelper.Delete(string.Format("/segments/{0}/rules.json", SegmentID), null);
        }

        public void Delete()
        {
            HttpHelper.Delete(string.Format("/segments/{0}.json", SegmentID), null);
        }
    }
}
