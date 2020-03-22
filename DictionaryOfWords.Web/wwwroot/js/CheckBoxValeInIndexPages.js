function ChangeCheckBoxVal() {
    let bottunChangeCheckBox = $("#bottunChangeCheckBox");
    let target = $(".isDelete");
    if (bottunChangeCheckBox[0].textContent === "Выделить все") {
        bottunChangeCheckBox[0].innerText = "Снять выделение";
        for (let i = 0; i < target.length; i++) {
            target[i].checked = true;
        }
    }
    else {
        bottunChangeCheckBox[0].innerText = "Выделить все";
        for (let i = 0; i < target.length; i++) {
            target[i].checked = false;
        }
    }
}