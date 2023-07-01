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
var prodId;
var updatedProduct;
var currentProduct;

$(document).ready(() => {
	if (token !== null) {
		$("#add-prod-button").append(
			`<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
								Add Product
							</button>`
		);
	}
	$("#add-prod-btn").click(() => {
		let product = {
			categoryId: $("#category-name").val(),
			name: $("#product-name").val(),
			weight: $("#product-weight").val(),
			unitPrice: $("#product-unit-price").val(),
			unitInStock: $("#product-unit-in-stock").val(),
		};
		$.ajax({
			url: "http://localhost:5063/api/Product/add",
			method: "POST",
			contentType: "application/json",
			data: JSON.stringify(product),
			headers: {
				Authorization: "Bearer " + token,
			},
			success: (response) => {
				alert('add successfully!');
				LoadProduct();
			},
			error: function (xhr, status, error) {
				console.log(error);
				alert(xhr.responseText);
			},
		});
	});
	$.ajax({
		url: "http://localhost:5063/api/Category/getall",
		method: "GET",
		contentType: "application/json",
		headers: {
			Authorization: "Bearer " + token,
		},
		success: (response) => {
			$("#category-name").append(
				response.map(
					(cat) => `<option value="${cat.categoryId}">${cat.categoryName}</option>`
				)
			);
			$("#category-name-update").append(
				response.map(
					(cat) =>
						`<option value="${cat.categoryId}">${cat.categoryName}</option>`
				)
			);
		},
		error: function (xhr, status, error) {
			console.log(error);
			alert(xhr.responseText);
		},
	});
	LoadProduct();
});
function updateProd(model) {
	currentProduct = {
		categoryId: model.getAttribute("data-prod-cate"),
		productName: model.getAttribute("data-prod-name"),
		weight: model.getAttribute("data-prod-weight"),
		unitPrice: model.getAttribute("data-prod-unit-price"),
		unitInStock: model.getAttribute("data-prod-unit-in-stock"),
	};

	$("#category-name-update").val(currentProduct.categoryId);
	$("#product-name-update").val(currentProduct.productName);
	$("#product-weight-update").val(currentProduct.weight);
	$("#product-unit-price-update").val(currentProduct.unitPrice);
	$("#product-unit-in-stock-update").val(currentProduct.unitInStock);

	prodId = model.getAttribute("data-prod-id");
}
function LoadProduct() {
	$.ajax({
		url: "http://localhost:5063/api/Product/getall",
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
								<th scope="row">${product.productId}</th>
									<td>${product.categoryName}</td>
									<td>${product.name}</td>
									<td>${product.weight}</td>
									<td>${product.unitPrice}$</td>
									<td>${product.unitInStock}</td>
									<td>
									<button type="button" class="btn btn-success" data-bs-toggle="modal"
														data-bs-target="#updateProductModal"
														data-prod-id="${product.productId}"
														data-prod-cate="${product.categoryId}"
														data-prod-name="${product.name}"
														data-prod-weight="${product.weight}"
														data-prod-unit-price="${product.unitPrice}"
														data-prod-unit-in-stock="${product.unitInStock}"
														onclick="updateProd(this)">
													<i class="fa-solid fa-pen-to-square"></i>
												</button>
												<button type="button" class="btn btn-danger" data-prod-id="${product.productId}"
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

$("#update-prod-btn").click(() => {
	updatedProduct = {
		name: $("#product-name-update").val(),
		weight: $("#product-weight-update").val(),
		unitPrice: $("#product-unit-price-update").val(),
		unitInStock: $("#product-unit-in-stock-update").val(),
	};

	$.ajax({
		url: "http://localhost:5063/api/Product/update/" + prodId,
		method: "PUT",
		contentType: "application/json",
		data: JSON.stringify(updatedProduct),
		headers: {
			Authorization: "Bearer " + token,
		},
		success: (response) => {
			alert('Update successfully!');
			LoadProduct();
		},
		error: function (xhr, status, error) {
			console.log(error);
			alert(xhr.responseText);
		},
	});
});
function deleteProd(model) {
	prodId = model.getAttribute("data-prod-id");
	$.ajax({
		url: "http://localhost:5063/api/Product/" + prodId,
		method: "DELETE",
		contentType: "application/json",
		headers: {
			Authorization: "Bearer " + token,
		},
		success: (response) => {
			alert('delete successfully');
			LoadProduct();
		},
		error: function (xhr, status, error) {
			console.log(error);
			alert(xhr.responseText);
		},
	})
};