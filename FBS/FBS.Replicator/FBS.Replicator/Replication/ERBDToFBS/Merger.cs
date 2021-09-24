using System;
using System.Collections.Generic;
using System.Linq;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;
using FBS.Common;

namespace FBS.Replicator.Replication.ERBDToFBS
{
    public static class ERBDToFBSMerger
    {
        public static bool Merge(Dictionary<ParticipantId, FBSParticipant> fbsParticipants, Dictionary<CertificateId, FBSCertificate> fbsCertificatesWithoutParticipant, Dictionary<CertificateMarkId, FBSCertificateMark> fbsCertificateMarksWithoutParticipant, Dictionary<ParticipantId, ERBDParticipant> erbdParticipants, Dictionary<CertificateId, ERBDCertificate> erbdCertificatesWithoutParticipant)
        {
            try
            {
                foreach (ParticipantId fbsParticipantId in fbsParticipants.Keys)
                {
                    if (erbdParticipants.ContainsKey(fbsParticipantId))
                    {
                        FBSParticipant fbsParticipant = fbsParticipants[fbsParticipantId];
                        ERBDParticipant erbdParticipant = erbdParticipants[fbsParticipantId];

                        if (fbsParticipant.HashCode != erbdParticipant.HashCode)
                        {
                            MarkForUpdate(erbdParticipant);
                        }
                        else
                        {
                            MarkForNotChange(fbsParticipant);
                            MarkForNotChange(erbdParticipant);
                        }

                        //Certificates
                        foreach (FBSCertificate fbsCertificate in fbsParticipant.Certificates)
                        {
                            ERBDCertificate erbdCertificate = erbdParticipant.Certificates.FirstOrDefault(x => x.Id.Equals(fbsCertificate.Id));
                            if (erbdCertificate != null)
                            {
                                if (fbsCertificate.HashCode != erbdCertificate.HashCode)
                                {
                                    MarkForUpdate(erbdCertificate);
                                }
                                else
                                {
                                    MarkForNotChange(fbsCertificate);
                                    MarkForNotChange(erbdCertificate);
                                }

                                //Marks
                                foreach (FBSCertificateMark fbsCertificateMark in fbsCertificate.CertificateMarks)
                                {
                                    ERBDCertificateMark erbdCertificateMark = erbdCertificate.GetMarkById(fbsCertificateMark.Id);
                                    if (erbdCertificateMark != null)
                                    {
                                        if (fbsCertificateMark.HashCode != erbdCertificateMark.HashCode)
                                        {
                                            MarkForUpdate(erbdCertificateMark);
                                        }
                                        else
                                        {
                                            MarkForNotChange(fbsCertificateMark);
                                            MarkForNotChange(erbdCertificateMark);
                                        }
                                    }
                                    else
                                    {
                                        MarkForDelete(fbsCertificateMark);
                                    }
                                }

                                foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                                {
                                    if (fbsCertificate.GetMarkById(erbdCertificateMark.Id) == null)
                                    {
                                        MarkForInsert(erbdCertificateMark);
                                    }
                                }

                                //CancelledCertificates
                                if (fbsCertificate.CancelledCertificate != null)
                                {
                                    if (erbdCertificate.CancelledCertificate != null)
                                    {
                                        if (fbsCertificate.CancelledCertificate.HashCode != erbdCertificate.CancelledCertificate.HashCode)
                                        {
                                            MarkForUpdate(erbdCertificate.CancelledCertificate);
                                        }
                                        else
                                        {
                                            MarkForNotChange(fbsCertificate.CancelledCertificate);
                                            MarkForNotChange(erbdCertificate.CancelledCertificate);
                                        }
                                    }
                                    else
                                    {
                                        MarkForDelete(fbsCertificate.CancelledCertificate);
                                    }
                                }

                                if ((erbdCertificate.CancelledCertificate != null) && (fbsCertificate.CancelledCertificate == null))
                                {
                                    MarkForInsert(erbdCertificate.CancelledCertificate);
                                }
                            }
                            else
                            {
                                MarkForDelete(fbsCertificate);
                            }
                        }

                        foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                        {
                            if (!fbsParticipant.Certificates.Any(x => x.Id.Equals(erbdCertificate.Id)))
                            {
                                MarkForInsert(erbdCertificate);
                            }
                        }

                        //Marks
                        foreach (FBSCertificateMark fbsCertificateMark in fbsParticipant.CertificateMarks)
                        {
                            ERBDCertificateMark erbdCertificateMark = erbdParticipant.CertificateMarks.FirstOrDefault(x => x.Id.Equals(fbsCertificateMark.Id));
                            if (erbdCertificateMark != null)
                            {
                                if (fbsCertificateMark.HashCode != erbdCertificateMark.HashCode)
                                {
                                    MarkForUpdate(erbdCertificateMark);
                                }
                                else
                                {
                                    MarkForNotChange(fbsCertificateMark);
                                    MarkForNotChange(erbdCertificateMark);
                                }
                            }
                            else
                            {
                                MarkForDelete(fbsCertificateMark);
                            }
                        }

                        foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                        {
                            if (!fbsParticipant.CertificateMarks.Any(x => x.Id.Equals(erbdCertificateMark.Id)))
                            {
                                MarkForInsert(erbdCertificateMark);
                            }
                        }
                    }
                    else
                    {
                        MarkForDelete(fbsParticipants[fbsParticipantId]);
                    }
                }

                foreach (ERBDParticipant erbdParticipant in erbdParticipants.Values)
                {
                    if (!fbsParticipants.ContainsKey(erbdParticipant.Id))
                    {
                        MarkForInsert(erbdParticipant);
                    }

                    foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                    {
                        if (fbsCertificatesWithoutParticipant.ContainsKey(erbdCertificate.Id))
                        {
                            FBSCertificate fbsCertificateWithoutParticipant = fbsCertificatesWithoutParticipant[erbdCertificate.Id];

                            MarkForUpdate(erbdCertificate);
                            MarkForNotChange(fbsCertificateWithoutParticipant);
                        }

                        foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                        {
                            if (fbsCertificateMarksWithoutParticipant.ContainsKey(erbdCertificateMark.Id))
                            {
                                FBSCertificateMark fbsCertificateMarkWithoutParticipant = fbsCertificateMarksWithoutParticipant[erbdCertificateMark.Id];

                                MarkForUpdate(erbdCertificateMark);
                                MarkForNotChange(fbsCertificateMarkWithoutParticipant);
                            }
                        }
                    }

                    foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                    {
                        if (fbsCertificateMarksWithoutParticipant.ContainsKey(erbdCertificateMark.Id))
                        {
                            FBSCertificateMark fbsCertificateMarkWithoutParticipant = fbsCertificateMarksWithoutParticipant[erbdCertificateMark.Id];

                            MarkForUpdate(erbdCertificateMark);
                            MarkForNotChange(fbsCertificateMarkWithoutParticipant);
                        }
                    }
                }

                foreach (CertificateId fbsCertificateId in fbsCertificatesWithoutParticipant.Keys)
                {
                    if (erbdCertificatesWithoutParticipant.ContainsKey(fbsCertificateId))
                    {
                        FBSCertificate fbsCertificate = fbsCertificatesWithoutParticipant[fbsCertificateId];
                        ERBDCertificate erbdCertificate = erbdCertificatesWithoutParticipant[fbsCertificateId];

                        if (fbsCertificate.HashCode != erbdCertificate.HashCode)
                        {
                            MarkForUpdate(erbdCertificate);
                        }
                        else
                        {
                            MarkForNotChange(fbsCertificate);
                            MarkForNotChange(erbdCertificate);
                        }
                    }
                    else
                    {
                        MarkForDelete(fbsCertificatesWithoutParticipant[fbsCertificateId]);
                    }
                }

                foreach (CertificateId erbdCertificateId in erbdCertificatesWithoutParticipant.Keys)
                {
                    if (!fbsCertificatesWithoutParticipant.ContainsKey(erbdCertificateId))
                    {
                        MarkForInsert(erbdCertificatesWithoutParticipant[erbdCertificateId]);
                    }
                }

                foreach (FBSCertificateMark fbsCertificateMark in fbsCertificateMarksWithoutParticipant.Values)
                {
                    MarkForDelete(fbsCertificateMark);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ОШИБКА сравнения данных: " + ex.Message + " (" + ex.StackTrace + ")");
                return false;
            }
        }

        private static void MarkForInsert(ERBDParticipant erbdParticipant)
        {
            SetAction(erbdParticipant, ERBDToFBSActions.Insert);
            foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
            {
                if (erbdCertificateMark.Action != ERBDToFBSActions.None)
                {
                    SetAction(erbdCertificateMark, ERBDToFBSActions.Insert);
                }
            }
            foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
            {
                SetAction(erbdCertificate, ERBDToFBSActions.Insert);
                foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                {
                    if (erbdCertificateMark.Action != ERBDToFBSActions.None)
                    {
                        SetAction(erbdCertificateMark, ERBDToFBSActions.Insert);
                    }
                }
                if (erbdCertificate.CancelledCertificate != null)
                {
                    SetAction(erbdCertificate.CancelledCertificate, ERBDToFBSActions.Insert);
                }
            }
        }

        private static void MarkForInsert(ERBDCertificate erbdCertificate)
        {
            if (erbdCertificate.Participant != null)
            {
                SetAction(erbdCertificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(erbdCertificate, ERBDToFBSActions.Insert);
            foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
            {
                if (erbdCertificateMark.Action != ERBDToFBSActions.None)
                {
                    SetAction(erbdCertificateMark, ERBDToFBSActions.Insert);
                    if (erbdCertificateMark.Participant != null)
                    {
                        SetAction(erbdCertificateMark.Participant, ERBDToFBSActions.UpdateRelated);
                    }
                }
            }
            if (erbdCertificate.CancelledCertificate != null)
            {
                SetAction(erbdCertificate.CancelledCertificate, ERBDToFBSActions.Insert);
            }
        }

        private static void MarkForInsert(ERBDCertificateMark erbdCertificateMark)
        {
            if (erbdCertificateMark.Participant != null)
            {
                SetAction(erbdCertificateMark.Participant, ERBDToFBSActions.UpdateRelated);
            }
            else if ((erbdCertificateMark.Certificate != null) && (erbdCertificateMark.Certificate.Participant != null))
            {
                SetAction(erbdCertificateMark.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }

            SetAction(erbdCertificateMark, ERBDToFBSActions.Insert);
        }

        private static void MarkForInsert(ERBDCancelledCertificate erbdCancelledCertificate)
        {
            if (erbdCancelledCertificate.Certificate.Participant != null)
            {
                SetAction(erbdCancelledCertificate.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(erbdCancelledCertificate, ERBDToFBSActions.Insert);
        }

        private static void MarkForUpdate(ERBDParticipant erbdParticipant)
        {
            SetAction(erbdParticipant, ERBDToFBSActions.Update);
        }

        private static void MarkForUpdate(ERBDCertificate erbdCertificate)
        {
            if (erbdCertificate.Participant != null)
            {
                SetAction(erbdCertificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(erbdCertificate, ERBDToFBSActions.Update);
        }

        private static void MarkForUpdate(ERBDCertificateMark erbdCertificateMark)
        {
            if (erbdCertificateMark.Participant != null)
            {
                SetAction(erbdCertificateMark.Participant, ERBDToFBSActions.UpdateRelated);
            }
            else if ((erbdCertificateMark.Certificate != null) && (erbdCertificateMark.Certificate.Participant != null))
            {
                SetAction(erbdCertificateMark.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(erbdCertificateMark, ERBDToFBSActions.Update);
        }

        private static void MarkForUpdate(ERBDCancelledCertificate erbdCancelledCertificate)
        {
            if (erbdCancelledCertificate.Certificate.Participant != null)
            {
                SetAction(erbdCancelledCertificate.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(erbdCancelledCertificate, ERBDToFBSActions.Update);
        }

        private static void MarkForDelete(FBSParticipant fbsParticipant)
        {
            SetAction(fbsParticipant, ERBDToFBSActions.Delete);
            foreach (FBSCertificateMark fbsCertificateMark in fbsParticipant.CertificateMarks)
            {
                SetAction(fbsCertificateMark, ERBDToFBSActions.Delete);
            }
            foreach (FBSCertificate fbsCertificate in fbsParticipant.Certificates)
            {
                SetAction(fbsCertificate, ERBDToFBSActions.Delete);
                foreach (FBSCertificateMark fbsCertificateMark in fbsCertificate.CertificateMarks)
                {
                    SetAction(fbsCertificateMark, ERBDToFBSActions.Delete);
                }
                if (fbsCertificate.CancelledCertificate != null)
                {
                    SetAction(fbsCertificate.CancelledCertificate, ERBDToFBSActions.Delete);
                }
            }
        }

        private static void MarkForDelete(FBSCertificate fbsCertificate)
        {
            if (fbsCertificate.Participant != null)
            {
                SetAction(fbsCertificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(fbsCertificate, ERBDToFBSActions.Delete);
            foreach (FBSCertificateMark fbsCertificateMark in fbsCertificate.CertificateMarks)
            {
                SetAction(fbsCertificateMark, ERBDToFBSActions.Delete);
                if (fbsCertificateMark.Participant != null)
                {
                    SetAction(fbsCertificateMark.Participant, ERBDToFBSActions.UpdateRelated);
                }
            }
            if (fbsCertificate.CancelledCertificate != null)
            {
                SetAction(fbsCertificate.CancelledCertificate, ERBDToFBSActions.Delete);
            }
        }

        private static void MarkForDelete(FBSCertificateMark fbsCertificateMark)
        {
            if (fbsCertificateMark.Participant != null)
            {
                SetAction(fbsCertificateMark.Participant, ERBDToFBSActions.UpdateRelated);
            }
            else if ((fbsCertificateMark.Certificate != null) && (fbsCertificateMark.Certificate.Participant != null))
            {
                SetAction(fbsCertificateMark.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(fbsCertificateMark, ERBDToFBSActions.Delete);
        }

        private static void MarkForDelete(FBSCancelledCertificate fbsCancelledCertificate)
        {
            if (fbsCancelledCertificate.Certificate.Participant != null)
            {
                SetAction(fbsCancelledCertificate.Certificate.Participant, ERBDToFBSActions.UpdateRelated);
            }
            SetAction(fbsCancelledCertificate, ERBDToFBSActions.Delete);
        }

        private static void MarkForNotChange(ERBDCertificateMark erbdCertificateMark)
        {
            SetAction(erbdCertificateMark, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(ERBDCertificate erbdCertificate)
        {
            SetAction(erbdCertificate, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(ERBDCancelledCertificate erbdCancelledCertificate)
        {
            SetAction(erbdCancelledCertificate, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(ERBDParticipant erbdParticipant)
        {
            SetAction(erbdParticipant, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(FBSCertificateMark fbsCertificateMark)
        {
            SetAction(fbsCertificateMark, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(FBSCertificate fbsCertificate)
        {
            SetAction(fbsCertificate, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(FBSCancelledCertificate fbsCancelledCertificate)
        {
            SetAction(fbsCancelledCertificate, ERBDToFBSActions.None);
        }

        private static void MarkForNotChange(FBSParticipant fbsParticipant)
        {
            SetAction(fbsParticipant, ERBDToFBSActions.None);
        }

        private static void SetAction(FBSParticipant fbsParticipant, ERBDToFBSActions action)
        {
            fbsParticipant.Action = MergeActions(fbsParticipant.Action, action);
            // DetailedLogger.WriteLine(String.Format("Участник ЕГЭ ФБС (ParticipantID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", fbsParticipant.Id.ParticipantID, fbsParticipant.Id.UseYear, fbsParticipant.Id.REGION, action.ToString(), fbsParticipant.Action.ToString()));
        }

        private static void SetAction(ERBDParticipant erbdParticipant, ERBDToFBSActions action)
        {
            erbdParticipant.Action = MergeActions(erbdParticipant.Action, action);
            //  DetailedLogger.WriteLine(String.Format("Участник ЕГЭ ЕРБД (ParticipantID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", erbdParticipant.Id.ParticipantID, erbdParticipant.Id.UseYear, erbdParticipant.Id.REGION, action.ToString(), erbdParticipant.Action.ToString()));
        }

        private static void SetAction(FBSCertificate fbsCertificate, ERBDToFBSActions action)
        {
            fbsCertificate.Action = MergeActions(fbsCertificate.Action, action);
            //DetailedLogger.WriteLine(String.Format("Свидетельство ФБС (CertificateID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", fbsCertificate.Id.CertificateID, fbsCertificate.Id.UseYear, fbsCertificate.Id.REGION, action.ToString(), fbsCertificate.Action.ToString()));
        }

        private static void SetAction(ERBDCertificate erbdCertificate, ERBDToFBSActions action)
        {
            erbdCertificate.Action = MergeActions(erbdCertificate.Action, action);
            //DetailedLogger.WriteLine(String.Format("Свидетельство ЕРБД (CertificateID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", erbdCertificate.Id.CertificateID, erbdCertificate.Id.UseYear, erbdCertificate.Id.REGION, action.ToString(), erbdCertificate.Action.ToString()));
        }

        private static void SetAction(FBSCertificateMark fbsCertificateMark, ERBDToFBSActions action)
        {
            fbsCertificateMark.Action = MergeActions(fbsCertificateMark.Action, action);
            //DetailedLogger.WriteLine(String.Format("Балл ФБС (CertificateMarkID = {0}, CertificateFK = {1}, UseYear = {2}, REGION= {3}): устанавливаемое действие: {4}; фактическое действие: {5}", fbsCertificateMark.Id.CertificateMarkID, fbsCertificateMark.Id.CertificateFK, fbsCertificateMark.Id.UseYear, fbsCertificateMark.Id.REGION, action.ToString(), fbsCertificateMark.Action.ToString()));
        }

        private static void SetAction(ERBDCertificateMark erbdCertificateMark, ERBDToFBSActions action)
        {
            erbdCertificateMark.Action = MergeActions(erbdCertificateMark.Action, action);
            //DetailedLogger.WriteLine(String.Format("Балл ЕРБД (CertificateMarkID = {0}, CertificateFK = {1}, UseYear = {2}, REGION= {3}): устанавливаемое действие: {4}; фактическое действие: {5}", erbdCertificateMark.Id.CertificateMarkID, erbdCertificateMark.Id.CertificateFK, erbdCertificateMark.Id.UseYear, erbdCertificateMark.Id.REGION, action.ToString(), erbdCertificateMark.Action.ToString()));
        }

        private static void SetAction(FBSCancelledCertificate fbsCancelledCertificate, ERBDToFBSActions action)
        {
            fbsCancelledCertificate.Action = MergeActions(fbsCancelledCertificate.Action, action);
            // DetailedLogger.WriteLine(String.Format("Запись об аннулировании ФБС (CertificateID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", fbsCancelledCertificate.Id.CertificateID, fbsCancelledCertificate.Id.UseYear, fbsCancelledCertificate.Id.REGION, action.ToString(), fbsCancelledCertificate.Action.ToString()));
        }

        private static void SetAction(ERBDCancelledCertificate erbdCancelledCertificate, ERBDToFBSActions action)
        {
            erbdCancelledCertificate.Action = MergeActions(erbdCancelledCertificate.Action, action);
            //DetailedLogger.WriteLine(String.Format("Запись об аннулировании ЕРБД (CertificateID = {0}, UseYear = {1}, REGION= {2}): устанавливаемое действие: {3}; фактическое действие: {4}", erbdCancelledCertificate.Id.CertificateID, erbdCancelledCertificate.Id.UseYear, erbdCancelledCertificate.Id.REGION, action.ToString(), erbdCancelledCertificate.Action.ToString()));
        }

        private static ERBDToFBSActions MergeActions(ERBDToFBSActions currentAction, ERBDToFBSActions newAction)
        {
            if (currentAction == ERBDToFBSActions.Undefined)
                return newAction;
            if (currentAction == newAction)
                return newAction;

            if (((currentAction == ERBDToFBSActions.None) && (newAction == ERBDToFBSActions.UpdateRelated))
                || ((currentAction == ERBDToFBSActions.UpdateRelated) && (newAction == ERBDToFBSActions.None)))
                return ERBDToFBSActions.UpdateRelated;

            if (((currentAction == ERBDToFBSActions.None) && (newAction == ERBDToFBSActions.Update))
                || ((currentAction == ERBDToFBSActions.Update) && (newAction == ERBDToFBSActions.None)))
                return ERBDToFBSActions.Update;

            if (((currentAction == ERBDToFBSActions.Update) && (newAction == ERBDToFBSActions.Insert))
                || ((currentAction == ERBDToFBSActions.Insert) && (newAction == ERBDToFBSActions.Update)))
                return ERBDToFBSActions.Update;
            if (((currentAction == ERBDToFBSActions.Update) && (newAction == ERBDToFBSActions.Delete))
                || ((currentAction == ERBDToFBSActions.Delete) && (newAction == ERBDToFBSActions.Update)))
                return ERBDToFBSActions.Update;
            if (((currentAction == ERBDToFBSActions.Insert) && (newAction == ERBDToFBSActions.Delete))
              || ((currentAction == ERBDToFBSActions.Delete) && (newAction == ERBDToFBSActions.Insert)))
                throw new Exception("Конфликт операций");

            if ((currentAction == ERBDToFBSActions.UpdateRelated)
                && ((newAction == ERBDToFBSActions.Delete) || (newAction == ERBDToFBSActions.Update) || (newAction == ERBDToFBSActions.Insert)))
                return newAction;

            if ((newAction == ERBDToFBSActions.UpdateRelated)
              && ((currentAction == ERBDToFBSActions.Delete) || (currentAction == ERBDToFBSActions.Update) || (currentAction == ERBDToFBSActions.Insert)))
                return currentAction;

            throw new Exception("Конфликт операций");
        }
    }
}
