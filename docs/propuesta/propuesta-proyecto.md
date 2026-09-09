UNIVERSIDAD AUTÓNOMA DE CHIAPAS\
\
Escuela de Tecnologías Digitales Aplicadas\
\
Ingeniería en Desarrollo y Tecnologías de Software\
\
5.º semestre, Grupo B

PROPUESTA DE PROYECTO DE PRÁCTICAS PROFESIONALES

TZISCA — Sistema Web de Reservas, Recomendación y Gestión de Cabinas para Spa

Empresa: Time Tracker\
\
Alumna: Guadalupe Orozco Hernández\
\
Responsable del proyecto: Adán Núñez\
\
Fecha: 14 de agosto de 2026

> ***Nota de versión.** Versión actualizada al 08/09/2026 para que la propuesta general coincida con el alcance funcional vigente del proyecto. Se actualizaron: el nombre del proyecto (TZISCA, ya no provisional); los cuatro roles del sistema (se incorpora Recepción y cabinas); el alcance funcional (carrito, bloqueo temporal, pagos, consulta de estado de pago, cancelaciones, devoluciones, sustitución de proveedores y reportes básicos); el flujo principal del cliente; los requerimientos funcionales (RF-17 en adelante, sin renumerar los existentes); la arquitectura (Angular → ASP.NET Core REST API → SQL Server); y los aspectos pendientes de aprobación. La problemática, el objetivo general y los objetivos específicos no se modificaron por seguir vigentes. Documento coherente con CU-01 a CU-43, Reglas de Negocio Horario y Políticas TZISCA (RN-01 a RN-90), el Diccionario de Datos y Modelo Lógico TZISCA y el Diseño de API REST TZISCA v1.*

# 1. Descripción del producto

El producto propuesto es una plataforma web para la administración y reservación de servicios y cabinas de un spa. Su propósito es reunir en una sola solución el catálogo de tratamientos, la información de las cabinas, la disponibilidad por fecha y hora, los proveedores de tratamiento y las reservaciones realizadas por los clientes.

Desde la primera versión, el sistema contemplará masajes terapéuticos, tratamientos faciales, hidroterapia y sauna, así como cabinas integrales/multifuncionales y de sal/haloterapia, conforme al Catálogo de Cabinas TZISCA vigente. Cada tratamiento tendrá una duración, características, beneficios y reglas de compatibilidad con determinadas cabinas. A su vez, cada cabina contará con una ficha propia que explique su capacidad, características, beneficios y los tratamientos para los que puede utilizarse.

La reservación será asistida por el sistema. Después de elegir un tratamiento, la aplicación filtrará las cabinas compatibles y realizará una preasignación automática considerando el tipo de servicio, la capacidad requerida y la disponibilidad. Al mismo tiempo, mostrará otras cabinas compatibles junto con una explicación de sus características y beneficios, de modo que el cliente pueda comparar alternativas antes de confirmar.

El cliente elegirá la fecha y la hora deseada. El sistema validará la disponibilidad durante toda la duración del tratamiento; si existe un conflicto, informará que el horario está ocupado y mostrará horarios alternativos disponibles. La solución tendrá interfaces diferenciadas para clientes, administradores y proveedores de tratamiento.

El alcance funcional confirmado incorpora, además del flujo de recomendación y reservación descrito, un carrito previo a la reservación, bloqueos temporales de recursos durante el proceso, el pago de la reservación cuando corresponda y la consulta de su estado, cancelaciones de tratamientos o de la reservación completa, devoluciones totales o parciales derivadas de cancelaciones con pago aprobado, la gestión y sustitución de proveedores de tratamiento ante indisponibilidades, y reportes básicos de operación. La solución mantendrá interfaces diferenciadas para los cuatro roles del sistema: Cliente, Administrador general, Recepción y cabinas y Proveedor de tratamiento.

*Figura 1. Arquitectura general propuesta del producto.*

# 2. Problemática

La operación de un spa implica coordinar simultáneamente varios recursos que dependen entre sí: tratamientos con distinta duración, cabinas con capacidades y características específicas, proveedores de tratamiento, clientes y periodos de tiempo disponibles. Una reservación no consiste únicamente en registrar una fecha; requiere comprobar que el espacio sea adecuado para el servicio, que tenga capacidad suficiente y que permanezca libre durante todo el intervalo requerido.

Cuando la información de servicios, cabinas y horarios no se encuentra integrada en un mismo proceso, la consulta de disponibilidad se vuelve más lenta y aumenta el riesgo de traslapes. Dos reservaciones pueden aparentar utilizar horas distintas y aun así entrar en conflicto si la duración de los tratamientos se superpone. De igual forma, una cabina puede estar libre pero no ser compatible con el tratamiento solicitado o no tener la capacidad necesaria para el número de personas.

A esta dificultad operativa se suma un problema de información para el cliente. Un usuario puede conocer el nombre de un tratamiento, pero no necesariamente las diferencias entre las cabinas que podrían utilizarse, sus características, beneficios o el tipo de experiencia para la que fueron configuradas. Si esa información no se presenta durante la reservación, la selección depende de explicaciones externas o de una decisión poco informada.

También se requiere separar las responsabilidades de los distintos participantes. El cliente necesita consultar opciones y reservar; el administrador necesita mantener tratamientos, cabinas, horarios, usuarios y reservaciones; y el proveedor de tratamiento necesita relacionarse con los servicios que puede atender. Sin una gestión centralizada, la información puede quedar dispersa y dificultar el seguimiento de una reservación desde su solicitud hasta su atención.

Por lo anterior, la necesidad principal es contar con una plataforma que centralice la información y aplique reglas de negocio sobre compatibilidad, capacidad, duración y disponibilidad. Además de reducir conflictos de agenda, la solución debe orientar al cliente mediante recomendaciones informativas de cabinas y alternativas de horario, haciendo que el proceso de reservación sea más claro y que la administración interna tenga una fuente única de información.

# 3. Objetivo general

Diseñar y desarrollar una plataforma web integral para la gestión y reservación de servicios y cabinas de spa, que centralice usuarios, tratamientos, cabinas, proveedores y horarios; permita al cliente registrarse, consultar información detallada de los tratamientos, recibir una preasignación automática y recomendaciones de cabinas compatibles, seleccionar una fecha y una hora, validar la disponibilidad durante toda la duración del servicio y confirmar su reservación; y proporcione al personal administrativo herramientas para mantener catálogos, disponibilidad y reservas desde una misma solución.

# 4. Objetivos específicos

- Implementar autenticación y control de acceso para clientes, administradores y proveedores de tratamiento.

- Administrar un catálogo de tratamientos con descripción, duración, beneficios y condiciones de uso dentro del sistema.

- Administrar cabinas con información de capacidad, características, beneficios, estado y tratamientos compatibles.

- Aplicar reglas de compatibilidad entre tratamiento, cabina, número de personas y disponibilidad.

- Realizar una preasignación automática de cabina y mostrar alternativas compatibles explicando por qué pueden ser adecuadas.

- Permitir al cliente elegir fecha y hora y consultar, antes de confirmar, si el intervalo requerido se encuentra disponible.

- Mostrar horarios alternativos cuando la opción seleccionada tenga un conflicto de disponibilidad.

- Registrar, consultar y administrar reservaciones evitando traslapes de cabina.

- Gestionar proveedores de tratamiento y relacionarlos con los servicios que pueden atender.

- Proporcionar un panel administrativo para mantener la información operativa del sistema y consultar el historial de reservaciones.

# 5. Funcionalidades principales del sistema

Las funcionalidades se definen por módulo para evitar duplicidades entre alcance, módulos y requerimientos. Cada módulo representa una capacidad concreta del producto.

## 5.1 Registro, autenticación y roles

El sistema permitirá crear cuentas, iniciar y cerrar sesión y controlar las funciones disponibles según el rol. El cliente gestionará sus propias reservas; el Administrador general tendrá acceso a la configuración general, usuarios, tratamientos, cabinas, proveedores y reportes; Recepción y cabinas dispondrá de las funciones operativas de agenda, disponibilidad, reservaciones manuales, cancelaciones y estados de cabina; y el Proveedor de tratamiento dispondrá de las funciones relacionadas con los servicios que atiende.

## 5.2 Catálogo de tratamientos

Permitirá registrar y consultar masajes terapéuticos, tratamientos faciales, hidroterapia, sauna, así como servicios en cabinas integrales/multifuncionales y de sal/haloterapia desde la primera versión. Cada tratamiento tendrá nombre, descripción, duración, beneficios, recomendaciones informativas, capacidad o restricciones operativas y relación con tipos de cabina.

## 5.3 Catálogo de cabinas

Cada cabina tendrá una ficha propia con nombre o identificador, tipo, capacidad, descripción, características, beneficios, estado y tratamientos compatibles. El administrador podrá dar de alta, modificar o desactivar cabinas sin modificar el código de la aplicación.

## 5.4 Compatibilidad y recomendación de cabinas

Al seleccionar un tratamiento, el sistema filtrará automáticamente las cabinas que cumplen las reglas de compatibilidad y capacidad. De ese conjunto realizará una preasignación y mostrará alternativas con su descripción y beneficios. Las recomendaciones serán informativas y no constituirán diagnóstico ni prescripción médica.

## 5.5 Agenda y disponibilidad

El usuario seleccionará una fecha y una hora. La aplicación calculará la hora de finalización a partir de la duración del tratamiento y verificará que la cabina permanezca libre durante todo el intervalo. Los horarios que produzcan traslapes se considerarán ocupados.

## 5.6 Alternativas de horario

Si la hora elegida no está disponible, el sistema mostrará una lista de horarios que sí pueden completar la reserva con una cabina compatible. El cliente podrá seleccionar una alternativa sin reiniciar el proceso.

## 5.7 Reservaciones

La reservación funciona como encabezado (Reservacion) que agrupa uno o varios tratamientos independientes (ReservacionTratamiento), cada uno con su propia cabina, número de personas, horario y estado. La reservación se crea en estado EN_PROCESO y pasa a CONFIRMADA cuando el pago es aprobado y la disponibilidad se revalida; el sistema impedirá confirmar dos tratamientos que ocupen la misma cabina en periodos traslapados.

## 5.8 Gestión de proveedores de tratamiento

Permitirá registrar al personal que presta los servicios y relacionarlo con uno o varios tratamientos. La asignación inicial del proveedor a cada tratamiento reservado corresponde al Administrador general; si el proveedor asignado registra una indisponibilidad, el sistema buscará un sustituto disponible y autorizado, informando al cliente antes de aplicar el cambio definitivo (ver 5.17 Sustitución de proveedores).

## 5.9 Portal del cliente

El cliente podrá consultar tratamientos y cabinas, gestionar su carrito, realizar reservas, y consultar su estado, historial y estado de pago. Podrá cancelar un tratamiento específico o la reservación completa conforme a las políticas establecidas. La modificación de una reservación ya confirmada se realiza cancelando el tratamiento o la reservación correspondiente y generando una nueva; TZISCA no ofrece una función de reprogramación directa.

## 5.10 Panel administrativo

Permitirá gestionar usuarios, tratamientos, cabinas, relaciones de compatibilidad, horarios, bloqueos, proveedores, reservaciones, cancelaciones, pagos y devoluciones, diferenciando las funciones del Administrador general y de Recepción y cabinas. También incorporará reportes básicos (reservaciones por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas) a partir de la información transaccional del sistema.

*Figura 2. Flujo funcional de reservación asistida y validación de disponibilidad.*

## 5.11 Carrito de reservación

El cliente podrá agregar uno o varios tratamientos a un carrito antes de confirmar la reservación, indicando número de personas, cabina y horario provisionales para cada uno. Agregar un tratamiento al carrito no equivale a reservarlo; el cliente podrá agregar, eliminar o modificar elementos antes de confirmar.

## 5.12 Bloqueo temporal

Al seleccionar una cabina, fecha y hora válidas, el sistema aplicará un bloqueo temporal que protege el recurso mientras el cliente completa el carrito y, en su caso, el pago. El bloqueo se libera automáticamente si expira, si el cliente cambia su selección o si abandona el carrito. La duración del bloqueo se maneja como un parámetro operativo configurable, no como un valor fijo en el código (pendiente de aprobación, sección 14).

## 5.13 Pagos

Cuando la reservación requiera pago, el sistema lo solicitará y procesará antes de confirmar definitivamente los recursos. La reservación y sus tratamientos permanecen en EN_PROCESO/PENDIENTE hasta que el pago sea aprobado y la disponibilidad se revalide; un pago fallido, cancelado o rechazado no confirma la reservación. TZISCA no almacenará datos bancarios sensibles completos.

## 5.14 Consulta de estado de pago

El cliente y el personal autorizado (Administrador general y Recepción y cabinas, según sus permisos) podrán consultar el estado del pago asociado a una reservación en cualquier momento del proceso.

## 5.15 Cancelaciones

El cliente podrá cancelar un tratamiento específico o la reservación completa; Recepción y cabinas y el Administrador general también podrán realizar cancelaciones operativas. Cancelar libera automáticamente cabina, horario y proveedor asignado, sin eliminar el registro histórico ni el pago o devolución relacionados. Cancelar un tratamiento no cancela automáticamente los demás tratamientos de la misma reservación.

## 5.16 Devoluciones

Cuando una cancelación tenga un pago aprobado relacionado, el sistema podrá generar una devolución total (reservación completa) o parcial (tratamiento específico), conservando trazabilidad de monto, fecha, motivo, estado y usuario responsable. Las políticas exactas que determinan cuándo corresponde devolución permanecen sujetas a aprobación (sección 14).

## 5.17 Sustitución de proveedores

Si un proveedor asignado registra un periodo de indisponibilidad, el sistema buscará automáticamente un sustituto activo, autorizado para el tratamiento y disponible durante todo el horario requerido. El cliente será informado antes del cambio definitivo y podrá aceptar al nuevo proveedor, cancelar solo el tratamiento afectado o cancelar toda la reservación. Si no existe sustituto disponible, el sistema informará al cliente.

## 5.18 Reportes básicos

El sistema generará reportes básicos a partir de la información transaccional ya registrada: reservaciones por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas. No se define una entidad Reporte independiente; los reportes se calculan mediante consultas sobre las demás entidades.

## 5.19 Flujo principal del cliente

El flujo principal de reservación del cliente queda definido como sigue:

Login → Catálogo → Detalle → Carrito → Personas → Recomendación → Cabina → Fecha/hora → Disponibilidad → Bloqueo → Resumen → Crear reservación EN_PROCESO → Pago → Validación → Confirmación → Mis reservaciones.

Este flujo sustituye, para efectos de diseño, la descripción funcional general de las secciones 1 y 5.4–5.7: el cliente inicia sesión, navega el catálogo y el detalle de un tratamiento, lo agrega al carrito indicando número de personas, recibe la recomendación y selecciona cabina, fecha y hora, el sistema valida la disponibilidad y aplica un bloqueo temporal, el cliente revisa el resumen y el sistema crea la reservación en estado EN_PROCESO, procesa el pago cuando corresponde, valida el resultado y, si es aprobado junto con la disponibilidad, confirma la reservación; el cliente puede entonces consultarla en Mis reservaciones.

# 6. Usuarios del sistema

## 6.1 Cliente

- Registrarse e iniciar sesión.

- Consultar tratamientos y cabinas compatibles.

- Recibir preasignación y alternativas de cabina.

- Seleccionar número de personas, fecha y hora; gestionar su carrito y bloqueo temporal.

- Consultar disponibilidad y horarios alternativos.

- Confirmar su reservación, pagar cuando corresponda y consultar el estado del pago.

- Consultar sus reservaciones, cancelar tratamientos o la reservación completa, y dar seguimiento a devoluciones relacionadas.

## 6.2 Administrador general

- Gestionar usuarios y roles.

- Gestionar tratamientos, cabinas y relaciones de compatibilidad.

- Gestionar proveedores de tratamiento y su asignación inicial.

- Consultar y administrar todas las reservaciones, pagos y devoluciones según sus permisos.

- Consultar reportes básicos e información operativa para seguimiento.

## 6.3 Recepción y cabinas

- Gestionar la agenda, la disponibilidad y las reservaciones manuales.

- Realizar cancelaciones operativas y verificar pagos o devoluciones relacionados.

- Gestionar el estado operativo de las cabinas y los bloqueos por mantenimiento, limpieza u otra causa.

- Consultar el estado de pago de las reservaciones que gestione.

## 6.4 Proveedor de tratamiento

- Mantenerse relacionado con los tratamientos que puede atender.

- Consultar los tratamientos que tenga asignados.

- Registrar periodos de indisponibilidad, activando la búsqueda de un proveedor sustituto cuando corresponda.

- Participar en la operación del servicio sin acceso a funciones administrativas ajenas a su rol.

# 7. Reglas de negocio principales

Las siguientes reglas resumen los criterios estructurales del sistema a nivel de propuesta. El detalle completo y numerado (RN-01 a RN-90) se documenta en Reglas de Negocio Horario y Políticas TZISCA.

- Una cabina no podrá estar asociada a dos reservaciones cuyos periodos de tiempo se traslapen.

- La cabina utilizada deberá ser compatible con el tratamiento solicitado.

- La capacidad de la cabina deberá ser igual o superior al número de personas de la reservación.

- La disponibilidad se validará desde la hora de inicio hasta la hora de finalización calculada con la duración del tratamiento.

- Un horario solo se ofrecerá como disponible cuando exista por lo menos una cabina válida para completar el servicio.

- La preasignación automática deberá considerar, como mínimo, compatibilidad, capacidad y disponibilidad.

- Las recomendaciones de cabina mostrarán información registrada por el administrador y tendrán carácter orientativo, no clínico.

- Las reglas detalladas de cancelaciones, sustitución de proveedores, pagos y devoluciones se documentan en Reglas de Negocio Horario y Políticas TZISCA (RN-01 a RN-90). Las políticas específicas de tolerancia, cancelación con derecho a devolución y los parámetros operativos de horario y bloqueo temporal permanecen sujetos a aprobación (sección 14).

# 8. Requerimientos funcionales preliminares

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

- RF-12. Registrar una reservación con cliente, tratamiento, cabina, fecha, horas y estado.

- RF-13. Permitir al cliente consultar su historial de reservaciones.

- RF-14. Permitir al administrador consultar y gestionar reservaciones.

- RF-15. Administrar proveedores de tratamiento y su relación con servicios.

- RF-16. Permitir agregar nuevos tratamientos y cabinas mediante configuración administrativa.

- **RF-17. Permitir al cliente agregar uno o varios tratamientos a un carrito antes de confirmar la reservación, sin que ello constituya una reservación definitiva.**

- **RF-18. Generar un bloqueo temporal sobre la cabina, fecha y hora seleccionadas mientras el cliente completa el carrito y el pago, liberándolo automáticamente si expira o si la selección se modifica.**

- **RF-19. Crear la reservación en estado EN_PROCESO, con sus tratamientos en estado PENDIENTE, al confirmarse el resumen del carrito y antes de completar el pago.**

- **RF-20. Solicitar y procesar el pago de la reservación cuando el servicio lo requiera, confirmando la reservación y sus tratamientos únicamente cuando el pago sea aprobado y la disponibilidad se revalide.**

- **RF-21. Permitir al cliente y al personal autorizado consultar el estado del pago asociado a una reservación.**

- **RF-22. Permitir cancelar un tratamiento específico o una reservación completa, liberando los recursos asociados sin eliminar el registro histórico.**

- **RF-23. Registrar y gestionar devoluciones totales o parciales derivadas de una cancelación con pago aprobado.**

- **RF-24. Buscar y proponer un proveedor sustituto cuando el proveedor asignado registre una indisponibilidad, permitiendo al cliente aceptar el cambio, cancelar el tratamiento afectado o cancelar toda la reservación.**

- **RF-25. Generar reportes básicos de reservaciones por día, tratamientos más solicitados, cabinas más utilizadas, cancelaciones y ocupación general de cabinas.**

# 9. Tecnologías y arquitectura propuestas

Frontend - Angular. Se utilizará para la interfaz web y las vistas de clientes, administradores y proveedores, incluyendo formularios, catálogos, carrito, selección de horarios y paneles de gestión.

Backend - ASP.NET Core (API REST). Contendrá la API y la lógica de negocio: autenticación, reglas de compatibilidad, cálculo de disponibilidad, preasignación de cabinas, carrito, bloqueos temporales, reservaciones, pagos, devoluciones y comunicación con la base de datos.

Base de datos - SQL Server. Almacenará usuarios, roles, tratamientos, cabinas, relaciones de compatibilidad, proveedores, carrito, reservas, pagos, devoluciones, horarios y demás información necesaria para mantener la operación del sistema.

La arquitectura general será cliente-servidor: **Angular → API REST desarrollada en ASP.NET Core → SQL Server**. El backend expone la API REST v1 (prefijo /api/v1) documentada en Diseño de API REST TZISCA, y accede a SQL Server según el Diccionario de Datos y Modelo Lógico TZISCA. Esta separación permite organizar la presentación, las reglas de negocio y la persistencia de datos en componentes diferenciados.

La pasarela de pago concreta a integrar permanece pendiente de definición (DP-TEC-02); mientras no se apruebe, el módulo de pagos maneja los estados definidos (PENDIENTE, PROCESANDO, PAGADO, FALLIDO, CANCELADO, REEMBOLSADO, REEMBOLSADO_PARCIALMENTE) sin depender de un proveedor específico. Los parámetros operativos de horario, duración del bloqueo temporal y políticas de devolución también permanecen sujetos a aprobación (ver sección 14).

# 10. Metodología de desarrollo

Se empleará una metodología híbrida que combine Waterfall y Ágil. Waterfall se utilizará al inicio para definir la problemática, levantar y documentar requerimientos, delimitar el alcance y establecer el diseño base de la arquitectura y la base de datos. Con ello se contará con una referencia estable antes de iniciar la construcción.

A partir de esa base, el desarrollo se realizará de forma ágil e incremental. Las funcionalidades se implementarán por módulos y cada incremento pasará por un ciclo breve de planificación, desarrollo, pruebas, revisión con el responsable y ajustes. De esta manera se conserva la documentación y orden de Waterfall, pero se permite adaptar el producto conforme aparezca retroalimentación durante el desarrollo.

*Figura 3. Aplicación de la metodología híbrida Waterfall + Ágil.*

# 11. Alcance de la primera versión

La primera versión cubrirá el flujo completo desde la consulta del tratamiento hasta la confirmación y consulta de la reservación, incluyendo pago, cancelación y devolución cuando corresponda. Se consideran dentro del alcance base:

- Registro, autenticación y roles (Cliente, Administrador general, Recepción y cabinas, Proveedor de tratamiento).

- Masajes terapéuticos, tratamientos faciales, hidroterapia, sauna, cabinas integrales/multifuncionales y de sal/haloterapia.

- Administración de tratamientos y cabinas.

- Descripciones, características y beneficios de tratamientos y cabinas.

- Compatibilidad tratamiento-cabina y preasignación automática.

- Recomendaciones informativas de cabinas alternativas.

- Carrito de reservación previo a la confirmación.

- Selección del número de personas, fecha y hora.

- Validación de disponibilidad por intervalo y prevención de traslapes.

- Bloqueo temporal de recursos durante el proceso de carrito y pago.

- Visualización de horarios alternativos.

- Creación, consulta y gestión de reservaciones (EN_PROCESO / CONFIRMADA / CANCELADA / EXPIRADA).

- Pago de la reservación cuando corresponda y consulta de su estado.

- Cancelaciones de tratamientos o de la reservación completa.

- Devoluciones totales o parciales derivadas de cancelaciones con pago aprobado.

- Administración de proveedores de tratamiento y sustitución automática ante indisponibilidades.

- Panel administrativo, historial de reservaciones y reportes básicos.

Las funciones de recordatorios, notificaciones y reportes avanzados no cuentan con respaldo en los documentos funcionales actuales y quedan fuera del alcance de esta versión; podrán evaluarse en etapas posteriores si el proyecto lo requiere. La reprogramación directa de una reservación tampoco forma parte del alcance: conforme a las reglas de negocio vigentes, una modificación se realiza cancelando el tratamiento o la reservación correspondiente y generando una nueva.

Los 43 casos de uso documentados (CU-01 a CU-43) representan el alcance funcional general del proyecto tal como se encuentra definido en este momento; no necesariamente se implementarán en un único sprint, sino de forma incremental conforme a la metodología híbrida descrita en la sección 10.

# 12. Entidades de información

El diseño detallado de entidades, atributos, llaves y relaciones se documenta en el Diccionario de Datos y Modelo Lógico TZISCA, que sustituye la lista preliminar de la versión anterior de esta propuesta. Dicho documento define 23 entidades confirmadas (Rol, Usuario, PreferenciaCliente, TipoCabina, Cabina, Tratamiento, TratamientoCabina, Proveedor, ProveedorTratamiento, Carrito, CarritoTratamiento, BloqueoTemporal, Reservacion, ReservacionTratamiento, AsignacionProveedor, BloqueoCabina, HistorialEstadoCabina, IndisponibilidadProveedor, HistorialEstadoTratamiento, Cancelacion, Pago, Devolucion) más TransaccionPago como entidad opcional, aplicable únicamente si se integra una pasarela de pago externa. También documenta, como propuesta pendiente de aprobación de valores, las entidades operativas de agenda (ParametroOperativo, DiaLaborable, ExcepcionOperativa).

# 13. Resultado esperado

Al finalizar el proyecto se espera contar con una aplicación web funcional que permita administrar los recursos principales de un spa y completar una reservación de principio a fin. El cliente dispondrá de información suficiente para comprender los tratamientos y las cabinas compatibles, recibirá una propuesta automática de cabina con alternativas, podrá elegir un horario y conocer inmediatamente si se encuentra disponible.

Para la operación interna, la plataforma centralizará tratamientos, cabinas, usuarios, proveedores y reservaciones, aplicando reglas que reduzcan conflictos de agenda y aseguren que el recurso seleccionado sea compatible con el servicio solicitado. El resultado será un producto que combine administración de recursos, control de disponibilidad y orientación al cliente dentro de un mismo flujo web.

El resultado incluye además el ciclo completo de pago, cancelación y devolución cuando corresponda, así como la sustitución de proveedores ante indisponibilidades, conforme al alcance funcional vigente (CU-01 a CU-43).

# 14. Aspectos por validar con el responsable del proyecto

Los siguientes puntos ya resueltos en la versión anterior de esta propuesta —nombre del proyecto, catálogo inicial de tratamientos y cabinas, criterios de prioridad para la preasignación y forma de asignar al proveedor— quedan documentados en el Catálogo de Cabinas TZISCA, en Reglas de Negocio Horario y Políticas TZISCA (RN-17 a RN-21, RN-46 a RN-56) y en el Diccionario de Datos y Modelo Lógico TZISCA, por lo que se retiran de esta lista. Permanecen pendientes de aprobación:

- Horarios de apertura y cierre, días laborables, excepciones/días no laborables, intervalo de agenda, anticipación mínima y máxima para reservar, y duración del bloqueo temporal (DP-OP-01 a DP-OP-08).

- Política de tolerancia ante llegada tardía, política de cancelación, condiciones de devolución (total, parcial o ninguna) y tiempo límite para cancelar con derecho a devolución (DP-OP-09 a DP-OP-13).

- Fórmula exacta del importe por tratamiento: si precio_base corresponde al tratamiento completo o es por persona, y si existen cargos adicionales (DP-EC-01).

- Tratamiento económico cuando un pago fue aprobado pero el bloqueo temporal expiró y la disponibilidad se perdió (DP-EC-02).

- Mecanismo concreto de autenticación y gestión de sesión (DP-TEC-01).

- Proveedor o pasarela de pago concreta a integrar (DP-TEC-02).

- Algoritmo determinista de recomendación de cabinas, con sus criterios de desempate (DP-TEC-03).

- Necesidad de notificaciones y recordatorios, no contemplados en el alcance funcional actual.
