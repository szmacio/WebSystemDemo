var menuSetting = {
    async: {
        enable: true,
        url: "/Apm/GetTrees",
        autoParam: ["id", "name=n", "level=lv"],//向后台请求数据时的参数
        otherParam: { "otherParam": "tree" }
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    check: {
        enable: true,
        chkboxType: { "Y": "s", "N": "s" },
        //onclick: nodeClick  
    },
    callback: {
        onClick: onClick
    }
};

function onClick(e, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("ApmTree");
    var nodes = zTree.getSelectedNodes();
    var no = nodes[0].id;
    $("#no").val(no);
    $("#oldno").val(no);
    $.ajax({
        url: "/Apm/GetDate/" + no,           //请求的地址（格式）：/Control/Action
        type: "POST",
        dataType: "json",//发送请求的方式
        success: function (data) {       //发送成功后的回调函数   $("#inputID").val("123123123");
            $("#no").val(data.no);
            $("#cinvcname").val(data.cinvcname);
            $("#parentcode").val(data.parentcode);
            $("#Memo").val(data.Memo);
            $("#va01").val(data.va01);
            $("#va02").val(data.va02);
            $("#va03").val(data.va03);
            $("#va04").val(data.va04);
            $("#in01").val(data.in01);
            $("#in02").val(data.in02);
            $("#in03").val(data.in03);
            $("#de01").val(data.de01);
            $("#de02").val(data.de02);
            $("#de03").val(data.de03);
        }
    });
}
//保存
function saveData() {
    var no = $("#no").val();
    var oldno = $("#oldno").val();
    var parentcode = $("#parentcode").val();
    var cinvcname = $("#cinvcname").val();
    var isParent;
    var zTree = $.fn.zTree.getZTreeObj("ApmTree");
    var nodes = zTree.getSelectedNodes();
    treeNode = nodes[0];
    //是否有选中点，没有选中点跳出警示吧
    if (!treeNode) {
        parent.layer.msg("没有选中任何点，无法保存");
        return;
    }
    var data = {
        no: no,
        oldno: oldno,
        parentcode: parentcode,
        cinvcname: cinvcname,
        Memo: $("#Memo").val(),
        va01: $("#va01").val(),
        va02: $("#va02").val(),
        va03: $("#va03").val(),
        va04: $("#va04").val(),
        in01: $("#in01").val(),
        in02: $("#in02").val(),
        in03: $("#in03").val(),
        de01: $("#de01").val(),
        de02: $("#de02").val(),
        de03: $("#de03").val(),
        isParent: isParent,
    };
    var jsdata = JSON.stringify(data);
    $.ajax({
        url: "/Apm/Add",
        type: "POST",
        data: jsdata,
        dataType: "JSON",
        contentType: "application/json",
        success: function (res) {
            parent.layer.msg(res);
        }
    });
    addNode(data)
}
//添加节点
function addNode(e) {
    var zTree = $.fn.zTree.getZTreeObj("ApmTree");
    if (!zTree) {
        var zNodes = [];
        zTree = $.fn.zTree.init($("#ApmTree"), setting, zNodes);
    }
    nodes = zTree.getSelectedNodes(),
    treeNode = nodes[0];
    //判断是同级添加节点，还是下级添加节点
    //如果当前编号长度大于当前长度，就表示是下级添加
    if (e.no.length > e.oldno.length) {
        treeNode = zTree.addNodes(treeNode, { id: e.no, pId: e.parentcode, isParent: e.isParent, name: e.no + " " + e.cinvcname });
    }
    else if (e.no.length == e.oldno.length)//如果当前编号长度等于当前长度，就表示是同级添加
    {
        pNode = treeNode.getParentNode();
        treeNode = zTree.addNodes(pNode, { id: e.no, pId: e.parentcode, isParent: e.isParent, name: e.no + " " + e.cinvcname });
    }
};
//删除节点
function delData() {
    parent.layer.confirm("确认要删除这条数据？", {
        btn: ['确认', '取消'] //按钮
    }, function () {
        var no = $("#oldno").val();
        var isParent;
        var zTree = $.fn.zTree.getZTreeObj("ApmTree");
        var nodes = zTree.getSelectedNodes();
        treeNode = nodes[0];
        //是否有选中点，没有选中点跳出警示吧
        if (!treeNode) {
            parent.layer.msg("没有选中任何点，无法保存");
            return;
        }
        if (typeof (treeNode.children) != "undefined") {
            parent.layer.msg("已经存在子节点，无法删除，请删除所有子节点后再试");
            return;
        }
        var data = {
            no: no
        };
        $.ajax({
            url: "/Apm/Delete",
            type: "POST",
            data: JSON.stringify(data),
            dataType: "JSON",
            contentType: "application/json",
            success: function (res) {
                parent.layer.msg(res);
            }
        });
        zTree.removeNode(treeNode, false);
    }, function () { })
}

//编辑节点
function editData() {
    var no = $("#no").val();
    var oldno = $("#oldno").val();
    var parentcode = $("#parentcode").val();
    var cinvcname = $("#cinvcname").val();
    var isParent;
    var zTree = $.fn.zTree.getZTreeObj("ApmTree");
    var nodes = zTree.getSelectedNodes();
    treeNode = nodes[0];
    //是否有选中点，没有选中点跳出警示吧
    if (!treeNode) {
        parent.layer.msg("没有选中任何点，无法保存");
        return;
    }
    var data = {
        no: no,
        oldno: oldno,
        parentcode: parentcode,
        cinvcname: cinvcname,
        Memo: $("#Memo").val(),
        va01: $("#va01").val(),
        va02: $("#va02").val(),
        va03: $("#va03").val(),
        va04: $("#va04").val(),
        in01: $("#in01").val(),
        in02: $("#in02").val(),
        in03: $("#in03").val(),
        de01: $("#de01").val(),
        de02: $("#de02").val(),
        de03: $("#de03").val(),
        isParent: isParent,
    };
    $.ajax({
        url: "/Apm/Edit",
        type: "POST",
        data: JSON.stringify(data),
        dataType: "JSON",
        contentType: "application/json",
        success: function (res) {
            if (res) {
                treeNode.name = oldno + " " + cinvcname;
                zTree.updateNode(treeNode, false);
                parent.layer.msg("编辑成功！");
            }
            else {
                parent.layer.msg("编辑不成功！，提醒编号不能修改");
            }
        }
    });

}
$(document).ready(function () {
    $.fn.zTree.init($("#ApmTree"), menuSetting);
    $("#btnSave").click(saveData);
    $("#btnDel").click(delData);
    $("#btnEdit").click(editData);
});