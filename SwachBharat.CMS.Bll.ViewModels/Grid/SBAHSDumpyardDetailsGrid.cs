﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwachBharat.CMS.Bll.ViewModels.Grid
{
    public class SBAHSDumpyardDetailsGrid
    {
        public int dumpId { get; set; }

        public int userId { get; set; }
        public string ReferanceId { get; set; }
        public string Name { get; set; }
        public string HouseLat { get; set; }
        public string HouseLong { get; set; }
        public string QRCodeImage { get; set; }
        public string modifiedDate { get; set; }
        public Nullable<bool> QRStatus { get; set; }
        public string QRStatusDate { get; set; }

        public byte[] BinaryQrCodeImage { get; set; }

    }
}
