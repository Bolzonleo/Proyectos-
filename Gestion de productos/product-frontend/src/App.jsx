import { useEffect, useMemo, useState } from "react";
import { api } from "./services/productService";
import { validateNonNegativeInteger, validatePositiveNumber, validateRequiredText } from "./services/validations";
import "./App.css";

const TABS = ["catalogo", "categorias", "carrito", "pedidos"];

const ESTADOS_PEDIDO = ["Pendiente", "Pagado", "Enviado", "Cancelado", "Completado"];

function App() {
  const [tabActiva, setTabActiva] = useState("catalogo");
  const [mensaje, setMensaje] = useState({ tipo: "", texto: "" });

  const [categorias, setCategorias] = useState([]);
  const [productos, setProductos] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [roles, setRoles] = useState([]);
  const [pedidos, setPedidos] = useState([]);
  const [pagos, setPagos] = useState([]);
  const [envios, setEnvios] = useState([]);

  const [filtroNombre, setFiltroNombre] = useState("");
  const [filtroCategoria, setFiltroCategoria] = useState("");

  const [formCategoria, setFormCategoria] = useState({ id: null, nombre: "" });
  const [formProducto, setFormProducto] = useState({
    id: null,
    nombre: "",
    descripcion: "",
    precio: "",
    stock: "",
    categoriaId: "",
  });

  const [usuarioId, setUsuarioId] = useState("");
  const [carrito, setCarrito] = useState(null);
  const [itemCarrito, setItemCarrito] = useState({ productoId: "", cantidad: 1 });

  useEffect(() => {
    inicializar();
  }, []);

  async function inicializar() {
    try {
      const [cats, prods, usrs, rls, peds, pgs, envs] = await Promise.all([
        api.categorias.listar(),
        api.productos.listar(),
        api.usuarios.listar(),
        api.roles.listar(),
        api.pedidos.listar(),
        api.pagos.listar(),
        api.envios.listar(),
      ]);

      setCategorias(cats);
      setProductos(prods);
      setUsuarios(usrs);
      setRoles(rls);
      setPedidos(peds);
      setPagos(pgs);
      setEnvios(envs);
      if (usrs.length > 0) {
        setUsuarioId(String(usrs[0].id));
      }
    } catch (error) {
      mostrarMensaje("error", `No se pudo inicializar: ${error.message}`);
    }
  }

  function mostrarMensaje(tipo, texto) {
    setMensaje({ tipo, texto });
    setTimeout(() => setMensaje({ tipo: "", texto: "" }), 3000);
  }

  async function recargarProductos() {
    const query = new URLSearchParams();
    if (filtroNombre.trim()) query.set("nombre", filtroNombre.trim());
    if (filtroCategoria) query.set("categoriaId", filtroCategoria);
    const suffix = query.toString() ? `?${query}` : "";

    const data = await api.productos.listar(suffix);
    setProductos(data);
  }

  async function onSubmitCategoria(e) {
    e.preventDefault();
    if (!validateRequiredText(formCategoria.nombre, 3, 50)) {
      mostrarMensaje("error", "Nombre de categoría inválido (3-50 caracteres)");
      return;
    }

    try {
      if (formCategoria.id) {
        await api.categorias.actualizar(formCategoria.id, { nombre: formCategoria.nombre });
        mostrarMensaje("exito", "Categoría actualizada");
      } else {
        await api.categorias.crear({ nombre: formCategoria.nombre });
        mostrarMensaje("exito", "Categoría creada");
      }
      setFormCategoria({ id: null, nombre: "" });
      setCategorias(await api.categorias.listar());
    } catch (error) {
      mostrarMensaje("error", error.message);
    }
  }

  async function onSubmitProducto(e) {
    e.preventDefault();
    if (!validateRequiredText(formProducto.nombre, 3, 100)) {
      mostrarMensaje("error", "Nombre de producto inválido");
      return;
    }

    if (!validatePositiveNumber(formProducto.precio, 0.01) || !validateNonNegativeInteger(formProducto.stock)) {
      mostrarMensaje("error", "Precio o stock inválidos");
      return;
    }

    if (!formProducto.categoriaId) {
      mostrarMensaje("error", "Selecciona una categoría");
      return;
    }

    const payload = {
      nombre: formProducto.nombre,
      descripcion: formProducto.descripcion,
      precio: Number(formProducto.precio),
      stock: Number(formProducto.stock),
      categoriaId: Number(formProducto.categoriaId),
    };

    try {
      if (formProducto.id) {
        await api.productos.actualizar(formProducto.id, payload);
        mostrarMensaje("exito", "Producto actualizado");
      } else {
        await api.productos.crear(payload);
        mostrarMensaje("exito", "Producto creado");
      }

      setFormProducto({ id: null, nombre: "", descripcion: "", precio: "", stock: "", categoriaId: "" });
      await recargarProductos();
    } catch (error) {
      mostrarMensaje("error", error.message);
    }
  }

  async function cargarCarrito() {
    if (!usuarioId) return;
    try {
      const data = await api.carrito.obtener(Number(usuarioId));
      setCarrito(data);
    } catch {
      const creado = await api.carrito.crear(Number(usuarioId));
      setCarrito(creado);
    }
  }

  async function agregarItemCarrito(e) {
    e.preventDefault();
    if (!usuarioId) return;
    if (!itemCarrito.productoId || !validateNonNegativeInteger(itemCarrito.cantidad) || Number(itemCarrito.cantidad) < 1) {
      mostrarMensaje("error", "Selecciona producto y cantidad válida");
      return;
    }

    try {
      const data = await api.carrito.agregarItem(Number(usuarioId), {
        productoId: Number(itemCarrito.productoId),
        cantidad: Number(itemCarrito.cantidad),
      });
      setCarrito(data);
      mostrarMensaje("exito", "Producto agregado al carrito");
    } catch (error) {
      mostrarMensaje("error", error.message);
    }
  }

  async function crearPedidoDesdeCarrito() {
    if (!usuarioId) return;
    try {
      await api.pedidos.crearDesdeCarrito(Number(usuarioId));
      mostrarMensaje("exito", "Pedido generado desde carrito");
      const [peds, pgs, envs] = await Promise.all([api.pedidos.listar(), api.pagos.listar(), api.envios.listar()]);
      setPedidos(peds);
      setPagos(pgs);
      setEnvios(envs);
      await cargarCarrito();
    } catch (error) {
      mostrarMensaje("error", error.message);
    }
  }

  const resumenCarrito = useMemo(() => {
    if (!carrito?.items) return { items: 0, total: 0 };
    const total = carrito.items.reduce((acc, i) => acc + i.cantidad * i.precioUnitario, 0);
    return { items: carrito.items.length, total };
  }, [carrito]);

  return (
    <main className="layout">
      <header className="hero">
        <p className="tag">Jardín ecommerce</p>
        <h1>Panel de gestión • Plantas, cactus y suculentas</h1>
        <p className="sub">Inspirado en tiendas online de suculentas: paleta verde/salvia, cards limpias y estética orgánica.</p>
      </header>

      <nav className="tabs">
        {TABS.map((tab) => (
          <button key={tab} className={tab === tabActiva ? "tab active" : "tab"} onClick={() => setTabActiva(tab)}>
            {tab}
          </button>
        ))}
      </nav>

      {mensaje.texto && <div className={`mensaje ${mensaje.tipo}`}>{mensaje.texto}</div>}

      {tabActiva === "catalogo" && (
        <section className="panel">
          <h2>Catálogo de productos</h2>
          <form className="grid" onSubmit={onSubmitProducto}>
            <input placeholder="Nombre" value={formProducto.nombre} onChange={(e) => setFormProducto({ ...formProducto, nombre: e.target.value })} />
            <input placeholder="Descripción" value={formProducto.descripcion} onChange={(e) => setFormProducto({ ...formProducto, descripcion: e.target.value })} />
            <input type="number" step="0.01" placeholder="Precio" value={formProducto.precio} onChange={(e) => setFormProducto({ ...formProducto, precio: e.target.value })} />
            <input type="number" step="1" placeholder="Stock" value={formProducto.stock} onChange={(e) => setFormProducto({ ...formProducto, stock: e.target.value })} />
            <select value={formProducto.categoriaId} onChange={(e) => setFormProducto({ ...formProducto, categoriaId: e.target.value })}>
              <option value="">Seleccionar categoría</option>
              {categorias.map((c) => <option key={c.id} value={c.id}>{c.nombre}</option>)}
            </select>
            <div className="actions">
              <button type="submit">{formProducto.id ? "Actualizar" : "Crear"}</button>
              <button type="button" className="ghost" onClick={() => setFormProducto({ id: null, nombre: "", descripcion: "", precio: "", stock: "", categoriaId: "" })}>Limpiar</button>
            </div>
          </form>

          <div className="filters">
            <input placeholder="Filtrar por nombre" value={filtroNombre} onChange={(e) => setFiltroNombre(e.target.value)} />
            <select value={filtroCategoria} onChange={(e) => setFiltroCategoria(e.target.value)}>
              <option value="">Todas las categorías</option>
              {categorias.map((c) => <option key={c.id} value={c.id}>{c.nombre}</option>)}
            </select>
            <button onClick={recargarProductos}>Aplicar filtros</button>
          </div>

          <table>
            <thead><tr><th>Producto</th><th>Categoría</th><th>Precio</th><th>Stock</th><th /></tr></thead>
            <tbody>
              {productos.map((p) => (
                <tr key={p.id}>
                  <td><strong>{p.nombre}</strong><br />{p.descripcion}</td>
                  <td>{p.categoriaNombre}</td>
                  <td>${Number(p.precio).toFixed(2)}</td>
                  <td>{p.stock}</td>
                  <td className="row-actions">
                    <button onClick={() => setFormProducto({ id: p.id, nombre: p.nombre, descripcion: p.descripcion, precio: p.precio, stock: p.stock, categoriaId: p.categoriaId })}>Editar</button>
                    <button className="danger" onClick={async () => { await api.productos.eliminar(p.id); await recargarProductos(); mostrarMensaje("exito", "Producto eliminado"); }}>Eliminar</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </section>
      )}

      {tabActiva === "categorias" && (
        <section className="panel">
          <h2>Categorías, usuarios y roles</h2>
          <form className="grid" onSubmit={onSubmitCategoria}>
            <input placeholder="Nombre categoría" value={formCategoria.nombre} onChange={(e) => setFormCategoria({ ...formCategoria, nombre: e.target.value })} />
            <div className="actions"><button type="submit">{formCategoria.id ? "Actualizar" : "Crear"}</button></div>
          </form>

          <div className="cards">
            <article>
              <h3>Categorías</h3>
              {categorias.map((c) => (
                <div key={c.id} className="list-row">
                  <span>{c.nombre}</span>
                  <div>
                    <button onClick={() => setFormCategoria({ id: c.id, nombre: c.nombre })}>Editar</button>
                    <button className="danger" onClick={async () => { await api.categorias.eliminar(c.id); setCategorias(await api.categorias.listar()); }}>Eliminar</button>
                  </div>
                </div>
              ))}
            </article>
            <article>
              <h3>Usuarios ({usuarios.length})</h3>
              {usuarios.map((u) => <p key={u.id}>{u.nombre} · {u.email} · Rol {u.rolId}</p>)}
              <h3>Roles</h3>
              <p>{roles.map((r) => r.nombre).join(", ")}</p>
            </article>
          </div>
        </section>
      )}

      {tabActiva === "carrito" && (
        <section className="panel">
          <h2>Carrito por usuario</h2>
          <div className="filters">
            <select value={usuarioId} onChange={(e) => setUsuarioId(e.target.value)}>
              <option value="">Selecciona usuario</option>
              {usuarios.map((u) => <option key={u.id} value={u.id}>{u.nombre}</option>)}
            </select>
            <button onClick={cargarCarrito}>Cargar carrito</button>
            <button className="ghost" onClick={async () => { await api.carrito.vaciar(Number(usuarioId)); await cargarCarrito(); }}>Vaciar</button>
          </div>

          <form className="grid" onSubmit={agregarItemCarrito}>
            <select value={itemCarrito.productoId} onChange={(e) => setItemCarrito({ ...itemCarrito, productoId: e.target.value })}>
              <option value="">Producto</option>
              {productos.map((p) => <option key={p.id} value={p.id}>{p.nombre}</option>)}
            </select>
            <input type="number" min="1" value={itemCarrito.cantidad} onChange={(e) => setItemCarrito({ ...itemCarrito, cantidad: e.target.value })} />
            <div className="actions"><button type="submit">Agregar al carrito</button></div>
          </form>

          <p><strong>Items:</strong> {resumenCarrito.items} · <strong>Total:</strong> ${resumenCarrito.total.toFixed(2)}</p>

          {carrito?.items?.map((i) => (
            <div key={i.id} className="list-row">
              <span>{i.productoNombre} x{i.cantidad}</span>
              <div>
                <button onClick={async () => { await api.carrito.actualizarCantidad(Number(usuarioId), i.productoId, i.cantidad + 1); await cargarCarrito(); }}>+1</button>
                <button className="danger" onClick={async () => { await api.carrito.quitarItem(Number(usuarioId), i.productoId); await cargarCarrito(); }}>Quitar</button>
              </div>
            </div>
          ))}

          <button onClick={crearPedidoDesdeCarrito}>Generar pedido desde carrito</button>
        </section>
      )}

      {tabActiva === "pedidos" && (
        <section className="panel">
          <h2>Pedidos, pagos y envíos</h2>
          <div className="cards three">
            <article>
              <h3>Pedidos</h3>
              {pedidos.map((p) => (
                <div key={p.id} className="list-row">
                  <span>#{p.id} · U{p.usuarioId} · ${p.total.toFixed(2)}</span>
                  <select value={p.estado} onChange={async (e) => {
                    await api.pedidos.actualizarEstado(p.id, e.target.value);
                    setPedidos(await api.pedidos.listar());
                  }}>
                    {ESTADOS_PEDIDO.map((estado) => <option key={estado}>{estado}</option>)}
                  </select>
                </div>
              ))}
            </article>
            <article>
              <h3>Pagos</h3>
              {pagos.map((p) => <p key={p.id}>Pedido #{p.pedidoId} · {p.formaPago} · {p.estado} · ${Number(p.monto).toFixed(2)}</p>)}
            </article>
            <article>
              <h3>Envíos</h3>
              {envios.map((e) => <p key={e.id}>Pedido #{e.pedidoId} · {e.estado} · {e.direccion}</p>)}
            </article>
          </div>
        </section>
      )}
    </main>
  );
}

export default App;
