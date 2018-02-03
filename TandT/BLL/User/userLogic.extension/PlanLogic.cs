using System;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using MongoManager;
using MongoManager.CollectionModels;
using MongoManager.Context;
using C5;


namespace BLL.userLogic.extension
{
    public static class PlanLogic
    {
        public static void AddNewPlan(this UserLogic UL, string id, Plan plan)
        {
            List<Plan> list = new List<Plan>();
            list.Add(plan);
            DataManager.Users.AddPlans(id, list.ToArray());
        }
        public static List<Plan> GetOpenPlan(this UserLogic UL, string id)
        {
            List<Plan> list = new List<Plan>();
            List<Plan> plan = DataManager.Users.ReadFull(id).Plans;
            DateTime time = DateTime.Now;
            foreach (var o in plan)
            {
                if (o.DateClose > time) list.Add(o);
            }
            return list;
        }
        public static List<Plan> GetTenOpenPlan(this UserLogic UL, string id, int k)
        {
            List<Plan> list = new List<Plan>();
            Plan[] plan = DataManager.Users.ReadFull(id).Plans.ToArray();
            IntervalHeap<Plan> Q = new IntervalHeap<Plan>();
            Q.AddAll(plan);

            return list;
        }
        public static void EditPlan(this UserLogic UL, string id, Plan oldPlan,Plan newPlan)
 
        {
            List<Plan> list = new List<Plan>();
            list.Add(oldPlan);
            DataManager.Users.DeletePlans(id,list.ToArray());
            list.Clear();
            list.Add(newPlan);
            DataManager.Users.AddPlans(id,list.ToArray());

        }
        public static void DeletePlan(this UserLogic UL, string id, Plan plan)
        {
            List<Plan> list = new List<Plan>();
            list.Add(plan);
            DataManager.Users.DeletePlans(id,list.ToArray());
        }
    }

}
