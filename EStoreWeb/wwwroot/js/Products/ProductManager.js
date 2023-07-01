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

$(document).ready(() => {
	LoadProduct();
	if (token !== null) {
		$("#add-prod-button").append(
			`<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
								Add Product
							</button>`
		);
	}
});

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
									<button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#exampleModal"
									data-bs-whatever="${product.productId}">
										<i class="fa-solid fa-pen-to-square"></i>
									</button>
									<button type="button" class="btn btn-danger">
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