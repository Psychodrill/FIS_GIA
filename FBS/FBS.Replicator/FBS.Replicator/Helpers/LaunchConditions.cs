using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBS.Replicator.Helpers
{
    public struct LaunchConditions
    {
        private const int ReplicationDefaultMinYear = 2011;
        private static int ReplicationMinYear { get { return ReplicationDefaultMinYear - 2; } }
        private static int ReplicationMaxYear { get { return DateTime.Now.Year + 2; } }

        public bool UseHelp { get; set; }
        public bool UseRepair { get; set; }
        public bool ERBDToFBS { get; set; }
        public bool FBSToGVUZ { get; set; }
        public bool LoadCompositions2015 { get; set; }
        public bool LoadCompositions2016Plus { get; set; }
        public bool EnableDetailedLog { get; set; }

        public List<Guid> DebugIdList { get; set; }
        public List<int> Years { get; set; }

        public LaunchConditions(string[] args)
        {
            this.UseHelp = false;
            this.UseRepair = false;
            this.ERBDToFBS = true;
            this.FBSToGVUZ = true;
            this.LoadCompositions2015 = false;
            this.LoadCompositions2016Plus = true;
            this.EnableDetailedLog = false;

            this.DebugIdList= new List<Guid>();
            this.Years = new List<int>();
            /*
            foreach (string arg in args)
            {
                if (arg.ToLower() == "help")
                {
                    this.UseHelp = true;
                    break;
                }
                else if ((arg.ToLower() == "year") || (arg.ToLower() == "year=now") || (arg.ToLower() == "year=current"))
                {
                    this.Years.Add(DateTime.Now.Year);
                }
                else if (arg.ToLower().StartsWith("year="))
                {
                    string yearsStr = arg.Substring("year=".Length);
                    foreach (string yearStr in yearsStr.Split(',', ';'))
                    {
                        int temp;
                        if (Int32.TryParse(yearStr, out temp))
                        {
                            if ((temp > ReplicationMaxYear) || (temp < ReplicationMinYear))
                            {
                                Logger.WriteLine("Год должен быть в диапазоне от " + ReplicationMinYear.ToString() + " до " + ReplicationMaxYear.ToString());
                                End("Синхронизация данных невозможна", true);

                                ERBDToFBS = false;
                                FBSToGVUZ = false;
                                return false;
                            }
                            else
                            {
                                yearsList.Add(temp);
                            }
                        }
                        else
                        {
                            Logger.WriteLine("Год задан неверно");
                            End("Синхронизация данных невозможна", true);
                            return false;
                        }
                    }
                }
                else if ((arg.ToLower() == "erbd") || (arg.ToLower() == "erbd=yes") || (arg.ToLower() == "erbd=1"))
                {
                    ERBDToFBS = true;
                }
                else if ((arg.ToLower() == "erbd=no") || (arg.ToLower() == "erbd=0"))
                {
                    ERBDToFBS = false;
                }
                else if ((arg.ToLower() == "rvi") || (arg.ToLower() == "rvi=yes") || (arg.ToLower() == "rvi=1"))
                {
                    FBSToGVUZ = true;
                }
                else if ((arg.ToLower() == "rvi=no") || (arg.ToLower() == "rvi=0"))
                {
                    FBSToGVUZ = false;
                }
                else if (arg.ToLower() == "repair")
                {
                    repair = true;
                }
                else if (arg.ToLower() == "compositions=2015")
                {
                    loadCompositions2015 = true;
                    loadCompositions2016Plus = false;
                }
                else if (arg.ToLower() == "compositions=2016")
                {
                    loadCompositions2015 = false;
                    loadCompositions2016Plus = true;
                }
                else if ((arg.ToLower() == "compositions=no") || (arg.ToLower() == "compositions=0"))
                {
                    loadCompositions2015 = false;
                    loadCompositions2016Plus = false;
                }
                else if ((arg.ToLower() == "detailslog") || (arg.ToLower() == "detailslog=yes") || (arg.ToLower() == "detailslog=1"))
                {
                    enableDetailedLog = true;
                }
                else if ((arg.ToLower() == "detailslog=no") || (arg.ToLower() == "detailslog=0"))
                {
                    enableDetailedLog = false;
                }
                else if (arg.ToLower().StartsWith("debugids="))
                {
                    enableDetailedLog = true;
                    string idsStr = arg.Substring("debugids=".Length);
                    foreach (string idStr in idsStr.Split(',', ';'))
                    {
                        Guid temp;
                        if (Guid.TryParse(idStr, out temp))
                        {
                            debugIdsList.Add(temp);
                        }
                    }
                }
            }

            if ((repair) || (help))
            {
                ERBDToFBS = false;
                FBSToGVUZ = false;
                loadCompositions2015 = false;
                loadCompositions2016Plus = false;
                debugIdsList.Clear();
            }

            if (!yearsList.Any())
            {
                for (int year = ReplicationDefaultMinYear; year <= DateTime.Now.Year; year++)
                {
                    yearsList.Add(year);
                }
            }

            if (!yearsList.Any(x => x == 2015))
            {
                loadCompositions2015 = false;
            }
            if (!yearsList.Any(x => x >= 2016))
            {
                loadCompositions2016Plus = false;
            }
            */

        }
    }
}
