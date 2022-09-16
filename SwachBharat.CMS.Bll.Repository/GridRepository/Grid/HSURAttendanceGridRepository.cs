﻿using SwachBharat.CMS.Bll.ViewModels.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachBharat.CMS.Bll.Repository.GridRepository.Grid
{
    public class HSURAttendanceGridRepository : IDataTableRepository
    {
        IEnumerable<HSUREmployeeAttendanceDetails> dataset;

        DashBoardRepository objRep = new DashBoardRepository();

        public HSURAttendanceGridRepository(long wildcard, string SearchString, DateTime? fdate, DateTime? tdate, int userId, string ClientId, int appId, string sortColumn = "", string sortColumnDir = "", string draw = "", string length = "", string start = "")
        {
            dataset = objRep.GetHSURAttendanceDetails(wildcard, SearchString, fdate, tdate, userId, ClientId, appId, sortColumn, sortColumnDir, draw, length, start);
        }

        public string GetDataTabelJson(string sortColumn, string sortColumnDir, string draw, string length, string searchValue, string start)
        {
            var json = dataset.GetDataTableJson(sortColumn, sortColumnDir, draw, length, searchValue, start);
            return json;
        }
    }
}
