﻿$(document).ready(function () {
    var UserId = $('#selectnumber').val();
    
   



    $.ajax({
        type: "post",
        url: "/Location/UserList",
        data: { userId: UserId, },
        datatype: "json",
        traditional: true,
        success: function (data) {
            district = '<option value="-1">Select Employee</option>';
            for (var i = 0; i < data.length; i++) {
                district = district + '<option value=' + data[i].Value + '>' + data[i].Text + '</option>';
            }
            //district = district + '</select>';
            $('#selectnumber').html(district);
        }
    });
    $("#demoGrid").DataTable({
        "sDom": "ltipr",
        "order": [[12, "desc"]],
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "pageLength": 10,

        "ajax": {
            "url": "/Datable/GetJqGridJson?rn=GarbageHouse",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
        [{
            "targets": [0],
            "visible": false,
            "searchable": false
        },
         {
            "targets": [12],
            "visible": false,
            "searchable": false
        },
         {
             "targets": [8],
             "visible": true,

             "render": function (data, type, full, meta) {
                 if (full["gpBeforImage"] != null) {
                     return "<div style='cursor:pointer;display:inline-flex;'  onclick=PopImages(this)><img id='beforeimage' alt='Photo Not Found'  src='" + full["gpBeforImage"] +
                         "' style='height:35px;width:35px;cursor:pointer;margin-left:0px;'></img><img id='afterimage' alt='Photo Not Found'  src='" + full["gpAfterImage"] +
                         "' style='display:none;height:35px;width:35px;cursor:pointer;margin-left:0px;'></img><span><ul class='dt_pop'  style='margin:2px -5px -5px -5px; padding:0px;list-style:none;display:none;'><li  class='li_date datediv' >" + full["attandDate"] + "</li><li  class='li_date1 datediv' >" + full["gpBeforImageTime"] + "</li><li class='addr-length' style='margin:0px 0px 0px 10px;'>"
                         + full["Address"] + "</li><li style='display:none' class='li_title' >Before Image </li></ul></span></div>";
                 }
                 else {

                     return "<img alt='Photo Not Found' onclick='noImageNotification()' src='/Images/default.png' style='height:35px;width:35px;cursor:pointer;'></img>";
                 }
             },
         },
             {
                 "targets": [9],
                 "visible": true,

                 "render": function (data, type, full, meta) {
                     if (full["gpAfterImage"] != null) {
                         return "<div style='cursor:pointer;display:inline-flex;'  onclick=PopImages(this)><img id='beforeimage' alt='Photo Not Found'  src='" + full["gpBeforImage"] +
                             "' style='display:none;height:35px;width:35px;cursor:pointer;margin-left:0px;'></img><img id='afterimage' alt='Photo Not Found'  src='" + full["gpAfterImage"] +
                             "' style='height:35px;width:35px;cursor:pointer;margin-left:0px;'></img><span><ul class='dt_pop'  style='margin:2px -5px -5px -5px; padding:0px;list-style:none;display:none;'><li  class='li_date datediv' >" + full["attandDate"] + "</li><li  class='li_date1 datediv' >" + full["gpBeforImageTime"] + "</li><li class='addr-length' style='margin:0px 0px 0px 10px;'>"
                             + full["Address"] + "</li><li style='display:none' class='li_title' >After Image </li></ul></span></div>";
                     }
                     else {

                         return "<img alt='Photo Not Found' onclick='noImageNotification()' src='/Images/default.png' style='height:35px;width:35px;cursor:pointer;'></img>";
                     }
                 },
             },


              {
                  "targets": [3],
                
                  "visible":true,

                  "render": function (data, type, full, meta) {

                            if(full["wastetype"] == "DW1") {
                               return "<div class='circle' style='height: 20px;width: 20px;background-color: #0462EA;border-radius: 50%;vertical-align: middle;display: inline-flex;'></div> (Dry Waste)";

    }
                     else if (full["wastetype"] == "WW1") {
                               return "<div class='circle' style='height: 20px;width: 20px;background-color: #186634;border-radius: 50%;vertical-align: middle;display: inline-flex;'></div> (Wet Waste)";

    }

                           else  if (full["type1"] == "0") {
                         return "<div class='circle' style='height: 20px;width: 20px;background-color: #f44336;border-radius: 50%;    vertical-align: middle;display: inline-flex;'></div> (Mixed Garbage)";
                     }
                     else if (full["type1"] == "1") {
                         return "<div class='circle' style='height: 20px;width: 20px;background-color: #388e3c;border-radius: 50%;vertical-align: middle;display: inline-flex;'></div> (Segregated Garbage)";

                     }
                     else if (full["type1"] == "2") {
                         return "<div class='circle' style='height: 20px;width: 20px;background-color: #fe9436;border-radius: 50%;vertical-align: middle;display: inline-flex;'></div> (Garbage not Received)";
                        
                     }

                 

                     else   {
                         return "<div class='circle' style='height: 20px;width: 20px;background-color: #0086c3;border-radius: 50%;vertical-align: middle;display: inline-flex;'></div> (Garbage type not specified)";

                     }
                   
                 },
             },
        ],

        "columns": [
              { "data": "Id", "name": "Id", "autoWidth": false },
              { "data": "attandDate", "name": "attandDate", "autoWidth": false },
              { "data": "Employee", "name": "Employee", "autoWidth": false }, 
            { "data": "type1", "name": "type1", "autoWidth": false },
              { "data": "UserName", "name": "UserName", "autoWidth": false },
              { "data": "Address", "name": "Address", "autoWidth": false }, 
              { "data": "VehicleNumber", "autoWidth": false },
              { "data": "Note", "autoWidth": false },
              { "data": "gpBeforImage", "name": "gpBeforImage", "autoWidth": false },
               { "data": "gpAfterImage", "name": "gpAfterImage", "autoWidth": false },
               { "data": "ReferanceId", "name": "ReferanceId", "autoWidth": false },
                 { "data": "batteryStatus", "name": "batteryStatus", "autoWidth": false },
            { "data": "gcDate", "name": "gcDate", "autoWidth": false },
            

        ]
    });


});

function DownloadQRCode(Id) {
    window.location.href = "/GarbagePoint/Export?Id=" + Id;
};

function showInventoriesGrid() {
    Search();
}
function noImageNotification() {
    document.getElementById("snackbar").innerHTML = "Image not uploaded...";
    var x = document.getElementById("snackbar");
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

function PopImages(cel) {

    $('#myModal_Image').modal('toggle');

    var addr = $(cel).find('.addr-length').text();
    var date = $(cel).find('.li_date').text();
    var imgsrc = $(cel).find('img').attr('src');

    var date1 = $(cel).find('.li_date1').text();
    var imgsrc1 = $(cel).find('#afterimage').attr('src');

    var head = $(cel).find('.li_title').text();
    jQuery("#latlongData").text(addr);
    jQuery("#dateData").text(date);
    jQuery("#imggg").attr('src', imgsrc);

    jQuery("#dateData1").text(date1);
    jQuery("#imggg1").attr('src', imgsrc1);
    //jQuery("#latlongData").text(cellValue);
    jQuery("#header_data").html(head);
}

function Search() {
    var txt_fdate, txt_tdate, Client, UserId;
    var name = [];
    var arr = [$('#txt_fdate').val(), $('#txt_tdate').val()];

    for (var i = 0; i <= arr.length - 1; i++) {
        name = arr[i].split("/");
        arr[i] = name[1] + "/" + name[0] + "/" + name[2];
    }

    txt_fdate = arr[0];
    txt_tdate = arr[1];
    UserId = $('#selectnumber').val();
    ZoneId = $('#ZoneId').val();
    WardId = $('#WardNo').val();
    AreaId = $('#AreaId').val();
    Client = " ";
    NesEvent = " ";
    var Product = "";
    var catProduct = "";
    var value = txt_fdate + "," + txt_tdate + "," + UserId + "," + $("#s").val() + "," + ZoneId + "," + WardId + "," + AreaId;//txt_fdate + "," + txt_tdate + "," + UserId + "," + Client + "," + NesEvent + "," + Product + "," + catProduct + "," + 1;
    // alert(value );
    oTable = $('#demoGrid').DataTable();
    oTable.search(value).draw();
    oTable.search("");
    document.getElementById('USER_ID_FK').value = -1;
}