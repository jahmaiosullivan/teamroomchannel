USE [YoorCity]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[LastActivityDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[IsConfirmed] [bit] NOT NULL,
	[Name] [nvarchar](128) NULL,
	[Avatar] [nvarchar](512) NULL,
	[City] [bigint] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Cards]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cards](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[City] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Location] [varchar](4096) NULL,
	[UpVotes] [int] NULL,
	[DownVotes] [int] NULL,
	[ImageUrl] [nvarchar](1024) NULL,
	[ThumbnailUrl] [nvarchar](1024) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Type] [int] NULL,
	[CreatedBy] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Cards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CardTags]    Script Date: 6/24/2014 9:29:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardTags](
	[CardId] [bigint] NOT NULL,
	[TagId] [bigint] NOT NULL,
 CONSTRAINT [PK_CardTags_1] PRIMARY KEY CLUSTERED 
(
	[CardId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Cities]    Script Date: 6/24/2014 9:29:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](512) NULL,
	[Region] [nvarchar](512) NULL,
	[Country] [nvarchar](512) NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Tags]    Script Date: 6/24/2014 9:29:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IconUrl] [nvarchar](256) NULL,
	[Related] [varchar](2048) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_CardTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserTags]    Script Date: 6/24/2014 9:29:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTags](
	[UserId] [nvarchar](128) NOT NULL,
	[TagId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserTags] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Votes]    Script Date: 6/24/2014 9:29:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CardId] [bigint] NOT NULL,
	[Value] [bit] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'201406171823376_InitialCreate', N'HobbyClue.Web.Models.ApplicationDbContext', 0x1F8B0800000000000400DD5CDB6EE336107D2FD07F10F4D416A9954B5B6C174E8BD4D974836E92C53A6D1F17B4443B44254A95A86CD25FEB433FA9BFD0A1445D488ABAD8F225C5020B5B1ACE0C87879C21799C7FFFFE67FAE353E05B8F384E4848CFED93C9B16D61EA861EA1AB733B65CBAF5FD93FFEF0F967D3375EF064FD56C89D713968499373FB81B1E8B5E324EE030E503209881B8749B86413370C1CE485CEE9F1F1F7CEC9898341850DBA2C6BFA21A58C0438FB025F67217571C452E4DF841EF613F11CDECC33ADD62D0A701221179FDB6FC3C5E279E6A778F23B5E4C7279DBBAF009025FE6D85FDA16A234648881A7AF7F4DF09CC5215DCD237880FCFBE70883DC12F909163D785D89F7EDCCF129EF8C53352C54B969C2C260A0C29333111D476DBE568CED327A10BF371067F6CC7B9DC5F0DCBEF670F6E843E843005483AF677ECC85CFED9BD2C44512DD6236291A4E72955731A8FB14C67F4CEA1A8FACDEED8E4A3401E8F8BF236B96FA2C8DF139C5298B917F64BD4F173E717FC1CFF7E11F989E9F9D2C9667AFBEFD0E7967DF7D83CFBEADF714FA0A72D20378F43E0E231C836F7859F6DFB61CB99DA3362C9BD5DAE451012CC1C4B0AD1BF4F40ED3157B802973FACAB6AEC813F68A27025CBF5202F3081AB13885AFB7A9EFA3858FCBF74EAB4DFE7F8B55F8388AD55BF44856D9D02BF661E2C430AF3E603F7B9B3C90289F5ED2787F1462577118F0EF32BEF2B71FE7611ABBBC33A151E41EC52BCC64EFA64E05DE5E90E6AAC68775A1F5F0A1CD3DD5E1DD28CA3BB4CE4C284CEC7A3614FE6ED76E6FC45D44110C5E062D1E9136C035A5AB89D2FEC892A4D6800F856EFD9F57C3B76180EFC34F748415B1DDD0255E2288F50CC6BBB0754DD9D9E9608FDFA1845DB88C3C82A64BC4CAB59C7FBE274167FB598C41D25BA72987D48ED28762F94D8088BFF5317A8F92049659EF2D4A1EB66E6C8EDD3486319C3314445BB7769D4029BC247180CB49F653086B1FA21BA4F2998F48D09CCB9585E863215AE5F366092DA71BC49AF27A9BABEFC215A1FD5C2D44CDAEE6129DAE0AB1A1AE7265FD3C1592664733814E3F73A9D12AA56C84C62F9532B5875F2B6D96F84C29A116C6390B63FC33A638E6CBF87BC4188E6935027DD6F07D24DA6CF8B8D1AD2F7599A5DF909F8E6D6AADD9902D02E3CF864CEDE1CF86013B87AC47F0F89178BCF0EDD1A210064F5ECE8643E9E6AECD4B31DBCFA6E72249429764B3A0E1E8486CFCE54EBCA19ED57D0A90F7463D49808E01D0094F79F004FA66ABC8BAA397D8C70C5BBCACE6476B3394B8C8D363091DF206385664D406C7AA1305D9B9AF349B00771CF346C8870A2E81994A28D3E706A12E8990DF1925A565CF14C6FB5EDA50DF5CE208536EB033127D8C371F2070074A3BCAA0744568EAD410D70E4443D56A1AF3AE12B61A776D5FBF134C76D4CE065C8AFA6D2BC06C8FD80EC0D91E923E0E1853DA3E002AF62A7D01A06E5C0E0DA0CA8EC900505152ED04A072C4F6005039242F0EA0F916B5EFF82BFBD54383A7BC51DE7D5A6F0DD71EB029C5E3C0A099D79ED086410B1CEBF0BC5CF097F889356CCEC04FB13F4B44A9AB42842B9F63261FD954F56E631DEAB42B5141D4A6B0025A8752718DA629D226D400E78AB3BC56EF441531406D71EED6AA56ACFD8ADA1A0674DDF5EBC49AA0F9D2510567AFDD47D9B3120D1AC87B6D166A7A1A00A12E5E72C77B04C5742EAB07A64F2D3CA41AAE754C0C464B803A2A5743908ACE8C1EA5029ADD516A2AC88694641B4549299F0C512A3A337A940446BB83D450140C280B360A919CC2479A6CC54947996DCA775327E719890753C740489ADEA02822745523288927D63C6727CDBE9E0F27ED04B90EC74D1AB83BA5B7A52516C6688595B7601A3CBD2271C22E11430BC4CF79665EA08935E656C3F25F98ACA74F7D108B3C5048F3CFC5B56DC3E5B7946DF5724468B9823E06BCA6C90ED21B10D0DCDCE29431E4A3B8E1EC7E16FA6940CD2596B9757E9B5A6F9F3FD1354C1DC57FAD84D2E2A515BA72F07B0D8D3E2D461BA6B286597FA8CC2A4C012F2AD07AC84D55A9594B714855D7623AB8DADBD0998A9981C3A5568AC347AB53C376E65645EDA8EBA89EF6D7247137EACAA417FDF5E9148EBA52FD6D7FCD12B9A3AE547AD15F5FC5F850278C699D3269120C8EBA1AF1A8BF0E999F515725BFE9AF512161D4552AAFFAEB94A816127CEB2F0E66996828A0475DE2F38DE0666BBC41C776D68D715244EDBE5B9A87D5E381BAC48DB6A64C3C3F483C1977431BE0293F01D80C4F061DDB458472F32B2DF9ED77DF669DD275AEB4209AEFC6B78E0D6D3BA48A94D6CB6D91B2FD998AAD48F78F36B4BD492E625B450060217F4E180E265C6032FFD39FF904FA5B09DC204A96386139E9C13E3D3E399D5CFC95C658F9EDC7E1FC0EC34912CF6FD8D0997E8C218FDC0E584CF411C5EE038A754EC106BF5528947E11A0A72FC7E25BB76F240E941D3F5A7865F2FB9A6AD7E6B6BF5C58AAA4712334D7248613BE3E8D440BF7E033CBB8DDC3DC6AA089AFAB4AA58D0F99C9BA36890ABE51E49BE8DE1B296CA4746FA4B181B6BD205DE0D880B5BBF3399941BD93D23A10225B5A2E350AEB4643ABD35407A8DB808ABADD0CB75716E76823DD48D21C4D7B030773E4DC3B1AC5F2505895D57DF77EC994BBE44FB69C8FFFAF68930740F469202EEC9F1CB96BAC990EEA0E9C61368C02796060137496FD131D770D36D329DE81836D109DF1C0B0B6AFFCB967A4F54EA17B2727EA3C0BC3817BD3215F17F9303F17854DFC220410E41565FE9BB166B64B1B53AFC3602562366AA6D9A886B589A3D9D524DACD0EEBAB48F8AD9D1532ED660DE4B436DB62FD6FB52D64DA6D1B285FFBA04D3692AE9AA8AC1DEB581B1BE425D124A59E74B072BB6AD6D6DBD397C48A1C2528D2EC315C01BE1C12E428211973EA0C203DEAB779903B6B7FA80DF2774256950AFE67DB2876A5AC59CA5CD36558246FC5A34244396FB9C10C7990522F624696C865F0DAC54992FDE8353B80E387CA0BEC5DD3BB944529832EE360E14BC751BC0868B39F313B659FA77751F6F71BC6E802B849F8F1FB1DFD2925BE57FA7DD570266450C1AB0B71C6CAC792F1B3D6D573A9E936A43D1589F09545D13D0E221F942577748E1EB1D9B7EE18CA119B5E12B48A5190081D557BF80AF0F382A71FFE03CE8605DE6A500000, N'6.1.0-alpha1-21218')
INSERT [dbo].[AspNetUsers] ([Id], [LastActivityDate], [CreatedDate], [UserName], [Email], [PasswordHash], [SecurityStamp], [IsConfirmed], [Name], [Avatar], [City]) VALUES (N'dad2a214-18d9-4e88-91ad-7f8683c32f89', CAST(0xFFFF2E4600000000 AS DateTime), CAST(0x0000A34E007666D4 AS DateTime), N'DavidSullivan', N'jahmai.osullivan@gmail.com', N'AJdwfjJveH11B28GetZRl1jlRsT0/jRa7UMBlxlSbJUc31PdPEHqRZSBF3DMPvcxYA==', N'0cc4b420-d7d2-4818-9095-9e16b9ab12e8', 1, N'David Sullivan', N'https://graph.facebook.com/10152492647437485/picture', 6)
SET IDENTITY_INSERT [dbo].[Cards] ON 

INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (2, 6, N'Miss this', N'{"GoogleId":"54dcb2a66469faa03f6a84f6f9aad62aaf815516","Address":"Main St","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Walmart Neighborhood Market","PhoneNumber":"(425) 643-9054","State":"WA","ZipCode":98007}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/2.jpg', NULL, 1, CAST(0x0000A34700226E7D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (3, 6, N'Green dinosaur', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/1.jpg', NULL, 1, CAST(0x0000A3480014B2DD AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (4, 6, N'Nightime dog', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/3.jpg', NULL, 1, CAST(0xFFFF2E4600000000 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (5, 6, N'Girl in window', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/4.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (6, 6, N'Man with mustache', N'{"GoogleId":"190839e63ae6818bc21d1559e5a4e26b2dc7e88f","Address":"S McClellan St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Beacon Hill Station","PhoneNumber":"(888) 889-6368","State":"WA","ZipCode":98144}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/5.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (7, 6, N'Reading a book with a crook behind her', N'{"GoogleId":"827356f5a1003f7f03c5ae81387a843299693c5a","Address":"4th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"The Capital Grille","PhoneNumber":"(206) 382-0900","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/6.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (8, 6, N'Bear at boy', N'{"GoogleId":"45d4c067238c53580e5dd9b620692f7052c79caa","Address":"202nd Ave NE","City":"Woodinville","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bear Creek Country Club","PhoneNumber":"(425) 883-4770","State":"WA","ZipCode":98077}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/7.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (9, 6, N'Girl on a bench', N'{"GoogleId":"0fb32ddf2960292a9d2c43ef143dda09a4ba2dce","Address":"Bell St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bench Market Medical","PhoneNumber":"(206) 701-7709","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/8.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (10, 6, N'Shocked in awe', N'', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/9.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (11, 6, N'A baker', N'{"GoogleId":"b81c0e2732eecf8cadeaf0f4ce23d477d1935e1a","Address":"1st Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Macrina Bakery & Cafe","PhoneNumber":"(206) 448-4032","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/10.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (12, 6, N'Walking her pet dinosaur', N'{"GoogleId":"2e718bad175cb3736e703babaf0fe98d12675b1b","Address":"S Jackson St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington Middle School","PhoneNumber":"(206) 252-2600","State":"WA","ZipCode":98144}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/11.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (13, 6, N'Hanging on a farm', N'{"GoogleId":"d4a034bc116993bf26b3f41e88365d1ba692a63e","Address":"S King St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Acme Farms Inc","PhoneNumber":"(206) 323-4300","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/12.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (14, 6, N'Fireball', N'{"GoogleId":"ded3cce4a4faa2d14a7a58f6b40fa6672bf137a1","Address":"166th Ave NE","City":"Redmond","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Redmond Town Center","PhoneNumber":"(425) 869-2640","State":"WA","ZipCode":98052}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-1e3a95256e27b3411d3c79d30b7a9493.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (15, 6, N'Staring into the city', N'{"GoogleId":"508dd243411eb0a6677294b043a58a5c9a9efcbf","Address":"108th Ave NE","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"City Center Bellevue","PhoneNumber":"(425) 732-5300","State":"WA","ZipCode":98004}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-2c2d2905e4a678f31c46b3e623cfa55c.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (16, 6, N'Temple', N'{"GoogleId":"60657723f9e263360feeb540c822d930dc5d8dbb","Address":"S Jackson St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Temple Billiards","PhoneNumber":"(206) 682-3242","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-185f545ab60b703d36e133c2502f3b80.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (17, 6, N'Man fading', N'{"GoogleId":"1cac3d6da6d6b6ba720c2084eeb3abe09c1522c7","Address":"Blanchard St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Belltown Billiards & Lounge","PhoneNumber":"(206) 420-3146","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-00597ec2dd1043f4f4c16536e40aac05.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (18, 6, N'On the river', N'{"GoogleId":"16eaf1fc774a9f27f643511579871e6c47ae9888","Address":"E Pike St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Caffe Vita","PhoneNumber":"(206) 709-4440","State":"WA","ZipCode":98122}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-05186aa4feb5d5a224b00ccddff220a5.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (19, 6, N'Looking at the Moon', N'{"GoogleId":"5cba3860ddc7c5e6f2c7dc3676b9c671d7447926","Address":"N 59th St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Woodland Park Zoo","PhoneNumber":"(206) 548-2500","State":"WA","ZipCode":98103}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-74618aa9ae95eae7e1255dd98cb068cd.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (20, 6, N'Man in the sunset', N'{"GoogleId":"e8da2ccc09667a050fd4c7e94449605fb26e6e12","Address":"Alki Ave SW","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Alki Beach Park","PhoneNumber":"(206) 684-4075","State":"WA","ZipCode":98116}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-90057f2bab62aba6e8c8e1180de903dc.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (21, 6, N'Snowing in June', N'{"GoogleId":"164cc336351b63e24ea9f53106e93550c4eea4ba","Address":"Bellevue Way NE","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bellevue Way Cleaners","PhoneNumber":"(425) 454-8644","State":"WA","ZipCode":98004}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-a047c06cdc07a9ecdc1a2b6b1510b238.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (22, 6, N'Crescent Moon', N'{"GoogleId":"266a5973ff8457272e806880d6de495cf446de7e","Address":"5th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Top Pot Doughnuts","PhoneNumber":"(206) 728-1966","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-bf1f84d02a6b05cde4f141cb02a0455e.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (23, 6, N'Beautiful woman', N'{"GoogleId":"7caa65421ab4326b5a99a3adb3282ba8f998abac","Address":"East Marginal Way S","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Restaurant Depot","PhoneNumber":"(206) 381-1555","State":"WA","ZipCode":98134}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c835153e1a1f8647283e83486a8dcfd8.jpg', NULL, 1, CAST(0x0000A34B00CCB83D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (24, 6, N'Eiffel Tower at night', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c6449495d072a07a51e318961bad96bc.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (25, 6, N'Eiffel Tower at night', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c6449495d072a07a51e318961bad96bc.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (26, 6, N'Burj Khalifa', N'', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-cfd496c3bfab62634b80b5aba373bc0d.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (27, 6, N'Colosseum', N'{"GoogleId":"5ddc4dc1f73c354fcb055ab40358155b9cc378ea","Address":"Piazza del Colosseo","City":"Rome","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Colosseum","PhoneNumber":"06 3996 7700","State":"Lazio","ZipCode":184}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-dc23ff66548d3e5598ba43d0d72ff0e3.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (28, 6, N'Eiffel Tower', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f55d0db4ec0285113d24276562caccd1.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (29, 6, N'Needle Fireworks', N'{"GoogleId":"2d3d3592254d212ad01ce51620be345b480df9bb","Address":"Broad St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Space Needle","PhoneNumber":"(206) 905-2100","State":"WA","ZipCode":98109}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f85ea624e1ba01a6c820bd5fed51039b.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (30, 6, N'Glass building', N'{"GoogleId":"b88173ff8a0cb9943906c98a7382476d68a8e0c9","Address":"4th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Seattle Public Library-Central Library","PhoneNumber":"(206) 386-4636","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f93e89fca4f297d8a1b6cff14854c35e.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (31, 6, N'Bird in the moonlight', N'{"GoogleId":"2a0c9c2cb4c64cecb77ffffe0496cfec33731387","Address":"Kaster Dr NE","City":"Bremerton","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bird''s-Eye View Bed & Breakfast","PhoneNumber":"(360) 698-2448","State":"WA","ZipCode":98311}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-fb0e7089f1bab2ce35e23debc4420715.jpg', NULL, 1, CAST(0x0000A3470014F92D AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (32, 6, N'Miss this', N'{"GoogleId":"54dcb2a66469faa03f6a84f6f9aad62aaf815516","Address":"Main St","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Walmart Neighborhood Market","PhoneNumber":"(425) 643-9054","State":"WA","ZipCode":98007}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/2.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (33, 6, N'On the river', N'{"GoogleId":"16eaf1fc774a9f27f643511579871e6c47ae9888","Address":"E Pike St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Caffe Vita","PhoneNumber":"(206) 709-4440","State":"WA","ZipCode":98122}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-05186aa4feb5d5a224b00ccddff220a5.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (34, 6, N'Looking at the Moon', N'{"GoogleId":"5cba3860ddc7c5e6f2c7dc3676b9c671d7447926","Address":"N 59th St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Woodland Park Zoo","PhoneNumber":"(206) 548-2500","State":"WA","ZipCode":98103}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-74618aa9ae95eae7e1255dd98cb068cd.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (35, 6, N'Man in the sunset', N'{"GoogleId":"e8da2ccc09667a050fd4c7e94449605fb26e6e12","Address":"Alki Ave SW","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Alki Beach Park","PhoneNumber":"(206) 684-4075","State":"WA","ZipCode":98116}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-90057f2bab62aba6e8c8e1180de903dc.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (36, 6, N'Snowing in June', N'{"GoogleId":"164cc336351b63e24ea9f53106e93550c4eea4ba","Address":"Bellevue Way NE","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bellevue Way Cleaners","PhoneNumber":"(425) 454-8644","State":"WA","ZipCode":98004}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-a047c06cdc07a9ecdc1a2b6b1510b238.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (37, 6, N'Miss this', N'{"GoogleId":"54dcb2a66469faa03f6a84f6f9aad62aaf815516","Address":"Main St","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Walmart Neighborhood Market","PhoneNumber":"(425) 643-9054","State":"WA","ZipCode":98007}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/2.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (38, 6, N'Green dinosaur', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/1.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (39, 6, N'Nightime dog', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/3.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (40, 6, N'Girl in window', N'{"GoogleId":"b0bde6bfba59c6814184d01f0397d12d0aa036da","Address":"Convention Pl","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington State Convention Center","PhoneNumber":"(206) 694-5030","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/4.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (41, 6, N'Man with mustache', N'{"GoogleId":"190839e63ae6818bc21d1559e5a4e26b2dc7e88f","Address":"S McClellan St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Beacon Hill Station","PhoneNumber":"(888) 889-6368","State":"WA","ZipCode":98144}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/5.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (42, 6, N'Bear at boy', N'{"GoogleId":"45d4c067238c53580e5dd9b620692f7052c79caa","Address":"202nd Ave NE","City":"Woodinville","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bear Creek Country Club","PhoneNumber":"(425) 883-4770","State":"WA","ZipCode":98077}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/7.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (43, 6, N'Girl on a bench', N'{"GoogleId":"0fb32ddf2960292a9d2c43ef143dda09a4ba2dce","Address":"Bell St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bench Market Medical","PhoneNumber":"(206) 701-7709","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/8.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (44, 6, N'Shocked in awe', N'', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/9.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (45, 6, N'A baker', N'{"GoogleId":"b81c0e2732eecf8cadeaf0f4ce23d477d1935e1a","Address":"1st Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Macrina Bakery & Cafe","PhoneNumber":"(206) 448-4032","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/10.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (46, 6, N'Walking her pet dinosaur', N'{"GoogleId":"2e718bad175cb3736e703babaf0fe98d12675b1b","Address":"S Jackson St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Washington Middle School","PhoneNumber":"(206) 252-2600","State":"WA","ZipCode":98144}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/11.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (47, 6, N'Hanging on a farm', N'{"GoogleId":"d4a034bc116993bf26b3f41e88365d1ba692a63e","Address":"S King St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Acme Farms Inc","PhoneNumber":"(206) 323-4300","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/12.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (48, 6, N'Fireball', N'{"GoogleId":"ded3cce4a4faa2d14a7a58f6b40fa6672bf137a1","Address":"166th Ave NE","City":"Redmond","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Redmond Town Center","PhoneNumber":"(425) 869-2640","State":"WA","ZipCode":98052}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-1e3a95256e27b3411d3c79d30b7a9493.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (49, 6, N'Staring into the city', N'{"GoogleId":"508dd243411eb0a6677294b043a58a5c9a9efcbf","Address":"108th Ave NE","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"City Center Bellevue","PhoneNumber":"(425) 732-5300","State":"WA","ZipCode":98004}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-2c2d2905e4a678f31c46b3e623cfa55c.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (50, 6, N'Temple', N'{"GoogleId":"60657723f9e263360feeb540c822d930dc5d8dbb","Address":"S Jackson St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Temple Billiards","PhoneNumber":"(206) 682-3242","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-185f545ab60b703d36e133c2502f3b80.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (51, 6, N'Man fading', N'{"GoogleId":"1cac3d6da6d6b6ba720c2084eeb3abe09c1522c7","Address":"Blanchard St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Belltown Billiards & Lounge","PhoneNumber":"(206) 420-3146","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-00597ec2dd1043f4f4c16536e40aac05.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (52, 6, N'On the river', N'{"GoogleId":"16eaf1fc774a9f27f643511579871e6c47ae9888","Address":"E Pike St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Caffe Vita","PhoneNumber":"(206) 709-4440","State":"WA","ZipCode":98122}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-05186aa4feb5d5a224b00ccddff220a5.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (53, 6, N'Looking at the Moon', N'{"GoogleId":"5cba3860ddc7c5e6f2c7dc3676b9c671d7447926","Address":"N 59th St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Woodland Park Zoo","PhoneNumber":"(206) 548-2500","State":"WA","ZipCode":98103}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-74618aa9ae95eae7e1255dd98cb068cd.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (54, 6, N'Man in the sunset', N'{"GoogleId":"e8da2ccc09667a050fd4c7e94449605fb26e6e12","Address":"Alki Ave SW","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Alki Beach Park","PhoneNumber":"(206) 684-4075","State":"WA","ZipCode":98116}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-90057f2bab62aba6e8c8e1180de903dc.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (55, 6, N'Snowing in June', N'{"GoogleId":"164cc336351b63e24ea9f53106e93550c4eea4ba","Address":"Bellevue Way NE","City":"Bellevue","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bellevue Way Cleaners","PhoneNumber":"(425) 454-8644","State":"WA","ZipCode":98004}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-a047c06cdc07a9ecdc1a2b6b1510b238.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (56, 6, N'Crescent Moon', N'{"GoogleId":"266a5973ff8457272e806880d6de495cf446de7e","Address":"5th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Top Pot Doughnuts","PhoneNumber":"(206) 728-1966","State":"WA","ZipCode":98121}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-bf1f84d02a6b05cde4f141cb02a0455e.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (57, 6, N'Beautiful woman', N'{"GoogleId":"7caa65421ab4326b5a99a3adb3282ba8f998abac","Address":"East Marginal Way S","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Restaurant Depot","PhoneNumber":"(206) 381-1555","State":"WA","ZipCode":98134}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c835153e1a1f8647283e83486a8dcfd8.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (58, 6, N'Eiffel Tower at night', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c6449495d072a07a51e318961bad96bc.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (59, 6, N'Eiffel Tower at night', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-c6449495d072a07a51e318961bad96bc.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (60, 6, N'Burj Khalifa', N'', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-cfd496c3bfab62634b80b5aba373bc0d.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (61, 6, N'Colosseum', N'{"GoogleId":"5ddc4dc1f73c354fcb055ab40358155b9cc378ea","Address":"Piazza del Colosseo","City":"Rome","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Colosseum","PhoneNumber":"06 3996 7700","State":"Lazio","ZipCode":184}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-dc23ff66548d3e5598ba43d0d72ff0e3.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (62, 6, N'Eiffel Tower', N'{"GoogleId":"3b76268fde490278947d1607804395871e128dc0","Address":"Eiffel Tower Ln","City":"Paris","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Eiffel Tower Ln","PhoneNumber":null,"State":"Tennessee","ZipCode":38242}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f55d0db4ec0285113d24276562caccd1.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (63, 6, N'Needle Fireworks', N'{"GoogleId":"2d3d3592254d212ad01ce51620be345b480df9bb","Address":"Broad St","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Space Needle","PhoneNumber":"(206) 905-2100","State":"WA","ZipCode":98109}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f85ea624e1ba01a6c820bd5fed51039b.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (64, 6, N'Glass building', N'{"GoogleId":"b88173ff8a0cb9943906c98a7382476d68a8e0c9","Address":"4th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Seattle Public Library-Central Library","PhoneNumber":"(206) 386-4636","State":"WA","ZipCode":98104}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-f93e89fca4f297d8a1b6cff14854c35e.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (65, 6, N'Bird in the moonlight', N'{"GoogleId":"2a0c9c2cb4c64cecb77ffffe0496cfec33731387","Address":"Kaster Dr NE","City":"Bremerton","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"Bird''s-Eye View Bed & Breakfast","PhoneNumber":"(360) 698-2448","State":"WA","ZipCode":98311}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/main-qimg-fb0e7089f1bab2ce35e23debc4420715.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
INSERT [dbo].[Cards] ([Id], [City], [Description], [Location], [UpVotes], [DownVotes], [ImageUrl], [ThumbnailUrl], [IsActive], [CreatedDate], [Type], [CreatedBy]) VALUES (66, 6, N'Reading a book with a crook behind her', N'{"GoogleId":"827356f5a1003f7f03c5ae81387a843299693c5a","Address":"4th Ave","City":"Seattle","Country":null,"Description":"","GoogleGeometryJson":",","Id":0,"Name":"The Capital Grille","PhoneNumber":"(206) 382-0900","State":"WA","ZipCode":98101}', 0, 0, N'https://hobbyclue.blob.core.windows.net/categoryimages/6.jpg', NULL, 1, CAST(0x0000A348003B5AB9 AS DateTime), 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89')
SET IDENTITY_INSERT [dbo].[Cards] OFF
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (2, 1)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (3, 2)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (3, 3)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (4, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (4, 5)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (5, 6)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (5, 7)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (6, 8)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (6, 9)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (7, 10)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (7, 11)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (8, 12)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (8, 13)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (9, 6)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (9, 14)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (9, 15)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (10, 16)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (10, 17)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (11, 18)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (11, 19)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (12, 2)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (12, 20)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (13, 21)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (13, 22)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (13, 23)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (14, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (14, 24)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (15, 25)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (15, 26)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (16, 27)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (16, 28)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (17, 8)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (17, 29)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (17, 30)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (17, 31)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (18, 32)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (18, 33)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (18, 34)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (19, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (19, 58)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (20, 8)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (20, 34)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (21, 35)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (21, 36)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (22, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (22, 37)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (22, 58)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (23, 38)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (23, 39)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (23, 59)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (24, 26)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (24, 40)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (24, 41)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (25, 26)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (25, 40)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (25, 41)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (26, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (26, 43)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (26, 44)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (26, 45)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (26, 46)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (27, 47)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (27, 48)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (27, 49)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (27, 50)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (28, 40)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (28, 41)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (29, 51)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (29, 52)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (29, 53)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (30, 44)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (30, 54)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (30, 55)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (31, 4)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (31, 56)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (31, 57)
INSERT [dbo].[CardTags] ([CardId], [TagId]) VALUES (31, 58)
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (6, N'Seattle', N'Washington', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (7, N'auburn', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (8, N'auburn', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (9, N'birmingham', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (10, N'dothan', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (11, N'florence-muscle shoals', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (12, N'gadsden-anniston', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (13, N'huntsville', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (14, N'mobile', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (15, N'montgomery', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (16, N'tuscaloosa', N'Alabama', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (17, N'anchorage', N'Alaska', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (18, N'fairbanks', N'Alaska', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (19, N'kenai', N'Alaska', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (20, N'juneau', N'Alaska', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (21, N'flagstaff', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (22, N'mohave county', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (23, N'phoenix', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (24, N'prescott', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (25, N'show low', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (26, N'sierra vista', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (27, N'tucson', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (28, N'yuma', N'Arizona', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (29, N'fayetteville', N'Arkansas', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (30, N'fort smith', N'Arkansas', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (31, N'jonesboro', N'Arkansas', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (32, N'little rock', N'Arkansas', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (33, N'texarkana', N'Arkansas', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (34, N'bakersfield', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (35, N'fresno', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (36, N'gold country', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (37, N'hanford-corcoran', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (38, N'humboldt county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (39, N'imperial county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (40, N'inland empire', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (41, N'los angeles', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (42, N'mendocino county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (43, N'merced', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (44, N'modesto', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (45, N'monterey bay', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (46, N'orange county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (47, N'palm springs', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (48, N'redding', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (49, N'sacramento', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (50, N'san diego', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (51, N'san francisco', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (52, N'san luis obispo', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (53, N'santa barbara', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (54, N'santa maria', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (55, N'siskiyou county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (56, N'stockton', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (57, N'susanville', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (58, N'ventura county', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (59, N'visalia-tulare', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (60, N'yuba-sutter', N'California', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (61, N'boulder', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (62, N'colorado springs', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (63, N'denver', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (64, N'eastern CO', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (65, N'fort collins / north CO', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (66, N'high rockies', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (67, N'pueblo', N'Colorado', N'United States')
INSERT [dbo].[Cities] ([Id], [Name], [Region], [Country]) VALUES (68, N'western slope', N'Colorado', N'United States')
SET IDENTITY_INSERT [dbo].[Cities] OFF
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (1, N'Walmart', NULL, NULL, NULL, NULL, CAST(0x0000A34701510706 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (2, N'Dinosaur', NULL, NULL, NULL, NULL, CAST(0x0000A34701510708 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (3, N'Sprinkler', NULL, NULL, NULL, NULL, CAST(0x0000A34701510709 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (4, N'Night', NULL, NULL, NULL, NULL, CAST(0x0000A3470151070A AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (5, N'Dog', NULL, NULL, NULL, NULL, CAST(0x0000A3470151070C AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (6, N'Girl', NULL, NULL, NULL, NULL, CAST(0x0000A3470151070D AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (7, N'Window', NULL, NULL, NULL, NULL, CAST(0x0000A3470151070E AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (8, N'Man', NULL, NULL, NULL, NULL, CAST(0x0000A3470151070F AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (9, N'Mustache', NULL, NULL, NULL, NULL, CAST(0x0000A34701510710 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (10, N'Reading', NULL, NULL, NULL, NULL, CAST(0x0000A34701510711 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (11, N'Crook', NULL, NULL, NULL, NULL, CAST(0x0000A34701510712 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (12, N'Bear', NULL, NULL, NULL, NULL, CAST(0x0000A34701510713 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (13, N'Boy', NULL, NULL, NULL, NULL, CAST(0x0000A34701510714 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (14, N'Bench', NULL, NULL, NULL, NULL, CAST(0x0000A34701510715 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (15, N'Redhead', NULL, NULL, NULL, NULL, CAST(0x0000A34701510716 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (16, N'Awe', NULL, NULL, NULL, NULL, CAST(0x0000A34701510717 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (17, N'Shocked', NULL, NULL, NULL, NULL, CAST(0x0000A34701510718 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (18, N'Bake', NULL, NULL, NULL, NULL, CAST(0x0000A34701510719 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (19, N'Baker', NULL, NULL, NULL, NULL, CAST(0x0000A34701510719 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (20, N'Walking', NULL, NULL, NULL, NULL, CAST(0x0000A3470151071B AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (21, N'Pig', NULL, NULL, NULL, NULL, CAST(0x0000A3470151071C AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (22, N'Farm', NULL, NULL, NULL, NULL, CAST(0x0000A3470151071D AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (23, N'Hills', NULL, NULL, NULL, NULL, CAST(0x0000A3470151071E AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (24, N'Fire', NULL, NULL, NULL, NULL, CAST(0x0000A3470151071F AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (25, N'City', NULL, NULL, NULL, NULL, CAST(0x0000A34701510720 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (26, N'Lights', NULL, NULL, NULL, NULL, CAST(0x0000A34701510721 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (27, N'Temple', NULL, NULL, NULL, NULL, CAST(0x0000A34701510722 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (28, N'India', NULL, NULL, NULL, NULL, CAST(0x0000A34701510722 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (29, N'Fading', NULL, NULL, NULL, NULL, CAST(0x0000A34701510723 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (30, N'Sea', NULL, NULL, NULL, NULL, CAST(0x0000A34701510724 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (31, N'Ocean', NULL, NULL, NULL, NULL, CAST(0x0000A34701510725 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (32, N'River', NULL, NULL, NULL, NULL, CAST(0x0000A34701510726 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (33, N'Boat', NULL, NULL, NULL, NULL, CAST(0x0000A34701510727 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (34, N'Sunset', NULL, NULL, NULL, NULL, CAST(0x0000A34701510728 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (35, N'Snowing', NULL, NULL, NULL, NULL, CAST(0x0000A34701510728 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (36, N'Light', NULL, NULL, NULL, NULL, CAST(0x0000A34701510729 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (37, N'Crescent', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072A AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (38, N'Restaurant', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072B AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (39, N'Date', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072C AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (40, N'Eiffel', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072D AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (41, N'Tower', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072E AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (43, N'Burj Khalifa', NULL, NULL, NULL, NULL, CAST(0x0000A3470151072F AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (44, N'Building', NULL, NULL, NULL, NULL, CAST(0x0000A34701510730 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (45, N'Tallest', NULL, NULL, NULL, NULL, CAST(0x0000A34701510731 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (46, N'Meditating', NULL, NULL, NULL, NULL, CAST(0x0000A34701510732 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (47, N'Roman', NULL, NULL, NULL, NULL, CAST(0x0000A34701510733 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (48, N'Rome', NULL, NULL, NULL, NULL, CAST(0x0000A34701510734 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (49, N'Colosseum', NULL, NULL, NULL, NULL, CAST(0x0000A34701510734 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (50, N'Plaza', NULL, NULL, NULL, NULL, CAST(0x0000A34701510735 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (51, N'Space Needle', NULL, NULL, NULL, NULL, CAST(0x0000A34701510736 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (52, N'Fourth of July', NULL, NULL, NULL, NULL, CAST(0x0000A34701510737 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (53, N'Fireworks', NULL, NULL, NULL, NULL, CAST(0x0000A34701510738 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (54, N'Library', NULL, NULL, NULL, NULL, CAST(0x0000A34701510739 AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (55, N'Glass', NULL, NULL, NULL, NULL, CAST(0x0000A3470151073A AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (56, N'Bird', NULL, NULL, NULL, NULL, CAST(0x0000A3470151073B AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (57, N'Pelican', NULL, NULL, NULL, NULL, CAST(0x0000A3470151073C AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (58, N'Moon', NULL, NULL, NULL, NULL, CAST(0x0000A348000A28AF AS DateTime), NULL)
INSERT [dbo].[Tags] ([Id], [Name], [Description], [IconUrl], [Related], [CreatedBy], [CreatedDate], [IsActive]) VALUES (59, N'Dinner', NULL, NULL, NULL, NULL, CAST(0x0000A348000A2DB3 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Tags] OFF
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 1, CAST(0x0000A35001470888 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 4, CAST(0x0000A3530141A553 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 29, CAST(0x0000A351000260A6 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 33, CAST(0x0000A3500165D4F6 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 37, CAST(0x0000A3500165D3E9 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 41, CAST(0x0000A3500165D354 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 43, CAST(0x0000A350016559A6 AS DateTime))
INSERT [dbo].[UserTags] ([UserId], [TagId], [CreatedDate]) VALUES (N'DAD2A214-18D9-4E88-91AD-7F8683C32F89', 46, CAST(0x0000A3500165D24D AS DateTime))
SET IDENTITY_INSERT [dbo].[Votes] ON 

INSERT [dbo].[Votes] ([Id], [CardId], [Value], [CreatedBy], [CreatedDate]) VALUES (62, 66, 0, N'dad2a214-18d9-4e88-91ad-7f8683c32f89', CAST(0x0000A35301416329 AS DateTime))
SET IDENTITY_INSERT [dbo].[Votes] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [CK_Tags_UniqueName]    Script Date: 6/24/2014 9:29:20 AM ******/
ALTER TABLE [dbo].[Tags] ADD  CONSTRAINT [CK_Tags_UniqueName] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [Votes_Card_User]    Script Date: 6/24/2014 9:29:20 AM ******/
ALTER TABLE [dbo].[Votes] ADD  CONSTRAINT [Votes_Card_User] UNIQUE NONCLUSTERED 
(
	[CardId] ASC,
	[CreatedBy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[Cards] ADD  CONSTRAINT [DF_Cards_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Cards] ADD  CONSTRAINT [CN_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Cards] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[Tags] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserTags] ADD  CONSTRAINT [DF_UserTags_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Votes] ADD  CONSTRAINT [DF_Votes_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH NOCHECK ADD  CONSTRAINT [FK_City_Users] FOREIGN KEY([City])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_City_Users]
GO
ALTER TABLE [dbo].[Cards]  WITH NOCHECK ADD  CONSTRAINT [FK_CreatedBy_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Cards] CHECK CONSTRAINT [FK_CreatedBy_User]
GO
ALTER TABLE [dbo].[CardTags]  WITH CHECK ADD  CONSTRAINT [FK_CardTags_Cards] FOREIGN KEY([CardId])
REFERENCES [dbo].[Cards] ([Id])
GO
ALTER TABLE [dbo].[CardTags] CHECK CONSTRAINT [FK_CardTags_Cards]
GO
ALTER TABLE [dbo].[CardTags]  WITH CHECK ADD  CONSTRAINT [FK_CardTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[CardTags] CHECK CONSTRAINT [FK_CardTags_Tags]
GO
ALTER TABLE [dbo].[UserTags]  WITH NOCHECK ADD  CONSTRAINT [FK_UserTags_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
GO
ALTER TABLE [dbo].[UserTags] CHECK CONSTRAINT [FK_UserTags_Tags]
GO
ALTER TABLE [dbo].[UserTags]  WITH CHECK ADD  CONSTRAINT [FK_UserTags_Users] FOREIGN KEY([UserId], [TagId])
REFERENCES [dbo].[UserTags] ([UserId], [TagId])
GO
ALTER TABLE [dbo].[UserTags] CHECK CONSTRAINT [FK_UserTags_Users]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Cards] FOREIGN KEY([Id])
REFERENCES [dbo].[Cards] ([Id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Cards]
GO
