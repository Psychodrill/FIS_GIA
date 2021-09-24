using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.GVUZ;
using FBS.Replicator.Entities.FBS;
using FBS.Common;

namespace FBS.Replicator.Replication.FBSToGVUZ
{
    public static class FBSToGVUZMerger
    {
        public static bool Merge(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons, Dictionary<int, IEnumerable<GVUZPerson>> gvuzPersons)
        {
            foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
            {
                foreach (FBSPerson fbsPerson in fbsPersonsByName)
                {
                    foreach (FBSPerson fbsSiblingPerson in fbsPersonsByName.Where(x => NamesAreEqual(fbsPerson, x) && DocumentsAreEqual(fbsPerson.Document, x.Document)))
                    {
                        if (fbsSiblingPerson == fbsPerson)
                            continue;

                        fbsPerson.AddSibling(fbsSiblingPerson);

                        if ((fbsPerson.BirthDay != fbsSiblingPerson.BirthDay) || (fbsPerson.Sex != fbsSiblingPerson.Sex))
                        {
                            Logger.WriteLine("КОНФЛИКТ сравнения данных: в БД ФБС найдены записи с совпадающими ФИО и номером документа, но различными датами рождения или полом (идентификатор ФБС (1): ParticipantID = " + fbsPerson.Id.ParticipantID.ToString() + ", UseYear = " + fbsPerson.Id.UseYear.ToString() + ", REGION=" + fbsPerson.Id.REGION.ToString() + "; идентификатор ФБС (2): ParticipantID = " + fbsSiblingPerson.Id.ParticipantID.ToString() + ", UseYear = " + fbsSiblingPerson.Id.UseYear.ToString() + ", REGION=" + fbsSiblingPerson.Id.REGION.ToString() + ")");
                        }
                    }

                    List<GVUZPerson> matchingGvuzPersons = new List<GVUZPerson>();
                    if (gvuzPersons.ContainsKey(fbsPerson.NameHashCode))
                    {
                        IEnumerable<GVUZPerson> matchingGvuzPersonsByName = gvuzPersons[fbsPerson.NameHashCode].Where(x => NamesAreEqual(fbsPerson, x));
                        foreach (GVUZPerson matchingGvuzPersonByName in matchingGvuzPersonsByName)
                        {
                            foreach (GVUZIdentityDocument gvuzIdentityDocument in matchingGvuzPersonByName.Documents)
                            {
                                if (DocumentsAreEqual(fbsPerson.Document, gvuzIdentityDocument))
                                {
                                    matchingGvuzPersons.Add(matchingGvuzPersonByName);
                                    break;
                                }
                            }
                        }
                    }

                    if (matchingGvuzPersons.Count == 0)
                    {
                        SetAction(fbsPerson, FBSToGVUZActions.Insert, null);
                    }
                    else
                    {
                        GVUZPerson singleMatchingPerson = null;
                        if (matchingGvuzPersons.Count == 1)
                        {
                            singleMatchingPerson = matchingGvuzPersons.First();
                        }
                        else
                        {
                            singleMatchingPerson = matchingGvuzPersons.First(x => x.CreateDate == matchingGvuzPersons.Min(y => y.CreateDate));
                            Logger.WriteLine("КОНФЛИКТ сравнения данных: найдено более 1 совпадающей записи о физическом лице (идентификаторы РВИ: " + String.Join("; ", matchingGvuzPersons.Select(x => x.Id.ToString())) + "). Будет обработана запись с наименьшим значением CreateDate (идентификатор РВИ: " + singleMatchingPerson.Id + ")");
                        }

                        if ((!fbsPerson.PersonId.HasValue) || (fbsPerson.PersonId.Value != singleMatchingPerson.Id))
                        {
                            SetAction(fbsPerson, FBSToGVUZActions.Link, singleMatchingPerson.Id);
                        } 
                    }
                }
            }
            foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
            {
                foreach (FBSPerson fbsPerson in fbsPersonsByName)
                {
                    SetAction(fbsPerson, FBSToGVUZActions.None, null);
                }
            }

            return true;
        }

        private static void SetAction(FBSPerson fbsPerson, FBSToGVUZActions action, int? personId)
        {
            if (fbsPerson.Action == FBSToGVUZActions.Undefined)
            {
                fbsPerson.Action = action;
                if (action != FBSToGVUZActions.None)
                {
                    fbsPerson.SetPersonId(personId);
                }
            }
        }

        private static bool NamesAreEqual(FBSPerson fbsPerson, GVUZPerson gvuzPerson)
        {
            return ((fbsPerson.NormNameStr == gvuzPerson.NormNameStr)
                && (fbsPerson.NormSurnameStr == gvuzPerson.NormSurnameStr)
                && (fbsPerson.NormSecondNameStr == gvuzPerson.NormSecondNameStr));
        }

        private static bool NamesAreEqual(FBSPerson fbsPerson1, FBSPerson fbsPerson2)
        {
            return ((fbsPerson1.NormNameStr == fbsPerson2.NormNameStr)
                && (fbsPerson1.NormSurnameStr == fbsPerson2.NormSurnameStr)
                && (fbsPerson1.NormSecondNameStr == fbsPerson2.NormSecondNameStr));
        }

        private static bool DocumentsAreEqual(FBSIdentityDocument fbsDocument, GVUZIdentityDocument gvuzDocument)
        {
            return ((fbsDocument.DocumentNumberStr == gvuzDocument.DocumentNumberStr)
                 && (fbsDocument.DocumentSeriesStr == gvuzDocument.DocumentSeriesStr));
        }

        private static bool DocumentsAreEqual(FBSIdentityDocument fbsDocument1, FBSIdentityDocument fbsDocument2)
        {
            return ((fbsDocument1.DocumentNumberStr == fbsDocument2.DocumentNumberStr)
                 && (fbsDocument1.DocumentSeriesStr == fbsDocument2.DocumentSeriesStr));
        }
    }
}
