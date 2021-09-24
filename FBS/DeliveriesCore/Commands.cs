using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DeliveriesCore
{
    public static class CommandSolutions
    {
        static Dictionary<string, Command> Map;
        public static SqlCommand GetCommand( string deliveryId,string deliveryType)
        {
            if (!Map.ContainsKey(deliveryType))
                return null;
            Command Cmd = Map[deliveryType];
            return Cmd.Get(deliveryId);
        }

        static CommandSolutions()
        {
            Map = new Dictionary<string, Command>();
            Map.Add("Users", new UsersCommand());
            Map.Add("NotRegistredOrgs", new NotRegistredOrgsCommand());
        }
    }

    public abstract class Command
    {
        public abstract SqlCommand Get(string arg);
    }

    public class UsersCommand : Command
    {
        public override SqlCommand Get(string deliveryId)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = @"SELECT 
                                Acc.EMail AS EMail,Acc.LastName AS LastName,Acc.FirstName AS FirstName,Acc.PatronymicName AS PatronymicName,'' AS OrgName
                                FROM Delivery Del 
                                RIGHT JOIN DeliveryRecipients Rec ON Del.Id=Rec.DeliveryId 
                                RIGHT JOIN GroupAccount GrAcc ON GrAcc.GroupId=Rec.RecipientCode 
                                RIGHT JOIN Account Acc ON Acc.Id=GrAcc.AccountId 
                                WHERE Del.Id = @Id";
            Cmd.Parameters.AddWithValue("Id", deliveryId);
            return Cmd;
        }
    }

    public class NotRegistredOrgsCommand : Command
    {
        public override SqlCommand Get(string arg)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = @"SELECT 
                                Org.EMail AS EMail,Org.FullName AS OrgName,'' AS LastName,'' AS FirstName,'' AS PatronymicName
                                FROM Organization2010 Org 
                                WHERE Org.Id NOT IN 
                                (
                                SELECT ISNULL(OrgReq.OrganizationId,0) FROM OrganizationRequest2010 OrgReq
                                )";
            return Cmd;
        }
    }
}
