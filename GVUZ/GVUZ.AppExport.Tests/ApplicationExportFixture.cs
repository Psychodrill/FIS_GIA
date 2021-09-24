using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using GVUZ.AppExport.Services;
using Moq;
using NUnit.Framework;

namespace GVUZ.AppExport.Tests
{
    [TestFixture]
    public class ApplicationExportFixture
    {
        [Test]
        [Ignore()]
        public void ExportDtoFromDatabase()
        {
            var token = new CancellationTokenSource();
            var cn = ConfigurationManager.ConnectionStrings["AppExport"].ConnectionString;

            var loader = new DbApplicationExportLoader(cn, token.Token, 100, 587, 2014);
            long count = 0;
            var processed = new HashSet<long>();
            loader.ApplicationFetched += (sender, args) =>
                {
                    Assert.IsFalse(processed.Contains(args.Data.AppId));
                    processed.Add(args.Data.AppId);
                    count++;
                };

            var timer = new Stopwatch();
            timer.Start();
            loader.Load();
            timer.Stop();
            Console.WriteLine("Total apps: {0}", count);
            Console.WriteLine("Elapsed: {0}", timer.Elapsed);
            timer.Restart();
            var esrpid = loader.GetInstitutionEsrpId(587);
            timer.Stop();
            Console.WriteLine("Get esrp id = {0} in {1}", esrpid, timer.Elapsed);
        }

        [Test]
        [Ignore()]
        public void ExportDtoFromEntities()
        {
            var loader = new ApplicationExportLoader(587, 2014);
            long count = 0;
            var processed = new HashSet<long>();
            loader.ApplicationFetched += (sender, args) =>
            {
                Assert.IsFalse(processed.Contains(args.Data.AppId));
                processed.Add(args.Data.AppId);
                count++;
            };

            var timer = new Stopwatch();
            timer.Start();
            loader.Load();
            timer.Stop();
            Console.WriteLine("Total apps: {0}", count);
            Console.WriteLine("Elapsed: {0}", timer.Elapsed);
            timer.Restart();
            var esrpid = loader.GetInstitutionEsrpId(587);
            timer.Stop();
            Console.WriteLine("Get esrp id = {0} in {1}", esrpid, timer.Elapsed);
        }

        [Test]
        [Ignore()]
        public void ExportDto()
        {
            var loaderMock = new Mock<IApplicationExportLoader>();
            loaderMock.Setup(x => x.Load()).Callback(() =>
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        var eargs = new ApplicationExportEventArgs(new ApplicationExportDto
                            {
                                AppId = i,
                                RegistrationDate = DateTime.Now,
                                StatusId = i*2,
                                LastDenyDate = i == 2 ? (DateTime?)null : DateTime.Now.AddDays(-1*i),
                                FinSourceAndEduForms = new HashSet<ApplicationExportFinSourceDto>
                                    {
                                        new ApplicationExportFinSourceDto
                                            {
                                                CommonBeneficiaryDocTypeId = 111,
                                                DirectionId = 222,
                                                EducationFormId = 333,
                                                EducationLevelId = 444,
                                                FinanceSourceId = 555,
                                                IsForBeneficiary = 666,
                                                Number = "appnum" + i,
                                                OrderTypeId = 777,
                                                UseBeneficiarySubject = 888,
                                                EntranceTestResults = new HashSet<ApplicationExportEntranceTestDto>
                                                    {
                                                        new ApplicationExportEntranceTestDto
                                                            {
                                                                EntranceTestResultId = 1,
                                                                EntranceTestTypeId = 2,
                                                                ResultSourceTypeId = 3,
                                                                ResultValue = 12.6m
                                                            },
                                                        new ApplicationExportEntranceTestDto
                                                            {
                                                                EntranceTestResultId = 1,
                                                                EntranceTestTypeId = 2,
                                                                ResultSourceTypeId = 3,
                                                                ResultValue = 12.6m
                                                            },
                                                        new ApplicationExportEntranceTestDto
                                                            {
                                                                EntranceTestResultId = 1,
                                                                EntranceTestTypeId = 2,
                                                                ResultSourceTypeId = 3,
                                                                ResultValue = 12.6m
                                                            }
                                                    }
                                            }
                                    }
                            });
                        loaderMock.Raise(x => x.ApplicationFetched += null, eargs);
                    }
                });

            var exporter = new ApplicationXmlExporter(loaderMock.Object, 42);
            using (var ms = new MemoryStream())
            {
                exporter.Export(ms);

                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }
    }
}
