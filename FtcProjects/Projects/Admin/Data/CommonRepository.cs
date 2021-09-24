using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.DBContext;
using Admin.Models;


namespace Admin.Data
{
    public class CommonRepository : IDisposable
    {

        protected CommonRepository(ApplicationContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected ApplicationContext Context { get; }


        //Поиск ОО по наименованию
        public static List<InstitutionDetail> SearchInstitutions(string search, ApplicationContext db)
        {
            List<InstitutionDetail> list = new List<InstitutionDetail>();
            try
            {
                var insts = db.Institution
                    .Where(p => p.BriefName.ToLower().Contains(search.ToLower()) || p.FullName.ToLower().Contains(search.ToLower())).Select(p => new
                    {
                        InstitutionId = p.InstitutionId,
                        FullName = p.FullName,
                        BriefName = p.BriefName
                    })
                    .OrderBy(p => p.FullName)
                    .Take(20);
                if (insts != null)
                {
                    foreach (var inst in insts)
                    {
                        InstitutionDetail item = new InstitutionDetail();
                        item.InstitutionId = inst.InstitutionId;
                        item.BriefName = inst.BriefName;
                        item.FullName = inst.FullName;
                        if (item != null) { list.Add(item); }
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }

            return list;
        }

        //Список ОО
        public static List<InstitutionDetail> GetInstitutesList(ApplicationContext db)
        {
            List<InstitutionDetail> list = new List<InstitutionDetail>();
            try
            {
                var insts = db.Institution
                    .Where(p => p.StatusId != (int)Admin.Models.DBContext.OrganizationStatus.Liquidated && p.BriefName != "")
                    .Select(p => new
                    {
                        InstitutionId = p.InstitutionId,
                        FullName = p.FullName,
                        BriefName = p.BriefName
                    })
                    .OrderBy(p => p.BriefName);
                    //.OrderBy(p => p.InstitutionId);
                if (insts != null)
                {
                    foreach (var inst in insts)
                    {
                        InstitutionDetail item = new InstitutionDetail();
                        item.InstitutionId = inst.InstitutionId;
                        item.BriefName = inst.BriefName;
                        item.FullName = inst.FullName;
                        if (item != null) { list.Add(item); }
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }

            return list;
        }

        //Список годов кампаний
        public static List<string> GetYears(int institution_id, ApplicationContext db)
        {
            List<string> list = new List<string>();
            try
            {
                var years = db.Campaign
                        .Where(p => p.InstitutionId == institution_id && p.StatusId < 2)
                        .Select(p => new
                        {
                            Year = p.YearStart
                        })
                        .Distinct()
                        .OrderBy(p => p.Year);

                if (years != null)
                {
                    foreach (var item in years)
                    {
                        list.Add(item.Year.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return list;
        }


        ////Список кампаний по году
        //public static List<CampaignViewModel> GetCampaignList(int institution_id, int year, ApplicationContext db)
        //{
        //    List<CampaignViewModel> list = new List<CampaignViewModel>();
        //    try
        //    {
        //        var campains = db.Campaign
        //                .Where(p => p.InstitutionId == institution_id && p.StatusId < 2 && p.YearStart == year)
        //                .Select(p => new
        //                {
        //                    CampaignId = p.CampaignId,
        //                    Name = p.Name,
        //                    YearStart = p.YearStart,
        //                    YearEnd = p.YearEnd
        //                })
        //                .Distinct();

        //        if (campains != null)
        //        {
        //            foreach (var item in campains)
        //            {
        //                CampaignViewModel campaing = new CampaignViewModel();
        //                campaing.CampaignId = item.CampaignId;
        //                campaing.Name = item.Name;
        //                campaing.YearStart = item.YearStart;
        //                campaing.YearEnd = item.YearEnd;

        //                if (campaing != null) { list.Add(campaing); }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string error = e.Message.ToString();
        //    }

        //    return list;
        //}


        public void Dispose()
        {
            Context?.Dispose();
        }


    }
}
