USE [prueba_tecnica]
GO
/****** Object:  StoredProcedure [dbo].[sp_users]    Script Date: 28/08/2023 5:28:23 p. m. ******/
DROP PROCEDURE [dbo].[sp_users]
GO
/****** Object:  StoredProcedure [dbo].[sp_application_user]    Script Date: 28/08/2023 5:28:23 p. m. ******/
DROP PROCEDURE [dbo].[sp_application_user]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF__Users__FechaCrea__4222D4EF]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF__Users__Estado__412EB0B6]
GO
ALTER TABLE [dbo].[ApplicationUser] DROP CONSTRAINT [DF__Applicatio__Uuid__4D94879B]
GO
/****** Object:  Index [IX_Users_Uuid]    Script Date: 28/08/2023 5:28:23 p. m. ******/
DROP INDEX [IX_Users_Uuid] ON [dbo].[Users]
GO
/****** Object:  Index [IDX_ApplicationUser_Uuid]    Script Date: 28/08/2023 5:28:23 p. m. ******/
DROP INDEX [IDX_ApplicationUser_Uuid] ON [dbo].[ApplicationUser]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28/08/2023 5:28:23 p. m. ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[ApplicationUser]    Script Date: 28/08/2023 5:28:23 p. m. ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationUser]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationUser]
GO
USE [master]
GO
/****** Object:  Database [prueba_tecnica]    Script Date: 28/08/2023 5:28:23 p. m. ******/
DROP DATABASE [prueba_tecnica]
GO
/****** Object:  Database [prueba_tecnica]    Script Date: 28/08/2023 5:28:23 p. m. ******/
CREATE DATABASE [prueba_tecnica]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'prueba_tecnica', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.ABARBOSA\MSSQL\DATA\prueba_tecnica.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'prueba_tecnica_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.ABARBOSA\MSSQL\DATA\prueba_tecnica_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [prueba_tecnica] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [prueba_tecnica].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [prueba_tecnica] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [prueba_tecnica] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [prueba_tecnica] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [prueba_tecnica] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [prueba_tecnica] SET ARITHABORT OFF 
GO
ALTER DATABASE [prueba_tecnica] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [prueba_tecnica] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [prueba_tecnica] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [prueba_tecnica] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [prueba_tecnica] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [prueba_tecnica] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [prueba_tecnica] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [prueba_tecnica] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [prueba_tecnica] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [prueba_tecnica] SET  DISABLE_BROKER 
GO
ALTER DATABASE [prueba_tecnica] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [prueba_tecnica] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [prueba_tecnica] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [prueba_tecnica] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [prueba_tecnica] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [prueba_tecnica] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [prueba_tecnica] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [prueba_tecnica] SET RECOVERY FULL 
GO
ALTER DATABASE [prueba_tecnica] SET  MULTI_USER 
GO
ALTER DATABASE [prueba_tecnica] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [prueba_tecnica] SET DB_CHAINING OFF 
GO
ALTER DATABASE [prueba_tecnica] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [prueba_tecnica] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [prueba_tecnica] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [prueba_tecnica] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'prueba_tecnica', N'ON'
GO
ALTER DATABASE [prueba_tecnica] SET QUERY_STORE = ON
GO
ALTER DATABASE [prueba_tecnica] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [prueba_tecnica]
GO
/****** Object:  Table [dbo].[ApplicationUser]    Script Date: 28/08/2023 5:28:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uuid] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[Telefono] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28/08/2023 5:28:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uuid] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[FechaNacimiento] [date] NULL,
	[Sexo] [char](1) NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[FechaModificacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ApplicationUser] ON 
GO
INSERT [dbo].[ApplicationUser] ([Id], [Uuid], [Nombre], [Email], [PasswordHash], [Telefono]) VALUES (1, N'972f81db-1712-48c4-b202-ca5fd8b22515', N'Admin', N'admin@admin.com', N'$2a$11$9964iPqldculkpPmHm6EyurRMf90llG6Oua1vz82K2Rg/hVnIXXnK', N'3132061200')
GO
INSERT [dbo].[ApplicationUser] ([Id], [Uuid], [Nombre], [Email], [PasswordHash], [Telefono]) VALUES (2, N'859a0f3c-bb6a-4126-892d-6dd54fb6a735', N'admin2', N'admin2@admin2.con', N'$2a$11$4rzNdhZFh.GMv.Zs7s7pMubwb4/C7/lxEY24yzMumoks1kuFkJL3C', N'3138361187')
GO
SET IDENTITY_INSERT [dbo].[ApplicationUser] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (7, N'2d5e2c14-fe25-4bc3-94f8-6d5f6cbe1fea', N'Usuario_1', CAST(N'2023-07-28' AS Date), N'M', 1, CAST(N'2023-07-28T17:20:29.723' AS DateTime), CAST(N'2023-08-01T16:24:43.280' AS DateTime))
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (8, N'84c14344-08c4-469e-9f91-b4eab3ee2dcd', N'Usuario_2', CAST(N'2023-07-28' AS Date), N'M', 1, CAST(N'2023-07-28T19:18:27.647' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (10, N'b977e344-c9f7-4855-8da8-408ee4f3b96c', N'Usuario_3', CAST(N'2023-07-29' AS Date), N'F', 1, CAST(N'2023-07-28T19:24:18.420' AS DateTime), CAST(N'2023-08-01T16:07:51.567' AS DateTime))
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (11, N'0248120f-eae0-4d63-bfbb-ae71b2fdcaaa', N'Usuario_4', CAST(N'2023-07-29' AS Date), N'F', 1, CAST(N'2023-07-29T09:30:24.717' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (12, N'62e04ef5-dd7b-4846-8ded-9f323fe8dfac', N'Usuario_5', CAST(N'2023-07-29' AS Date), N'M', 1, CAST(N'2023-07-29T10:31:06.000' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (13, N'58de2acf-9f44-4d6f-9822-2e69dd812f76', N'Usuario_6', CAST(N'2023-06-26' AS Date), N'M', 1, CAST(N'2023-07-29T10:41:52.133' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (14, N'b51b722e-ac29-4704-8228-f117712657d6', N'Usuario_7', CAST(N'2023-06-26' AS Date), N'M', 1, CAST(N'2023-07-29T11:07:47.327' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (15, N'134fd8b6-5cdb-42c0-a006-a6ff9b2c41ef', N'Usuario_8', CAST(N'2023-07-26' AS Date), N'M', 1, CAST(N'2023-07-30T22:08:33.547' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (16, N'63128dca-b29b-4401-acef-5a81d9a25b03', N'Usuario_9', CAST(N'2023-07-27' AS Date), N'M', 1, CAST(N'2023-07-31T12:08:07.470' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (18, N'ef7c39c2-22c4-4c33-b164-de870cc60950', N'Usuario_10', CAST(N'2023-08-09' AS Date), N'M', 1, CAST(N'2023-08-01T12:19:17.900' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (19, N'6049385c-85a4-4b03-af2b-3e899093514e', N'Usuario_11', CAST(N'2023-08-26' AS Date), N'M', 1, CAST(N'2023-08-01T12:19:43.403' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (20, N'e6bc241d-b990-42e4-b189-07de15784ab4', N'Usuario_12', CAST(N'2023-08-31' AS Date), N'M', 1, CAST(N'2023-08-01T12:20:28.987' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (21, N'd9104a46-c692-424f-afe6-10ca2a5bb5a1', N'Usuario_13', CAST(N'2023-08-11' AS Date), N'F', 1, CAST(N'2023-08-01T14:39:48.790' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (22, N'413529f7-8b5d-47cc-a21e-ffe5549599d8', N'Usuario_14', CAST(N'2023-08-11' AS Date), N'F', 1, CAST(N'2023-08-01T14:40:21.613' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (23, N'09b49b57-773c-4a7a-97e5-27cf434f71a0', N'Usuario_15', CAST(N'2023-08-11' AS Date), N'F', 1, CAST(N'2023-08-01T14:40:38.077' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (24, N'688d4b88-272b-427f-bcc7-7b726c0b2227', N'Usuario_16', CAST(N'2023-08-11' AS Date), N'F', 1, CAST(N'2023-08-01T14:41:58.310' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([Id], [Uuid], [Nombre], [FechaNacimiento], [Sexo], [Estado], [FechaCreacion], [FechaModificacion]) VALUES (25, N'1d904778-bc28-483c-b336-ca6a6f26409e', N'Usuario_17', CAST(N'2023-08-03' AS Date), N'M', 1, CAST(N'2023-08-01T14:43:06.243' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IDX_ApplicationUser_Uuid]    Script Date: 28/08/2023 5:28:24 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [IDX_ApplicationUser_Uuid] ON [dbo].[ApplicationUser]
(
	[Uuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_Uuid]    Script Date: 28/08/2023 5:28:24 p. m. ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_Uuid] ON [dbo].[Users]
(
	[Uuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApplicationUser] ADD  DEFAULT (newid()) FOR [Uuid]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
/****** Object:  StoredProcedure [dbo].[sp_application_user]    Script Date: 28/08/2023 5:28:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_application_user]
    @Accion CHAR(1),
    @Uuid UNIQUEIDENTIFIER = NULL OUTPUT,
    @Nombre NVARCHAR(256) = NULL,
    @Email NVARCHAR(256) = NULL,
    @PasswordHash NVARCHAR(MAX) = NULL,
    @Telefono NVARCHAR(MAX) = NULL,    
    @TotalRecords INT = NULL OUTPUT,
    @RegistroCreado BIT = NULL OUTPUT,
    @RegistroActualizado BIT = NULL OUTPUT,
    @RegistroEliminado BIT = NULL OUTPUT,
	@PageNumber INT = NULL,
    @PageSize INT = NULL
AS
BEGIN
    IF @Accion = 'C'
    BEGIN
        BEGIN TRY
            INSERT INTO ApplicationUser (Uuid, Nombre, Email, PasswordHash, Telefono)
            VALUES (@Uuid, @Nombre, @Email, @PasswordHash, @Telefono)
            SET @RegistroCreado = 1;
            SET @Uuid = @Uuid; 
        END TRY
        BEGIN CATCH
            SET @RegistroCreado = 0;
            SET @Uuid = NULL; 
        END CATCH
    END
    ELSE IF @Accion = 'R'
    BEGIN
        IF (@PageNumber IS NULL OR @PageSize IS NULL)
        BEGIN
            SELECT *, ROW_NUMBER() OVER (ORDER BY Id) as RowNum 
            FROM ApplicationUser 
            WHERE Email = @Email
        END
        ELSE
        BEGIN
            SELECT *,
                COUNT(*) OVER () AS TotalRecords
            FROM (
                SELECT 
                    Id
                    ,Uuid
                    ,Nombre
                    ,Email
                    ,PasswordHash
                    ,Telefono
                    ,ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
                FROM ApplicationUser
            ) AS temp_table
            WHERE temp_table.RowNum > (@PageNumber - 1) * @PageSize AND temp_table.RowNum <= @PageNumber * @PageSize;

            SELECT @TotalRecords = COUNT(*) OVER () FROM ApplicationUser;
        END
    END
    ELSE IF @Accion = 'U'
    BEGIN
        BEGIN TRY
            UPDATE ApplicationUser
            SET Nombre = @Nombre,
                Email = @Email,
                PasswordHash = @PasswordHash,
                Telefono = @Telefono
            WHERE Uuid = @Uuid;

            SET @RegistroActualizado = 1;
            SET @Uuid = @Uuid; 
        END TRY
        BEGIN CATCH
            SET @RegistroActualizado = 0;
            SET @Uuid = NULL; 
        END CATCH
    END
    ELSE IF @Accion = 'D'
    BEGIN
        BEGIN TRY
            DELETE FROM ApplicationUser WHERE Uuid = @Uuid;
            SET @RegistroEliminado = 1;
            SET @Uuid = @Uuid;
        END TRY
        BEGIN CATCH
            SET @RegistroEliminado = 0;
            SET @Uuid = NULL; 
        END CATCH
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_users]    Script Date: 28/08/2023 5:28:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_users]
    @Accion CHAR(1),
    @Id INT = NULL OUTPUT,
    @Uuid UNIQUEIDENTIFIER = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaNacimiento DATE = NULL,
    @Sexo CHAR(1) = NULL,
    @Estado BIT = NULL,
    @PageNumber INT = NULL,
    @PageSize INT = NULL,
    @TotalRecords INT = NULL OUTPUT
AS
BEGIN
    IF @Accion = 'C'
    BEGIN
        INSERT INTO Users (Uuid, Nombre, FechaNacimiento, Sexo, Estado)
        VALUES (@Uuid, @Nombre, @FechaNacimiento, @Sexo, @Estado)

        SET @Id = SCOPE_IDENTITY()
    END
    ELSE IF @Accion = 'R'
    BEGIN
        IF (@PageNumber IS NULL OR @PageSize IS NULL)
        BEGIN
			SELECT *, ROW_NUMBER() OVER (ORDER BY Id) as RowNum 
			FROM Users 
			WHERE Uuid = @Uuid        
        END
        ELSE
        BEGIN
		 SELECT *,
				   COUNT(*) OVER () AS TotalRecords
			FROM (
				SELECT 
					Id
					,Uuid
					,Nombre
					,cast(FechaNacimiento as date) as FechaNacimiento
					,Sexo
					,Estado
					,FechaCreacion
					,FechaModificacion
					,ROW_NUMBER() OVER (ORDER BY Id) AS RowNum
				FROM Users
			) AS temp_table
				WHERE temp_table.RowNum > (@PageNumber - 1) * @PageSize AND temp_table.RowNum <= @PageNumber * @PageSize;

				SELECT @TotalRecords = COUNT(*) OVER () FROM Users;
        END
    END
    ELSE IF @Accion = 'U'
    BEGIN
        UPDATE Users SET             
            Nombre = @Nombre, 
            FechaNacimiento = @FechaNacimiento, 
            Sexo = @Sexo,             
            FechaModificacion = GETDATE()
        WHERE Uuid = @Uuid
    END
    ELSE IF @Accion = 'D'
    BEGIN
        DELETE FROM Users WHERE Uuid = @Uuid
    END
END
GO
USE [master]
GO
ALTER DATABASE [prueba_tecnica] SET  READ_WRITE 
GO
