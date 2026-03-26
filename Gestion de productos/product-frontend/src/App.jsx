import { useEffect, useState } from "react";
import { getProductos, crearProducto, actualizarProducto, eliminarProducto } from "./services/productService";
import { validateProductName, validateProductPrice, validateProductQuantity } from "./services/validations";
import "./App.css";

function App() {
  const [productos, setProductos] = useState([]);
  const [nuevoProducto, setNuevoProducto] = useState({
    nombre: "",
    descripcion: "",
    precio: 0,
    stock: 0,
  });

  // Estados para edición
  const [editando, setEditando] = useState(false);
  const [idEditando, setIdEditando] = useState(null);

  // Estados para validación y mensajes
  const [errores, setErrores] = useState({});
  const [mensaje, setMensaje] = useState({ tipo: "", texto: "" });

  useEffect(() => {
    cargarProductos();
  }, []);

  const cargarProductos = async () => {
    try {
      const data = await getProductos();
      setProductos(data);
    } catch (error) {
      mostrarMensaje("error", "Error al cargar productos");
      console.error(error);
    }
  };

  const mostrarMensaje = (tipo, texto) => {
    setMensaje({ tipo, texto });
    setTimeout(() => setMensaje({ tipo: "", texto: "" }), 3000);
  };

  const validarFormulario = () => {
    const nuevosErrores = {};

    // Validar nombre
    if (!validateProductName(nuevoProducto.nombre)) {
      nuevosErrores.nombre = "El nombre del producto es requerido";
    }
    if (nuevoProducto.nombre.trim().length < 3) {
      nuevosErrores.nombre = "El nombre debe tener al menos 3 caracteres";
    }
    if (nuevoProducto.nombre.trim().length > 100) {
      nuevosErrores.nombre = "El nombre no puede exceder 100 caracteres";
    }

    // Validar descripción
    if (nuevoProducto.descripcion.trim().length > 500) {
      nuevosErrores.descripcion = "La descripción no puede exceder 500 caracteres";
    }

    // Validar precio
    if (!validateProductPrice(nuevoProducto.precio)) {
      nuevosErrores.precio = "El precio debe ser un número positivo";
    }
    if (nuevoProducto.precio < 0.01) {
      nuevosErrores.precio = "El precio debe ser mayor a 0";
    }
    if (nuevoProducto.precio > 999999.99) {
      nuevosErrores.precio = "El precio no puede exceder 999,999.99";
    }

    // Validar stock
    if (!validateProductQuantity(nuevoProducto.stock)) {
      nuevosErrores.stock = "El stock debe ser un número entero positivo";
    }
    if (nuevoProducto.stock < 0) {
      nuevosErrores.stock = "El stock no puede ser negativo";
    }
    if (nuevoProducto.stock > 999999) {
      nuevosErrores.stock = "El stock no puede exceder 999,999";
    }

    setErrores(nuevosErrores);
    return Object.keys(nuevosErrores).length === 0;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setNuevoProducto({
      ...nuevoProducto,
      [name]: name === "precio" || name === "stock" ? (value === "" ? 0 : parseFloat(value)) : value,
    });
    // Limpiar error del campo cuando el usuario empieza a escribir
    if (errores[name]) {
      setErrores({ ...errores, [name]: "" });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validarFormulario()) {
      mostrarMensaje("error", "Por favor, corrige los errores del formulario");
      return;
    }

    try {
      if (editando) {
        await actualizarProducto(idEditando, nuevoProducto);
        mostrarMensaje("exito", "✅ Producto actualizado correctamente");
        setEditando(false);
        setIdEditando(null);
      } else {
        await crearProducto(nuevoProducto);
        mostrarMensaje("exito", "✅ Producto creado correctamente");
      }

      setNuevoProducto({
        nombre: "",
        descripcion: "",
        precio: 0,
        stock: 0,
      });

      cargarProductos();
    } catch (error) {
      mostrarMensaje("error", "❌ Error al guardar el producto");
      console.error(error);
    }
  };

  const handleEditar = (producto) => {
    setNuevoProducto(producto);
    setEditando(true);
    setIdEditando(producto.id);
    setErrores({});
  };

  const handleEliminar = async (id) => {
    const confirmar = window.confirm("¿Estás seguro de que quieres eliminar este producto?");

    if (!confirmar) return;

    try {
      await eliminarProducto(id);
      mostrarMensaje("exito", "✅ Producto eliminado correctamente");
      cargarProductos();
    } catch (error) {
      mostrarMensaje("error", "❌ Error al eliminar el producto");
      console.error(error);
    }
  };

  const handleCancelar = () => {
    setEditando(false);
    setIdEditando(null);
    setNuevoProducto({
      nombre: "",
      descripcion: "",
      precio: 0,
      stock: 0,
    });
    setErrores({});
  };

  return (
    <div className="container">
      <h1>📦 Gestión de Productos</h1>

      {/* Mensaje de estado */}
      {mensaje.texto && (
        <div className={`mensaje ${mensaje.tipo}`}>
          {mensaje.texto}
        </div>
      )}

      {/* Formulario */}
      <div className="form-container">
        <h2>{editando ? "✏️ Editar Producto" : "➕ Nuevo Producto"}</h2>

        <form onSubmit={handleSubmit}>
          {/* Campo Nombre */}
          <div className="form-group">
            <label htmlFor="nombre">Nombre *</label>
            <input
              type="text"
              id="nombre"
              name="nombre"
              placeholder="Ej: iPhone 15"
              value={nuevoProducto.nombre}
              onChange={handleChange}
              className={errores.nombre ? "input-error" : ""}
              maxLength="100"
            />
            {errores.nombre && <span className="error-text">⚠️ {errores.nombre}</span>}
            <span className="char-count">{nuevoProducto.nombre.length}/100</span>
          </div>

          {/* Campo Descripción */}
          <div className="form-group">
            <label htmlFor="descripcion">Descripción</label>
            <textarea
              id="descripcion"
              name="descripcion"
              placeholder="Describe el producto..."
              value={nuevoProducto.descripcion}
              onChange={handleChange}
              className={errores.descripcion ? "input-error" : ""}
              maxLength="500"
              rows="3"
            />
            {errores.descripcion && <span className="error-text">⚠️ {errores.descripcion}</span>}
            <span className="char-count">{nuevoProducto.descripcion.length}/500</span>
          </div>

          {/* Campo Precio */}
          <div className="form-group">
            <label htmlFor="precio">Precio ($) *</label>
            <input
              type="number"
              id="precio"
              name="precio"
              placeholder="0.00"
              value={nuevoProducto.precio || ""}
              onChange={handleChange}
              className={errores.precio ? "input-error" : ""}
              step="0.01"
              min="0"
              max="999999.99"
            />
            {errores.precio && <span className="error-text">⚠️ {errores.precio}</span>}
          </div>

          {/* Campo Stock */}
          <div className="form-group">
            <label htmlFor="stock">Stock (unidades) *</label>
            <input
              type="number"
              id="stock"
              name="stock"
              placeholder="0"
              value={nuevoProducto.stock || ""}
              onChange={handleChange}
              className={errores.stock ? "input-error" : ""}
              step="1"
              min="0"
              max="999999"
            />
            {errores.stock && <span className="error-text">⚠️ {errores.stock}</span>}
          </div>

          {/* Botones */}
          <div className="button-group">
            <button type="submit" className="btn-submit">
              {editando ? "✅ Actualizar" : "➕ Crear"}
            </button>
            {editando && (
              <button type="button" onClick={handleCancelar} className="btn-cancel">
                ❌ Cancelar
              </button>
            )}
          </div>
        </form>
      </div>

      {/* Tabla de productos */}
      <div className="products-container">
        <h2>📋 Lista de Productos ({productos.length})</h2>

        {productos.length === 0 ? (
          <p className="no-products">No hay productos disponibles</p>
        ) : (
          <table className="products-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nombre</th>
                <th>Descripción</th>
                <th>Precio</th>
                <th>Stock</th>
                <th>Acciones</th>
              </tr>
            </thead>

            <tbody>
              {productos.map((p) => (
                <tr key={p.id}>
                  <td className="id-cell">{p.id}</td>
                  <td className="nombre-cell">{p.nombre}</td>
                  <td className="descripcion-cell">{p.descripcion || "-"}</td>
                  <td className="precio-cell">${p.precio.toFixed(2)}</td>
                  <td className="stock-cell">
                    <span className={p.stock === 0 ? "stock-bajo" : ""}>
                      {p.stock} unidades
                    </span>
                  </td>
                  <td className="actions-cell">
                    <button
                      onClick={() => handleEditar(p)}
                      className="btn-edit"
                      title="Editar producto"
                    >
                      ✏️ Editar
                    </button>
                    <button
                      onClick={() => handleEliminar(p.id)}
                      className="btn-delete"
                      title="Eliminar producto"
                    >
                      🗑️ Eliminar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

export default App;