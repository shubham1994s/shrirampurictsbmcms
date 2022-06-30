﻿$(document).ready(function () {

    $("#demoGrid").DataTable({
        "sDom": "ltipr",
        "order": [[0, "desc"]],
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "pageLength": 10,
        "oLanguage": {
            "sInfo": "Showing _START_ to _END_ ",// text you want show for info section
        },

        "ajax": {
            "url": "/Datable/GetJqGridJson?rn=AppAreaMap",
            "type": "POST",
            "datatype": "json"
        },

        "columnDefs":
            [{
                "targets": [],
                "visible": false,
                "searchable": false
            }],

        "columns": [

            { "data": "AppId", "name": "AppId", "autoWidth": false },
            { "data": "AppName", "name": "AppName", "autoWidth": false },
            { "render": function (data, type, full, meta) { return '<a  data-toggle="modal" class="tooltip1" style="cursor:pointer"  onclick="Edit(' + full["AppId"] + ')" ><i class="material-icons edit-icon">edit</i><span class="tooltiptext1">Edit</span> </a>'; }, "width": "10%" },
        ]
    });
});



function Edit(Id) {
    //alert("Aa");
    if (Id != null) {
        var url = "/HouseScanifyEmp/AddAppAreaMap?AppId=" + Id;

        window.location.href = url;

    }
};

function Search() {
    var value = ",,," + $("#s").val();//txt_fdate + "," + txt_tdate + "," + UserId + "," + Client + "," + NesEvent + "," + Product + "," + catProduct + "," + 1;
    // alert(value );
    oTable = $('#demoGrid').DataTable();
    oTable.search(value).draw();
    oTable.search("");
    document.getElementById('USER_ID_FK').value = -1;
}
