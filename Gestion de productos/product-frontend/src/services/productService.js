const API_BASE = import.meta.env.VITE_API_URL ?? "https://localhost:7257/api";

async function apiRequest(endpoint, options = {}) {
  const response = await fetch(`${API_BASE}${endpoint}`, {
    headers: {
      "Content-Type": "application/json",
      ...(options.headers ?? {}),
    },
    ...options,
  });

  const contentType = response.headers.get("content-type") || "";
  const hasJson = contentType.includes("application/json");
  const data = hasJson ? await response.json() : null;

  if (!response.ok) {
    const message = data?.message || data?.title || `Error ${response.status}`;
    throw new Error(message);
  }

  return data;
}

export const api = {
  productos: {
    listar: (params = "") => apiRequest(`/Productos${params}`),
    crear: (payload) => apiRequest("/Productos", { method: "POST", body: JSON.stringify(payload) }),
    actualizar: (id, payload) => apiRequest(`/Productos/${id}`, { method: "PUT", body: JSON.stringify(payload) }),
    eliminar: (id) => apiRequest(`/Productos/${id}`, { method: "DELETE" }),
  },
  categorias: {
    listar: () => apiRequest("/Categorias"),
    crear: (payload) => apiRequest("/Categorias", { method: "POST", body: JSON.stringify(payload) }),
    actualizar: (id, payload) => apiRequest(`/Categorias/${id}`, { method: "PUT", body: JSON.stringify(payload) }),
    eliminar: (id) => apiRequest(`/Categorias/${id}`, { method: "DELETE" }),
  },
  roles: {
    listar: () => apiRequest("/Roles"),
  },
  usuarios: {
    listar: () => apiRequest("/Usuarios"),
    crear: (payload) => apiRequest("/Usuarios", { method: "POST", body: JSON.stringify(payload) }),
  },
  carrito: {
    obtener: (usuarioId) => apiRequest(`/Carritos/usuario/${usuarioId}`),
    crear: (usuarioId) => apiRequest(`/Carritos/usuario/${usuarioId}`, { method: "POST" }),
    agregarItem: (usuarioId, payload) =>
      apiRequest(`/Carritos/usuario/${usuarioId}/items`, { method: "POST", body: JSON.stringify(payload) }),
    actualizarCantidad: (usuarioId, productoId, cantidad) =>
      apiRequest(`/Carritos/usuario/${usuarioId}/items/${productoId}?cantidad=${cantidad}`, { method: "PUT" }),
    quitarItem: (usuarioId, productoId) =>
      apiRequest(`/Carritos/usuario/${usuarioId}/items/${productoId}`, { method: "DELETE" }),
    vaciar: (usuarioId) => apiRequest(`/Carritos/usuario/${usuarioId}/items`, { method: "DELETE" }),
  },
  pedidos: {
    listar: () => apiRequest("/Pedidos"),
    crearDesdeCarrito: (usuarioId) => apiRequest(`/Pedidos/desde-carrito/${usuarioId}`, { method: "POST" }),
    actualizarEstado: (id, estado) =>
      apiRequest(`/Pedidos/${id}/estado`, { method: "PATCH", body: JSON.stringify({ estado }) }),
  },
  pagos: {
    listar: () => apiRequest("/Pagos"),
  },
  envios: {
    listar: () => apiRequest("/Envios"),
  },
};
