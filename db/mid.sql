USE [master]
GO
/****** Object:  Database [MidAtlanticFinanace-SolutionByText]    Script Date: 1/9/2017 9:39:45 PM ******/
CREATE DATABASE [MidAtlanticFinanace-SolutionByText]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MidAtlanticFinanace-SolutionByText', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\MidAtlanticFinanace-SolutionByText.mdf' , SIZE = 30272KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MidAtlanticFinanace-SolutionByText_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\MidAtlanticFinanace-SolutionByText_log.ldf' , SIZE = 136064KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MidAtlanticFinanace-SolutionByText].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ARITHABORT OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET RECOVERY FULL 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET  MULTI_USER 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'MidAtlanticFinanace-SolutionByText', N'ON'
GO
USE [MidAtlanticFinanace-SolutionByText]
GO
/****** Object:  StoredProcedure [dbo].[SBTGetAdditionalNotifications]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu Team
-- Create date: 8/09/2016
-- Description:	Stored procedure to get additional notifications based on account number
-- =============================================
CREATE PROCEDURE [dbo].[SBTGetAdditionalNotifications] 
	@AccountNumber nvarchar(50)
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT SubscriberID,
	TextNumber,
	IsPaymentReminderNotification,
	IsPaymentConfirmationNotification, 
	IsPayByTextNotification,
	IsChatByTextNotification as IsCommunicationByTextNotification
	FROM tblSBTSubscriber 
	WHERE AccountNumber = @AccountNumber 
	AND IsActive = 1
	ORDER BY CreatedDate DESC
END

GO
/****** Object:  StoredProcedure [dbo].[SBTGetSecurityToken]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CHETU 
-- Create date: 6/14/2016
-- Description:	Get security token for solution by text api
-- =============================================
CREATE PROCEDURE [dbo].[SBTGetSecurityToken]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SecurityToken nvarchar(max), @tokenExpire int, @TokenDateTime datetime;

    Select @SecurityToken = SecurityToken, @tokenExpire = TokenExpireDuration, @TokenDateTime = TokenDateTime 
	FROM tblSBTSolutionByTextInfo WHERE IsProduction = 0

	IF(@SecurityToken IS NOT NULL AND @tokenExpire IS NOT NULL AND @TokenDateTime IS NOT NULL )
	BEGIN
		IF(DATEDIFF(MINUTE, @TokenDateTime,  GETDATE()) <= @tokenExpire)
		BEGIN
			SELECT @SecurityToken AS SecurityToken;
		END
		ELSE
		BEGIN
			SELECT '' as SecurityToken;
		END	
	END
	ELSE
		BEGIN
			SELECT '' as SecurityToken;
		END

END


GO
/****** Object:  StoredProcedure [dbo].[SBTGetSubscriberByAccount]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CHETU	
-- Create date: 6/14/2016
-- Description:	Get Active Subscriber Detail by Account Number
-- [dbo].[GetSubscriberByAccount] '311676'
-- =============================================
CREATE PROCEDURE [dbo].[SBTGetSubscriberByAccount]
	@AccountNumber nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT  SubscriberID, TextNumber, IsActive, DoNotText, CreatedBy, IsPaymentReminderNotification,
		IsPaymentConfirmationNotification, IsPayByTextNotification, IsChatByTextNotification as IsCommunicationByTextNotification
		, IsApproved
	FROM tblSBTSubscriber 
	WHERE AccountNumber = @AccountNumber 
	AND IsActive = 1
	ORDER BY CreatedDate DESC
END


GO
/****** Object:  StoredProcedure [dbo].[SBTInsertSendTextMessage]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CHETU
-- Create date: 10/28/2016
-- Description:	Add Send Text Message entry
-- =============================================
CREATE PROCEDURE [dbo].[SBTInsertSendTextMessage]
	@AccountNumber varchar(50),
	@TextNumber varchar(15),
	@Textmessage varchar(MAX)

AS
BEGIN

	SET NOCOUNT ON;


	INSERT INTO [dbo].[tblSBTTextMsgSendlog] (AccountID,AccountNo,TxtNumber,TxtMessage,DateSend,ActionLogType)
	VALUES(
	(SELECT TOP 1 AccountId FROM Reserves.[dbo].[tblAccounts] WHERE AcctNo=@AccountNumber),
	@AccountNumber,
	@TextNumber,
	@Textmessage,
	GETDATE(),
	'CUS'
	)
	RETURN 1;
END

GO
/****** Object:  StoredProcedure [dbo].[SBTInsertTextLog]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CHETU
-- Create date: 6/14/2016
-- Description:	Add Text Log entry
-- =============================================
CREATE PROCEDURE [dbo].[SBTInsertTextLog]
	@AccountNumber varchar(50),
	@TextNumber varchar(15),
	@TextLogType varchar(20),
	@TextLogEntry nvarchar(255),
	@TextLogUserName nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO tblSBTTextActionLog
	VALUES(
	(SELECT TOP 1 AccountID FROM Reserves.dbo.tblAccounts WHERE AcctNo = @AccountNumber),
	@TextNumber,
	'SBT', 'CUS: '+@TextLogEntry,
	GETDATE(),
	@TextLogUserName,@TextLogType
	)
	RETURN 1;
END


GO
/****** Object:  StoredProcedure [dbo].[SBTOptInSubscriber]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu
-- Create date: 6/14/2016
-- Description:	To Opt In subscriber 
-- =============================================
CREATE PROCEDURE [dbo].[SBTOptInSubscriber]
	@AccountNumber nvarchar(50),
	@TextNumber varchar(15),
	@OptInIPAddress varchar(15),
	@CreatedBy nvarchar(255),
	@IsPaymentReminderNotification bit = 0,
	@IsPaymentConfirmationNotification bit = 0,
	@IsPayByTextNotification bit = 0,
	@IsCommunicationByTextNotification bit
AS
BEGIN
	SET NOCOUNT ON;

	Declare @ConsumerID int,  @accountID int;

	SELECT @ConsumerID = tblAcct.ApplicantID FROM Reserves.dbo.tblAccounts tblAcct WHERE tblAcct.AcctNo = @AccountNumber;
	SELECT TOP 1 @accountID = AccountID FROM Reserves.dbo.tblAccounts WHERE AcctNo = @AccountNumber

	--IF NOT EXISTS (SELECT * FROM SBTSubscriber WHERE AccountNumber = @AccountNumber AND ModifiedDate IS NULL)
	--BEGIN
		INSERT INTO tblSBTSubscriber(AccountNumber, ConsumerID, TextNumber, OptInIPAddress, 
		CreatedDate, CreatedBy, IsPaymentReminderNotification,
		IsPaymentConfirmationNotification, IsPayByTextNotification, IsChatByTextNotification, IsApproved
		)
		VALUES(@AccountNumber, @ConsumerID, @TextNumber, @OptInIPAddress, GETDATE(), @CreatedBy,
		@IsPaymentReminderNotification,
		@IsPaymentConfirmationNotification,
		@IsPayByTextNotification,
		@IsCommunicationByTextNotification, 1)
	--END

	--Insert into Collection db tblInfoLog
	INSERT INTO Collection.dbo.tblInfoLog 
	(InfoLogAccountID, InfoLogType, InfoLogSubtype, InfoLogDate, InfoLogUsername, InfoLogEntry)
	VALUES
	(@accountID, 'TXT', 'SBT', GETDATE(),
	@CreatedBy, @TextNumber + ' ADDED '+ FORMAT(GETDATE(), 'g') + @CreatedBy
	)
	RETURN 1;
END


GO
/****** Object:  StoredProcedure [dbo].[SBTOptOutSubscriber]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu
-- Create date: 6/14/2016
-- Description:	Opt Out subscriber
-- =============================================
CREATE PROCEDURE [dbo].[SBTOptOutSubscriber]
	@SubscriberId int,
	@OptOutIPAddress varchar(15),
	@ModifiedBy nvarchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @accountID int;

	SELECT TOP 1 @accountID = a.AccountID 
	FROM Reserves.dbo.tblAccounts a INNER JOIN tblSBTSubscriber s
	ON a.AcctNo = s.AccountNumber
	WHERE s.SubscriberID = @SubscriberId

    UPDATE tblSBTSubscriber
	SET IsActive = 0,
	OptOutIPAddress = @OptOutIPAddress,
	ModifiedDate = GETDATE(),
	ModifiedBy = @ModifiedBy,
	IsPaymentReminderNotification = 0,
	IsPaymentConfirmationNotification = 0,
	IsPayByTextNotification = 0,
	IsChatByTextNotification = 0
	WHERE SubscriberID = @SubscriberId

	--Delete from Collection db tblInfoLog
	DELETE 
	FROM Collection.dbo.tblInfoLog 
	WHERE InfoLogType='TXT'
	AND InfoLogSubtype = 'SBT'
	AND InfoLogAccountID = @accountID

	RETURN 1;
END


GO
/****** Object:  StoredProcedure [dbo].[SBTSaveCallBackUrlInfo]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu 
-- Create date: 6/14/2016
-- Description:	To save  messages received at CallBack url from SBT
-- =============================================
CREATE PROCEDURE [dbo].[SBTSaveCallBackUrlInfo] 
	@Title varchar(250),
	@Code varchar(250),
	@ShortCode varchar(250),
	@Message varchar(250),
	@Phone  varchar(250),
	@Carrier  varchar(250),
	@Keyword  varchar(250),
	@Group  varchar(250),
	@Note  varchar(250),
	@UniqueId  varchar(250),
	@Default_Keyword  varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO tblSBTCallBackUrlInfo 
	VALUES (@Title, @Code, @ShortCode, @Message, @Phone, @Carrier, @Keyword, @Group, @Note, @UniqueId, @Default_Keyword, GETDATE()); 
END


GO
/****** Object:  StoredProcedure [dbo].[SBTSaveSecurityToken]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		CHETU 
-- Create date: 6/14/2016
-- Description:	Save security token for solution by text api
-- =============================================
CREATE PROCEDURE [dbo].[SBTSaveSecurityToken]
	@SecurityToken nvarchar(max),
	@TokenExpireInMinutes int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM tblSBTSolutionByTextInfo WHERE IsProduction = 0)
	BEGIN
		UPDATE tblSBTSolutionByTextInfo 
		SET SecurityToken = @SecurityToken,
		TokenDateTime = GETDATE(),
		TokenExpireDuration = @TokenExpireInMinutes
		 WHERE IsProduction = 0
	END
	ELSE
	BEGIN
		INSERT INTO tblSBTSolutionByTextInfo 
		VALUES(@SecurityToken, GETDATE(), @TokenExpireInMinutes, 0)
	END
	RETURN 1;
END


GO
/****** Object:  StoredProcedure [dbo].[SBTSaveStatusUrlInfo]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu 
-- Create date: 6/14/2016
-- Description:	To save status of messages received at status url from SBT
-- =============================================
CREATE PROCEDURE [dbo].[SBTSaveStatusUrlInfo] 
	@Code varchar(255),
	@Message varchar(255),
	@SendTo  varchar(255),
	@OrgCode  varchar(255),
	@Status  varchar(255),
	@Ticket  varchar(255),
	@Note  varchar(255),
	@UniqueId  varchar(255),
	@MessageCategory  varchar(255),
	@MessageSubCategory  varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO tblSBTStatusUrlInfo 
	VALUES (@Code, @Message, @SendTo, @OrgCode, @Status, @Ticket, @Note, @UniqueId, @MessageCategory, @MessageSubCategory, GETDATE()); 
END


GO
/****** Object:  StoredProcedure [dbo].[SBTUpdateAdditionalNotification]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu Team
-- Create date: 8/09/2016
-- Description:	Stored procedure to update additional notifications based on account number
-- =============================================
CREATE PROCEDURE [dbo].[SBTUpdateAdditionalNotification] 
	@SubscriberId int,
	@IsPaymentReminderNotification bit,
	@IsPaymentConfirmationNotification bit,
	@IsPayByTextNotification bit,
	@IsCommunicationByTextNotification bit
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM tblSBTSubscriber WHERE SubscriberID = @SubscriberId AND IsApproved = 0)
	BEGIN
		Declare @accountID int, @TextNumber varchar(12), @CreatedBy varchar(20);

		SELECT TOP 1 @accountID = a.AccountID , @TextNumber = s.TextNumber, @CreatedBy = s.CreatedBy
		FROM Reserves.dbo.tblAccounts a INNER JOIN tblSBTSubscriber s
		ON a.AcctNo = s.AccountNumber
		WHERE s.SubscriberID = @SubscriberId

		UPDATE tblSBTSubscriber
		SET 
		IsPaymentReminderNotification = @IsPaymentReminderNotification,
		IsPaymentConfirmationNotification = @IsPaymentConfirmationNotification,
		IsPayByTextNotification = @IsPayByTextNotification,
		IsChatByTextNotification = @IsCommunicationByTextNotification,
		IsApproved = 1
		WHERE SubscriberID = @SubscriberId

		--Insert Action Log
		INSERT INTO tblSBTTextActionLog
		VALUES(
		@accountID,
		@TextNumber,
		'SBT', 'CUS: Approved from WEB.',
		GETDATE(),
		@CreatedBy,'Approved'
		)
	END
	ELSE
	BEGIN
		UPDATE tblSBTSubscriber
		SET 
		IsPaymentReminderNotification = @IsPaymentReminderNotification,
		IsPaymentConfirmationNotification = @IsPaymentConfirmationNotification,
		IsPayByTextNotification = @IsPayByTextNotification,
		IsChatByTextNotification = @IsCommunicationByTextNotification
		WHERE SubscriberID = @SubscriberId
	END
    
	RETURN 1;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetActivePayByTextSetup]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Chetu
-- Create date: 10/18/2016
-- Description:	Procedure to get active pay by text setup
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetActivePayByTextSetup]
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT acct.AcctNo, acct.AcctNextDueDate as DueDate, acct.AcctPastDueAmt as DueAmount,
	pbt.TextNumber, pbt.BankABA, pbt.BankAcctNo, BankAcctType, BankHolder, BankName, SubscriptionId
	FROM Reserves.dbo.tblaccounts acct
	INNER JOIN Reserves.dbo.tblPayByTextSetup pbt
	ON acct.AcctNo = pbt.AcctNo AND pbt.IsActive = 1 AND pbt.IsDeleted = 0
	INNER JOIN tblSBTSubscriber sbts
	ON pbt.AcctNo = sbts.AccountNumber AND pbt.TextNumber = sbts.TextNumber
	AND sbts.IsPayByTextNotification = 1 AND sbts.IsActive = 1 AND sbts.IsApproved = 1 
	WHERE DATEDIFF(DAY, GETDATE(), acct.AcctNextDueDate) BETWEEN 0 AND 4

END

GO
/****** Object:  Table [dbo].[tblSBTCallBackUrlInfo]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTCallBackUrlInfo](
	[Title] [varchar](250) NULL,
	[Code] [varchar](250) NULL,
	[ShortCode] [varchar](250) NULL,
	[Message] [varchar](250) NULL,
	[Phone] [varchar](250) NULL,
	[Carrier] [varchar](250) NULL,
	[Keyword] [varchar](250) NULL,
	[Group] [varchar](250) NULL,
	[Note] [varchar](250) NULL,
	[UniqueId] [varchar](250) NULL,
	[Default_Keyword] [varchar](250) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntryDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CallBackUrl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTLogin]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTLogin](
	[OrgCode] [varchar](20) NOT NULL,
	[APIKey] [nvarchar](max) NOT NULL,
	[QrgLevel] [int] NULL,
 CONSTRAINT [PK_tbl_SBTLogin] PRIMARY KEY CLUSTERED 
(
	[OrgCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTSolutionByTextInfo]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSBTSolutionByTextInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityToken] [nvarchar](max) NOT NULL,
	[TokenDateTime] [datetime] NOT NULL,
	[TokenExpireDuration] [int] NOT NULL,
	[IsProduction] [bit] NOT NULL,
 CONSTRAINT [PK_SolutionByTextInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblSBTStatusUrlInfo]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTStatusUrlInfo](
	[Code] [varchar](255) NULL,
	[Message] [varchar](255) NULL,
	[SendTo] [varchar](255) NULL,
	[OrgCode] [varchar](255) NULL,
	[Status] [varchar](255) NULL,
	[Ticket] [varchar](255) NULL,
	[Note] [varchar](255) NULL,
	[UniqueId] [varchar](255) NULL,
	[MessageCategory] [varchar](255) NULL,
	[MessageSubCategory] [varchar](255) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntryDate] [datetime] NOT NULL,
 CONSTRAINT [PK_StatusUrl] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTSubscriber]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTSubscriber](
	[SubscriberID] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [nvarchar](50) NOT NULL,
	[ConsumerID] [int] NOT NULL,
	[TextNumber] [varchar](15) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[OptInIPAddress] [varchar](15) NOT NULL,
	[OptOutIPAddress] [varchar](15) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[DoNotText] [bit] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedBy] [nvarchar](255) NULL,
	[IsPaymentReminderNotification] [bit] NOT NULL,
	[IsPaymentConfirmationNotification] [bit] NOT NULL,
	[IsPayByTextNotification] [bit] NOT NULL,
	[IsChatByTextNotification] [bit] NOT NULL,
	[IsApproved] [bit] NULL,
 CONSTRAINT [PK_Subscriber] PRIMARY KEY CLUSTERED 
(
	[SubscriberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTTemplate]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSBTTemplate](
	[TempNo] [float] NULL,
	[TempID] [nvarchar](max) NULL,
	[TempSubject] [nvarchar](max) NULL,
	[TempSource] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[TempTitle] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblSBTTextActionLog]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTTextActionLog](
	[ActionLogID] [int] IDENTITY(1,1) NOT NULL,
	[ActionLogAccountID] [int] NOT NULL,
	[ActionLogTxtNumber] [varchar](12) NOT NULL,
	[ActionLogType] [nvarchar](20) NULL,
	[ActionLogEntry] [nvarchar](255) NOT NULL,
	[ActionLogDate] [datetime] NOT NULL,
	[ActionLogUsername] [nvarchar](100) NOT NULL,
	[ActionLogSubType] [nvarchar](20) NULL,
 CONSTRAINT [PK_TextLog] PRIMARY KEY CLUSTERED 
(
	[ActionLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTTextMsgSendlog]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSBTTextMsgSendlog](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[TempID] [nvarchar](50) NULL,
	[AccountID] [varchar](10) NOT NULL,
	[AccountNo] [varchar](10) NOT NULL,
	[TxtNumber] [varchar](20) NOT NULL,
	[TxtMessage] [nvarchar](250) NOT NULL,
	[SourceType] [varchar](30) NULL,
	[DateSend] [datetime] NOT NULL,
	[CompName] [varchar](20) NULL,
	[UserName] [varchar](20) NULL,
	[ActionLogType] [varchar](4) NULL,
 CONSTRAINT [PK__tblSBTTe__3214EC2769B4EBB7] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSBTURLs]    Script Date: 1/9/2017 9:39:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSBTURLs](
	[StatusURL] [nvarchar](max) NOT NULL,
	[CallbackURL] [nvarchar](max) NOT NULL,
	[AutoPayTermsURL] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[tblSBTCallBackUrlInfo] ADD  CONSTRAINT [DF_SBTCallBackUrlInfo_EntryDate]  DEFAULT (getdate()) FOR [EntryDate]
GO
ALTER TABLE [dbo].[tblSBTStatusUrlInfo] ADD  CONSTRAINT [DF_SBTStatusUrlInfo_EntryDate]  DEFAULT (getdate()) FOR [EntryDate]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_SBTSubscriber_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_SBTSubscriber_DoNotText]  DEFAULT ((0)) FOR [DoNotText]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_SBTSubscriber_IsPaymentReminder]  DEFAULT ((0)) FOR [IsPaymentReminderNotification]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_SBTSubscriber_IsPaymentConfirmation]  DEFAULT ((0)) FOR [IsPaymentConfirmationNotification]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_SBTSubscriber_IsPayByTextNotification]  DEFAULT ((0)) FOR [IsPayByTextNotification]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_tblSBTSubscriber_IsCommunicationByTextNotification]  DEFAULT ((0)) FOR [IsChatByTextNotification]
GO
ALTER TABLE [dbo].[tblSBTSubscriber] ADD  CONSTRAINT [DF_tblSBTSubscriber_IsApproved]  DEFAULT ((0)) FOR [IsApproved]
GO
USE [master]
GO
ALTER DATABASE [MidAtlanticFinanace-SolutionByText] SET  READ_WRITE 
GO
