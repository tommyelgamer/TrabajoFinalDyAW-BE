USE [master]
GO
/****** Object:  Database [TrabajoFinalDyAW]    Script Date: 7/9/2024 11:43:48 AM ******/
CREATE DATABASE [TrabajoFinalDyAW]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TrabajoFinalDyAW', FILENAME = N'C:\Users\tommy\TrabajoFinalDyAW.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TrabajoFinalDyAW_log', FILENAME = N'C:\Users\tommy\TrabajoFinalDyAW_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TrabajoFinalDyAW] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TrabajoFinalDyAW].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ARITHABORT OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET  MULTI_USER 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TrabajoFinalDyAW] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TrabajoFinalDyAW] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TrabajoFinalDyAW] SET QUERY_STORE = OFF
GO
USE [TrabajoFinalDyAW]
GO
/****** Object:  Table [dbo].[merchandise]    Script Date: 7/9/2024 11:43:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[merchandise](
	[merchandise_id] [uniqueidentifier] NOT NULL,
	[merchandise_name] [varchar](255) NOT NULL,
	[merchandise_description] [varchar](255) NULL,
	[merchandise_barcode] [varchar](255) NOT NULL,
	[merchandise_stock] [int] NOT NULL,
 CONSTRAINT [PK_merchandise] PRIMARY KEY CLUSTERED 
(
	[merchandise_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[permissionclaim]    Script Date: 7/9/2024 11:43:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[permissionclaim](
	[permissionclaim_name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_permissionclaim] PRIMARY KEY CLUSTERED 
(
	[permissionclaim_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[refreshtoken]    Script Date: 7/9/2024 11:43:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refreshtoken](
	[refreshtoken_id] [uniqueidentifier] NOT NULL,
	[refreshtoken_value] [varchar](max) NULL,
	[refreshtoken_expire] [datetime] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_refreshtoken] PRIMARY KEY CLUSTERED 
(
	[refreshtoken_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 7/9/2024 11:43:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[user_id] [uniqueidentifier] NOT NULL,
	[user_username] [varchar](255) NOT NULL,
	[user_password] [varchar](255) NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userpermisssionclaim]    Script Date: 7/9/2024 11:43:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userpermisssionclaim](
	[userpermissionclaim_id] [uniqueidentifier] NOT NULL,
	[permissionclaim_name] [varchar](255) NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_userpermisssionclaim] PRIMARY KEY CLUSTERED 
(
	[userpermissionclaim_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'CREATE_MERCHANDISE')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'CREATE_USER')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'DELETE_MERCHANDISE')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'DELETE_USER')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'GET_MERCHANDISE')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'GET_USER')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'UPDATE_MERCHANDISE')
INSERT [dbo].[permissionclaim] ([permissionclaim_name]) VALUES (N'UPDATE_USER')
GO
INSERT [dbo].[user] ([user_id], [user_username], [user_password]) VALUES (N'6f22a109-bffc-4265-b75c-dc90b58d4e26', N'admin', N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9')
GO
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'90482d2f-6e80-4b5e-94bf-09a37ca2439a', N'DELETE_USER', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'fe7eecc0-c74e-415c-9387-3d16583d5b0b', N'UPDATE_USER', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'eac4ce2f-6324-4dfd-8651-86f9bf2cdbcf', N'CREATE_USER', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'e029656c-e1d5-491e-8b61-b4009b5b9ede', N'GET_USER', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'e029656c-e1d5-491e-8b61-b4009b5b9eda', N'GET_MERCHANDISE', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'e029656c-e1d5-491e-8b61-b4009b5b9edb', N'UPDATE_MERCHANDISE', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'e029656c-e1d5-491e-8b61-b4009b5b9edc', N'DELETE_MERCHANDISE', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
INSERT [dbo].[userpermisssionclaim] ([userpermissionclaim_id], [permissionclaim_name], [user_id]) VALUES (N'e029656c-e1d5-491e-8b61-b4009b5b9edd', N'CREATE_MERCHANDISE', N'6f22a109-bffc-4265-b75c-dc90b58d4e26')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_merchandise]    Script Date: 7/9/2024 11:43:48 AM ******/
ALTER TABLE [dbo].[merchandise] ADD  CONSTRAINT [IX_merchandise] UNIQUE NONCLUSTERED 
(
	[merchandise_barcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_user]    Script Date: 7/9/2024 11:43:48 AM ******/
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [IX_user] UNIQUE NONCLUSTERED 
(
	[user_username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[merchandise] ADD  CONSTRAINT [DF_merchandise_merchandise_id]  DEFAULT (newid()) FOR [merchandise_id]
GO
ALTER TABLE [dbo].[merchandise] ADD  CONSTRAINT [DF_merchandise_merchandise_stock]  DEFAULT ((0)) FOR [merchandise_stock]
GO
ALTER TABLE [dbo].[user] ADD  CONSTRAINT [DF_user_user_id]  DEFAULT (newid()) FOR [user_id]
GO
ALTER TABLE [dbo].[refreshtoken]  WITH CHECK ADD  CONSTRAINT [FK_refreshtoken_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[refreshtoken] CHECK CONSTRAINT [FK_refreshtoken_user]
GO
ALTER TABLE [dbo].[userpermisssionclaim]  WITH CHECK ADD  CONSTRAINT [FK_userpermisssionclaim_permissionclaim] FOREIGN KEY([permissionclaim_name])
REFERENCES [dbo].[permissionclaim] ([permissionclaim_name])
GO
ALTER TABLE [dbo].[userpermisssionclaim] CHECK CONSTRAINT [FK_userpermisssionclaim_permissionclaim]
GO
ALTER TABLE [dbo].[userpermisssionclaim]  WITH CHECK ADD  CONSTRAINT [FK_userpermisssionclaim_user] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[userpermisssionclaim] CHECK CONSTRAINT [FK_userpermisssionclaim_user]
GO
USE [master]
GO
ALTER DATABASE [TrabajoFinalDyAW] SET  READ_WRITE 
GO
