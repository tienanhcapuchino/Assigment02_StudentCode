function getTokenFromCookie() {
    const cookies = document.cookie.split("; ");
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].split("=");
        const name = cookie[0];
        const value = cookie[1];
        if (name === "token") {
            return value;
        }
    }
    return null;
}

var token = getTokenFromCookie();

$('#createReport').click((e) => {
    e.preventDefault();
    var fromDate = $("#fromDate").val();
    var toDate = $("#toDate").val();
    $.ajax({
        url: "http://localhost:5063/api/User/salereport/" + fromDate +"/" + toDate,
        method: "GET",
        contentType: "application/json",
        headers: {
            Authorization: "Bearer " + token,
        },
        success: (response) => {
            $("#sale-report").empty()
                $("#sale-report").append(
                    response.map((item) => (
                        `<tr>
                            <td>${formatDate(item.orderDate)}</td>
                            <td>${item.productName}</td>
                            <td>${item.unitPrice}$</td>
                            <td>${item.quantity}</td>
                            <td>${item.sales}$</td>
                        </tr>`
                    ))
                )
        },
        error: function (xhr, status, error) {
            console.log(error);
            alert(xhr.responseText);
        },
    });
});

function formatDate(date) {
    const d = new Date(date);
    const day = d.getDate().toString().padStart(2, '0');
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const year = d.getFullYear().toString();
    return `${day}/${month}/${year}`;
}