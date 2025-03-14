﻿using SwachBharat.CMS.Bll.ViewModels.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachBharat.CMS.Bll.Repository.GridRepository.Grid
{
   public class HouseBunchGridRepository : IDataTableRepository
    {
        DashBoardRepository objRep = new DashBoardRepository();
        IEnumerable<SBAHouseBunchGridRow> dataset;

        public HouseBunchGridRepository(long wildcard, string SearchString, int AppId)
        {
            dataset = objRep.GetHouseBunchData(wildcard, SearchString, AppId);
        }

        public string GetDataTabelJson(string sortColumn, string sortColumnDir, string draw, string length, string searchValue, string start)
        {
            var json = dataset.GetDataTableJson(sortColumn, sortColumnDir, draw, length, searchValue, start);
            return json;
        }
    }
}
