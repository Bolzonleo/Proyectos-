import { useEffect, useState } from "react";
import { getProductos, crearProducto, eliminarProducto } from "./services/productService";

function App() {
  const [productos, setProductos] = useState([]);

  const [nuevoProducto, setNuevoProducto] = useState({
    nombre: "",
    descripcion: "",
    precio: 0,
    stock: 0,
  });

  useEffect(() => {
    cargarProductos();
  }, []);

  const cargarProductos = async () => {
    const data = await getProductos();
    setProductos(data);
  };

  const handleChange = (e) => {
    setNuevoProducto({
      ...nuevoProducto,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    await crearProducto(nuevoProducto);

    setNuevoProducto({
      nombre: "",
      descripcion: "",
      precio: 0,
      stock: 0,
    });

    cargarProductos();
  };

  const handleEliminar = async (id) => {
    const confirmar = window.confirm("¿Seguro que querés eliminar este producto?");

  if (!confirmar) return; // si cancela, no hace nada
    await eliminarProducto(id);
    cargarProductos();
  };

  return (
    <div>
      <h1>Gestión de Productos</h1>

      <h2>Crear Producto</h2>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="nombre"
          placeholder="Nombre"
          value={nuevoProducto.nombre}
          onChange={handleChange}
        />

        <input
          type="text"
          name="descripcion"
          placeholder="Descripción"
          value={nuevoProducto.descripcion}
          onChange={handleChange}
        />

        <input
          type="number"
          name="precio"
          placeholder="Precio"
          value={nuevoProducto.precio}
          onChange={handleChange}
        />

        <input
          type="number"
          name="stock"
          placeholder="Stock"
          value={nuevoProducto.stock}
          onChange={handleChange}
        />

        <button type="submit">Crear</button>
      </form>

      <h2>Lista de Productos</h2>

      <table border="1">
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
              <td>{p.id}</td>
              <td>{p.nombre}</td>
              <td>{p.descripcion}</td>
              <td>{p.precio}</td>
              <td>{p.stock}</td>
              <td>
                <button onClick={() => handleEliminar(p.id)}>
                  Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;