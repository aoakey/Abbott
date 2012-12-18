$(document).ready(function () {

    //date controls
    $(".date").each(function (index, item) {

        var id = "#ctrl_" + $(item).find(".id").text();
        var day = $(item).find("._day").text();
        var month = $(item).find("._month").text();
        var year = $(item).find("._year").text();
        var minYear = $(item).find(".minYear").text();
        var maxYear = $(item).find(".maxYear").text();

        PopulateDays(year, month, id);
        PopulateYears(minYear, maxYear, id);

        $(id + " .day").val(day);
        $(id + " .month").val(month);
        $(id + " .year").val(year);

        $(id + " .year").change(function () {
            var y = $(id + " .year").val();
            var m = $(id + " .month").val();
            PopulateDays(y, m, id);
            SetDateValue(id);
        });

        $(id + " .month").change(function () {
            var y = $(id + " .year").val();
            var m = $(id + " .month").val();
            PopulateDays(y, m, id);
            SetDateValue(id);
        });

        $(id + " .day").change(function () {
            SetDateValue(id);
        });

        $(id + " .day").val(day);
        $(id + " .month").val(month);
        $(id + " .year").val(year);

        SetDateValue(id);

    });

    //time controls
    $(".time").each(function (index, item) {
        var id = "#ctrl_" + $(item).find(".id").text();
        var hours = $(item).find("._hour").text();
        var minutes = $(item).find("._min").text();
        var div = $(item).find("._div").text();
        PopulateTime(hours, minutes, div, id);

        $(id + " .hours").change(function () {
            SetTimeValue(id);
        });

        $(id + " .minutes").change(function () {
            SetTimeValue(id);
        });

        SetTimeValue(id);
    });    

});

//date function
function DaysInMonth(y, m) {
    return new Date(y, m, 0).getDate();
}

function PopulateDays(year, month, id) {
    var days = DaysInMonth(year, month);
    $(id + " .day").empty();
    for (n = 1; n <= days; n++) {
        var o = new Option(n, n);
        $(o).html(n);
        $(id + " .day").append(o);        
    }
}

function PopulateYears(min, max, id) {
    $(id + " .year").empty();

    for (n = min; n <= max; n++) {
        var o = new Option(n, n);
        $(o).html(n);
        $(id + " .year").append(o);
    }
}

function SetDateValue(id) {
    var d = $(id + " .day").val();
    var m = $(id + " .month").val();
    var y = $(id + " .year").val();
    var date = zeroFill(m,2) + "/" + zeroFill(d,2) + "/" + y;
    $(id + " .DateValue").val(date);
}

//time functions
function PopulateTime(h, m, div, id) {

    $(id + " .hours").empty();
    for (i = 0; i <= 23; i++) {
        if (i < 12) {
            value = i.toString() + " AM";
        }
        else if (i == 12) {
            value = i.toString() + " PM";
        }
        else {
            value = (i - 12).toString() + " PM";
        }
        var o = new Option(i, i);
        $(o).html(value);
        $(id + " .hours").append(o);
    }

    $(id + " .minutes").empty();
    for (j = 0; j <= 59; j++) {
        if (j % div == 0) {
            var o = new Option(j, j);
            $(o).html(zeroFill(j, 2));
            $(id + " .minutes").append(o);
        }
    }

    $(id + " .hours").val(h);
    $(id + " .minutes").val(m);

}

function SetTimeValue(id) {
    var h = $(id + " .hours").val();
    var m = $(id + " .minutes").val();
    var time = zeroFill(h,2) + ":" + zeroFill(m,2)
    $(id + " .TimeValue").val(time);
}

function zeroFill(num, len) { return (Array(len).join("0") + num).slice(-len); }

