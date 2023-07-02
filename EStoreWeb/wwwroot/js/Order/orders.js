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
var orderId;
var updatedOrder;
var currentOrder;

function LoadAllOrders() {
    $.ajax({
		url: "http://localhost:5063/api/Order/getall",
		method: "GET",
		contentType: "application/json",
		headers: {
			Authorization: "Bearer " + token,
		},
		success: (response) => {
			$("#productTable").empty();
			$("#productTable").append(
				response.map(
					(product) => (
						`<tr>
								<th scope="row">${product.orderId}</th>
									<td>${product.username}</td>
									<td>${product.orderDate}</td>
									<td>${product.requiredDate}</td>
									<td>${product.shipDate}$</td>
									<td>${product.freight}</td>
									<td>
									<button type="button" class="btn btn-success" data-bs-toggle="modal"
														data-bs-target="#updateProductModal"
														data-prod-id="${product.orderId}"
                                                        data-order-memberId="${product.memberId}"
														data-prod-cate="${product.username}"
														data-prod-name="${product.requiredDate}"
														data-prod-weight="${product.shipDate}"
														data-prod-unit-in-stock="${product.freight}"
														onclick="updateProd(this)">
													<i class="fa-solid fa-pen-to-square"></i>
												</button>
												<button type="button" class="btn btn-danger" data-prod-id="${product.orderId}"
												onclick="deleteProd(this)">
											<i class="fa-solid fa-trash"></i>
										</button>
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
function updateOrder(model) {
	currentOrder = {
		orderDate: model.getAttribute("data-prod-name"),
		weight: model.getAttribute("data-prod-weight"),
		unitPrice: model.getAttribute("data-prod-unit-price"),
		unitInStock: model.getAttribute("data-prod-unit-in-stock"),
        memberId: model.getAttribute("data-order-memberId"),
	};

	$("#category-name-update").val(currentOrder.memberId);
	$("#product-name-update").val(currentOrder.productName);
	$("#product-weight-update").val(currentOrder.weight);
	$("#product-unit-price-update").val(currentOrder.unitPrice);
	$("#product-unit-in-stock-update").val(currentOrder.unitInStock);

	orderId = model.getAttribute("data-prod-id");
}
$(document).ready(() => {
    if (token !== null) {
		$("#add-prod-button").append(
			`<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
								Add Product
							</button>`
		);
	}
    LoadAllOrders();
});