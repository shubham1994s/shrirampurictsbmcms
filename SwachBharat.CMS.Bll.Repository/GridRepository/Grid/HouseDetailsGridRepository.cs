using SwachBharat.CMS.Bll.Repository.GridRepository;
using System.Collections.Generic;
using System.Collections.Specialized;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Model;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Grid;
using SwachBharat.CMS.Bll.Repository.GridRepository.Grid;
using System;
using System.Web.Script.Serialization;
using System.Linq;

namespace SwachBharat.CMS.Bll.Repository.RepositoryGrid.Grid
{
    public class HouseDetailsGridRepository : IDataTableRepository
    {
        IEnumerable <SBAHouseDetailsGridRow> dataSet;
        DashBoardRepository objRep = new DashBoardRepository();

        public HouseDetailsGridRepository(long wildcard, string SearchString, int appId, string sortColumn = "", string sortColumnDir = "", string draw = "", string length = "", string start = "")
        {
            dataSet = objRep.GetHouseDetailsData(wildcard, SearchString, appId, sortColumn, sortColumnDir, draw, length, start);
        }

        public string GetDataTabelJson(string sortColumn, string sortColumnDir, string draw, string length, string searchValue, string start)
        {
            //var json = dataSet.GetDataTableJson(sortColumn, sortColumnDir, draw, length, searchValue, start);
            //return json;
            int recordsTotal = 0;
            if (dataSet != null && dataSet.Count() > 0)
            {
                recordsTotal = dataSet.First().totalRowCount;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var result = serializer.Serialize(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataSet });
            return result;
        }

     
    }
}