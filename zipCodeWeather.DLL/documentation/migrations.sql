/* 06.08.2021 */
/* DB and query table creation */
CREATE DATABASE zipCodeWeather;
GO
USE zipCodeWeather;
GO
CREATE TABLE Queries(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NULL,
	[Temperature] [float] NULL,
	[TimeZone] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[ErrorMessage] [nvarchar](200) NULL,
	[Requested] [datetime] NOT NULL,
 CONSTRAINT [PK_Queries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/* get connection string */
select
    'data source=' + @@servername +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=' + suser_name() + ';password=<<YourPassword>>'
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()