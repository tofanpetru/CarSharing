function toggleReadOnly(typ) {
    if (typ == 'on') {
        document.getElementById('IP_Address').readOnly = false
    } else {
        document.getElementById('IP_Address').readOnly = true
    }
}