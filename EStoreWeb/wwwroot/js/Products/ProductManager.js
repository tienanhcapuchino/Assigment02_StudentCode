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
//$(document).ready(() => {
//    let token = getTokenFromCookie();
//    console.log(token);
//});

//console.log(getTokenFromCookie());

function LoadProduct() {
	$.ajax({
		url: "http://localhost:5063/api/Product/getall",
		method: "GET",
		contentType: "application/json",
		success: (response) => {
			$("#productTable").empty();
			// if (role === "Admin") {
				$("#productTable").append(
					response.map(
						(product) => (
							`<tr>
								<th scope="row">${product.productId}</th>
									<td>${product.categoryName}</td>
									<td>${product.Name}</td>
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
			// } else {
			// 	$("#product-table").append(
			// 		response.map(
			// 			(product) => (
			// 				`<tr>
			// 					<th scope="row">${product.productId}</th>
			// 					<td>${product.category}</td>
			// 					<td>${product.productName}</td>
			// 					<td>${product.weight}</td>
			// 					<td>${product.unitPrice}$</td>
			// 					<td>${product.unitInStock}</td>
			// 					<td>
			// 						<i class="fa-solid fa-cart-shopping"></i>
			// 					</td>
			// 				</tr>`
			// 			)
			// 		)
			// 	);
			// }
		},
		error: function (xhr, status, error) {
			console.log(error);
			alert(xhr.responseText);
		},
	});
}

$(document).ready(() => {
	LoadProduct();
});