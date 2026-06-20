# Web API Consulta de Datos

## Descripción

Web API Consulta de Datos es una aplicación de escritorio desarrollada en **C# con Windows Forms** que permite consumir servicios web REST para validar información académica y personal. El sistema realiza consultas a una API mediante peticiones HTTP y muestra los resultados de forma visual e intuitiva.

La aplicación permite validar grupos universitarios y CURP, mostrando los resultados en tiempo real y notificando el estado de la conexión con el servidor.

---

## Características

* Validación de grupos mediante Web API.
* Validación de CURP mediante Web API.
* Configuración dinámica del puerto de conexión.
* Consumo de servicios REST utilizando HttpClient.
* Procesamiento de respuestas JSON.
* Notificaciones emergentes de estado de conexión.
* Interfaz gráfica desarrollada con Windows Forms.
* Ejecución asíncrona para evitar bloqueos de la interfaz.
* Limpieza rápida de resultados y campos de entrada.

---

## Tecnologías Utilizadas

* C#
* .NET Framework
* Windows Forms
* REST API
* HttpClient
* JSON
* Newtonsoft.Json
* Programación Asíncrona (async/await)

---

## Funcionamiento

### Validación de Grupo

El usuario ingresa un grupo y la aplicación envía una solicitud HTTP GET al servicio web.

Ejemplo:

```http
http://localhost:51347/Universidad/ValidarGrupo?grupo=5A
```

La API responde:

```text
true
```

o

```text
false
```

El resultado se muestra en pantalla utilizando colores para facilitar su interpretación.

---

### Validación de CURP

El usuario introduce una CURP y la aplicación consulta el servicio web correspondiente.

Ejemplo:

```http
http://localhost:51347/Universidad/ValidarCURP?curp=XXXX000000HDFXXX00
```

La API devuelve una respuesta JSON con la información asociada al registro.

Ejemplo:

```json
{
  "nombre": "Juan Pérez",
  "curp": "XXXX000000HDFXXX00",
  "estatus": "Activo"
}
```

---

## Estructura del Proyecto

```text
Web_Api_Consulta_Datos/
│
├── Form1.cs
├── Form1.Designer.cs
├── Program.cs
├── Properties/
├── Resources/
└── App.config
```

---

## Componentes Principales

### HttpClient

Se utiliza para realizar peticiones HTTP hacia la API.

### Newtonsoft.Json

Permite procesar y manipular respuestas JSON recibidas desde el servidor.

### NotifyIcon

Muestra notificaciones emergentes sobre el estado de la conexión.

### Async/Await

Permite ejecutar consultas al servidor sin bloquear la interfaz gráfica.

---

## Requisitos

* Visual Studio 2019 o superior.
* .NET Framework compatible con Windows Forms.
* Servicio Web API en ejecución.
* Puerto configurado correctamente.

---

## Objetivo del Proyecto

Facilitar la consulta y validación de información mediante servicios web REST, proporcionando una interfaz amigable para usuarios que requieren verificar datos académicos y personales desde una aplicación de escritorio.

---

## Autor

Proyecto desarrollado en C# utilizando Windows Forms y tecnologías Web API para la integración de servicios REST y procesamiento de datos JSON.
