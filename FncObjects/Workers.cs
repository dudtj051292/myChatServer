using myChatServer.OracleConnector;
using myChetServer.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace FncObjects
{
    public class WorkerList
    {
        Dictionary<Dept, Worker> workerDic = null;

        public WorkerList()
        {
            if(workerDic == null)
            {
                workerDic = new Dictionary<Dept, Worker>();


                string sqlString = "SELECT NAME, DEPT, TITLE  FROM FNC_USER";

                DataTable USER = myOracleConnection.getSqlData(sqlString);

                sqlString = "SELECT DEPTNAME, DEPT FROM FNC_DEPT";
                DataTable DEPT = myOracleConnection.getSqlData(sqlString);


                foreach (DataRow row in USER.Rows)
                {
                    foreach (DataRow dr in DEPT.Rows)
                    {
                        if (Utils.getObjectToString(row["DEPT"]) 
                            == Utils.getObjectToString(dr["DEPT"].ToString()))
                        {
                            workerDic.Add(
                                          new Dept(Utils.getObjectToString(dr["DEPT"]), Utils.getObjectToString(dr["DEPTNAME"])),
                                          new Worker(Utils.getObjectToString(dr["NAME"]), Utils.getObjectToString(dr["DEPT"]), Utils.getObjectToString(dr["TITLE"]))
                                         );
                            continue;
                        }
                    }

                }



            }
        }


    }


    [System.Serializable]
    public class Worker
    {
        public Worker(string name, string dept, string title)
        {
            this.fncName = name;
            this.fncDept = dept;
            this.fncTitle = title;
        }
        public string fncName { get; set; }
        public string fncDept { get; set; }
        public string fncTitle { get; set; }

    }
    [System.Serializable]
    public class Dept
    {
        public Dept(string dept,string deptname)
        {
            this.dept = dept;
            this.deptname = deptname;
        }
        public string dept { get; set; }
        public string deptname { get; set; }

    }

}
