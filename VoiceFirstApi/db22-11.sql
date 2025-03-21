USE [master]
GO
/****** Object:  Database [dev_voice_first]    Script Date: 22-11-2024 12:24:01 ******/
CREATE DATABASE [dev_voice_first]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dev_voice_first', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\dev_voice_first.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dev_voice_first_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\dev_voice_first_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [dev_voice_first] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dev_voice_first].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dev_voice_first] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dev_voice_first] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dev_voice_first] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dev_voice_first] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dev_voice_first] SET ARITHABORT OFF 
GO
ALTER DATABASE [dev_voice_first] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dev_voice_first] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dev_voice_first] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dev_voice_first] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dev_voice_first] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dev_voice_first] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dev_voice_first] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dev_voice_first] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dev_voice_first] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dev_voice_first] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dev_voice_first] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dev_voice_first] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dev_voice_first] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dev_voice_first] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dev_voice_first] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dev_voice_first] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dev_voice_first] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dev_voice_first] SET RECOVERY FULL 
GO
ALTER DATABASE [dev_voice_first] SET  MULTI_USER 
GO
ALTER DATABASE [dev_voice_first] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dev_voice_first] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dev_voice_first] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dev_voice_first] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dev_voice_first] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [dev_voice_first] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'dev_voice_first', N'ON'
GO
ALTER DATABASE [dev_voice_first] SET QUERY_STORE = ON
GO
ALTER DATABASE [dev_voice_first] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [dev_voice_first]
GO
/****** Object:  Table [dbo].[t1_company]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t1_company](
	[id_t1_company] [nvarchar](191) NOT NULL,
	[t1_company_name] [nvarchar](50) NOT NULL,
	[id_company_type] [nvarchar](191) NOT NULL,
	[id_currency] [nvarchar](191) NOT NULL,
	[is_delete] [int] NULL,
	[is_active_till_date] [datetime] NOT NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t2_1_country]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t2_1_country](
	[id_t2_1_country] [nvarchar](191) NOT NULL,
	[t2_1_country_name] [nvarchar](50) NOT NULL,
	[t2_1_div1_called] [nvarchar](50) NULL,
	[t2_1_div2_called] [nvarchar](50) NULL,
	[t2_1_div3_called] [nvarchar](50) NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t2_1_div1]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t2_1_div1](
	[id_t2_1_div1] [nvarchar](191) NOT NULL,
	[id_t2_1_country] [nvarchar](191) NOT NULL,
	[t2_1_div1_name] [nvarchar](50) NOT NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t2_1_div2]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t2_1_div2](
	[id_t2_1_div2] [nvarchar](191) NOT NULL,
	[id_t2_1_div1] [nvarchar](191) NOT NULL,
	[t2_1_div2_name] [nvarchar](50) NOT NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t2_1_div3]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t2_1_div3](
	[id_t2_1_div3] [nvarchar](191) NOT NULL,
	[id_t2_1_div2] [nvarchar](191) NOT NULL,
	[t2_1_div3_name] [nvarchar](50) NOT NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t2_company_Locations]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t2_company_Locations](
	[id_t2_company_Locations] [nvarchar](191) NOT NULL,
	[id_t1_company] [nvarchar](191) NOT NULL,
	[t2_company_location_name] [nvarchar](50) NOT NULL,
	[id_Location_type] [nvarchar](191) NOT NULL,
	[t2_address_1] [nvarchar](50) NOT NULL,
	[t2_address_2] [nvarchar](50) NULL,
	[t2_zip_code] [nvarchar](10) NOT NULL,
	[t2_mobile_no] [nvarchar](15) NULL,
	[t2_phone_no] [nvarchar](15) NULL,
	[t2_email] [nvarchar](50) NOT NULL,
	[id_t2_1_country] [nvarchar](191) NOT NULL,
	[id_t2_1_div1] [nvarchar](191) NULL,
	[id_t2_1_div2] [nvarchar](191) NULL,
	[id_t2_1_div3] [nvarchar](191) NULL,
	[inserted_by] [nvarchar](191) NULL,
	[inserted_date] [datetime] NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[test]    Script Date: 22-11-2024 12:24:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[test](
	[id] [nvarchar](191) NOT NULL,
	[name] [nchar](50) NULL,
	[inserted_by] [nvarchar](191) NOT NULL,
	[inserted_date] [datetime] NOT NULL,
	[updated_by] [nvarchar](191) NULL,
	[updated_date] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[t1_company] ADD  CONSTRAINT [DF_t1_company_is_delete]  DEFAULT ((0)) FOR [is_delete]
GO
USE [master]
GO
ALTER DATABASE [dev_voice_first] SET  READ_WRITE 
GO
