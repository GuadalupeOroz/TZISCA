UNIVERSIDAD AUTÓNOMA DE CHIAPAS

Escuela de Tecnologías Digitales Aplicadas

Ingeniería en Desarrollo y Tecnologías de Software

5.º semestre, Grupo B

# Propuesta de proyecto de prácticas profesionales

**TZISCA — Sistema Web de Citas, Recomendación y Gestión de Cabinas para Spa**

Empresa: Time Tracker

Alumna: Guadalupe Orozco Hernández

Responsable del proyecto: Adán Núñez

Fecha: 14 de agosto de 2026

Nota de versión. Actualización del 13/09/2026 alineada con las 19 entidades, RN-01 a RN-108, Cita como unidad de agenda, Cliente como entidad explícita, paquetes, Stripe, Identity/JWT y las políticas operativas aprobadas.

## 1. Descripción del producto

El producto propuesto es una plataforma web para la administración de citas de servicios y cabinas de un spa. Su propósito es reunir en una sola solución el catálogo de tratamientos, la información de las cabinas, la disponibilidad por fecha y hora, los proveedores de tratamiento y las citas realizadas por los clientes.

Desde la primera versión, el sistema contemplará masajes terapéuticos, tratamientos faciales, hidroterapia y sauna, así como cabinas integrales/multifuncionales y de sal/haloterapia, conforme al Catálogo de Cabinas TZISCA vigente. Cada tratamiento tendrá una duración, características, beneficios y reglas de compatibilidad con determinadas cabinas. A su vez, cada cabina contará con una ficha propia que explique su capacidad, características, beneficios y los tratamientos para los que puede utilizarse.

La cita será asistida por el sistema. Después de elegir un tratamiento, la aplicación filtrará las cabinas compatibles y realizará una preasignación automática considerando el tipo de servicio, la capacidad requerida y la disponibilidad. Al mismo tiempo, mostrará otras cabinas compatibles junto con una explicación de sus características y beneficios, de modo que el cliente pueda comparar alternativas antes de confirmar.

El cliente elegirá la fecha y la hora deseada. El sistema validará la disponibilidad durante toda la duración del tratamiento; si existe un conflicto, informará que el horario está ocupado y mostrará horarios alternativos disponibles. La solución tendrá interfaces diferenciadas para clientes, administradores y proveedores de tratamiento.

El alcance funcional confirmado incorpora, además del flujo de recomendación y cita descrito, un carrito previo a la cita, bloqueos temporales de recursos durante el proceso, el pago de la cita cuando corresponda y la consulta de su estado, cancelaciones de tratamientos o de la cita completa, devoluciones totales o parciales derivadas de cancelaciones con pago aprobado, la gestión y sustitución de proveedores de tratamiento ante indisponibilidades, y reportes básicos de operación. La solución mantendrá interfaces diferenciadas para los cuatro roles del sistema: Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

Figura 1. Arquitectura general propuesta del producto.

## 2. Problemática

La operación de un spa implica coordinar simultáneamente varios recursos que dependen entre sí: tratamientos con distinta duración, cabinas con capacidades y características específicas, proveedores de tratamiento, clientes y periodos de tiempo disponibles. Una cita no consiste únicamente en registrar una fecha; requiere comprobar que el espacio sea adecuado para el servicio, que tenga capacidad suficiente y que permanezca libre durante todo el intervalo requerido.

Cuando la información de servicios, cabinas y horarios no se encuentra integrada en un mismo proceso, la consulta de disponibilidad se vuelve más lenta y aumenta el riesgo de traslapes. Dos citas pueden aparentar utilizar horas distintas y aun así entrar en conflicto si la duración de los tratamientos se superpone. De igual forma, una cabina puede estar libre pero no ser compatible con el tratamiento solicitado o no tener la capacidad necesaria para el número de personas.

A esta dificultad operativa se suma un problema de información para el cliente. Un usuario puede conocer el nombre de un tratamiento, pero no necesariamente las diferencias entre las cabinas que podrían utilizarse, sus características, beneficios o el tipo de experiencia para la que fueron configuradas. Si esa información no se presenta durante la cita, la selección depende de explicaciones externas o de una decisión poco informada.

También se requiere separar las responsabilidades de los distintos participantes. El cliente necesita consultar opciones y reservar; el administrador necesita mantener tratamientos, cabinas, horarios, usuarios y citas; y el proveedor de tratamiento necesita relacionarse con los servicios que puede atender. Sin una gestión centralizada, la información puede quedar dispersa y dificultar el seguimiento de una cita desde su solicitud hasta su atención.

Por lo anterior, la necesidad principal es contar con una plataforma que centralice la información y aplique reglas de negocio sobre compatibilidad, capacidad, duración y disponibilidad. Además de reducir conflictos de agenda, la solución debe orientar al cliente mediante recomendaciones informativas de cabinas y alternativas de horario, haciendo que el proceso de cita sea más claro y que la administración interna tenga una fuente única de información.

## 3. Objetivo general

Diseñar y desarrollar una plataforma web integral para la gestión de citas de servicios y cabinas de spa, que centralice usuarios, tratamientos, cabinas, proveedores y horarios; permita al cliente registrarse, consultar información detallada de los tratamientos, recibir una preasignación automática y recomendaciones de cabinas compatibles, seleccionar una fecha y una hora, validar la disponibilidad durante toda la duración del servicio y confirmar su cita; y proporcione al personal administrativo herramientas para mantener catálogos, disponibilidad y reservas desde una misma solución.

## 4. Objetivos específicos

- Implementar autenticación y control de acceso para clientes, administradores y proveedores de tratamiento.

- Administrar un catálogo de tratamientos con descripción, duración, beneficios y condiciones de uso dentro del sistema.

- Administrar cabinas con información de capacidad, características, beneficios, estado y tratamientos compatibles.

- Aplicar reglas de compatibilidad entre tratamiento, cabina, número de personas y disponibilidad.

- Realizar una preasignación automática de cabina y mostrar alternativas compatibles explicando por qué pueden ser adecuadas.

- Permitir al cliente elegir fecha y hora y consultar, antes de confirmar, si el intervalo requerido se encuentra disponible.

- Mostrar horarios alternativos cuando la opción seleccionada tenga un conflicto de disponibilidad.

- Registrar, consultar y administrar citas evitando traslapes de cabina.

- Gestionar proveedores de tratamiento y relacionarlos con los servicios que pueden atender.

- Proporcionar un panel administrativo para mantener la información operativa del sistema y consultar el historial de citas.

## 5. Funcionalidades principales del sistema

Las funcionalidades se definen por módulo para evitar duplicidades entre alcance, módulos y requerimientos. Cada módulo representa una capacidad concreta del producto.

### 5.1 Registro, autenticación y roles

El sistema permitirá crear cuentas, iniciar y cerrar sesión y controlar las funciones disponibles según el rol. El cliente gestionará sus propias reservas; el Administrador general tendrá acceso a la configuración general, usuarios, tratamientos, cabinas, proveedores y reportes; Recepción y cabinas dispondrá de las funciones operativas de agenda, disponibilidad, citas manuales, cancelaciones y estados de cabina; y el Proveedor de tratamiento dispondrá de las funciones relacionadas con los servicios que atiende.

### 5.2 Catálogo de tratamientos

Permitirá registrar y consultar masajes terapéuticos, tratamientos faciales, hidroterapia, sauna, así como servicios en cabinas integrales/multifuncionales y de sal/haloterapia desde la primera versión. Cada tratamiento tendrá nombre, descripción, duración, beneficios, recomendaciones informativas, capacidad o restricciones operativas y relación con tipos de cabina.

### 5.3 Catálogo de cabinas

Cada cabina tendrá una ficha propia con nombre o identificador, tipo, capacidad, descripción, características, beneficios, estado y tratamientos compatibles. El administrador podrá dar de alta, modificar o desactivar cabinas sin modificar el código de la aplicación.

### 5.4 Compatibilidad y recomendación de cabinas

Al seleccionar un tratamiento, el sistema filtrará automáticamente las cabinas que cumplen las reglas de compatibilidad y capacidad. De ese conjunto realizará una preasignación y mostrará alternativas con su descripción y beneficios. Las recomendaciones serán informativas y no constituirán diagnóstico ni prescripción médica.

### 5.5 Agenda y disponibilidad

El usuario seleccionará una fecha y una hora. La aplicación calculará la hora de finalización a partir de la duración del tratamiento y verificará que la cabina permanezca libre durante todo el intervalo. Los horarios que produzcan traslapes se considerarán ocupados.

### 5.6 Alternativas de horario

Si la hora elegida no está disponible, el sistema mostrará una lista de horarios que sí pueden completar la reserva con una cabina compatible. El cliente podrá seleccionar una alternativa sin reiniciar el proceso.

### 5.7 Citas

Cada Cita representa un Tratamiento programado con su propia cabina, número de personas, horario, proveedor y estado. Un Carrito puede originar varias Citas PENDIENTES, que pasan a CONFIRMADA de forma independiente después del pago requerido y la revalidación de disponibilidad.

### 5.8 Gestión de proveedores de tratamiento

Permitirá registrar al personal que presta los servicios y relacionarlo con uno o varios tratamientos. La asignación inicial del proveedor a cada tratamiento reservado corresponde al Administrador general; si el proveedor asignado registra una indisponibilidad, el sistema buscará un sustituto disponible y autorizado, informando al cliente antes de aplicar el cambio definitivo (ver 5.17 Sustitución de proveedores).

### 5.9 Portal del cliente

El cliente podrá consultar tratamientos y cabinas, gestionar su carrito, realizar reservas y consultar estado, historial y pago. Podrá cancelar una Cita específica o todas las Citas de una operación conforme a las políticas establecidas. La modificación de una Cita ya confirmada se realiza cancelándola y generando una nueva; TZISCA no ofrece una función de reprogramación directa.

### 5.10 Panel administrativo

Permitirá gestionar usuarios, tratamientos, cabinas, relaciones de compatibilidad, horarios, bloqueos, proveedores, citas, cancelaciones, pagos y devoluciones, diferenciando las funciones del Administrador general y de Recepción y cabinas. También incorporará reportes básicos (citas por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas) a partir de la información transaccional del sistema.

Figura 2. Flujo funcional de cita asistida y validación de disponibilidad.

### 5.11 Carrito de selección

El cliente podrá agregar uno o varios tratamientos a un carrito antes de confirmar la cita, indicando número de personas, cabina y horario provisionales para cada uno. Agregar un tratamiento al carrito no equivale a reservarlo; el cliente podrá agregar, eliminar o modificar elementos antes de confirmar.

### 5.12 Bloqueo temporal

Al seleccionar una cabina, fecha y hora válidas, el sistema aplicará un bloqueo temporal de 15 minutos. El bloqueo se libera si vence, si el cliente cambia su selección o si abandona el carrito.

### 5.13 Pagos

Cuando una Cita requiera pago, permanecerá PENDIENTE hasta que el Pago quede PAGADO y la disponibilidad se revalide; un pago FALLIDO o CANCELADO no confirma la Cita. TZISCA no almacenará datos bancarios sensibles completos.

### 5.14 Consulta de estado de pago

El cliente y el personal autorizado (Administrador general y Recepción y cabinas, según sus permisos) podrán consultar el estado del pago asociado a una cita en cualquier momento del proceso.

### 5.15 Cancelaciones

El cliente podrá cancelar una Cita específica o todas las Citas de la operación; Recepción y cabinas y el Administrador general también podrán realizar cancelaciones operativas. Cancelar libera automáticamente cabina, horario y proveedor asignado, sin eliminar el registro histórico ni el pago o devolución relacionados. Cancelar una Cita no cancela automáticamente las demás Citas de la misma operación.

### 5.16 Devoluciones

Cuando una Cancelacion tenga un Pago PAGADO, el sistema aplicará la política aprobada: 100 % con 24 horas o más, 50 % entre 6 y menos de 24 horas y 0 % con menos de 6 horas; una causa atribuible al spa genera 100 %. No hay devolución por inasistencia ni por servicio iniciado o completado.

### 5.17 Sustitución de proveedores

Si un proveedor asignado registra un periodo de indisponibilidad, el sistema buscará automáticamente un sustituto activo, autorizado para el tratamiento y disponible durante todo el horario requerido. El cliente será informado antes del cambio definitivo y podrá aceptar al nuevo proveedor, cancelar solo el tratamiento afectado o cancelar toda la cita. Si no existe sustituto disponible, el sistema informará al cliente.

### 5.18 Reportes básicos

El sistema generará reportes básicos a partir de la información transaccional ya registrada: citas por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas. No se define una entidad Reporte independiente; los reportes se calculan mediante consultas sobre las demás entidades.

### 5.19 Flujo principal del cliente

El flujo principal de cita del cliente queda definido como sigue:

Login → Catálogo o Paquete → Carrito → Personas → Recomendación → Cabina → Fecha/hora → Disponibilidad → Bloqueo de 15 minutos → Crear Cita PENDIENTE → Pago → Revalidación → Cita CONFIRMADA → Mis citas.

El Cliente inicia sesión, elige un Tratamiento o un Paquete, configura el Carrito y recibe una recomendación determinista de Cabina. Tras validar fecha y hora, TZISCA crea una Cita PENDIENTE por cada Tratamiento seleccionado, procesa el Pago cuando corresponde y confirma únicamente las Citas cuya disponibilidad siga siendo válida.

## 6. Usuarios del sistema

### 6.1 Cliente

- Registrarse e iniciar sesión.

- Consultar tratamientos y cabinas compatibles.

- Recibir preasignación y alternativas de cabina.

- Seleccionar número de personas, fecha y hora; gestionar su carrito y bloqueo temporal.

- Consultar disponibilidad y horarios alternativos.

- Confirmar su cita, pagar cuando corresponda y consultar el estado del pago.

- Consultar sus citas, cancelar tratamientos o la cita completa, y dar seguimiento a devoluciones relacionadas.

### 6.2 Administrador general

- Gestionar usuarios y roles.

- Gestionar tratamientos, cabinas y relaciones de compatibilidad.

- Gestionar proveedores de tratamiento y su asignación inicial.

- Consultar y administrar todas las citas, pagos y devoluciones según sus permisos.

- Consultar reportes básicos e información operativa para seguimiento.

### 6.3 Recepción y cabinas

- Gestionar la agenda, la disponibilidad y las citas manuales.

- Realizar cancelaciones operativas y verificar pagos o devoluciones relacionados.

- Gestionar el estado operativo de las cabinas y los bloqueos por mantenimiento, limpieza u otra causa.

- Consultar el estado de pago de las citas que gestione.

### 6.4 Proveedor de tratamiento

- Mantenerse relacionado con los tratamientos que puede atender.

- Consultar los tratamientos que tenga asignados.

- Registrar periodos de indisponibilidad, activando la búsqueda de un proveedor sustituto cuando corresponda.

- Participar en la operación del servicio sin acceso a funciones administrativas ajenas a su rol.

## 7. Reglas de negocio principales

Las siguientes reglas resumen los criterios estructurales del sistema a nivel de propuesta. El detalle completo y numerado (RN-01 a RN-108) se documenta en Reglas de Negocio Horario y Políticas TZISCA.

- Una cabina no podrá estar asociada a dos citas cuyos periodos de tiempo se traslapen.

- La cabina utilizada deberá ser compatible con el tratamiento solicitado.

- La capacidad de la cabina deberá ser igual o superior al número de personas de la cita.

- La disponibilidad se validará desde la hora de inicio hasta la hora de finalización calculada con la duración del tratamiento.

- Un horario solo se ofrecerá como disponible cuando exista por lo menos una cabina válida para completar el servicio.

- La preasignación automática deberá considerar, como mínimo, compatibilidad, capacidad y disponibilidad.

- Las recomendaciones de cabina mostrarán información registrada por el administrador y tendrán carácter orientativo, no clínico.

- Las reglas detalladas de cancelaciones, sustitución de proveedores, pagos y devoluciones se documentan en RN-01 a RN-108. Los horarios, el bloqueo, la tolerancia, los porcentajes, el cálculo económico, Identity/JWT, Stripe y el orden determinista ya están aprobados.

## 8. Requerimientos funcionales preliminares

Los requerimientos RF-01 a RF-16 corresponden a la propuesta original y no se renumeran. Los requerimientos RF-17 en adelante se agregan en esta actualización para cubrir el alcance funcional incorporado (carrito, bloqueo temporal, pagos, devoluciones, cancelaciones y sustitución de proveedores).

- RF-01. Registrar clientes e iniciar/cerrar sesión.

- RF-02. Aplicar permisos según rol de usuario.

- RF-03. Consultar y administrar tratamientos.

- RF-04. Consultar y administrar cabinas.

- RF-05. Relacionar tratamientos con cabinas compatibles.

- RF-06. Filtrar cabinas por tratamiento, capacidad y disponibilidad.

- RF-07. Realizar una preasignación automática y mostrar cabinas alternativas con descripción y beneficios.

- RF-08. Permitir seleccionar número de personas, fecha y hora.

- RF-09. Calcular la hora final según la duración del tratamiento.

- RF-10. Validar conflictos de disponibilidad en todo el intervalo de la reserva.

- RF-11. Mostrar horarios alternativos cuando el seleccionado esté ocupado.

- RF-12. Registrar una cita con cliente, tratamiento, cabina, fecha, horas y estado.

- RF-13. Permitir al cliente consultar su historial de citas.

- RF-14. Permitir al administrador consultar y gestionar citas.

- RF-15. Administrar proveedores de tratamiento y su relación con servicios.

- RF-16. Permitir agregar nuevos tratamientos y cabinas mediante configuración administrativa.

- RF-17. Permitir al cliente agregar uno o varios tratamientos a un carrito antes de confirmar la cita, sin que ello constituya una cita definitiva.

- RF-18. Generar un bloqueo temporal sobre la cabina, fecha y hora seleccionadas mientras el cliente completa el carrito y el pago, liberándolo automáticamente si expira o si la selección se modifica.

- RF-19. Crear una Cita en estado PENDIENTE por cada Tratamiento seleccionado al confirmar el resumen del Carrito y antes de completar el Pago.

- RF-20. Solicitar y procesar el pago de la operación cuando el servicio lo requiera, confirmando cada Cita únicamente cuando el pago sea aprobado y la disponibilidad se revalide.

- RF-21. Permitir al cliente y al personal autorizado consultar el estado del pago asociado a una cita.

- RF-22. Permitir cancelar un tratamiento específico o una cita completa, liberando los recursos asociados sin eliminar el registro histórico.

- RF-23. Registrar y gestionar devoluciones totales o parciales derivadas de una cancelación con pago aprobado.

- RF-24. Buscar y proponer un proveedor sustituto cuando el proveedor asignado registre una indisponibilidad, permitiendo al cliente aceptar el cambio, cancelar el tratamiento afectado o cancelar toda la cita.

- RF-25. Generar reportes básicos de citas por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas.

## 9. Tecnologías y arquitectura propuestas

Frontend - Angular. Se utilizará para la interfaz web y las vistas de clientes, administradores y proveedores, incluyendo formularios, catálogos, carrito, selección de horarios y paneles de gestión.

Backend - ASP.NET Core (API REST). Contendrá la API y la lógica de negocio: autenticación, reglas de compatibilidad, cálculo de disponibilidad, preasignación de cabinas, carrito, bloqueos temporales, citas, pagos, devoluciones y comunicación con la base de datos.

Base de datos - SQL Server. Almacenará usuarios, roles, tratamientos, cabinas, relaciones de compatibilidad, proveedores, carrito, reservas, pagos, devoluciones, horarios y demás información necesaria para mantener la operación del sistema.

La arquitectura general será cliente-servidor: Angular → API REST desarrollada en ASP.NET Core → SQL Server. El backend expone la API REST v1 (prefijo /api/v1) documentada en Diseño de API REST TZISCA, y accede a SQL Server según el Diccionario de Datos y Modelo Lógico TZISCA. Esta separación permite organizar la presentación, las reglas de negocio y la persistencia de datos en componentes diferenciados.

Stripe será la pasarela inicial y se integrará detrás de la abstracción PaymentService. ASP.NET Core Identity gestionará la autenticación con JWT como access token y refresh token seguro.

## 10. Metodología de desarrollo

Se empleará una metodología híbrida que combine Waterfall y Ágil. Waterfall se utilizará al inicio para definir la problemática, levantar y documentar requerimientos, delimitar el alcance y establecer el diseño base de la arquitectura y la base de datos. Con ello se contará con una referencia estable antes de iniciar la construcción.

A partir de esa base, el desarrollo se realizará de forma ágil e incremental. Las funcionalidades se implementarán por módulos y cada incremento pasará por un ciclo breve de planificación, desarrollo, pruebas, revisión con el responsable y ajustes. De esta manera se conserva la documentación y orden de Waterfall, pero se permite adaptar el producto conforme aparezca retroalimentación durante el desarrollo.

Figura 3. Aplicación de la metodología híbrida Waterfall + Ágil.

## 11. Alcance de la primera versión

La primera versión cubrirá el flujo completo desde la consulta del tratamiento hasta la confirmación y consulta de la cita, incluyendo pago, cancelación y devolución cuando corresponda. Se consideran dentro del alcance base:

- Registro, autenticación y roles (Cliente, Administrador general, Recepción y cabinas, Proveedor de tratamiento).

- Masajes terapéuticos, tratamientos faciales, hidroterapia, sauna, cabinas integrales/multifuncionales y de sal/haloterapia.

- Administración de tratamientos y cabinas.

- Descripciones, características y beneficios de tratamientos y cabinas.

- Compatibilidad tratamiento-cabina y preasignación automática.

- Recomendaciones informativas de cabinas alternativas.

- Carrito de selección previo a la confirmación.

- Selección del número de personas, fecha y hora.

- Validación de disponibilidad por intervalo y prevención de traslapes.

- Bloqueo temporal de recursos durante el proceso de carrito y pago.

- Visualización de horarios alternativos.

- Creación, consulta y gestión de Citas (PENDIENTE, CONFIRMADA, EN_ATENCION, COMPLETADA, CANCELADA o EXPIRADA).

- Pago de la cita cuando corresponda y consulta de su estado.

- Cancelaciones de tratamientos o de la cita completa.

- Devoluciones totales o parciales derivadas de cancelaciones con pago aprobado.

- Administración de proveedores de tratamiento y sustitución automática ante indisponibilidades.

- Panel administrativo, historial de citas y reportes básicos.

Las funciones de recordatorios, notificaciones y reportes avanzados no cuentan con respaldo en los documentos funcionales actuales y quedan fuera del alcance de esta versión; podrán evaluarse en etapas posteriores si el proyecto lo requiere. La reprogramación directa de una cita tampoco forma parte del alcance: conforme a las reglas de negocio vigentes, una modificación se realiza cancelando el tratamiento o la cita correspondiente y generando una nueva.

Los 43 casos de uso documentados (CU-01 a CU-43) representan el alcance funcional general del proyecto tal como se encuentra definido en este momento; no necesariamente se implementarán en un único sprint, sino de forma incremental conforme a la metodología híbrida descrita en la sección 10.

## 12. Entidades de información

El Diccionario de Datos y Modelo Lógico TZISCA define 19 entidades oficiales: Rol, Usuario, Cliente, PreferenciaCliente, Tratamiento, Carrito, Proveedor, TratamientoProveedor, DisponibilidadProveedor, Paquete, PaqueteTratamiento, Cabina, EstadoCabina, Cita, CitaCabina, Pago, Cancelacion, Devolucion y Transaccion. Se distribuyen en seguridad, catalogo, reservas, operacion y pagos.

## 13. Resultado esperado

Al finalizar el proyecto se espera contar con una aplicación web funcional que permita administrar los recursos principales de un spa y completar una cita de principio a fin. El cliente dispondrá de información suficiente para comprender los tratamientos y las cabinas compatibles, recibirá una propuesta automática de cabina con alternativas, podrá elegir un horario y conocer inmediatamente si se encuentra disponible.

Para la operación interna, la plataforma centralizará tratamientos, cabinas, usuarios, proveedores y citas, aplicando reglas que reduzcan conflictos de agenda y aseguren que el recurso seleccionado sea compatible con el servicio solicitado. El resultado será un producto que combine administración de recursos, control de disponibilidad y orientación al cliente dentro de un mismo flujo web.

El resultado incluye además el ciclo completo de pago, cancelación y devolución cuando corresponda, así como la sustitución de proveedores ante indisponibilidades, conforme al alcance funcional vigente (CU-01 a CU-43).

## 14. Decisiones aprobadas y verificación técnica

Las decisiones operativas, económicas y técnicas de DP-OP-01 a DP-OP-13, DP-EC-01, DP-EC-02 y DP-TEC-01 a DP-TEC-03 ya están resueltas en RN-91 a RN-108. Solo queda verificar su traducción exacta contra los scripts SQL y la configuración de despliegue cuando estén disponibles.

- Necesidad de notificaciones y recordatorios, no contemplados en el alcance funcional actual.

## 15. Modelo de información vigente

- seguridad: Rol, Usuario, Cliente y PreferenciaCliente.

- catalogo: Tratamiento, Paquete y PaqueteTratamiento.

- reservas: Carrito, Cita y CitaCabina.

- operacion: Proveedor, TratamientoProveedor, DisponibilidadProveedor, Cabina y EstadoCabina.

- pagos: Pago, Cancelacion, Devolucion y Transaccion.

- Total oficial: 19 entidades. Cliente 1:N Cita; Tratamiento N:M Proveedor; Paquete N:M Tratamiento; Cita N:M Cabina.

## 16. Decisiones operativas aprobadas

- Operación de lunes a sábado, 09:00 a 20:00; ningún tratamiento termina después de las 20:00.

- Inicios cada 30 minutos, anticipación de 2 horas a 60 días, bloqueo de 15 minutos y tolerancia de 15 minutos.

- Política de devolución por tramos de 100 %, 50 % y 0 %, con 100 % cuando la causa sea atribuible al spa.

- Precio base por persona, sin cargos adicionales en el MVP.

- ASP.NET Core Identity, JWT, refresh token seguro, Stripe detrás de PaymentService y recomendación determinista.
