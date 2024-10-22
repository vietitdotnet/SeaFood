function showDetails(data) {

    var icon = data.childNodes[1];
    var collapse = data.parentElement.nextElementSibling;

    if (!collapse.classList.contains("myshow")) {
        collapse.classList.add("myshow");
        icon.classList.add("myactive");
    } else {
        collapse.classList.remove("myshow");
        icon.classList.remove("myactive")
    }
}
