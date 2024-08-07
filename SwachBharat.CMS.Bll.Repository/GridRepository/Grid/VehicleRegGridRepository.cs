﻿using SwachBharat.CMS.Bll.Repository.GridRepository;
using SwachBharat.CMS.Bll.Repository.GridRepository.Grid;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Grid;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace SwachBharat.CMS.Bll.Repository.GridRepository.Grid
{
    public class VehicleRegGridRepository : IDataTableRepository
    {
        IEnumerable<SBAVehicleRegGridRow> dataSet;
        DashBoardRepository objRep = new DashBoardRepository();


        public VehicleRegGridRepository(long wildcard, string SearchString, int appId)
        {
            dataSet = objRep.GetVehicleRegData(wildcard, SearchString, appId);
        }

        public string GetDataTabelJson(string sortColumn, string sortColumnDir, string draw, string length, string searchValue, string start)
        {
            var json = dataSet.GetDataTableJson(sortColumn, sortColumnDir, draw, length, searchValue, start);
            return json;
        }
    }
}
