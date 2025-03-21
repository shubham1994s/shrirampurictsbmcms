﻿using SwachBharat.CMS.Bll.ViewModels.ChildModel.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachBharat.CMS.Bll.Repository.GridRepository.Grid
{
    public class LocationGridRepository : IDataTableRepository
    {
        IEnumerable<SBALocationGridRow> dataSet;
        DashBoardRepository objRep = new DashBoardRepository();

        public LocationGridRepository(long wildcard, string SearchString, DateTime? fdate, DateTime? tdate, int userId, int appId,string Emptype)
        {
            dataSet = objRep.GetLocatioData(wildcard, SearchString, fdate, tdate, userId, appId, Emptype);
        }

        public string GetDataTabelJson(string sortColumn, string sortColumnDir, string draw, string length, string searchValue, string start)
        {
            var json = dataSet.GetDataTableJson(sortColumn, sortColumnDir, draw, length, searchValue, start);
            return json;
        }

    }
}
