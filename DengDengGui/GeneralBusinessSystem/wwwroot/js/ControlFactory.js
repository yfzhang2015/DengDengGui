/*************************************
 * 控件创建器
 ************************************/

$.extend({
    includePath: '',
    include: function (file) {
        var files = typeof file == "string" ? [file] : file;
        for (var i = 0; i < files.length; i++) {          
            var name = files[i]//.replace(/^s|s$/g, "");
            console.log(name)
            var att = name.split('.');         
            var ext = att[att.length - 1].toLowerCase();         
            var isCSS = ext == "css";
            var tag = isCSS ? "link" : "script";
            var attr = isCSS ? " type='text/css' rel='stylesheet' " : " language='javascript' type='text/javascript' ";
            var link = (isCSS ? "href" : "src") + "='" + $.includePath + name + "'";
            if ($(tag + "[" + link + "]").length == 0) {            
                document.write("<" + tag + attr + link + "></" + tag + ">");
            }
        }
    }
});
$.include(['../lib/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js', '../lib/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js','../lib/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css']);


//日期时间创造器
function createDateTime(field) {
    var dateTimeHtml = '\
     <div class="form-group">\
         <div class="input-group date form_datetime" data-date="" data-date-format="yyyy-MM-dd HH:ii:00" data-link-field="dtp_input1">\
              <input id="satrt_rq" class="form-control" size="19" type="text" value="" readonly>\
              <span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>\
         </div>\
         <input type="hidden" id="dtp_input1" value="" />\
     </div>';
    return dateTimeHtml;
}

//日期创造器
function createDate() {
    var dateHtml = '\
    <div class="form-group">\
        <div  class="input-group date form_date" data-data="" data-link-format="yyyy-mm-dd" data-date-format="yyyy MM dd" data-link-field="dtp_input2">\
            <input id="end_rq" class="form-control" size="16" type="text" value="" readonly>\
                <span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>\
                </div>\
            <input type="hidden" id="dtp_input2" value="" />\
        </div>';
    return dateHtml;
}

//文本创造器
function createText() {
}

//单选框创造器
function createRadio() {
}

//复选框创造器
function createCheckbox() {
}

//下拉列表创造器
function createSelect() {
}

//自动下拉创造器
function createAutoComplete() {
}

//弹框列表创造器
function createListDialog() {
}