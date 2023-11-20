

-- Crear la tabla de Roles
CREATE TABLE Rol (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);

-- Insertar roles iniciales (puedes ajustar según tus necesidades)
INSERT INTO Rol (Nombre) VALUES ('Admin'), ('Usuario');



-- Crear la tabla de Usuario
CREATE TABLE Usuario (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellidos VARCHAR(50) NOT NULL,
    Correo VARCHAR(50) UNIQUE NOT NULL,
    Contraseña VARCHAR(256) NOT NULL,
    Celular VARCHAR(15) NOT NULL,
    RolID INT,
    FOREIGN KEY (RolID) REFERENCES Rol(RolID)
);


Insert into Usuario values ('Olainy','West','OlainyWest@admin.com','03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4','809-120-0320', 1); --Contrase;a es 1234 rol ADMIN
Insert into Usuario values ('Maireni','West','MaireniWest2@admin.com','03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4','809-120-0320', 2); --Contrase;a es 1234 rol ADMIN

-- Crear la tabla de Vehículo
CREATE TABLE Vehiculo (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Tipo VARCHAR(50) NOT NULL,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    Año INT NOT NULL,
    Color VARCHAR(20) NOT NULL,
    Condicion VARCHAR(20) NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Imagen VARCHAR(100), -- Puedes ajustar la longitud según tus necesidades
    Descripcion VARCHAR(255) NOT NULL,
    PropietarioID INT,
    Vendido BIT NOT NULL DEFAULT 0, -- Nueva columna para indicar si el vehículo ha sido vendido
    FOREIGN KEY (PropietarioID) REFERENCES Usuario(ID)
);

-- Crear la tabla de TipoTransaccion
CREATE TABLE TipoTransaccion (
    TipoTransaccionID INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);

-- Insertar tipos de transacción iniciales
INSERT INTO TipoTransaccion (Nombre) VALUES ('Venta'), ('Compra');

-- Crear la tabla de Transaccion
CREATE TABLE Transaccion (
    ID INT PRIMARY KEY IDENTITY(1,1),
    TipoTransaccionID INT NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    FechaTransaccion DATETIME NOT NULL,
    VehiculoID INT,
    ClienteID INT,
    FOREIGN KEY (TipoTransaccionID) REFERENCES TipoTransaccion(TipoTransaccionID),
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculo(ID),
    FOREIGN KEY (ClienteID) REFERENCES Usuario(ID)
);