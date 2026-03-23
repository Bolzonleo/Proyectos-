const API_URL = "https://localhost:7257/api/Productos";

export const getProductos = async () => {
  const response = await fetch(API_URL);
  const data = await response.json();
  return data;
};

export const crearProducto = async (producto) => {
  const response = await fetch("https://localhost:7257/api/Productos", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(producto),
  });

  return await response.json();
};

export const actualizarProducto = async (id, producto) => {
  const response = await fetch(`https://localhost:7257/api/Productos/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(producto),
  });

  if (!response.ok) {
    throw new Error(`Error al actualizar producto: ${response.statusText}`);
  }

  return response;
};

export const eliminarProducto = async (id) => {
  await fetch(`https://localhost:7257/api/Productos/${id}`, {
    method: "DELETE",
  });
};