$(document).ready(function () {
    $("input.check-box").click(function (event) {
        ChangeResultText(event.target);
    });

    $(".testResult").each(function () {
        ChangeResultTextForTestResult($(this));
    });
});

function ChangeResultText(target) {
    var testResult = $("#" + target.id).closest(".testResult");

    this.ChangeResultTextForTestResult(testResult);
};

function ChangeResultTextForTestResult(testResult) {
    var reportType = $("#ReportType").val();
    var subject = testResult.find(".subject").first().text();

    if (reportType == "Gym" && subject == "") {
        return;
    }

    var wasAbsent = false;
    testResult.find("input.check-box[name$='Absent']").each(function () {
        if ($(this).is(':checked')) {
            wasAbsent = true;
        }
    });

    var totalCounter = 0;
    var checkedCounter = 0;
    testResult.find("input.check-box[name$='Result']").each(function () {
        totalCounter++;

        if (wasAbsent) {
            $(this).removeAttr('checked');
        }
        else if ($(this).is(':checked')) {
            checkedCounter++;
        }
    });

    var resultTextBox = testResult.find("textarea").first();
    if (wasAbsent) {
        resultTextBox.val("");
    }
    else {
        // Correct the counter when there are less than 3 checkboxes
        if (checkedCounter > 0) {
            checkedCounter = checkedCounter + (3 - totalCounter);
        }

        var result1 = "We raden aan om buiten de schooluren regelmatig te gaan zwemmen. Eventueel enkele extra zwemlessen overwegen?!";
        var result2 = "Je bent op goede weg, nog een beetje oefenen en je wordt een echte zwemkampioen!";
        var result3 = "Schitterend, doe zo voort!";
        var resultNone = "GEEN ENKEL ONDERDEEL LUKT, we raden sterk aan om buiten de schooluren regelmatig te gaan zwemmen. Eventueel enkele extra zwemlessen overwegen?!";

        if (reportType == "Gym") {
            result1 = "Extra oefenen!";
            result2 = "Je bent op goede weg, blijven oefenen!";
            resultNone = "GEEN ENKEL ONDERDEEL LUKT!?";
        }

        switch (checkedCounter) {
            case 1:
                resultTextBox.val(result1);
                break;
            case 2:
                resultTextBox.val(result2);
                break;
            case 3:
                resultTextBox.val(result3);
                break;
            default:
                resultTextBox.val(resultNone);
                break;
        }
    }
};
