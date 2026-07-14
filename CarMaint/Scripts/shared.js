function makeTableScroll(tableId, maxRows) {
    var table = document.getElementById(tableId);
    if (!table) return;

    var wrapper = table.parentNode;
    var rows = table.rows.length;

    // Skip header row
    var dataRows = rows - 1;
    var height = 0;

    if (dataRows > maxRows) {
        for (var i = 1; i <= maxRows; i++) {
            height += table.rows[i].clientHeight;
        }
        wrapper.style.height = height + "px";
    }
}
