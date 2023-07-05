$(document).ready(() => {
    LoadAllProduct();
});

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

function AddToCart(productId) {
    $.ajax({
        url: "http://localhost:5063/api/Cart/add/" + productId,
        method: "GET",
        contentType: "application/json",
        headers: {
            Authorization: "Bearer " + token,
        },
        success: (response) => {
            alert('add successfully!');
            LoadAllProduct();
        },
        error: function (xhr, status, error) {
            console.log(error);
            alert(xhr.responseText);
        },
    });
}

function LoadAllProduct() {
    $.ajax({
        url: "http://localhost:5063/api/Product/getall",
        method: "GET",
        contentType: "application/json",
        headers: {
            Authorization: "Bearer " + token,
        },
        success: (response) => {
            $("#product-user").empty();
            $("#product-user").append(
                response.map(
                    (product) => (
                        `<tr>
                            <td>${product.categoryName}</td>
                            <td>${product.name}</td>
                            <td>${product.unitPrice}</td>
                            <td>${product.unitInStock}</td>
                            <td>${product.weight}</td>
                            <td>
                                <a onclick="AddToCart(${product.productId})">
                                    <i class="fa-solid fa-cart-shopping"></i>
                                </a>
                            </td>
                        </tr>`
                    )
                )
            );
        },
        error: function (xhr, status, error) {
            console.log(error);
            alert(xhr.responseText);
        },
    });
}