var ElementStatusHandler = function () {
    var DisableClassName = "BetTypeInactive";

    return {
        EnableElement: function (element) {
            if (typeof (element) != 'object') return;
            if (element.nodeType == 3) return; // text node

            var isDisabled = (element.className.indexOf(DisableClassName) != -1);
            if (element.tagName == "DIV" || element.tagName == "LABEL") {
                if (isDisabled) {
                    var previousValue = element.getAttribute("previousValue");
                    if (previousValue != null && typeof (previousValue) != 'undefined') {
                        element.setAttribute("previousValue", element.className);
                        element.className = previousValue;
                    }
                }
            }

            var count = element.childNodes.length;
            if (typeof (count) == 'undefined') return;
            for (var i = 0; i < count; i++) {
                if (element.childNodes[i].tagName == "INPUT" || element.childNodes[i].tagName == "SELECT") {
                    if (isDisabled) {
                        var previousValue = element.childNodes[i].getAttribute("previousValue");
                        if (previousValue != null && typeof (previousValue) != 'undefined') {
                            element.childNodes[i].setAttribute("previousValue", element.childNodes[i].disabled);                            
                            if (previousValue == "true" || previousValue == true) {
                                //element.childNodes[i].disabled = (previousValue == "true");
                                element.childNodes[i].setAttribute('disabled', 'disabled');
                            }
                            else {
                                element.childNodes[i].removeAttribute('disabled');
                            }
                        }
                    }
                }
                else {
                    ElementStatusHandler.EnableElement(element.childNodes[i]);
                }
            }
        },
        DisableElement: function (element) {
            if (typeof (element) != 'object') return;
            if (element.nodeType == 3) return; // text node

            var isDisabled = (element.className.indexOf(DisableClassName) != -1);

            if (element.tagName == "DIV" || element.tagName == "LABEL") {
                if (!isDisabled) {
                    element.setAttribute("previousValue", element.className);
                    element.className += " " + DisableClassName;
                };
            };

            var count = element.childNodes.length;
            if (typeof (count) == 'undefined') return;

            for (var i = 0; i < count; i++) {
                if (element.childNodes[i].tagName == "INPUT" || element.childNodes[i].tagName == "SELECT") {
                    var previousValue = element.childNodes[i].disabled;
                    if (!isDisabled || !element.childNodes[i].disabled) {
                        //element.childNodes[i].disabled = true;
                        element.childNodes[i].setAttribute('disabled', 'disabled');
                        element.childNodes[i].setAttribute("previousValue", previousValue);
                    };
                } else {
                    ElementStatusHandler.DisableElement(element.childNodes[i]);
                }
            }
        }
    }
} ();

/*
This function support 3 cases: error, success & warning
errCode < -1 : success (Note: -1 is a Agent system error)
errCode >= 50 && errCode <= 99 : warning
errCode : all of remaing ones are errors.
*/
function GenerateMessage(ArrCodes, ArrMsgs, ArrUserName, isAdd) {
    var isSucc = ageMsg.CheckMessageIsSucc2(ArrCodes);

    function getClass(errCode) {
        var className = '';
        if (errCode < -1) {
            className = 'EnrichSuccmsg';
        }
        else if (errCode >= 50 && errCode <= 99) {
            className = 'EnrichWarmsg';
        }
        else {
            className = 'EnrichErrmsg';
        }

        return className;
    }

    function InsertRow(tableId, classname, message) {
        var tr = document.createElement('tr');
        var td = document.createElement('td');
        td.className = classname;
        td.innerHTML = message;
        tr.appendChild(td);
        var tbody = document.getElementById(tableId).getElementsByTagName('tbody')[0];
        tbody.appendChild(tr);
    }

    if (isAdd) // Add
    {
        for (var j = 0; j < ArrUserName.length; j++) {
            InsertRow('tblMessage', 'EnrichUserName', ArrUserName[j]);
            for (var i = 0; i < ArrCodes.length; i++) {
                InsertRow('tblMessage', getClass(ArrCodes[i]), ArrMsgs[i]);
            }
        }
    }
    else // Reset table
    {
        var myTable = '<table id="tblMessage" cellpadding="0" cellspacing="0" border="0" style="width:100%;"><tbody>';
        var myEndTable = '</tbody></table>';

        var tableBodyHTML = '';
        for (var j = 0; j < ArrUserName.length; j++) {
            tableBodyHTML += '<tr><td class="EnrichUserName">';
            tableBodyHTML += ArrUserName[j];
            tableBodyHTML += "</td></tr>";

            for (var i = 0; i < ArrCodes.length; i++) {
                tableBodyHTML += '<tr><td class="' + getClass(ArrCodes[i]) + '">' + ArrMsgs[i] + "</td></tr>";
            }
        }
        myTable += tableBodyHTML + myEndTable;

        ageMsg.ShowMsgs(myTable, isSucc);
    }
}

function ValidateMessage(elementInput, errorMessage, xpos, pointerx) {
    var isXPos = (typeof (xpos) != "undefined" && xpos != null);
    var isPointerXPos = (typeof (pointerx) != "undefined" && pointerx != null);
    if (isXPos && pointerx) {
        Validators.show(elementInput, errorMessage, xpos, pointerx);
    }
    else {
        Validators.show(elementInput, errorMessage);
    }
    Validators.animate(elementInput);
}
